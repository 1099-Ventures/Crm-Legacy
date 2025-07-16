using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Messages;
using Microsoft.Xrm.Sdk.Query;
using Azuro.Models;

namespace Azuro.Integration
{
    public interface IAttachmentService
    {
        Attachment SelectById(Guid id);

        List<Attachment> SelectByEmailId(Guid id);

        OrganizationResponse Insert(Attachment entity);
    }
}
