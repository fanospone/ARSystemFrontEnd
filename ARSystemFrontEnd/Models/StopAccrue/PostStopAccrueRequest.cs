using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ARSystemFrontEnd.Models
{
    public class PostStopAccrueRequest
    {
        
        public int DetailID { get; set; }
        public int? CategoryCaseID { get; set; }
        public int? DetailCaseID { get; set; }
        public string FileName { get; set; }
        public string SONumber { get; set; }
        public string Remarks { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public string RequestNumber { get; set; }
    }
}