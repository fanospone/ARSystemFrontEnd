using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ARSystem.Domain.DAL;
using ARSystem.Domain.Models;
using System.Data.SqlClient;

namespace ARSystem.Domain.Repositories
{
    public class vwMonitoringBapsDoneHeaderRepository : BaseRepository<vwMonitoringBapsDoneHeader>
    {
        private DbContext _context;
        public vwMonitoringBapsDoneHeaderRepository(DbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<vwMonitoringBapsDoneHeader>> GetList(MonitoringBapsDoneHeaderParam param, int RowSkip, int PageSize)
        {
            return await pGetList(param, RowSkip, PageSize);
        }

        public int GetCount(MonitoringBapsDoneHeaderParam param)
        {
            return pGetCount(param);
        }

        private async Task<List<vwMonitoringBapsDoneHeader>> pGetList(MonitoringBapsDoneHeaderParam param, int RowSkip, int PageSize)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "[dbo].[uspMonitoringBapsDone]";
                command.Parameters.Add(command.CreateParameter("@vGroupBy", param.GroupBy));
                command.Parameters.Add(command.CreateParameter("@vBapsType", param.BapsType));
                command.Parameters.Add(command.CreateParameter("@vCustomerID", param.CustomerID));
                command.Parameters.Add(command.CreateParameter("@vCompanyID", param.CompanyId));
                command.Parameters.Add(command.CreateParameter("@vStipCategoryID", param.StipID));
                command.Parameters.Add(command.CreateParameter("@vYear", param.Year));
                command.Parameters.Add(command.CreateParameter("@vRegionID", param.RegionID));
                command.Parameters.Add(command.CreateParameter("@vProvinceID", param.ProvinceID));
                command.Parameters.Add(command.CreateParameter("@vTenantTypeID", param.ProductID));
                command.Parameters.Add(command.CreateParameter("@vPowerTypeID", param.PowerTypeID));
                command.Parameters.Add(command.CreateParameter("@vRowSkip", RowSkip));
                command.Parameters.Add(command.CreateParameter("@vPageSize", PageSize));
                command.Parameters.Add(command.CreateParameter("@vType", "GetPage"));
                command.Parameters.Add(command.CreateParameter("@vDataType",param.DataType));

                List<vwMonitoringBapsDoneHeader> list = new List<vwMonitoringBapsDoneHeader>();
                SqlDataReader dr = (SqlDataReader)command.ExecuteReader();
                vwMonitoringBapsDoneHeader temp;
                while (dr.Read())
                {
                    temp = new vwMonitoringBapsDoneHeader();
                    temp.RowIndex = dr["RowIndex"].ToString();
                    temp.Descrip = dr["Descrip"].ToString();
                    temp.DescripID = dr["DescripID"].ToString();
                    temp.Jan = dr["Jan"].ToString();
                    temp.Feb = dr["Feb"].ToString();
                    temp.Mar = dr["Mar"].ToString();
                    temp.Apr = dr["Apr"].ToString();
                    temp.Mei = dr["Mei"].ToString();
                    temp.Jun = dr["Jun"].ToString();
                    temp.Jul = dr["Jul"].ToString();
                    temp.Agu = dr["Agu"].ToString();
                    temp.Sep = dr["Sep"].ToString();
                    temp.Okt = dr["Okt"].ToString();
                    temp.Nov = dr["Nov"].ToString();
                    temp.Des = dr["Des"].ToString();

                    list.Add(temp);
                }
                dr.Close();

                return this.ReadTransaction(command).ToList();
            }

        }

        private int pGetCount(MonitoringBapsDoneHeaderParam param)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "[dbo].[uspMonitoringBapsDone]";
                command.Parameters.Add(command.CreateParameter("@vGroupBy", param.GroupBy));
                command.Parameters.Add(command.CreateParameter("@vBapsType", param.BapsType));
                command.Parameters.Add(command.CreateParameter("@vCustomerID", param.CustomerID));
                command.Parameters.Add(command.CreateParameter("@vCompanyID", param.CompanyId));
                command.Parameters.Add(command.CreateParameter("@vStipCategoryID", param.StipID));
                command.Parameters.Add(command.CreateParameter("@vYear", param.Year));
                command.Parameters.Add(command.CreateParameter("@vRegionID", param.RegionID));
                command.Parameters.Add(command.CreateParameter("@vProvinceID", param.ProvinceID));
                command.Parameters.Add(command.CreateParameter("@vTenantTypeID", param.ProductID));
                command.Parameters.Add(command.CreateParameter("@vPowerTypeID", param.PowerTypeID));
                command.Parameters.Add(command.CreateParameter("@vType", "GetCount"));
                command.Parameters.Add(command.CreateParameter("@vDataType", param.DataType));
                return this.CountTransaction(command);
            }
        }


    }
}
