using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Service
{
    public static class ConstantAccrueStatusHelper
    {
        public enum AccrueStatus : int
        {
            NotYet = 1,           
            WaitingFinanceConfirmation = 2,
            WaitingUserConfirmation = 3,
            WaitingFinalConfirmation = 4,
            WaitingReConfirmConfirmation = 5,
            Done = 6
        };
    }
}
