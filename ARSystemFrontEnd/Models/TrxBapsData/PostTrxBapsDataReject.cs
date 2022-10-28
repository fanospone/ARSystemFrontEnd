using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ARSystemFrontEnd.Models
{
    public class PostTrxBapsDataReject
    {
        public List<int> ListId { get; set; }
        public string Remarks { get; set; }
        public string Department { get; set; }
        public int MstRejectDtlId { get; set; }
        public string RejectType { get; set; }
        public string Recipient { get; set; }
        public string CC { get; set; }
        public string statusRejectPOConfirm { get; set; }

    }
}