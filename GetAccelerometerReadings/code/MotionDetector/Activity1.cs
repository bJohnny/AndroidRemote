﻿using System;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.Hardware;
using Android.Locations;
using Android.OS;
using Android.Util;
using Android.Views;
using Android.Widget;
using Java.IO;
using Org.Json;


namespace MotionDetector
{
    [Activity(Label = "MotionDetector", MainLauncher = true, Icon = "@drawable/icon", ScreenOrientation = ScreenOrientation.Landscape)]
    public class Activity1 : Activity, ISensorEventListener
    {
        private static readonly object SyncLock = new object();
        private SensorManager _sensorManager;
        private TextView _sensorTextView;
        private TextView _testView;
        private ImageView _picture;
        private FrameLayout _frame;
        private Button _connectButton;
        private EditText _inputEditText;
        private IPAddress _address;
        private Client client;

        public static string MessageString = String.Empty;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.Main);
            _sensorManager = (SensorManager) GetSystemService(SensorService);
            _sensorTextView = FindViewById<TextView>(Resource.Id.accelerometer_text);
            _testView = FindViewById<TextView>(Resource.Id.textView1);
            _inputEditText = FindViewById<EditText>(Resource.Id.IPEndPoint);
         
            _picture = FindViewById<ImageView>(Resource.Id.imageView);
            _frame = FindViewById<FrameLayout>(Resource.Id.frameView);

            Window.SetFlags(WindowManagerFlags.KeepScreenOn, WindowManagerFlags.KeepScreenOn);
            
            

            _connectButton = FindViewById<Button>(Resource.Id.connectButton);
            Button _disconnectButton = FindViewById<Button>(Resource.Id.disconnectButton);
            

            _connectButton.Click += delegate
            {
                if (!_inputEditText.Text.Equals(""))
                {
                    IPAddress.TryParse(_inputEditText.Text, out _address);
                    Log.Info("HA", _address.ToString());
                    //MotionDetector.Client.IpEp = new IPEndPoint(_address, 9050);

                    //client = new Client();
                    var tcpClient = new Thread(StartClient) { IsBackground = true };
                    Log.Info("HA", "started new Thread");
                    tcpClient.Start(this);

                    if (tcpClient.IsAlive)
                    {
                        Thread.Sleep(2000);
                       
                            String element = Client.getElement();

                            switch (element)
                            {
                                case "fire":
                                    _frame.Visibility = ViewStates.Visible;
                                    _picture.SetImageResource(Resource.Drawable.fire);
                                    break;
                                case "earth":
                                    _frame.Visibility = ViewStates.Visible;
                                    _picture.SetImageResource(Resource.Drawable.earth);
                                    break;
                                case "air":
                                    _frame.Visibility = ViewStates.Visible;
                                    _picture.SetImageResource(Resource.Drawable.air);
                                    break;
                                case "water":
                                    _frame.Visibility = ViewStates.Visible;
                                    _picture.SetImageResource(Resource.Drawable.water);
                                    break;

                            
                        
                    }
                   
                    }
                  

                    


                   
               
                }
                else
                    Toast.MakeText(Android.App.Application.Context, "IP-Adress Empty", ToastLength.Long).Show();
            };

            _disconnectButton.Click += delegate
            {
                client.Close();
            };

        }

        private void StartClient()
        {
            Log.Info("HA", "Reached StartClient");
            
            var p = new Client(_address);
            p.Start();
        }


        protected override void OnResume()
        {
            base.OnResume();
            //_sensorManager.RegisterListener(this, _sensorManager.GetDefaultSensor(SensorType.Orientation), SensorDelay.Game);
        }

        protected override void OnPause()
        {
            base.OnPause();
            //_sensorManager.UnregisterListener(this);
        }

        public void OnAccuracyChanged(Sensor sensor, SensorStatus accuracy)
        {
            // We don't want to do anything here.
        }

        public void OnSensorChanged(SensorEvent e)
        {
            if (e.Sensor.Type == SensorType.Orientation)
            {
                lock (SyncLock)
                {
                    var text = new StringBuilder("Sensor: Orientation. \n ")
                                    .Append("\nYaw : ")
                                    .Append(e.Values[0])
                                    .Append("\nRoll : ")
                                    .Append(e.Values[1])
                                    .Append("\nPitch : ")
                                    .Append(e.Values[2])
                                    .Append("\nAccuracy : ")
                                    .Append(e.Accuracy);
                    _sensorTextView.Text = text.ToString();
                    MessageString = "Pitch: " + e.Values[2] + ", Roll:" + e.Values[1];
                    
                    //var Message = new {pitch = e.Values[2], roll = e.Values[1]};
                    

                }
            }
         }

        public void OnAccuracyChanged(Sensor sensor, int accuracy)
        {
            if (sensor.Type == Android.Hardware.SensorType.Orientation)
            {
                if (Android.Hardware.SensorStatus.AccuracyHigh ==
         (Android.Hardware.SensorStatus)accuracy)
                {
                    // Maybe do something if nessecary
                }
            }
        }
    }

   
}