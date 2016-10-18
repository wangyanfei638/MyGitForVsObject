using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.IO.Ports;
using System.Threading;
using System.IO;

//using System.Diagnostics;

namespace 华春板卡继电器测试
{
    public partial class Form1 : Form
    {


        #region 变量
        private string sComPort = "";
        private int iBaudRate = 115200;

        private int iTestNum = 0;
        private int iInternalTime = 0;
        private int iCurrentTestCount = 0;

        public Thread testThread = null;
        private Thread ReceiveThread = null;
        

        byte[] SendCommand = new byte[3];

        bool flag1 = false;
        //bool flag2 = false;
        //private int itemp = 0;

        //bool flagReceive = false; //true表示收到回帧
        byte[] ReceiveCmdBytes = new byte[256];

        byte[] port485RecvBuf = new byte[256]; //接收缓冲区
        //int port485RecvPos = 0;  //接收字符长度

        static int SendNum = 0;     //发包数
        int ReceiveNum = 0;  //收包数
        int LostNum = 0;     //丢包数
        double WuMaLv = 0.0; //丢包率

        static int SendSizeNum = 0;     //发字节数
        static int ReceiveSizeNum = 0;  //实际收字节数
        int LostSizeNum = 0;     //丢字节数
        double SizeWuMaLv = 0.0; //丢字节率

        static bool flag485 = true;

        static int EachReturnBytes = 4; //发送一帧时应返回的字节数
        static int ReturnBytes = 0; //应收字节数

        //bool timerflag = true;
        HighPerformanceTimer pt;
        private Thread timerThread = null;
        //Stopwatch stopWatch;

        int textbox6num = 0;  //控件textBox6显示文本的行数记录，到一定值时，使textBox6清零，防止溢出索引
        int textbox5num = 0;  //同上

        public int index = 0;  //循环发送帧个数
        public int SendTime = 0;  //自动循环发送间隔
        public string[] cmd = new string[20]; //循环发送帧数组

        int XianShiNum = 300;  //发送区、接收区显示的行数

        #endregion 


        public Form1()
        {
            InitializeComponent();
            comboBox2.SelectedIndex = 1; //波特率115200
            comboBox1.SelectedIndex = 6; //COM7

            comboBox6.SelectedIndex = 3; //
            comboBox3.SelectedIndex = 0; //
            comboBox4.SelectedIndex = 0; //
            comboBox5.SelectedIndex = 2; //
        }

        ////设置延时
        //private double SetDelayTime(double dTime)   //界面没体现
        //{
        //    LARGE_INTEGER startCount;
        //    LARGE_INTEGER endCount;
        //    LARGE_INTEGER FrqCount;

        //    double Elapsed = 0.0;
        //    QueryPerformanceFrequency(&FrqCount);
        //    QueryPerformanceCounter(&startCount);
        //    while (Elapsed < dTime)
        //    {
        //        QueryPerformanceCounter(&endCount);
        //        Elapsed = (double)(endCount.QuadPart - startCount.QuadPart) / FrqCount.QuadPart;
        //    }
        //    return Elapsed;
        //}


        private void timer2_Tick(object sender, EventArgs e)
        {
            textBox5.AppendText(ReceiveCmdBytes[0].ToString());
        }

        private void ReceiveCmd()
        {
            int res = 0;
            int size = 0;
            byte[] tempbuffer = new byte[256];
           
            if (OpenPort() == false)
                return;

            while (true)
            {
                try
                {
                    if (!flag485)
                    {
                        size = serialPort1.BytesToRead;

                        for (int i = 0; i < 3; i++)
                        {
                            if (size > 0)
                            {
                           
                                res = serialPort1.Read(tempbuffer, 0, size);//接收端口1个字节的数据
                                ReceiveNum++;
                                ReceiveSizeNum += size;  //实收字节数
                                //ReturnBytes += EachReturnBytes;  //应收字节数
                                DealPortRecv(tempbuffer, size);
                                //flag485 = true;
                                i=3;
                            }
                            else
                            {
                                Thread.Sleep(20);  //如果某帧没有收到，则延时20ms,然后再次读串口，循环3次后，若仍未收到回帧，则判为丢帧、丢字节
                                size = serialPort1.BytesToRead;
                                i++;
                            }
                        }

                        if(size <= 0)
                        {
                            //LostNum++;
                            //flag485 = true;
                        }

                        flag485 = true;

                    }
                    
                }
                catch 
                {
                    return;
                	
                }
            }

        }

