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
        private static string element = "";
        private static bool isConnected = false;

        public static string getElement()
        {
            return element;
        }

        public static bool getConnected()
        {
            return isConnected;
        }

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
            isConnected = true;
            //client.Connect(ep);

            NetworkStream clientStream = client.GetStream();


            if (clientStream.CanRead)
            {
              //  StringBuilder RecvMessage;
                int recv;
                byte[] data = new byte[1024];
                
                while (true)
                {
                    Log.Info("HA", "Reading Stream");
                    recv = clientStream.Read(data, 0, data.Length);
                    //TODO: if client disconnects --> IOExeption, fix it (maybe client.Close() in the Android App!
                    element = Encoding.ASCII.GetString(data, 0, recv);
                    Log.Info("HA", element);
                    if (!element.Equals("")) break;
                    
                }
                
                    

            }


            if (clientStream.CanWrite)
            {
                Log.Info("HA", "Can write in Stream");
                

                while (true)
                {
                    if (!string.IsNullOrEmpty((Activity1.MessageString)))
                    {
                        Log.Info("HA", Activity1.MessageString);
                        var massage = Activity1.MessageString + ";";
                        Byte[] sendBytes = Encoding.ASCII.GetBytes(massage);
                        //clientStream.Read(sendBytes, 0, sendBytes.Length);
                        clientStream.Write(sendBytes, 0, sendBytes.Length);
                        clientStream.Flush();
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