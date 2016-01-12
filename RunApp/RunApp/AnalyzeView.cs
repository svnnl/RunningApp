using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace RunApp
{
    // Custom class for the analyze screen
    class AnalyzeView : View
    {
        string message;
        List<Track> track = new List<Track>();

        public AnalyzeView(Context c) : base(c)
        {
            message = AnalyzeApp.message;
        }

        protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);

            if (message != null)
            {
                string[] values = message.Split('\n');

                foreach (string s in values)
                {
                    string[] data = s.Split(' ');
                    PointF point = new PointF(float.Parse(data[0]), float.Parse(data[1]));
                    TimeSpan time = new TimeSpan(int.Parse(data[2]));
                    Track t = new Track(point, time);
                    track.Add(t);
                }
            }
        }
    }
}