        private void DealPortRecv(byte[] buf, int len)
        {
            if (textbox5num == XianShiNum)
            {
                textBox5.Clear();
                textbox5num = 0;
            }
            else
            {
                textbox5num++;
                textBox5.AppendText(System.DateTime.Now.ToString() + " 接收：" + byteToHexStr2(buf, len) + "\n");
            }
            
            
            toolStripStatusLabel4.Text = "收包数：" + ReceiveNum;
            LostNum = SendNum - ReceiveNum;
            toolStripStatusLabel5.Text = "丢包数：" + LostNum.ToString();
            WuMaLv = (double)LostNum / SendNum * 100.0;
            toolStripStatusLabel6.Text = "丢包率：" + WuMaLv.ToString() + "%";

            toolStripStatusLabel11.Text = "应收字节数：" + ReturnBytes;
            toolStripStatusLabel8.Text = "实收字节数：" + ReceiveSizeNum;
            LostSizeNum = ReturnBytes - ReceiveSizeNum;
            toolStripStatusLabel9.Text = "丢字节数：" + LostSizeNum.ToString();
            SizeWuMaLv = (double)LostSizeNum / ReturnBytes * 100.0;
            toolStripStatusLabel10.Text = "丢字节率：" + SizeWuMaLv.ToString() + "%";

            
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (OpenPort() == true)
            {
                ReceiveThread = new Thread(new ThreadStart(ReceiveCmd));
                Control.CheckForIllegalCrossThreadCalls = false; //加上次语句的功能在线程中操作控件
                ReceiveThread.Start();//启动线程

                //timer2.Start();

                button1.Enabled = false;
            }
        }

        private bool OpenPort()
        {
            if (serialPort1.IsOpen == false)
            {

                sComPort = ComSelect();
                serialPort1.Parity = ParitySelect();
                serialPort1.PortName = sComPort;
                serialPort1.BaudRate = BaudRateSelect();
                serialPort1.DataBits = DataBitsSelect();
                serialPort1.StopBits = StopBitsSelect();
                //serialPort1.

                try
                {
                    serialPort1.Open();
                }
                catch 
                {
                    toolStripStatusLabel2.Text = sComPort + "打开失败！";
                    return false;
                }
                toolStripStatusLabel2.Text = sComPort+"打开成功！";
                toolStripStatusLabel12.Text = "bps:" +iBaudRate.ToString();
            }
            return true;
        }

        private int BaudRateSelect()
        {
            switch (comboBox2.SelectedIndex)
            {
                case 0:
                    iBaudRate = 9600;
                    return iBaudRate;
                case 1:
                    iBaudRate = 115200;
                    return iBaudRate;
                default:
                    iBaudRate = 9600;
                    return iBaudRate;
            }
        }

        //serialPort1.StopBits = StopBits.One;
        private StopBits StopBitsSelect()
        {
            switch (comboBox3.SelectedIndex)
            {
                case 0:
                    return StopBits.One;
                //break;
                case 1:
                    return StopBits.OnePointFive;
                //break;
                case 2:
                    return StopBits.Two;
               
                default:
                    return StopBits.One;

            }
        }



        //serialPort1.DataBits = DataBitsSelect();
        private int DataBitsSelect()
        {
            switch (comboBox6.SelectedIndex)
            {
                case 0:
                    return 5;
                //break;
                case 1:
                    return 6;
                //break;
                case 2:
                    return 7;
                //break;
                case 3:
                    return 8;
                default:
                    return 8;

            }
        }


