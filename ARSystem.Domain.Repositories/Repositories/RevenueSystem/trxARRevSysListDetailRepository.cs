
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
	public class trxARRevSysListDetailRepository : BaseRepository<trxARRevSysListDetail>
	{
		private DbContext _context;
		public trxARRevSysListDetailRepository(DbContext context) : base(context)
		{
			_context = context;
		}

		public int GetCount(string whereClause = "")
		{
			return pGetCount(whereClause);
		}

		public List<trxARRevSysListDetail> GetList(string whereClause = "", string orderBy = "")
		{
			return pGetList(whereClause, orderBy);
		}

        public List<trxARRevSysListDetail> GetList(string SoNumber , int? stipSiro, string orderBy = "")
        {
            return pGetList(SoNumber, stipSiro, orderBy);
        }

        public List<trxARRevSysListDetail> GetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
		{
			return pGetPaged(whereClause, orderBy, rowSkip, pageSize);
		}

		public trxARRevSysListDetail Create(trxARRevSysListDetail data)
		{
			return pCreate(data);
		}

		public List<trxARRevSysListDetail> CreateBulky(List<trxARRevSysListDetail> data)
		{
			return pCreateBulky(data);
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
				command.CommandText = "dbo.usptrxARRevSysListDetail";

				command.Parameters.Add(command.CreateParameter("@vType", "GetCount"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

				return this.CountTransaction(command);
			}
		}
		private List<trxARRevSysListDetail> pGetList(string whereClause, string orderBy)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.spARRevSysDetailPerSonumb";

                command.Parameters.Add(command.CreateParameter("@Sonumb", whereClause));
                //command.CommandText = "dbo.usptrxARRevSysListDetail";

                //command.Parameters.Add(command.CreateParameter("@vType", "GetList"));
                //command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
                //command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));

                return this.ReadTransaction(command).ToList();
			}
		}
        private List<trxARRevSysListDetail> pGetList(string whereClause, int? stipSiro, string orderBy)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.spARRevSysDetailPerSonumb";

                command.Parameters.Add(command.CreateParameter("@Sonumb", whereClause));
                command.Parameters.Add(command.CreateParameter("@StipSiro", stipSiro));
                return this.ReadTransaction(command).ToList();
            }
        }
        private List<trxARRevSysListDetail> pGetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxARRevSysListDetail";

				command.Parameters.Add(command.CreateParameter("@vType", "GetPaged"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
				command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));
				command.Parameters.Add(command.CreateParameter("@vRowSkip", rowSkip));
				command.Parameters.Add(command.CreateParameter("@vPageSize", pageSize));

				return this.ReadTransaction(command).ToList();
			}
		}
		private trxARRevSysListDetail pCreate(trxARRevSysListDetail data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<trxARRevSysListDetail>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxARRevSysListDetail";

				command.Parameters.Add(command.CreateParameter("@vType", "Create"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));

				this.WriteTransaction(command);

				return data;
			}
		}
		private List<trxARRevSysListDetail> pCreateBulky(List<trxARRevSysListDetail> data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<List<trxARRevSysListDetail>>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxARRevSysListDetail";

				command.Parameters.Add(command.CreateParameter("@vType", "CreateBulky"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));

				command.ExecuteNonQuery();

				return data;
			}
		}
		private void pDeleteByFilter(string whereClause)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxARRevSysListDetail";

				command.Parameters.Add(command.CreateParameter("@vType", "DeleteByFilter"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

				command.ExecuteNonQuery();
			}
		}
		#endregion

	}
}