using System;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Collections.Generic;
using RestSharp;

namespace AtlCodeCamp.iOS
{	
	public class UITableViewDataHandler<T> : BaseDataHandler
	{
		UITableViewController _controller;

		public UITableViewDataHandler (UITableViewController controller)
		{
			_controller = controller;
			_controller.TableView.Source = new GenericTableSource<T> ();
		}

		public override bool IsReachable ()
		{
			return Reachability.RemoteHostStatus () != NetworkStatus.NotReachable;
		}

		public override void ShowUIUnreachble ()
		{
			_controller.InvokeOnMainThread(() =>
				new UIAlertView("Network Message", "You need to have an internet connection", null, "Close", null).Show());
		}

		public override void OnReceivedData (IRestResponse<List<Question>> response)
		{
			_controller.InvokeOnMainThread (delegate {
				if (response.ErrorException == null) {
					((GenericTableSource<Question>)_controller.TableView.Source).Data = response.Data;
					_controller.TableView.ReloadData ();
				} else {
					_controller.InvokeOnMainThread (() =>
					                               new UIAlertView ("Network Message", "You need to have an internet connection", null, "Close", null).Show ());
				}
				if (_controller.RefreshControl.Refreshing) {
					_controller.RefreshControl.EndRefreshing ();
				}
			});
		}
	}
}
