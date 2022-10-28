using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ARSystemFrontEnd.Models
{
    public class PostTrxManageMergedInvoiceOnlyBuildingView : DatatableAjaxModel
    {
        public string customerName { get; set; }
        public string companyId { get; set; }
        public List<int> listHeaderId { get; set; }
        public int PICAReprintID { get; set; }
        public string ReprintRemark { get; set; }
        public string InvNo { get; set; }
        public string ApprovalStatus { get; set; }
    }
}