using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Runtime.InteropServices;  //使用DllImport
using System.ComponentModel;   //使用throw new Win32Exception();
using System.Threading;

namespace 华春板卡继电器测试
{
    class HighPerformanceTimer
    {
        [DllImport("Kernel32.dll")]
        private static extern bool QueryPerformanceCounter(out long lpPerformanceCounter);

        [DllImport("Kernel32.dll")]
        private static extern bool QueryPerformanceFrequency(out long lpFrequency);

        private long startTime, stopTime;
        private long freq;

        public HighPerformanceTimer()
        {
            stopTime = 0;
            startTime = 0;

            if (QueryPerformanceFrequency(out freq) == false)
            {
                throw new Win32Exception();
            }
        }


        public void Start()
        {
            Thread.Sleep(0);
            QueryPerformanceCounter(out startTime);

        }

        public void Stop()
        {
            QueryPerformanceCounter(out stopTime);
        }

        public double Duration
        {
            get
            {
                return (double)(stopTime - startTime)/(double)freq;

            }
        }

        public double Delay(double delaytime)  //delaytime延时时间，单位秒
        {
            //LARGE_INTEGER startCount;
            //LARGE_INTEGER endCount;
            //LARGE_INTEGER FrqCount;

            //double Elapsed = 0.0;
            //QueryPerformanceFrequency(&FrqCount);
            //QueryPerformanceCounter(&startCount);
            //while (Elapsed < dTime)
            //{
            //    QueryPerformanceCounter(&endCount);
            //    Elapsed = (double)(endCount.QuadPart - startCount.QuadPart) / FrqCount.QuadPart;
            //}
            //return Elapsed;


            long starttime = 0;
            long stoptime = 0;

            double Elapsed = 0;
            QueryPerformanceCounter(out starttime);
            while (Elapsed < delaytime)
            {
                QueryPerformanceCounter(out stoptime);
                Elapsed = (double)(stoptime - starttime) / (double)freq;

            }
            return Elapsed;

        }

    }
}
