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
    public class RTILeadTimeDetailRepository : BaseRepository<RTINOverdueDetailModel>
    {

        private DbContext _context;
        public RTILeadTimeDetailRepository(DbContext context) : base(context)
        {
            _context = context;
        }


        #region public methode

        public async Task<List<RTINOverdueDetailModel>> GetPaged(string LeadTime, int Year, string CustomerID, int RowSkip, int PageSize)
        {
            return await pGetPaged(LeadTime, Year, CustomerID, RowSkip, PageSize);
        }

        public async Task<List<RTINOverdueDetailModel>> GetList(string LeadTime, int Year, string CustomerID)
        {
            return await pGetList(LeadTime, Year, CustomerID);
        }

        public int GetCount(string LeadTime, int Year, string CustomerID)
        {
            return pGetCount(LeadTime, Year, CustomerID);
        }

        public async Task<List<RTINOverdueDetailModel>> GetPagedStatusReconcile(string CustomerID, int Year, string CurrentStatus, string type, int rowSkip, int pageSize)
        {
            return await pGetPagedStatusReconcile(CustomerID, Year, CurrentStatus, type, rowSkip, pageSize);
        }

        public int GetCountStatusReconcile(string CustomerID, int Year, string CurrentStatus)
        {
            return pGetCountStatusReconcile(CustomerID, Year, CurrentStatus);
        }

        #endregion

        #region private methode

        private async Task<List<RTINOverdueDetailModel>> pGetPaged(string LeadTime, int Year, string CustomerID, int RowSkip, int PageSize)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "[dbo].[uspDashboardLeadTimeRTIDetail]";

                command.Parameters.Add(command.CreateParameter("@vLeadTime", LeadTime));
                command.Parameters.Add(command.CreateParameter("@vCustomerID", CustomerID));
                command.Parameters.Add(command.CreateParameter("@vYear", Year));
                command.Parameters.Add(command.CreateParameter("@vRowSkip", RowSkip));
                command.Parameters.Add(command.CreateParameter("@vPageSize", PageSize));
                command.Parameters.Add(command.CreateParameter("@vType", "GetPaged"));

                return this.ReadTransaction(command).ToList();
            }

        }

        private async Task<List<RTINOverdueDetailModel>> pGetPaged2(string LeadTime, int Year, string CustomerID, int RowSkip, int PageSize)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "[dbo].[uspGetLeadTimeRTIDetail]";

                command.Parameters.Add(command.CreateParameter("@vLeadTime", LeadTime));
                command.Parameters.Add(command.CreateParameter("@vCustomerID", CustomerID));
                command.Parameters.Add(command.CreateParameter("@vYear", Year));
                command.Parameters.Add(command.CreateParameter("@vRowSkip", RowSkip));
                command.Parameters.Add(command.CreateParameter("@vPageSize", PageSize));
                command.Parameters.Add(command.CreateParameter("@vType", "GetPaged"));

                return this.ReadTransaction(command).ToList();
            }

        }


        private async Task<List<RTINOverdueDetailModel>> pGetList(string LeadTime, int Year, string CustomerID)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "[dbo].[uspDashboardLeadTimeRTIDetail]";

                command.Parameters.Add(command.CreateParameter("@vLeadTime", LeadTime));
                command.Parameters.Add(command.CreateParameter("@vCustomerID", CustomerID));
                command.Parameters.Add(command.CreateParameter("@vYear", Year));
                command.Parameters.Add(command.CreateParameter("@vType", "GetList"));

                return this.ReadTransaction(command).ToList();
            }

        }

        private async Task<List<RTINOverdueDetailModel>> pGetList2(string LeadTime, int Year, string CustomerID)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "[dbo].[uspGetLeadTimeRTIDetail]";

                command.Parameters.Add(command.CreateParameter("@vLeadTime", LeadTime));
                command.Parameters.Add(command.CreateParameter("@vCustomerID", CustomerID));
                command.Parameters.Add(command.CreateParameter("@vYear", Year));
                command.Parameters.Add(command.CreateParameter("@vType", "GetList"));

                return this.ReadTransaction(command).ToList();
            }

        }

        private int pGetCount(string LeadTime, int Year, string CustomerID)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "[dbo].[uspDashboardLeadTimeRTIDetail]";


                command.Parameters.Add(command.CreateParameter("@vLeadTime", LeadTime));
                command.Parameters.Add(command.CreateParameter("@vCustomerID", CustomerID));
                command.Parameters.Add(command.CreateParameter("@vYear", Year));
                command.Parameters.Add(command.CreateParameter("@vType", "GetCount"));
                return this.CountTransaction(command);
            }

        }

        private int pGetCount2(string LeadTime, int Year, string CustomerID)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "[dbo].[uspGetLeadTimeRTIDetail]";


                command.Parameters.Add(command.CreateParameter("@vLeadTime", LeadTime));
                command.Parameters.Add(command.CreateParameter("@vCustomerID", CustomerID));
                command.Parameters.Add(command.CreateParameter("@vYear", Year));
                command.Parameters.Add(command.CreateParameter("@vType", "GetCount"));
                return this.CountTransaction(command);
            }

        }


        private async Task<List<RTINOverdueDetailModel>> pGetPagedStatusReconcile(string CustomerID, int Year, string CurrentStatus, string type, int rowSkip, int pageSize)
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
