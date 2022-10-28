using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
    public class vmReportInvoiceYear : BaseClass
    {
        public vmReportInvoiceYear()
        {
            this.ErrorType = 0;
            this.ErrorMessage = "";
        }
        public vmReportInvoiceYear(int errorType, string errorMessage)
        {
            this.ErrorType = errorType;
            this.ErrorMessage = errorMessage;
        }

        public string ValueId { get; set; }
        public string ValueDesc { get; set; }
    }
}
