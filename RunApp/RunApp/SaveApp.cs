using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace RunApp
{
    [Activity(Label = "Save")]
    public class SaveApp : Activity
    {
        ListView lView;

        protected override void OnCreate(Bundle b)
        {
            base.OnCreate(b);

            lView = new ListView(this);

            SetContentView(lView);
        }
    }
}