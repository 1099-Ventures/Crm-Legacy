using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Azuro.Models;

namespace Azuro.CrmIntegration
{
    public interface IContractService
    {
        Contract SelectContractById(Guid id);

        Contract SelectContractByCustomerId(Guid id);
    }
}
