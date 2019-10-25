using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO.Ports;

namespace DSPContest
{
    public partial class SettingForm : Form
    {
        SerialPort portA, portB;
        public int NumberBit;
        public bool Done;
        
        public SettingForm(SerialPort portA, SerialPort portB)
        {
            InitializeComponent();

            this.portA = portA;
            this.portB = portB;

            string[] port = SerialPort.GetPortNames();
            comboBox1.Items.Clear();
            comboBox2.Items.Clear();
            foreach (string name in port)
            {
                comboBox1.Items.Add(name);
                comboBox2.Items.Add(name);
            }
            if(port.Length > 0)
            {
                comboBox1.SelectedIndex = 0;
                comboBox2.SelectedIndex = 0;
            }

            comboBox3.SelectedIndex = 0;

            Done = false;
        }

        private void comboBox1_DropDown(object sender, EventArgs e)
        {
            string[] port = SerialPort.GetPortNames();
            comboBox1.Items.Clear();
            foreach (string name in port)
            {
                comboBox1.Items.Add(name);
            }
        }

        private void comboBox2_DropDown(object sender, EventArgs e)
        {
            string[] port = SerialPort.GetPortNames();
            comboBox2.Items.Clear();
            foreach (string name in port)
            {
                comboBox2.Items.Add(name);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (portA != null && portA.IsOpen)
                {
                    portA.Close();
                }

                if (portA != null && portB.IsOpen)
                {
                    portB.Close();
                }

                portA.PortName = (String)comboBox1.SelectedItem;
                portA.BaudRate = Int32.Parse((String)comboBox3.SelectedItem);
                portA.Open();

                //portB.PortName = (String)comboBox2.SelectedItem;
                //portB.BaudRate = Int32.Parse((String)comboBox3.SelectedItem);
                //portB.Open();

                NumberBit = Int32.Parse(textBox1.Text);

                Done = true;
            }
            catch (Exception)
            {
                Done = false;

                if (portA != null && portA.IsOpen)
                {
                    portA.Close();
                }

                if (portA != null && portB.IsOpen)
                {
                    portB.Close();
                }
            }

            this.Close();
        }
    }
}
