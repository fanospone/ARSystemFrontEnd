using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ARSystemFrontEnd.Models
{
    public class PostReportARCBS : DatatableAjaxModel
    {
        public int DT_RowId { get; set; }
        public string Month { get; set; }
        public string Remarks { get; set; }
        public bool isAccounting { get; set; }
        public string mstInvoiceStatusId { get; set; }
        public string Week { get; set; }
        public string Operator { get; set; }
        public string RevenueType { get; set; }
        public decimal Week1Rev { get; set; }
        public int Week1NonRev { get; set; }
        public decimal Week2Rev { get; set; }
        public int Week2NonRev { get; set; }
        public decimal Week3Rev { get; set; }
        public decimal Week3NonRev { get; set; }
        public decimal Week4Rev { get; set; }
        public decimal Week4NonRev { get; set; }
        public decimal Week5Rev { get; set; }
        public int Week5NonRev { get; set; }
        public decimal Week6Rev { get; set; }
        public decimal Week6NonRev { get; set; }
    }
}