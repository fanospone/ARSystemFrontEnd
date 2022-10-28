using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
    public class vmReportInvoiceMaxMinLogDate : BaseClass
    {
        public vmReportInvoiceMaxMinLogDate()
        {
            this.ErrorType = 0;
            this.ErrorMessage = "";
        }
        public vmReportInvoiceMaxMinLogDate(int errorType, string errorMessage)
        {
            this.ErrorType = errorType;
            this.ErrorMessage = errorMessage;
        }

        public DateTime MaxLogDate { get; set; }
        public DateTime MinLogDate { get; set; }
    }
}
