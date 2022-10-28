
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
    public class DashboardPotentialRecurringRepository : BaseRepository<dwhRAOutstandingTSELGenerateDetail>
    {
        private DbContext _context;
        public DashboardPotentialRecurringRepository(DbContext context) : base(context)
        {
            _context = context;
        }

         

        public DataTable GetDashboardAllOperatorOutstandingSummaryList(string Type, int? STIPDate, int? RFIDate, string SectionID,string SOWID, int? ProductID, int? STIPID, int? RegionalID, string CompanyID, string year, string month, string desc, string Customer)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspGetDashboardPotentialRecurring";

                command.Parameters.Add(command.CreateParameter("@vType", Type));

                command.Parameters.Add(command.CreateParameter("@vSTIPDate", STIPDate));
                command.Parameters.Add(command.CreateParameter("@vRFIDate", RFIDate));
                command.Parameters.Add(command.CreateParameter("@vSectionID", SectionID));
                command.Parameters.Add(command.CreateParameter("@vSOWID", SOWID));
                command.Parameters.Add(command.CreateParameter("@vProductID", ProductID));
                command.Parameters.Add(command.CreateParameter("@vSTIPID", STIPID));
                command.Parameters.Add(command.CreateParameter("@vRegionalID", RegionalID));
                command.Parameters.Add(command.CreateParameter("@vCompanyID", CompanyID));
				command.Parameters.Add(command.CreateParameter("@vYear", year));
				command.Parameters.Add(command.CreateParameter("@vMonth", month));
				command.Parameters.Add(command.CreateParameter("@vDesc", desc));
				command.Parameters.Add(command.CreateParameter("@vCustomer", Customer));

				var data = command.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(data);
                return dt;
            }
        }

		#region 
		public int GetCount(string whereClause = "")
		{
			return pGetCount(whereClause);
		}

		public List<dwhRAOutstandingTSELGenerateDetail> GetList(string whereClause = "", string orderBy = "")
		{
			return pGetList(whereClause, orderBy);
		}

		public List<dwhRAOutstandingTSELGenerateDetail> GetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
		{
			return pGetPaged(whereClause, orderBy, rowSkip, pageSize);
		}

		public dwhRAOutstandingTSELGenerateDetail GetByPK(int iD)
		{
			return pGetByPK(iD);
		}


		#endregion
		#region Private

		private int pGetCount(string whereClause)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspdwhDashboardRAPotentialRecurringDetail";

				command.Parameters.Add(command.CreateParameter("@vType", "GetCount"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

				return this.CountTransaction(command);
			}
		}
		private List<dwhRAOutstandingTSELGenerateDetail> pGetList(string whereClause, string orderBy)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspdwhDashboardRAPotentialRecurringDetail";
                command.CommandTimeout = 240;

				command.Parameters.Add(command.CreateParameter("@vType", "GetList"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
				command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));

				return this.ReadTransaction(command).ToList();
			}
		}
		private List<dwhRAOutstandingTSELGenerateDetail> pGetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspdwhDashboardRAPotentialRecurringDetail";

				command.Parameters.Add(command.CreateParameter("@vType", "GetPaged"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
				command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));
				command.Parameters.Add(command.CreateParameter("@vRowSkip", rowSkip));
				command.Parameters.Add(command.CreateParameter("@vPageSize", pageSize));

				return this.ReadTransaction(command).ToList();
			}
		}
		private dwhRAOutstandingTSELGenerateDetail pGetByPK(int iD)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspdwhDashboardRAPotentialRecurringDetail";

				command.Parameters.Add(command.CreateParameter("@vType", "GetByPK"));
				command.Parameters.Add(command.CreateParameter("@ID", iD));

				return this.ReadTransaction(command).SingleOrDefault();
			}
		}
		private dwhRAOutstandingTSELGenerateDetail pCreate(dwhRAOutstandingTSELGenerateDetail data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<dwhRAOutstandingTSELGenerateDetail>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspdwhDashboardRAPotentialRecurringDetail";

				command.Parameters.Add(command.CreateParameter("@vType", "Create"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));

				data.ID = this.WriteTransaction(command);

				return data;
			}
		}
 
		 
		 
		#endregion
	}
}
