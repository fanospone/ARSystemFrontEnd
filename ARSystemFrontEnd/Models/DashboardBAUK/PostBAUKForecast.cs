using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ARSystemFrontEnd.Models
{
    public class PostBAUKForecast : DatatableAjaxModel
    {

        public string GroupSum { get; set; }
        public string GroupSumID { get; set; }
        public int TotOutstanding { get; set; }
        public float AmountTotOutstanding { get; set; }
        public int TotW1 { get; set; }
        public float AmountTotW1 { get; set; }
        public int TotW2 { get; set; }
        public float AmountTotW2 { get; set; }
        public int TotW3 { get; set; }
        public float AmountTotW3 { get; set; }
        public int TotW4 { get; set; }
        public float AmountTotW4 { get; set; }
        public int TotW5 { get; set; }
        public float AmountTotW5 { get; set; }
        public int TotM1 { get; set; }
        public float AmountTotM1 { get; set; }
        public int TotM2 { get; set; }
        public float AmountTotM2 { get; set; }
        public int TotM3 { get; set; }
        public float AmountTotM3 { get; set; }
        public int TotM4 { get; set; }
        public float AmountTotM4 { get; set; }
        public int TotM5 { get; set; }
        public float AmountTotM5 { get; set; }
        public int TotNA { get; set; }
        public float AmountTotNA { get; set; }
    }
}