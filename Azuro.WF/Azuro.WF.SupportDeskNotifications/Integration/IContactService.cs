using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Azuro.Models;

namespace Azuro.CrmIntegration
{
    public interface IContactService
    {
        Contact SelectById(Guid id);

        Contact SelectByFullName(string fullName);

        Contact SelectByEmail(string email);

        Contact SelectByEmailId(Guid id);
    }
}
