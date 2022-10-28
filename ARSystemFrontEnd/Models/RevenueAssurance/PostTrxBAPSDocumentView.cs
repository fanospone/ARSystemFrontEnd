using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ARSystemFrontEnd.Models
{
    public class PostTrxBAPSDocumentView : DatatableAjaxModel
    {         
        public string DocId { get; set; }
        public string CustomerID { get; set; }
        public string DocName { get; set; }

    } 
} 