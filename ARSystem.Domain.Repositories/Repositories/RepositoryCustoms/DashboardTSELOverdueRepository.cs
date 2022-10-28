
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
    public class DashboardTSELOverdueRepository : BaseRepository<vmDashboardTSELOverdue>
    {
        private DbContext _context;
        public DashboardTSELOverdueRepository(DbContext context) : base(context)
        {
            _context = context;
        }

        public List<vmDashboardTSELOverdue> GetDataTSELOverduePercentage(string vType, string vYearBill)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspGetDashboardTSELOverdue";

                command.Parameters.Add(command.CreateParameter("@vType", vType));
                command.Parameters.Add(command.CreateParameter("@vYearBill", vYearBill));
                //return Convert.ToDecimal(command.ExecuteScalar().ToString());
                return this.ReadTransaction(command).ToList();
            }
        }

        public List<vmDashboardTSELOverdue> GetDataTSELOverdueChartList(string vType, string vYearBill)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspGetDashboardTSELOverdue";

                command.Parameters.Add(command.CreateParameter("@vType", vType));
                command.Parameters.Add(command.CreateParameter("@vYearBill", vYearBill));
                return this.ReadTransaction(command).ToList();
            }
        }

        public List<vmDashboardTSELOverdue> GetMasterSTIPList()
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspGetDashboardTSELOverdue";

                command.Parameters.Add(command.CreateParameter("@vType", "GetListMasterSTIP"));                
                return this.ReadTransaction(command).ToList();
            }
        }
        public List<vmDashboardTSELOverdue> GetMasterCompanyList()
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspGetDashboardTSELOverdue";

                command.Parameters.Add(command.CreateParameter("@vType", "GetListCompany"));
                return this.ReadTransaction(command).ToList();
            }
        }
        public List<vmDashboardTSELOverdue> GetSOWList(string SectionID)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspGetDashboardTSELOverdue";

                command.Parameters.Add(command.CreateParameter("@vType", "GetListSOW"));               
                command.Parameters.Add(command.CreateParameter("@vSectionID", SectionID));
                return this.ReadTransaction(command).ToList();
            }
        }
        public List<vmDashboardTSELOverdue> GetProductList(string SectionID, string SowID)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspGetDashboardTSELOverdue";

                command.Parameters.Add(command.CreateParameter("@vType", "GetListProduct"));
                command.Parameters.Add(command.CreateParameter("@vSowID", SowID));
                command.Parameters.Add(command.CreateParameter("@vSectionID", SectionID));
                return this.ReadTransaction(command).ToList();
            }
        }

        public DataTable GetDashboardTSELOutstandingSummaryList(string Type, int? STIPDate, int? RFIDate, int? SectionID, int? SOWID, int? ProductID, int? STIPID, int? RegionalID, string CompanyID)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspGetDashboardTSELOutstanding";

                command.Parameters.Add(command.CreateParameter("@vType", Type));

                command.Parameters.Add(command.CreateParameter("@vSTIPDate", STIPDate));
                command.Parameters.Add(command.CreateParameter("@vRFIDate", RFIDate));
                command.Parameters.Add(command.CreateParameter("@vSectionID", SectionID));
                command.Parameters.Add(command.CreateParameter("@vSOWID", SOWID));
                command.Parameters.Add(command.CreateParameter("@vProductID", ProductID));
                command.Parameters.Add(command.CreateParameter("@vSTIPID", STIPID));
                command.Parameters.Add(command.CreateParameter("@vRegionalID", RegionalID));
                command.Parameters.Add(command.CreateParameter("@vCompanyID", CompanyID)); 

                var data = command.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(data);
                return dt;
            }
        }

    }
}
