﻿//
/*   Server Program    */

using System;
using System.Text;
using System.Net;
using System.Net.Sockets;

public class serv
{
    
    //Reads local IPv4 as String
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

    public static void Main()
    {
        Console.WriteLine(IpList());
        try
        {
            IPAddress ipAd = IPAddress.Parse(IpList());
            // use local m/c IP address, and 
            // use the same in the client

            /* Initializes the Listener */
            TcpListener myList = new TcpListener(ipAd, 8001);

            /* Start Listeneting at the specified port */
            myList.Start();

            Console.WriteLine("The server is running at port 8001...");
            Console.WriteLine("The local End point is  :" +
                              myList.LocalEndpoint);
            Console.WriteLine("Waiting for a connection.....");

            Socket s = myList.AcceptSocket();
            Console.WriteLine("Connection accepted from " + s.RemoteEndPoint);

            byte[] b = new byte[100];
            int k = s.Receive(b);
            Console.WriteLine("Recieved...");
            for (int i = 0; i < k; i++)
                Console.Write(Convert.ToChar(b[i]));

            ASCIIEncoding asen = new ASCIIEncoding();
            s.Send(asen.GetBytes("The string was recieved by the server."));
            Console.WriteLine("\nSent Acknowledgement");
            Console.ReadLine();
            /* clean up */
            s.Close();
            myList.Stop();

        }
        catch (Exception e)
        {
            Console.WriteLine("Error..... " + e.StackTrace);
        }
    }

}