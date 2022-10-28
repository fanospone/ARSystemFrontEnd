using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
    public class vmARRevSysPerSonumb : BaseClass
    {
        public vmARRevSysPerSonumb()
        {
            this.ErrorType = 0;
            this.ErrorMessage = null;
        }
        public vmARRevSysPerSonumb(int errorType, string errorMessage)
        {
            this.ErrorType = errorType;
            this.ErrorMessage = errorMessage;
        }
        public string SoNumber { get; set; }
        public string SiteName { get; set; }
        public string Operator { get; set; }
        public string RegionName { get; set; }
        public decimal? Jan { get; set; }
        public decimal? Feb { get; set; }
        public decimal? Mar { get; set; }
        public decimal? Apr { get; set; }
        public decimal? May { get; set; }
        public decimal? Jun { get; set; }
        public decimal? Jul { get; set; }
        public decimal? Aug { get; set; }
        public decimal? Sep { get; set; }
        public decimal? Oct { get; set; }
        public decimal? Nov { get; set; }
        public decimal? Dec { get; set; }
        public decimal? TotalBalance { get; set; }
        public string SiteID { get; set; }
        public string CustomerID { get; set; }
        public string ProductName { get; set; }
        public string Status { get; set; }
        public List<trxARRevSysListDetail> Detail { get; set; }
    }
}
