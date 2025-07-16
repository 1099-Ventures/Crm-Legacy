using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Azuro.CrmIntegration.Exceptions
{
    public class OrganisationServiceException : Exception
    {
        #region Constructors

        public OrganisationServiceException()
            : base()
        { }

        public OrganisationServiceException(string message)
            : base(message)
        { }

        #endregion
    }
}
