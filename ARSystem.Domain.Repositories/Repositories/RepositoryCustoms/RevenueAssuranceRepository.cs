using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ARSystem.Domain.DAL;
using ARSystem.Domain.Models;
using System.Data;
using System.Data.SqlClient;

namespace ARSystem.Domain.Repositories
{
    public class RevenueAssuranceRepository : BaseRepository<mstDropdown>
    {
        private DbContext _context;
        public RevenueAssuranceRepository(DbContext context) : base(context)
        {
            _context = context;
        }

        //public List<vmAreaOM> GetAreaOM(string userID)
        //{
        //    List<vmAreaOM> list = new List<vmAreaOM>();
        //    using (var command = _context.CreateCommand())
        //        {
        //            command.CommandType = CommandType.StoredProcedure;
        //            command.CommandText = "dbo.uspGetAreaOM";

        //            command.Parameters.Add(command.CreateParameter("@userID", userID));

        //            SqlDataReader dr = (SqlDataReader)command.ExecuteReader();
        //            vmAreaOM temp;
        //            while (dr.Read())
        //            {
        //                temp = new vmAreaOM();
        //                temp.ValueId = dr["AreaID"].ToString();
        //                temp.ValueDesc = dr["AreaName"].ToString();

        //            list.Add(temp);
        //            }
        //            dr.Close();
        //        }
        //        return list;
        //}

        //public List<vmYears> GetYears()
        //{
        //    List<vmYears> list = new List<vmYears>();
        //    using (var command = _context.CreateCommand())
        //    {
        //        command.CommandType = CommandType.StoredProcedure;
        //        command.CommandText = "dbo.uspGetYears";

        //        SqlDataReader dr = (SqlDataReader)command.ExecuteReader();
        //        vmYears temp;
        //        while (dr.Read())
        //        {
        //            temp = new vmYears();
        //            temp.ValueId = dr["ValueId"].ToString();
        //            temp.ValueDesc = dr["ValueDesc"].ToString();

        //            list.Add(temp);
        //        }
        //        dr.Close();
        //    }
        //    return list;
        //}

        public List<vmStatus> GetStatus(string CurrentActivity)
        {
            List<vmStatus> list = new List<vmStatus>();
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspGetStatus";

                command.Parameters.Add(command.CreateParameter("@CurrentActivity", CurrentActivity));

                SqlDataReader dr = (SqlDataReader)command.ExecuteReader();
                vmStatus temp;
                while (dr.Read())
                {
                    temp = new vmStatus();
                    temp.ValueId = dr["NextActivityID"].ToString();
                    temp.ValueDesc = dr["Status"].ToString();

                    list.Add(temp);
                }
                dr.Close();
            }
            return list;
        }

        public List<mstDropdown> GetNextActivity(int CurrentActivity,string CustomerID="")
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspGetNextStep";

                command.Parameters.Add(command.CreateParameter("@CurrentStep", CurrentActivity));
                command.Parameters.Add(command.CreateParameter("@CustomerID", CustomerID));

