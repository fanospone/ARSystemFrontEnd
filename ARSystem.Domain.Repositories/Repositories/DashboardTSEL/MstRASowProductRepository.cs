
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
	public class MstRASowProductRepository : BaseRepository<MstRASowProduct>
	{
		private DbContext _context;
		public MstRASowProductRepository(DbContext context) : base(context)
		{
			_context = context;
		}

		public int GetCount(string whereClause = "")
		{
			return pGetCount(whereClause);
		}

		public List<MstRASowProduct> GetList(string whereClause = "", string orderBy = "")
		{
			return pGetList(whereClause, orderBy);
		}

		public List<MstRASowProduct> GetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
		{
			return pGetPaged(whereClause, orderBy, rowSkip, pageSize);
		}

		public MstRASowProduct GetByPK(int sowProductId)
		{
			return pGetByPK(sowProductId);
		}

		public MstRASowProduct Create(MstRASowProduct data)
		{
			return pCreate(data);
		}

		public List<MstRASowProduct> CreateBulky(List<MstRASowProduct> data)
		{
			return pCreateBulky(data);
		}

		public MstRASowProduct Update(MstRASowProduct data)
		{
			return pUpdate(data);
		}

		public bool DeleteByPK(int sowProductId)
		{
			pDeleteByPK(sowProductId);
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
				command.CommandText = "dbo.uspMstRASowProduct";

				command.Parameters.Add(command.CreateParameter("@vType", "GetCount"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

				return this.CountTransaction(command);
			}
		}
		private List<MstRASowProduct> pGetList(string whereClause, string orderBy)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspMstRASowProduct";

				command.Parameters.Add(command.CreateParameter("@vType", "GetList"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
				command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));

				return this.ReadTransaction(command).ToList();
			}
		}
		private List<MstRASowProduct> pGetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspMstRASowProduct";

				command.Parameters.Add(command.CreateParameter("@vType", "GetPaged"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
				command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));
				command.Parameters.Add(command.CreateParameter("@vRowSkip", rowSkip));
				command.Parameters.Add(command.CreateParameter("@vPageSize", pageSize));

				return this.ReadTransaction(command).ToList();
			}
		}
		private MstRASowProduct pGetByPK(int sowProductId)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspMstRASowProduct";

				command.Parameters.Add(command.CreateParameter("@vType", "GetByPK"));
				command.Parameters.Add(command.CreateParameter("@SowProductId", sowProductId));

				return this.ReadTransaction(command).SingleOrDefault();
			}
		}
		private MstRASowProduct pCreate(MstRASowProduct data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<MstRASowProduct>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspMstRASowProduct";

				command.Parameters.Add(command.CreateParameter("@vType", "Create"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));

				data.SowProductId = this.WriteTransaction(command);

				return data;
			}
		}
		private List<MstRASowProduct> pCreateBulky(List<MstRASowProduct> data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<List<MstRASowProduct>>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspMstRASowProduct";

				command.Parameters.Add(command.CreateParameter("@vType", "CreateBulky"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));

				command.ExecuteNonQuery();

				return data;
			}
		}
		private MstRASowProduct pUpdate(MstRASowProduct data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<MstRASowProduct>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspMstRASowProduct";

				command.Parameters.Add(command.CreateParameter("@vType", "Update"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));
				command.Parameters.Add(command.CreateParameter("@SowProductId", data.SowProductId));

				command.ExecuteNonQuery();

				return data;
			}
		}
		private void pDeleteByPK(int sowProductId)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspMstRASowProduct";

				command.Parameters.Add(command.CreateParameter("@vType", "DeleteByPK"));
				command.Parameters.Add(command.CreateParameter("@SowProductId", sowProductId));

				command.ExecuteNonQuery();
			}
		}
		private void pDeleteByFilter(string whereClause)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspMstRASowProduct";

				command.Parameters.Add(command.CreateParameter("@vType", "DeleteByFilter"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

				command.ExecuteNonQuery();
			}
		}
		#endregion

	}
}