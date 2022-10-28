using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
    public class vmUserCredential : BaseClass
    {
        public vmUserCredential()
        {
            this.ErrorType = 0;
            this.ErrorMessage = "";
        }
        public vmUserCredential(int errorType, string errorMessage)
        {
            this.ErrorType = errorType;
            this.ErrorMessage = errorMessage;
        }
        public string UserID { get; set; }
        public int UserRoleID { get; set; }
    }
}
