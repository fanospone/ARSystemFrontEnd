using ARSystem.Domain.DAL;
using ARSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Domain.Repositories
{
    public class GetDropDownListRepository : BaseRepository<GetDropDownList>
    {
        private DbContext _context;
        public GetDropDownListRepository(DbContext context) : base(context)
        {
            _context = context;
        }

        public List<GetDropDownList> GetDdlCustomer()
        {
            return pGetDdlCustomer();
        }
        public List<GetDropDownList> GetDdlCompany()
        {
            return pGetDdlCompany();
        }

        public List<GetDropDownList> GetDdlProduct()
        {
            return pGetDdlProduct();
        }
        public List<GetDropDownList> GetDdlBapsType()
        {
            return pGetDdlBapsType();
        }

        public List<GetDropDownList> GetDdlStipCategory()
        {
            return pGetDdlStipCategory();
        }

        public List<GetDropDownList> GetDdlActivityStatus()
        {
            return pGetDdlActivityStatus();
        }

        public List<GetDropDownList> GetDdlCategoryPICA()
        {
            return pGetDdlCategoryPICA();
        }

        public List<GetDropDownList> GetDdlPICA(string PICA)
        {
            return pGetDdlPICA(PICA);
        }

        #region Private

        private List<GetDropDownList> pGetDdlCustomer()
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspPICARASystem";

                command.Parameters.Add(command.CreateParameter("@vType", "ddlCustomer"));

                return this.ReadTransaction(command).ToList();
            }
        }

        private List<GetDropDownList> pGetDdlCompany()
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspPICARASystem";

                command.Parameters.Add(command.CreateParameter("@vType", "ddlCompany"));

                return this.ReadTransaction(command).ToList();
            }
        }

        private List<GetDropDownList> pGetDdlProduct()
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspPICARASystem";

                command.Parameters.Add(command.CreateParameter("@vType", "ddlProduct"));

                return this.ReadTransaction(command).ToList();
            }
        }

        private List<GetDropDownList> pGetDdlBapsType()
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspPICARASystem";

                command.Parameters.Add(command.CreateParameter("@vType", "ddlBapsType"));

                return this.ReadTransaction(command).ToList();
            }
        }

        private List<GetDropDownList> pGetDdlStipCategory()
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspPICARASystem";

                command.Parameters.Add(command.CreateParameter("@vType", "ddlStipCategory"));

                return this.ReadTransaction(command).ToList();
            }
        }

        private List<GetDropDownList> pGetDdlActivityStatus()
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspPICARASystem";

                command.Parameters.Add(command.CreateParameter("@vType", "ddlActivityStatus"));

                return this.ReadTransaction(command).ToList();
            }
        }

        private List<GetDropDownList> pGetDdlCategoryPICA()
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspPICARASystem";

                command.Parameters.Add(command.CreateParameter("@vType", "ddlCategoryPICA"));

                return this.ReadTransaction(command).ToList();
            }
        }

        private List<GetDropDownList> pGetDdlPICA(string PICA)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspPICARASystem";

                command.Parameters.Add(command.CreateParameter("@vType", "ddlPICA"));
                command.Parameters.Add(command.CreateParameter("@CategoryPICA", PICA));

                return this.ReadTransaction(command).ToList();
            }
        }
        #endregion
    }
}

