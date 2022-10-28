using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ARSystemFrontEnd.Models
{
    public class PostTrxARRMonitoringCashInRemarkDetailQuarterly : DatatableAjaxModel
    {
		public int Periode { get; set; }
		public int Quarter { get; set; }
        public string OperatorID { get; set; }
        public string Remarks { get; set; }
}
}