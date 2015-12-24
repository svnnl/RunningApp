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
        public Button centre, start, clear;
        LinearLayout buttons, superstack;
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

            // The updated location status
            status = new TextView(this);
            status.SetTextColor(Color.Black);
            status.Text = "Waiting for GPS signal...";

            // Layout for horizontal oriented buttons
            buttons = new LinearLayout(this);
            buttons.Orientation = Orientation.Horizontal;
            buttons.AddView(centre);
            buttons.AddView(start);
            buttons.AddView(clear);
            buttons.SetBackgroundColor(Color.DarkCyan);

            // Adding all the views
            superstack = new LinearLayout(this);
            superstack.Orientation = Orientation.Vertical;
            superstack.AddView(buttons);
            superstack.AddView(status);
            superstack.AddView(runv);
            superstack.SetBackgroundColor(Color.Cyan);

            // Set our view from the LinearLayout
            SetContentView(superstack);
        }
    }
}

// Sebastiaan van Nijen 5532795
// Basil Morsi 5754003