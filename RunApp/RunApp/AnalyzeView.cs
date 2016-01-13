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
        List<DataPoint> track = new List<DataPoint>();

        public AnalyzeView(Context c, string s) : base(c)
        {            
            this.message = s;
        }

        protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);

            if (message != "")
            {
                string[] values = message.Split('\n');

                foreach (string s in values)
                {
                    string[] data = s.Split(' ');
                    PointF point = new PointF(float.Parse(data[0]), float.Parse(data[1]));
                    TimeSpan time = new TimeSpan(int.Parse(data[2]));
                    DataPoint t = new DataPoint(point, time);
                    track.Add(t);
                }
            }
        }

        public static double Distance(DataPoint a, DataPoint b)
        {
            float x = b.currentLocation.X - a.currentLocation.X;
            float y = b.currentLocation.Y - a.currentLocation.Y;
            return Math.Sqrt(x * x + y * y);
        }

        public static double totalDistance(List<DataPoint> track)
        {
            double distance = 0;
            int count = track.Count;
            for(int t = 0; t < count; t++)
            {
                distance = Distance(track[t], track[t++]);
            }
           
            return distance;
        }
    }
}