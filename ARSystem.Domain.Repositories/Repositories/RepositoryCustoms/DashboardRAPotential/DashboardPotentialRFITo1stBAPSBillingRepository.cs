
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
    public class DashboardPotentialRFITo1stBAPSBillingRepository : BaseRepository<dwhDashboardPotentialRFITo1stBAPSBillingDetailModel>
    {
        private DbContext _context;

        public DashboardPotentialRFITo1stBAPSBillingRepository(DbContext context) : base(context)
        {
            _context = context;
        }

		 
		public DataTable GetDashboardSummaryList(string Type, string STIP, string year, string month, string desc)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspGetDashboardPotentialRFITo1stBAPSBilling";

				command.Parameters.Add(command.CreateParameter("@vType", Type));

				command.Parameters.Add(command.CreateParameter("@vFilerSTIPCategory", STIP)); 
				command.Parameters.Add(command.CreateParameter("@vFilterRFIYear", year));
				command.Parameters.Add(command.CreateParameter("@vMonth", month));
				command.Parameters.Add(command.CreateParameter("@vDesc", desc));
	

				var data = command.ExecuteReader();
				DataTable dt = new DataTable();
				dt.Load(data);
				return dt;
			}
		}

		public DataTable GetFilterList(string Type)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspGetDashboardPotentialRFITo1stBAPSBilling";
				command.Parameters.Add(command.CreateParameter("@vType", Type)); 


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

		public List<dwhDashboardPotentialRFITo1stBAPSBillingDetailModel> GetList(string whereClause = "", string orderBy = "")
		{
			return pGetList(whereClause, orderBy);
		}

		public List<dwhDashboardPotentialRFITo1stBAPSBillingDetailModel> GetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
		{
			return pGetPaged(whereClause, orderBy, rowSkip, pageSize);
		}

		public dwhDashboardPotentialRFITo1stBAPSBillingDetailModel GetByPK(int iD)
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
				command.CommandText = "dbo.uspGetDashboardPotentialRFITo1stBAPSBilling";

				command.Parameters.Add(command.CreateParameter("@vType", "GetCount"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

				return this.CountTransaction(command);
			}
		}
		private List<dwhDashboardPotentialRFITo1stBAPSBillingDetailModel> pGetList(string whereClause, string orderBy)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspGetDashboardPotentialRFITo1stBAPSBilling";

				command.Parameters.Add(command.CreateParameter("@vType", "GetList"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
				command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));

				return this.ReadTransaction(command).ToList();
			}
		}
		private List<dwhDashboardPotentialRFITo1stBAPSBillingDetailModel> pGetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspGetDashboardPotentialRFITo1stBAPSBilling";

				command.Parameters.Add(command.CreateParameter("@vType", "GetPaged"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
				command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));
				command.Parameters.Add(command.CreateParameter("@vRowSkip", rowSkip));
				command.Parameters.Add(command.CreateParameter("@vPageSize", pageSize));

				return this.ReadTransaction(command).ToList();
			}
		}
		private dwhDashboardPotentialRFITo1stBAPSBillingDetailModel pGetByPK(int iD)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspGetDashboardPotentialRFITo1stBAPSBilling";

				command.Parameters.Add(command.CreateParameter("@vType", "GetByPK"));
				command.Parameters.Add(command.CreateParameter("@ID", iD));

				return this.ReadTransaction(command).SingleOrDefault();
			}
		}
		private dwhDashboardPotentialRFITo1stBAPSBillingDetailModel pCreate(dwhDashboardPotentialRFITo1stBAPSBillingDetailModel data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<dwhDashboardPotentialRFITo1stBAPSBillingDetailModel>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspGetDashboardPotentialRFITo1stBAPSBilling";

				command.Parameters.Add(command.CreateParameter("@vType", "Create"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));

				data.ID = this.WriteTransaction(command);

				return data;
			}
		}



		#endregion

	}
}
