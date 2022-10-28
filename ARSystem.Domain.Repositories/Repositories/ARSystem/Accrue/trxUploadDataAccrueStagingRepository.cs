
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
	public class trxUploadDataAccrueStagingRepository : BaseRepository<trxUploadDataAccrueStaging>
	{
		private DbContext _context;
		public trxUploadDataAccrueStagingRepository(DbContext context) : base(context)
		{
			_context = context;
		}

		public int GetCount(string whereClause = "")
		{
			return pGetCount(whereClause);
		}

		public List<trxUploadDataAccrueStaging> GetList(string whereClause = "", string orderBy = "")
		{
			return pGetList(whereClause, orderBy);
		}

		public List<trxUploadDataAccrueStaging> GetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
		{
			return pGetPaged(whereClause, orderBy, rowSkip, pageSize);
		}

		public trxUploadDataAccrueStaging GetByPK(long iD)
		{
			return pGetByPK(iD);
		}

		public trxUploadDataAccrueStaging Create(trxUploadDataAccrueStaging data)
		{
			return pCreate(data);
		}

        public bool Create(string UserID)
        {
            pCreate(UserID);
            return true;
        }

        public List<trxUploadDataAccrueStaging> CreateBulky(List<trxUploadDataAccrueStaging> data)
		{
			return pCreateBulky(data);
		}

		public trxUploadDataAccrueStaging Update(trxUploadDataAccrueStaging data)
		{
			return pUpdate(data);
		}

        public bool UpdateByFilter(string whereClause)
        {
            pUpdateByFilter(whereClause);
            return true;
        }

        public bool UpdateDeleteByFilter(string whereClause)
        {
            pUpdateDeleteByFilter(whereClause);
            return true;
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
				command.CommandText = "dbo.usptrxUploadDataAccrueStaging";

				command.Parameters.Add(command.CreateParameter("@vType", "GetCount"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

				return this.CountTransaction(command);
			}
		}
		private List<trxUploadDataAccrueStaging> pGetList(string whereClause, string orderBy)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxUploadDataAccrueStaging";

				command.Parameters.Add(command.CreateParameter("@vType", "GetList"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
				command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));

				return this.ReadTransaction(command).ToList();
			}
		}
		private List<trxUploadDataAccrueStaging> pGetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxUploadDataAccrueStaging";

				command.Parameters.Add(command.CreateParameter("@vType", "GetPaged"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
				command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));
				command.Parameters.Add(command.CreateParameter("@vRowSkip", rowSkip));
				command.Parameters.Add(command.CreateParameter("@vPageSize", pageSize));

				return this.ReadTransaction(command).ToList();
			}
		}
		private trxUploadDataAccrueStaging pGetByPK(long iD)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxUploadDataAccrueStaging";

				command.Parameters.Add(command.CreateParameter("@vType", "GetByPK"));
				command.Parameters.Add(command.CreateParameter("@ID", iD));

				return this.ReadTransaction(command).SingleOrDefault();
			}
		}
		private trxUploadDataAccrueStaging pCreate(trxUploadDataAccrueStaging data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<trxUploadDataAccrueStaging>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxUploadDataAccrueStaging";

				command.Parameters.Add(command.CreateParameter("@vType", "Create"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));

				data.ID = this.WriteTransaction(command);

				return data;
			}
		}
        private void pCreate(string UserID)
        {
            using (var command = _context.CreateCommand())
            {

                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspSubmittrxUploadDataAccrueStaging";

                command.Parameters.Add(command.CreateParameter("@userID", UserID));

                command.ExecuteNonQuery();
            }
        }
        private List<trxUploadDataAccrueStaging> pCreateBulky(List<trxUploadDataAccrueStaging> data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<List<trxUploadDataAccrueStaging>>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxUploadDataAccrueStaging";

				command.Parameters.Add(command.CreateParameter("@vType", "CreateBulky"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));

				command.ExecuteNonQuery();

				return data;
			}
		}
		private trxUploadDataAccrueStaging pUpdate(trxUploadDataAccrueStaging data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<trxUploadDataAccrueStaging>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxUploadDataAccrueStaging";

				command.Parameters.Add(command.CreateParameter("@vType", "Update"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));
				command.Parameters.Add(command.CreateParameter("@ID", data.ID));

				command.ExecuteNonQuery();

				return data;
			}
		}

        private void pUpdateByFilter(string whereClause)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.usptrxUploadDataAccrueStaging";

                command.Parameters.Add(command.CreateParameter("@vType", "UpdateByFilter"));
                command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

                command.ExecuteNonQuery();
            }
        }

        private void pUpdateDeleteByFilter(string whereClause)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.usptrxUploadDataAccrueStaging";

                command.Parameters.Add(command.CreateParameter("@vType", "UpdateDeleteByParams"));
                command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

                command.ExecuteNonQuery();
            }
        }
        private void pDeleteByPK(long iD)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxUploadDataAccrueStaging";

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
				command.CommandText = "dbo.usptrxUploadDataAccrueStaging";

				command.Parameters.Add(command.CreateParameter("@vType", "DeleteByFilter"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

				command.ExecuteNonQuery();
			}
		}
		#endregion

	}
}