using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Models
{
    public class RTIAchievementModel : BaseClass
    {
        public RTIAchievementModel()
        {
            this.ErrorType = 0;
            this.ErrorMessage = null;
        }
        public RTIAchievementModel(int errorType, string errorMessage)
        {
            this.ErrorType = errorType;
            this.ErrorMessage = errorMessage;
        }

        public decimal InvAmount { get; set; }
        public string CustomerID { get; set; }
        public string MonthName { get; set; }
        public decimal AmountTarget { get; set; }
    }
}
