using Azuro.Common.Configuration;
using Azuro.Common.Messaging;
using Azuro.Crm.Integration.Nable.Entities;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Azuro.Crm.Integration.Nable
{
	public class CaseResolvedMessageHandler : IMessageHandler<CaseResolvedNotification>
	{
		private static Logger Logger = LogManager.GetCurrentClassLogger();
	
		private NableConfigurationSection _config;
		private NableConfigurationSection Config
		{
			get { return _config ?? (_config = ConfigurationHelper.GetSection<NableConfigurationSection>(NableConfigurationSection.SectionName)); }
		}

		public void ProcessMessage(CaseResolvedNotification msg)
		{
			//	Acknowledge the N-Able Alert
			var svc = new Nable.ServerEIClient();
			var response = svc.AcknowledgeNotification(new Nable.AcknowledgeNotificationRequest(new int[] { msg.ExternalReference, },
																									new int[] { },
																									Config.NableLoginDetails.UserName,
																									Config.NableLoginDetails.Password, 
																									msg.CaseNumber + "[" + msg.CaseStatus + "] - " + msg.ResolutionDescription,
																									true,
																									false));
			//	Response is an empty document, nothing to do here.
			Logger.Trace("NAble Case [{0}] Acknowledged.", msg.CaseNumber);
		}
	}
}
