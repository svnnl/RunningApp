using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace RunApp
{
    [Activity(Label = "Running App", MainLauncher = true, Icon = "@drawable/icon")]
    public class RunningApp : Activity
    {
        // Declaring variables
        RunningView runv;
        TextView header, status;
        Button centre, start, clear;
        LinearLayout buttons, superstack;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            runv = new RunningView(this);

            // The header of the App
            header = new TextView(this);
            header.Text = "Buttons";

            // The buttons
            centre = new Button(this);
            centre.Text = "Centre";
            centre.Click += runv.centreMap;

            start = new Button(this);
            start.Text = "Start";
            start.Click += runv.startRoute;

            clear = new Button(this);
            clear.Text = "Clear";
            clear.Click += runv.clearMap;

            // The updates location status
            status = new TextView(this);
            status.Text = "Location:"; // Coordinates

            // Layout for horizontal oriented buttons
            buttons = new LinearLayout(this);
            buttons.Orientation = Orientation.Horizontal;
            buttons.AddView(centre);
            buttons.AddView(start);
            buttons.AddView(clear);

            // Adding all the views
            superstack = new LinearLayout(this);
            superstack.Orientation = Orientation.Vertical;
            superstack.AddView(header);
            superstack.AddView(buttons);
            superstack.AddView(status);
            superstack.AddView(runv);

            // Set our view from the LinearLayout
            SetContentView(superstack);
        }
    }
}

