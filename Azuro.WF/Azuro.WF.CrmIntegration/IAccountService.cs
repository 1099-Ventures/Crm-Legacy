using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Azuro.Models;

namespace Azuro.CrmIntegration
{
    public interface IAccountService
    {
        Account SelectById(Guid id);

        Account SelectByName(string name);

        Account SelectByEmail(string email);

        Account SelectByEmailId(Guid id);
    }
}
