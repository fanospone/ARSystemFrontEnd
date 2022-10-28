using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ARSystemFrontEnd.Models
{
    public class HighchartData
    {
        public string type { get; set; }
        public string name { get; set; }
        public int[] data { get; set; }
        public string color { get; set; }
        public int yAxis { get; set; }
    }
}