                return this.ReadTransaction(command).ToList();
            }
        }
        
        public List<vmBAPSRecurring> GetDataBAPSRecurring(int ID)
        {
            List<vmBAPSRecurring> list = new List<vmBAPSRecurring>();
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspGetDataBAPSRecurring";

                command.Parameters.Add(command.CreateParameter("@ID", ID));

                SqlDataReader dr = (SqlDataReader)command.ExecuteReader();
                vmBAPSRecurring temp;
                while (dr.Read())
                {
                    temp = new vmBAPSRecurring();
                    temp.TotalTenant = Convert.ToInt32(dr["TotalTenant"].ToString());
                    temp.TotalAmount = Convert.ToDecimal(dr["TotalAmount"].ToString());
                    list.Add(temp);
                }
                dr.Close();
            }
            return list;
        }
        
        public void RejectBOQBulky(int mstRABoqID)
        {
            using (var command = _context.CreateCommand())
            {

                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.usptrxReconcile";
                
                command.Parameters.Add(command.CreateParameter("@vType", "RejectBOQBulky"));
                command.Parameters.Add(command.CreateParameter("@ID", mstRABoqID));

                command.ExecuteNonQuery();

            }
        }

        public trxReconcile UpdateDetailBAPS(vmSaveBAPSBulky ModelSaveBAPSBulky)
        {
            trxReconcile trx = new trxReconcile();

            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspUpdateDetailBAPS";

                command.Parameters.Add(command.CreateParameter("@ID", ModelSaveBAPSBulky.trxReconcileID)); 
                command.Parameters.Add(command.CreateParameter("@SoNumber", ModelSaveBAPSBulky.SONumber));
                command.Parameters.Add(command.CreateParameter("@CustomerSiteID", ModelSaveBAPSBulky.CustomerSiteID));
                command.Parameters.Add(command.CreateParameter("@CustomerMLANumber", ModelSaveBAPSBulky.CustomerMLANumber));
                command.Parameters.Add(command.CreateParameter("@BaseLeasePrice", ModelSaveBAPSBulky.BaseLeasePrice));
                command.Parameters.Add(command.CreateParameter("@DeductionAmount", ModelSaveBAPSBulky.DeductionAmount));
                command.Parameters.Add(command.CreateParameter("@ServicePrice", ModelSaveBAPSBulky.ServicePrice));
                command.Parameters.Add(command.CreateParameter("@AmountIDR", ModelSaveBAPSBulky.AmountIDR));
                command.Parameters.Add(command.CreateParameter("@RFIOprDate", ModelSaveBAPSBulky.RFIOprDate));

                command.ExecuteNonQuery();
            }

            return trx;
        }

        public int UpdateInflationAmount(long ID, decimal InflationAmount=0)
        {
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspUpdateInflationAmount";

                command.Parameters.Add(command.CreateParameter("@ID", ID));
                command.Parameters.Add(command.CreateParameter("@InflationAmount", InflationAmount));

                return command.ExecuteNonQuery();
            }
        }

        public int UpdateBulkyReconcileAmount(List<trxReconcile> data)
        {
            using (var command = _context.CreateCommand())
            {
                string xml = Helper.XmlSerializer<List<trxReconcile>>(data);
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.usptrxReconcile";

                command.Parameters.Add(command.CreateParameter("@vType", "UpdateBulkyAmount"));
                command.Parameters.Add(command.CreateParameter("@vXml", xml));

                return command.ExecuteNonQuery();
            }
        }

        public List<vmStatus> CheckSplitPO(List<vmStatus> data)
        {
            List<vmStatus> result = new List<vmStatus>();

            using (var command = _context.CreateCommand())
            {
                string xml = Helper.XmlSerializer<List<vmStatus>>(data);
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspCheckSplitPO";
                
                command.Parameters.Add(command.CreateParameter("@vXml", xml));

                using (var rdr = command.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        result.Add(new vmStatus { ValueId = rdr["ValueId"].ToString() });
                    }
                }

                return result;
            }
        }

        public int SaveRTIPartition(string userID, vmSaveRTIPartition ModelSaveRTIPartition)
        {
            //trxReconcile trx = new trxReconcile();

            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.spSaveSplitRTIData";

                command.Parameters.Add(command.CreateParameter("@_State", ModelSaveRTIPartition.State));
                command.Parameters.Add(command.CreateParameter("@_trxReconcileID", ModelSaveRTIPartition.trxReconcileID));
                command.Parameters.Add(command.CreateParameter("@_Term", ModelSaveRTIPartition.Term));
                command.Parameters.Add(command.CreateParameter("@_EndInvoiceDate", ModelSaveRTIPartition.EndInvoiceDate));
                command.Parameters.Add(command.CreateParameter("@_StartPeriodInvoiceDate", ModelSaveRTIPartition.StartPeriodInvoiceDate));
                command.Parameters.Add(command.CreateParameter("@_EndPeriodInvoiceDate", ModelSaveRTIPartition.EndPeriodInvoiceDate));
                command.Parameters.Add(command.CreateParameter("@_CustomerID", ModelSaveRTIPartition.CustomerID));
                command.Parameters.Add(command.CreateParameter("@_BaseLeasePrice", ModelSaveRTIPartition.BaseLeasePrice));
                command.Parameters.Add(command.CreateParameter("@_OMPrice", ModelSaveRTIPartition.ServicePrice));
                command.Parameters.Add(command.CreateParameter("@_UserID", userID));

                return command.ExecuteNonQuery();

            }

        }

        public bool ValidationUpload(string detail)
        {
            bool validate = false;
            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspGetRTIValidationUpload";

                command.Parameters.Add(command.CreateParameter("@vtrxReconcileID", detail));

                SqlDataReader dr = (SqlDataReader)command.ExecuteReader();
                while (dr.Read())
                {
                    validate = Convert.ToBoolean(dr["Validate"]);
                }
                dr.Close();
            }

            return validate;
        }


        public trxBapsData CheckBapsData(int ID)
        {
            trxBapsData result = new trxBapsData();

            using (var command = _context.CreateCommand())
            {
                command.CommandType = CommandType.StoredProcedure;
                command.CommandText = "dbo.uspCheckTrxBapsData";

                command.Parameters.Add(command.CreateParameter("@Id", ID));

                using (var rdr = command.ExecuteReader())
                {
                    while (rdr.Read())
                    {
                        result.trxBapsDataId = Convert.ToInt32(rdr["trxBapsDataId"].ToString());
                        result.SONumber = rdr["SONumber"].ToString();
                    }
                }

                return result;
            }
        }
    }

}
