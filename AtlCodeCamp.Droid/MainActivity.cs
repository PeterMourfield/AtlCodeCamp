using System;
using System.Collections.Generic;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using RestSharp;

namespace AtlCodeCamp.Droid
{
	[Activity (Label = "AtlCodeCamp.Droid", MainLauncher = true)]
	public class MainActivity : ListActivity
	{
		IDataHandler _handler;

		protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			ListAdapter = new HomeScreenAdapter<Question>(this);

			_handler = new ListActivityDataHandler<Question> (this);

			_handler.LoadData ();
		}
	}

	public class ListActivityDataHandler<T> : BaseDataHandler
	{
		ListActivity _activity;

		public ListActivityDataHandler(ListActivity activity)
		{
			_activity = activity;
		}

		public override void OnReceivedData (IRestResponse<List<Question>> response)
		{
			if (response.ErrorException == null) {
				_activity.RunOnUiThread (() => {
					((HomeScreenAdapter<Question>)_activity.ListAdapter).Data = response.Data;
					((HomeScreenAdapter<Question>)_activity.ListAdapter).NotifyDataSetChanged ();
				});
			} else {
			}
		}
	}
}


