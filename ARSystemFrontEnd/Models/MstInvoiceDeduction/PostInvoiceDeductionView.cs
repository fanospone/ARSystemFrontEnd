using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ARSystemFrontEnd.Models
{
    public class PostInvoiceDeductionView : DatatableAjaxModel
    {
        public int mstDeductionTypeId { get; set; }
        public string operatorId { get; set; }
        public string companyId { get; set; }
    }
}