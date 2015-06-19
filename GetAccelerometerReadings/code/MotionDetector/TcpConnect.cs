using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using Android.Provider;
using Android.Util;

namespace MotionDetector
{
    public class Client
    {
        public TcpClient client;
        private IPAddress address;

        public static IPEndPoint IpEp { get; set; }

        public Client()
        {
            
        }

        public Client(IPAddress address)
        {
            this.address = address;
        }
        

        void Work(object obj)
        {
            //IPEndPoint ep = new IPEndPoint(address, 3000);
            Log.Info("HA", "Connecting... !!!");
            client = new TcpClient(address.ToString(), 3000);
            Log.Info("HA", "Connected !!!");

            //client.Connect(ep);

            NetworkStream clientStream = client.GetStream();

            if (clientStream.CanWrite)
            {
                Log.Info("HA", "we are using the stream");
                

                while (true)
                {
                    if (!string.IsNullOrEmpty((Activity1.MessageString)))
                    {
                        Log.Info("HA", Activity1.MessageString);
                        var massage = Activity1.MessageString + ";";
                        Byte[] sendBytes = Encoding.ASCII.GetBytes(massage);
                        clientStream.Write(sendBytes, 0, sendBytes.Length);
                        //Thread.Sleep(2000);
                    }
                }
                

            }
            else
            {
                Log.Warn("HA", "You cannot write in this stream!");
                client.Close();
                clientStream.Close();
                return;
            }

            //TODO Fix the delimiter and get back to this bit
            //using (NetworkStream stream = client.GetStream())
            //{
            //    while(true)
            //    {
            //        if (!string.IsNullOrEmpty(Activity1.MessageString))
            //        {
            //            //Log.Info("HA", "I have a dream");
            //            string request = Activity1.MessageString + ";";
                    
            //            stream.Write(Encoding.ASCII.GetBytes(request), 0, request.Length);
            //            //stream.Flush();
            //            Thread.Sleep(200);
            //        }
            //        else
            //        {
            //            string s = "EMPTY";
            //        }
            //    }
            //}

            

            
            

            
            //client.Close(); //TODO: close OnBackPressed(), at the moment you never get here
            

        }

        public void Close()
        {
            client.Close();
        }

        public bool Connected()
        {
            return client.Connected;
        }
        
        public void Start()
        {
                ThreadPool.QueueUserWorkItem(Work);           
        }
    }

}