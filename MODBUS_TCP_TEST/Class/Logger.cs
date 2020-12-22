using System;
using System.Data;
using System.Reflection;
using System.Windows.Forms;
using System.IO;

namespace MODBUS_TCP
{
    /// <summary>
    /// 로그 종류의 열거형
    /// </summary>
    public enum LogType
    {
        Transmitter,
        Receiver
    }

    /// <summary>
    /// Logger Class
    /// </summary>
    public class Logger
    {
        public string   LogDir      = "";               // 로그의 고정 위치를 저장
        private object  objLock_1   = new object();     // 로그 파일 저장 동작을 보호
        public DataRow drData;

        /// <summary>
        /// Logger의 생성자, Logger Initialize 합니다.
        /// </summary>
        public Logger()
        {
            try
            {
                LogDir = Program.mSolution.SolutionPath + "\\LOGGER"; // 현재 절대 위치를 불러오고 log폴더 위치를 추가합니다.

                if (!Directory.Exists(LogDir))
                    Directory.CreateDirectory(LogDir); // 만악 log폴더가 없을 경우 생성합니다.
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + " : " + MethodBase.GetCurrentMethod().Name + ", " + ex.Message, "Waring!");
            }
        }

        /// <summary>
        /// 로그 종류를 받아와 현재 시간과 조합하여 형식에 맞춘 문자열을 반환합니다.
        /// </summary>
        /// <param name="logType">LogType에 정의되어 있는 알맞은 값을 인자값으로 요청합니다.</param>
        private string getLogPath(LogType logType) 
        {
            string logPath = string.Format(@"{0}\{1:00}\{2:00}\log_{3:0000}{4:00}{5:00}_" + logType.ToString() + ".LOG", LogDir, DateTime.Now.Month, DateTime.Now.Day, DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            // 2018년 11월 08일 DEBUG 로그 파일    ex:) (고정위치)\11\08\log_20181108_DEBUG.LOG
            // 2018년 12월 18일 ERROR 로그 파일    ex:) (고정위치)\12\18\log_20181218_ERROR.LOG
            // 2019년 01월 08일 INFO 로그 파일  ex:) (고정위치)\01\08\log_20190108_INFO.LOG

            return logPath;
        }

        /// <summary>
        /// 현재 시간을 포멧에 맞추어 문자열로 반환합니다.
        /// </summary>
        private string makeFileNameWithTime()
        {
            string strTime = string.Format(@"{0:00}:{1:00}:{2:00}.{3:000}", DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second, DateTime.Now.Millisecond);
            return strTime;
        }

        /// <summary>
        /// 해당 주소의 하위 폴더와 하위 파일을 모두 제거합니다.
        /// </summary>
        /// <param name="dir">제거할 폴더(파일)의 상위 폴더를 지목합니다.</param>
        /// <param name="searchPattern">확장명을 의미합니다.</param>
        /// <param name="day">Log가 기록된 날을 인자값으로 요청합니다.</param>
        public static void DeleteFilesInDir(string dir, string searchPattern, int day)
        {
            DirectoryInfo path = new DirectoryInfo(dir);    // 인자값으로 들어온 절대 주소를 객체로 정의합니다.
            Dirs(path, searchPattern, day);                 // 삭제를 시작합니다.
        }

        /// <summary>
        /// 해당 주소의 하위 폴더를 검색하여 반복문을 실행합니다.
        /// </summary>
        /// <param name="dir">제거할 폴더(파일)의 상위 폴더를 지목합니다.</param>
        /// <param name="searchPattern">확장명을 의미합니다.</param>
        /// <param name="day">Log가 기록된 날을 인자값으로 요청합니다.</param>
        private static void Dirs(DirectoryInfo dir, string searchPattern, int day)
        {
            DirectoryInfo[] di = dir.GetDirectories(); // 받은 주소의 하위 폴더 주소들을 반환합니다.

            if (di.Length < 1) // 반환받은 주소가 없을 경우 빠져나갑니다.
            {
                return;
            }

            for (int i = 0; i < di.Length; i++) // 반환받은 주소의 수 만큼 반복문을 실행시킵니다.
            {
                Files(di[i], searchPattern, day); // n번째 주소로 FilesSearch함수를 실행시킵니다.
                Dirs(di[i], searchPattern, day); // n번째 주소로 DirSearch함수를 실행시킵니다.
            }
        }

        /// <summary>
        /// 해당 주소의 하위 파일를 검색하여 반복문을 실행하고 제거합니다.
        /// </summary>
        /// <param name="dir">제거할 파일의 상위 폴더를 지목합니다.</param>
        /// <param name="searchPattern">확장명을 의미합니다.</param>
        /// <param name="day">Log가 기록된 날을 인자값으로 요청합니다.</param>
        private static void Files(DirectoryInfo dir, string searchPattern, int day)
        {
            try
            {
                DateTime dayAgoTime = DateTime.Now.AddSeconds(-(day * 86400)); // 인자로 받은 날을 객체로 정의합니다.

                foreach (FileInfo fileName in dir.GetFiles()) // 해당 폴더에 파일 갯수 만큼 반복합니다.
                {
                    if (searchPattern.Equals(".*")) //확장명이 .*일 경우 모든 파일을 제거합니다.
                    {
                        DateTime dt = fileName.CreationTime; // 파일을 만들었던 시간을 객체로 정의합니다.
                        if (dayAgoTime > dt) // 사용자가 설정한 날보다 더 이전에 만들었을 경우
                        {
                            fileName.Delete(); // 파일을 제거합니다.
                        }
                    }
                    else if (fileName.Extension.Equals(searchPattern)) // 인자값의 확장명이 반복문의 확장명과 같을 경우 제거합니다.
                    {
                        DateTime dt = fileName.CreationTime; // 파일을 만들었던 시간을 객체로 정의합니다.
                        if (dayAgoTime > dt) // 사용자가 설정한 날보다 더 이전에 만들었을 경우
                        {
                            fileName.Delete(); // 파일을 제거합니다.
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Logger : " + MethodBase.GetCurrentMethod().Name + ", " + ex.Message, "Waring!");
            }
        }

        /// <summary>
        /// 로그를 기록합니다.
        /// </summary>
        /// <param name="logMassage">로그의 내용을 문자열로 입력합니다.</param>
        /// <param name="logType">어떤 동작을 하는 로그인지 구분합니다.</param>
        /// <param name="bUpdateUI">GUI로 사용자에게 보여줄지 구분합니다.</param>
        public void log(string logMassage, LogType logType, bool bUpdateUI)
        {
            string logpath = getLogPath(logType); // 저장할 파일주소를 반환받습니다.
            string strDir = logpath.Substring(0, logpath.LastIndexOf('\\')); // 파일 제목 이전의 폴더 주소만 가져옵니다.
            string fname = makeFileNameWithTime(); // 기록할 로그의 입력시간을 문자열로 받아옵니다.
            try
            {
                lock (objLock_1)
                {
                    if (!Directory.Exists(strDir)) // 저장할 주소의 폴더들이 존재하는지 검사합니다.
                        Directory.CreateDirectory(strDir); // 폴더를 생성합니다.

                    StreamWriter log = new StreamWriter(logpath, true); // Log 파일을 생성하거나 불러옵니다.
                    string LogMassage = "[" + fname + "] " + logMassage; // 기록할 내용을 형식에 맞추어 저장합니다.
                    using (log)
                        log.WriteLine(LogMassage); // Log 파일에 기록합니다.

                    if (bUpdateUI) // GUI에 기록할 경우
                        if (Program.mFormMain != null) // MainForm이 존재할 경우
                            Program.mFormMain.UpdateLogMsg(LogMassage); // MainForm에 기록합니다.
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this.ToString() + " : " + MethodBase.GetCurrentMethod().Name + ", " + ex.Message, "Waring!");
            }
        }
    }
}