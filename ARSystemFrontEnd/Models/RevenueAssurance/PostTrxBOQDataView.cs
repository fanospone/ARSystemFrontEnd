using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ARSystemFrontEnd.Models
{
    public class PostTrxBOQDataView : DatatableAjaxModel
    {
        public string strOperator { get; set; }
        public string strYear { get; set; }
        public string strCompanyId { get; set; }
        public string strArea { get; set; }
        public string strRegional { get; set; }
        public List<string> strSONumber { get; set; }
        public List<int> ListId { get; set; }
        public List<string> strBOQNumber { get; set; }

    }
} 