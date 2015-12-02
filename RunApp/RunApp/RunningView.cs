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

    // Custom View for the App
    class RunningView : View
    {
        public RunningView(Context c) : base(c)
        {

        }

        protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);
        }
    }
}