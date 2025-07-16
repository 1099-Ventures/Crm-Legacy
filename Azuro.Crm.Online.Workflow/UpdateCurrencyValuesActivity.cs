using Azuro.Crm.Online.Integration;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;
using Microsoft.Xrm.Sdk.Workflow;
using System;
using System.Activities;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

namespace Azuro.Crm.Online.Workflow
{
	public sealed class UpdateCurrencyValuesActivity : ACrmCodeActivity
	{
		[Input("Currency API Key")]
		[RequiredArgument]
		public InArgument<string> APIKey { get; set; }

		[Input("Base Currency")]
		public InArgument<string> BaseCurrency { get; set; }

		[Input("Save History")]
		public InArgument<bool> SaveHistory { get; set; }

		[Input("History Service Url")]
		public InArgument<string> SaveSvcUrl { get; set; }

		protected override void OnExecute(CodeActivityContext context)
		{
			//	Connect to currency service
			var baseCurrency = BaseCurrency.Get(context);
			var saveHistory = SaveHistory.Get(context);
			var svcUrl = SaveSvcUrl.Get(context);

			var currencyRates = FetchCurrencyRates(APIKey.Get(context), baseCurrency);
			//	Retrieve all configured Currencies
			var crmCurrencies = FetchCurrencies(context);
			//	Update all currencies with located Currency Conversion Value
			foreach (var c in crmCurrencies)
			{
				var currency = c.Attributes["isocurrencycode"].ToString();
				if (string.Compare(currency, baseCurrency ?? "USD", true) == 0)
					continue;

				var rate = currencyRates.FirstOrDefault(r => string.Compare(r.Key, currency, true) == 0);
				if (rate.Value != 0.0m && rate.Value != 1.0m) //	Exclude rate of 0 (not found) and 1 (equal)
				{
					c.Attributes["exchangerate"] = rate.Value;
					GetOrganizationService(context).Update(c);
					if (saveHistory && !string.IsNullOrEmpty(svcUrl))
						SaveHistoryToDb(context, svcUrl, currency, rate.Value);
				}
			}
		}

		private DataCollection<Entity> FetchCurrencies(CodeActivityContext context)
		{
			var qe = new QueryExpression();
			qe.EntityName = "transactioncurrency";
			qe.ColumnSet = new ColumnSet { AllColumns = true };

			var ret = GetOrganizationService(context).RetrieveMultiple(qe);
			return ret.Entities;
		}

		private Dictionary<string, decimal> FetchCurrencyRates(string apiKey, string baseCurrency)
		{
			string url = string.Format("https://openexchangerates.org/api/latest.json?app_id={0}{1}", apiKey,
				string.IsNullOrWhiteSpace(baseCurrency) || string.Compare(baseCurrency, "USD", true) == 0 ? string.Empty : string.Format("&base={0}", baseCurrency));

			var response = HttpHelper.Get(url);

			if (response.StatusCode != HttpStatusCode.OK)
				throw new InvalidOperationException(string.Format("Unable to fetch Currencies using Url: {0} - {1}|{2}", url, response.StatusCode, response.StatusDescription));

			var currencyResponse = new CurrencyResponse(response.Data);

			return currencyResponse.Rates;
		}

		private void SaveHistoryToDb(CodeActivityContext context, string svcUrl, string currency, decimal value)
		{
			var json = string.Format("{{currency:\"{1}{0}rate:\"{2}{0}}}", Environment.NewLine, currency, value);
			var response = HttpHelper.Put(svcUrl, json);

			if (response.StatusCode != HttpStatusCode.OK)
				Trace(context, "Failed to Save Currency History {0}", response.StatusCode);
		}
	}
}
