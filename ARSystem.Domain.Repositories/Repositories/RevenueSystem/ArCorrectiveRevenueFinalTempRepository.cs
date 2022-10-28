
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
	public class ArCorrectiveRevenueFinalTempRepository : BaseRepository<ArCorrectiveRevenueFinalTemp>
	{
		private DbContext _context;
		public ArCorrectiveRevenueFinalTempRepository(DbContext context) : base(context)
		{
			_context = context;
		}

		public int GetCount(string whereClause = "")
		{
			return pGetCount(whereClause);
		}

		public List<ArCorrectiveRevenueFinalTemp> GetList(string whereClause = "", string orderBy = "")
		{
			return pGetList(whereClause, orderBy);
		}

		public List<ArCorrectiveRevenueFinalTemp> GetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
		{
			return pGetPaged(whereClause, orderBy, rowSkip, pageSize);
		}

		public ArCorrectiveRevenueFinalTemp GetByPK(int id)
		{
			return pGetByPK(id);
		}

		public ArCorrectiveRevenueFinalTemp Create(ArCorrectiveRevenueFinalTemp data)
		{
			return pCreate(data);
		}

		public List<ArCorrectiveRevenueFinalTemp> CreateBulky(List<ArCorrectiveRevenueFinalTemp> data)
		{
			return pCreateBulky(data);
		}

        public List<ArCorrectiveRevenueFinalTemp> DeleteBulky(List<ArCorrectiveRevenueFinalTemp> data, bool isDeleteAll, string userId)
        {
            return pDeleteBulky(data, isDeleteAll, userId);
        }

        public ArCorrectiveRevenueFinalTemp Update(ArCorrectiveRevenueFinalTemp data)
		{
			return pUpdate(data);
		}

        public string ProcessData(string userId)
        {
            return pProcessData(userId);
        }


        public bool DeleteByPK(int id)
		{
			pDeleteByPK(id);
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
				command.CommandText = "dbo.uspArCorrectiveRevenueFinalTemp";

				command.Parameters.Add(command.CreateParameter("@vType", "GetCount"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

				return this.CountTransaction(command);
			}
		}
		private List<ArCorrectiveRevenueFinalTemp> pGetList(string whereClause, string orderBy)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspArCorrectiveRevenueFinalTemp";

				command.Parameters.Add(command.CreateParameter("@vType", "GetList"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
				command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));

				return this.ReadTransaction(command).ToList();
			}
		}
		private List<ArCorrectiveRevenueFinalTemp> pGetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspArCorrectiveRevenueFinalTemp";

				command.Parameters.Add(command.CreateParameter("@vType", "GetPaged"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
				command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));
				command.Parameters.Add(command.CreateParameter("@vRowSkip", rowSkip));
				command.Parameters.Add(command.CreateParameter("@vPageSize", pageSize));

				return this.ReadTransaction(command).ToList();
			}
		}
		private ArCorrectiveRevenueFinalTemp pGetByPK(int id)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspArCorrectiveRevenueFinalTemp";

				command.Parameters.Add(command.CreateParameter("@vType", "GetByPK"));
				command.Parameters.Add(command.CreateParameter("@Id", id));

				return this.ReadTransaction(command).SingleOrDefault();
			}
		}
		private ArCorrectiveRevenueFinalTemp pCreate(ArCorrectiveRevenueFinalTemp data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<ArCorrectiveRevenueFinalTemp>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspArCorrectiveRevenueFinalTemp";

				command.Parameters.Add(command.CreateParameter("@vType", "Create"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));

				data.Id = this.WriteTransaction(command);

				return data;
			}
		}
		private List<ArCorrectiveRevenueFinalTemp> pCreateBulky(List<ArCorrectiveRevenueFinalTemp> data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<List<ArCorrectiveRevenueFinalTemp>>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspArCorrectiveRevenueFinalTemp";

				command.Parameters.Add(command.CreateParameter("@vType", "CreateBulky"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));

				command.ExecuteNonQuery();

				return data;
			}
		}

        private List<ArCorrectiveRevenueFinalTemp> pDeleteBulky(List<ArCorrectiveRevenueFinalTemp> data, bool isDeleteAll, string userId)
        {
            using (var command = _context.CreateCommand())
            {
                string xml = Helper.XmlSerializer<List<ArCorrectiveRevenueFinalTemp>>(data);

                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspArCorrectiveRevenueFinalTemp";

                command.Parameters.Add(command.CreateParameter("@vType", "DeleteBulky"));
                command.Parameters.Add(command.CreateParameter("@vWhereClause", (isDeleteAll==true?"DeleteAll":"")));
                command.Parameters.Add(command.CreateParameter("@vUserId", userId));
                command.Parameters.Add(command.CreateParameter("@vXml", xml));

                command.ExecuteNonQuery();

                return data;
            }
        }
        private ArCorrectiveRevenueFinalTemp pUpdate(ArCorrectiveRevenueFinalTemp data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<ArCorrectiveRevenueFinalTemp>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspArCorrectiveRevenueFinalTemp";

				command.Parameters.Add(command.CreateParameter("@vType", "Update"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));
				command.Parameters.Add(command.CreateParameter("@Id", data.Id));

				command.ExecuteNonQuery();

				return data;
			}
		}
		private void pDeleteByPK(int id)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspArCorrectiveRevenueFinalTemp";

				command.Parameters.Add(command.CreateParameter("@vType", "DeleteByPK"));
				command.Parameters.Add(command.CreateParameter("@Id", id));

				command.ExecuteNonQuery();
			}
		}
		private void pDeleteByFilter(string whereClause)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspArCorrectiveRevenueFinalTemp";

				command.Parameters.Add(command.CreateParameter("@vType", "DeleteByFilter"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

				command.ExecuteNonQuery();
			}
		}

        private string pProcessData(string userId)
        {
            using (var command = _context.CreateCommand())
            {
              
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspArCorrectiveRevenueFinalTemp";

                command.Parameters.Add(command.CreateParameter("@vType", "ProcessData"));
                command.Parameters.Add(command.CreateParameter("@vUserId", userId));

                command.ExecuteNonQuery();
                return "";
            }
        }
        #endregion

    }
}