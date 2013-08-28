using System;
using RestSharp;
using System.Collections.Generic;

namespace AtlCodeCamp
{

	public class Question
	{
		public string title
		{
			get;
			set;
		}

		public override string ToString()
		{
			return title;
		}
	}

	public interface IDataHandler
	{
		void LoadData();
	}

	public class BaseDataHandler : IDataHandler
	{
		public virtual void LoadData()
		{
			if (!IsReachable ()) {
				ShowUIUnreachble ();
				return;
			}

			var request = new RestRequest {RootElement = "items", Resource = "/questions/featured"};
			request.AddParameter("site", "stackoverflow");

			var client = new RestClient("http://api.stackexchange.com/2.1");
			client.ExecuteAsync<List<Question>> (request, response => {
				OnReceivedData(response);
			});
		}

		public virtual void OnReceivedData (IRestResponse<List<Question>> response)
		{
		}

		public virtual bool IsReachable()
		{
			return true;
		}

		public virtual void ShowUIUnreachble()
		{
		}
	}
}
