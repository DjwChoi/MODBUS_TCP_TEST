using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MODBUS_TCP
{
    static class Program
    {
        public static FormMain mFormMain = null;

        /// <summary>
        /// 해당 응용 프로그램의 주 진입점입니다.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                mFormMain = new FormMain();

                Application.Run(mFormMain);
            }
            catch (Exception eMain)
            {
                MessageBox.Show(eMain.Message);
            }
        }
    }
}
