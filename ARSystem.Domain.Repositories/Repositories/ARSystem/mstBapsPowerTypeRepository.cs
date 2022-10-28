
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
	public class mstBapsPowerTypeRepository : BaseRepository<mstBapsPowerType>
	{
		private DbContext _context;
		public mstBapsPowerTypeRepository(DbContext context) : base(context)
		{
			_context = context;
		}

		public int GetCount(string whereClause = "")
		{
			return pGetCount(whereClause);
		}

		public List<mstBapsPowerType> GetList(string whereClause = "", string orderBy = "")
		{
			return pGetList(whereClause, orderBy);
		}

		public List<mstBapsPowerType> GetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
		{
			return pGetPaged(whereClause, orderBy, rowSkip, pageSize);
		}

		public mstBapsPowerType GetByPK(string kodeType)
		{
			return pGetByPK(kodeType);
		}

		public mstBapsPowerType Create(mstBapsPowerType data)
		{
			return pCreate(data);
		}

		public List<mstBapsPowerType> CreateBulky(List<mstBapsPowerType> data)
		{
			return pCreateBulky(data);
		}

		public mstBapsPowerType Update(mstBapsPowerType data)
		{
			return pUpdate(data);
		}

		public bool DeleteByPK(string kodeType)
		{
			pDeleteByPK(kodeType);
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
				command.CommandText = "dbo.uspmstBapsPowerType";

				command.Parameters.Add(command.CreateParameter("@vType", "GetCount"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

				return this.CountTransaction(command);
			}
		}
		private List<mstBapsPowerType> pGetList(string whereClause, string orderBy)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspmstBapsPowerType";

				command.Parameters.Add(command.CreateParameter("@vType", "GetList"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
				command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));

				return this.ReadTransaction(command).ToList();
			}
		}
		private List<mstBapsPowerType> pGetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspmstBapsPowerType";

				command.Parameters.Add(command.CreateParameter("@vType", "GetPaged"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
				command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));
				command.Parameters.Add(command.CreateParameter("@vRowSkip", rowSkip));
				command.Parameters.Add(command.CreateParameter("@vPageSize", pageSize));

				return this.ReadTransaction(command).ToList();
			}
		}
		private mstBapsPowerType pGetByPK(string kodeType)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspmstBapsPowerType";

				command.Parameters.Add(command.CreateParameter("@vType", "GetByPK"));
				command.Parameters.Add(command.CreateParameter("@KodeType", kodeType));

				return this.ReadTransaction(command).SingleOrDefault();
			}
		}
		private mstBapsPowerType pCreate(mstBapsPowerType data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<mstBapsPowerType>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspmstBapsPowerType";

				command.Parameters.Add(command.CreateParameter("@vType", "Create"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));

				this.WriteTransaction(command);

				return data;
			}
		}
		private List<mstBapsPowerType> pCreateBulky(List<mstBapsPowerType> data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<List<mstBapsPowerType>>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspmstBapsPowerType";

				command.Parameters.Add(command.CreateParameter("@vType", "CreateBulky"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));

				command.ExecuteNonQuery();

				return data;
			}
		}
		private mstBapsPowerType pUpdate(mstBapsPowerType data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<mstBapsPowerType>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspmstBapsPowerType";

				command.Parameters.Add(command.CreateParameter("@vType", "Update"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));
				command.Parameters.Add(command.CreateParameter("@KodeType", data.KodeType));

				command.ExecuteNonQuery();

				return data;
			}
		}
		private void pDeleteByPK(string kodeType)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspmstBapsPowerType";

				command.Parameters.Add(command.CreateParameter("@vType", "DeleteByPK"));
				command.Parameters.Add(command.CreateParameter("@KodeType", kodeType));

				command.ExecuteNonQuery();
			}
		}
		private void pDeleteByFilter(string whereClause)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspmstBapsPowerType";

				command.Parameters.Add(command.CreateParameter("@vType", "DeleteByFilter"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

				command.ExecuteNonQuery();
			}
		}
		#endregion

	}
}