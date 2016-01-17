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
    /// <summary>
    /// Activity for the save button
    /// </summary>

    [Activity(Label = "Track list")]
    public class SaveApp : Activity
    {
        ListView lView;
        ArrayAdapter<string> adp;
        string[] trackList = { "Track 1", "Track 2" };
        string message;
        Button save;
        LinearLayout stack;
        SQLiteConnection database;

        protected override void OnCreate(Bundle b)
        {
            base.OnCreate(b);

            save = new Button(this);
            save.Text = "Click here to save the current track.";
            save.Click += saveCurrentTrack;

            message = Intent.GetStringExtra("message") ?? "Empty string";

            lView = new ListView(this);
            adp = new ArrayAdapter<string>(this, Android.Resource.Layout.SimpleSelectableListItem, trackList);
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
            string t = trackList[pos];
            Intent.PutExtra("item", t);

            Toast.MakeText(this, t, ToastLength.Short).Show();

            // StartActivity(i);
        }

        private void saveCurrentTrack(object sender, EventArgs ea)
        {

        }

        public void startSaving()
        {
            string docsFolder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
            string path = System.IO.Path.Combine(docsFolder, "tracks.db");
            bool exists = File.Exists(path);
            database = new SQLiteConnection(path);
            if (!exists)
            {
                database.CreateTable<string>();
                foreach(string s in trackList)
                {
                    database.Insert(s);
                }
            }
        }

        public void readTrack()
        {

        }
    }
}