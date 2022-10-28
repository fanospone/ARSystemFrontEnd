
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ARSystem.Domain.DAL;
using ARSystem.Domain.Models.TBIGSYSDB01.TBGARSystem; 

namespace ARSystem.Domain.Repositories.TBIGSYSDB01.TBGARSystem
{
	public class mstRevSysKursRepository : BaseRepository<mstRevSysKurs>
	{
		private DbContext _context;
		public mstRevSysKursRepository(DbContext context) : base(context)
		{
			_context = context;
		}

		public int GetCount(string whereClause = "")
		{
			return pGetCount(whereClause);
		}

		public List<mstRevSysKurs> GetList(string whereClause = "", string orderBy = "")
		{
			return pGetList(whereClause, orderBy);
		}

		public List<mstRevSysKurs> GetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
		{
			return pGetPaged(whereClause, orderBy, rowSkip, pageSize);
		}

		public mstRevSysKurs GetByPK(string currency, DateTime startDate, DateTime endDate)
		{
			return pGetByPK(currency, startDate, endDate);
		}

		public mstRevSysKurs Create(mstRevSysKurs data)
		{
			return pCreate(data);
		}

		public List<mstRevSysKurs> CreateBulky(List<mstRevSysKurs> data)
		{
			return pCreateBulky(data);
		}

		public mstRevSysKurs Update(mstRevSysKurs data)
		{
			return pUpdate(data);
		}

		public bool DeleteByPK(string currency, DateTime startDate, DateTime endDate)
		{
			pDeleteByPK(currency, startDate, endDate);
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
				command.CommandText = "dbo.uspmstRevSysKurs";

				command.Parameters.Add(command.CreateParameter("@vType", "GetCount"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

				return this.CountTransaction(command);
			}
		}
		private List<mstRevSysKurs> pGetList(string whereClause, string orderBy)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspmstRevSysKurs";

				command.Parameters.Add(command.CreateParameter("@vType", "GetList"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
				command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));

				return this.ReadTransaction(command).ToList();
			}
		}
		private List<mstRevSysKurs> pGetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspmstRevSysKurs";

				command.Parameters.Add(command.CreateParameter("@vType", "GetPaged"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
				command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));
				command.Parameters.Add(command.CreateParameter("@vRowSkip", rowSkip));
				command.Parameters.Add(command.CreateParameter("@vPageSize", pageSize));

				return this.ReadTransaction(command).ToList();
			}
		}
		private mstRevSysKurs pGetByPK(string currency, DateTime startDate, DateTime endDate)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspmstRevSysKurs";

				command.Parameters.Add(command.CreateParameter("@vType", "GetByPK"));
				command.Parameters.Add(command.CreateParameter("@Currency", currency));
				command.Parameters.Add(command.CreateParameter("@StartDate", startDate));
				command.Parameters.Add(command.CreateParameter("@EndDate", endDate));

				return this.ReadTransaction(command).SingleOrDefault();
			}
		}
		private mstRevSysKurs pCreate(mstRevSysKurs data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<mstRevSysKurs>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspmstRevSysKurs";

				command.Parameters.Add(command.CreateParameter("@vType", "Create"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));

				this.WriteTransaction(command);

				return data;
			}
		}
		private List<mstRevSysKurs> pCreateBulky(List<mstRevSysKurs> data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<List<mstRevSysKurs>>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspmstRevSysKurs";

				command.Parameters.Add(command.CreateParameter("@vType", "CreateBulky"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));

				command.ExecuteNonQuery();

				return data;
			}
		}
		private mstRevSysKurs pUpdate(mstRevSysKurs data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<mstRevSysKurs>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspmstRevSysKurs";

				command.Parameters.Add(command.CreateParameter("@vType", "Update"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));
				command.Parameters.Add(command.CreateParameter("@Currency", data.Currency));
				command.Parameters.Add(command.CreateParameter("@StartDate", data.UpdatedStartDate));
				command.Parameters.Add(command.CreateParameter("@EndDate", data.UpdatedEndDate));

				command.ExecuteNonQuery();

				return data;
			}
		}
		private void pDeleteByPK(string currency, DateTime startDate, DateTime endDate)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspmstRevSysKurs";

				command.Parameters.Add(command.CreateParameter("@vType", "DeleteByPK"));
				command.Parameters.Add(command.CreateParameter("@Currency", currency));
				command.Parameters.Add(command.CreateParameter("@StartDate", startDate));
				command.Parameters.Add(command.CreateParameter("@EndDate", endDate));

				command.ExecuteNonQuery();
			}
		}
		private void pDeleteByFilter(string whereClause)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspmstRevSysKurs";

				command.Parameters.Add(command.CreateParameter("@vType", "DeleteByFilter"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

				command.ExecuteNonQuery();
			}
		}
		#endregion

	}
}