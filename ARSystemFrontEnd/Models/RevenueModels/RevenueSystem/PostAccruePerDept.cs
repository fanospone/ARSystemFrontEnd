using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ARSystemFrontEnd.Models
{
    public class PostAccruePerDept : DatatableAjaxModel
    {
        public string Category { get; set; }
        public string year { get; set; }
        public string month { get; set; }
        public string week { get; set; }
    }
}