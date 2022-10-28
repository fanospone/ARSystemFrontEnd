
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
	public class mstPICATypeRepository : BaseRepository<mstPICAType>
	{
		private DbContext _context;
		public mstPICATypeRepository(DbContext context) : base(context)
		{
			_context = context;
		}

		public int GetCount(string whereClause = "")
		{
			return pGetCount(whereClause);
		}

		public List<mstPICAType> GetList(string whereClause = "", string orderBy = "")
		{
			return pGetList(whereClause, orderBy);
		}

		public List<mstPICAType> GetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
		{
			return pGetPaged(whereClause, orderBy, rowSkip, pageSize);
		}

		public mstPICAType GetByPK(int mstPICATypeID)
		{
			return pGetByPK(mstPICATypeID);
		}

		public mstPICAType Create(mstPICAType data)
		{
			return pCreate(data);
		}

		public List<mstPICAType> CreateBulky(List<mstPICAType> data)
		{
			return pCreateBulky(data);
		}

		public mstPICAType Update(mstPICAType data)
		{
			return pUpdate(data);
		}

		public bool DeleteByPK(int mstPICATypeID)
		{
			pDeleteByPK(mstPICATypeID);
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
				command.CommandText = "dbo.uspmstPICAType";

				command.Parameters.Add(command.CreateParameter("@vType", "GetCount"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

				return this.CountTransaction(command);
			}
		}
		private List<mstPICAType> pGetList(string whereClause, string orderBy)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspmstPICAType";

				command.Parameters.Add(command.CreateParameter("@vType", "GetList"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
				command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));

				return this.ReadTransaction(command).ToList();
			}
		}
		private List<mstPICAType> pGetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspmstPICAType";

				command.Parameters.Add(command.CreateParameter("@vType", "GetPaged"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
				command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));
				command.Parameters.Add(command.CreateParameter("@vRowSkip", rowSkip));
				command.Parameters.Add(command.CreateParameter("@vPageSize", pageSize));

				return this.ReadTransaction(command).ToList();
			}
		}
		private mstPICAType pGetByPK(int mstPICATypeID)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspmstPICAType";

				command.Parameters.Add(command.CreateParameter("@vType", "GetByPK"));
				command.Parameters.Add(command.CreateParameter("@mstPICATypeID", mstPICATypeID));

				return this.ReadTransaction(command).SingleOrDefault();
			}
		}
		private mstPICAType pCreate(mstPICAType data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<mstPICAType>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspmstPICAType";

				command.Parameters.Add(command.CreateParameter("@vType", "Create"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));

				data.mstPICATypeID = this.WriteTransaction(command);

				return data;
			}
		}
		private List<mstPICAType> pCreateBulky(List<mstPICAType> data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<List<mstPICAType>>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspmstPICAType";

				command.Parameters.Add(command.CreateParameter("@vType", "CreateBulky"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));

				command.ExecuteNonQuery();

				return data;
			}
		}
		private mstPICAType pUpdate(mstPICAType data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<mstPICAType>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspmstPICAType";

				command.Parameters.Add(command.CreateParameter("@vType", "Update"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));
				command.Parameters.Add(command.CreateParameter("@mstPICATypeID", data.mstPICATypeID));

				command.ExecuteNonQuery();

				return data;
			}
		}
		private void pDeleteByPK(int mstPICATypeID)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspmstPICAType";

				command.Parameters.Add(command.CreateParameter("@vType", "DeleteByPK"));
				command.Parameters.Add(command.CreateParameter("@mstPICATypeID", mstPICATypeID));

				command.ExecuteNonQuery();
			}
		}
		private void pDeleteByFilter(string whereClause)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspmstPICAType";

				command.Parameters.Add(command.CreateParameter("@vType", "DeleteByFilter"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

				command.ExecuteNonQuery();
			}
		}
		#endregion

	}
}