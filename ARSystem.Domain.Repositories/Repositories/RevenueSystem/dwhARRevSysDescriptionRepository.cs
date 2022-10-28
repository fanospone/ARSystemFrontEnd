
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
	public class dwhARRevSysDescriptionRepository : BaseRepository<dwhARRevSysDescription>
	{
		private DbContext _context;
		public dwhARRevSysDescriptionRepository(DbContext context) : base(context)
		{
			_context = context;
		}

		public int GetCount(string whereClause = "")
		{
			return pGetCount(whereClause);
		}

		public List<dwhARRevSysDescription> GetList(string whereClause = "", string orderBy = "")
		{
			return pGetList(whereClause, orderBy);
		}

		public List<dwhARRevSysDescription> GetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
		{
			return pGetPaged(whereClause, orderBy, rowSkip, pageSize);
		}

		public dwhARRevSysDescription GetByPK(string soNumber, short stipId, int dataYear, int dataMonthNumber)
		{
			return pGetByPK(soNumber, stipId, dataYear, dataMonthNumber);
		}

		public dwhARRevSysDescription Create(dwhARRevSysDescription data)
		{
			return pCreate(data);
		}

		public List<dwhARRevSysDescription> CreateBulky(List<dwhARRevSysDescription> data)
		{
			return pCreateBulky(data);
		}

		public dwhARRevSysDescription Update(dwhARRevSysDescription data)
		{
			return pUpdate(data);
		}

		public bool DeleteByPK(string soNumber, short stipId, int dataYear, int dataMonthNumber)
		{
			pDeleteByPK(soNumber, stipId, dataYear, dataMonthNumber);
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
				command.CommandText = "dbo.uspdwhARRevSysDescription";

				command.Parameters.Add(command.CreateParameter("@vType", "GetCount"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

				return this.CountTransaction(command);
			}
		}
		private List<dwhARRevSysDescription> pGetList(string whereClause, string orderBy)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspdwhARRevSysDescription";

				command.Parameters.Add(command.CreateParameter("@vType", "GetList"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
				command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));

				return this.ReadTransaction(command).ToList();
			}
		}
		private List<dwhARRevSysDescription> pGetPaged(string whereClause, string orderBy, int rowSkip, int pageSize)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspdwhARRevSysDescription";

				command.Parameters.Add(command.CreateParameter("@vType", "GetPaged"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));
				command.Parameters.Add(command.CreateParameter("@vOrderBy", orderBy));
				command.Parameters.Add(command.CreateParameter("@vRowSkip", rowSkip));
				command.Parameters.Add(command.CreateParameter("@vPageSize", pageSize));

				return this.ReadTransaction(command).ToList();
			}
		}
		private dwhARRevSysDescription pGetByPK(string soNumber, short stipId, int dataYear, int dataMonthNumber)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspdwhARRevSysDescription";

				command.Parameters.Add(command.CreateParameter("@vType", "GetByPK"));
				command.Parameters.Add(command.CreateParameter("@SoNumber", soNumber));
				command.Parameters.Add(command.CreateParameter("@StipId", stipId));
				command.Parameters.Add(command.CreateParameter("@DataYear", dataYear));
				command.Parameters.Add(command.CreateParameter("@DataMonthNumber", dataMonthNumber));

				return this.ReadTransaction(command).SingleOrDefault();
			}
		}
		private dwhARRevSysDescription pCreate(dwhARRevSysDescription data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<dwhARRevSysDescription>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspdwhARRevSysDescription";

				command.Parameters.Add(command.CreateParameter("@vType", "Create"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));

				this.WriteTransaction(command);

				return data;
			}
		}
		private List<dwhARRevSysDescription> pCreateBulky(List<dwhARRevSysDescription> data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<List<dwhARRevSysDescription>>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspdwhARRevSysDescription";

				command.Parameters.Add(command.CreateParameter("@vType", "CreateBulky"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));

				command.ExecuteNonQuery();

				return data;
			}
		}
		private dwhARRevSysDescription pUpdate(dwhARRevSysDescription data)
		{
			using (var command = _context.CreateCommand())
			{
				string xml = Helper.XmlSerializer<dwhARRevSysDescription>(data);

				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspdwhARRevSysDescription";

				command.Parameters.Add(command.CreateParameter("@vType", "Update"));
				command.Parameters.Add(command.CreateParameter("@vXml", xml));
				command.Parameters.Add(command.CreateParameter("@SoNumber", data.SoNumber));
				command.Parameters.Add(command.CreateParameter("@StipId", data.StipId));
				command.Parameters.Add(command.CreateParameter("@DataYear", data.DataYear));
				command.Parameters.Add(command.CreateParameter("@DataMonthNumber", data.DataMonthNumber));

				command.ExecuteNonQuery();

				return data;
			}
		}
		private void pDeleteByPK(string soNumber, short stipId, int dataYear, int dataMonthNumber)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspdwhARRevSysDescription";

				command.Parameters.Add(command.CreateParameter("@vType", "DeleteByPK"));
				command.Parameters.Add(command.CreateParameter("@SoNumber", soNumber));
				command.Parameters.Add(command.CreateParameter("@StipId", stipId));
				command.Parameters.Add(command.CreateParameter("@DataYear", dataYear));
				command.Parameters.Add(command.CreateParameter("@DataMonthNumber", dataMonthNumber));

				command.ExecuteNonQuery();
			}
		}
		private void pDeleteByFilter(string whereClause)
		{
			using (var command = _context.CreateCommand())
			{
				command.CommandType = CommandType.StoredProcedure;
				command.CommandText = "dbo.uspdwhARRevSysDescription";

				command.Parameters.Add(command.CreateParameter("@vType", "DeleteByFilter"));
				command.Parameters.Add(command.CreateParameter("@vWhereClause", whereClause));

				command.ExecuteNonQuery();
			}
		}
		#endregion

	}
}