using System;
using System.Text;

using System.Net;
using System.Net.Sockets;
using System.IO;

using Android.App;
using Android.Content;
using Android.Hardware;
using Android.OS;
using Android.Widget;

namespace MotionDetector
{
    [Activity(Label = "MotionDetector", MainLauncher = true, Icon = "@drawable/icon")]
    public class Activity1 : Activity, ISensorEventListener
    {
        private static readonly object _syncLock = new object();
        private SensorManager _sensorManager;
        private TextView _sensorTextView;
        private TextView _testView;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);
            _sensorManager = (SensorManager) GetSystemService(SensorService);
            _sensorTextView = FindViewById<TextView>(Resource.Id.accelerometer_text);
            _testView = FindViewById<TextView>(Resource.Id.textView1);
        }

        protected override void OnResume()
        {
            base.OnResume();
            _sensorManager.RegisterListener(this, _sensorManager.GetDefaultSensor(SensorType.Accelerometer),
                SensorDelay.Ui);

            TcpTest();
        }

        protected override void OnPause()
        {
            base.OnPause();
            _sensorManager.UnregisterListener(this);
        }

        public void OnAccuracyChanged(Sensor sensor, SensorStatus accuracy)
        {
            // We don't want to do anything here.
        }

        public void OnSensorChanged(SensorEvent e)
        {
            lock (_syncLock)
            {
                var text = new StringBuilder("x = ")
                    .Append(e.Values[0])
                    .Append(", y=")
                    .Append(e.Values[1])
                    .Append(", z=")
                    .Append(e.Values[2]);
                _sensorTextView.Text = text.ToString();
            }

            var test = new StringBuilder("so schreibe ich etwas auf den Screen");
            _testView.Text = test.ToString();
         }

        public void TcpTest()
        {
            try
            {
                var tcpclnt = new TcpClient();
                //Console.WriteLine("Connecting.....");

                tcpclnt.Connect("192.168.2.120", 9050);
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