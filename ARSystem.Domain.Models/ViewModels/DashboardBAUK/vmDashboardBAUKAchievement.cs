using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
    public class vmDashboardBAUKAchievement : BaseClass
    {
        public vmDashboardBAUKAchievement()
        {
            this.ErrorType = 0;
            this.ErrorMessage = "";
        }
        public vmDashboardBAUKAchievement(int errorType, string errorMessage)
        {
            this.ErrorType = errorType;
            this.ErrorMessage = errorMessage;
        }
        public string GroupSum { get; set; }
        public string GroupSumID { get; set; }
        public int TotM1 { get; set; }
        public int TotLTM1 { get; set; }
        public int TotM2 { get; set; }
        public int TotLTM2 { get; set; }
        public int TotM3 { get; set; }
        public int TotLTM3 { get; set; }
        public int TotM4 { get; set; }
        public int TotLTM4 { get; set; }
        public int TotM5 { get; set; }
        public int TotLTM5 { get; set; }
        public int TotM6 { get; set; }
        public int TotLTM6 { get; set; }
        public int TotM7 { get; set; }
        public int TotLTM7 { get; set; }
        public int TotM8 { get; set; }
        public int TotLTM8 { get; set; }
        public int TotM9 { get; set; }
        public int TotLTM9 { get; set; }
        public int TotM10 { get; set; }
        public int TotLTM10 { get; set; }
        public int TotM11 { get; set; }
        public int TotLTM11 { get; set; }
        public int TotM12 { get; set; }
        public int TotLTM12 { get; set; }
    }
}
