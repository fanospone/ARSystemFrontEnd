using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ARSystemFrontEnd.Models
{
    public class PostEmailView : DatatableAjaxModel
    {
        public string strEmailName { get; set; }
        public int intIsActive { get; set; }
    }
}