﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
   public class DocumentCheckingSummary : BaseClass
    {
        public DocumentCheckingSummary()
        {
            this.ErrorType = 0;
            this.ErrorMessage = null;
        }
        public DocumentCheckingSummary(int errorType, string errorMessage)
        {
            this.ErrorType = errorType;
            this.ErrorMessage = errorMessage;
        }

        public string Number { get; set; }
        public string SoNumber { get; set; }
        public string StipCode { get; set; }
        public string SiteID { get; set; }
        public string SiteName { get; set; }
        public string Product { get; set; }
        public string TenantType { get; set; }
        public string DeptSubmitBAUK { get; set; }
        public string PICSubmitBAUK { get; set; }
        public string PICReveiewHardCopyBAUK { get; set; }
        public DateTime? DateReview { get; set; }
        public string StatusBAUK { get; set; }
        public string ApprStatus { get; set; }
        public string Remarks { get; set; }
        public string Customer { get; set; }
    }
}
