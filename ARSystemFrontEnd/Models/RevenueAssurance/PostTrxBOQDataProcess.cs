using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ARSystemFrontEnd.Models
{
    public class PostTrxBOQDataProcess : DatatableAjaxModel
    {
        public string AreaId { get; set; }
        public string CompanyId { get; set; }
        public string BOQNumber { get; set; }
        public int TotalTenant { get; set; }
        public decimal TotalAmount { get; set; }
        public List<int> detailIDs { get; set; }
        public List<ARSystemService.trxReconcile> ListTrxReconcile { get; set; }

    }
} 