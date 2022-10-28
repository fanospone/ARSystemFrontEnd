using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ARSystemFrontEnd.Models
{
    public class PostCustomReport
    {
        public ARSystemService.mstRAGeneratorPDF Report { get; set; }

        public ARSystemService.LogData Log { get; set; }
    }
}