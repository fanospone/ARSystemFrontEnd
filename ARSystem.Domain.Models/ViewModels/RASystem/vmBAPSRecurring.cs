using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
    public class vmBAPSRecurring : BaseClass
    {
        public vmBAPSRecurring()
        {
            this.ErrorType = 0;
            this.ErrorMessage = "";
        }
        public vmBAPSRecurring(int errorType, string errorMessage)
        {
            this.ErrorType = errorType;
            this.ErrorMessage = errorMessage;
        }

        public int TotalTenant { get; set; }

        public decimal TotalAmount { get; set; }

    }
}
