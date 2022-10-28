using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ARSystemFrontEnd.Models
{
    public class PostARProcessInvoiceTowerReceipt
    {
        public int trxPICAARID { get; set; }
        public int trxInvoiceHeaderID { get; set; }
        public int mstPICATypeID { get; set; }
        public int mstPICAMajorID { get; set; }
        public int mstPICADetailID { get; set; }
        public bool NeedCN { get; set; }
        public string Remark { get; set; }
        public int mstInvoiceCategoryId { get; set; }
    }
}