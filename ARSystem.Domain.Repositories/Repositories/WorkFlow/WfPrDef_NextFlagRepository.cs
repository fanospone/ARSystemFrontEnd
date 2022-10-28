
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
	public class WfPrDef_NextFlagRepository : BaseRepository<WfPrDef_NextFlag>
	{
		private DbContext _context;
		public WfPrDef_NextFlagRepository(DbContext context) : base(context)
		{
			_context = context;
		}

		public int GetCount(string whereClause = "")
		{
			return pGetCount(whereClause);
		}

		public List<WfPrDef_NextFlag> GetList(string whereClause = "", string orderBy = "")
		{
			return pGetList(whereClause, orderBy);
		}

		public List<WfPrDef_NextFlag> GetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
		{
			return pGetPaged(whereClause, orderBy, rowSkip, pageSize);
		}

		public WfPrDef_NextFlag GetByPK(int flagID)
		{
			return pGetByPK(flagID);
		}

		public WfPrDef_NextFlag Create(WfPrDef_NextFlag data)
		{
			return pCreate(data);
		}

		public List<WfPrDef_NextFlag> CreateBulky(List<WfPrDef_NextFlag> data)
		{
			return pCreateBulky(data);
		}

		public WfPrDef_NextFlag Update(WfPrDef_NextFlag data)
		{
			return pUpdate(data);
		}

		public bool DeleteByPK(int flagID)
		{
			pDeleteByPK(flagID);
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
				command.CommandText = "tbg.uspWfPrDef_NextFlag";

				command.Parameters.Add(command.CreateParameter("@vType", "GetCount"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

				return this.CountTransaction(command);
			}
		}
		private List<WfPrDef_NextFlag> pGetList(string whereClause, string orderBy)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "tbg.uspWfPrDef_NextFlag";

				command.Parameters.Add(command.CreateParameter("@vType", "GetList"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
				command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));

				return this.ReadTransaction(command).ToList();
			}
		}
		private List<WfPrDef_NextFlag> pGetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "tbg.uspWfPrDef_NextFlag";

				command.Parameters.Add(command.CreateParameter("@vType", "GetPaged"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
				command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));
				command.Parameters.Add(command.CreateParameter("@vRowSkip", rowSkip));
				command.Parameters.Add(command.CreateParameter("@vPageSize", pageSize));

				return this.ReadTransaction(command).ToList();
			}
		}
		private WfPrDef_NextFlag pGetByPK(int flagID)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "tbg.uspWfPrDef_NextFlag";

				command.Parameters.Add(command.CreateParameter("@vType", "GetByPK"));
				command.Parameters.Add(command.CreateParameter("@FlagID", flagID));

				return this.ReadTransaction(command).SingleOrDefault();
			}
		}
		private WfPrDef_NextFlag pCreate(WfPrDef_NextFlag data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<WfPrDef_NextFlag>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "tbg.uspWfPrDef_NextFlag";

				command.Parameters.Add(command.CreateParameter("@vType", "Create"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));

				data.FlagID = this.WriteTransaction(command);

				return data;
			}
		}
		private List<WfPrDef_NextFlag> pCreateBulky(List<WfPrDef_NextFlag> data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<List<WfPrDef_NextFlag>>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "tbg.uspWfPrDef_NextFlag";

				command.Parameters.Add(command.CreateParameter("@vType", "CreateBulky"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));

				command.ExecuteNonQuery();

				return data;
			}
		}
		private WfPrDef_NextFlag pUpdate(WfPrDef_NextFlag data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<WfPrDef_NextFlag>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "tbg.uspWfPrDef_NextFlag";

				command.Parameters.Add(command.CreateParameter("@vType", "Update"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));
				command.Parameters.Add(command.CreateParameter("@FlagID", data.FlagID));

				command.ExecuteNonQuery();

				return data;
			}
		}
		private void pDeleteByPK(int flagID)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "tbg.uspWfPrDef_NextFlag";

				command.Parameters.Add(command.CreateParameter("@vType", "DeleteByPK"));
				command.Parameters.Add(command.CreateParameter("@FlagID", flagID));

				command.ExecuteNonQuery();
			}
		}
		private void pDeleteByFilter(string whereClause)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "tbg.uspWfPrDef_NextFlag";

				command.Parameters.Add(command.CreateParameter("@vType", "DeleteByFilter"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

				command.ExecuteNonQuery();
			}
		}
		#endregion

	}
}