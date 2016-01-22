using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace RunApp
{
    public class TrackAdapter : BaseAdapter<TrackItem>
    {
        IList<TrackItem> items;
        Activity context;

        public TrackAdapter(Activity context, IList<TrackItem> items)
        {
            this.context = context;
            this.items = items;
        }

        public override long GetItemId(int position)
        {
            return position;
        }

        public override TrackItem this[int position]
        {
            get
            {
                return items[position];
            }
        }

        public override int Count
        {
            get
            {
                return items.Count;
            }
        }

        public override View GetView(int position, View convert, ViewGroup root)
        {
            var item = items[position];
            TextView view = (TextView)(convert ?? context.LayoutInflater.Inflate(Android.Resource.Layout.SimpleListItem1, null));
            view.Text = $"{item.Id}: {item.name}";

            return view;
        }
    }
}