using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ARSystemFrontEnd.Models
{
    public class PostTrxBOQDataDone : DatatableAjaxModel
    {
        public int mstRABoqID { get; set; }

        public string PrintID { get; set; }

        public string BatchID { get; set; }
        public List<ARSystemService.trxRABOQSignatory> ListSignatory { get; set; } 
    }
} 