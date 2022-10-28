using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ARSystemFrontEnd.Models
{
    public class PostCategoryBuilding : DatatableAjaxModel
    {
        public string strCategoryBuilding { get; set; }
        public ARSystemService.mstCategoryBuilding model = new ARSystemService.mstCategoryBuilding();
    }
}