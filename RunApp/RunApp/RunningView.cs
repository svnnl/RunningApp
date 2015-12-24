using System;
using System.Collections.Generic; // Vanwege IList

using Android.App;
using Android.Content;
using Android.Graphics; // Vanwege Color
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Locations; // Vanwege ILocationListener;
using Android.Hardware; // Vanwege ISensorEventListener;

namespace RunApp
{
    // Custom View for the App
    class RunningView : View, ILocationListener, ISensorEventListener
    {
        // Managers for location and sensor
        LocationManager lm;
        SensorManager sm;

        // Drawing variables
        Matrix mat;
        Bitmap map, cursor;

        // Location variables
        float north, east;
        PointF current;

        // Map centering variables
        PointF centre;
        float midx, midy;

        // Variables for touch-event
        PointF v1, v2;
        PointF s1, s2;
        float scale;
        float oldScale;
        bool pinching = false;
        PointF dragstart;

        // Angle for the compass
        float angle;

        // Variables for tracking
        List<PointF> track = new List<PointF>();   // List for the drawn points for tracking
        bool tracking = false;

        AlertDialog.Builder alert;

        // Constructor
        public RunningView(Context c) : base(c)
        {
            // Sensormanager for compass            
            sm = (SensorManager)c.GetSystemService(Context.SensorService);
            sm.RegisterListener(this, sm.GetDefaultSensor(SensorType.Orientation), SensorDelay.Ui);

            // Locationmanager for the GPS
            lm = (LocationManager)c.GetSystemService(Context.LocationService);
            Criteria crit = new Criteria();
            crit.Accuracy = Accuracy.Fine;
            IList<string> alp = lm.GetProviders(crit, true);
            if (alp != null && alp.Count > 0)
            {
                string lp = alp[0];
                lm.RequestLocationUpdates(lp, 500, 4, this);
            }

            BitmapFactory.Options opties = new BitmapFactory.Options();
            opties.InScaled = false;

            // Storing the images in bitmaps
            map = BitmapFactory.DecodeResource(c.Resources, Resource.Drawable.Utrecht, opties);
            cursor = BitmapFactory.DecodeResource(c.Resources, Resource.Drawable.cursor, opties);

            this.Touch += touch;

            centre = new PointF(139000, 455000); // Centre of the map

            alert = new AlertDialog.Builder(c); // For confirmation dialogs
        }

        // Calculates the distance between two points
        static float Afstand(PointF p1, PointF p2)
        {
            float a = p1.X - p2.X;
            float b = p1.Y - p2.Y;
            return (float)Math.Sqrt(a * a + b * b);
        }

        // Action when Location has changed
        public void OnLocationChanged(Location loc)
        {
            north = (float)loc.Latitude;
            east = (float)loc.Longitude;
            string info = $"{north} Latitude, {east} Longitude";		// Doesn't work in Xamarin
            // string info = north.ToString() + " Latitude, " + east.ToString() + " Longitude";  // Works in Xamarin
            RunningApp.status.Text = "Location: " + info;
            PointF geo = new PointF(north, east);
            current = Projectie.Geo2RD(geo);

            // Starts adding points to a list when start button is pressed
            if (tracking == true)
                track.Add(current);

            Invalidate();
        }

        protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);

            if (scale == 0)
                scale = Math.Min(((float)this.Width) / this.map.Width, ((float)this.Height) / this.map.Height);

            // Defines the maximum and minimum scale
            if (scale < 1f)
                scale = 1f;
            if (scale > 4f)
                scale = 4f;

            // Drawing the map
            Paint verf = new Paint();
            verf.Color = Color.Red;
            mat = new Matrix();

            midx = (centre.X - 136000) * 0.4f;
            midy = -(centre.Y - 458000) * 0.4f;

            canvas.Save();

            mat.PostTranslate(-midx, -midy);
            mat.PostScale(scale, scale);
            mat.PostTranslate(Width / 2, Height / 2);

            canvas.DrawBitmap(map, mat, verf);
            canvas.Restore();

