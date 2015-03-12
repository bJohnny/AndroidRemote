using System;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace client
{
    class Program
    {
        
        const int NumberOfThreads = 10;
        void Work(object obj)
        {
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse("192.168.2.120"), 9050);
            TcpClient client = new TcpClient();
            client.Connect(ep);

            StringBuilder sb = new StringBuilder();
            using (NetworkStream stream = client.GetStream())
            {
                string request = "Test" + '\0';
                Console.WriteLine("sent: " + request);
                stream.Write(Encoding.ASCII.GetBytes(request), 0, request.Length);

                int i;
                while ((i = stream.ReadByte()) != 0)
                {
                    sb.Append((char)i);
                }
            }
            client.Close();

            Console.WriteLine(sb.ToString());
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