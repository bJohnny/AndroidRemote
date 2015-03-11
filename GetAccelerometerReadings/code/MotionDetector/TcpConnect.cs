using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

using System.Net;
using System.Net.Sockets;
using System.IO;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace MotionDetector
{
    class TcpConnect
    {

        public void TcpTest()
        {
            try
            {
                var tcpclnt = new TcpClient();
                //Console.WriteLine("Connecting.....");

                tcpclnt.Connect("141.28.133.128", 8001);
                // use the ipaddress as in the server program

                //Console.WriteLine("Connected");
                //Console.Write("Enter the string to be transmitted : ");

                String str = "Test";//Console.ReadLine();
                Stream stm = tcpclnt.GetStream();

                var asen = new ASCIIEncoding();
                byte[] ba = asen.GetBytes(str);
                //Console.WriteLine("Transmitting.....");

                stm.Write(ba, 0, ba.Length);

                var bb = new byte[100];
                int k = stm.Read(bb, 0, 100);

                for (int i = 0; i < k; i++)
                    Console.Write(Convert.ToChar(bb[i]));

                tcpclnt.Close();
            }

            catch (Exception v)
            {
                Console.WriteLine("Error..... " + v.StackTrace);
            }
        }

    }
}