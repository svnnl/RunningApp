using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace RunApp
{
	[Activity (Label = "Running App", MainLauncher = true, Icon = "@drawable/icon")]
	public class RunningApp : Activity
	{
		// Declaring variables
		RunningView runv;
		TextView status;
		Button centre, start, clear;
		LinearLayout buttons, superstack;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			runv = new RunningView (this);

            // Detects the amount of pixels of your screen
            var metrics = Resources.DisplayMetrics;

			// The buttons
			centre = new Button (this);
			centre.Text = "Centre";
			centre.SetWidth(metrics.WidthPixels / 3);
			centre.Click += runv.centreMap;

			start = new Button (this);
			start.Text = "Start";
			start.SetWidth(metrics.WidthPixels / 3);
			start.Click += runv.startRoute;

			clear = new Button (this);
			clear.Text = "Clear";
			clear.SetWidth(metrics.WidthPixels / 3);
			clear.Click += runv.clearMap;


			// The updated location status
			status = new TextView (this);
            status.Text = "Location: ";

			// Layout for horizontal oriented buttons
			buttons = new LinearLayout (this);
			buttons.Orientation = Orientation.Horizontal;
			buttons.AddView (centre);
			buttons.AddView (start);
			buttons.AddView (clear);

			// Adding all the views
			superstack = new LinearLayout (this);
			superstack.Orientation = Orientation.Vertical;
			superstack.AddView (buttons);
			superstack.AddView (status);
			superstack.AddView (runv);

			// Set our view from the LinearLayout
			SetContentView (superstack);
		}
	}
}

