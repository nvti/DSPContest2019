using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DSPContest
{
    public partial class MainForm : Form
    {
        SettingForm setting;
        Random rnd;

        int bitErr = 0;
        int byteTransfered = 0;
        bool transferEnable = false;
        byte[] data = new byte[125000];

        byte[] data_rec = new byte[125000];
        //DateTime start_time;
        Stopwatch sw;

        public MainForm()
        {
            InitializeComponent();

            setting = new SettingForm(serialPort_A, serialPort_B);

            rnd = new Random();
            rnd.NextBytes(data);

            int a = countBit(0x03);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(button1.Text == "Start")
            {
                button1.Text = "Stop";
                button1.BackColor = Color.Red;
                button2.Enabled = false;

                // Start
                try
                {
                    int byte_tranfer = 0;
                    while(byte_tranfer < setting.NumberBit / 8)
                    {
                        serialPort_A.Write(data, byte_tranfer, 100);
                        byte_tranfer += 100;
                        //Thread.Sleep(1);
                    }
                    

                    bitErr = 0;
                    byteTransfered = 0;
                    chart1.Series["BER"].Points.Clear();
                    chart1.Series["bit per sec"].Points.Clear();

                    //start_time = DateTime.Now;
                    sw = Stopwatch.StartNew();
                    transferEnable = true;
                }
                catch(Exception ex)
                {
                    Console.WriteLine("button1_Click" + ex.Message);
                }
            }
            else
            {
                // Stop
                stopTransfer();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            setting.ShowDialog();

            if (setting.Done)
            {
                button1.Text = "Start";
                button1.BackColor = Color.Lime;
                button1.Enabled = true;
            }
            else
            {
                button1.Enabled = false;
            }
        }

        private void serialPort_B_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            if (!transferEnable)
            {
                serialPort_A.ReadExisting();
                return;
            }
            try
            {
                while(serialPort_A.BytesToRead > 0 && transferEnable)
                {
                    byte data_a = (byte)(serialPort_A.ReadByte() & 0xff);
                    data_rec[byteTransfered] = data_a;
                    bitErr += countBit(data_a ^ this.data[byteTransfered]);
              
                    byteTransfered++;

                    if(byteTransfered % 200 == 0)
                    {
                        //this.Invoke(new MethodInvoker(updateChart));
                    }

                    if (byteTransfered >= setting.NumberBit / 8)
                    {
                        this.Invoke(new MethodInvoker(stopTransfer));
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("serialPort_B_DataReceived " + ex.Message);
            }
        }

        void updateChart()
        {
            progressBar1.Value = byteTransfered * 800 / setting.NumberBit;

            Console.WriteLine("Biterr: " + bitErr);
            if(bitErr == 0)
            {
                chart1.Series["BER"].Points.Add(-10);
            }
            else
            {
                chart1.Series["BER"].Points.Add(Math.Log10((double)bitErr / byteTransfered / 8));
            }
            
            //DateTime then = DateTime.Now;
            chart1.Series["bit per sec"].Points.Add((float)byteTransfered * 8000 / (sw.ElapsedMilliseconds));
        }

        int countBit(int value)
        {
            int count = 0;
            while (value != 0)
            {
                count++;
                value &= value - 1;
            }
            return count;
        }

        void stopTransfer()
        {
            transferEnable = false;
            button1.Text = "Start";
            button1.BackColor = Color.Lime;
            button2.Enabled = true;

            string ber;
            if (bitErr == 0)
            {
                ber = "0";
            }
            else
            {
                ber = "10 ^ " + (Math.Log10((double)bitErr / byteTransfered / 8));
            }

            MessageBox.Show("Number of bit: " + setting.NumberBit + "\nBit error: " + bitErr + 
                "\nTime: " + sw.ElapsedMilliseconds/1000 + "\nBER: " + ber + "\nbps: " + (float)byteTransfered * 8000 / (sw.ElapsedMilliseconds));
            sw.Stop();
        }
    }
}
