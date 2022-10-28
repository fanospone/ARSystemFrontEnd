
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
	public class mstArLogDateKpiRepository : BaseRepository<mstArLogDateKpi>
	{
		private DbContext _context;
		public mstArLogDateKpiRepository(DbContext context) : base(context)
		{
			_context = context;
		}

		public int GetCount(string whereClause = "")
		{
			return pGetCount(whereClause);
		}

		public List<mstArLogDateKpi> GetList(string whereClause = "", string orderBy = "")
		{
			return pGetList(whereClause, orderBy);
		}

		public List<mstArLogDateKpi> GetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
		{
			return pGetPaged(whereClause, orderBy, rowSkip, pageSize);
		}

		public mstArLogDateKpi GetByPK(int mstArLogDateKpiId)
		{
			return pGetByPK(mstArLogDateKpiId);
		}

		public mstArLogDateKpi Create(mstArLogDateKpi data)
		{
			return pCreate(data);
		}

		public List<mstArLogDateKpi> CreateBulky(List<mstArLogDateKpi> data)
		{
			return pCreateBulky(data);
		}

		public mstArLogDateKpi Update(mstArLogDateKpi data)
		{
			return pUpdate(data);
		}

		public bool DeleteByPK(int mstArLogDateKpiId)
		{
			pDeleteByPK(mstArLogDateKpiId);
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
				command.CommandText = "dbo.uspmstArLogDateKpi";

				command.Parameters.Add(command.CreateParameter("@vType", "GetCount"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

				return this.CountTransaction(command);
			}
		}
		private List<mstArLogDateKpi> pGetList(string whereClause, string orderBy)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspmstArLogDateKpi";

				command.Parameters.Add(command.CreateParameter("@vType", "GetList"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
				command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));

				return this.ReadTransaction(command).ToList();
			}
		}
		private List<mstArLogDateKpi> pGetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspmstArLogDateKpi";

				command.Parameters.Add(command.CreateParameter("@vType", "GetPaged"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
				command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));
				command.Parameters.Add(command.CreateParameter("@vRowSkip", rowSkip));
				command.Parameters.Add(command.CreateParameter("@vPageSize", pageSize));

				return this.ReadTransaction(command).ToList();
			}
		}
		private mstArLogDateKpi pGetByPK(int mstArLogDateKpiId)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspmstArLogDateKpi";

				command.Parameters.Add(command.CreateParameter("@vType", "GetByPK"));
				command.Parameters.Add(command.CreateParameter("@mstArLogDateKpiId", mstArLogDateKpiId));

				return this.ReadTransaction(command).SingleOrDefault();
			}
		}
		private mstArLogDateKpi pCreate(mstArLogDateKpi data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<mstArLogDateKpi>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspmstArLogDateKpi";

				command.Parameters.Add(command.CreateParameter("@vType", "Create"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));

				data.mstArLogDateKpiId = this.WriteTransaction(command);

				return data;
			}
		}
		private List<mstArLogDateKpi> pCreateBulky(List<mstArLogDateKpi> data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<List<mstArLogDateKpi>>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspmstArLogDateKpi";

				command.Parameters.Add(command.CreateParameter("@vType", "CreateBulky"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));

				command.ExecuteNonQuery();

				return data;
			}
		}
		private mstArLogDateKpi pUpdate(mstArLogDateKpi data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<mstArLogDateKpi>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspmstArLogDateKpi";

				command.Parameters.Add(command.CreateParameter("@vType", "Update"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));
				command.Parameters.Add(command.CreateParameter("@mstArLogDateKpiId", data.mstArLogDateKpiId));

				command.ExecuteNonQuery();

				return data;
			}
		}
		private void pDeleteByPK(int mstArLogDateKpiId)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspmstArLogDateKpi";

				command.Parameters.Add(command.CreateParameter("@vType", "DeleteByPK"));
				command.Parameters.Add(command.CreateParameter("@mstArLogDateKpiId", mstArLogDateKpiId));

				command.ExecuteNonQuery();
			}
		}
		private void pDeleteByFilter(string whereClause)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspmstArLogDateKpi";

				command.Parameters.Add(command.CreateParameter("@vType", "DeleteByFilter"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

				command.ExecuteNonQuery();
			}
		}
		#endregion

	}
}