using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Threading;

namespace 华春板卡继电器测试
{
    public partial class Form2 : Form
    {
        Form1 mainform = new Form1();

        public Form2(Form1 form1)
        {
            mainform = form1;
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {

            if (mainform.serialPort1.IsOpen == false)
            {

                MessageBox.Show("串口未打开，请先打开串口", "提示");
                return;

            }

            int size = 0;
            byte[] tempbuffer = new byte[256];

            
            size = mainform.serialPort1.BytesToRead;
            mainform.serialPort1.Read(tempbuffer, 0, size);//第一次发命令前，先把串口缓冲区读空

            
            byte[] Sendcmd = mainform.StringToByte2(textBox1.Text);
            if (!mainform.PortSend(Sendcmd))
            {
                MessageBox.Show("发送错误", "提示");
                return;
            }
            mainform.ShowMessage(Sendcmd);
        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (mainform.serialPort1.IsOpen == false)
            {

                MessageBox.Show("串口未打开，请先打开串口", "提示");
                return;

            }

            int size = 0;
            byte[] tempbuffer = new byte[256];


            size = mainform.serialPort1.BytesToRead;
            mainform.serialPort1.Read(tempbuffer, 0, size);//第一次发命令前，先把串口缓冲区读空


            byte[] Sendcmd = mainform.StringToByte2(textBox2.Text);
            if (!mainform.PortSend(Sendcmd))
            {
                MessageBox.Show("发送错误", "提示");
                return;
            }
            mainform.ShowMessage(Sendcmd);
        }

        private void button2_Click(object sender, EventArgs e)
        {

            if (mainform.serialPort1.IsOpen == false)
            {
                MessageBox.Show("串口未打开，请先打开串口", "提示");
                return;
            }

            int size = 0;
            byte[] tempbuffer = new byte[256];

            size = mainform.serialPort1.BytesToRead;
            mainform.serialPort1.Read(tempbuffer, 0, size);//第一次发命令前，先把串口缓冲区读空

            byte[] Sendcmd = mainform.StringToByte2(textBox3.Text);
            if (!mainform.PortSend(Sendcmd))
            {
                MessageBox.Show("发送错误", "提示");
                return;
            }
            mainform.ShowMessage(Sendcmd);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (mainform.serialPort1.IsOpen == false)
            {
                MessageBox.Show("串口未打开，请先打开串口", "提示");
                return;
            }

            int size = 0;
            byte[] tempbuffer = new byte[256];

            size = mainform.serialPort1.BytesToRead;
            mainform.serialPort1.Read(tempbuffer, 0, size);//第一次发命令前，先把串口缓冲区读空

            byte[] Sendcmd = mainform.StringToByte2(textBox6.Text);
            if (!mainform.PortSend(Sendcmd))
            {
                MessageBox.Show("发送错误", "提示");
                return;
            }
            mainform.ShowMessage(Sendcmd);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (mainform.serialPort1.IsOpen == false)
            {
                MessageBox.Show("串口未打开，请先打开串口", "提示");
                return;
            }

            int size = 0;
            byte[] tempbuffer = new byte[256];

            size = mainform.serialPort1.BytesToRead;
            mainform.serialPort1.Read(tempbuffer, 0, size);//第一次发命令前，先把串口缓冲区读空

            byte[] Sendcmd = mainform.StringToByte2(textBox5.Text);
            if (!mainform.PortSend(Sendcmd))
            {
                MessageBox.Show("发送错误", "提示");
                return;
            }
            mainform.ShowMessage(Sendcmd);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (mainform.serialPort1.IsOpen == false)
            {
                MessageBox.Show("串口未打开，请先打开串口", "提示");
                return;
            }

            int size = 0;
            byte[] tempbuffer = new byte[256];

            size = mainform.serialPort1.BytesToRead;
            mainform.serialPort1.Read(tempbuffer, 0, size);//第一次发命令前，先把串口缓冲区读空

            byte[] Sendcmd = mainform.StringToByte2(textBox4.Text);
            if (!mainform.PortSend(Sendcmd))
            {
                MessageBox.Show("发送错误", "提示");
                return;
            }
            mainform.ShowMessage(Sendcmd);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (mainform.serialPort1.IsOpen == false)
            {
                MessageBox.Show("串口未打开，请先打开串口", "提示");
                return;
            }

            int size = 0;
            byte[] tempbuffer = new byte[256];

            size = mainform.serialPort1.BytesToRead;
            mainform.serialPort1.Read(tempbuffer, 0, size);//第一次发命令前，先把串口缓冲区读空

            byte[] Sendcmd = mainform.StringToByte2(textBox9.Text);
            if (!mainform.PortSend(Sendcmd))
            {
                MessageBox.Show("发送错误", "提示");
                return;
            }
            mainform.ShowMessage(Sendcmd);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (mainform.serialPort1.IsOpen == false)
            {
                MessageBox.Show("串口未打开，请先打开串口", "提示");
                return;
            }

            int size = 0;
            byte[] tempbuffer = new byte[256];

            size = mainform.serialPort1.BytesToRead;
            mainform.serialPort1.Read(tempbuffer, 0, size);//第一次发命令前，先把串口缓冲区读空

            byte[] Sendcmd = mainform.StringToByte2(textBox8.Text);
            if (!mainform.PortSend(Sendcmd))
            {
                MessageBox.Show("发送错误", "提示");
                return;
            }
            mainform.ShowMessage(Sendcmd);
        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (mainform.serialPort1.IsOpen == false)
            {
                MessageBox.Show("串口未打开，请先打开串口", "提示");
                return;
            }

            int size = 0;
            byte[] tempbuffer = new byte[256];

            size = mainform.serialPort1.BytesToRead;
            mainform.serialPort1.Read(tempbuffer, 0, size);//第一次发命令前，先把串口缓冲区读空

            byte[] Sendcmd = mainform.StringToByte2(textBox7.Text);
            if (!mainform.PortSend(Sendcmd))
            {
                MessageBox.Show("发送错误", "提示");
                return;
            }
            mainform.ShowMessage(Sendcmd);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (mainform.serialPort1.IsOpen == false)
            {
                MessageBox.Show("串口未打开，请先打开串口", "提示");
                return;
            }

            int size = 0;
            byte[] tempbuffer = new byte[256];

            size = mainform.serialPort1.BytesToRead;
            mainform.serialPort1.Read(tempbuffer, 0, size);//第一次发命令前，先把串口缓冲区读空

            byte[] Sendcmd = mainform.StringToByte2(textBox10.Text);
            if (!mainform.PortSend(Sendcmd))
            {
                MessageBox.Show("发送错误", "提示");
                return;
            }
            mainform.ShowMessage(Sendcmd);
        }

        private void button20_Click(object sender, EventArgs e)
        {
            if (mainform.serialPort1.IsOpen == false)
            {
                MessageBox.Show("串口未打开，请先打开串口", "提示");
                return;
            }

            int size = 0;
            byte[] tempbuffer = new byte[256];

            size = mainform.serialPort1.BytesToRead;
            mainform.serialPort1.Read(tempbuffer, 0, size);//第一次发命令前，先把串口缓冲区读空

            byte[] Sendcmd = mainform.StringToByte2(textBox21.Text);
            if (!mainform.PortSend(Sendcmd))
            {
                MessageBox.Show("发送错误", "提示");
                return;
            }
            mainform.ShowMessage(Sendcmd);
        }

        private void button19_Click(object sender, EventArgs e)
        {
            if (mainform.serialPort1.IsOpen == false)
            {
                MessageBox.Show("串口未打开，请先打开串口", "提示");
                return;
            }

            int size = 0;
            byte[] tempbuffer = new byte[256];

            size = mainform.serialPort1.BytesToRead;
            mainform.serialPort1.Read(tempbuffer, 0, size);//第一次发命令前，先把串口缓冲区读空

            byte[] Sendcmd = mainform.StringToByte2(textBox20.Text);
            if (!mainform.PortSend(Sendcmd))
            {
                MessageBox.Show("发送错误", "提示");
                return;
            }
            mainform.ShowMessage(Sendcmd);
        }

        private void button18_Click(object sender, EventArgs e)
        {
            if (mainform.serialPort1.IsOpen == false)
            {
                MessageBox.Show("串口未打开，请先打开串口", "提示");
                return;
            }

            int size = 0;
            byte[] tempbuffer = new byte[256];

            size = mainform.serialPort1.BytesToRead;
            mainform.serialPort1.Read(tempbuffer, 0, size);//第一次发命令前，先把串口缓冲区读空

            byte[] Sendcmd = mainform.StringToByte2(textBox19.Text);
            if (!mainform.PortSend(Sendcmd))
            {
                MessageBox.Show("发送错误", "提示");
                return;
            }
            mainform.ShowMessage(Sendcmd);
        }

        private void button17_Click(object sender, EventArgs e)
        {
            if (mainform.serialPort1.IsOpen == false)
            {
                MessageBox.Show("串口未打开，请先打开串口", "提示");
                return;
            }

            int size = 0;
            byte[] tempbuffer = new byte[256];

            size = mainform.serialPort1.BytesToRead;
            mainform.serialPort1.Read(tempbuffer, 0, size);//第一次发命令前，先把串口缓冲区读空

            byte[] Sendcmd = mainform.StringToByte2(textBox18.Text);
            if (!mainform.PortSend(Sendcmd))
            {
                MessageBox.Show("发送错误", "提示");
                return;
            }
            mainform.ShowMessage(Sendcmd);
        }

        private void button16_Click(object sender, EventArgs e)
        {
            if (mainform.serialPort1.IsOpen == false)
            {
                MessageBox.Show("串口未打开，请先打开串口", "提示");
                return;
            }

            int size = 0;
            byte[] tempbuffer = new byte[256];

            size = mainform.serialPort1.BytesToRead;
            mainform.serialPort1.Read(tempbuffer, 0, size);//第一次发命令前，先把串口缓冲区读空

            byte[] Sendcmd = mainform.StringToByte2(textBox17.Text);
            if (!mainform.PortSend(Sendcmd))
            {
                MessageBox.Show("发送错误", "提示");
                return;
            }
            mainform.ShowMessage(Sendcmd);
        }

        private void button15_Click(object sender, EventArgs e)
        {
            if (mainform.serialPort1.IsOpen == false)
            {
                MessageBox.Show("串口未打开，请先打开串口", "提示");
                return;
            }

            int size = 0;
            byte[] tempbuffer = new byte[256];

            size = mainform.serialPort1.BytesToRead;
            mainform.serialPort1.Read(tempbuffer, 0, size);//第一次发命令前，先把串口缓冲区读空

            byte[] Sendcmd = mainform.StringToByte2(textBox16.Text);
            if (!mainform.PortSend(Sendcmd))
            {
                MessageBox.Show("发送错误", "提示");
                return;
            }
            mainform.ShowMessage(Sendcmd);
        }

        private void button14_Click(object sender, EventArgs e)
        {
            if (mainform.serialPort1.IsOpen == false)
            {
                MessageBox.Show("串口未打开，请先打开串口", "提示");
                return;
            }

            int size = 0;
            byte[] tempbuffer = new byte[256];

            size = mainform.serialPort1.BytesToRead;
            mainform.serialPort1.Read(tempbuffer, 0, size);//第一次发命令前，先把串口缓冲区读空

            byte[] Sendcmd = mainform.StringToByte2(textBox15.Text);
            if (!mainform.PortSend(Sendcmd))
            {
                MessageBox.Show("发送错误", "提示");
                return;
            }
            mainform.ShowMessage(Sendcmd);
        }

        private void button13_Click(object sender, EventArgs e)
        {
            if (mainform.serialPort1.IsOpen == false)
            {
                MessageBox.Show("串口未打开，请先打开串口", "提示");
                return;
            }

            int size = 0;
            byte[] tempbuffer = new byte[256];

            size = mainform.serialPort1.BytesToRead;
            mainform.serialPort1.Read(tempbuffer, 0, size);//第一次发命令前，先把串口缓冲区读空

            byte[] Sendcmd = mainform.StringToByte2(textBox14.Text);
            if (!mainform.PortSend(Sendcmd))
            {
                MessageBox.Show("发送错误", "提示");
                return;
            }
            mainform.ShowMessage(Sendcmd);
        }

        private void button12_Click(object sender, EventArgs e)
        {
            if (mainform.serialPort1.IsOpen == false)
            {
                MessageBox.Show("串口未打开，请先打开串口", "提示");
                return;
            }

            int size = 0;
            byte[] tempbuffer = new byte[256];

            size = mainform.serialPort1.BytesToRead;
            mainform.serialPort1.Read(tempbuffer, 0, size);//第一次发命令前，先把串口缓冲区读空

            byte[] Sendcmd = mainform.StringToByte2(textBox13.Text);
            if (!mainform.PortSend(Sendcmd))
            {
                MessageBox.Show("发送错误", "提示");
                return;
            }
            mainform.ShowMessage(Sendcmd);
        }

        private void button11_Click(object sender, EventArgs e)
        {
            if (mainform.serialPort1.IsOpen == false)
            {
                MessageBox.Show("串口未打开，请先打开串口", "提示");
                return;
            }

            int size = 0;
            byte[] tempbuffer = new byte[256];

            size = mainform.serialPort1.BytesToRead;
            mainform.serialPort1.Read(tempbuffer, 0, size);//第一次发命令前，先把串口缓冲区读空

            byte[] Sendcmd = mainform.StringToByte2(textBox12.Text);
            if (!mainform.PortSend(Sendcmd))
            {
                MessageBox.Show("发送错误", "提示");
                return;
            }
            mainform.ShowMessage(Sendcmd);
        }

        private void checkBox22_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox22.Checked == true)
            {
                checkBox4.Checked = true;
                checkBox5.Checked = true;
                checkBox6.Checked = true;
                checkBox7.Checked = true;
                checkBox8.Checked = true;
                checkBox9.Checked = true;
                checkBox10.Checked = true;
                checkBox11.Checked = true;
                checkBox3.Checked = true;
                checkBox2.Checked = true;

                checkBox21.Checked = true;
                checkBox20.Checked = true;
                checkBox19.Checked = true;
                checkBox18.Checked = true;
                checkBox17.Checked = true;
                checkBox16.Checked = true;
                checkBox15.Checked = true;
                checkBox14.Checked = true;
                checkBox13.Checked = true;
                checkBox12.Checked = true;
                
            }
            else
            {
                checkBox4.Checked = false;
                checkBox5.Checked = false;
                checkBox6.Checked = false;
                checkBox7.Checked = false;
                checkBox8.Checked = false;
                checkBox9.Checked = false;
                checkBox10.Checked = false;
                checkBox11.Checked = false;
                checkBox3.Checked = false;
                checkBox2.Checked = false;

                checkBox21.Checked = false;
                checkBox20.Checked = false;
                checkBox19.Checked = false;
                checkBox18.Checked = false;
                checkBox17.Checked = false;
                checkBox16.Checked = false;
                checkBox15.Checked = false;
                checkBox14.Checked = false;
                checkBox13.Checked = false;
                checkBox12.Checked = false;

            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                if (mainform.serialPort1.IsOpen == false)
                {
                    checkBox1.Checked = false;
                    MessageBox.Show("串口未打开，请先打开串口", "提示");
                    return;

                }

                mainform.index = 0;
                if (checkBox4.Checked == true && textBox1.Text != "")
                {
                    mainform.cmd[mainform.index] = textBox1.Text;
                    mainform.index++;
                }
                if (checkBox5.Checked == true && textBox2.Text != "")
                {
                    mainform.cmd[mainform.index] = textBox2.Text;
                    mainform.index++;
                }
                if (checkBox6.Checked == true && textBox3.Text != "")
                {
                    mainform.cmd[mainform.index] = textBox3.Text;
                    mainform.index++;
                }
                if (checkBox7.Checked == true && textBox6.Text != "")
                {
                    mainform.cmd[mainform.index] = textBox6.Text;
                    mainform.index++;
                }
                if (checkBox8.Checked == true && textBox5.Text != "")
                {
                    mainform.cmd[mainform.index] = textBox5.Text;
                    mainform.index++;
                }
                if (checkBox9.Checked == true && textBox4.Text != "")
                {
                    mainform.cmd[mainform.index] = textBox4.Text;
                    mainform.index++;
                }
                if (checkBox10.Checked == true && textBox9.Text != "")
                {
                    mainform.cmd[mainform.index] = textBox9.Text;
                    mainform.index++;
                }
                if (checkBox11.Checked == true && textBox8.Text != "")
                {
                    mainform.cmd[mainform.index] = textBox8.Text;
                    mainform.index++;
                }
                if (checkBox3.Checked == true && textBox7.Text != "")
                {
                    mainform.cmd[mainform.index] = textBox7.Text;
                    mainform.index++;
                }
                if (checkBox2.Checked == true && textBox10.Text != "")
                {
                    mainform.cmd[mainform.index] = textBox10.Text;
                    mainform.index++;
                }
                if (checkBox21.Checked == true && textBox21.Text != "")
                {
                    mainform.cmd[mainform.index] = textBox21.Text;
                    mainform.index++;
                }
                if (checkBox20.Checked == true && textBox20.Text != "")
                {
                    mainform.cmd[mainform.index] = textBox20.Text;
                    mainform.index++;
                }
                if (checkBox19.Checked == true && textBox19.Text != "")
                {
                    mainform.cmd[mainform.index] = textBox19.Text;
                    mainform.index++;
                }
                if (checkBox18.Checked == true && textBox18.Text != "")
                {
                    mainform.cmd[mainform.index] = textBox18.Text;
                    mainform.index++;
                }
                if (checkBox17.Checked == true && textBox17.Text != "")
                {
                    mainform.cmd[mainform.index] = textBox17.Text;
                    mainform.index++;
                }
                if (checkBox16.Checked == true && textBox16.Text != "")
                {
                    mainform.cmd[mainform.index] = textBox16.Text;
                    mainform.index++;
                }
                if (checkBox15.Checked == true && textBox15.Text != "")
                {
                    mainform.cmd[mainform.index] = textBox15.Text;
                    mainform.index++;
                }
                if (checkBox14.Checked == true && textBox14.Text != "")
                {
                    mainform.cmd[mainform.index] = textBox14.Text;
                    mainform.index++;
                }
                if (checkBox13.Checked == true && textBox13.Text != "")
                {
                    mainform.cmd[mainform.index] = textBox13.Text;
                    mainform.index++;
                }
                if (checkBox12.Checked == true && textBox12.Text != "")
                {
                    mainform.cmd[mainform.index] = textBox12.Text;
                    mainform.index++;
                }
                

                if (mainform.index == 0)
                {
                    checkBox1.Checked = false;
                    MessageBox.Show("请选择要发送的命令帧", "提示");
                    return;

                }

                mainform.SendTime = Convert.ToInt32(textBox11.Text);
                mainform.testThread = new Thread(new ThreadStart(mainform.testfunction2));
                Control.CheckForIllegalCrossThreadCalls = false; //加上次语句的功能在线程中操作控件
                mainform.testThread.Start();
            }
            else
            {

                //button19.Text = "暂停";
                //button19.BackColor = Color.Lime;

                if (mainform.testThread != null)
                {
                    //if (testThread.ThreadState == ThreadState.Running || testThread.ThreadState == ThreadState.Suspended)
                    if (mainform.testThread.ThreadState == ThreadState.Running)
                    {

                        mainform.testThread.Abort();
                    }
                }
            }
        }

        private void button21_Click(object sender, EventArgs e)
        {
            if (button21.Text == "暂停")
            {
                if (mainform.testThread != null)
                {
                    if (mainform.testThread.ThreadState == ThreadState.Running)
                    {
                        mainform.testThread.Suspend();
                        button21.Text = "继续";
                        button21.BackColor = Color.Red;
                    }
                }
            }
            else
            {
                if (mainform.testThread != null)
                {
                    if (mainform.testThread.ThreadState == ThreadState.Suspended)
                    {
                        mainform.testThread.Resume();
                        button21.Text = "暂停";
                        button21.BackColor = Color.Lime;
                    }
                }
            }
        }
    }
}
