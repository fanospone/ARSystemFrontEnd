using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ARSystem.Domain.DAL;
using ARSystem.Domain.Models;
using System.Data;
using System.Data.SqlClient;

namespace ARSystem.Domain.Repositories
{
    public class ChangePPHFinalRepository
    {
        private DbContext _context;
        public ChangePPHFinalRepository(DbContext context)
        {
            _context = context;
        }

        public int UpdatePPHFinal(string UserID, vwChangePPHFinal vwChangePPHFinal, int intPPHType)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspChangePPHFinal";

                command.Parameters.Add(command.CreateParameter("@SONumber", vwChangePPHFinal.SONumber));
                if (!(vwChangePPHFinal.BAPSNumber == null))
                {
                    command.Parameters.Add(command.CreateParameter("@BAPSNumber", vwChangePPHFinal.BAPSNumber));
                }
                if (!(vwChangePPHFinal.BAPSType == null))
                {
                    command.Parameters.Add(command.CreateParameter("@BAPSType", vwChangePPHFinal.BAPSType));
                }
                if (!(vwChangePPHFinal.BAPSPeriod == null))
                {
                    command.Parameters.Add(command.CreateParameter("@BAPSPeriod", vwChangePPHFinal.BAPSPeriod));
                }
                if (!(vwChangePPHFinal.PONumber == null))
                {
                    command.Parameters.Add(command.CreateParameter("@PONumber", vwChangePPHFinal.PONumber));
                }
                command.Parameters.Add(command.CreateParameter("@StipSiroId", vwChangePPHFinal.STIPSiroID));
                command.Parameters.Add(command.CreateParameter("@StartDateInvoice", vwChangePPHFinal.StartDateInvoice));
                command.Parameters.Add(command.CreateParameter("@EndDateInvoice", vwChangePPHFinal.EndDateInvoice));
                command.Parameters.Add(command.CreateParameter("@IsPPHFinal", intPPHType));
                if (!string.IsNullOrWhiteSpace(UserID))
                {
                    command.Parameters.Add(command.CreateParameter("@UserID", UserID));
                }

                return command.ExecuteNonQuery();
            }
        }
    }
}