        //serialPort1.Parity = Parity.None;
        private Parity ParitySelect()
        {
            switch (comboBox4.SelectedIndex)
            {
                case 0:
                    return Parity.None;
                //break;
                case 1:
                    return Parity.Odd;
                //break;
                case 2:
                    return Parity.Even;
                //break;
                case 3:
                    return Parity.Mark;
                //break;
                case 4:
                    return Parity.Space;
                
                default:
                    return Parity.None;

            }
        }

        private string ComSelect()
        {
            switch (comboBox1.SelectedIndex)
            {
                case 0:
                    return "COM1";
                    //break;
                case 1:
                    return "COM2";
                    //break;
                case 2:
                    return "COM3";
                    //break;
                case 3:
                    return "COM4";
                    //break;
                case 4:
                    return "COM5";
                    //break;
                case 5:
                    return "COM6";
                    //break;
                case 6:
                    return "COM7";
                    //break;
                case 7:
                    return "COM8";
                    //break;
                case 8:
                    return "COM9";
                    //break;
                default:
                    return "COM1";
                   
            }
        }



        public bool PortSend(byte[] byteSend)
        {
            try
            {
                //serialPort1.Read()
                serialPort1.Write(byteSend, 0, byteSend.Length);
                flag485 = false;
                SendNum++;
                SendSizeNum += byteSend.Length;  //发字节数

                ReturnBytes += EachReturnBytes;  //应收字节数
                
            }
            catch
            {
                MessageBox.Show("发送失败！", "错误", 0);
                return false;
            }
            return true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (timerThread != null)
            {
                if (timerThread.ThreadState == ThreadState.Running)
                {
                    timerThread.Abort();
                }
            }

            timer3.Stop();

            if (ReceiveThread != null)
            {
                if (ReceiveThread.ThreadState == ThreadState.Running)
                    ReceiveThread.Abort();
            }
            button11.Enabled = true;

            if (testThread != null)
            {
                if (testThread.ThreadState == ThreadState.Running)
                {
                    testThread.Abort();
                }
            }
            checkBox3.Checked = false;

            if (serialPort1.IsOpen == true)
            {
                try
                {
                    serialPort1.Close();
                    toolStripStatusLabel2.Text = sComPort + "已关闭！";
                    button1.Enabled = true;
                }
                catch
                {
                    return;
                }
            }
        }

        
        public void ShowMessage(byte[] ShowMsg)
        {
            if (textbox6num == XianShiNum)
            {
                textBox6.Clear();
                textbox6num = 0;
            }
            else
            {
                textbox6num++;
                textBox6.AppendText(System.DateTime.Now.ToString() + " 发送：" + byteToHexStr(ShowMsg) + "\n");
            }
            
            toolStripStatusLabel3.Text = "发包数：" + SendNum;
            toolStripStatusLabel7.Text = "发字节数：" + SendSizeNum;
        }

        private void ShowMessage2(byte[] ShowMsg)
        {
            if (textbox6num == XianShiNum)
            {
                textBox6.Clear();
                textbox6num = 0;
            }
            else
            {
                textbox6num++;
                textBox6.AppendText(System.DateTime.Now.ToString() + " 第" + iCurrentTestCount.ToString() + "次发送：" + byteToHexStr(ShowMsg) + "\n");
            }

        }

        public static string byteToHexStr(byte[] bytes)
        {
            string returnStr = "";
            if(bytes != null)
            {
                for(int i = 0;i<bytes.Length;i++)
                {
                    returnStr += bytes[i].ToString("X2");
                    returnStr += " ";
                }
            }
            return returnStr;
        }


        public static string byteToHexStr2(byte[] bytes,int len)
        {
            string returnStr = "";
            if (bytes != null)
            {
                for (int i = 0; i < len; i++)
                {
                    returnStr += bytes[i].ToString("X2");
                    returnStr += " ";
                }
            }
            return returnStr;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            textBox6.Clear();
        }

