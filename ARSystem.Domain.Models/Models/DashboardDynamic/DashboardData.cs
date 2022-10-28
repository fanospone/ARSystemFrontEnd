
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
    public class DashboardData : BaseClass
    {
        public DashboardData()
        {
            this.ErrorType = 0;
            this.ErrorMessage = null;
        }
        public DashboardData(int errorType, string errorMessage)
        {
            this.ErrorType = errorType;
            this.ErrorMessage = errorMessage;
        }
        public List<Dictionary<string, string>> dataList { get; set; }

    }
}