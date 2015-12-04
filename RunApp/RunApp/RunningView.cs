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
        // LocationManager lm;
        Bitmap b;
        
        float scale;
        // float angle;
        double north, east;
        AlertDialog.Builder alert;
        ScaleGestureDetector det;

        // Constructor
        public RunningView(Context c) : base(c)
        {
            this.Touch += touch;
            det = new ScaleGestureDetector(c, this);
            alert = new AlertDialog.Builder(c);

            // SensorManager sm = (SensorManager)c.GetSystemService(Context.SensorService);
            // sm.RegisterListener(this, sm.GetDefaultSensor(SensorType.Orientation), SensorDelay.Ui);
        
            BitmapFactory.Options opties = new BitmapFactory.Options();

            b = BitmapFactory.DecodeResource(c.Resources, Resource.Drawable.Utrecht, opties);
            opties.InScaled = false;
                        
            // lm.RequestLocationUpdates()
        }

        protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);

            if(scale == 0)
                scale = Math.Min(((float)this.Width) / this.b.Width, ((float)this.Height) / this.b.Height);

            Paint verf = new Paint();
            verf.TextSize = 30;

            Matrix mat = new Matrix();
            mat.PostTranslate(-this.b.Width / 2, -this.b.Height / 2);
            mat.PostScale(this.scale, this.scale);
            // mat.PostRotate(-this.angle);
            mat.PostTranslate(this.Width / 2, this.Height / 2);
            canvas.DrawBitmap(b, mat, verf);
        }

        // Touch Event
        public void touch(object sender, TouchEventArgs tea)
        {
            det.OnTouchEvent(tea.Event);
        }
        
        public void centreMap(object sender, EventArgs ea)
        {

        }
        
        public void startRoute(object sender, EventArgs ea)
        {

        }

        // Clears track when dialog is confirmed
        public void clearMap(object sender, EventArgs ea)
        {
            alert.SetMessage("Are you sure you want to delete your track?");
            alert.SetCancelable(false);
            // alert.SetPositiveButton("Yes",  )
            alert.Show();
        }

        public void OnLocationChanged(Location loc)
        {
            north = loc.Latitude;
            east = loc.Longitude;
        }

        // Necessary methods for Interface implementation
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

        public void OnAccuracyChanged(Sensor sensor, [GeneratedEnum] SensorStatus accuracy)
        {
            throw new NotImplementedException();
        }

        public void OnSensorChanged(SensorEvent e)
        {
            throw new NotImplementedException();
        }

        public bool OnScale(ScaleGestureDetector detector)
        {
            throw new NotImplementedException();
        }

        public bool OnScaleBegin(ScaleGestureDetector detector)
        {
            this.scale *= det.ScaleFactor;
            this.Invalidate();
            return true;
        }

        public void OnScaleEnd(ScaleGestureDetector detector)
        {
            throw new NotImplementedException();
        }
    }
}