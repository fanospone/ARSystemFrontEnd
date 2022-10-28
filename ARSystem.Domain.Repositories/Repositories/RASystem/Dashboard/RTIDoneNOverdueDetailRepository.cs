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
    public class RTIDoneNOverdueDetailRepository : BaseRepository<RTINOverdueDetailModel>
    {

        private DbContext _context;
        public RTIDoneNOverdueDetailRepository(DbContext context) : base(context)
        {
            _context = context;
        }


        #region public methode

        public async Task<List<RTINOverdueDetailModel>> GetPaged(string DataType, int Year, int Month, string CustomerID, int RowSkip, int PageSize)
        {
            return await pGetPaged(DataType, Year, Month, CustomerID, RowSkip, PageSize);
        }

        public async Task<List<RTINOverdueDetailModel>> GetList(string DataType, int Year, int Month, string CustomerID)
        {
            return await pGetList(DataType, Year, Month, CustomerID);
        }

        public int GetCount(string DataType, int Year, int Month, string CustomerID)
        {
            return pGetCount(DataType, Year, Month, CustomerID);
        }

        #endregion

        #region private methode

        private async Task<List<RTINOverdueDetailModel>> pGetPaged(string DataType, int Year, int Month, string CustomerID, int RowSkip, int PageSize)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "[dbo].[uspDashboardRTIOverdueDetail]";


                command.Parameters.Add(command.CreateParameter("@vDataType", DataType));
                command.Parameters.Add(command.CreateParameter("@vYear", Year));
                command.Parameters.Add(command.CreateParameter("@vMonth", Month));
                command.Parameters.Add(command.CreateParameter("@vCustomerID", CustomerID));
                command.Parameters.Add(command.CreateParameter("@vRowSkip", RowSkip));
                command.Parameters.Add(command.CreateParameter("@vPageSize", PageSize));
                command.Parameters.Add(command.CreateParameter("@vType", "GetPaged"));

                return this.ReadTransaction(command).ToList();
            }

        }

        private async Task<List<RTINOverdueDetailModel>> pGetPaged2(string DataType, int Year, int Month, string CustomerID, int RowSkip, int PageSize)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "[dbo].[uspGetRTIDoneNOverdueChartDetail]";


                command.Parameters.Add(command.CreateParameter("@vDataType", DataType));
                command.Parameters.Add(command.CreateParameter("@vYear", Year));
                command.Parameters.Add(command.CreateParameter("@vMonth", Month));
                command.Parameters.Add(command.CreateParameter("@vCustomerID", CustomerID));
                command.Parameters.Add(command.CreateParameter("@vRowSkip", RowSkip));
                command.Parameters.Add(command.CreateParameter("@vPageSize", PageSize));
                command.Parameters.Add(command.CreateParameter("@vType", "GetPaged"));

                return this.ReadTransaction(command).ToList();
            }

        }


        private async Task<List<RTINOverdueDetailModel>> pGetList(string DataType, int Year, int Month, string CustomerID)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "[dbo].[uspDashboardRTIOverdueDetail]";


                command.Parameters.Add(command.CreateParameter("@vDataType", DataType));
                command.Parameters.Add(command.CreateParameter("@vYear", Year));
                command.Parameters.Add(command.CreateParameter("@vMonth", Month));
                command.Parameters.Add(command.CreateParameter("@vCustomerID", CustomerID));
                command.Parameters.Add(command.CreateParameter("@vType", "GetList"));

                return this.ReadTransaction(command).ToList();
            }

        }

        private async Task<List<RTINOverdueDetailModel>> pGetList2(string DataType, int Year, int Month, string CustomerID)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "[dbo].[uspGetRTIDoneNOverdueChartDetail]";


                command.Parameters.Add(command.CreateParameter("@vDataType", DataType));
                command.Parameters.Add(command.CreateParameter("@vYear", Year));
                command.Parameters.Add(command.CreateParameter("@vMonth", Month));
                command.Parameters.Add(command.CreateParameter("@vCustomerID", CustomerID));
                command.Parameters.Add(command.CreateParameter("@vType", "GetList"));

                return this.ReadTransaction(command).ToList();
            }

        }

        private int pGetCount(string DataType, int Year, int Month, string CustomerID)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "[dbo].[uspDashboardRTIOverdueDetail]";


                command.Parameters.Add(command.CreateParameter("@vDataType", DataType));
                command.Parameters.Add(command.CreateParameter("@vYear", Year));
                command.Parameters.Add(command.CreateParameter("@vMonth", Month));
                command.Parameters.Add(command.CreateParameter("@vCustomerID", CustomerID));
                command.Parameters.Add(command.CreateParameter("@vType", "GetCount"));
                return this.CountTransaction(command);
            }

        }

        private int pGetCount2(string DataType, int Year, int Month, string CustomerID)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "[dbo].[uspGetRTIDoneNOverdueChartDetail]";


                command.Parameters.Add(command.CreateParameter("@vDataType", DataType));
                command.Parameters.Add(command.CreateParameter("@vYear", Year));
                command.Parameters.Add(command.CreateParameter("@vMonth", Month));
                command.Parameters.Add(command.CreateParameter("@vCustomerID", CustomerID));
                command.Parameters.Add(command.CreateParameter("@vType", "GetCount"));
                return this.CountTransaction(command);
            }

        }

        #endregion


    }


}
