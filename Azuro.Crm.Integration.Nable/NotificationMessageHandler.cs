using Azuro.Common.Configuration;
using Azuro.Common.Messaging;
using Azuro.Crm.Entities;
using Azuro.Crm.Integration.Nable.Entities;
using NLog;
using System;

namespace Azuro.Crm.Integration.Nable
{
    public class NotificationMessageHandler : IMessageHandler<Notification>
    {
        private static Logger Logger = LogManager.GetCurrentClassLogger();

        private NableConfigurationSection _config;
        private NableConfigurationSection Config
        {
            get { return _config ?? (_config = ConfigurationHelper.GetSection<NableConfigurationSection>(NableConfigurationSection.SectionName)); }
        }

        private ICrmHelper _crmHelper;
        private ICrmHelper CrmHelper
        {
            get { return _crmHelper ?? (_crmHelper = CrmHelperFactory.Create(Config.OrganizationId)); }
        }

        public void ProcessMessage(Notification msg)
        {
            try
            {
                Logger.Trace("Entering Azuro.Crm.Integration.Nable.ProcessMessage");
                Logger.Trace(Azuro.Common.Serialization.Serializer.SerializeToXml(msg));

                //	Lookup customer (if possible)
                //	Identify Contract, Default severity
                var account = CrmHelper.GetEntity<Account>("accountnumber", msg.ExternalCustomerID);
                if (account == null && Config.DefaultAccount != Guid.Empty)
                    account = CrmHelper.GetEntity<Account>(Config.DefaultAccount);

                //	Unpack notification and create CRM Case
                var caseItem = new Case
                {
                    Title = string.Format("N-Able Generated - {0} - {1} - {2}", msg.CustomerName, msg.DeviceName, msg.AffectedService),
                    Description = string.Format("Qualitative Old State: {1}{0}Qualitative New State:{2}{0}Quantitative New State: {3}", Environment.NewLine, msg.QualitativeOldState, msg.QualitativeNewState, msg.QuantitativeNewState),
                    ExternalSystemReference = msg.ActiveNotificationTriggerID,
                    Severity = Config.DefaultSeverity,
                };

                //	Create ownerId for case based on running user?
                User user = null;
                if (!string.IsNullOrEmpty(Config.CrmUser))
                {
                    Logger.Trace("Retrieving User with Id: [{0}]", Config.CrmUser);
                    user = CrmHelper.GetEntity<User>("domainname", Config.CrmUser);
                }

                if (user == null)
                {
                    Logger.Trace("User is null.");
                    user = CrmHelper.GetEntity<User>("domainname", Environment.UserName);
                    if (user == null)
                        Logger.Trace("User is still null.");
                }
#if DEBUG
                if (user == null)
                    user = CrmHelper.GetEntity<User>("domainname", "azurodev\\-admin-johannu");
                if (user == null)
                    user = CrmHelper.GetEntity<User>("domainname", "-admin-johannu");
#endif
                var caseOrigin = CrmHelper.GetOptionSetValueForText("incident_caseorigincode", "NAble");
                if (caseOrigin.HasValue)
                    caseItem.CaseOriginCode = caseOrigin.Value;

                var caseHelper = new CaseHelper(CrmHelper);
                Logger.Trace("CaseHelper Loaded: [{0}]", caseHelper != null);
                if (!caseHelper.CreateCase(account, null, caseItem, new CrmEntityReference(User.LogicalName, user.FullName, user.Id)))
                    Logger.Error("Unable to create case.");
                else
                    Logger.Trace("Case successfully created");
            }
            catch (Exception ex)
            {
                Logger.Error(ex, "Exception processing Msmq message");
            }
        }
    }
}
