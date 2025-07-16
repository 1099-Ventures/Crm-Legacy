using Azuro.Common.WindowsService;
using NLog;
using System;
using System.ComponentModel;
using System.ServiceProcess;

namespace Azuro.Crm.SmsProcessor
{
	[Description("Processes incoming and outgoing SMS messages")]
	[DisplayName("Azuro SMS Processing Service")]
	[ServiceName("Azuro.Crm.SmsProcessor")]
	[ServiceStartType(ServiceStartMode.Automatic)]
	public class SmsProcessingService : Azuro.Common.WindowsService.WindowsServiceHelper<SmsProcessingService>
	{
		private static Logger Logger = LogManager.GetCurrentClassLogger();

		private MSMQ.MSMQProcessor m_processor;
		private MSMQ.MSMQProcessor Processor { get { return m_processor ?? (m_processor = new MSMQ.MSMQProcessor()); } }

		static void Main(string[] args)
		{
#if TrialBuild
			if (DateTime.Now > System.IO.File.GetCreationTime(System.Reflection.Assembly.GetExecutingAssembly().Location).AddDays(30))
			{
				Logger.Error("Trial period has expired. Please contact Azuro for a full version build.");
				return;
			}
#endif
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
			base.OnStart(args);

			Logger.Trace("Starting SMS Processor.");

			Processor.ProcessMessages();
		}
	}

	[RunInstaller(true)]
	public class SmsProcessingServiceInstaller : Azuro.Common.WindowsService.WindowsServiceInstaller<SmsProcessingService> { }
}
