using ARSystem.Domain.DAL;
using ARSystem.Domain.Models;
using ARSystem.Domain.Models.Models.ARSystem;
using ARSystem.Domain.Models.Models.RevenueAssurance;
using ARSystem.Domain.Repositories;
using ARSystem.Domain.Repositories.Repositories.ARSystem;
using ARSystem.Domain.Repositories.Repositories.RevenueAssurance;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Service.ARSystem
{
    public class ReconcileDataService
    {
        private string vwRAReconcile = "uspvwRAReconcile";
        private string vwRAReconcileProcessed = "uspvwRAReconcileProcessed";

        public List<mstRASchedule> GetMstRAScheduleList(List<string> mstIDs, string orderBy = "", string userID = "") {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var mstScheduleDataRepo = new mstRAScheduleRepository(context);

            List<mstRASchedule> dataList = new List<mstRASchedule>();
            try
            {
                string strWhereClause = "";

                if (mstIDs.Count () > 0)
                {
                    strWhereClause += " msc.ID IN (";

                    foreach (var ids in mstIDs)
                    {
                        strWhereClause += $"{ids},";
                    }

                    strWhereClause = strWhereClause.TrimEnd(',');
                    strWhereClause +=  ")";
                }


                dataList = mstScheduleDataRepo.GetMappingList(strWhereClause, orderBy);

                return dataList;
            }
            catch (Exception ex)
            {
                dataList.Add(new mstRASchedule((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "ReconcileDataService", "GetMstRAScheduleList", userID)));
                return dataList;
            }
            finally
            {
                context.Dispose();
            }
        }

        public List<mstRASchedule> GetMstRAScheduleListByReconcileID(List<string> mstIDs, string orderBy = "", string userID = "")
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var mstScheduleDataRepo = new mstRAScheduleRepository(context);

            List<mstRASchedule> dataList = new List<mstRASchedule>();
            try
            {
                string strWhereClause = "";

                if (mstIDs.Count() > 0)
                {
                    strWhereClause += " rc.ID IN (";

                    foreach (var ids in mstIDs)
                    {
                        strWhereClause += $"{ids},";
                    }

                    strWhereClause = strWhereClause.TrimEnd(',');
                    strWhereClause += ")";
                }


                dataList = mstScheduleDataRepo.GetMappingRASChedule(strWhereClause, orderBy);

                return dataList;
            }
            catch (Exception ex)
            {
                dataList.Add(new mstRASchedule((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "ReconcileDataService", "GetMstRAScheduleList", userID)));
                return dataList;
            }
            finally
            {
                context.Dispose();
            }
        }


        public List<vwRAReconcile> GetReconcileDataToList(string strToken, string strCompanyId, string strOperator, string strRenewalYear, string strRenewalYearSeq, string strReconcileType,
    string strCurrency, string strRegional, string strProvince, string strDueDatePerMonth, string Batch, List<string> strSONumber, string TenantType, string strOrderBy = "", int intRowSkip = 0, int intPageSize = 0, int isRaw = 0, string userID = "")
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var ReconcileDataRepo = new vwRAReconcileRepository(context);
            string SP = "";

            List<vwRAReconcile> listReconcileData = new List<vwRAReconcile>();
            List<vwRAReconcile> filter = new List<vwRAReconcile>();
            try
            {
                string strWhereClause = "";
                if (isRaw == 0)
                {
                    strWhereClause = "StatusRecon = 1 ";
                    SP = vwRAReconcile;
                }

                if (isRaw == 1)
                {
                    strWhereClause = "StatusRecon = 2 ";
                    SP = vwRAReconcileProcessed;
                }

                if (isRaw == 2)
                {
                    strWhereClause = "StatusRecon = 3 ";
                    SP = vwRAReconcileProcessed;
                }

                if (isRaw == 3)
                {
                    //strWhereClause = " EXISTS(Select 1 From PICAReconcile pica Where pica.mstRAScheduleID = vwRAReconcile.ID AND IsActive = 1)";
                    strWhereClause = " 1 = 1 ";
                    SP = "uspvwRAPICAReconcile";
                }

                if (!string.IsNullOrEmpty(Batch))
                {

                    if (Batch.Substring(Batch.Length - 2, 2) == "01")
                        strWhereClause += " AND BATCH = '" + Batch + "' ";
                    else
                        strWhereClause += " AND (( Term = 2 OR MaxTerm = Term OR ((MONTH(StartInvoiceDate) = 1 AND DAY(StartInvoiceDate) = 1) AND (MONTH(EndInvoiceDate) = 12 AND DAY(EndInvoiceDate) = 31)) ) AND YEAR(StartInvoiceDate) <= " + Batch.Substring(0, 4) + " )";
                    //strWhereClause += "  AND ((CAST(StartInvoiceDate AS DATE) = '" + Batch.Substring(0, 4) + "0101' and CAST(EndInvoiceDate AS DATE) = '" + Batch.Substring(0, 4) + "1231') or MaxTerm = Term) AND YEAR(StartInvoiceDate) = " + Batch.Substring(0, 4);
                }

                if (!string.IsNullOrEmpty(TenantType))
                {
                    strWhereClause += " AND ProductID = " + TenantType;
                }

                if (strSONumber.Count > 0 && strSONumber[0] != null)
                {
                    foreach (var item in strSONumber)
                    {
                        filter.Add(new vwRAReconcile
                        {
                            SONumber = item
                        });
                    }
                }

                //Added by Ibnu Setiawan - Filtering data list for dynamic query
                if (!string.IsNullOrEmpty(strOperator))
                    strWhereClause += " AND CustomerID = '" + strOperator + "'";
                if (!string.IsNullOrEmpty(strCompanyId))
                    strWhereClause += " AND CompanyInvoice = '" + strCompanyId + "'";

                if (!string.IsNullOrEmpty(strRenewalYear))
                    strWhereClause += " AND YEAR(StartInvoiceDate) = " + strRenewalYear;
                if (!string.IsNullOrEmpty(strDueDatePerMonth))
                    strWhereClause += " AND MONTH(StartInvoiceDate) = " + strDueDatePerMonth;
                if (!string.IsNullOrEmpty(strRenewalYearSeq))
                    strWhereClause += " AND Quartal = " + strRenewalYearSeq;
                if (!string.IsNullOrEmpty(strCurrency))
                    strWhereClause += " AND Currency = '" + strCurrency + "'";
                if (!string.IsNullOrEmpty(strRegional))
                    strWhereClause += " AND RegionID = " + strRegional;
                if (!string.IsNullOrEmpty(strProvince))
                    strWhereClause += " AND ProvinceID = " + strProvince;

                if (intPageSize > 0)
                    listReconcileData = ReconcileDataRepo.GetPaged(strWhereClause, strOrderBy, intRowSkip, intPageSize, SP, filter);
                else
                    listReconcileData = ReconcileDataRepo.GetList(strWhereClause, strOrderBy, SP, filter);

                return listReconcileData;
            }
            catch (Exception ex)
            {
                listReconcileData.Add(new vwRAReconcile((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "ReconcileDataService", "GetReconcileDataToList", userID)));
                return listReconcileData;
            }
            finally
            {
                context.Dispose();
            }
        }

        public int GetReconcileDataCount(string strToken, string strCompanyId, string strOperator, string strRenewalYear, string strRenewalYearSeq, string strReconcileType, string strCurrency,
            string strRegional, string strProvince, string strDueDatePerMonth, string Batch, List<string> strSONumber, string TenantType, int isRaw = 0, string userID = "")
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var ReconcileDataRepo = new vwRAReconcileRepository(context);
            List<vwRAReconcile> listReconcileData = new List<vwRAReconcile>();
            List<vwRAReconcile> filter = new List<vwRAReconcile>();

            try
            {
                string SP = "";
                string strWhereClause = "";
                if (isRaw == 0)
                {
                    strWhereClause = "StatusRecon = 1 ";
                    SP = vwRAReconcile;
                }

                if (isRaw == 1)
                {
                    strWhereClause = "StatusRecon = 2 ";
                    SP = vwRAReconcileProcessed;
                }

                if (isRaw == 2)
                {
                    strWhereClause = "StatusRecon = 3 ";
                    SP = vwRAReconcileProcessed;
                }

                if (isRaw == 3)
                {
                    //strWhereClause = " EXISTS(Select 1 From PICAReconcile pica Where pica.mstRAScheduleID = vwRAReconcile.ID AND IsActive = 1)";
                    strWhereClause = " 1 = 1 ";
                    SP = "uspvwRAPICAReconcile";
                }
                //Added by Ibnu Setiawan - Filtering data list for dynamic query
                if (!string.IsNullOrEmpty(Batch))
                {

                    if (Batch.Substring(Batch.Length - 2, 2) == "01")
                        strWhereClause += " AND BATCH = '" + Batch + "' ";
                    else
                        strWhereClause += " AND (( Term = 2 OR MaxTerm = Term OR ((MONTH(StartInvoiceDate) = 1 AND DAY(StartInvoiceDate) = 1) AND (MONTH(EndInvoiceDate) = 12 AND DAY(EndInvoiceDate) = 31)) ) AND YEAR(StartInvoiceDate) <= " + Batch.Substring(0, 4) + " )";
                    //strWhereClause += "  AND ((CAST(StartInvoiceDate AS DATE) = '" + Batch.Substring(0, 4) + "0101' and CAST(EndInvoiceDate AS DATE) = '" + Batch.Substring(0, 4) + "1231') or MaxTerm = Term) AND YEAR(StartInvoiceDate) = " + Batch.Substring(0, 4);
                }
                //if (!string.IsNullOrEmpty(Batch))
                //{
                //    strWhereClause += " AND BATCH = '" + Batch + "' ";
                //    //strWhereClause += "  AND ((CAST(StartInvoiceDate AS DATE) = '" + Batch.Substring(0, 4) + "0101' and CAST(EndInvoiceDate AS DATE) = '" + Batch.Substring(0, 4) + "1231') or MaxTerm = Term) AND YEAR(StartInvoiceDate) = " + Batch.Substring(0, 4);
                //}

                if (!string.IsNullOrEmpty(TenantType))
                {
                    strWhereClause += " AND ProductID = " + TenantType;
                }

                if (strSONumber.Count > 0 && strSONumber[0] != null)
                {
                    foreach (var item in strSONumber)
                    {
                        filter.Add(new vwRAReconcile
                        {
                            SONumber = item
                        });
                    }
                }

                //Added by Ibnu Setiawan - Filtering data list for dynamic query
                if (!string.IsNullOrEmpty(strOperator))
                    strWhereClause += " AND CustomerID = '" + strOperator + "'";
                if (!string.IsNullOrEmpty(strCompanyId))
                    strWhereClause += " AND CompanyInvoice = '" + strCompanyId + "'";
                if (!string.IsNullOrEmpty(strRenewalYear))
                    strWhereClause += " AND YEAR(StartInvoiceDate) = " + strRenewalYear;
                if (!string.IsNullOrEmpty(strDueDatePerMonth))
                    strWhereClause += " AND MONTH(StartInvoiceDate) = " + strDueDatePerMonth;
                if (!string.IsNullOrEmpty(strRenewalYearSeq))
                    strWhereClause += " AND [Quartal] = " + strRenewalYearSeq;
                if (!string.IsNullOrEmpty(strCurrency))
                    strWhereClause += " AND Currency = '" + strCurrency + "'";
                if (!string.IsNullOrEmpty(strRegional))
                    strWhereClause += " AND RegionID = " + strRegional;
                if (!string.IsNullOrEmpty(strProvince))
                    strWhereClause += " AND ProvinceID = " + strProvince;


                return ReconcileDataRepo.GetCount(strWhereClause, SP, filter);
            }
            catch (Exception ex)
            {
                Helper.logError(ex.Message.ToString(), "ReconcileDataService", "GetReconcileDataCount", userID);
                return 0;
            }
            finally
            {
                context.Dispose();
            }
        }

        public List<int> GetReconcileDataListId(string strToken, string strCompanyId, string strOperator, string strRenewalYear, string strRenewalYearSeq, string strReconcileType, string strCurrency,
            string strRegional, string strProvince, string strDueDatePerMonth, string Batch, List<string> strSONumber, string TenantType, int isRaw = 0, string userID = "")
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var CustomRepo = new GetListIdRepository(context);
            List<int> ListId = new List<int>();
            List<vwRAReconcile> filter = new List<vwRAReconcile>();

            try
            {
                string strWhereClause = "";
                string SP = "";
                if (isRaw == 0)
                {
                    strWhereClause = "StatusRecon = 1 ";
                    SP = "vwRAReconcile";
                }

                if (isRaw == 1)
                {
                    strWhereClause = "StatusRecon = 2 ";
                    SP = "vwRAReconcileProcessed";
                }

                if (isRaw == 2)
                {
                    strWhereClause = "StatusRecon = 3 ";
                    SP = "vwRAReconcileProcessed";
                }

                if (isRaw == 3)
                {
                    //strWhereClause = " EXISTS(Select 1 From PICAReconcile pica Where pica.mstRAScheduleID = vwRAReconcile.ID AND IsActive = 1)";
                    strWhereClause = " 1 = 1 ";
                    SP = "vwRAPICAReconcile";
                }
                //Added by Ibnu Setiawan - Filtering data list for dynamic query
                if (!string.IsNullOrEmpty(Batch))
                {

                    if (Batch.Substring(Batch.Length - 2, 2) == "01")
                        strWhereClause += " AND BATCH = '" + Batch + "' ";
                    else
                        strWhereClause += " AND (( Term = 2 OR MaxTerm = Term OR ((MONTH(StartInvoiceDate) = 1 AND DAY(StartInvoiceDate) = 1) AND (MONTH(EndInvoiceDate) = 12 AND DAY(EndInvoiceDate) = 31)) ) AND YEAR(StartInvoiceDate) <= " + Batch.Substring(0, 4) + " )";
                    //strWhereClause += "  AND ((CAST(StartInvoiceDate AS DATE) = '" + Batch.Substring(0, 4) + "0101' and CAST(EndInvoiceDate AS DATE) = '" + Batch.Substring(0, 4) + "1231') or MaxTerm = Term) AND YEAR(StartInvoiceDate) = " + Batch.Substring(0, 4);
                }

                //if (!string.IsNullOrEmpty(Batch))
                //{
                //    strWhereClause += " AND BATCH = '" + Batch + "' ";
                //    //strWhereClause += "  AND ((CAST(StartInvoiceDate AS DATE) = '" + Batch.Substring(0, 4) + "0101' and CAST(EndInvoiceDate AS DATE) = '" + Batch.Substring(0, 4) + "1231') or MaxTerm = Term) AND YEAR(StartInvoiceDate) = " + Batch.Substring(0, 4);
                //}

                if (!string.IsNullOrEmpty(TenantType))
                {
                    strWhereClause += " AND ProductID = " + TenantType;
                }

                if (strSONumber.Count > 0 && strSONumber[0] != null)
                {
                    strWhereClause += " AND SONumber IN ('-X'";
                    foreach (var item in strSONumber)
                    {
                        strWhereClause += ",'" + item + "'";
                        //filter.Add(new Domain.Models.vwRAReconcile
                        //{
                        //    SONumber = item
                        //});
                    }
                    strWhereClause += ")";
                }

                //Added by Ibnu Setiawan - Filtering data list for dynamic query
                if (!string.IsNullOrEmpty(strOperator))
                    strWhereClause += " AND CustomerID = '" + strOperator + "'";
                if (!string.IsNullOrEmpty(strCompanyId))
                    strWhereClause += " AND CompanyInvoice = '" + strCompanyId + "'";
                if (!string.IsNullOrEmpty(strRenewalYear))
                    strWhereClause += " AND YEAR(StartInvoiceDate) = " + strRenewalYear;
                if (!string.IsNullOrEmpty(strDueDatePerMonth))
                    strWhereClause += " AND MONTH(StartInvoiceDate) = " + strDueDatePerMonth;
                if (!string.IsNullOrEmpty(strRenewalYearSeq))
                    strWhereClause += " AND [Quartal] = " + strRenewalYearSeq;
                if (!string.IsNullOrEmpty(strCurrency))
                    strWhereClause += " AND Currency = '" + strCurrency + "'";
                if (!string.IsNullOrEmpty(strRegional))
                    strWhereClause += " AND RegionID = " + strRegional;
                if (!string.IsNullOrEmpty(strProvince))
                    strWhereClause += " AND ProvinceID = " + strProvince;

                ListId = CustomRepo.GetListId("Id", SP, strWhereClause);
                return ListId;
            }
            catch (Exception ex)
            {
                Helper.logError(ex.Message.ToString(), "ReconcileDataService", "GetReconcileDataListId", userID);
                return ListId;
            }
            finally
            {
                context.Dispose();
            }
        }


        public int ProcessNextActivity(string strToken, string Status, List<long> Id, string ListID, string userID = "")
        {
            int ListId = 0;

            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var mstRAScheduleRepository = new mstRAScheduleRepository(context);
            var log = new trxRALogActivityRepository(context);
            List<trxRALogActivity> logs = new List<trxRALogActivity>();

            List<mstRASchedule> datas = new List<mstRASchedule>();

            try
            {

                var result = mstRAScheduleRepository.CreateBulky(Int32.Parse(Status), ListID, userID);
                if (result != 0)
                {
                    foreach (var data in Id)
                    {
                        //datas.Add(new mstRASchedule
                        //{
                        //    ID = data,
                        //    CreatedBy = userCredential.UserID,
                        //    CreatedDate = DateTime.Now
                        //});

                        logs.Add(new trxRALogActivity
                        {
                            Label = "Reconcile ProcessNextActivity",
                            LogDate = DateTime.Now,
                            LogState = true,
                            mstRAActivityID = Int32.Parse(Status),
                            Remarks = "Reconcile ProcessNextActivity with Next Activity : " + Status,
                            TransactionID = data,
                            UserID = userID
                        });
                    }

                    ListId = 1;
                    log.CreateBulky(logs);
                }

                return ListId;
            }
            catch (Exception ex)
            {
                Helper.logError(ex.Message.ToString(), "ReconcileDataService", "ProcessNextActivity", userID);
                return 0;
            }
            finally
            {
                context.Dispose();
            }

        }

        public int UpdateNextActivity(string strToken, string Id, string Activity, string userID = "")
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));

            int ListId = new int();
            try
            {

                var command = context.CreateCommand();
                command.CommandType = CommandType.Text;
                if (Activity == "1")
                    command.CommandText = "DELETE FROM trxReconcile " + " WHERE ID IN (" + Id + ")";
                else
                    command.CommandText = "UPDATE trxReconcile SET mstRAActivityID = " + Activity + " WHERE ID IN (" + Id + ")";

                var data = command.CommandText = "UPDATE trxReconcile SET mstRAActivityID = " + Activity + " WHERE ID IN (" + Id + ")";

                ListId = command.ExecuteNonQuery();

                if (ListId != 0)
                {
                    var log = new trxRALogActivityRepository(context);
                    List<trxRALogActivity> logs = new List<trxRALogActivity>();

                    if (Id.Split(',').Length < 2)
                    {
                        logs.Add(new trxRALogActivity
                        {
                            Label = "Reconcile ProcessNextActivity",
                            LogDate = DateTime.Now,
                            LogState = true,
                            mstRAActivityID = Int32.Parse(Activity),
                            Remarks = "Reconcile ProcessNextActivity with Next Activity : " + Activity,
                            TransactionID = Int64.Parse(Id),
                            UserID = userID
                        });
                    }
                    else
                    {
                        foreach (var item in Id.Split(','))
                        {
                            logs.Add(new trxRALogActivity
                            {
                                Label = "Reconcile ProcessNextActivity",
                                LogDate = DateTime.Now,
                                LogState = true,
                                mstRAActivityID = Int32.Parse(Activity),
                                Remarks = "Reconcile ProcessNextActivity with Next Activity : " + Activity,
                                TransactionID = Int64.Parse(item),
                                UserID = userID
                            });
                        }
                    }


                    log.CreateBulky(logs);
                }

                return ListId;
            }
            catch (Exception ex)
            {
                Helper.logError(ex.Message.ToString(), "ReconcileDataService", "UpdateNextActivity", userID);
                return 0;
            }
            finally
            {
                context.Dispose();
            }

        }

    }
}
