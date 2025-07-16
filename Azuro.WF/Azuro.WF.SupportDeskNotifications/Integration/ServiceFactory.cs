using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xrm.Sdk;
using Azuro.CrmIntegration.Exceptions;

namespace Azuro.CrmIntegration
{
    public class ServiceFactory
    {
        #region Private Members

        private string version = "2011";

        private static ServiceFactory _instance = null;

        #endregion

        #region Public Methods

        public string Version { get; set; }

        public IOrganizationService OrganizationService { get; set; }

        public static ServiceFactory Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new ServiceFactory();

                return _instance;
            }
        }

        public IExclusionTimeService ExclusionTimeService
        {
            get
            {
                if (OrganizationService == null)
                    throw new OrganisationServiceException("Organisation service has not been set");

                if (string.IsNullOrEmpty(version) || version == "2011")
                    return new CRM.ExclusionTimeService(OrganizationService);
                else
                    return new CRM.ExclusionTimeService(OrganizationService);
            }
        }

        public IPublicHolidayService PublicHolidayService
        {
            get
            {
                if (OrganizationService == null)
                    throw new OrganisationServiceException("Organisation service has not been set");

                if (string.IsNullOrEmpty(version) || version == "2011")
                    return new CRM.PublicHolidayService(OrganizationService);
                else
                    return new CRM.PublicHolidayService(OrganizationService);
            }
        }

        public ISmsService SmsService
        {
            get
            {
                if (OrganizationService == null)
                    throw new OrganisationServiceException("Organisation service has not been set");

                if (string.IsNullOrEmpty(version) || version == "2011")
                    return new CRM.SmsService(OrganizationService);
                else
                    return new CRM.SmsService(OrganizationService);
            }
        }

        public IEmailService EmailService
        {
            get
            {
                if (OrganizationService == null)
                    throw new OrganisationServiceException("Organisation service has not been set");

                if (string.IsNullOrEmpty(version) || version == "2011")
                    return new CRM.EmailService(OrganizationService);
                else
                    return new CRM.EmailService(OrganizationService);
            }
        }

        public ICaseService CaseService
        {
            get
            {
                if (OrganizationService == null)
                    throw new OrganisationServiceException("Organisation service has not been set");

                if (string.IsNullOrEmpty(version) || version == "2011")
                    return new CRM.CaseService(OrganizationService);
                else
                    return new CRM.CaseService(OrganizationService);
            }
        }

        public IContactService ContactService
        {
            get
            {
                if (OrganizationService == null)
                    throw new OrganisationServiceException("Organisation service has not been set");

                if (string.IsNullOrEmpty(version) || version == "2011")
                    return new CRM.ContactService(OrganizationService);
                else
                    return new CRM.ContactService(OrganizationService);
            }
        }

        public IAccountService AccountService
        {
            get
            {
                if (OrganizationService == null)
                    throw new OrganisationServiceException("Organisation service has not been set");

                if (string.IsNullOrEmpty(version) || version == "2011")
                    return new CRM.AccountService(OrganizationService);
                else
                    return new CRM.AccountService(OrganizationService);
            }
        }

        public IUserService UserService
        {
            get
            {
                if (OrganizationService == null)
                    throw new OrganisationServiceException("Organisation service has not been set");

                if (string.IsNullOrEmpty(version) || version == "2011")
                    return new CRM.UserService(OrganizationService);
                else
                    return new CRM.UserService(OrganizationService);
            }
        }

        public IQueueItemService QueueItemService
        {
            get
            {
                if (OrganizationService == null)
                    throw new OrganisationServiceException("Organisation service has not been set");

                if (string.IsNullOrEmpty(version) || version == "2011")
                    return new CRM.QueueItemService(OrganizationService);
                else
                    return new CRM.QueueItemService(OrganizationService);
            }
        }

        public IQueueService QueueService
        {
            get
            {
                if (OrganizationService == null)
                    throw new OrganisationServiceException("Organisation service has not been set");

                if (string.IsNullOrEmpty(version) || version == "2011")
                    return new CRM.QueueService(OrganizationService);
                else
                    return new CRM.QueueService(OrganizationService);
            }
        }

        #endregion
    }
}
