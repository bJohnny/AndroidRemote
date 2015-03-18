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
            lock (SyncLock)
            {
                var text = new StringBuilder("x = ")
                    .Append(e.Values[0])
                    .Append(", y=")
                    .Append(e.Values[1])
                    .Append(", z=")
                    .Append(e.Values[2]);
                _sensorTextView.Text = text.ToString();
                MessageString = text.ToString();
            }

            var test = new StringBuilder("so schreibe ich etwas auf den Screen");
            _testView.Text = test.ToString();
         }

        //public override void OnBackPressed()
        //{
            
        //    Client.client.Close();

        //}

    }
}