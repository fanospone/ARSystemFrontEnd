using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ARSystem.Domain.Models;
using ARSystem.Domain.DAL;
using System.Data;


namespace ARSystem.Domain.Repositories
{
    public class RTIDoneNOverdueByGroupRepository : BaseRepository<vwRTINOverdueModel>
    {

        private DbContext _context;
        public RTIDoneNOverdueByGroupRepository(DbContext context) : base(context)
        {
            _context = context;
        }


        #region public methode


        public async Task<List<vwRTINOverdueModel>> GetChartDataByGroup(int Year, string customerid, string groupby)
        {
            return await pGetChartDataByGroup(Year, customerid, groupby);
        }

        #endregion

        #region private methode
        private async Task<List<vwRTINOverdueModel>> pGetChartDataByGroup(int Year, string customerid, string groupby)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "[dbo].[uspDashboardRTIOverdue]";
                command.Parameters.Add(command.CreateParameter("@vYear", Year));
                command.Parameters.Add(command.CreateParameter("@vIsSummary", 0));
                command.Parameters.Add(command.CreateParameter("@vGroupBy", groupby));


                return this.ReadTransaction(command).ToList();
            }
        }

        private async Task<List<vwRTINOverdueModel>> pGetChartDataByGroup2(int Year, string customerid, string groupby)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "[dbo].[uspGetRTIDoneNOverdueChart]";
                command.Parameters.Add(command.CreateParameter("@vYear", Year));
                command.Parameters.Add(command.CreateParameter("@vCustomerID", customerid));
                command.Parameters.Add(command.CreateParameter("@vAction", 1));
                command.Parameters.Add(command.CreateParameter("@vGroupBy", groupby));


                return this.ReadTransaction(command).ToList();
            }
        }

        #endregion


    }


}
