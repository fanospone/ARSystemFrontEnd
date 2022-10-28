using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ARSystemFrontEnd.Models
{
    public class PostTrxBOQDataProcessApproval : DatatableAjaxModel
    {
        public string BOQNumber { get; set; }
        public int TotalRenewalTenant { get; set; }
        public decimal TotalAmount { get; set; }
        public string StatusBOQ { get; set; }
        public string RemarkOnApproval { get; set; }

    }
} 