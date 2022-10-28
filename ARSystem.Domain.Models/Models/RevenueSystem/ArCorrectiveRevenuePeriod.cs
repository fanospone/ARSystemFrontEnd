

namespace ARSystem.Domain.Models
{
    public class ArCorrectiveRevenuePeriod : BaseClass
    {
        public ArCorrectiveRevenuePeriod()
        {
            this.ErrorType = 0;
            this.ErrorMessage = null;
        }
        public ArCorrectiveRevenuePeriod(int errorType, string errorMessage)
        {
            this.ErrorType = errorType;
            this.ErrorMessage = errorMessage;
        }

        public byte MonthPeriod { get; set; }
        public short YearPeriod { get; set; }
    }
}
