using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ARSystemFrontEnd.Models
{
    public class PostDashboardView : DatatableAjaxModel
    {
        public int Year { get; set; }
        public int Month { get; set; }
        public int Week { get; set; }
        public string Currency { get; set; }
    }
}