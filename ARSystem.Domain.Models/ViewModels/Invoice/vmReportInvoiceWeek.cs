using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
    public class vmReportInvoiceWeek : BaseClass
    {
        public vmReportInvoiceWeek()
        {
            this.ErrorType = 0;
            this.ErrorMessage = "";
        }
        public vmReportInvoiceWeek(int errorType, string errorMessage)
        {
            this.ErrorType = errorType;
            this.ErrorMessage = errorMessage;
        }

        public string ValueId { get; set; }
        public string ValueDesc { get; set; }
    }
}
