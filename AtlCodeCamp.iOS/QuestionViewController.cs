using System;
using System.Collections.Generic;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace AtlCodeCamp.iOS
{
	public class QuestionViewController : UITableViewController
	{
		IDataHandler _handler;

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			
			TableView.Source = new GenericTableSource<Question>();

			_handler = new UITableViewDataHandler<Question> (this);

			RefreshControl = new UIRefreshControl();
			RefreshControl.ValueChanged += (sender, e) => _handler.LoadData ();

			_handler.LoadData();
		}
	}
}

