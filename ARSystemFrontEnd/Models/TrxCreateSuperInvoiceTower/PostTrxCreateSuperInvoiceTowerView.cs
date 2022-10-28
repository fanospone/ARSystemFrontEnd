﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ARSystemFrontEnd.Models
{
    public class PostTrxCreateSuperInvoiceTowerView : DatatableAjaxModel
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
    }
}