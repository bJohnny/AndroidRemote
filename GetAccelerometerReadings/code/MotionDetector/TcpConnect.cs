using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace MotionDetector
{
    public class Client
    {
        public TcpClient client;

        void Work(object obj)
        {
            IPEndPoint ep = new IPEndPoint(IPAddress.Parse("141.28.135.108"), 9050);
            client = new TcpClient();
            client.Connect(ep);

            using (NetworkStream stream = client.GetStream())
            {
                while(true)
                {
                    if (!string.IsNullOrEmpty(Activity1.MessageString))
                    {
                        string request = Activity1.MessageString + ";";
                    
                        stream.Write(Encoding.ASCII.GetBytes(request), 0, request.Length);
                        //stream.Flush();
                        Thread.Sleep(200);
                    }
                    else
                    {
                        string s = "EMPTY";
                    }
                }
            }
            client.Close(); //TODO: close OnBackPressed(), at the moment you never get here
            Console.WriteLine("Done");

        }
        
        public void Start()
        {
                ThreadPool.QueueUserWorkItem(Work);           
        }
    }

}