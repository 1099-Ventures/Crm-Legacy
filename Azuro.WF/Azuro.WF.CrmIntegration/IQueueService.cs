using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Azuro.Models;
using Microsoft.Xrm.Sdk;

namespace Azuro.CrmIntegration
{
    public interface IQueueService
    {
        Queue SelectById(Guid id);

        Queue GetQueueByName(string name);
    }
}
