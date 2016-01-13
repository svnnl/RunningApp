using System;
using Android.App;
using Android.Content;
using Android.Widget;
using Android.OS;
using Android.Graphics;

namespace RunApp
{
    [Activity(Label = "5532795 RunForrest", MainLauncher = true, Icon = "@drawable/run")]
    public class RunningApp : Activity
    {
        // Declaring variables
        RunningView runv;
        public static TextView status;
        public Button centre, start, clear, share, analyze, save;
        LinearLayout buttons1, buttons2, superstack;
        AlertDialog.Builder alert;

        // Asks if you really want to close the app when pressing 'Back' button
        public override void OnBackPressed()
        {
            alert = new AlertDialog.Builder(this);
            alert.SetTitle("Quit")
            .SetMessage("Are you sure you want to exit?")
            .SetCancelable(false)
            .SetPositiveButton("Yes", (object sender, DialogClickEventArgs ea) =>
            {
                Finish();
            })
            .SetNegativeButton("No", (object sender, DialogClickEventArgs ea) =>
            {
                // Do nothing
            })
            .Show();
        }

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            runv = new RunningView(this);

            // Detects the amount of pixels of your screen
            var metrics = Resources.DisplayMetrics;

            // The buttons
            centre = new Button(this);
            centre.Text = "Centre";
            centre.SetWidth(metrics.WidthPixels / 3);
            centre.Click += runv.centreMap;

            start = new Button(this);
            start.Text = "Start";
            start.SetWidth(metrics.WidthPixels / 3);
            start.Click += runv.startRoute;

            clear = new Button(this);
            clear.Text = "Clear";
            clear.SetWidth(metrics.WidthPixels / 3);
            clear.Click += runv.clearMap;

            share = new Button(this);
            share.Text = "Share";
            share.SetWidth(metrics.WidthPixels / 3);
            share.Click += shareTrack;

            analyze = new Button(this);
            analyze.Text = "Analyze";
            analyze.SetWidth(metrics.WidthPixels / 3);
            analyze.Click += analyzeTrack;

            save = new Button(this);
            save.Text = "Save";
            save.SetWidth(metrics.WidthPixels / 3);
            save.Click += saveTrack;

            // The updated location status
            status = new TextView(this);
            status.SetTextColor(Color.White);
            status.Text = "Waiting for GPS signal...";

            // Layout for horizontal oriented buttons
            buttons1 = new LinearLayout(this);
            buttons1.Orientation = Orientation.Horizontal;
            buttons1.AddView(centre);
            buttons1.AddView(start);
            buttons1.AddView(clear);

            buttons2 = new LinearLayout(this);
            buttons2.Orientation = Orientation.Horizontal;
            buttons2.AddView(share);
            buttons2.AddView(analyze);
            buttons2.AddView(save);

            // Adding all the views
            superstack = new LinearLayout(this);
            superstack.Orientation = Orientation.Vertical;
            superstack.AddView(buttons1);
            superstack.AddView(buttons2);
            superstack.AddView(status);
            superstack.AddView(runv);
            // superstack.SetBackgroundColor(Color.Cyan);

            // Set our view from the LinearLayout
            SetContentView(superstack);
        }

        // Option to share the track with different kinds of social media
        public void shareTrack(object sender, EventArgs ea)
        {          
            Intent i = new Intent(Intent.ActionSend);
            i.SetType("text/plain");

            string message = runv.ToString();

            i.PutExtra(Intent.ExtraText, message);
            StartActivity(i);
        }

        // Starts new activity that analyzes the track
        public void analyzeTrack(object sender, EventArgs ea)
        {
            Intent i = new Intent(this, typeof(AnalyzeApp));
            string message = runv.ToString();

            i.PutExtra("message", message);

            StartActivity(i);
        }

        // Starts new activity with a track list and option to save the current track
        public void saveTrack(object sender, EventArgs ea)
        {
            Intent i = new Intent(this, typeof(SaveApp));

            string message = runv.ToString();
            i.PutExtra("message", message);

            StartActivity(i);
        }
    }
}

// Sebastiaan van Nijen 5532795
// Basil Morsi 5754003