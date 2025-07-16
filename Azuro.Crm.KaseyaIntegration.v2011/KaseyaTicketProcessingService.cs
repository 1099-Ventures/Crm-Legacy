using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Azuro.Common.WindowsService;
using System.ServiceProcess;
using System.ComponentModel;

namespace Azuro.Crm.KaseyaIntegration
{
	[Description("Imports and tickets from Kaseya to CRM, Processes closed tickets from CRM. ")]
	[DisplayName("Azuro Kaseya Tickets Processing Service")]
	[ServiceName("Azuro.Crm.KaseyaIntegration.KaseyaTicketProcessingService")]
	[ServiceStartType(ServiceStartMode.Automatic)]
	public class KaseyaTicketProcessingService : WindowsServiceHelper<KaseyaTicketProcessingService>
	{
        protected override void PrintConsoleCommands()
        {
            throw new NotImplementedException();
        }

        protected override void ProcessCommandLine(string[] args)
        {
            base.ProcessCommandLine(args);
        }
	}

	public class KaseyaTicketProcessingServiceInstaller : WindowsServiceInstaller<KaseyaTicketProcessingService>
	{
 
	}
}
