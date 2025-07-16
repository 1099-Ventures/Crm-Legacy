using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Azuro.Models;
using Microsoft.Xrm.Sdk;

namespace Azuro.CrmIntegration
{
    public interface IQueueItemService
    {
        QueueItem SelectById(Guid id);

        QueueItem GetQueueByName(string name);

        OrganizationResponse Insert(QueueItem entity);

        OrganizationResponse Delete(Guid id);
    }
}
