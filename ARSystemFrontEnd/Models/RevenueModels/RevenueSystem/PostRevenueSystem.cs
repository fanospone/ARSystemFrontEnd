using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ARSystemFrontEnd.Models
{
    public class PostRevenueSystem : DatatableAjaxModel
    {
        public string spParam { get; set; }
        public string comAsset { get; set; }
        public string comInv { get; set; }
        public string region { get; set; }
        public string OperatorID { get; set; }
        public string Month { get; set; }
        public string Year { get; set; }
        public string Sonumb { get; set; }
        public DateTime StartPeriod { get; set; }
        public DateTime EndPeriod { get; set; }
        public string KategoryRevenue { get; set; }
        public string MonthYear { get; set; }
        public string[] columnValue { get; set; }
        public string InvoiceDate { get; set; }
        //public string[] columns { get; set; }
    }
}