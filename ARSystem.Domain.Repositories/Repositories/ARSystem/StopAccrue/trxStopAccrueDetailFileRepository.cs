
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
	public class trxStopAccrueDetailFileRepository : BaseRepository<trxStopAccrueDetailFile>
	{
		private DbContext _context;
		public trxStopAccrueDetailFileRepository(DbContext context) : base(context)
		{
			_context = context;
		}

		public int GetCount(string whereClause = "")
		{
			return pGetCount(whereClause);
		}

		public List<trxStopAccrueDetailFile> GetList(string whereClause = "", string orderBy = "")
		{
			return pGetList(whereClause, orderBy);
		}

		public List<trxStopAccrueDetailFile> GetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
		{
			return pGetPaged(whereClause, orderBy, rowSkip, pageSize);
		}

		public trxStopAccrueDetailFile GetByPK(long iD)
		{
			return pGetByPK(iD);
		}

		public trxStopAccrueDetailFile Create(trxStopAccrueDetailFile data)
		{
			return pCreate(data);
		}

		public List<trxStopAccrueDetailFile> CreateBulky(List<trxStopAccrueDetailFile> data)
		{
			return pCreateBulky(data);
		}
        public List<trxStopAccrueDetailFile> CreateBulkyDraft(List<trxStopAccrueDetailFile> data)
        {
            return pCreateBulkyDraft(data);
        }
        public List<trxStopAccrueDetailFile> CreateBulkyEdit(List<trxStopAccrueDetailFile> data)
        {
            return pCreateBulkyEdit(data);
        }
        public trxStopAccrueDetailFile Update(trxStopAccrueDetailFile data)
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
				command.CommandText = "dbo.usptrxStopAccrueDetailFile";

				command.Parameters.Add(command.CreateParameter("@vType", "GetCount"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

				return this.CountTransaction(command);
			}
		}
		private List<trxStopAccrueDetailFile> pGetList(string whereClause, string orderBy)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxStopAccrueDetailFile";

				command.Parameters.Add(command.CreateParameter("@vType", "GetList"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
				command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));

				return this.ReadTransaction(command).ToList();
			}
		}
		private List<trxStopAccrueDetailFile> pGetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxStopAccrueDetailFile";

				command.Parameters.Add(command.CreateParameter("@vType", "GetPaged"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
				command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));
				command.Parameters.Add(command.CreateParameter("@vRowSkip", rowSkip));
				command.Parameters.Add(command.CreateParameter("@vPageSize", pageSize));

				return this.ReadTransaction(command).ToList();
			}
		}
		private trxStopAccrueDetailFile pGetByPK(long iD)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxStopAccrueDetailFile";

				command.Parameters.Add(command.CreateParameter("@vType", "GetByPK"));
				command.Parameters.Add(command.CreateParameter("@ID", iD));

				return this.ReadTransaction(command).SingleOrDefault();
			}
		}
		private trxStopAccrueDetailFile pCreate(trxStopAccrueDetailFile data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<trxStopAccrueDetailFile>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxStopAccrueDetailFile";

				command.Parameters.Add(command.CreateParameter("@vType", "Create"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));

				this.WriteTransaction(command);

				return data;
			}
		}
		private List<trxStopAccrueDetailFile> pCreateBulky(List<trxStopAccrueDetailFile> data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<List<trxStopAccrueDetailFile>>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxStopAccrueDetailFile";

				command.Parameters.Add(command.CreateParameter("@vType", "CreateBulky"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));

				command.ExecuteNonQuery();

				return data;
			}
		}
        private List<trxStopAccrueDetailFile> pCreateBulkyDraft(List<trxStopAccrueDetailFile> data)
        {
            using (var command = _context.CreateCommand())
            {
                string xml = Helper.XmlSerializer<List<trxStopAccrueDetailFile>>(data);

                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.usptrxStopAccrueDetailFile";

                command.Parameters.Add(command.CreateParameter("@vType", "CreateBulkyDraft"));
                command.Parameters.Add(command.CreateParameter("@vXml", xml));

                command.ExecuteNonQuery();

                return data;
            }
        }
        private List<trxStopAccrueDetailFile> pCreateBulkyEdit(List<trxStopAccrueDetailFile> data)
        {
            using (var command = _context.CreateCommand())
            {
                string xml = Helper.XmlSerializer<List<trxStopAccrueDetailFile>>(data);

                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.usptrxStopAccrueDetailFile";

                command.Parameters.Add(command.CreateParameter("@vType", "CreateBulkyEdit"));
                command.Parameters.Add(command.CreateParameter("@vXml", xml));

                command.ExecuteNonQuery();

                return data;
            }
        }
        private trxStopAccrueDetailFile pUpdate(trxStopAccrueDetailFile data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<trxStopAccrueDetailFile>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxStopAccrueDetailFile";

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
				command.CommandText = "dbo.usptrxStopAccrueDetailFile";

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
				command.CommandText = "dbo.usptrxStopAccrueDetailFile";

				command.Parameters.Add(command.CreateParameter("@vType", "DeleteByFilter"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

				command.ExecuteNonQuery();
			}
		}
		#endregion

	}
}