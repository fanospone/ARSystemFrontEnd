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
    public class JobReturnToBAPSRepository
    {
        private DbContext _context;
        public JobReturnToBAPSRepository(DbContext context)
        {
            _context = context;
        }


        /// <summary>
        /// untuk Update Data BAPS di Server TBGSys
        /// </summary>
        /// <param name="ReturnToBaps"></param>
        /// <returns></returns>
        public void ReturnToBAPS(string Where, string UserID)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspJobBapsReturnTo";

                command.Parameters.Add(command.CreateParameter("@Where", Where));
                command.Parameters.Add(command.CreateParameter("@UserID", UserID));

                command.ExecuteNonQuery();
            }
        }

        public void ReturnToBAPSV2(string ID, string Where, string WherePO)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspReturnFromFinance";

                command.Parameters.Add(command.CreateParameter("@mstRAActivityID", ID));
                command.Parameters.Add(command.CreateParameter("@Where", Where));
                command.Parameters.Add(command.CreateParameter("@WherePO", WherePO));

                command.ExecuteNonQuery();
            }
        }
    }
}
