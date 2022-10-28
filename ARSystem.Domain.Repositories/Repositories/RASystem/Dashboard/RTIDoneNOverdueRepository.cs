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
    public class RTIDoneNOverdueRepository : BaseRepository<RTINOverdueModel>
    {

        private DbContext _context;
        public RTIDoneNOverdueRepository(DbContext context) : base(context)
        {
            _context = context;
        }


        #region public methode

        public async Task<List<RTINOverdueModel>> GetPaged(string DataType, int Year, int Month, int RowSkip, int PageSize)
        {
            return await pGetPaged(DataType, Year, Month, RowSkip, PageSize);
        }
        public async Task<List<RTINOverdueModel>> GetChartData( int Year , string customerid)
        {
            return await pGetChartData(Year, customerid);
        }

     
        #endregion

        #region private methode
        private async Task<List<RTINOverdueModel>> pGetChartData(int Year, string customerid)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "[dbo].[uspDashboardRTIOverdue]";
                command.Parameters.Add(command.CreateParameter("@vYear", Year));
                command.Parameters.Add(command.CreateParameter("@vCustomerID", customerid));
                command.Parameters.Add(command.CreateParameter("@vIsSummary", 0));
                return this.ReadTransaction(command).ToList();
            }
        }

        private async Task<List<RTINOverdueModel>> pGetChartData2(int Year, string customerid)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "[dbo].[uspGetRTIDoneNOverdueChart]";
                command.Parameters.Add(command.CreateParameter("@vYear", Year));
                command.Parameters.Add(command.CreateParameter("@vCustomerID", customerid));
                command.Parameters.Add(command.CreateParameter("@vAction", 1));
                return this.ReadTransaction(command).ToList();
            }
        }

        private async Task<List<RTINOverdueModel>> pGetPaged(string DataType, int Year, int Month, int RowSkip, int PageSize)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "[dbo].[uspGetRTIDoneNOverdueChartDetail]";


                command.Parameters.Add(command.CreateParameter("@vDataType", DataType));
                command.Parameters.Add(command.CreateParameter("@vYear", Year));
                command.Parameters.Add(command.CreateParameter("@vMonth", Month));
                command.Parameters.Add(command.CreateParameter("@vRowSkip", RowSkip));
                command.Parameters.Add(command.CreateParameter("@vPageSize", PageSize));

                return this.ReadTransaction(command).ToList();
            }

        }

        //private async Task<RTINOverdueModel> pGetChartData(int year)
        //{
        //    RTINOverdueModel data = new RTINOverdueModel();
        //    using (var command = _context.CreateCommand())
        //    {


        //        command.CommandType = CommandType.StoredProcedure;
        //        command.CommandText = "[dbo].[uspGetRTIDoneNOverdueChart]";
        //        command.Parameters.Add(command.CreateParameter("@vYear", year));
        //        data.Data = command.ExecuteReader().DictionaryList2();
        //    }
        //    return data;
        //}



        #endregion


    }


}
