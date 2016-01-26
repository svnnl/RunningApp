using System;
using System.Collections.Generic;
using System.IO;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Graphics;
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
        List<DataPoint> track = new List<DataPoint>();
        string message;
        Button save;
        LinearLayout stack;
        SQLiteConnection database;

        protected override void OnCreate(Bundle b)
        {
            base.OnCreate(b);
            this.startSaving();

            save = new Button(this);
            save.Text = "Click here to save the current track.";
            save.Click += saveCurrentTrack;

            message = Intent.GetStringExtra("message") ?? "";

            lView = new ListView(this);
            lView.ChoiceMode = ChoiceMode.None;
            lView.ItemClick += analyze;     // Analyzes with a single click
            lView.ItemLongClick += delete;  // Deletes when item is selected for a longer time
            this.readTrack();

            stack = new LinearLayout(this);
            stack.Orientation = Orientation.Vertical;
            stack.AddView(save);
            stack.AddView(lView);

            SetContentView(stack);
        }

        /// <summary>
        /// Analyzes the selected item
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void analyze(object sender, AdapterView.ItemClickEventArgs e)
        {
            int pos = e.Position;
            Intent i = new Intent(this, typeof(AnalyzeApp));

            string message = trackAdapter[pos].value;
            i.PutExtra("message", message);
            Toast.MakeText(this, $"Track {trackAdapter[pos].Id} will now be analyzed.", ToastLength.Short).Show();
            StartActivity(i);
        }

        /// <summary>
        /// Deletes the item that has been long clicked
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void delete(object sender, AdapterView.ItemLongClickEventArgs e)
        {
            int pos = e.Position;

            AlertDialog.Builder alert = new AlertDialog.Builder(this);
            alert.SetTitle("Delete item")
            .SetMessage("Are you sure you want to delete this track?")
            .SetCancelable(false)
            .SetPositiveButton("Yes", (object o, DialogClickEventArgs d) =>
            {
                database.Delete(trackAdapter[pos]); // Deletes the item that has been selected
                Toast.MakeText(this, $"Track {trackAdapter[pos].Id} has been deleted.", ToastLength.Short).Show();
                this.readTrack();
            })
            .SetNegativeButton("No", (object o, DialogClickEventArgs d) =>
            {
                // Do nothing
            })
            .Show();

        }

        /// <summary>
        /// Saves the active track from the RunningView class
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="ea"></param>
        private void saveCurrentTrack(object sender, EventArgs ea)
        {
            if (message != "")
            {
                database.Insert(new TrackItem("Track", message));
                Toast.MakeText(this, $"Track has been saved as Track {trackAdapter.Count + 1}", ToastLength.Short).Show();
                this.readTrack();
            }
            else
                Toast.MakeText(this, "There is no active track to save.", ToastLength.Short).Show();
        }

        /// <summary>
        /// Creates a database file and stores dummy track in it
        /// </summary>
        protected virtual void startSaving()
        {
            string docsFolder = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
            string path = System.IO.Path.Combine(docsFolder, "tracks.db");
            bool exists = File.Exists(path);
            database = new SQLiteConnection(path);
            if (!exists)
            {
                database.CreateTable<TrackItem>();
                track.Clear();
                track.Add(new DataPoint(new PointF(139191.1f, 455584.9f), new TimeSpan(0, 0, 0, 1, 734)));
                track.Add(new DataPoint(new PointF(139175.0f, 455596.5f), new TimeSpan(0, 0, 0, 4, 936)));
                track.Add(new DataPoint(new PointF(139156.3f, 455614.8f), new TimeSpan(0, 0, 0, 5, 736)));
                track.Add(new DataPoint(new PointF(139135.8f, 455632.2f), new TimeSpan(0, 0, 0, 7, 733)));
                track.Add(new DataPoint(new PointF(139114.9f, 455652.7f), new TimeSpan(0, 0, 0, 9, 780)));
                track.Add(new DataPoint(new PointF(139095.4f, 455675.7f), new TimeSpan(0, 0, 0, 11, 772)));
                track.Add(new DataPoint(new PointF(139078.4f, 455702.9f), new TimeSpan(0, 0, 0, 13, 733)));
                track.Add(new DataPoint(new PointF(139064.9f, 455730.5f), new TimeSpan(0, 0, 0, 15, 752)));
                track.Add(new DataPoint(new PointF(139053.2f, 455760.7f), new TimeSpan(0, 0, 0, 17, 756)));
                track.Add(new DataPoint(new PointF(139041.3f, 455790.4f), new TimeSpan(0, 0, 0, 19, 751)));
                track.Add(new DataPoint(new PointF(139028.9f, 455821.0f), new TimeSpan(0, 0, 0, 21, 732)));
                string s = RunningView.TrackToString(track);
                database.Insert(new TrackItem("Dummy track", s));
                track.Clear();
            }
        }

        /// <summary>
        /// Reads and updates the adapter
        /// </summary>
        protected virtual void readTrack()
        {
            List<TrackItem> trackList = new List<TrackItem>();
            TableQuery<TrackItem> query = database.Table<TrackItem>();
            foreach (TrackItem t in query)
            {
                trackList.Add(t);
            }
            trackAdapter = new TrackAdapter(this, trackList);
            lView.Adapter = trackAdapter;
        }
    }
}