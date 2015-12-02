using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace RunApp
{
    [Activity(Label = "Running App", MainLauncher = true, Icon = "@drawable/icon")]
    public class RunningApp : Activity
    {
        RunningView runv;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            runv = new RunningView(this);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
        }
    }
}

