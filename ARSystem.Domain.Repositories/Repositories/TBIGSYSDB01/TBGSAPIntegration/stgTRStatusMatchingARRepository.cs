
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
	public class stgTRStatusMatchingARRepository : BaseRepository<stgTRStatusMatchingAR>
	{
		private DbContext _context;
		public stgTRStatusMatchingARRepository(DbContext context) : base(context)
		{
			_context = context;
		}

		public int GetCount(string whereClause = "")
		{
			return pGetCount(whereClause);
		}

		public List<stgTRStatusMatchingAR> GetList(string whereClause = "", string orderBy = "")
		{
			return pGetList(whereClause, orderBy);
		}

		public List<stgTRStatusMatchingAR> GetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
		{
			return pGetPaged(whereClause, orderBy, rowSkip, pageSize);
		}

		public stgTRStatusMatchingAR GetByPK(long iD)
		{
			return pGetByPK(iD);
		}

		public stgTRStatusMatchingAR Create(stgTRStatusMatchingAR data)
		{
			return pCreate(data);
		}

		public List<stgTRStatusMatchingAR> CreateBulky(List<stgTRStatusMatchingAR> data)
		{
			return pCreateBulky(data);
		}

		public stgTRStatusMatchingAR Update(stgTRStatusMatchingAR data)
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
				command.CommandText = "dbo.uspstgTRStatusMatchingAR";

				command.Parameters.Add(command.CreateParameter("@vType", "GetCount"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

				return this.CountTransaction(command);
			}
		}
		private List<stgTRStatusMatchingAR> pGetList(string whereClause, string orderBy)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspstgTRStatusMatchingAR";

				command.Parameters.Add(command.CreateParameter("@vType", "GetList"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
				command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));

				return this.ReadTransaction(command).ToList();
			}
		}
		private List<stgTRStatusMatchingAR> pGetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspstgTRStatusMatchingAR";

				command.Parameters.Add(command.CreateParameter("@vType", "GetPaged"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
				command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));
				command.Parameters.Add(command.CreateParameter("@vRowSkip", rowSkip));
				command.Parameters.Add(command.CreateParameter("@vPageSize", pageSize));

				return this.ReadTransaction(command).ToList();
			}
		}
		private stgTRStatusMatchingAR pGetByPK(long iD)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspstgTRStatusMatchingAR";

				command.Parameters.Add(command.CreateParameter("@vType", "GetByPK"));
				command.Parameters.Add(command.CreateParameter("@ID", iD));

				return this.ReadTransaction(command).SingleOrDefault();
			}
		}
		private stgTRStatusMatchingAR pCreate(stgTRStatusMatchingAR data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<stgTRStatusMatchingAR>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspstgTRStatusMatchingAR";

				command.Parameters.Add(command.CreateParameter("@vType", "Create"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));

				data.ID = this.WriteTransaction(command);

				return data;
			}
		}
		private List<stgTRStatusMatchingAR> pCreateBulky(List<stgTRStatusMatchingAR> data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<List<stgTRStatusMatchingAR>>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspstgTRStatusMatchingAR";

				command.Parameters.Add(command.CreateParameter("@vType", "CreateBulky"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));

				command.ExecuteNonQuery();

				return data;
			}
		}
		private stgTRStatusMatchingAR pUpdate(stgTRStatusMatchingAR data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<stgTRStatusMatchingAR>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspstgTRStatusMatchingAR";

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
				command.CommandText = "dbo.uspstgTRStatusMatchingAR";

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
				command.CommandText = "dbo.uspstgTRStatusMatchingAR";

				command.Parameters.Add(command.CreateParameter("@vType", "DeleteByFilter"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

				command.ExecuteNonQuery();
			}
		}
		#endregion

	}
}