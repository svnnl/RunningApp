using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;


namespace RunApp
{
    [Activity(Label = "Track list")]
    public class SaveApp : Activity
    {
        ListView lView;
        ArrayAdapter<string> adp;
        string message;
        Button save;
        LinearLayout stack;

        protected override void OnCreate(Bundle b)
        {
            base.OnCreate(b);

            save = new Button(this);
            save.Text = "Click here to save the current track.";
            save.Click += saveCurrentTrack;

            message = Intent.GetStringExtra("message") ?? "Empty string";

            lView = new ListView(this);
            adp = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSelectableListItem);
            lView.Adapter = adp;

            lView.ChoiceMode = ChoiceMode.None;

            lView.ItemClick += click;

            stack = new LinearLayout(this);
            stack.Orientation = Orientation.Vertical;
            stack.AddView(save);
            stack.AddView(lView);

            SetContentView(stack);
        }

        public void click(object sender, AdapterView.ItemClickEventArgs e)
        {
            int pos = e.Position;
            Intent i = new Intent(this, typeof(AnalyzeApp));
            // Position of item
            // Intent.PutExtra("item", string van item);

            StartActivity(i);
        }

        private void saveCurrentTrack(object sender, EventArgs ea)
        {

        }
    }
}