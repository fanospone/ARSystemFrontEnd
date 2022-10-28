using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ARSystemFrontEnd.Models
{
    public class PostMstApprovalBAPS : DatatableAjaxModel
    {
        public int strApprovalID { get; set; }
        public int strRegionID { get; set; }
        public string strApprovalName { get; set; }
        public string strPosition { get; set; }
        public string strCustomerID { get; set; }
      

      public  ARSystemService.MstApprovalBAPS ApprBaps = new ARSystemService.MstApprovalBAPS();
    }
}