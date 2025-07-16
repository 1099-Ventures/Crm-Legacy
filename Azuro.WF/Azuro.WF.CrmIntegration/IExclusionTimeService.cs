using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Azuro.Models;
using Microsoft.Xrm.Sdk;

namespace Azuro.CrmIntegration
{
    public interface IExclusionTimeService
    {
        ExclusionTime SelectById(Guid id);

        List<ExclusionTime> SelectAll();

        OrganizationResponse Insert(ExclusionTime entity);

        OrganizationResponse Update(ExclusionTime entity);
    }
}
