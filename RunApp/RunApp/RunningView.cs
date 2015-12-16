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
    class RunningView : View , ILocationListener, ISensorEventListener
    {
        // Managers for location and sensor
        LocationManager lm;
        SensorManager sm;

        Matrix mat;
        Bitmap map, cursor;
        
        float north, east;
        PointF current;

        PointF centre;
        float midx, midy;
        
        // Variables for touch-event
        PointF v1, v2;
        PointF s1, s2;
        float scale;
        float oldScale;
        bool pinching = false;
        PointF dragstart;

        float angle;
        float ax;
        float ay;

        string info;

        List<PointF> track = new List<PointF>(); // List for the drawn points for tracking

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
            if(alp != null && alp.Count >0)
            {            
                string lp = alp[0];
                lm.RequestLocationUpdates(lp, 500, 10, this);               
            }

            BitmapFactory.Options opties = new BitmapFactory.Options();

            map = BitmapFactory.DecodeResource(c.Resources, Resource.Drawable.Utrecht, opties);            
            cursor = BitmapFactory.DecodeResource(c.Resources, Resource.Drawable.cursor, opties);

            opties.InScaled = false;
            
           this.Touch += touch;

            centre = new PointF(139000, 455500);
            ax = 0;
            ay = 0;
            
            alert = new AlertDialog.Builder(c);
        }

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
            info = $"{north} Latitude, {east} Longitude";
            PointF geo = new PointF(north, east);
            current = Projectie.Geo2RD(geo);
            Invalidate();
        }

        protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);       

            if(scale == 0)
                scale = Math.Min(((float)this.Width) / this.map.Width, ((float)this.Height) / this.map.Height);

        // Drawing the map
            Paint verf = new Paint();
            mat = new Matrix();

            midx = (centre.X - 136000) * 0.4f;
            midy = -(centre.Y - 458000) * 0.4f;

            mat.PostTranslate(-(map.Width / 2 - ax), -(map.Height / 2 - ay));
            
            // mat.PostTranslate(-midx, -midy);
            mat.PostScale(scale, scale);
            mat.PostTranslate(Width / 2, Height / 2);
          
            canvas.DrawBitmap(map, mat, verf);
            
        // Drawing the cursor on your location
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

                mat2.PostTranslate(x, y);
                mat2.PostScale(scale, scale);
                mat2.PostRotate(-angle);
                mat2.PostTranslate(Width / 2, Height / 2);

                // canvas.DrawCircle(x, y, 10, verf);
                canvas.DrawBitmap(cursor, mat2, verf);
            }

        // Drawing the track
           /* foreach(PointF point in track)
            {
                canvas.DrawPoint(point.X, point.Y, verf);
            }*/
        }

    // Touch Event
       public void touch(object sender, TouchEventArgs tea)
        {
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
                    if (scale < 4f)
                        scale = 4f;
                    if (scale > 100f)
                        scale = 100f;
                    Invalidate();
                }
                
            }
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
                    ax = (px / 0.4f);
                    //dragstart.X = x;
                    //centre.X = centre.X - ax;

                    /*if (centre.X > 142000)
                        centre.X = 142000;
                    if (centre.X < 136000)
                        centre.X = 136000;*/

                    float y = tea.Event.GetY();
                    float sy = y - dragstart.Y;
                    float py = sy / scale;
                    ay = (py) / 0.4f;
                    //dragstart.Y = y;
                    //centre.Y = centre.Y - ay;

                   /* if (centre.Y > 458000)
                        centre.Y = 458000;
                    if (centre.Y < 453000)
                        centre.Y = 453000;*/

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
            // midx = x;
            // midy = y;
            // Invalidate();
        }

        // Starts route when Start button is clicked
        public void startRoute(object sender, EventArgs ea)
        {
            Button startButton = (Button)sender;
            string buttonText = startButton.Text;

            if(buttonText == "Start")
            {
                // Code for tracking 

                startButton.Text = "Stop";
            }
            else
            {
                // Do nothing
                startButton.Text = "Start";
            }
                       
        }

        // Clears track when dialog is confirmed
        public void clearMap(object sender, EventArgs ea)
        {
            alert.SetMessage("Are you sure you want to delete your track?");
            alert.SetCancelable(true);
            alert.SetPositiveButton("Yes", (object o, DialogClickEventArgs e) =>
            {
                track.Clear(); // Clears the list of drawn lines for the track
            });
            alert.SetNegativeButton("No", (object o, DialogClickEventArgs e) =>
            {
                // Do nothing
            });
            alert.Show();
        }

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