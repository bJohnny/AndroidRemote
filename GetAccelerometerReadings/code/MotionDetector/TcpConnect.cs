using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace MotionDetector
{
    public class Client
    {
        //const int NumberOfThreads = 1;
        void Work(object obj)
        {
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse("141.28.133.128"), 9050);
            TcpClient client = new TcpClient();
            client.Connect(ep);

            using (NetworkStream stream = client.GetStream())
            {
                //TODO soll nur dann senden solange etwas in der Message steht. Keine neue Verbindung bei SensorChange
                /** /
                for (int i = 0; i < 10000; i++)
                {
                    StringBuilder sb = new StringBuilder();
                    
                    string request = Activity1.messageString + ";";
                    
                    stream.Write(Encoding.ASCII.GetBytes(request), 0, request.Length);
                    //stream.Flush();
                    Thread.Sleep(1000);                    
                }
                /**/
                while(Activity1.MessageString != String.Empty)
                {
                    StringBuilder sb = new StringBuilder();
                    
                    string request = Activity1.MessageString + ";";
                    
                    stream.Write(Encoding.ASCII.GetBytes(request), 0, request.Length);
                    //stream.Flush();
                    Thread.Sleep(1000);
                    
                }
            }
            //client.Close();

            Console.WriteLine("Done");

        }

        public void start()
        {
                ThreadPool.QueueUserWorkItem(Work);           
        }

        /*static void Main(string[] args)
        {
            Client p = new Client();
            p.start();

            //press any key to exit
            //Console.ReadKey();
            //Environment.Exit(0);
        }*/
    }
}