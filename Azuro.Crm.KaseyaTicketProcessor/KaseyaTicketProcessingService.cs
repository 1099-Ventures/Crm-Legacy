using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Timers;
using Azuro.Common.WindowsService;
using Azuro.Common.Configuration;
using Azuro.Crm.KaseyaIntegration;
using System.ServiceProcess;
using Azuro.Crm.Integration;
using NLog;

namespace Azuro.Crm.KaseyaTicketProcessor
{

	[Description("Import tickets from Kaseya to CRM, Processes closed tickets from CRM. ")]
	[DisplayName("Azuro Crm Kaseya Integration")]
	[ServiceName("Azuro.Crm.KaseyaIntegration.KaseyaTicketProcessingService")]
	[ServiceStartType(ServiceStartMode.Manual)]
	public class KaseyaTicketProcessingService : WindowsServiceHelper<KaseyaTicketProcessingService>
	{
		private static Logger Logger = LogManager.GetCurrentClassLogger();

		private MSMQ.MSMQProcessor _processor;
		private MSMQ.MSMQProcessor Processor { get { return _processor ?? (_processor = new MSMQ.MSMQProcessor()); } }

		public static decimal _sessionIDMain = 0;
		
		private KaseyaConfigurationSection _config = null;
		private KaseyaConfigurationSection Config { get { return _config ?? (_config = ConfigurationHelper.GetSection<KaseyaConfigurationSection>(KaseyaConfigurationSection.SectionName) ?? new KaseyaConfigurationSection()); } }

		static void Main(string[] args)
		{
			Start(args);
		}
			
		protected override void PrintConsoleCommands()
		{
			Console.WriteLine("Type 'exit' to exit.");
		}

		protected override bool ProcessConsoleCommands()
		{
			while (Console.ReadLine().ToLower() != "exit") ;

			return false;
		}

		protected override void OnStart(string[] args)
		{
			try
			{
				base.OnStart(args);
				Processor.ProcessMessages();
				Logger.Trace("The message processor has been initiated.");
				ProcessImportTickets();
				Logger.Trace("The teckit import process has been initiated.");
				//--Testing purpose
				//ExecuteImportTask();
			}
			catch (Exception exc)
			{
				Logger.Error("Error starting service, {0}.", exc.Message);
				throw exc;
			}
		}

		private void ProcessImportTickets()
		{
			Logger.Trace("In the [ProcessImportTickets()].");
			Timer timer = new Timer();
			timer.Interval = (1000 * 60) * Config.KaseyaImportTimer;
			Logger.Trace("The timer will run after each {0} minutes.", ((timer.Interval/1000)/60).ToString());
			timer.Elapsed += new ElapsedEventHandler(ExecuteImportTask);
			timer.Start();
			Logger.Trace("Done [ProcessImportTickets()], timer is now started.");
	
		}

        private void ExecuteImportTask(object obj, ElapsedEventArgs args)
        {//Start

			try
			{
				Logger.Trace("Executing the Import function for Kaseya Tickets,  {0}", obj.ToString());
				KaseyaHelper kaseyaHelper = (KaseyaHelper)KaseyaHelperFactory.Create();
				Logger.Trace("Session ID at before validation {0}.", _sessionIDMain);
				kaseyaHelper.ValidateSessionID(ref _sessionIDMain);
				Logger.Trace("Session ID after validation {0}.", _sessionIDMain);
				CrmHelper helpre = ((CrmHelper)Azuro.Crm.Integration.CrmHelperFactory.Create(new Guid(kaseyaHelper.GetOrganisationID())));
				KaseyaCrmHelper crmhelper = new KaseyaCrmHelper(helpre);

				Logger.Trace("Organisation ID  to Import tickets to {0}", helpre.OrganisationId.ToString());

				List<KaseyaTicket> listOfTickets = null;
				int count = 0;
				try
				{
					listOfTickets = kaseyaHelper.GetEntityList<KaseyaTicket>(null);
				}
				catch (InvalidSessionIDException e)
				{
					Logger.Trace("This is an login error from Kaseya, {0}", e.Message);
					_sessionIDMain = 0;
					//If the session is invalid, recreate a new one and try and retrieve tickets.
					kaseyaHelper.ValidateSessionID(ref _sessionIDMain);
					listOfTickets = kaseyaHelper.GetEntityList<KaseyaTicket>(null);
					if (count == 5)
					{
						Logger.Error("The module could not log in to the Kaseya Server, please check the login details proved and the kaseya messages in the log, the is after trying for {0} amount of time.", count);
						return;
					}
					else
					{
						count++;

					}

				}
				catch (Exception exc)
				{
					Logger.Error("Error occured, {0}. The session ID will be recreated on the next request to Kaseya.", exc.Message);
					_sessionIDMain = 0;
					return;
				}

				if (listOfTickets.Count == 0)
				{
					Logger.Trace("The number of tickets retrieved is {0}.", listOfTickets.Count.ToString());
				}
				else
				{
					Logger.Trace("The number of tickets retrieved is {0}.", listOfTickets.Count.ToString());

					foreach (KaseyaTicket ticket in listOfTickets)
					{
						//Get CRM iformation
						KaseyaCrmInfo crmInfo = crmhelper.GetKaseyaInfo(ticket.GroupName);
						
						//If no data found, try and search. Search for first part of group name
						string[] searchStrings = ticket.GroupName.Split('.');
						foreach (string searchString in searchStrings)
						{
 
						}

						Guid accountID = crmInfo.AccountID.ReferencedEntityId;
						Logger.Trace("Case Account ID: {0}.", accountID.ToString());
						Guid contractID = crmInfo.ContractID.ReferencedEntityId;
						Logger.Trace("Case Contract ID: {0}.", contractID.ToString());
						Entities.ContractLine contractLine = crmhelper.GetContractDetail(contractID);
						Guid contractDetailID = contractLine.Id;
						Guid productID = contractLine.ProductId.ReferencedEntityId;
						Logger.Trace("Case Contract Details ID: {0}", contractDetailID.ToString());

						var convertedEntity = kaseyaHelper.ChangeEntityType<Entities.Case>(ticket);
						convertedEntity.AccountId.ReferencedEntityId = accountID;
						convertedEntity.ContractId.ReferencedEntityId = contractID;
						convertedEntity.ContractDetailId.ReferencedEntityId = contractDetailID;
						convertedEntity.ProductId.ReferencedEntityId = productID;
						Logger.Trace("Insert ticket to CRM ID: {0} ", convertedEntity.TicketNumber);
						crmhelper.Insert(convertedEntity);
					}
				}
			}
			catch (Exception exc)
			{
				Logger.Error("Issue trying to import tickets: {0}.", exc.Message);
			}
        }
		
	}//End

	[RunInstaller(true)]
	public class KaseyaTicketProcessingServiceInstaller : WindowsServiceInstaller<KaseyaTicketProcessingService>
	{}
}
