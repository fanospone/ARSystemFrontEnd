using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ARSystemFrontEnd.Models
{
    public class PostISPInvoiceInformation : DatatableAjaxModel
    {
      
        public string slCompany { get; set; }
        public string  slCustomer { get; set; }
        public string slStipCode { get; set; }
        public string fSONumber { get; set; }
        public string fSiteID { get; set; }
        public string fSiteName { get; set; }
        public string fSiteIDOpr { get; set; }
        public string fSiteNameOpr { get; set; }
        public string SONumber { get; set; }


    }
}