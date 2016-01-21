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
    /// <summary>
    /// View for the drawing of the analysis graph
    /// </summary>
    class AnalyzeView : View
    {
        string message;
        List<DataPoint> track = new List<DataPoint>();

        public AnalyzeView(Context c, string s) : base(c)
        {
            this.message = s;

            // Insert dummy track here
            track.Add(new DataPoint(new PointF(139191.1f, 455584.9f), new TimeSpan(0, 0, 0, 1, 734)));
            track.Add(new DataPoint(new PointF(139175.0f, 455596.5f), new TimeSpan(0, 0, 0, 4, 936)));
            track.Add(new DataPoint(new PointF(139156.3f, 455614.8f), new TimeSpan(0, 0, 0, 5, 736)));
            track.Add(new DataPoint(new PointF(139135.8f, 455632.2f), new TimeSpan(0, 0, 0, 7, 733)));
            track.Add(new DataPoint(new PointF(139114.9f, 455652.7f), new TimeSpan(0, 0, 0, 9, 780)));
            track.Add(new DataPoint(new PointF(139095.4f, 455675.7f), new TimeSpan(0, 0, 0, 11, 772)));
            track.Add(new DataPoint(new PointF(139078.4f, 455702.9f), new TimeSpan(0, 0, 0, 13, 733)));
        }

        protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);

            Paint paint = new Paint();
            paint.Color = Color.White;
            paint.TextSize = 50;

            // Turns the string into a list with DataPoints
            if (message != "")
            {
                track.Clear();  // Clears dummy Track for the new track

                string[] values = message.Split('\n');
                
                foreach (string s in values)
                {
                    string[] data = s.Split(' ');
                    PointF point = new PointF(float.Parse(data[0]), float.Parse(data[1]));
                    TimeSpan time = TimeSpan.FromTicks(long.Parse(data[2]));
                    DataPoint t = new DataPoint(point, time);
                    track.Add(t);
                }
            }

            // Drawing the graph
            canvas.DrawText("Total distance: " + Math.Round(totalDistance(track),2).ToString() + " m", 50, 50, paint);
            canvas.DrawText("Average velocity: " + Math.Round(avgSpeed(track),2).ToString() + " m/s", 50, 100, paint);
            canvas.DrawText("In a total of " + Math.Round(Time(track.First(), track.Last()),2).ToString()+" seconds", 50, 150, paint);
        }

        /// <summary>
        /// Calculates the distance between two points
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static double Distance(DataPoint a, DataPoint b)
        {
            float x = b.currentLocation.X - a.currentLocation.X;
            float y = b.currentLocation.Y - a.currentLocation.Y;
            return Math.Sqrt(x * x + y * y);
        }

        /// <summary>
        /// Calculates the total distance of the track
        /// </summary>
        /// <param name="track"></param>
        /// <returns></returns>
        public static double totalDistance(List<DataPoint> track)
        {
            double distance = 0;
            int count = track.Count;
            for (int t = 0; t <= count - 2; t++)
            {
                distance += Distance(track[t], track[t + 1]);
            }

            return distance;
        }

        /// <summary>
        /// Calcules the time between two points
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static double Time(DataPoint a, DataPoint b)
        {
            return b.currentTime.TotalSeconds - a.currentTime.TotalSeconds;
        }

        /// <summary>
        /// Calculates the speed between two points in m/s
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static double Speed(DataPoint a, DataPoint b)
        {
            double distance = Distance(a, b);
            double time = Time(a, b);
            return distance / time;
        }

        /// <summary>
        /// Calculates the average speed of the whole track in m/s
        /// </summary>
        /// <param name="track"></param>
        /// <returns></returns>
        public static double avgSpeed(List<DataPoint> track)
        {
            int count = track.Count;
            return Speed(track[0], track[count - 1]);
        }
    }
}