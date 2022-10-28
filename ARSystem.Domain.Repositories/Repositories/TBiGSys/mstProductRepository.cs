
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
	public class mstProductRepository : BaseRepository<mstProduct>
	{
		private DbContext _context;
		public mstProductRepository(DbContext context) : base(context)
		{
			_context = context;
		}

		public int GetCount(string whereClause = "")
		{
			return pGetCount(whereClause);
		}

		public List<mstProduct> GetList(string whereClause = "", string orderBy = "")
		{
			return pGetList(whereClause, orderBy);
		}

		public List<mstProduct> GetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
		{
			return pGetPaged(whereClause, orderBy, rowSkip, pageSize);
		}

		public mstProduct GetByPK(int productID)
		{
			return pGetByPK(productID);
		}

		public mstProduct Create(mstProduct data)
		{
			return pCreate(data);
		}

		public List<mstProduct> CreateBulky(List<mstProduct> data)
		{
			return pCreateBulky(data);
		}

		public mstProduct Update(mstProduct data)
		{
			return pUpdate(data);
		}

		public bool DeleteByPK(int productID)
		{
			pDeleteByPK(productID);
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
				command.CommandText = "dbo.uspmstProduct";

				command.Parameters.Add(command.CreateParameter("@vType", "GetCount"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

				return this.CountTransaction(command);
			}
		}
		private List<mstProduct> pGetList(string whereClause, string orderBy)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspmstProduct";

				command.Parameters.Add(command.CreateParameter("@vType", "GetList"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
				command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));

				return this.ReadTransaction(command).ToList();
			}
		}
		private List<mstProduct> pGetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspmstProduct";

				command.Parameters.Add(command.CreateParameter("@vType", "GetPaged"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
				command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));
				command.Parameters.Add(command.CreateParameter("@vRowSkip", rowSkip));
				command.Parameters.Add(command.CreateParameter("@vPageSize", pageSize));

				return this.ReadTransaction(command).ToList();
			}
		}
		private mstProduct pGetByPK(int productID)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspmstProduct";

				command.Parameters.Add(command.CreateParameter("@vType", "GetByPK"));
				command.Parameters.Add(command.CreateParameter("@ProductID", productID));

				return this.ReadTransaction(command).SingleOrDefault();
			}
		}
		private mstProduct pCreate(mstProduct data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<mstProduct>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspmstProduct";

				command.Parameters.Add(command.CreateParameter("@vType", "Create"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));

				this.WriteTransaction(command);

				return data;
			}
		}
		private List<mstProduct> pCreateBulky(List<mstProduct> data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<List<mstProduct>>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspmstProduct";

				command.Parameters.Add(command.CreateParameter("@vType", "CreateBulky"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));

				command.ExecuteNonQuery();

				return data;
			}
		}
		private mstProduct pUpdate(mstProduct data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<mstProduct>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspmstProduct";

				command.Parameters.Add(command.CreateParameter("@vType", "Update"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));
				command.Parameters.Add(command.CreateParameter("@ProductID", data.ProductID));

				command.ExecuteNonQuery();

				return data;
			}
		}
		private void pDeleteByPK(int productID)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspmstProduct";

				command.Parameters.Add(command.CreateParameter("@vType", "DeleteByPK"));
				command.Parameters.Add(command.CreateParameter("@ProductID", productID));

				command.ExecuteNonQuery();
			}
		}
		private void pDeleteByFilter(string whereClause)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspmstProduct";

				command.Parameters.Add(command.CreateParameter("@vType", "DeleteByFilter"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

				command.ExecuteNonQuery();
			}
		}
		#endregion

	}
}