using System;
using System.Collections.Generic;
using Android.App;
using Android.Widget;
using Android.Views;

namespace AtlCodeCamp.Droid
{
	public class HomeScreenAdapter<T> : BaseAdapter<T>
	{
		public List<T> Data { get; set; }
		Activity context;

		public HomeScreenAdapter(Activity context)
		{
			this.context = context;
			Data = new List<T>();
		}

		public override long GetItemId(int position)
		{
			return position;
		}

		public override T this[int index] {
			get { return Data [index]; }
		}

		public override int Count {
			get { return Data.Count; }
		}

		public override View GetView(int position, View convertView, ViewGroup parent)
		{
			View view = convertView ?? 
				context.LayoutInflater.Inflate(Android.Resource.Layout.SimpleListItem1, null);
			view.FindViewById<TextView>(Android.Resource.Id.Text1).Text = Data[position].ToString();
			return view;
		}
	}
}

