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
    public class RTIDoneNOverdueByStatusRepository : BaseRepository<Operator>
    {

        private DbContext _context;
        public RTIDoneNOverdueByStatusRepository(DbContext context) : base(context)
        {
            _context = context;
        }


        #region public methode

      
 

        public async Task<List<Operator>> GetChartDataByStatus(int Year, string customerid)
        {
            return await pGetChartDataByStatus(Year, customerid);
        }


        #endregion

        #region private methode

        private async Task<List<Operator>> pGetChartDataByStatus(int Year, string customerid)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "[dbo].[uspDashboardRTIOverdue]";
                command.Parameters.Add(command.CreateParameter("@vYear", Year));
                command.Parameters.Add(command.CreateParameter("@vCustomerID", customerid));
                command.Parameters.Add(command.CreateParameter("@vIsSummary", 1));

                
                return this.ReadTransaction(command).ToList();
            }
        }

        private async Task<List<Operator>> pGetChartDataByStatus2(int Year, string customerid)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "[dbo].[uspGetRTIDoneNOverdueChart]";
                command.Parameters.Add(command.CreateParameter("@vYear", Year));
                command.Parameters.Add(command.CreateParameter("@vCustomerID", customerid));
                command.Parameters.Add(command.CreateParameter("@vAction", 0));


                return this.ReadTransaction(command).ToList();
            }
        }



        #endregion


    }


}
