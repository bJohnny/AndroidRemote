using System;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace server
{
    public class server
    {
        ManualResetEvent tcpClientConnected = new ManualResetEvent(false);

        void ProcessIncomingData(object obj)
        {
            TcpClient client = (TcpClient)obj;
            StringBuilder sb = new StringBuilder();

            using (NetworkStream stream = client.GetStream())
            {
                int i;
                while ((i = stream.ReadByte()) != 0)
                {
                    sb.Append((char)i);
                }

                string reply = "ack: " + sb.ToString() + '\0';
                stream.Write(Encoding.ASCII.GetBytes(reply), 0, reply.Length);
            }
            Console.WriteLine(sb.ToString());
        }

        void ProcessIncomingConnection(IAsyncResult ar)
        {
            TcpListener listener = (TcpListener)ar.AsyncState;
            TcpClient client = listener.EndAcceptTcpClient(ar);

            //Brings Thread to Backgrounhd Thread - listen to other Clients
            ThreadPool.QueueUserWorkItem(ProcessIncomingData, client);
            tcpClientConnected.Set();
        }


        public static string IpList()
        {
            IPHostEntry Host = Dns.GetHostEntry(Dns.GetHostName());

            foreach (IPAddress ip in Host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }

            return string.Empty;
        }
        
        
        public void start()
        {
            IPEndPoint endpoint = new IPEndPoint(IPAddress.Parse(IpList()), 5000);
            TcpListener listener = new TcpListener(endpoint);
            listener.Start();

            Console.WriteLine("The local End point is  :" +
                              listener.LocalEndpoint);

            while (true)
            {
                tcpClientConnected.Reset();
                listener.BeginAcceptTcpClient(new AsyncCallback(ProcessIncomingConnection), listener);
                tcpClientConnected.WaitOne();
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            server s = new server();
            s.start();
        }
    }
}