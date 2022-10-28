using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ARSystemFrontEnd.Models
{
    public class PostReserveInvoiceNumber : DatatableAjaxModel
    {
        public string OperatorId { get; set; }
        public string CompanyId { get; set; }
        public int AmountReserve { get; set; }
        public string Remarks { get; set; }
        public string StartDateRequest { get; set; }
        public string EndDateRequest { get; set; }
    }
}