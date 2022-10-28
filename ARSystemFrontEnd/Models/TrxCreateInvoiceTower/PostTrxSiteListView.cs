using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ARSystemFrontEnd.Models
{
    public class PostTrxSiteListView : DatatableAjaxModel
    {
        public List<int> ListId { get; set; }
        public List<int> ListCategoryId { get; set; }
        public ARSystemService.vmGetInvoicePostedList vm { get; set; }

    }
}