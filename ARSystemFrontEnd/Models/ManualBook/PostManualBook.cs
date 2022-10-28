using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ARSystemFrontEnd.Models
{
    public class PostManualBook : DatatableAjaxModel
    {
        public string strProjectName { get; set; }
        public string strProjectDescription { get; set; }
    }
}