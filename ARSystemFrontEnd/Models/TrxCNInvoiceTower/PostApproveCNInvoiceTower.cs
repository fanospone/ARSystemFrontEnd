using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ARSystemFrontEnd.Models
{
    public class PostApproveCNInvoiceTower
    {
        public int trxInvoiceHeaderID { get; set; }
        public int mstInvoiceCategoryId { get; set; }
        public int mstPICATypeIDSection { get; set; }
        public int mstPICADetailIDSection { get; set; }
        public string RejectRole { get; set; }
        public string userID { get; set; }
        public string vSource { get; set; }
    }
}