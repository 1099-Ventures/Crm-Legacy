using System;
using Azuro.Models;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;

namespace Azuro.CrmIntegration
{
    public interface INoteService
    {
        Note SelectById(Guid id);

        OrganizationResponse Update(Note note);

        CreateResponse Insert(Note note);
    }
}
