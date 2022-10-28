
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ARSystem.Domain.DAL;
using ARSystem.Domain.Models;

namespace ARSystem.Domain.Repositories
{
    public class DashboardBAUKActivityDetailRepository : BaseRepository<vmDashboardBAUKDetail>
    {
        private DbContext _context;
        public DashboardBAUKActivityDetailRepository(DbContext context) : base(context)
        {
            _context = context;
        }

        public List<vmDashboardBAUKDetail> GetList(vmDashboardBAUKDetail filter)
        {
            return pGetList(filter);
        }

        #region Private

        private List<vmDashboardBAUKDetail> pGetList(vmDashboardBAUKDetail filter)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspDashboardBAUKActivityDetail";

                command.Parameters.Add(command.CreateParameter("@vLastSelectedMonth", filter.Month));
                command.Parameters.Add(command.CreateParameter("@vMonth", filter.strMonth));
                command.Parameters.Add(command.CreateParameter("@vYear", filter.Year));
                command.Parameters.Add(command.CreateParameter("@vCustomer", filter.strCustomer == null ? "" : filter.strCustomer));
                command.Parameters.Add(command.CreateParameter("@vCompany", filter.strCompany == null ? "" : filter.strCompany));
                command.Parameters.Add(command.CreateParameter("@vSTIP", filter.strSTIP == null ? "" : filter.strSTIP));
                command.Parameters.Add(command.CreateParameter("@vProduct", filter.strProduct == null ? "" : filter.strProduct));
                command.Parameters.Add(command.CreateParameter("@vSelectedData", filter.SelectedData));

                return this.ReadTransaction(command).ToList();
            }
        }

        #endregion
    }
}