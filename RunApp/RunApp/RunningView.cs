using System;
using System.Collections.Generic; // Vanwege IList
using System.Linq;
using System.Text;

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
        LocationManager lm;
        SensorManager sm;

        Matrix mat;
        Bitmap map, cursor;
        
        float north, east;
        PointF current;

        PointF centre;
        float midx, midy;
        
        PointF v1, v2;
        PointF s1, s2;
        float scale;
        float oldScale;
        bool pinching = false;

        float angle;

        string info;

        AlertDialog.Builder alert;

        // Constructor
        public RunningView(Context c) : base(c)
        {               
            sm = (SensorManager)c.GetSystemService(Context.SensorService);
            sm.RegisterListener(this, sm.GetDefaultSensor(SensorType.Orientation), SensorDelay.Ui);
            
           /* lm = (LocationManager)c.GetSystemService(Context.LocationService);
            Criteria crit = new Criteria();
            crit.Accuracy = Accuracy.Fine;
            IList<string> alp = lm.GetProviders(crit, true);
            if(alp != null)
            {
                try
                {
                    string lp = alp[0];
                    lm.RequestLocationUpdates(lp, 0, 0, this);
                }
                catch(ArgumentOutOfRangeException e)
                {

                }
            }*/

            BitmapFactory.Options opties = new BitmapFactory.Options();

            map = BitmapFactory.DecodeResource(c.Resources, Resource.Drawable.Utrecht, opties);            
            cursor = BitmapFactory.DecodeResource(c.Resources, Resource.Drawable.cursor, opties);

            opties.InScaled = false;
            
            this.Touch += touch;

            alert = new AlertDialog.Builder(c);
        }

        static float Afstand(PointF p1, PointF p2)
        {
            float a = p1.X - p2.X;
            float b = p1.Y - p2.Y;
            return (float)Math.Sqrt(a * a + b * b);
        }

        // Touch Event
        public void touch(object sender, TouchEventArgs tea)
        {
            v1 = new PointF(tea.Event.GetX(0), tea.Event.GetY(0));
            if(tea.Event.PointerCount == 2)
            {
                v2 = new PointF(tea.Event.GetX(1), tea.Event.GetY(1));
                if(tea.Event.Action == MotionEventActions.Pointer2Down)
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
                    scale = Math.Max(1f, Math.Min(scale, 10f));
                    Invalidate();
                }
            }
            else if (!pinching)
            {                
                float x = tea.Event.GetX();
                float sx = x - v1.X;
                float px = sx / scale;
                float ax = (float) (px / 0.4);
                centre.X -= ax;

                float y = tea.Event.GetY();
                float sy = y - v1.Y;
                float py = sy / scale;
                float ay = (float)(py / 0.4);
                centre.Y -= ay;
            }
        }

        protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);

            centre = new PointF(138300, 454300);
            midx = (float) ((centre.X - 136000) * 0.4);
            midy = (float) -((centre.Y - 458000) * 0.4);

            if(scale == 0)
                scale = Math.Min(((float)this.Width) / this.map.Width, ((float)this.Height) / this.map.Height);

            Paint verf = new Paint();

            mat = new Matrix();
            Matrix mat2 = new Matrix();

            mat2.PostTranslate(-this.cursor.Width / 2, -this.cursor.Height / 2);
            mat2.PostScale(scale, scale);
            mat2.PostRotate(-this.angle);
            mat2.PostTranslate(Width / 2, Height / 2);
            
            mat.PostTranslate(-this.map.Width / 2, -this.map.Height / 2);
            // mat.PostTranslate(midx, midy);
            mat.PostScale(this.scale, this.scale);
            mat.PostTranslate(this.Width / 2, this.Height / 2);

          
            canvas.DrawBitmap(map, mat, verf);
            canvas.DrawBitmap(cursor, mat2, verf);
        }

        // Centers map on your location
        public void centreMap(object sender, EventArgs ea)
        {

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
            alert.SetPositiveButton("Yes", ((object o, DialogClickEventArgs e) =>
            {
                // Delete track
            }));
            alert.SetNegativeButton("No", ((object o, DialogClickEventArgs e) =>
            {
                // Do nothing
            }));
            alert.Show();
        }

        // Action when Location has changed
        public void OnLocationChanged(Location loc)
        {
            north = (float) loc.Latitude;
            east = (float) loc.Longitude;
            info = $"{north} Latitude, {east} Longitude";
            PointF geo = new PointF(north, east);
            current = Projectie.Geo2RD(geo);
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

        public void OnSensorChanged(SensorEvent e)
        {
            angle = e.Values[0];
            Invalidate();
        }
    }
}