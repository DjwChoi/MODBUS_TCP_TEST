using System;
using System.Reflection;
using System.Windows.Forms;

namespace MODBUS_TCP
{
    public partial class FormMain : Form
    {
        private Logger mLogger = null;
        private Master mMaster = null;

        private bool[] isTransactionID = new bool[65535];

        public FormMain()
        {
            InitializeComponent();
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            try
            {
                // Build Version
                tsslVersion.Text = "Build Version " + Assembly.GetExecutingAssembly().GetName().Version.ToString();

                // Connection Status
                tsslConnectionStatus.Text = "Disconnected.";

                // Enable GroupBoxs
                gbTCPSetting.Enabled = true;

                // Disenable GroupBoxs
                gbModbusTools.Enabled = false;
                gbTransmitterTools.Enabled = false;
                gbTransmitterLog.Enabled = false;
                gbReceiverLog.Enabled = false;

                // Do Initialize for Member, Logger Class 
                mLogger = new Logger();
                mLogger.OnLogged += mLogger_OnLogged;

                // Do Initialize for Member, Master Class
                mMaster = new Master();
                mMaster.OnReceivedData += mMaster_OnReceivedData;
                mMaster.OnException += mMaster_OnException;

            }
            catch (Exception eFormMain_Load)
            {
                MessageBox.Show(this.ToString() + " : " + MethodBase.GetCurrentMethod().Name + ", " + eFormMain_Load.Message, "Waring!");
            }
        }

        #region Logger Event

        private void mLogger_OnLogged(string LogMassage, LogType LogType)
        {
            switch (LogType)
            {
                case LogType.Transmitter:

                    lbTransmitter.Items.Add(LogMassage);
                    break;

                case LogType.Receiver:

                    lbReceiver.Items.Add(LogMassage);
                    break;
            }
        }

        #endregion

        #region Master Events

        private void mMaster_OnReceivedData(byte[] data)
        {
            if (isTransactionID[data[0]] == true) isTransactionID[data[0]] = false;

            mLogger.log(System.BitConverter.ToString(data), LogType.Receiver, true);
        }

        private void mMaster_OnException(ushort id, byte unit, byte function, ExceptionCode exception)
        {

        }

        #endregion

        #region IP TextBoxs Events

        private void tbIP1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))
            {
                e.Handled = true;
            }
        }

        private void tbIP1_TextChanged(object sender, EventArgs e)
        {
            if (tbIP1.TextLength == 3)
                this.ActiveControl = tbIP2;
        }

        private void tbIP1_Leave(object sender, EventArgs e)
        {
            while (tbIP1.TextLength != 3)
            {
                tbIP1.Text = "0" + tbIP1.Text;
            }
        }

        private void tbIP2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))
            {
                e.Handled = true;
            }
        }

        private void tbIP2_TextChanged(object sender, EventArgs e)
        {
            if (tbIP2.TextLength == 3)
                this.ActiveControl = tbIP3;
        }

        private void tbIP2_Leave(object sender, EventArgs e)
        {
            while (tbIP2.TextLength != 3)
            {
                tbIP2.Text = "0" + tbIP2.Text;
            }
        }

        private void tbIP3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))
            {
                e.Handled = true;
            }
        }

        private void tbIP3_TextChanged(object sender, EventArgs e)
        {
            if (tbIP3.TextLength == 3)
                this.ActiveControl = tbIP4;
        }

        private void tbIP3_Leave(object sender, EventArgs e)
        {
            while (tbIP3.TextLength != 3)
            {
                tbIP3.Text = "0" + tbIP3.Text;
            }
        }

        private void tbIP4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))
            {
                e.Handled = true;
            }
        }

        private void tbIP4_TextChanged(object sender, EventArgs e)
        {
            if (tbIP4.TextLength == 3)
                this.ActiveControl = tbPort;
        }

        private void tbIP4_Leave(object sender, EventArgs e)
        {
            while (tbIP4.TextLength != 3)
            {
                tbIP4.Text = "0" + tbIP4.Text;
            }
        }

        #endregion

        #region Port TextBoxs Events

        private void tbPort_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsDigit(e.KeyChar) || e.KeyChar == Convert.ToChar(Keys.Back)))
            {
                e.Handled = true;
            }
        }

        private void tbPort_Leave(object sender, EventArgs e)
        {
            while (tbPort.TextLength != 5)
            {
                tbPort.Text = "0" + tbPort.Text;
            }

            if (Convert.ToInt32(tbPort.Text) > 65535)
                tbPort.Text = "65535";
        }

        #endregion

        private void btConnect_Click(object sender, EventArgs e)
        {
            try
            {
                if (!mMaster.bConnected)
                {
                    mMaster.Connect(string.Format("{0}.{1}.{2}.{3}", tbIP1.Text, tbIP2.Text, tbIP3.Text, tbIP4.Text), Convert.ToUInt16(tbPort.Text));

                    // Connection Status
                    btConnect.Text = "Disconnect";
                    tsslConnectionStatus.Text = "Connected.";

                    // Enable GroupBoxs
                    gbModbusTools.Enabled = true;
                    gbTransmitterTools.Enabled = true;
                    gbTransmitterLog.Enabled = true;
                    gbReceiverLog.Enabled = true;

                    // Disenable GroupBoxs
                    tbIP1.Enabled = false;
                    tbIP2.Enabled = false;
                    tbIP3.Enabled = false;
                    tbIP4.Enabled = false;
                    tbPort.Enabled = false;
                }
                else
                {
                    mMaster.Disconnect();

                    // Connection Status
                    btConnect.Text = "Connect";
                    tsslConnectionStatus.Text = "Disconnected.";

                    // Enable GroupBoxs
                    gbTCPSetting.Enabled = true;

                    // Disenable GroupBoxs
                    gbModbusTools.Enabled = false;
                    gbTransmitterTools.Enabled = false;
                    gbTransmitterLog.Enabled = false;
                    gbReceiverLog.Enabled = false;
                }
                
            }
            catch (Exception eFormMain_Load)
            {
                MessageBox.Show(this.ToString() + " : " + MethodBase.GetCurrentMethod().Name + ", " + eFormMain_Load.Message, "Waring!");
            }
        }

        private void btSendMassage_Click(object sender, EventArgs e)
        {
            byte[] Message = Modbus.Protocol(ref isTransactionID, 00, FunctionCode.ReadHoldingRegister, 11, 1);
            mMaster.WriteData(Message);
            mLogger.log(System.BitConverter.ToString(Message), LogType.Transmitter, true);
        }
    }
}