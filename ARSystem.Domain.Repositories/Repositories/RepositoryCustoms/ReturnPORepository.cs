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
    public class ReturnPORepository
    {
        private DbContext _context;
        public ReturnPORepository(DbContext context)
        {
            _context = context;
        }

        public void ReturnToPOProcessMarketing(string SONumber, string BapsType, int? StipsiroId, int Renewal, string UserID)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "tbg.spUpdatePOStatusReturn";

                command.Parameters.Add(command.CreateParameter("@SOnumb", SONumber));
                command.Parameters.Add(command.CreateParameter("@POType", BapsType));
                command.Parameters.Add(command.CreateParameter("@StipSiro", StipsiroId));
                command.Parameters.Add(command.CreateParameter("@Renewal", Renewal));
                command.Parameters.Add(command.CreateParameter("@UserID", UserID));

                command.ExecuteNonQuery();
            }
        }
    }
}
