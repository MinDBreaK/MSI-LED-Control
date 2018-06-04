using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace MSI_LED_Custom
{
    public partial class Form_Debug : Form
    {
        public Form_Debug()
        {
            InitializeComponent();
        }

        private void Form_Debug_Load(object sender, EventArgs e)
        {
            if (Program.manufacturer == Manufacturer.AMD)
            {
                label11.Text = Program.vendorCode;
                label13.Text = Program.deviceCode;
                label15.Text = Program.subVendorCode;

                textBox1.Text = "PnP Card : " + Program.AdlGraphicsInfo.Card_PNP + "\r\n"
                              + "Vendor Code : " + Program.vendorCode + "\r\n"
                              + "Device Code : " + Program.deviceCode + "\r\n"
                              + "Subvendor Code : " + Program.subVendorCode + "\r\n";
            }
            else if (Program.manufacturer == Manufacturer.Nvidia)
            {
                label11.Text = Program.vendorCode;
                label13.Text = Program.deviceCode;
                label15.Text = Program.subVendorCode;

                textBox1.Text = "PnP Card : " + Program.NdaGraphicsInfo.Card_pDeviceId + "\r\n"
                              + "Vendor Code : " + Program.vendorCode + "\r\n"
                              + "Device Code : " + Program.deviceCode + "\r\n"
                              + "Subvendor Code : " + Program.subVendorCode + "\r\n"
                              + "Card name : " + Program.NdaGraphicsInfo.Card_FullName + "\r\n"
                              + "Card PnP2 : " + Program.NdaGraphicsInfo.Card_PNP;
            }
            if (Program.AdlGraphicsInfo.Card_PNP != null)
            {
                label9.Text = Program.AdlGraphicsInfo.Card_PNP;
            }

            

            textBox1.AppendText("Manufacturer : " + Program.manufacturer + "\r\n");

            for (int i = 0; i < Program.args.Length; i++)
            {
                textBox1.AppendText( "Arg[" + i + "] =" + Program.args[i] + "\r\n");
            }
        }
    }
}
