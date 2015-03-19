using System;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using Android.App;
using Android.Content.PM;
using Android.Hardware;
using Android.OS;
using Android.Views;
using Android.Widget;


namespace MotionDetector
{
    [Activity(Label = "MotionDetector", MainLauncher = true, Icon = "@drawable/icon", ScreenOrientation = ScreenOrientation.Landscape)]
    public class Activity1 : Activity, ISensorEventListener
    {
        private static readonly object SyncLock = new object();
        private SensorManager _sensorManager;
        private TextView _sensorTextView;
        private TextView _testView;

        public static string MessageString = "";

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);
            _sensorManager = (SensorManager) GetSystemService(SensorService);
            _sensorTextView = FindViewById<TextView>(Resource.Id.accelerometer_text);
            _testView = FindViewById<TextView>(Resource.Id.textView1);

            Window.SetFlags(WindowManagerFlags.KeepScreenOn, WindowManagerFlags.KeepScreenOn);
            
            var tcpClient = new Thread(StartClient) {IsBackground = true};
            tcpClient.Start(this);
        }

        public static void StartClient()
        {
            var p = new Client();
            p.Start();
        }

        protected override void OnResume()
        {
            base.OnResume();
            _sensorManager.RegisterListener(this, _sensorManager.GetDefaultSensor(SensorType.Accelerometer),
               SensorDelay.Ui);

            //Register a the Gyroscope

            //_sensorManager.RegisterListener(this, _sensorManager.GetDefaultSensor(SensorType.Gyroscope), SensorDelay.Ui);
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


            if (e.Sensor.Type == SensorType.Accelerometer)
            {
                var calibrationValue = SensorManager.StandardGravity;
                lock (SyncLock)
                {
                    var text = new StringBuilder("Sensor: Accelerometer. \n Beschleunigung entlang der Achsen")
                        .Append("\nx : ")
                        .Append(Math.Round(e.Values[0], 2))
                        .Append(" Lateral")
                        .Append("\ny : ")
                        .Append(Math.Round(e.Values[1], 2))
                        .Append(" Longitudinal")
                        .Append("\nz : ")
                        .Append(Math.Round(e.Values[2], 2))
                        .Append(" Vertikal (mit Erdanziehung)")
                        .Append("\nAccuracy : ")
                        .Append(e.Accuracy);
                    _sensorTextView.Text = text.ToString();
                    MessageString = text.ToString();

                    // Calculate total acceleration w positive values and eliminate gravity
                    var SumOfSq = Math.Pow(e.Values[0], 2) + Math.Pow(e.Values[1], 2) + Math.Pow(e.Values[2], 2);
                    var mach = Math.Pow(SumOfSq, .5) - calibrationValue;
                    RunOnUiThread(() => 
                        _testView.Text = "Beschleunigung gesamt: " + mach.ToString());
                } 
            }

            //if (e.Sensor.Type == SensorType.Gyroscope)
            //{
            //    lock (SyncLock)
            //    {
            //        var text = new StringBuilder("Sensor: Gyorscope. \n Orientierung in Grad entlang der Achsen")
            //                        .Append("\nx : ")
            //                        .Append(e.Values[0])
            //                        .Append("\ny : ")
            //                        .Append(e.Values[1])
            //                        .Append("\nz : ")
            //                        .Append(e.Values[2])
            //                        .Append("\nAccuracy : ")
            //                        .Append(e.Accuracy);
            //        _sensorTextView.Text = text.ToString();
            //        MessageString = text.ToString(); 
            //    }
            //}

            

            //var test = new StringBuilder("so schreibe ich etwas auf den Screen");
            //_testView.Text = test.ToString();
         }

        public void OnAccuracyChanged(Sensor sensor, int accuracy)
        {
            if (sensor.Type == Android.Hardware.SensorType.Accelerometer)
            {
                if (Android.Hardware.SensorStatus.AccuracyHigh ==
         (Android.Hardware.SensorStatus)accuracy)
                {

                }
            }
        }

        //public override void OnBackPressed()
        //{
            
        //    Client.client.Close();

        //}

    }
}