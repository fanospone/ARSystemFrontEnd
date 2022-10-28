
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
    public class vwARRevSysPerSonumb : BaseClass
    {
        public vwARRevSysPerSonumb()
        {
            this.ErrorType = 0;
            this.ErrorMessage = null;
        }
        public vwARRevSysPerSonumb(int errorType, string errorMessage)
        {
            this.ErrorType = errorType;
            this.ErrorMessage = errorMessage;
        }
        public string SoNumber { get; set; }
        public int? Periode { get; set; }
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




        public decimal? Jan_RevenueNormal { get; set; }
        public decimal? Jan_Inflasi { get; set; }
        public decimal? Jan_Overblast { get; set; }
        public decimal? Jan_NetRevenue { get; set; }
        public decimal? Jan_PPN { get; set; }
        public decimal? Feb_RevenueNormal { get; set; }
        public decimal? Feb_Inflasi { get; set; }
        public decimal? Feb_Overblast { get; set; }
        public decimal? Feb_NetRevenue { get; set; }
        public decimal? Feb_PPN { get; set; }
        public decimal? Mar_RevenueNormal { get; set; }
        public decimal? Mar_Inflasi { get; set; }
        public decimal? Mar_Overblast { get; set; }
        public decimal? Mar_NetRevenue { get; set; }
        public decimal? Mar_PPN { get; set; }
        public decimal? Apr_RevenueNormal { get; set; }
        public decimal? Apr_Inflasi { get; set; }
        public decimal? Apr_Overblast { get; set; }
        public decimal? Apr_NetRevenue { get; set; }
        public decimal? Apr_PPN { get; set; }
        public decimal? May_RevenueNormal { get; set; }
        public decimal? May_Inflasi { get; set; }
        public decimal? May_Overblast { get; set; }
        public decimal? May_NetRevenue { get; set; }
        public decimal? May_PPN { get; set; }
        public decimal? Jun_RevenueNormal { get; set; }
        public decimal? Jun_Inflasi { get; set; }
        public decimal? Jun_Overblast { get; set; }
        public decimal? Jun_NetRevenue { get; set; }
        public decimal? Jun_PPN { get; set; }
        public decimal? Jul_RevenueNormal { get; set; }
        public decimal? Jul_Inflasi { get; set; }
        public decimal? Jul_Overblast { get; set; }
        public decimal? Jul_NetRevenue { get; set; }
        public decimal? Jul_PPN { get; set; }
        public decimal? Aug_RevenueNormal { get; set; }
        public decimal? Aug_Inflasi { get; set; }
        public decimal? Aug_Overblast { get; set; }
        public decimal? Aug_NetRevenue { get; set; }
        public decimal? Aug_PPN { get; set; }
        public decimal? Sep_RevenueNormal { get; set; }
        public decimal? Sep_Inflasi { get; set; }
        public decimal? Sep_Overblast { get; set; }
        public decimal? Sep_NetRevenue { get; set; }
        public decimal? Sep_PPN { get; set; }
        public decimal? Oct_RevenueNormal { get; set; }
        public decimal? Oct_Inflasi { get; set; }
        public decimal? Oct_Overblast { get; set; }
        public decimal? Oct_NetRevenue { get; set; }
        public decimal? Oct_PPN { get; set; }
        public decimal? Nov_RevenueNormal { get; set; }
        public decimal? Nov_Inflasi { get; set; }
        public decimal? Nov_Overblast { get; set; }
        public decimal? Nov_NetRevenue { get; set; }
        public decimal? Nov_PPN { get; set; }
        public decimal? Dec_RevenueNormal { get; set; }
        public decimal? Dec_Inflasi { get; set; }
        public decimal? Dec_Overblast { get; set; }
        public decimal? Dec_NetRevenue { get; set; }
        public decimal? Dec_PPN { get; set; }

        public decimal? TotalBalance { get; set; }
        public string SiteID { get; set; }
        public string CustomerID { get; set; }
        public string ProductName { get; set; }
        public string Status { get; set; }

    }
}