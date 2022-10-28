using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace ARSystemFrontEnd.Providers
{
    public class ARPrincipal : IPrincipal
    {
        public ARPrincipal(IIdentity identity)
        {
            Identity = identity;
        }

        public IIdentity Identity
        {
            get;
            private set;
        }

        public SecureAccessService.vmUserLogin User { get; set; }

        public bool IsInRole(string role)
        {
            return true;
        }
    }
}