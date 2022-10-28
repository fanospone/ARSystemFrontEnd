using System;
using System.Collections.Generic;
using ARSystem.Domain.Models;

namespace ARSystemFrontEnd.Models
{
    public class StopAccrue
    {
        public List<vwStopAccrueDetail> vwStopAccrueDetail = new List<vwStopAccrueDetail>();
        public string HtmlElements { get; set; }
    }
}