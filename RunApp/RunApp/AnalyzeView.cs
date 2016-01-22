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
            track.Add(new DataPoint(new PointF(139064.9f, 455730.5f), new TimeSpan(0, 0, 0, 15, 752)));
            track.Add(new DataPoint(new PointF(139053.2f, 455760.7f), new TimeSpan(0, 0, 0, 17, 756)));
            track.Add(new DataPoint(new PointF(139041.3f, 455790.4f), new TimeSpan(0, 0, 0, 19, 751)));
            track.Add(new DataPoint(new PointF(139028.9f, 455821.0f), new TimeSpan(0, 0, 0, 21, 732)));
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

            // Local variables for the drawing
            int n = track.Count;            // Number of points to draw
            int x = 20;                     // Pixels from the left side of screen
            int y = (int)maxSpeed(track);   // Maximum value on Y-axis
            int w = Width / n;              // Pixels per point on the X-axis
            int h = Height / y;             // Pixels per 1 m/s

            // Drawing the statistics
            canvas.DrawText("Total distance: " + Math.Round(totalDistance(track), 2).ToString() + " m", x, 50, paint);
            canvas.DrawText("Average velocity: " + Math.Round(avgSpeed(track), 2).ToString() + " m/s", x, 100, paint);
            canvas.DrawText("With a maximum velocity of: " + Math.Round(maxSpeed(track), 2).ToString() + " m/s", x, 150, paint);
            canvas.DrawText("In a total of " + Math.Round(Time(track.First(), track.Last()), 2).ToString() + " seconds", x, 200, paint);

            // Drawing the axis
            canvas.DrawLine(x, Height - 20, Width -20, Height-20,paint );      // X-axis
            canvas.DrawLine(x, Height -20, x, 250,paint );       // Y-axis

            // Drawing the graph            
            if (track.Count >= 3)
            {
                for (int t = 1; t <= n - 3; t++)
                {
                    double speed1 = Speed(track[t - 1], track[t]);
                    double speed2 = Speed(track[t], track[t + 1]);
                    double avgSpeed = (speed1 + speed2) / 2;

                    double speed3 = Speed(track[t], track[t + 1]);
                    double speed4 = Speed(track[t + 1], track[t + 2]);
                    double avgSpeed2 = (speed3 + speed4) / 2;
                    
                    canvas.DrawPoint(w * t, (float)avgSpeed * h, paint);
                    canvas.DrawLine(w * t, (float)avgSpeed * h, w * (t + 1), (float)avgSpeed2 * h, paint); 
                }
            }
           
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

        /// <summary>
        /// Calculates the maximum speed
        /// </summary>
        /// <param name="track"></param>
        /// <returns></returns>
        public static double maxSpeed(List<DataPoint> track)
        {
            if (track.Count >= 2)
            {
                double maxspeed = Speed(track[0], track[1]);
                for (int t = 1; t <= track.Count - 2; t++)
                {
                    double speed = Speed(track[t], track[t + 1]);
                    if (speed > maxspeed)
                        maxspeed = speed;
                }

                return maxspeed;
            }
            else
                return 0;
        }
    }
}