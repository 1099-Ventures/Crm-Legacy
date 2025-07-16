using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk;
using System.ServiceModel.Description;

namespace Azuro.WF.TestApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Uri organizationUri = new Uri("http://ares.tangentsolutions.co.za:5555/azurodev/XRMServices/2011/Organization.svc");
                Uri homeRealmUri = null;

                var credentials = new ClientCredentials();
                credentials.Windows.ClientCredential = new System.Net.NetworkCredential("Dave", "T$D@v$123", "Tangent");

                var serviceProxy = new OrganizationServiceProxy(organizationUri, homeRealmUri, credentials, null);
                var service = (IOrganizationService)serviceProxy;

                //var activty = new SupportDeskNotifications.NotificationActivity();
                //activty.Execute(new Guid("E578A960-AD00-E211-B3F8-00155D179A04"), service);

                //var activity = new EmailToCase.EmailToCaseActivity();
                //activity.Execute(new Guid("24A60A56-5D03-E211-B3F8-00155D179A04"), service);

                var activity = new OnCreateEmail.OnCreateActivity();
                activity.Execute(new Guid("24A60A56-5D03-E211-B3F8-00155D179A04"), service);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
            finally
            {
                Console.WriteLine("Finished work");
                Console.ReadLine();
            }
        }
    }
}