        //private void button8_Click(object sender, EventArgs e)
        //{
        //    iTestNum = Convert.ToInt32(textBox2.Text);
        //    iInternalTime = Convert.ToInt32(textBox1.Text);
        //    iCurrentTestCount = 0;
            
        //    timer1.Interval = iInternalTime * 1000;
        //    timer1.Start();
        //    Thread.Sleep(900);

        //    testThread = new Thread(new ThreadStart(testfunction));
        //    testThread.Start();

        //}

        private void testfunction()
        {
            SendCommand[0] = 0x55;
            SendCommand[1] = 0xaa;
            
            for (int i = 0; i < iTestNum; )
            {
                iCurrentTestCount++;

                SendCommand[2] = 0x00;
                //发送信息
                if (!PortSend(SendCommand))
                {
                    MessageBox.Show("发送错误", "提示");
                    return;
                }
                //ShowMessage(SendCommand);
                //textBox6.AppendText(System.DateTime.Now.ToString() + " 发送：" + byteToHexStr(SendCommand) + "\n");

                Thread.Sleep(iInternalTime * 1000);

                i++;
                if (i == iTestNum)
                {
                    flag1 = true;
                    return;
                }
                iCurrentTestCount++;
                SendCommand[2] = 0x01;
                //发送信息
                if (!PortSend(SendCommand))
                {
                    MessageBox.Show("发送错误", "提示");
                    return;
                }
                //ShowMessage(SendCommand);

                Thread.Sleep(iInternalTime * 1000);

                
                i++;
                if (i == iTestNum)
                {
                    flag1 = true;
                    return;
                }
                iCurrentTestCount++;
                SendCommand[2] = 0x02;
                //发送信息
                if (!PortSend(SendCommand))
                {
                    MessageBox.Show("发送错误", "提示");
                    return;
                }
               // ShowMessage(SendCommand);

                Thread.Sleep(iInternalTime * 1000);

                
                i++;
                if (i == iTestNum)
                {
                    flag1 = true;
                    return;
                }
                iCurrentTestCount++;
                SendCommand[2] = 0x03;
                //发送信息
                if (!PortSend(SendCommand))
                {
                    MessageBox.Show("发送错误", "提示");
                    return;
                }
                //ShowMessage(SendCommand);

                Thread.Sleep(iInternalTime * 1000);

                
                i++;
                if (i == iTestNum)
                {
                    flag1 = true;
                    return;
                }
                
            }


            //button8.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
           
            //if (flag1)
            //{
            //    flag1 = false;
            //    timer1.Stop();
            //}
            //else
            //{
            //    ShowMessage2(SendCommand);
            //    textBox3.Text = iCurrentTestCount.ToString();
            //}
            
        }


        public void testfunction2()
        {
            double time = SendTime / 1000.0;
            int i = 0, j = 0;
            pt = new HighPerformanceTimer();
            while (true)
            {
                if (flag485)
                {
                    i = i % index;
                    byte[] Sendcmd = StringToByte2(cmd[i]);
                    if (!PortSend(Sendcmd))
                    {
                        MessageBox.Show("发送错误", "提示");
                        return;
                    }
                    ShowMessage(Sendcmd);
                    i++;
                }
                pt.Delay(time);
            }

        }

       
        //private void button9_Click(object sender, EventArgs e)
        //{
        //    flag1 = true;
        //    testThread.Abort();
        //}

        private void button11_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen == false)
            {

                MessageBox.Show("串口未打开，请先打开串口", "提示");
                return;
                
            }

            if (textBox4.Text == "")
            {

                MessageBox.Show("发送内容为空，请写入发送帧", "提示");
                return;

            }

