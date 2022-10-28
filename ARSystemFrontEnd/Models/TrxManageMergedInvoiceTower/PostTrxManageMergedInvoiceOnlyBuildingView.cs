using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ARSystemFrontEnd.Models
{
    public class PostTrxManageMergedInvoiceOnlyTowerView : DatatableAjaxModel
    {
        public string operatorId { get; set; }
        public string companyId { get; set; }
        public List<int> listHeaderId { get; set; }
        public int PICAReprintID { get; set; }
        public string ReprintRemark { get; set; }
        public string InvNo { get; set; }
        public string ApprovalStatus { get; set; }
    }
}