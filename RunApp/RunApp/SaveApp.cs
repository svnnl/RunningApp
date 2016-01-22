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
        TrackAdapter trackAdapter;
        List<TrackItem> trackList;
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

            message = Intent.GetStringExtra("message") ?? "";

            lView = new ListView(this);
            lView.ChoiceMode = ChoiceMode.None;
            lView.ItemClick += click;
            this.readTrack();

            stack = new LinearLayout(this);
            stack.Orientation = Orientation.Vertical;
            stack.AddView(save);
            stack.AddView(lView);

            SetContentView(stack);
        }

        /// <summary>
        /// Event on click of a List Item
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void click(object sender, AdapterView.ItemClickEventArgs e)
        {
            int pos = e.Position;
            Intent i = new Intent(this, typeof(AnalyzeApp));
            // Position of item
            string t = trackList[pos].value;
            Intent.PutExtra("message", t);

            StartActivity(i);
        }

        /// <summary>
        /// Event on click the Save button
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="ea"></param>
        private void saveCurrentTrack(object sender, EventArgs ea)
        {
            if (message != "")
            {
                database.Insert(new TrackItem("track", message));
                trackList.Add(new TrackItem("track", message));
            }
        }

        protected virtual void startSaving()
        {
            string docsFolder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
            string path = System.IO.Path.Combine(docsFolder, "tracks.db");
            bool exists = File.Exists(path);
            database = new SQLiteConnection(path);
            if (!exists)
            {
                database.CreateTable<TrackItem>();
                foreach (TrackItem t in trackList)
                {
                    database.Insert(t);
                }
            }
        }

        protected virtual void readTrack()
        {
            trackList = new List<TrackItem>();
            TableQuery<TrackItem> query = database.Table<TrackItem>();
            foreach(TrackItem t in query)
            {
                trackList.Add(t);
            }
            trackAdapter = new TrackAdapter(this, trackList);
            lView.Adapter = trackAdapter;
        }
    }
}