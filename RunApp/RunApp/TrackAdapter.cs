using System.Collections.Generic;

using Android.App;
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
            view.Text = $"{item.name} {item.Id}";

            return view;
        }
    }
}