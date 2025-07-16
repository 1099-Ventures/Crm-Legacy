using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.ServiceModel.Description;
using Microsoft.Xrm.Sdk.Client;
using Microsoft.Xrm.Sdk;
using Azuro.CrmIntegration.CRM;
using Azuro.Models;

namespace Azuro.WF.CreateIncomingEmail
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                Uri organizationUri = new Uri("http://ares.tangentsolutions.co.za:5555/azurodev/XRMServices/2011/Organization.svc");
                Uri homeRealmUri = null;

                var credentials = new ClientCredentials();
                credentials.Windows.ClientCredential = new System.Net.NetworkCredential("Dave", "T$D@v$123", "Tangent");

                var serviceProxy = new OrganizationServiceProxy(organizationUri, homeRealmUri, credentials, null);
                var service = (IOrganizationService)serviceProxy;

                var emailService = new EmailService(service);

                var email = new Email();
                email.OwnerId = new Guid(txtOwner.Text);
                email.Subject = txtSubject.Text;
                email.Description = txtDesc.Text;
                email.From = txtFrom.Text;
                email.FromType = "contact";
                email.To = txtTo.Text;
                email.ToType = "queue";

                emailService.Insert(email);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message + " - " + ex.StackTrace);
            }
        }
    }
}
