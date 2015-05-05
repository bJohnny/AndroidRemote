using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace MotionDetector
{
    class Klient
    {
        private TcpClient _tcpClient;

        private IPEndPoint _serverEndPoint;

        public Klient(IPEndPoint serverEndPoint)
        {
            _serverEndPoint = serverEndPoint;
        }

        public TcpClient TcpClient
        {
            get { return _tcpClient; }
            set { _tcpClient = value; }
        }
    }
}