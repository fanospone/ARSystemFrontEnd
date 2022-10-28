
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
	public class dwhTaskToDoRANewDetailRepository : BaseRepository<dwhTaskToDoRANewDetail>
	{
		private DbContext _context;
		public dwhTaskToDoRANewDetailRepository(DbContext context) : base(context)
		{
			_context = context;
		}

		public int GetCount(string whereClause = "")
		{
			return pGetCount(whereClause);
		}

		public List<dwhTaskToDoRANewDetail> GetList(string whereClause = "", string orderBy = "")
		{
			return pGetList(whereClause, orderBy);
		}

		public List<dwhTaskToDoRANewDetail> GetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
		{
			return pGetPaged(whereClause, orderBy, rowSkip, pageSize);
		}

		public dwhTaskToDoRANewDetail GetByPK(short id, string customerId, string taskToDoName)
		{
			return pGetByPK(id, customerId, taskToDoName);
		}

		public dwhTaskToDoRANewDetail Create(dwhTaskToDoRANewDetail data)
		{
			return pCreate(data);
		}

		public List<dwhTaskToDoRANewDetail> CreateBulky(List<dwhTaskToDoRANewDetail> data)
		{
			return pCreateBulky(data);
		}

		public dwhTaskToDoRANewDetail Update(dwhTaskToDoRANewDetail data)
		{
			return pUpdate(data);
		}

		public bool DeleteByPK(short id, string customerId, string taskToDoName)
		{
			pDeleteByPK(id, customerId, taskToDoName);
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
				command.CommandText = "dbo.uspdwhTaskToDoRANewDetail";

				command.Parameters.Add(command.CreateParameter("@vType", "GetCount"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

				return this.CountTransaction(command);
			}
		}
		private List<dwhTaskToDoRANewDetail> pGetList(string whereClause, string orderBy)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspdwhTaskToDoRANewDetail";

				command.Parameters.Add(command.CreateParameter("@vType", "GetList"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
				command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));

				return this.ReadTransaction(command).ToList();
			}
		}
		private List<dwhTaskToDoRANewDetail> pGetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspdwhTaskToDoRANewDetail";

				command.Parameters.Add(command.CreateParameter("@vType", "GetPaged"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
				command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));
				command.Parameters.Add(command.CreateParameter("@vRowSkip", rowSkip));
				command.Parameters.Add(command.CreateParameter("@vPageSize", pageSize));

				return this.ReadTransaction(command).ToList();
			}
		}
		private dwhTaskToDoRANewDetail pGetByPK(short id, string customerId, string taskToDoName)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspdwhTaskToDoRANewDetail";

				command.Parameters.Add(command.CreateParameter("@vType", "GetByPK"));
				command.Parameters.Add(command.CreateParameter("@Id", id));
				command.Parameters.Add(command.CreateParameter("@CustomerId", customerId));
				command.Parameters.Add(command.CreateParameter("@TaskToDoName", taskToDoName));

				return this.ReadTransaction(command).SingleOrDefault();
			}
		}
		private dwhTaskToDoRANewDetail pCreate(dwhTaskToDoRANewDetail data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<dwhTaskToDoRANewDetail>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspdwhTaskToDoRANewDetail";

				command.Parameters.Add(command.CreateParameter("@vType", "Create"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));

				//data.Id = this.WriteTransaction(command);

				return data;
			}
		}
		private List<dwhTaskToDoRANewDetail> pCreateBulky(List<dwhTaskToDoRANewDetail> data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<List<dwhTaskToDoRANewDetail>>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspdwhTaskToDoRANewDetail";

				command.Parameters.Add(command.CreateParameter("@vType", "CreateBulky"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));

				command.ExecuteNonQuery();

				return data;
			}
		}
		private dwhTaskToDoRANewDetail pUpdate(dwhTaskToDoRANewDetail data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<dwhTaskToDoRANewDetail>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspdwhTaskToDoRANewDetail";

				command.Parameters.Add(command.CreateParameter("@vType", "Update"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));
				command.Parameters.Add(command.CreateParameter("@Id", data.Id));
				command.Parameters.Add(command.CreateParameter("@CustomerId", data.CustomerId));
				command.Parameters.Add(command.CreateParameter("@TaskToDoName", data.TaskToDoName));

				command.ExecuteNonQuery();

				return data;
			}
		}
		private void pDeleteByPK(short id, string customerId, string taskToDoName)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspdwhTaskToDoRANewDetail";

				command.Parameters.Add(command.CreateParameter("@vType", "DeleteByPK"));
				command.Parameters.Add(command.CreateParameter("@Id", id));
				command.Parameters.Add(command.CreateParameter("@CustomerId", customerId));
				command.Parameters.Add(command.CreateParameter("@TaskToDoName", taskToDoName));

				command.ExecuteNonQuery();
			}
		}
		private void pDeleteByFilter(string whereClause)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspdwhTaskToDoRANewDetail";

				command.Parameters.Add(command.CreateParameter("@vType", "DeleteByFilter"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

				command.ExecuteNonQuery();
			}
		}
		#endregion

	}
}