﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ARSystemFrontEnd.Models
{
    public class PostTrxCreateInvoiceTowerView : DatatableAjaxModel
    {
        public string strCompanyId { get; set; }
        public string strOperator { get; set; }
        public string strBapsType { get; set; }
        public string strPeriodInvoice { get; set; }
        public string strInvoiceType { get; set; }
        public string strRegional { get; set; }
        public int InvoiceCategoryId { get; set; }
        public string strSoNumber { get; set; }

        public int intmstInvoiceStatusId { get; set; }
        public string strPONumber { get; set; }
        public string strBAPSNumber { get; set; }
        public string strSiteIdOld { get; set; }
        public string strStartPeriod { get; set; }
        public string strEndPeriod { get; set; }

        public int invoiceManual { get; set; }

        public int isFreeze { get; set; }


    }
}