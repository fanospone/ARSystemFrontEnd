using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ARSystemFrontEnd.Models
{
    public class PostKursView : DatatableAjaxModel
    {
        public DateTime? kursDate { get; set; }
        public decimal kursTengahBI { get; set; }
        public decimal kursPajak { get; set; }
    }
}