            if ((checkBox1.Checked == true) && (checkBox2.Checked == false))
            {
                int size = 0;
                byte[] tempbuffer = new byte[256];
                
                size = serialPort1.BytesToRead;
                serialPort1.Read(tempbuffer, 0, size);//第一次发命令前，先把串口缓冲区读空


                int time = Convert.ToInt32(textBox7.Text);
                timer3.Interval = time;
                button12.Visible = true;

                timer3.Start();

                button11.Enabled = false;
            }
            else if ((checkBox1.Checked == true) && (checkBox2.Checked == true))
            {
                int size = 0;
                byte[] tempbuffer = new byte[256];

                size = serialPort1.BytesToRead;
                serialPort1.Read(tempbuffer, 0, size);//第一次发命令前，先把串口缓冲区读空

                button12.Visible = true;
                button11.Enabled = false;

                timerThread = new Thread(new ThreadStart(timerProcess));
                Control.CheckForIllegalCrossThreadCalls = false; //加上次语句的功能在线程中操作控件
                timerThread.Start();
            }
            else
            {
                if (flag485)
                {
                    int size = 0;
                    byte[] tempbuffer = new byte[256];

                    size = serialPort1.BytesToRead;
                    serialPort1.Read(tempbuffer, 0, size);//第一次发命令前，先把串口缓冲区读空

                    

                    byte[] Sendcmd = StringToByte2(textBox4.Text);
                    if (!PortSend(Sendcmd))
                    {
                        MessageBox.Show("发送错误", "提示");
                        return;
                    }
                    ShowMessage(Sendcmd);
                }
            }

            
        }

        public static byte[] StringToByte(string InString)
        {
            
            string[] ByteStrings = new string[InString.Length / 2];
            int TmpIndex;
            for (int j = 0; j < InString.Length / 2; j++)
            {
                ByteStrings[j] = InString.Substring(j * 2, 2);
            }
            byte[] ByteOut;
            ByteOut = new byte[ByteStrings.Length];
            for (int i = 0; i <= ByteStrings.Length - 1; i++)
            {
                TmpIndex = ByteStrings[i].ToLower().IndexOf("0x");
                if (TmpIndex >= 0)
                {
                    ByteOut[i] = byte.Parse(ByteStrings[i].Remove(TmpIndex, 2), System.Globalization.NumberStyles.HexNumber);
                }
                else
                {
                    ByteOut[i] = byte.Parse(ByteStrings[i], System.Globalization.NumberStyles.HexNumber);
                }
            }
            return ByteOut;
        }

        public byte[] StringToByte2(string InString)
        {
            
            string[] ByteStrings = InString.Split(' ');
            byte[] ByteOut = new byte[ByteStrings.Length];
            int TmpIndex;
            

            //string[] ByteStrings = new string[InString.Length / 2];
            //int TmpIndex;
            //for (int j = 0; j < InString.Length / 2; j++)
            //{
            //    ByteStrings[j] = InString.Substring(j * 2, 2);
            //}
            //byte[] ByteOut;
            //ByteOut = new byte[ByteStrings.Length];

            try
            {
                for (int i = 0; i <= ByteStrings.Length - 1; i++)
                {
                    TmpIndex = ByteStrings[i].ToLower().IndexOf("0x");

                    if (TmpIndex >= 0)
                    {
                        ByteOut[i] = byte.Parse(ByteStrings[i].Remove(TmpIndex, 2), System.Globalization.NumberStyles.HexNumber);
                    }
                    else
                    {
                        ByteOut[i] = byte.Parse(ByteStrings[i], System.Globalization.NumberStyles.HexNumber);
                    }
                }
            }
            catch
            {
                MessageBox.Show("请检查命令帧格式，格式有误！", "提示");
                return ByteOut;
            }

            
            return ByteOut;
        }

        private void button11_Click_1(object sender, EventArgs e)
        {
            byte[] Sendcmd = StringToByte(textBox4.Text);

        }

        private void timer3_Tick(object sender, EventArgs e)
        {
            if (flag485)
            {
                byte[] Sendcmd = StringToByte2(textBox4.Text);
                if (!PortSend(Sendcmd))
                {
                    MessageBox.Show("发送错误", "提示");
                    return;
                }
                ShowMessage(Sendcmd);
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            if (timerThread != null)
            {
                if (timerThread.ThreadState == ThreadState.Running)
                {
                    timerThread.Abort();
                }
            }
            timer3.Stop();
            button11.Enabled = true;
        }

        private void button13_Click(object sender, EventArgs e)
        {
            textBox5.Clear();
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {

                button12.Visible = true;

            }
            else
            {
                button12.Visible = false;
            }
        }



        private void button15_Click(object sender, EventArgs e)
        {
            SendNum = 0;
            ReceiveNum = 0;
            LostNum = 0;
            WuMaLv = 0;

            ReturnBytes = 0;
            SendSizeNum = 0;
            ReceiveSizeNum = 0;
            LostSizeNum = 0;
            SizeWuMaLv = 0;

            toolStripStatusLabel3.Text = "发包数：" + SendNum;
            toolStripStatusLabel4.Text = "收包数：" + ReceiveNum;
            toolStripStatusLabel5.Text = "丢包数：" + LostNum;
            toolStripStatusLabel6.Text = "丢包率：" + WuMaLv;

            toolStripStatusLabel11.Text = "应收字节数" + ReturnBytes;
            toolStripStatusLabel7.Text = "发字节数：" + SendSizeNum;
            toolStripStatusLabel8.Text = "实收字节数：" + ReceiveSizeNum;
            toolStripStatusLabel9.Text = "丢字节数：" + LostSizeNum;
            toolStripStatusLabel10.Text = "丢字节率：" + SizeWuMaLv;
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (testThread != null)
            {
                if (testThread.ThreadState == ThreadState.Running)
                    testThread.Abort();
            }

            if (ReceiveThread != null)
            {
                if (ReceiveThread.ThreadState == ThreadState.Running)
                    ReceiveThread.Abort();
            }

            serialPort1.Close();
        }

        private void textBox7_TextChanged(object sender, EventArgs e)
        {

        }

        private void label8_Click(object sender, EventArgs e)
        {

        }

        private void textBox8_TextChanged(object sender, EventArgs e)
        {
            EachReturnBytes = Convert.ToInt32(textBox8.Text);
        }

        private void button10_Click(object sender, EventArgs e)
        {

            timerThread = new Thread(new ThreadStart(timerProcess));
            Control.CheckForIllegalCrossThreadCalls = false; //加上次语句的功能在线程中操作控件
            timerThread.Start();
        }

        private void button14_Click(object sender, EventArgs e)
        {
            if (timerThread != null)
            {
                if (timerThread.ThreadState == ThreadState.Running)
                {
                    timerThread.Abort();
                }
            }
        }

        private void timerProcess()
        {
            double duration = Convert.ToDouble(textBox9.Text);
            //double time=0;
            pt = new HighPerformanceTimer();

            while (true)
            {

                if (flag485)
                {
                    byte[] Sendcmd = StringToByte2(textBox4.Text);
                    if (!PortSend(Sendcmd))
                    {
                        MessageBox.Show("发送错误", "提示");
                        return;
                    }
                    ShowMessage(Sendcmd);
                }

                //ShowMessage3();
                //textBox6.AppendText(System.DateTime.Now.ToString() + "\n");
                //pt.Start();
                pt.Delay(duration);
                //pt.Stop();
                //time = pt.Duration;
            }
        }

        private void ShowMessage3()
        {
            if (textbox6num == XianShiNum)
            {
                textBox6.Clear();
                textbox6num = 0;
            }
            else
            {
                textbox6num++;
                textBox6.AppendText(System.DateTime.Now.ToString() + "\n");
            }

            

        }

        private void timer4_Tick(object sender, EventArgs e)
        {
            if (label14.Right < 10)
            {
                //不更新 (滚动一次或设定次数后不更新)
            }
            label14.Left -= 2;
            if (label14.Right < 10)
            {
                label14.Left = this.Width;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Form2 frm2 = new Form2(this);
            frm2.Show();
        }

        private void button18_Click(object sender, EventArgs e)
        {
            //string[] cmd = new string[10];
            //cmd[0] = textBox14.Text;
            //byte[] Sendcmd2 = StringToByte2(cmd[0]);

            if (serialPort1.IsOpen == false)
            {
                MessageBox.Show("串口未打开，请先打开串口", "提示");
                return;
            }

            int size = 0;
            byte[] tempbuffer = new byte[256];

            size = serialPort1.BytesToRead;
            serialPort1.Read(tempbuffer, 0, size);//第一次发命令前，先把串口缓冲区读空

            

            byte[] Sendcmd = StringToByte2(textBox14.Text);
            if (!PortSend(Sendcmd))
            {
                MessageBox.Show("发送错误", "提示");
                return;
            }

            ShowMessage(Sendcmd);
        }

        private void button17_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen == false)
            {

                MessageBox.Show("串口未打开，请先打开串口", "提示");
                return;

            }

            int size = 0;
            byte[] tempbuffer = new byte[256];


            size = serialPort1.BytesToRead;
            serialPort1.Read(tempbuffer, 0, size);//第一次发命令前，先把串口缓冲区读空


            byte[] Sendcmd = StringToByte2(textBox13.Text);
            if (!PortSend(Sendcmd))
            {
                MessageBox.Show("发送错误", "提示");
                return;
            }
            ShowMessage(Sendcmd);
        }

        private void button16_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen == false)
            {

                MessageBox.Show("串口未打开，请先打开串口", "提示");
                return;

            }

            int size = 0;
            byte[] tempbuffer = new byte[256];


            size = serialPort1.BytesToRead;
            serialPort1.Read(tempbuffer, 0, size);//第一次发命令前，先把串口缓冲区读空


            byte[] Sendcmd = StringToByte2(textBox12.Text);
            if (!PortSend(Sendcmd))
            {
                MessageBox.Show("发送错误", "提示");
                return;
            }
            ShowMessage(Sendcmd);
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen == false)
            {

                MessageBox.Show("串口未打开，请先打开串口", "提示");
                return;

            }

            int size = 0;
            byte[] tempbuffer = new byte[256];


            size = serialPort1.BytesToRead;
            serialPort1.Read(tempbuffer, 0, size);//第一次发命令前，先把串口缓冲区读空


            byte[] Sendcmd = StringToByte2(textBox11.Text);
            if (!PortSend(Sendcmd))
            {
                MessageBox.Show("发送错误", "提示");
                return;
            }
            ShowMessage(Sendcmd);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen == false)
            {

                MessageBox.Show("串口未打开，请先打开串口", "提示");
                return;

            }

            int size = 0;
            byte[] tempbuffer = new byte[256];


            size = serialPort1.BytesToRead;
            serialPort1.Read(tempbuffer, 0, size);//第一次发命令前，先把串口缓冲区读空


            byte[] Sendcmd = StringToByte2(textBox10.Text);
            if (!PortSend(Sendcmd))
            {
                MessageBox.Show("发送错误", "提示");
                return;
            }
            ShowMessage(Sendcmd);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen == false)
            {

                MessageBox.Show("串口未打开，请先打开串口", "提示");
                return;

            }

            int size = 0;
            byte[] tempbuffer = new byte[256];


            size = serialPort1.BytesToRead;
            serialPort1.Read(tempbuffer, 0, size);//第一次发命令前，先把串口缓冲区读空


            byte[] Sendcmd = StringToByte2(textBox3.Text);
            if (!PortSend(Sendcmd))
            {
                MessageBox.Show("发送错误", "提示");
                return;
            }
            ShowMessage(Sendcmd);
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen == false)
            {

                MessageBox.Show("串口未打开，请先打开串口", "提示");
                return;

            }

            int size = 0;
            byte[] tempbuffer = new byte[256];


            size = serialPort1.BytesToRead;
            serialPort1.Read(tempbuffer, 0, size);//第一次发命令前，先把串口缓冲区读空


            byte[] Sendcmd = StringToByte2(textBox2.Text);
            if (!PortSend(Sendcmd))
            {
                MessageBox.Show("发送错误", "提示");
                return;
            }
            ShowMessage(Sendcmd);
        }

        private void button8_Click(object sender, EventArgs e)
        {
            if (serialPort1.IsOpen == false)
            {

                MessageBox.Show("串口未打开，请先打开串口", "提示");
                return;

            }

            int size = 0;
            byte[] tempbuffer = new byte[256];

            size = serialPort1.BytesToRead;
            serialPort1.Read(tempbuffer, 0, size);//第一次发命令前，先把串口缓冲区读空

            byte[] Sendcmd = StringToByte2(textBox1.Text);
            if (!PortSend(Sendcmd))
            {
                MessageBox.Show("发送错误", "提示");
                return;
            }
            ShowMessage(Sendcmd);
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Checked == true)
            {
                if (serialPort1.IsOpen == false)
                {
                    checkBox3.Checked = false;
                    MessageBox.Show("串口未打开，请先打开串口", "提示");
                    return;

                }

                index = 0;
                if (checkBox4.Checked == true && textBox14.Text != "")
                {
                    cmd[index] = textBox14.Text;
                    index++;
                }
                if (checkBox5.Checked == true && textBox13.Text != "")
                {
                    cmd[index] = textBox13.Text;
                    index++;
                }
                if (checkBox6.Checked == true && textBox12.Text != "")
                {
                    cmd[index] = textBox12.Text;
                    index++;
                }
                if (checkBox7.Checked == true && textBox11.Text != "")
                {
                    cmd[index] = textBox11.Text;
                    index++;
                }
                if (checkBox8.Checked == true && textBox10.Text != "")
                {
                    cmd[index] = textBox10.Text;
                    index++;
                }
                if (checkBox9.Checked == true && textBox3.Text != "")
                {
                    cmd[index] = textBox3.Text;
                    index++;
                }
                if (checkBox10.Checked == true && textBox2.Text != "")
                {
                    cmd[index] = textBox2.Text;
                    index++;
                }
                if (checkBox11.Checked == true && textBox1.Text != "")
                {
                    cmd[index] = textBox1.Text;
                    index++;
                }

                if (index == 0)
                {
                    checkBox3.Checked = false;
                    MessageBox.Show("请选择要发送的命令帧", "提示");
                    return;

                }

                SendTime = Convert.ToInt32(textBox15.Text);           
                testThread = new Thread(new ThreadStart(testfunction2));
                Control.CheckForIllegalCrossThreadCalls = false; //加上次语句的功能在线程中操作控件
                testThread.Start();
            }
            else
            {

                //button19.Text = "暂停";
                //button19.BackColor = Color.Lime;

                if (testThread != null)
                {
                    //if (testThread.ThreadState == ThreadState.Running || testThread.ThreadState == ThreadState.Suspended)
                    if (testThread.ThreadState == ThreadState.Running)
                    {

                        testThread.Abort();
                    }
                }
            }

        }

        private void button19_Click(object sender, EventArgs e)
        {
            if (button19.Text == "暂停")
            {
                if (testThread != null)
                {
                    if (testThread.ThreadState == ThreadState.Running)
                    {
                        testThread.Suspend();
                        button19.Text = "继续";
                        button19.BackColor = Color.Red;
                    }
                }
            }
            else
            {
                if (testThread != null)
                {
                    if (testThread.ThreadState == ThreadState.Suspended)
                    {
                        testThread.Resume();
                        button19.Text = "暂停";
                        button19.BackColor = Color.Lime;
                    }
                }
            }

            
        }

        private void button20_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "文本文件(.txt)|*.txt";
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                StreamWriter sw = new StreamWriter(saveFileDialog1.FileName, true);
                sw.WriteLine("发送窗口：");
                sw.WriteLine(textBox6.Text);

                sw.WriteLine("接收窗口：");
                sw.WriteLine(textBox5.Text);

                sw.Close();
            }
        }



    }
}
