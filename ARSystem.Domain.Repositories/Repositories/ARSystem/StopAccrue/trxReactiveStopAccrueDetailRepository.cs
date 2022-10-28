
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
	public class trxReactiveStopAccrueDetailRepository : BaseRepository<trxReactiveStopAccrueDetail>
	{
		private DbContext _context;
		public trxReactiveStopAccrueDetailRepository(DbContext context) : base(context)
		{
			_context = context;
		}

		public int GetCount(string whereClause = "")
		{
			return pGetCount(whereClause);
		}

		public List<trxReactiveStopAccrueDetail> GetList(string whereClause = "", string orderBy = "")
		{
			return pGetList(whereClause, orderBy);
		}

		public List<trxReactiveStopAccrueDetail> GetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
		{
			return pGetPaged(whereClause, orderBy, rowSkip, pageSize);
		}
        public trxReactiveStopAccrueDetail GetByPK(long iD)
        {
            return pGetByPK(iD);
        }

        public trxReactiveStopAccrueDetail Create(trxReactiveStopAccrueDetail data)
        {
            return pCreate(data);
        }

        public List<trxReactiveStopAccrueDetail> CreateBulky(List<trxReactiveStopAccrueDetail> data)
        {
            return pCreateBulky(data);
        }

        public List<trxReactiveStopAccrueDetail> CreateBulkyDraft(List<trxReactiveStopAccrueDetail> data)
        {
            return pCreateBulkyDraft(data);
        }

        public List<trxReactiveStopAccrueDetail> CreateBulkyEdit(List<trxReactiveStopAccrueDetail> data)
        {
            return pCreateBulkyEdit(data);
        }

        public trxReactiveStopAccrueDetail Update(trxReactiveStopAccrueDetail data)
        {
            return pUpdate(data);
        }

        public trxReactiveStopAccrueDetail UpdateCapexRevenue(trxReactiveStopAccrueDetail data)
        {
            return pUpdateCapexRevenue(data);
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

        public List<trxReactiveStopAccrueDetail> DeleteDraft(string whereClause = "")
        {
            return pDeleteDraft(whereClause);
        }

        #region Private

        private int pGetCount(string whereClause)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspvwTrxReactiveStopAccrueDetail";

				command.Parameters.Add(command.CreateParameter("@vType", "GetCount"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

				return this.CountTransaction(command);
			}
		}

		private List<trxReactiveStopAccrueDetail> pGetList(string whereClause, string orderBy)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspvwTrxReactiveStopAccrueDetail";

				command.Parameters.Add(command.CreateParameter("@vType", "GetList"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
				command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));

				return this.ReadTransaction(command).ToList();
			}
		}

		private List<trxReactiveStopAccrueDetail> pGetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspvwTrxReactiveStopAccrueDetail";

				command.Parameters.Add(command.CreateParameter("@vType", "GetPaged"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
				command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));
				command.Parameters.Add(command.CreateParameter("@vRowSkip", rowSkip));
				command.Parameters.Add(command.CreateParameter("@vPageSize", pageSize));

				return this.ReadTransaction(command).ToList();
			}
		}

        private trxReactiveStopAccrueDetail pGetByPK(long iD)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspvwTrxReactiveStopAccrueDetail";

                command.Parameters.Add(command.CreateParameter("@vType", "GetByPK"));
                command.Parameters.Add(command.CreateParameter("@ID", iD));

                return this.ReadTransaction(command).SingleOrDefault();
            }
        }

        private trxReactiveStopAccrueDetail pCreate(trxReactiveStopAccrueDetail data)
        {
            using (var command = _context.CreateCommand())
            {
                string xml = Helper.XmlSerializer<trxReactiveStopAccrueDetail>(data);

                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspvwTrxReactiveStopAccrueDetail";

                command.Parameters.Add(command.CreateParameter("@vType", "Create"));
                command.Parameters.Add(command.CreateParameter("@vXml", xml));

                this.WriteTransaction(command);

                return data;
            }
        }

        private List<trxReactiveStopAccrueDetail> pCreateBulky(List<trxReactiveStopAccrueDetail> data)
        {
            using (var command = _context.CreateCommand())
            {
                string xml = Helper.XmlSerializer<List<trxReactiveStopAccrueDetail>>(data);

                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspvwTrxReactiveStopAccrueDetail";

                command.Parameters.Add(command.CreateParameter("@vType", "CreateBulky"));
                command.Parameters.Add(command.CreateParameter("@vXml", xml));

                command.ExecuteNonQuery();

                return data;
            }
        }

        private List<trxReactiveStopAccrueDetail> pCreateBulkyEdit(List<trxReactiveStopAccrueDetail> data)
        {
            using (var command = _context.CreateCommand())
            {
                string xml = Helper.XmlSerializer<List<trxReactiveStopAccrueDetail>>(data);

                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspvwTrxReactiveStopAccrueDetail";

                command.Parameters.Add(command.CreateParameter("@vType", "CreateBulkyEdit"));
                command.Parameters.Add(command.CreateParameter("@vXml", xml));

                command.ExecuteNonQuery();

                return data;
            }
        }

        private List<trxReactiveStopAccrueDetail> pCreateBulkyDraft(List<trxReactiveStopAccrueDetail> data)
        {
            using (var command = _context.CreateCommand())
            {
                string xml = Helper.XmlSerializer<List<trxReactiveStopAccrueDetail>>(data);

                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspvwTrxReactiveStopAccrueDetail";

                command.Parameters.Add(command.CreateParameter("@vType", "CreateBulkyDraft"));
                command.Parameters.Add(command.CreateParameter("@vXml", xml));

                command.ExecuteNonQuery();

                return data;
            }
        }

        private trxReactiveStopAccrueDetail pUpdateCapexRevenue(trxReactiveStopAccrueDetail data)
        {
            using (var command = _context.CreateCommand())
            {
                string xml = Helper.XmlSerializer<trxReactiveStopAccrueDetail>(data);

                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspvwTrxReactiveStopAccrueDetail";

                command.Parameters.Add(command.CreateParameter("@vType", "UpdateCapexRevenue"));
                command.Parameters.Add(command.CreateParameter("@vXml", xml));
                command.Parameters.Add(command.CreateParameter("@ID", data.ID));

                command.ExecuteNonQuery();

                return data;
            }
        }

        private trxReactiveStopAccrueDetail pUpdate(trxReactiveStopAccrueDetail data)
        {
            using (var command = _context.CreateCommand())
            {
                string xml = Helper.XmlSerializer<trxReactiveStopAccrueDetail>(data);

                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspvwTrxReactiveStopAccrueDetail";

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
                command.CommandText = "dbo.uspvwTrxReactiveStopAccrueDetail";

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
                command.CommandText = "dbo.uspvwTrxReactiveStopAccrueDetail";

                command.Parameters.Add(command.CreateParameter("@vType", "DeleteByFilter"));
                command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

                command.ExecuteNonQuery();
            }
        }

        private List<trxReactiveStopAccrueDetail> pDeleteDraft(string whereClause)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.usptrxStopAccrueDeleteDraft";

                command.Parameters.Add(command.CreateParameter("@Initiator", whereClause));

                return this.ReadTransaction(command).ToList();
            }
        }

        #endregion

    }
}