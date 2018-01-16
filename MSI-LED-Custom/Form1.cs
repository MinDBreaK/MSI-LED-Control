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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            label1.Text = Program.ledColor.ToString();
            label4.Text = Program.adapterIndexes.Count.ToString();  

            textBox1.Text = Program.ledColor.R.ToString();
            textBox2.Text = Program.ledColor.G.ToString();
            textBox3.Text = Program.ledColor.B.ToString();

            textBox4.Text = Program.tempMin.ToString();
            textBox5.Text = Program.tempMax.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Int32.Parse(textBox1.Text) > 255)
            {
                textBox1.Text = "255";
            }
            if (Int32.Parse(textBox1.Text) < 0)
            {
                textBox1.Text = "0";
            }
            if (Int32.Parse(textBox2.Text) > 255)
            {
                textBox2.Text = "255";
            }
            if (Int32.Parse(textBox2.Text) < 0)
            {
                textBox2.Text = "0";
            }
            if (Int32.Parse(textBox3.Text) > 255)
            {
                textBox3.Text = "255";
            }
            if (Int32.Parse(textBox3.Text) < 0)
            {
                textBox3.Text = "0";
            }
            

            Program.ledColor = Color.FromArgb(255, Int32.Parse(textBox1.Text), Int32.Parse(textBox2.Text), Int32.Parse(textBox3.Text));
            label1.Text = Program.ledColor.ToString();
            Program.tempMin = Int32.Parse(textBox4.Text);
            Program.tempMax = Int32.Parse(textBox5.Text);

            Program.ledManager.UpdateAll(Program.ledColor, Program.animationType);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch ( comboBox1.SelectedIndex )
            {
                case (int)AnimationType.NoAnimation:
                    Program.animationType = AnimationType.NoAnimation;
                    break;

                case (int)AnimationType.Breathing:
                    Program.animationType = AnimationType.Breathing;
                    break;

                case (int)AnimationType.Flashing:
                    Program.animationType = AnimationType.Flashing;
                    break;

                case (int)AnimationType.DoubleFlashing:
                    Program.animationType = AnimationType.DoubleFlashing;
                    break;

                case (int)AnimationType.Off:
                    Program.animationType = AnimationType.Off;
                    break;

                case (int)AnimationType.TemperatureBased:
                    Program.animationType = AnimationType.TemperatureBased;
                    break;
            }
        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form_Debug form_debug = new Form_Debug();
            form_debug.Show();
        }

        private void label9_Click(object sender, EventArgs e)
        {

        }
    }
}
