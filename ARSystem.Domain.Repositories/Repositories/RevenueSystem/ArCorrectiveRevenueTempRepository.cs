
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
	public class ArCorrectiveRevenueTempRepository : BaseRepository<ArCorrectiveRevenueTemp>
	{
		private DbContext _context;
		public ArCorrectiveRevenueTempRepository(DbContext context) : base(context)
		{
			_context = context;
		}

		public int GetCount(string whereClause = "")
		{
			return pGetCount(whereClause);
		}

		public List<ArCorrectiveRevenueTemp> GetList(string whereClause = "", string orderBy = "")
		{
			return pGetList(whereClause, orderBy);
		}

		public List<ArCorrectiveRevenueTemp> GetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
		{
			return pGetPaged(whereClause, orderBy, rowSkip, pageSize);
		}

		public ArCorrectiveRevenueTemp GetByPK(string soNumber, string typeAdjusment)
		{
			return pGetByPK(soNumber, typeAdjusment);
		}

		public ArCorrectiveRevenueTemp Create(ArCorrectiveRevenueTemp data)
		{
			return pCreate(data);
		}

		public List<ArCorrectiveRevenueTemp> CreateBulky(List<ArCorrectiveRevenueTemp> data)
		{
			return pCreateBulky(data);
		}

		public ArCorrectiveRevenueTemp Update(ArCorrectiveRevenueTemp data)
		{
			return pUpdate(data);
		}

		public bool DeleteByPK(string soNumber, string typeAdjusment)
		{
			pDeleteByPK(soNumber, typeAdjusment);
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
				command.CommandText = "dbo.uspArCorrectiveRevenueTemp";

				command.Parameters.Add(command.CreateParameter("@vType", "GetCount"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

				return this.CountTransaction(command);
			}
		}
		private List<ArCorrectiveRevenueTemp> pGetList(string whereClause, string orderBy)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspArCorrectiveRevenueTemp";

				command.Parameters.Add(command.CreateParameter("@vType", "GetList"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
				command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));

				return this.ReadTransaction(command).ToList();
			}
		}
		private List<ArCorrectiveRevenueTemp> pGetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspArCorrectiveRevenueTemp";

				command.Parameters.Add(command.CreateParameter("@vType", "GetPaged"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
				command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));
				command.Parameters.Add(command.CreateParameter("@vRowSkip", rowSkip));
				command.Parameters.Add(command.CreateParameter("@vPageSize", pageSize));

				return this.ReadTransaction(command).ToList();
			}
		}
		private ArCorrectiveRevenueTemp pGetByPK(string soNumber, string typeAdjusment)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspArCorrectiveRevenueTemp";

				command.Parameters.Add(command.CreateParameter("@vType", "GetByPK"));
				command.Parameters.Add(command.CreateParameter("@SoNumber", soNumber));
				command.Parameters.Add(command.CreateParameter("@TypeAdjusment", typeAdjusment));

				return this.ReadTransaction(command).SingleOrDefault();
			}
		}
		private ArCorrectiveRevenueTemp pCreate(ArCorrectiveRevenueTemp data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<ArCorrectiveRevenueTemp>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspArCorrectiveRevenueTemp";

				command.Parameters.Add(command.CreateParameter("@vType", "Create"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));

				this.WriteTransaction(command);

				return data;
			}
		}
		private List<ArCorrectiveRevenueTemp> pCreateBulky(List<ArCorrectiveRevenueTemp> data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<List<ArCorrectiveRevenueTemp>>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspArCorrectiveRevenueTemp";

				command.Parameters.Add(command.CreateParameter("@vType", "CreateBulky"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));
				command.Parameters.Add(command.CreateParameter("@vUserId", data[0].CreatedBy));

				command.ExecuteNonQuery();

				return data;
			}
		}
		private ArCorrectiveRevenueTemp pUpdate(ArCorrectiveRevenueTemp data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<ArCorrectiveRevenueTemp>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspArCorrectiveRevenueTemp";

				command.Parameters.Add(command.CreateParameter("@vType", "Update"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));
				command.Parameters.Add(command.CreateParameter("@SoNumber", data.SoNumber));
				command.Parameters.Add(command.CreateParameter("@TypeAdjusment", data.TypeAdjusment));

				command.ExecuteNonQuery();

				return data;
			}
		}
		private void pDeleteByPK(string soNumber, string typeAdjusment)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspArCorrectiveRevenueTemp";

				command.Parameters.Add(command.CreateParameter("@vType", "DeleteByPK"));
				command.Parameters.Add(command.CreateParameter("@SoNumber", soNumber));
				command.Parameters.Add(command.CreateParameter("@TypeAdjusment", typeAdjusment));

				command.ExecuteNonQuery();
			}
		}
		private void pDeleteByFilter(string whereClause)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspArCorrectiveRevenueTemp";

				command.Parameters.Add(command.CreateParameter("@vType", "DeleteByFilter"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

				command.ExecuteNonQuery();
			}
		}
		#endregion

	}
}