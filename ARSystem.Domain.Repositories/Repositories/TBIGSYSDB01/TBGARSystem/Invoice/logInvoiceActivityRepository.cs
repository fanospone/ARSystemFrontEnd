
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
	public class logInvoiceActivityRepository : BaseRepository<logInvoiceActivity>
	{
		private DbContext _context;
		public logInvoiceActivityRepository(DbContext context) : base(context)
		{
			_context = context;
		}

		public int GetCount(string whereClause = "")
		{
			return pGetCount(whereClause);
		}

		public List<logInvoiceActivity> GetList(string whereClause = "", string orderBy = "")
		{
			return pGetList(whereClause, orderBy);
		}

		public List<logInvoiceActivity> GetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
		{
			return pGetPaged(whereClause, orderBy, rowSkip, pageSize);
		}

		public logInvoiceActivity GetByPK(int logInvoiceActivityId)
		{
			return pGetByPK(logInvoiceActivityId);
		}

		public logInvoiceActivity Create(logInvoiceActivity data)
		{
			return pCreate(data);
		}

		public List<logInvoiceActivity> CreateBulky(List<logInvoiceActivity> data)
		{
			return pCreateBulky(data);
		}

		public logInvoiceActivity Update(logInvoiceActivity data)
		{
			return pUpdate(data);
		}

		public bool DeleteByPK(int logInvoiceActivityId)
		{
			pDeleteByPK(logInvoiceActivityId);
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
				command.CommandText = "dbo.usplogInvoiceActivity";

				command.Parameters.Add(command.CreateParameter("@vType", "GetCount"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

				return this.CountTransaction(command);
			}
		}
		private List<logInvoiceActivity> pGetList(string whereClause, string orderBy)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usplogInvoiceActivity";

				command.Parameters.Add(command.CreateParameter("@vType", "GetList"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
				command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));

				return this.ReadTransaction(command).ToList();
			}
		}
		private List<logInvoiceActivity> pGetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usplogInvoiceActivity";

				command.Parameters.Add(command.CreateParameter("@vType", "GetPaged"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
				command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));
				command.Parameters.Add(command.CreateParameter("@vRowSkip", rowSkip));
				command.Parameters.Add(command.CreateParameter("@vPageSize", pageSize));

				return this.ReadTransaction(command).ToList();
			}
		}
		private logInvoiceActivity pGetByPK(int logInvoiceActivityId)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usplogInvoiceActivity";

				command.Parameters.Add(command.CreateParameter("@vType", "GetByPK"));
				command.Parameters.Add(command.CreateParameter("@logInvoiceActivityId", logInvoiceActivityId));

				return this.ReadTransaction(command).SingleOrDefault();
			}
		}
		private logInvoiceActivity pCreate(logInvoiceActivity data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<logInvoiceActivity>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usplogInvoiceActivity";

				command.Parameters.Add(command.CreateParameter("@vType", "Create"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));

				data.logInvoiceActivityId = this.WriteTransaction(command);

				return data;
			}
		}
		private List<logInvoiceActivity> pCreateBulky(List<logInvoiceActivity> data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<List<logInvoiceActivity>>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usplogInvoiceActivity";

				command.Parameters.Add(command.CreateParameter("@vType", "CreateBulky"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));

				command.ExecuteNonQuery();

				return data;
			}
		}
		private logInvoiceActivity pUpdate(logInvoiceActivity data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<logInvoiceActivity>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usplogInvoiceActivity";

				command.Parameters.Add(command.CreateParameter("@vType", "Update"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));
				command.Parameters.Add(command.CreateParameter("@logInvoiceActivityId", data.logInvoiceActivityId));

				command.ExecuteNonQuery();

				return data;
			}
		}
		private void pDeleteByPK(int logInvoiceActivityId)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usplogInvoiceActivity";

				command.Parameters.Add(command.CreateParameter("@vType", "DeleteByPK"));
				command.Parameters.Add(command.CreateParameter("@logInvoiceActivityId", logInvoiceActivityId));

				command.ExecuteNonQuery();
			}
		}
		private void pDeleteByFilter(string whereClause)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usplogInvoiceActivity";

				command.Parameters.Add(command.CreateParameter("@vType", "DeleteByFilter"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

				command.ExecuteNonQuery();
			}
		}
		#endregion

	}
}