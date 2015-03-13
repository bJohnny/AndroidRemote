using System;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace client
{
    class Program
    {
        
        const int NumberOfThreads = 1;
        void Work(object obj)
        {
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse("141.28.133.128"), 9050);
            TcpClient client = new TcpClient();
            client.Connect(ep);

            using (NetworkStream stream = client.GetStream())
            {
                for (int i = 0; i < 10000; i++)
                {
                    StringBuilder sb = new StringBuilder();
                    string request = "Message No. " + i + ";";
                    Console.WriteLine("sent: " + request);
                    stream.Write(Encoding.ASCII.GetBytes(request), 0, request.Length);
                    stream.Flush();
                    Thread.Sleep(1000);
                    /*
                    int i;
                    while ((i = stream.ReadByte()) != 0)
                    {
                        sb.Append((char)i);
                    }
                        */
                }
            }
            //client.Close();

            Console.WriteLine("Done");

        }

        void start()
        {
            for (int i = 0; i < NumberOfThreads; i++)
            {
                ThreadPool.QueueUserWorkItem(Work);
            }
        }

        static void Main(string[] args)
        {
            Program p = new Program();
            p.start();

            //press any key to exit
            Console.ReadKey();
            Environment.Exit(0);
        }
    }
}