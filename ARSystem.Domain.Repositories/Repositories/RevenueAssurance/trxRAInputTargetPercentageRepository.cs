
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
	public class trxRAInputTargetPercentageRepository : BaseRepository<trxRAInputTargetPercentage>
	{
		private DbContext _context;
		public trxRAInputTargetPercentageRepository(DbContext context) : base(context)
		{
			_context = context;
		}

		public int GetCount(string whereClause = "")
		{
			return pGetCount(whereClause);
		}

		public List<trxRAInputTargetPercentage> GetList(string whereClause = "", string orderBy = "")
		{
			return pGetList(whereClause, orderBy);
		}

		public List<trxRAInputTargetPercentage> GetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
		{
			return pGetPaged(whereClause, orderBy, rowSkip, pageSize);
		}

		public trxRAInputTargetPercentage Create(trxRAInputTargetPercentage data)
		{
			return pCreate(data);
		}

        public trxRAInputTargetPercentage Update(trxRAInputTargetPercentage data)
        {
            return pUpdate(data);
        }
        public trxRAInputTargetPercentage FindByFilter(string whereClause)
        {
            return pFindByFilter(whereClause);
        }

        
        public List<trxRAInputTargetPercentage> CreateBulky(List<trxRAInputTargetPercentage> data)
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
				command.CommandText = "dbo.usptrxRAInputTargetPercentage";

				command.Parameters.Add(command.CreateParameter("@vType", "GetCount"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

				return this.CountTransaction(command);
			}
		}
		private List<trxRAInputTargetPercentage> pGetList(string whereClause, string orderBy)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxRAInputTargetPercentage";

				command.Parameters.Add(command.CreateParameter("@vType", "GetList"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
				command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));

				return this.ReadTransaction(command).ToList();
			}
		}
		private List<trxRAInputTargetPercentage> pGetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxRAInputTargetPercentage";

				command.Parameters.Add(command.CreateParameter("@vType", "GetPaged"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
				command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));
				command.Parameters.Add(command.CreateParameter("@vRowSkip", rowSkip));
				command.Parameters.Add(command.CreateParameter("@vPageSize", pageSize));

				return this.ReadTransaction(command).ToList();
			}
		}
		private trxRAInputTargetPercentage pCreate(trxRAInputTargetPercentage data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<trxRAInputTargetPercentage>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxRAInputTargetPercentage";

				command.Parameters.Add(command.CreateParameter("@vType", "Create"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));

				data.ID = this.WriteTransaction(command);

				return data;
			}
		}
        private trxRAInputTargetPercentage pFindByFilter(string whereClause)
        {
            using (var command = _context.CreateCommand())
            {

                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.usptrxRAInputTargetPercentage";

                command.Parameters.Add(command.CreateParameter("@vType", "FindByFilter"));
                command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

                return this.ReadTransaction(command).FirstOrDefault();

            }
        }

        

        private trxRAInputTargetPercentage pUpdate(trxRAInputTargetPercentage data)
        {
            using (var command = _context.CreateCommand())
            {
                string xml = Helper.XmlSerializer<trxRAInputTargetPercentage>(data);

                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.usptrxRAInputTargetPercentage";

                command.Parameters.Add(command.CreateParameter("@vType", "Update"));
                command.Parameters.Add(command.CreateParameter("@vXml", xml));

                data.ID = this.WriteTransaction(command);

                return data;
            }
        }
        private List<trxRAInputTargetPercentage> pCreateBulky(List<trxRAInputTargetPercentage> data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<List<trxRAInputTargetPercentage>>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.usptrxRAInputTargetPercentage";

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
				command.CommandText = "dbo.usptrxRAInputTargetPercentage";

				command.Parameters.Add(command.CreateParameter("@vType", "DeleteByFilter"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

				command.ExecuteNonQuery();
			}
		}
		#endregion

	}
}