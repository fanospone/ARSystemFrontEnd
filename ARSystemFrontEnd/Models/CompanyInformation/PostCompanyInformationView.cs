using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ARSystemFrontEnd.Models
{
    public class PostCompanyInformationView : DatatableAjaxModel
    {
        public string strCompany { get; set; }
        public string strTerm { get; set; }
        public int intIsActive { get; set; }
        public string strCompanyType { get; set; }
    }
}