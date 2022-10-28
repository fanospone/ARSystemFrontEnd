using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ARSystemFrontEnd.Models
{
    public class PostMstRejectHdrDtlView : DatatableAjaxModel
    {
        public string strHdr { get; set; }
        public int intIsActive { get; set; }
        public int mstUserGroupId { get; set; }


    }
}