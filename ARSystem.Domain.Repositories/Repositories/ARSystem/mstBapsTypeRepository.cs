
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
	public class mstBapsTypeRepository : BaseRepository<mstBapsType>
	{
		private DbContext _context;
		public mstBapsTypeRepository(DbContext context) : base(context)
		{
			_context = context;
		}

		public int GetCount(string whereClause = "")
		{
			return pGetCount(whereClause);
		}

		public List<mstBapsType> GetList(string whereClause = "", string orderBy = "")
		{
			return pGetList(whereClause, orderBy);
		}

		public List<mstBapsType> GetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
		{
			return pGetPaged(whereClause, orderBy, rowSkip, pageSize);
		}

		public mstBapsType GetByPK(int mstBapsTypeId)
		{
			return pGetByPK(mstBapsTypeId);
		}

		public mstBapsType Create(mstBapsType data)
		{
			return pCreate(data);
		}

		public List<mstBapsType> CreateBulky(List<mstBapsType> data)
		{
			return pCreateBulky(data);
		}

		public mstBapsType Update(mstBapsType data)
		{
			return pUpdate(data);
		}

		public bool DeleteByPK(int mstBapsTypeId)
		{
			pDeleteByPK(mstBapsTypeId);
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
				command.CommandText = "dbo.uspmstBapsType";

				command.Parameters.Add(command.CreateParameter("@vType", "GetCount"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

				return this.CountTransaction(command);
			}
		}
		private List<mstBapsType> pGetList(string whereClause, string orderBy)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspmstBapsType";

				command.Parameters.Add(command.CreateParameter("@vType", "GetList"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
				command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));

				return this.ReadTransaction(command).ToList();
			}
		}
		private List<mstBapsType> pGetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspmstBapsType";

				command.Parameters.Add(command.CreateParameter("@vType", "GetPaged"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
				command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));
				command.Parameters.Add(command.CreateParameter("@vRowSkip", rowSkip));
				command.Parameters.Add(command.CreateParameter("@vPageSize", pageSize));

				return this.ReadTransaction(command).ToList();
			}
		}
		private mstBapsType pGetByPK(int mstBapsTypeId)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspmstBapsType";

				command.Parameters.Add(command.CreateParameter("@vType", "GetByPK"));
				command.Parameters.Add(command.CreateParameter("@mstBapsTypeId", mstBapsTypeId));

				return this.ReadTransaction(command).SingleOrDefault();
			}
		}
		private mstBapsType pCreate(mstBapsType data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<mstBapsType>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspmstBapsType";

				command.Parameters.Add(command.CreateParameter("@vType", "Create"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));

				data.mstBapsTypeId = this.WriteTransaction(command);

				return data;
			}
		}
		private List<mstBapsType> pCreateBulky(List<mstBapsType> data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<List<mstBapsType>>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspmstBapsType";

				command.Parameters.Add(command.CreateParameter("@vType", "CreateBulky"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));

				command.ExecuteNonQuery();

				return data;
			}
		}
		private mstBapsType pUpdate(mstBapsType data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<mstBapsType>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspmstBapsType";

				command.Parameters.Add(command.CreateParameter("@vType", "Update"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));
				command.Parameters.Add(command.CreateParameter("@mstBapsTypeId", data.mstBapsTypeId));

				command.ExecuteNonQuery();

				return data;
			}
		}
		private void pDeleteByPK(int mstBapsTypeId)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspmstBapsType";

				command.Parameters.Add(command.CreateParameter("@vType", "DeleteByPK"));
				command.Parameters.Add(command.CreateParameter("@mstBapsTypeId", mstBapsTypeId));

				command.ExecuteNonQuery();
			}
		}
		private void pDeleteByFilter(string whereClause)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspmstBapsType";

				command.Parameters.Add(command.CreateParameter("@vType", "DeleteByFilter"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

				command.ExecuteNonQuery();
			}
		}
		#endregion

	}
}