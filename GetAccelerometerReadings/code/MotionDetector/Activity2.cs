using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace MotionDetector
{
    [Activity(Label = "Activity2")]
    public class Activity2 : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            var hui = new Klient(IPAddress.Parse("127.0.0.1"));
        }

        
    }
}