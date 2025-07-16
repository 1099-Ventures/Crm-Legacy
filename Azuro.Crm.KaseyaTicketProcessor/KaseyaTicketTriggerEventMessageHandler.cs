using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Azuro.Common.Messaging;
using Azuro.Crm.Integration;
using Azuro.Crm.KaseyaIntegration;
using NLog;

namespace Azuro.Crm.KaseyaTicketProcessor
{
    public class KaseyaTicketTriggerEventMessageHandler : IMessageHandler<KaseyaTicketTriggerEventMessage>
    {
        private static Logger Logger = LogManager.GetCurrentClassLogger();

        public void ProcessMessage(KaseyaTicketTriggerEventMessage msg)
        {
            Logger.Trace("Processing message [KaseyaTicketTriggerEventMessage]");

            KaseyaHelper kaseyaHelper = (KaseyaHelper)KaseyaHelperFactory.Create();

            Logger.Trace("Creating CrmHelper for Organisation [{0}]", msg.OrganisationId);

            Logger.Trace("Get Entity from Crm [{0}]", msg.Incidentid);
            Logger.Trace("Session ID is {0}, before validation.", KaseyaTicketProcessingService._sessionIDMain.ToString());
            var incident = new KaseyaTicket();
            kaseyaHelper.ValidateSessionID(ref KaseyaTicketProcessingService._sessionIDMain);

            Logger.Trace("Session ID is {0}, after validation.", KaseyaTicketProcessingService._sessionIDMain.ToString());
            incident.TicketID = msg.ticketnumber;

            Logger.Trace("Ticket ID {0}.", incident.TicketID);
            int count = 0;
            try
            {
                kaseyaHelper.UpdateTicket(incident, null);
            }
            catch (InvalidSessionIDException e)
            {
                Logger.Trace(e.Message);
                KaseyaTicketProcessingService._sessionIDMain = 0;
                kaseyaHelper.ValidateSessionID(ref KaseyaTicketProcessingService._sessionIDMain);
                if (count == 3)
                {
                    Logger.Error("The module could not log in to the Kaseya Server, please check the login details proved and the kaseya messages in the log, the is after trying for {0} amount of time.", count);
                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Caught an unhandled exception");
                return;
            }

            Logger.Trace("Entity was updated successfully");
        }
    }
}
