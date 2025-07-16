using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Azuro.Models;
using Microsoft.Xrm.Sdk;

namespace Azuro.CrmIntegration
{
    public interface IPublicHolidayService
    {
        PublicHoliday SelectById(Guid id);

        List<PublicHoliday> SelectAll();

        OrganizationResponse Insert(PublicHoliday entity);

        OrganizationResponse Update(PublicHoliday entity);
    }
}
