
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
	public class MstInitialDepartmentRepository : BaseRepository<MstInitialDepartment>
	{
		private DbContext _context;
		public MstInitialDepartmentRepository(DbContext context) : base(context)
		{
			_context = context;
		}

		public int GetCount(string whereClause = "")
		{
			return pGetCount(whereClause);
		}

		public List<MstInitialDepartment> GetList(string whereClause = "", string orderBy = "")
		{
			return pGetList(whereClause, orderBy);
		}

		public List<MstInitialDepartment> GetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
		{
			return pGetPaged(whereClause, orderBy, rowSkip, pageSize);
		}

		public MstInitialDepartment GetByPK(int iD)
		{
			return pGetByPK(iD);
		}

		public MstInitialDepartment Create(MstInitialDepartment data)
		{
			return pCreate(data);
		}

		public List<MstInitialDepartment> CreateBulky(List<MstInitialDepartment> data)
		{
			return pCreateBulky(data);
		}

		public MstInitialDepartment Update(MstInitialDepartment data)
		{
			return pUpdate(data);
		}

		public bool DeleteByPK(int iD)
		{
			pDeleteByPK(iD);
			return true;
		}

		public bool DeleteByFilter(string whereClause)
		{
			pDeleteByFilter(whereClause);
			return true;
		}

        public List<MstInitialDepartment> GetDirectorate()
        {
            return pGetDirectorate();
            
        }

        #region Private

		private int pGetCount(string whereClause)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspMstInitialDepartment";

				command.Parameters.Add(command.CreateParameter("@vType", "GetCount"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

				return this.CountTransaction(command);
			}
		}
		private List<MstInitialDepartment> pGetList(string whereClause, string orderBy)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspMstInitialDepartment";

				command.Parameters.Add(command.CreateParameter("@vType", "GetList"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
				command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));

				return this.ReadTransaction(command).ToList();
			}
		}
		private List<MstInitialDepartment> pGetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspMstInitialDepartment";

				command.Parameters.Add(command.CreateParameter("@vType", "GetPaged"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
				command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));
				command.Parameters.Add(command.CreateParameter("@vRowSkip", rowSkip));
				command.Parameters.Add(command.CreateParameter("@vPageSize", pageSize));

				return this.ReadTransaction(command).ToList();
			}
		}
		private MstInitialDepartment pGetByPK(int iD)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspMstInitialDepartment";

				command.Parameters.Add(command.CreateParameter("@vType", "GetByPK"));
				command.Parameters.Add(command.CreateParameter("@ID", iD));

				return this.ReadTransaction(command).SingleOrDefault();
			}
		}
		private MstInitialDepartment pCreate(MstInitialDepartment data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<MstInitialDepartment>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspMstInitialDepartment";

				command.Parameters.Add(command.CreateParameter("@vType", "Create"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));

				data.ID = this.WriteTransaction(command);

				return data;
			}
		}
		private List<MstInitialDepartment> pCreateBulky(List<MstInitialDepartment> data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<List<MstInitialDepartment>>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspMstInitialDepartment";

				command.Parameters.Add(command.CreateParameter("@vType", "CreateBulky"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));

				command.ExecuteNonQuery();

				return data;
			}
		}
		private MstInitialDepartment pUpdate(MstInitialDepartment data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<MstInitialDepartment>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspMstInitialDepartment";

				command.Parameters.Add(command.CreateParameter("@vType", "Update"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));
				command.Parameters.Add(command.CreateParameter("@ID", data.ID));

				command.ExecuteNonQuery();

				return data;
			}
		}
		private void pDeleteByPK(int iD)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspMstInitialDepartment";

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
				command.CommandText = "dbo.uspMstInitialDepartment";

				command.Parameters.Add(command.CreateParameter("@vType", "DeleteByFilter"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

				command.ExecuteNonQuery();
			}
		}

        private List<MstInitialDepartment> pGetDirectorate()
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspvwStopAccrueDashboard";

                command.Parameters.Add(command.CreateParameter("@v_Type", "GetDirectorate"));
                
                return this.ReadTransaction(command).ToList();
            }
        }
        #endregion

	}
}