using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ARSystemFrontEnd.Models
{
    public class PostTrxARRMonitoringCashInRemarkDetailWeekly: DatatableAjaxModel
    {
		public int Periode { get; set; }
		public int Month { get; set; }
        public int Week { get; set; }
        public string OperatorID { get; set; }
        public string Remarks { get; set; }
}
}