
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
	public class dwhTaskToDoRANewRepository : BaseRepository<dwhTaskToDoRANew>
	{
		private DbContext _context;
		public dwhTaskToDoRANewRepository(DbContext context) : base(context)
		{
			_context = context;
		}

		public int GetCount(string whereClause = "")
		{
			return pGetCount(whereClause);
		}

		public List<dwhTaskToDoRANew> GetList(string whereClause = "", string orderBy = "")
		{
			return pGetList(whereClause, orderBy);
		}

		public List<dwhTaskToDoRANew> GetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
		{
			return pGetPaged(whereClause, orderBy, rowSkip, pageSize);
		}

		public dwhTaskToDoRANew GetByPK(byte id, string toDoName)
		{
			return pGetByPK(id, toDoName);
		}

		public dwhTaskToDoRANew Create(dwhTaskToDoRANew data)
		{
			return pCreate(data);
		}

		public List<dwhTaskToDoRANew> CreateBulky(List<dwhTaskToDoRANew> data)
		{
			return pCreateBulky(data);
		}

		public dwhTaskToDoRANew Update(dwhTaskToDoRANew data)
		{
			return pUpdate(data);
		}

		public bool DeleteByPK(byte id, string toDoName)
		{
			pDeleteByPK(id, toDoName);
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
				command.CommandText = "dbo.uspdwhTaskToDoRANew";

				command.Parameters.Add(command.CreateParameter("@vType", "GetCount"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

				return this.CountTransaction(command);
			}
		}
		private List<dwhTaskToDoRANew> pGetList(string whereClause, string orderBy)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspdwhTaskToDoRANew";

				command.Parameters.Add(command.CreateParameter("@vType", "GetList"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
				command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));

				return this.ReadTransaction(command).ToList();
			}
		}
		private List<dwhTaskToDoRANew> pGetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspdwhTaskToDoRANew";

				command.Parameters.Add(command.CreateParameter("@vType", "GetPaged"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
				command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));
				command.Parameters.Add(command.CreateParameter("@vRowSkip", rowSkip));
				command.Parameters.Add(command.CreateParameter("@vPageSize", pageSize));

				return this.ReadTransaction(command).ToList();
			}
		}
		private dwhTaskToDoRANew pGetByPK(byte id, string toDoName)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspdwhTaskToDoRANew";

				command.Parameters.Add(command.CreateParameter("@vType", "GetByPK"));
				command.Parameters.Add(command.CreateParameter("@Id", id));
				command.Parameters.Add(command.CreateParameter("@ToDoName", toDoName));

				return this.ReadTransaction(command).SingleOrDefault();
			}
		}
		private dwhTaskToDoRANew pCreate(dwhTaskToDoRANew data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<dwhTaskToDoRANew>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspdwhTaskToDoRANew";

				command.Parameters.Add(command.CreateParameter("@vType", "Create"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));

				//data.Id = this.WriteTransaction(command);

				return data;
			}
		}
		private List<dwhTaskToDoRANew> pCreateBulky(List<dwhTaskToDoRANew> data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<List<dwhTaskToDoRANew>>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspdwhTaskToDoRANew";

				command.Parameters.Add(command.CreateParameter("@vType", "CreateBulky"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));

				command.ExecuteNonQuery();

				return data;
			}
		}
		private dwhTaskToDoRANew pUpdate(dwhTaskToDoRANew data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<dwhTaskToDoRANew>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspdwhTaskToDoRANew";

				command.Parameters.Add(command.CreateParameter("@vType", "Update"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));
				command.Parameters.Add(command.CreateParameter("@Id", data.Id));
				command.Parameters.Add(command.CreateParameter("@ToDoName", data.TaskToDoName));

				command.ExecuteNonQuery();

				return data;
			}
		}
		private void pDeleteByPK(byte id, string toDoName)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspdwhTaskToDoRANew";

				command.Parameters.Add(command.CreateParameter("@vType", "DeleteByPK"));
				command.Parameters.Add(command.CreateParameter("@Id", id));
				command.Parameters.Add(command.CreateParameter("@ToDoName", toDoName));

				command.ExecuteNonQuery();
			}
		}
		private void pDeleteByFilter(string whereClause)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspdwhTaskToDoRANew";

				command.Parameters.Add(command.CreateParameter("@vType", "DeleteByFilter"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

				command.ExecuteNonQuery();
			}
		}
		#endregion

	}
}