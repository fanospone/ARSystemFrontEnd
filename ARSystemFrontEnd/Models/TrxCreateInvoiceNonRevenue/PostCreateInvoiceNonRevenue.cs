using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using ARSystem.Domain.Models.TBIGSYSDB01.TBGARSystem;

namespace ARSystemFrontEnd.Models
{
    public class PostCreateInvoiceNonRevenue
    {
        public int vtrxInvoiceNonRevenueID { get; set; }
        public decimal vAmount { get; set; }
        public decimal vDiscount { get; set; }
        public decimal vDPP { get; set; }
        public decimal vTotalPPN { get; set; }
        public decimal vTotalPPH { get; set; }
        public decimal vPenalty { get; set; }
        public decimal vInvoiceTotal { get; set; }
        public bool IsPPN { get; set; }
        public bool IsPPH { get; set; }
        public string vCompany { get; set; }
        public string vOperator { get; set; }
        public string vDescription { get; set; }
        public bool IsPPHFinal { get; set; }
        public List<trxInvoiceNonRevenueSite> ListInvoiceNonRevenueSite { get; set; }
        public int mstCategoryInvoiceID { get; set; }
    }
}