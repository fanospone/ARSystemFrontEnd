using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
    public class vmGetInvoicePostedList : BaseClass
    {
        public vmGetInvoicePostedList()
        {
            this.ErrorType = 0;
            this.ErrorMessage = "";
        }
        public vmGetInvoicePostedList(int errorType, string errorMessage)
        {
            this.ErrorType = errorType;
            this.ErrorMessage = errorMessage;
        }
        public List<int> HeaderId { get; set; }
        public List<int> CategoryId { get; set; }
        public List<int> isCNInvoice { get; set; }
        public List<int> BatchNumber { get; set; }
        public List<int> mstPaymentCodeId { get; set; }

        public int InttrxInvoiceHeaderId { get; set; }
        public int IntCategoryId { get; set; }
        public int IntBatchNumber { get; set; }
        public int IntmstPaymentCodeId { get; set; }
        public int IntmstInvoiceStatusId { get; set; }
        public int IntmstInvoiceStatusIdLog { get; set; }
        public string UserID { get; set; }
        public int LogWeek { get; set; }
        public int Counter { get; set; }

    }
}
