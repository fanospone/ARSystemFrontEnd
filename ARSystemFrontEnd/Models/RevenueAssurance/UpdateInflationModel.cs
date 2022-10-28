using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ARSystemFrontEnd.Models
{
    public class UpdateInflationModel
    {
        public string ListID { get; set; }
        public decimal Percentage { get; set; }
        public int Year { get; set; }
    }
}