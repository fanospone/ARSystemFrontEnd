using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ARSystemFrontEnd.Models
{
    public class PostProductTypeView : DatatableAjaxModel
    {
        public string productType { get; set; }
        public string productCode { get; set; }
        public int intIsActive { get; set; }
    }
}