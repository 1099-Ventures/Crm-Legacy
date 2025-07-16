using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Azuro.Models;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;

namespace Azuro.CrmIntegration
{
    public interface ICaseService
    {
        Case SelectById(Guid id);

        Case SelectByReference(string reference);

        OrganizationResponse Update(Case entity);

        CreateResponse Insert(Case entity);
    }
}
