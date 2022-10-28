using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ARSystemFrontEnd.Models
{
    public class PostApproveCNInvoiceBuilding
    {
        public int trxInvoiceHeaderID { get; set; }
        public string RejectRole { get; set; }
        public string userID { get; set; }
    }
}