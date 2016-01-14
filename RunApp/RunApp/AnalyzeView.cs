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

            // Turns the string into a list with DataPoints
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

            // Drawing the graph

        }

        // Calculates the distance between two points
        public static double Distance(DataPoint a, DataPoint b)
        {
            float x = b.currentLocation.X - a.currentLocation.X;
            float y = b.currentLocation.Y - a.currentLocation.Y;
            return Math.Sqrt(x * x + y * y);
        }

        // Calculates the total distance of the track
        public static double totalDistance(List<DataPoint> track)
        {
            double distance = 0;
            int count = track.Count;
            for (int t = 0; t < count; t++)
            {
                distance = Distance(track[t], track[t++]);
            }

            return distance;
        }

        // Calcules the time between two points
        public static double Time(DataPoint a, DataPoint b)
        {
            return b.currentTime.TotalSeconds - a.currentTime.TotalSeconds;
        }

        // Calculates the speed between two points in m/s
        public static double Speed(DataPoint a, DataPoint b)
        {
            double distance = Distance(a, b);
            double time = Time(a, b);
            return distance / time;
        }

        // Calculates the average speed of the whole track in m/s
        public static double avgSpeed(List<DataPoint> track)
        {
            int count = track.Count;
            return Speed(track[0], track[count-1]);
        }
    }
}