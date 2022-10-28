using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ARSystemFrontEnd.Models
{
    public class PostCustomerTenantView : DatatableAjaxModel
    {
        public string customerName { get; set; }
        public int customerTypeId { get; set; }
        public int intIsActive { get; set; }
        public string companyType { get; set; }
        public int customerId { get; set; }
    }
}