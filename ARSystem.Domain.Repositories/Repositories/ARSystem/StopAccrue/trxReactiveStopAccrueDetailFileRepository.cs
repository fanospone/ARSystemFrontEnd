
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ARSystem.Domain.DAL;
using ARSystem.Domain.Models.TBIGSYSDB01.TBGARSystem;
using ARSystem.Domain.Models;
namespace ARSystem.Domain.Repositories
{
	public class trxReactiveStopAccrueDetailFileRepository : BaseRepository<trxReactiveStopAccrueDetailFile>
	{
		private DbContext _context;
		public trxReactiveStopAccrueDetailFileRepository(DbContext context) : base(context)
		{
			_context = context;
		}

		public int GetCount(string whereClause = "")
		{
			return pGetCount(whereClause);
		}

		public List<trxReactiveStopAccrueDetailFile> GetList(string whereClause = "", string orderBy = "")
		{
			return pGetList(whereClause, orderBy);
		}

		public List<trxReactiveStopAccrueDetailFile> GetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
		{
			return pGetPaged(whereClause, orderBy, rowSkip, pageSize);
		}

        public trxReactiveStopAccrueDetailFile GetByPK(long iD)
        {
            return pGetByPK(iD);
        }

        public trxReactiveStopAccrueDetailFile Create(trxReactiveStopAccrueDetailFile data)
        {
            return pCreate(data);
        }

        public List<trxReactiveStopAccrueDetailFile> CreateBulky(List<trxReactiveStopAccrueDetailFile> data)
        {
            return pCreateBulky(data);
        }

        public List<trxReactiveStopAccrueDetailFile> CreateBulkyDraft(List<trxReactiveStopAccrueDetailFile> data)
        {
            return pCreateBulkyDraft(data);
        }

        public List<trxReactiveStopAccrueDetailFile> CreateBulkyEdit(List<trxReactiveStopAccrueDetailFile> data)
        {
            return pCreateBulkyEdit(data);
        }

        public trxReactiveStopAccrueDetailFile Update(trxReactiveStopAccrueDetailFile data)
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
				command.CommandText = "dbo.uspvwTrxReactiveStopAccrueDetailFile";

				command.Parameters.Add(command.CreateParameter("@vType", "GetCount"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

				return this.CountTransaction(command);
			}
		}
		private List<trxReactiveStopAccrueDetailFile> pGetList(string whereClause, string orderBy)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspvwTrxReactiveStopAccrueDetailFile";

				command.Parameters.Add(command.CreateParameter("@vType", "GetList"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
				command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));

				return this.ReadTransaction(command).ToList();
			}
		}
		private List<trxReactiveStopAccrueDetailFile> pGetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspvwTrxReactiveStopAccrueDetailFile";

				command.Parameters.Add(command.CreateParameter("@vType", "GetPaged"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
				command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));
				command.Parameters.Add(command.CreateParameter("@vRowSkip", rowSkip));
				command.Parameters.Add(command.CreateParameter("@vPageSize", pageSize));

				return this.ReadTransaction(command).ToList();
			}
		}

        private trxReactiveStopAccrueDetailFile pGetByPK(long iD)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspvwTrxReactiveStopAccrueDetailFile";

                command.Parameters.Add(command.CreateParameter("@vType", "GetByPK"));
                command.Parameters.Add(command.CreateParameter("@ID", iD));

                return this.ReadTransaction(command).SingleOrDefault();
            }
        }

        private trxReactiveStopAccrueDetailFile pCreate(trxReactiveStopAccrueDetailFile data)
        {
            using (var command = _context.CreateCommand())
            {
                string xml = Helper.XmlSerializer<trxReactiveStopAccrueDetailFile>(data);

                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspvwTrxReactiveStopAccrueDetailFile";

                command.Parameters.Add(command.CreateParameter("@vType", "Create"));
                command.Parameters.Add(command.CreateParameter("@vXml", xml));

                this.WriteTransaction(command);

                return data;
            }
        }

        private List<trxReactiveStopAccrueDetailFile> pCreateBulky(List<trxReactiveStopAccrueDetailFile> data)
        {
            using (var command = _context.CreateCommand())
            {
                string xml = Helper.XmlSerializer<List<trxReactiveStopAccrueDetailFile>>(data);

                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspvwTrxReactiveStopAccrueDetailFile";

                command.Parameters.Add(command.CreateParameter("@vType", "CreateBulky"));
                command.Parameters.Add(command.CreateParameter("@vXml", xml));

                command.ExecuteNonQuery();

                return data;
            }
        }

        private List<trxReactiveStopAccrueDetailFile> pCreateBulkyDraft(List<trxReactiveStopAccrueDetailFile> data)
        {
            using (var command = _context.CreateCommand())
            {
                string xml = Helper.XmlSerializer<List<trxReactiveStopAccrueDetailFile>>(data);

                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspvwTrxReactiveStopAccrueDetailFile";

                command.Parameters.Add(command.CreateParameter("@vType", "CreateBulkyDraft"));
                command.Parameters.Add(command.CreateParameter("@vXml", xml));

                command.ExecuteNonQuery();

                return data;
            }
        }

        private List<trxReactiveStopAccrueDetailFile> pCreateBulkyEdit(List<trxReactiveStopAccrueDetailFile> data)
        {
            using (var command = _context.CreateCommand())
            {
                string xml = Helper.XmlSerializer<List<trxReactiveStopAccrueDetailFile>>(data);

                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspvwTrxReactiveStopAccrueDetailFile";

                command.Parameters.Add(command.CreateParameter("@vType", "CreateBulkyEdit"));
                command.Parameters.Add(command.CreateParameter("@vXml", xml));

                command.ExecuteNonQuery();

                return data;
            }
        }

        private trxReactiveStopAccrueDetailFile pUpdate(trxReactiveStopAccrueDetailFile data)
        {
            using (var command = _context.CreateCommand())
            {
                string xml = Helper.XmlSerializer<trxReactiveStopAccrueDetailFile>(data);

                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspvwTrxReactiveStopAccrueDetailFile";

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
                command.CommandText = "dbo.uspvwTrxReactiveStopAccrueDetailFile";

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
                command.CommandText = "dbo.uspvwTrxReactiveStopAccrueDetailFile";

                command.Parameters.Add(command.CreateParameter("@vType", "DeleteByFilter"));
                command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

                command.ExecuteNonQuery();
            }
        }

        #endregion

    }
}