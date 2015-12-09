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
using Android.Locations;
using Android.Hardware;

namespace RunApp
{
    // Custom View for the App
    class RunningView : View ,
        ILocationListener, ISensorEventListener, ScaleGestureDetector.IOnScaleGestureListener
    {
        LocationManager lm;
        SensorManager sm;

        Matrix mat;
        Bitmap map, pointer;
        double north, east;
        float scale;
        PointF current;
        float angle;
        string info;

        AlertDialog.Builder alert;
        ScaleGestureDetector detector;

        // Constructor
        public RunningView(Context c) : base(c)
        {               
            sm = (SensorManager)c.GetSystemService(Context.SensorService);
            sm.RegisterListener(this, sm.GetDefaultSensor(SensorType.Orientation), SensorDelay.Ui);
            
            /*lm = (LocationManager)c.GetSystemService(Context.LocationService);
            Criteria crit = new Criteria();
            crit.Accuracy = Accuracy.Fine;
            IList<string> alp = lm.GetProviders(crit, true);
            if(alp != null)
            {
                string lp = alp[0];
                lm.RequestLocationUpdates(lp, 0, 0, this);
            }*/

            BitmapFactory.Options opties = new BitmapFactory.Options();

            map = BitmapFactory.DecodeResource(c.Resources, Resource.Drawable.Utrecht, opties);
            opties.InScaled = false;

            this.Touch += touch;

            detector = new ScaleGestureDetector(c, this);
            alert = new AlertDialog.Builder(c);
        }

        // Touch Event
        public void touch(object sender, TouchEventArgs tea)
        {
            
        }

        protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);

            if(scale == 0)
                scale = Math.Min(((float)this.Width) / this.map.Width, ((float)this.Height) / this.map.Height);

            Paint verf = new Paint();

            mat = new Matrix();
            
            mat.PostTranslate(-this.map.Width / 2, -this.map.Height / 2);
            mat.PostScale(this.scale, this.scale);
            // mat.PostRotate(-this.angle);
            mat.PostTranslate(this.Width / 2, this.Height / 2);

            canvas.DrawBitmap(map, mat, verf);
    
        }

        // Centers map on your location
        public void centreMap(object sender, EventArgs ea)
        {

        }

        // Starts route when Start button is clicked
        public void startRoute(object sender, EventArgs ea)
        {
            
            bool stop = false;
            if(stop == false)
            {
                
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

        // Gesture detector interface methods
        public bool OnScale(ScaleGestureDetector detector)
        {
            scale *= detector.ScaleFactor;
            scale = Math.Max(1f, Math.Min(scale, 6f));
            Invalidate();
            return true;
        }

        public bool OnScaleBegin(ScaleGestureDetector detector)
        {
            return true;
        }

        public void OnScaleEnd(ScaleGestureDetector detector)
        {
            
        }

        // Action when Location has changed
        public void OnLocationChanged(Location loc)
        {
            north = loc.Latitude;
            east = loc.Longitude;
            info = $"{north} Latitude, {east} Longitude";
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