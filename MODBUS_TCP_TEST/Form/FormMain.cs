using System;
using System.Reflection;
using System.Windows.Forms;

namespace MODBUS_TCP
{
    public partial class FormMain : Form
    {
        private Logger mLogger = null;

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
                tsslConnectionStatus.Text = "Disconneted.";

                // Disenable GroupBoxs
                gbModbusTools.Enabled = false;
                gbTransmitterTools.Enabled = false;
                gbTransmitterLog.Enabled = false;
                gbReceiverLog.Enabled = false;

                // Do Initialize for Member, Logger Class 
                mLogger = new Logger();
                mLogger.OnLogged += mLogger_OnLogged;
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

        }
    }
}