using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ARSystem.Domain.Models.ViewModels.Datatable;

namespace ARSystem.Domain.Models.ViewModels.RevenueSystem
{
    public class GridInflasiParam : DatatableAjaxModel
    {
        public int ID { get; set; }
        public int? Year { get; set; }
        public decimal? Percentage { get; set; }
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public string FilePath { get; set; }
        public string FileExtension { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string Sonumb { get; set; }
        public string SiteID { get; set; }
        public string SiteName { get; set; }
        public string SiteIDOpr { get; set; }
        public string SiteNameOpr { get; set; }
        public string Customer { get; set; }
        public string Company { get; set; }
        public string Regional { get; set; }
    }
}
