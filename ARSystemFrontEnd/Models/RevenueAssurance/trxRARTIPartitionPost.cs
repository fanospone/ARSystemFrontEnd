using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ARSystemFrontEnd.Models
{
    public class trxRARTIPartitionPost : DatatableAjaxModel
    {
        public string State { get; set; }
        public int ID { get; set; }
        public int trxReconcileID { get; set; }
        public int Term { get; set; }
        public DateTime EndInvoiceDate { get; set; }
        public DateTime StartInvoiceDate { get; set; }
        public DateTime StartPeriodInvoiceDate { get; set; }
        public DateTime EndPeriodInvoiceDate { get; set; }
        public string CustomerID { get; set; }
        public decimal BaseLeasePrice { get; set; }
        public decimal ServicePrice { get; set; }
        public int? DropFODistance { get; set; }
        public int? ProductID { get; set; }
    }
}