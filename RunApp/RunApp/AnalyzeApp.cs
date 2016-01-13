using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Graphics;

namespace RunApp
{
    [Activity(Label = "Track analysis")]
    public class AnalyzeApp : Activity
    {
        AnalyzeView analyzeView;
        
        public string message;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            message = Intent.GetStringExtra("message");
            

            analyzeView = new AnalyzeView(this, message);
            
            SetContentView(analyzeView);
        }
    }
}