using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ARSystemFrontEnd.Models
{
    public class PostInvoiceDeductionProcess
    {
        public int mstDeductionTypeId { get; set; }
        public string operatorId { get; set; }
        public string companyId { get; set; }
        public string UploadBA { get; set; }
        public DateTime startPeriod { get; set; }
        public DateTime endPeriod { get; set; }
        public decimal AmountWAPU { get; set; }
    }
}