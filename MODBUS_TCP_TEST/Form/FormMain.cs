using System;
using System.Reflection;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MODBUS_TCP
{
    public partial class FormMain : Form
    {
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
            }
            catch (Exception eFormMain_Load)
            {
                MessageBox.Show(eFormMain_Load.Message);
            }
        }
    }
}
