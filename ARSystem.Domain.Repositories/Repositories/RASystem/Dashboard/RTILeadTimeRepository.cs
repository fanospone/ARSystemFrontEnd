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
    public class RTILeadTimeRepository : BaseRepository<RTILeadTimeModel>
    {

        private DbContext _context;
        public RTILeadTimeRepository(DbContext context) : base(context)
        {
            _context = context;
        }


        #region public methode

        public async Task<List<RTILeadTimeModel>> GetDataLeadeTime(int Year, string GroupBy)
        {
            return await pGetDataLeadeTime(Year, GroupBy);
        }


        public async Task<List<RTILeadTimeModel>> GetDataLeadeTimeAvg(int Year, string GroupBy)
        {
            return await pGetDataLeadeTimeAvg(Year, GroupBy);
        }

        public async Task<List<RTILeadTimeModel>> GetStatusReconcile(string CustomerID, int Year)
        {
            return await pGetStatusReconcile(CustomerID, Year);
        }

        public async Task<List<RTILeadTimeModel>> GetPagedStatusReconcile(string CustomerID, int Year, string CurrentStatus, string type, int rowSkip, int pageSize)
        {
            return await pGetPagedStatusReconcile(CustomerID, Year, CurrentStatus, type, rowSkip, pageSize);
        }

        public int GetCountStatusReconcile(string CustomerID, int Year, string CurrentStatus)
        {
            return pGetCountStatusReconcile(CustomerID, Year, CurrentStatus);
        }
        #endregion

        #region private methode
        private async Task<List<RTILeadTimeModel>> pGetDataLeadeTime(int Year, string GroupBy)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "[dbo].[uspDashboardLeadTimeRTI]";
                command.Parameters.Add(command.CreateParameter("@vYear", Year));
                command.Parameters.Add(command.CreateParameter("@vIsEverage", 0));
                command.Parameters.Add(command.CreateParameter("@vGroupBy", GroupBy));
                return this.ReadTransaction(command).ToList();
            }
        }

        private async Task<List<RTILeadTimeModel>> pGetDataLeadeTime2(int Year, string GroupBy)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "[dbo].[uspGetLeadTimeRTI]";
                command.Parameters.Add(command.CreateParameter("@vYear", Year));
                command.Parameters.Add(command.CreateParameter("@vType", 0));
                command.Parameters.Add(command.CreateParameter("@vGroupBy", GroupBy));
                return this.ReadTransaction(command).ToList();
            }
        }

        private async Task<List<RTILeadTimeModel>> pGetDataLeadeTimeAvg(int Year, string GroupBy)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "[dbo].[uspDashboardLeadTimeRTI]";
                command.Parameters.Add(command.CreateParameter("@vYear", Year));
                command.Parameters.Add(command.CreateParameter("@vIsEverage", 1));
                command.Parameters.Add(command.CreateParameter("@vGroupBy", GroupBy));
                return this.ReadTransaction(command).ToList();
            }
        }

        private async Task<List<RTILeadTimeModel>> pGetDataLeadeTimeAvg2(int Year, string GroupBy)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "[dbo].[uspGetLeadTimeRTI]";
                command.Parameters.Add(command.CreateParameter("@vYear", Year));
                command.Parameters.Add(command.CreateParameter("@vType", 1));
                command.Parameters.Add(command.CreateParameter("@vGroupBy", GroupBy));
                return this.ReadTransaction(command).ToList();
            }
        }

        private async Task<List<RTILeadTimeModel>> pGetStatusReconcile(string CustomerID, int Year)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "[dbo].[uspDAshboardCountStatusReconcile]";
                command.Parameters.Add(command.CreateParameter("@vYear", Year));
                command.Parameters.Add(command.CreateParameter("@vCustomerID", CustomerID));
                return this.ReadTransaction(command).ToList();
            }
        }

        private async Task<List<RTILeadTimeModel>> pGetPagedStatusReconcile(string CustomerID, int Year, string CurrentStatus, string type, int rowSkip, int pageSize)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "[dbo].[uspDAshboardCountStatusReconcile]";
                command.Parameters.Add(command.CreateParameter("@vYear", Year));
                command.Parameters.Add(command.CreateParameter("@vCustomerID", CustomerID));
                command.Parameters.Add(command.CreateParameter("@vType", type));
                command.Parameters.Add(command.CreateParameter("@vStatus", CurrentStatus));
                command.Parameters.Add(command.CreateParameter("@vRowSkip", rowSkip));
                command.Parameters.Add(command.CreateParameter("@vPageSize", pageSize));
                return this.ReadTransaction(command).ToList();
            }
        }

        private int pGetCountStatusReconcile(string CustomerID, int Year, string CurrentStatus)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "[dbo].[uspDAshboardCountStatusReconcile]";
                command.Parameters.Add(command.CreateParameter("@vYear", Year));
                command.Parameters.Add(command.CreateParameter("@vCustomerID", CustomerID));
                command.Parameters.Add(command.CreateParameter("@vStatus", CurrentStatus));
                command.Parameters.Add(command.CreateParameter("@vType", "GetCount"));
                return this.CountTransaction(command);
            }
        }


        #endregion


    }


}
