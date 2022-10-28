
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
	public class DashboardPotentialSIRORepository : BaseRepository<dwhDashboardRAPotentialSIRODetailModel>
	{
		private DbContext _context;
		public DashboardPotentialSIRORepository(DbContext context) : base(context)
		{
			_context = context;
		}



		public DataTable GetDashboardAllOperatorOutstandingSummaryList
		(
			string Type ,
			string Key ,
			string ProductID ,
			string STIPID ,
			string Customer ,
			string CompanyID ,
			string Year ,
			string Month ,
			string Desc 			
		)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspGetDashboardPotentialSIRO";

				command.Parameters.Add(command.CreateParameter("@vType", Type));
				command.Parameters.Add(command.CreateParameter("@vKey", Key));
				command.Parameters.Add(command.CreateParameter("@vProductID", ProductID));
				command.Parameters.Add(command.CreateParameter("@vSTIPID", STIPID));
				command.Parameters.Add(command.CreateParameter("@vCompanyID", CompanyID));
				command.Parameters.Add(command.CreateParameter("@vCustomer", Customer));
				command.Parameters.Add(command.CreateParameter("@vYear", Year));
				command.Parameters.Add(command.CreateParameter("@vMonth", Month));
				command.Parameters.Add(command.CreateParameter("@vDesc", Desc));

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

		public List<dwhDashboardRAPotentialSIRODetailModel> GetList(string whereClause = "", string orderBy = "")
		{
			return pGetList(whereClause, orderBy);
		}

		public List<dwhDashboardRAPotentialSIRODetailModel> GetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
		{
			return pGetPaged(whereClause, orderBy, rowSkip, pageSize);
		}

		public dwhDashboardRAPotentialSIRODetailModel GetByPK(int iD)
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
				command.CommandText = "dbo.uspdwhDashboardRAPotentialSIRODetail";

				command.Parameters.Add(command.CreateParameter("@vType", "GetCount"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

				return this.CountTransaction(command);
			}
		}
		private List<dwhDashboardRAPotentialSIRODetailModel> pGetList(string whereClause, string orderBy)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspdwhDashboardRAPotentialSIRODetail";

				command.Parameters.Add(command.CreateParameter("@vType", "GetList"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
				command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));

				return this.ReadTransaction(command).ToList();
			}
		}
		private List<dwhDashboardRAPotentialSIRODetailModel> pGetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspdwhDashboardRAPotentialSIRODetail";

				command.Parameters.Add(command.CreateParameter("@vType", "GetPaged"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
				command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));
				command.Parameters.Add(command.CreateParameter("@vRowSkip", rowSkip));
				command.Parameters.Add(command.CreateParameter("@vPageSize", pageSize));

				return this.ReadTransaction(command).ToList();
			}
		}
		private dwhDashboardRAPotentialSIRODetailModel pGetByPK(int iD)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspdwhDashboardRAPotentialSIRODetail";

				command.Parameters.Add(command.CreateParameter("@vType", "GetByPK"));
				command.Parameters.Add(command.CreateParameter("@ID", iD));

				return this.ReadTransaction(command).SingleOrDefault();
			}
		}
		private dwhDashboardRAPotentialSIRODetailModel pCreate(dwhDashboardRAPotentialSIRODetailModel data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<dwhDashboardRAPotentialSIRODetailModel>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspdwhDashboardRAPotentialSIRODetail";

				command.Parameters.Add(command.CreateParameter("@vType", "Create"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));

				data.ID = this.WriteTransaction(command);

				return data;
			}
		}



		#endregion
	}
}
