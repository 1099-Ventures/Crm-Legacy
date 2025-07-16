using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Azuro.Models;
using Microsoft.Xrm.Sdk;

namespace Azuro.CrmIntegration
{
    public interface ISupportNotificationService
    {
        SupportNotification SelectById(Guid id);

        List<SupportNotification> SelectByContractLine(Guid id);
    }
}
