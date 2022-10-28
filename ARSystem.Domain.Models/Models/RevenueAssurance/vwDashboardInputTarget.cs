using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
    public class vwDashboardInputTarget : BaseClass
    {
        public vwDashboardInputTarget()
        {
            this.ErrorType = 0;
            this.ErrorMessage = null;
        }
        public vwDashboardInputTarget(int errorType, string errorMessage)
        {
            this.ErrorType = errorType;
            this.ErrorMessage = errorMessage;
        }
        public string DepartmentName { get; set; }
        public string DepartmentCode { get; set; }
        public int? Year { get; set; }
        public decimal? Jan_AmountIDR { get; set; }
        public decimal? Jan_OptimistIDR { get; set; }
        public decimal? Jan_MostLikelyIDR { get; set; }
        public decimal? Jan_PessimistIDR { get; set; }

        public decimal? Feb_AmountIDR { get; set; }
        public decimal? Feb_OptimistIDR { get; set; }
        public decimal? Feb_MostLikelyIDR { get; set; }
        public decimal? Feb_PessimistIDR { get; set; }

        public decimal? Mar_AmountIDR { get; set; }
        public decimal? Mar_OptimistIDR { get; set; }
        public decimal? Mar_MostLikelyIDR { get; set; }
        public decimal? Mar_PessimistIDR { get; set; }

        public decimal? Apr_AmountIDR { get; set; }
        public decimal? Apr_OptimistIDR { get; set; }
        public decimal? Apr_MostLikelyIDR { get; set; }
        public decimal? Apr_PessimistIDR { get; set; }

        public decimal? May_AmountIDR { get; set; }
        public decimal? May_OptimistIDR { get; set; }
        public decimal? May_MostLikelyIDR { get; set; }
        public decimal? May_PessimistIDR { get; set; }

        public decimal? Jun_AmountIDR { get; set; }
        public decimal? Jun_OptimistIDR { get; set; }
        public decimal? Jun_MostLikelyIDR { get; set; }
        public decimal? Jun_PessimistIDR { get; set; }

        public decimal? Jul_AmountIDR { get; set; }
        public decimal? Jul_OptimistIDR { get; set; }
        public decimal? Jul_MostLikelyIDR { get; set; }
        public decimal? Jul_PessimistIDR { get; set; }

        public decimal? Aug_AmountIDR { get; set; }
        public decimal? Aug_OptimistIDR { get; set; }
        public decimal? Aug_MostLikelyIDR { get; set; }
        public decimal? Aug_PessimistIDR { get; set; }

        public decimal? Sep_AmountIDR { get; set; }
        public decimal? Sep_OptimistIDR { get; set; }
        public decimal? Sep_MostLikelyIDR { get; set; }
        public decimal? Sep_PessimistIDR { get; set; }

        public decimal? Oct_AmountIDR { get; set; }
        public decimal? Oct_OptimistIDR { get; set; }
        public decimal? Oct_MostLikelyIDR { get; set; }
        public decimal? Oct_PessimistIDR { get; set; }

        public decimal? Nov_AmountIDR { get; set; }
        public decimal? Nov_OptimistIDR { get; set; }
        public decimal? Nov_MostLikelyIDR { get; set; }
        public decimal? Nov_PessimistIDR { get; set; }

        public decimal? Dec_AmountIDR { get; set; }
        public decimal? Dec_OptimistIDR { get; set; }
        public decimal? Dec_MostLikelyIDR { get; set; }
        public decimal? Dec_PessimistIDR { get; set; }


    }
}
		