using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ARSystemFrontEnd.Models
{
    public class PostProductView : DatatableAjaxModel
    {
        public string ProductName { get; set; }
        public int ProductTypeId { get; set; }
        public int IsOperator { get; set; }
        public int CustomerTypeId { get; set; }
        public int ProductId { get; set; }
    }
}