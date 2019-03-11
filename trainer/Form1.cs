using System;
using System.Threading;
using System.Windows.Forms;

namespace trainer
{
    public partial class Form1 : Form
    {
        Thread guiThread;
        Thread connectThread;
        Memory memory;

        string sunAddress = "0019926C";
        int[] sunOffSet = { 0x5560 };
        int sunValue;

        public Form1()
        {
            InitializeComponent();

            connectThread = new Thread(connect);
            connectThread.IsBackground = true;
            connectThread.Start();
            guiThread = new Thread(gui);
            guiThread.IsBackground = true;
            guiThread.Start();
        }

        void gui()
        {
            while(true)
            {
                Thread.Sleep(100);
                try
                {
                    this.Invoke(new MethodInvoker(delegate ()
                    {
                        if (memory != null)
                        {
                            if (memory.connected == true)
                            {
                                label2.Text = "Conectat la joc";
                                panel1.Enabled = true;
                            }
                            else
                            {
                                label2.Text = "Nu gasesc jocul";
                                panel1.Enabled = false;
                            }

                            sunValue = memory.PointerRead(sunAddress, sunOffSet);
                            label1.Text = "Sun: " + sunValue;
                        }
                        else
                        {
                            label2.Text = "Nu gasesc jocul";
                            panel1.Enabled = false;
                        }
                    }));
                }
                catch
                {
                    ;
                }
            }
        }

        void connect()
        {
            memory = new Memory("popcapgame1");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int newValue = int.Parse(sunTxtBox.Text);
            memory.PointerWrite(sunAddress, BitConverter.GetBytes(newValue), sunOffSet);
            sunTxtBox.Text = "";
        }
    }
}
