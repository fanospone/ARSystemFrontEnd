using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ARSystemFrontEnd.Models
{
    public class PostReplaceInvoiceNumber : DatatableAjaxModel
    {
        public string OperatorId { get; set; }
        public string CompanyId { get; set; }
        public string ReserveNo { get; set; }
        public string Year { get; set; }
        public ARSystemService.vwReserveInvoiceNumberDetail DataReservedInvoiceNumber { get; set; }
        public string InvNoHeader { get; set; }
    }
}