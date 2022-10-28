using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ARSystemFrontEnd.Models
{
    public class PostPICAReprintView : DatatableAjaxModel
    {
        public string picaReprint { get; set; }
        public int isActive { get; set; }
    }
}