using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
    public class vmBAPSValidationUpdate : BaseClass
    {
        public vmBAPSValidationUpdate()
        {
            this.ErrorType = 0;
            this.ErrorMessage = "";
        }
        public vmBAPSValidationUpdate(int errorType, string errorMessage)
        {
            this.ErrorType = errorType;
            this.ErrorMessage = errorMessage;
        }

        public trxSSRDropFO SSRDropFO { get; set; }
    }
}