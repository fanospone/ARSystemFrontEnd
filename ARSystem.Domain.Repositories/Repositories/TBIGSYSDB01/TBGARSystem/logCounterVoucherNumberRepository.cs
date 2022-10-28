
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
	public class logCounterVoucherNumberRepository : BaseRepository<logCounterVoucherNumber>
	{
		private DbContext _context;
		public logCounterVoucherNumberRepository(DbContext context) : base(context)
		{
			_context = context;
		}

		public int GetCount(string whereClause = "")
		{
			return pGetCount(whereClause);
		}

		public List<logCounterVoucherNumber> GetList(string whereClause = "", string orderBy = "")
		{
			return pGetList(whereClause, orderBy);
		}

		public List<logCounterVoucherNumber> GetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
		{
			return pGetPaged(whereClause, orderBy, rowSkip, pageSize);
		}

		public logCounterVoucherNumber GetByPK(int logCounterVoucherNumberId)
		{
			return pGetByPK(logCounterVoucherNumberId);
		}

		public logCounterVoucherNumber Create(logCounterVoucherNumber data)
		{
			return pCreate(data);
		}

		public List<logCounterVoucherNumber> CreateBulky(List<logCounterVoucherNumber> data)
		{
			return pCreateBulky(data);
		}

		public logCounterVoucherNumber Update(logCounterVoucherNumber data)
		{
			return pUpdate(data);
		}

		public bool DeleteByPK(int logCounterVoucherNumberId)
		{
			pDeleteByPK(logCounterVoucherNumberId);
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
				command.CommandText = "dbo.usplogCounterVoucherNumber";

				command.Parameters.Add(command.CreateParameter("@vType", "GetCount"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

				return this.CountTransaction(command);
			}
		}
		private List<logCounterVoucherNumber> pGetList(string whereClause, string orderBy)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usplogCounterVoucherNumber";

				command.Parameters.Add(command.CreateParameter("@vType", "GetList"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
				command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));

				return this.ReadTransaction(command).ToList();
			}
		}
		private List<logCounterVoucherNumber> pGetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usplogCounterVoucherNumber";

				command.Parameters.Add(command.CreateParameter("@vType", "GetPaged"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
				command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));
				command.Parameters.Add(command.CreateParameter("@vRowSkip", rowSkip));
				command.Parameters.Add(command.CreateParameter("@vPageSize", pageSize));

				return this.ReadTransaction(command).ToList();
			}
		}
		private logCounterVoucherNumber pGetByPK(int logCounterVoucherNumberId)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usplogCounterVoucherNumber";

				command.Parameters.Add(command.CreateParameter("@vType", "GetByPK"));
				command.Parameters.Add(command.CreateParameter("@logCounterVoucherNumberId", logCounterVoucherNumberId));

				return this.ReadTransaction(command).SingleOrDefault();
			}
		}
		private logCounterVoucherNumber pCreate(logCounterVoucherNumber data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<logCounterVoucherNumber>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usplogCounterVoucherNumber";

				command.Parameters.Add(command.CreateParameter("@vType", "Create"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));

				data.logCounterVoucherNumberId = this.WriteTransaction(command);

				return data;
			}
		}
		private List<logCounterVoucherNumber> pCreateBulky(List<logCounterVoucherNumber> data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<List<logCounterVoucherNumber>>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usplogCounterVoucherNumber";

				command.Parameters.Add(command.CreateParameter("@vType", "CreateBulky"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));

				command.ExecuteNonQuery();

				return data;
			}
		}
		private logCounterVoucherNumber pUpdate(logCounterVoucherNumber data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<logCounterVoucherNumber>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usplogCounterVoucherNumber";

				command.Parameters.Add(command.CreateParameter("@vType", "Update"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));
				command.Parameters.Add(command.CreateParameter("@logCounterVoucherNumberId", data.logCounterVoucherNumberId));

				command.ExecuteNonQuery();

				return data;
			}
		}
		private void pDeleteByPK(int logCounterVoucherNumberId)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usplogCounterVoucherNumber";

				command.Parameters.Add(command.CreateParameter("@vType", "DeleteByPK"));
				command.Parameters.Add(command.CreateParameter("@logCounterVoucherNumberId", logCounterVoucherNumberId));

				command.ExecuteNonQuery();
			}
		}
		private void pDeleteByFilter(string whereClause)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usplogCounterVoucherNumber";

				command.Parameters.Add(command.CreateParameter("@vType", "DeleteByFilter"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

				command.ExecuteNonQuery();
			}
		}
		#endregion

	}
}