            // Drawing your location
            if (current != null)
            {
                float ax = current.X - centre.X;
                float px = ax * 0.4f;
                float sx = px * scale;
                float x = Width / 2 + sx;

                float ay = centre.Y - current.Y;
                float py = ay * 0.4f;
                float sy = py * scale;
                float y = Height / 2 + sy;

                Matrix mat2 = new Matrix();
                mat2.PostTranslate(-cursor.Width / 2, -cursor.Height / 2);
                mat2.PostRotate(angle);
                mat2.PostTranslate(x, y);

                canvas.DrawBitmap(cursor, mat2, verf);

                // Draws the track
                foreach (PointF point in track)
                {
                    ax = point.X - centre.X;
                    px = ax * 0.4f;
                    sx = px * scale;
                    x = Width / 2 + sx;

                    ay = centre.Y - point.Y;
                    py = ay * 0.4f;
                    sy = py * scale;
                    y = Height / 2 + sy;

                    verf.Color = Color.DarkRed;
                    canvas.DrawCircle(x, y, 15, verf);
                }
            }
        }

        // Touch Event
        public void touch(object sender, TouchEventArgs tea)
        {
            // Pinch
            v1 = new PointF(tea.Event.GetX(0), tea.Event.GetY(0));
            if (tea.Event.PointerCount == 2)
            {
                v2 = new PointF(tea.Event.GetX(1), tea.Event.GetY(1));
                if (tea.Event.Action == MotionEventActions.Pointer2Down)
                {
                    s1 = v1;
                    s2 = v2;
                    oldScale = scale;
                }
                pinching = true;
                float dist = Afstand(v1, v2);
                float start = Afstand(s1, s2);
                if (dist != 0 && start != 0)
                {
                    float factor = dist / start;
                    scale = oldScale * factor;
                    Invalidate();
                }
            }
            // Drag
            else if (!pinching)
            {
                // Documents the start point when a finger hits the screen
                if (tea.Event.Action == MotionEventActions.Down)
                    dragstart = new PointF(tea.Event.GetX(), tea.Event.GetY());

                // Records the coordinates while finger in moving on screen
                if (tea.Event.Action == MotionEventActions.Move)
                {
                    float x = tea.Event.GetX();
                    float sx = x - dragstart.X;
                    float px = sx / scale;
                    float ax = px / 0.4f;
                    centre.X -= ax;

                    // Remember the X-coordinate for the next drag movement
                    dragstart.X = x;

                    // Limitations of horizontal dragging
                    if (centre.X > 141665)
                        centre.X = 141665;
                    if (centre.X < 136335)
                        centre.X = 136335;

                    float y = tea.Event.GetY();
                    float sy = y - dragstart.Y;
                    float py = sy / scale;
                    float ay = py / 0.4f;
                    centre.Y += ay;

                    // Remember the Y-coordinate for the next drag movement
                    dragstart.Y = y;

                    // Limitations of vertical dragging
                    if (centre.Y > 457575)
                        centre.Y = 457575;
                    if (centre.Y < 453425)
                        centre.Y = 453425;

                    Invalidate();
                }
            }
            // Resets the pinching bool when finger lifts from screen
            if (tea.Event.Action == MotionEventActions.Up)
                pinching = false;
        }

        // Centers map on your location
        public void centreMap(object sender, EventArgs ea)
        {
            if (current != null)
            {
                centre = new PointF(current.X, current.Y);
                Invalidate();
            }
        }

        // Starts route when Start button is clicked
        public void startRoute(object sender, EventArgs ea)
        {
            Button startButton = (Button)sender;
            string buttonText = startButton.Text;

            if (buttonText == "Start")
            {
                // Code for tracking 
                tracking = true;
                startButton.Text = "Stop";
                RunningApp.status.Text = "Tracking has started.";
            }
            if (buttonText == "Stop")
            {
                tracking = false;
                startButton.Text = "Start";
                RunningApp.status.Text = "Tracking has stopped.";
            }
        }

        // Clears track when dialog is confirmed
        public void clearMap(object sender, EventArgs ea)
        {
            alert.SetTitle("Clear track")
            .SetMessage("Are you sure you want to delete your track?")
            .SetCancelable(false)
            .SetPositiveButton("Yes", (object o, DialogClickEventArgs e) =>
           {
               track.Clear(); // Clears the list of drawn lines for the track
           })
            .SetNegativeButton("No", (object o, DialogClickEventArgs e) =>
            {
                // Do nothing
            })
            .Show();
        }

        // Gives angle its value
        public void OnSensorChanged(SensorEvent e)
        {
            angle = e.Values[0];
            Invalidate();
        }

        // Necessary methods for Location interface
        public void OnProviderDisabled(string provider)
        {
        }

        public void OnProviderEnabled(string provider)
        {
        }

        public void OnStatusChanged(string provider, [GeneratedEnum] Availability status, Bundle extras)
        {
        }

        // Sensor interface methods
        public void OnAccuracyChanged(Sensor sensor, [GeneratedEnum] SensorStatus accuracy)
        {
        }
    }
}