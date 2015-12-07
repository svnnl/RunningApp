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

        Bitmap b;
        
        float scale;
        // float angle;
        double north, east;
        AlertDialog.Builder alert;
        ScaleGestureDetector detector;

        // Constructor
        public RunningView(Context c) : base(c)
        {               
            sm = (SensorManager)c.GetSystemService(Context.SensorService);
            sm.RegisterListener(this, sm.GetDefaultSensor(SensorType.Orientation), SensorDelay.Ui);
            
            lm = (LocationManager)c.GetSystemService(Context.LocationService);
           //lm.RequestLocationUpdates(500, 5 );

            BitmapFactory.Options opties = new BitmapFactory.Options();

            b = BitmapFactory.DecodeResource(c.Resources, Resource.Drawable.Utrecht, opties);
            opties.InScaled = false;

            this.Touch += touch;

            detector = new ScaleGestureDetector(c, this);
            alert = new AlertDialog.Builder(c);
        }

        protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);

            if(scale == 0)
                scale = Math.Min(((float)this.Width) / this.b.Width, ((float)this.Height) / this.b.Height);

            Paint verf = new Paint();

            Matrix mat = new Matrix();
            mat.PostTranslate(-this.b.Width / 2, -this.b.Height / 2);
            mat.PostScale(this.scale, this.scale);
            // mat.PostRotate(-this.angle);
            mat.PostTranslate(this.Width / 2, this.Height / 2);
            canvas.DrawBitmap(b, mat, verf);
        }

        // Centers map on your location
        public void centreMap(object sender, EventArgs ea)
        {

        }

        // Starts route when Start button is clicked
        public void startRoute(object sender, EventArgs ea)
        {

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

        // Touch Event
        public void touch(object sender, TouchEventArgs tea)
        {
            detector.OnTouchEvent(tea.Event);
        }

        // Gesture detector interface methods
        public bool OnScale(ScaleGestureDetector detector)
        {
            this.scale *= detector.ScaleFactor;
            this.Invalidate();
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
        }

        // Necessary methods for Location interface
        public void OnProviderDisabled(string provider)
        {
            throw new NotImplementedException();
        }

        public void OnProviderEnabled(string provider)
        {
            throw new NotImplementedException();
        }

        public void OnStatusChanged(string provider, [GeneratedEnum] Availability status, Bundle extras)
        {
            throw new NotImplementedException();
        }

        // Sensor interface methods
        public void OnAccuracyChanged(Sensor sensor, [GeneratedEnum] SensorStatus accuracy)
        {
            throw new NotImplementedException();
        }

        public void OnSensorChanged(SensorEvent e)
        {
            throw new NotImplementedException();
        }
    }
}