using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ARSystem.Domain.Models;
using ARSystem.Domain.DAL;
using System.Data;
using ARSystem.Domain.Repositories;


namespace ARSystem.Domain.Repositories 
{
	public class dwhDashboardPotentialRFITo1stBAPSBillingDetailRepository : BaseRepository<dwhDashboardPotentialRFITo1stBAPSBillingDetailModel>
    {
		private DbContext _context;
		public dwhDashboardPotentialRFITo1stBAPSBillingDetailRepository(DbContext context) : base(context)
		{
			_context = context;
		}

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

		public dwhDashboardPotentialRFITo1stBAPSBillingDetailModel Create(dwhDashboardPotentialRFITo1stBAPSBillingDetailModel data)
		{
			return pCreate(data);
		}

		public List<dwhDashboardPotentialRFITo1stBAPSBillingDetailModel> CreateBulky(List<dwhDashboardPotentialRFITo1stBAPSBillingDetailModel> data)
		{
			return pCreateBulky(data);
		}

		public dwhDashboardPotentialRFITo1stBAPSBillingDetailModel Update(dwhDashboardPotentialRFITo1stBAPSBillingDetailModel data)
		{
			return pUpdate(data);
		}

		public bool DeleteByPK(int iD)
		{
			pDeleteByPK(iD);
			return true;
		}

		public bool DeleteByFilter(string whereClause)
		{
			pDeleteByFilter(whereClause);
			return true;
		}

		#region Private

		private int pGetCount(string whereClause)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspdwhDashboardPotentialRFITo1stBAPSBillingDetail";

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
				command.CommandText = "dbo.uspdwhDashboardPotentialRFITo1stBAPSBillingDetail";

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
				command.CommandText = "dbo.uspdwhDashboardPotentialRFITo1stBAPSBillingDetail";

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
				command.CommandText = "dbo.uspdwhDashboardPotentialRFITo1stBAPSBillingDetail";

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
				command.CommandText = "dbo.uspdwhDashboardPotentialRFITo1stBAPSBillingDetail";

				command.Parameters.Add(command.CreateParameter("@vType", "Create"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));

				data.ID = this.WriteTransaction(command);

				return data;
			}
		}
		private List<dwhDashboardPotentialRFITo1stBAPSBillingDetailModel> pCreateBulky(List<dwhDashboardPotentialRFITo1stBAPSBillingDetailModel> data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<List<dwhDashboardPotentialRFITo1stBAPSBillingDetailModel>>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspdwhDashboardPotentialRFITo1stBAPSBillingDetail";

				command.Parameters.Add(command.CreateParameter("@vType", "CreateBulky"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));

				command.ExecuteNonQuery();

				return data;
			}
		}
		private dwhDashboardPotentialRFITo1stBAPSBillingDetailModel pUpdate(dwhDashboardPotentialRFITo1stBAPSBillingDetailModel data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<dwhDashboardPotentialRFITo1stBAPSBillingDetailModel>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspdwhDashboardPotentialRFITo1stBAPSBillingDetail";

				command.Parameters.Add(command.CreateParameter("@vType", "Update"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));
				command.Parameters.Add(command.CreateParameter("@ID", data.ID));

				command.ExecuteNonQuery();

				return data;
			}
		}
		private void pDeleteByPK(int iD)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspdwhDashboardPotentialRFITo1stBAPSBillingDetail";

				command.Parameters.Add(command.CreateParameter("@vType", "DeleteByPK"));
				command.Parameters.Add(command.CreateParameter("@ID", iD));

				command.ExecuteNonQuery();
			}
		}
		private void pDeleteByFilter(string whereClause)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspdwhDashboardPotentialRFITo1stBAPSBillingDetail";

				command.Parameters.Add(command.CreateParameter("@vType", "DeleteByFilter"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

				command.ExecuteNonQuery();
			}
		}
		#endregion
	}
}
