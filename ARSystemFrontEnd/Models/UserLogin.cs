using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystemFrontEnd.Models
{
    public class UserLogin
    {
        public string UserFullName { get; set; }
        public string UserToken { get; set; }
        public string UserRole { get; set; }
        public string Application { get; set; }
        public string CompanyCode { get; set; }
        public bool IsPasswordExpired { get; set; }
    }
}
