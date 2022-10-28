using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models.Models
{
    public class mstRACustomerInvoice : BaseClass
    {
        public mstRACustomerInvoice()
        {
            this.ErrorType = 0;
            this.ErrorMessage = null;
        }
        public mstRACustomerInvoice(int errorType, string errorMessage)
        {
            this.ErrorType = errorType;
            this.ErrorMessage = errorMessage;
        }
        public int ID { get; set; }
        public int mstRAInvoiceTypeID { get; set; }
        public string CustomerID { get; set; }
        public string CompanyID { get; set; }
        public bool? IsActive { get; set; }
        public int? mstRARoleActivityID { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
    }
}
