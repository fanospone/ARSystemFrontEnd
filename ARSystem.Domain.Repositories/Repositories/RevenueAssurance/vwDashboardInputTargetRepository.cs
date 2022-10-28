using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ARSystem.Domain.DAL;
using ARSystem.Domain.Models;
using System.Data;

namespace ARSystem.Domain.Repositories
{
    public class vwDashboardInputTargetRepository : BaseRepository<vwDashboardInputTarget>
    {
        private DbContext _context;
        public vwDashboardInputTargetRepository(DbContext context) : base(context)
        {
            _context = context;
        }
        public List<vwDashboardInputTarget> GetList(string whereClause, int year)
        {
            return pGetList(whereClause, year);
        }

        #region Private
        private List<vwDashboardInputTarget> pGetList(string whereClause, int year)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.usptrxRADashboardInputTarget";
                command.Parameters.Add(command.CreateParameter("@vType", "GetPaged"));
                command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
                command.Parameters.Add(command.CreateParameter("@vYear", year));

                var x = this.ReadTransaction(command).ToList();
                return x;
            }
        }
        #endregion
    }
}
