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

namespace RunApp
{

    // Custom View for the App
    class RunningView : View , ILocationListener
    {
        LocationManager lm;
        Bitmap b;
        PointF v1, v2;
        float schaal;
        double north, east;

        // Constructor
        public RunningView(Context c) : base(c)
        {
            this.Touch += pinch;
            b = BitmapFactory.DecodeResource(c.Resources, Resource.Drawable.Utrecht);
           // lm = new LocationManager()
            // lm.RequestLocationUpdates()
        }

        protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);
        }
   
        // Touch Event
        public void pinch(object sender, TouchEventArgs tea)
        {
            v1 = new PointF(tea.Event.GetX(), tea.Event.GetY());

            // Pinch event
            if (tea.Event.PointerCount == 2)
            {
                v2 = new PointF(tea.Event.GetX(1), tea.Event.GetY(1));
               // if (tea.Event.Action ==  )

            }
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
    }
}