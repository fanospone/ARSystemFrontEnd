using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ARSystemFrontEnd.Models
{
    public class PostPaymentBankView : DatatableAjaxModel
    {
        public string companyId { get; set; }
        public string bankGroupId { get; set; }
        public string accountNo { get; set; }
    }
}