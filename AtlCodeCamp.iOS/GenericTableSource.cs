using System;
using System.Collections.Generic;
using System.Drawing;
using MonoTouch.Foundation;
using MonoTouch.UIKit;

namespace AtlCodeCamp.iOS
{

	public class GenericTableSource<T> : UITableViewSource
	{
		protected string CellIdentifier = "GenericTableCell";

		public List<T> Data { get; set; }

		public GenericTableSource()
		{
			Data = new List<T>();
		}

		public override int RowsInSection(UITableView tableview, int section)
		{
			return Data.Count;
		}

		public override UITableViewCell GetCell(UITableView tableView, MonoTouch.Foundation.NSIndexPath indexPath)
		{
			UITableViewCell cell = tableView.DequeueReusableCell(CellIdentifier) ??
				new UITableViewCell(UITableViewCellStyle.Default, CellIdentifier);
			cell.TextLabel.Text = Data[indexPath.Row].ToString();
			return cell;
		}

		public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
		{
			new UIAlertView("Row Selected", Data[indexPath.Row].ToString(), null, "OK", null).Show();
			tableView.DeselectRow (indexPath, true);
		}
	}
}
