
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ARSystem.Domain.DAL;
using ARSystem.Domain.Models.TBIGSYSDB01.TBGSAPIntegration;

namespace ARSystem.Domain.Repositories.TBIGSYSDB01.TBGSAPIntegration
{
	public class stgTRStatusMatchingARLogErrorRepository : BaseRepository<stgTRStatusMatchingARLogError>
	{
		private DbContext _context;
		public stgTRStatusMatchingARLogErrorRepository(DbContext context) : base(context)
		{
			_context = context;
		}

		public int GetCount(string whereClause = "")
		{
			return pGetCount(whereClause);
		}

		public List<stgTRStatusMatchingARLogError> GetList(string whereClause = "", string orderBy = "")
		{
			return pGetList(whereClause, orderBy);
		}

		public List<stgTRStatusMatchingARLogError> GetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
		{
			return pGetPaged(whereClause, orderBy, rowSkip, pageSize);
		}

		public stgTRStatusMatchingARLogError GetByPK(long iD)
		{
			return pGetByPK(iD);
		}

		public stgTRStatusMatchingARLogError Create(stgTRStatusMatchingARLogError data)
		{
			return pCreate(data);
		}

		public List<stgTRStatusMatchingARLogError> CreateBulky(List<stgTRStatusMatchingARLogError> data)
		{
			return pCreateBulky(data);
		}

		public stgTRStatusMatchingARLogError Update(stgTRStatusMatchingARLogError data)
		{
			return pUpdate(data);
		}

		public bool DeleteByPK(long iD)
		{
			pDeleteByPK(iD);
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
				command.CommandText = "dbo.uspstgTRStatusMatchingARLogError";

				command.Parameters.Add(command.CreateParameter("@vType", "GetCount"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

				return this.CountTransaction(command);
			}
		}
		private List<stgTRStatusMatchingARLogError> pGetList(string whereClause, string orderBy)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspstgTRStatusMatchingARLogError";

				command.Parameters.Add(command.CreateParameter("@vType", "GetList"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
				command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));

				return this.ReadTransaction(command).ToList();
			}
		}
		private List<stgTRStatusMatchingARLogError> pGetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspstgTRStatusMatchingARLogError";

				command.Parameters.Add(command.CreateParameter("@vType", "GetPaged"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
				command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));
				command.Parameters.Add(command.CreateParameter("@vRowSkip", rowSkip));
				command.Parameters.Add(command.CreateParameter("@vPageSize", pageSize));

				return this.ReadTransaction(command).ToList();
			}
		}
		private stgTRStatusMatchingARLogError pGetByPK(long iD)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspstgTRStatusMatchingARLogError";

				command.Parameters.Add(command.CreateParameter("@vType", "GetByPK"));
				command.Parameters.Add(command.CreateParameter("@ID", iD));

				return this.ReadTransaction(command).SingleOrDefault();
			}
		}
		private stgTRStatusMatchingARLogError pCreate(stgTRStatusMatchingARLogError data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<stgTRStatusMatchingARLogError>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspstgTRStatusMatchingARLogError";

				command.Parameters.Add(command.CreateParameter("@vType", "Create"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));

				data.ID = this.WriteTransaction(command);

				return data;
			}
		}
		private List<stgTRStatusMatchingARLogError> pCreateBulky(List<stgTRStatusMatchingARLogError> data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<List<stgTRStatusMatchingARLogError>>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspstgTRStatusMatchingARLogError";

				command.Parameters.Add(command.CreateParameter("@vType", "CreateBulky"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));

				command.ExecuteNonQuery();

				return data;
			}
		}
		private stgTRStatusMatchingARLogError pUpdate(stgTRStatusMatchingARLogError data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<stgTRStatusMatchingARLogError>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspstgTRStatusMatchingARLogError";

				command.Parameters.Add(command.CreateParameter("@vType", "Update"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));
				command.Parameters.Add(command.CreateParameter("@ID", data.ID));

				command.ExecuteNonQuery();

				return data;
			}
		}
		private void pDeleteByPK(long iD)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspstgTRStatusMatchingARLogError";

				command.Parameters.Add(command.CreateParameter("@vType", "DeleteByPK"));
				command.Parameters.Add(command.CreateParameter("@ID", iD));

				command.ExecuteNonQuery();
			}
		}
		private void pDeleteByFilter(string whereClause)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspstgTRStatusMatchingARLogError";

				command.Parameters.Add(command.CreateParameter("@vType", "DeleteByFilter"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

				command.ExecuteNonQuery();
			}
		}
		#endregion

	}
}