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
    [Activity(Label = "Analyze")]
    public class AnalyzeApp : Activity
    {
        AnalyzeView analyzeView;
        
        public static string message;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            analyzeView = new AnalyzeView(this);

            message = Intent.GetStringExtra("message");
            

            SetContentView(analyzeView);
        }
    }
}