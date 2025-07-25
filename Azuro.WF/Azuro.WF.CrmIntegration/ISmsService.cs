﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Azuro.Models;
using Microsoft.Xrm.Sdk;

namespace Azuro.CrmIntegration
{
    public interface ISmsService
    {
        Sms SelectById(Guid id);

        List<Sms> SelectUnsent();

        OrganizationResponse Insert(Sms entity);

        OrganizationResponse Update(Sms entity);
    }
}
