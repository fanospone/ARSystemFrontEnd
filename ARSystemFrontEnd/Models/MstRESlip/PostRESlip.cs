using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ARSystemFrontEnd.Models
{
    public class PostRESlip : DatatableAjaxModel
    {
        public string SONumber { get; set; }
        public ARSystemService.mstRESlip model = new ARSystemService.mstRESlip();

    }
}
