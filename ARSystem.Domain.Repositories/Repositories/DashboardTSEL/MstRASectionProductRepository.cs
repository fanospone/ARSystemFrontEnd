
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
	public class MstRASectionProductRepository : BaseRepository<MstRASectionProduct>
	{
		private DbContext _context;
		public MstRASectionProductRepository(DbContext context) : base(context)
		{
			_context = context;
		}

		public int GetCount(string whereClause = "")
		{
			return pGetCount(whereClause);
		}

		public List<MstRASectionProduct> GetList(string whereClause = "", string orderBy = "")
		{
			return pGetList(whereClause, orderBy);
		}

		public List<MstRASectionProduct> GetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
		{
			return pGetPaged(whereClause, orderBy, rowSkip, pageSize);
		}

		public MstRASectionProduct GetByPK(int sectionProductId)
		{
			return pGetByPK(sectionProductId);
		}

		public MstRASectionProduct Create(MstRASectionProduct data)
		{
			return pCreate(data);
		}

		public List<MstRASectionProduct> CreateBulky(List<MstRASectionProduct> data)
		{
			return pCreateBulky(data);
		}

		public MstRASectionProduct Update(MstRASectionProduct data)
		{
			return pUpdate(data);
		}

		public bool DeleteByPK(int sectionProductId)
		{
			pDeleteByPK(sectionProductId);
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
				command.CommandText = "dbo.uspMstRASectionProduct";

				command.Parameters.Add(command.CreateParameter("@vType", "GetCount"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

				return this.CountTransaction(command);
			}
		}
		private List<MstRASectionProduct> pGetList(string whereClause, string orderBy)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspMstRASectionProduct";

				command.Parameters.Add(command.CreateParameter("@vType", "GetList"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
				command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));

				return this.ReadTransaction(command).ToList();
			}
		}
		private List<MstRASectionProduct> pGetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspMstRASectionProduct";

				command.Parameters.Add(command.CreateParameter("@vType", "GetPaged"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
				command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));
				command.Parameters.Add(command.CreateParameter("@vRowSkip", rowSkip));
				command.Parameters.Add(command.CreateParameter("@vPageSize", pageSize));

				return this.ReadTransaction(command).ToList();
			}
		}
		private MstRASectionProduct pGetByPK(int sectionProductId)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspMstRASectionProduct";

				command.Parameters.Add(command.CreateParameter("@vType", "GetByPK"));
				command.Parameters.Add(command.CreateParameter("@SectionProductId", sectionProductId));

				return this.ReadTransaction(command).SingleOrDefault();
			}
		}
		private MstRASectionProduct pCreate(MstRASectionProduct data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<MstRASectionProduct>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspMstRASectionProduct";

				command.Parameters.Add(command.CreateParameter("@vType", "Create"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));

				data.SectionProductId = this.WriteTransaction(command);

				return data;
			}
		}
		private List<MstRASectionProduct> pCreateBulky(List<MstRASectionProduct> data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<List<MstRASectionProduct>>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspMstRASectionProduct";

				command.Parameters.Add(command.CreateParameter("@vType", "CreateBulky"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));

				command.ExecuteNonQuery();

				return data;
			}
		}
		private MstRASectionProduct pUpdate(MstRASectionProduct data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<MstRASectionProduct>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspMstRASectionProduct";

				command.Parameters.Add(command.CreateParameter("@vType", "Update"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));
				command.Parameters.Add(command.CreateParameter("@SectionProductId", data.SectionProductId));

				command.ExecuteNonQuery();

				return data;
			}
		}
		private void pDeleteByPK(int sectionProductId)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspMstRASectionProduct";

				command.Parameters.Add(command.CreateParameter("@vType", "DeleteByPK"));
				command.Parameters.Add(command.CreateParameter("@SectionProductId", sectionProductId));

				command.ExecuteNonQuery();
			}
		}
		private void pDeleteByFilter(string whereClause)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspMstRASectionProduct";

				command.Parameters.Add(command.CreateParameter("@vType", "DeleteByFilter"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

				command.ExecuteNonQuery();
			}
		}
		#endregion

	}
}