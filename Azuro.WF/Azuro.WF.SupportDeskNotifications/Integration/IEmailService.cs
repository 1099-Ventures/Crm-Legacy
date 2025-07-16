using System;
using Azuro.Models;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;

namespace Azuro.CrmIntegration
{
    public interface IEmailService
    {
        Email SelectById(Guid id);

        OrganizationResponse Update(Email email);

        CreateResponse Insert(Email email);
    }
}
