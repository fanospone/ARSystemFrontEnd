using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using ARSystem.Domain.Models;
using ARSystem.Domain.Repositories;
using ARSystem.Domain.DAL;

namespace ARSystem.Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "RevenueSystemService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select RevenueSystemService.svc or RevenueSystemService.svc.cs at the Solution Explorer and start debugging.
    public partial class RevenueSystemService
    {
        //#region RevSys-Tower-Overblast-Overquota
        //public List<vmRevSysCompanyAsset> GetRevSysAttributeHeader(string userID, string param)
        //{
        //    var context = new DbContext(Helper.GetConnection("ARSystem"));
        //    var repo = new RevSysRepository(context);
        //    List<vmRevSysCompanyAsset> list = new List<vmRevSysCompanyAsset>();

        //    try
        //    {
        //        list = repo.GetRevSysAttributeHeader(param);

        //        return list;
        //    }
        //    catch (Exception ex)
        //    {
        //        list.Add(new vmRevSysCompanyAsset((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "RevenueSystemService", "GetRevSysAttributeHeader", userID)));
        //        return list;
        //    }
        //    finally
        //    {
        //        context.Dispose();
        //    }
        //}


        //public List<vwRevSysMaxAsatDate> GetRevSysMaxAssatDate(string userID)
        //{
        //    var context = new DbContext(Helper.GetConnection("ARSystem"));
        //    var repo = new vwRevSysMaxAsatDateRepository(context);
        //    List<vwRevSysMaxAsatDate> list = new List<vwRevSysMaxAsatDate>();


        //    try
        //    {
        //        list = repo.GetList("", "");
        //        return list;
        //    }
        //    catch (Exception ex)
        //    {
        //        list.Add(new vwRevSysMaxAsatDate((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "RevenueSystemService", "pGetList".Trim(), userID)));
        //        return list;
        //    }
        //    finally
        //    {
        //        context.Dispose();
        //    }
        //}

        //public List<vmRevSysAmountHeader> GetRevSysAmountHeader(string userID, vmRevSysParameters param)
        //{
        //    var context = new DbContext(Helper.GetConnection("ARSystem"));
        //    var repo = new RevSysRepository(context);
        //    List<vmRevSysAmountHeader> list = new List<vmRevSysAmountHeader>();


        //    try
        //    {
        //        list = repo.GetRevSysAmountHeader(param);
        //        return list;
        //    }
        //    catch (Exception ex)
        //    {
        //        list.Add(new vmRevSysAmountHeader((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "RevenueSystemService", "GetRevSysAmountHeader", userID)));
        //        return list;
        //    }
        //    finally
        //    {
        //        context.Dispose();
        //    }
        //}

        //public List<vmRevSysDataDetail> GetRevSysDataDetail(string userID, vmRevSysParameters param)
        //{
        //    var context = new DbContext(Helper.GetConnection("ARSystem"));
        //    var repo = new RevSysRepository(context);
        //    List<vmRevSysDataDetail> list = new List<vmRevSysDataDetail>();

        //    try
        //    {
        //        list = repo.GetRevSysDataDetail(param);
        //        return list;
        //    }
        //    catch (Exception ex)
        //    {
        //        list.Add(new vmRevSysDataDetail((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "RevenueSystemService", "GetRevSysDataDetail", userID)));
        //        return list;
        //    }
        //    finally
        //    {
        //        context.Dispose();
        //    }
        //}

        //public List<vmRevSysDataDetail> GetRevSysDetailInvoice(string userID, vmRevSysParameters param)
        //{
        //    var context = new DbContext(Helper.GetConnection("ARSystem"));
        //    var repo = new RevSysRepository(context);

        //    List<vmRevSysDataDetail> listInvoice = new List<vmRevSysDataDetail>();


        //    try
        //    {
        //        listInvoice = repo.GetRevSysDetailInvoice(param);
        //        return listInvoice;
        //    }
        //    catch (Exception ex)
        //    {
        //        listInvoice.Add(new vmRevSysDataDetail((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "RevenueSystemService", "GetRevSysDetailInvoice", userID)));
        //        return listInvoice;
        //    }
        //    finally
        //    {
        //        context.Dispose();
        //    }
        //}

        //public List<vmRevSysDataDetail> GetRevSysDetailDescInv(string userID, vmRevSysParameters param)
        //{
        //    var context = new DbContext(Helper.GetConnection("ARSystem"));
        //    var repo = new RevSysRepository(context);

        //    List<vmRevSysDataDetail> listInvoice = new List<vmRevSysDataDetail>();


        //    try
        //    {
        //        listInvoice = repo.GetRevSysDetailDescInv(param);
        //        return listInvoice;
        //    }
        //    catch (Exception ex)
        //    {
        //        listInvoice.Add(new vmRevSysDataDetail((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "RevenueSystemService", "GetRevSysDetailDescInv", userID)));
        //        return listInvoice;
        //    }
        //    finally
        //    {
        //        context.Dispose();
        //    }
        //}

        //public List<vmRevSysDataHeader> GetRevSysSearchDataHeader(string userID, string whereclouse, vmRevSysParameters param)
        //{
        //    var context = new DbContext(Helper.GetConnection("ARSystem"));
        //    var repo = new RevSysRepository(context);
        //    List<vmRevSysDataHeader> list = new List<vmRevSysDataHeader>();



        //    try
        //    {
        //        list = repo.GetRevSysSearchDataHeader(param, whereclouse);
        //        return list;
        //    }
        //    catch (Exception ex)
        //    {
        //        list.Add(new vmRevSysDataHeader((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "RevenueSystemService", "GetRevSysDataHeader", userID)));
        //        return list;
        //    }
        //    finally
        //    {
        //        context.Dispose();
        //    }
        //}


        //public List<vmRevSysDataHeader> GetRevSysDataHeader(string userID, vmRevSysParameters param)
        //{
        //    var context = new DbContext(Helper.GetConnection("ARSystem"));
        //    var repo = new RevSysRepository(context);
        //    List<vmRevSysDataHeader> list = new List<vmRevSysDataHeader>();


        //    try
        //    {
        //        list = repo.GetRevSysDataHeader(param);
        //        return list;
        //    }
        //    catch (Exception ex)
        //    {
        //        list.Add(new vmRevSysDataHeader((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "RevenueSystemService", "GetRevSysDataHeader", userID)));
        //        return list;
        //    }
        //    finally
        //    {
        //        context.Dispose();
        //    }
        //}

        //public int GetRevSysDataHeaderCount(string userID, vmRevSysParameters param)
        //{
        //    var context = new DbContext(Helper.GetConnection("ARSystem"));
        //    var repo = new RevSysRepository(context);


        //    try
        //    {
        //        return repo.GetRevSysCountDataHeader(param);
        //    }
        //    catch (Exception ex)
        //    {
        //        new vmRevSysDataHeader((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "RevenueSystemService", "GetRevSysDataHeaderCount", userID));
        //        return 0;
        //    }
        //    finally
        //    {
        //        context.Dispose();
        //    }
        //}

        //public int GetRevSysSearchDataHeaderCount(string userID, vmRevSysParameters param, string whereClouse)
        //{
        //    var context = new DbContext(Helper.GetConnection("ARSystem"));
        //    var repo = new RevSysRepository(context);


        //    try
        //    {
        //        return repo.GetRevSysSearchCountDataHeader(param, whereClouse);
        //    }
        //    catch (Exception ex)
        //    {
        //        new vmRevSysDataHeader((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "RevenueSystemService", "GetRevSysSearchCountDataHeader", userID));
        //        return 0;
        //    }
        //    finally
        //    {
        //        context.Dispose();
        //    }
        //}

        //#endregion

        //#region RevSys Hold-Stop Accrue
        //public List<vmRevSysDataHoldStopAccrue> GetRevSysDataHoldStopAccrue(string userID, vmRevSysParamMasterListAccrue param)
        //{
        //    var context = new DbContext(Helper.GetConnection("ARSystem"));
        //    var repo = new RevSysRepository(context);
        //    List<vmRevSysDataHoldStopAccrue> list = new List<vmRevSysDataHoldStopAccrue>();


        //    try
        //    {
        //        list = repo.GetRevSysLoadHoldStopAccrue(param);

        //        return list;
        //    }
        //    catch (Exception ex)
        //    {
        //        list.Add(new vmRevSysDataHoldStopAccrue((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "RevenueSystemService", "GetRevSysLoadHoldStopAccrue", userID)));
        //        return list;
        //    }
        //    finally
        //    {
        //        context.Dispose();
        //    }
        //}

        //public int GetRevSysCountHoldStopAccrue(string userID, vmRevSysParamMasterListAccrue param)
        //{
        //    var context = new DbContext(Helper.GetConnection("ARSystem"));
        //    var repo = new RevSysRepository(context);


        //    try
        //    {
        //        return repo.GetRevSysCountHoldStopAccrue(param);
        //    }
        //    catch (Exception ex)
        //    {
        //        new vmRevSysDataHeader((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "RevenueSystemService", "GetRevSysCountHoldStopAccrue", userID));
        //        return 0;
        //    }
        //    finally
        //    {
        //        context.Dispose();
        //    }
        //}

        //public bool SaveRevSysHoldStopAccrue(string userID, vmRevSysParamMasterListAccrue param)
        //{
        //    var context = new DbContext(Helper.GetConnection("ARSystem"));
        //    var repo = new RevSysRepository(context);


        //    try
        //    {
        //        return repo.SaveRevSysHoldStopAccrue(param);
        //    }
        //    catch (Exception ex)
        //    {
        //        new vmRevSysDataHeader((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "RevenueSystemService", "SaveRevSysHoldStopAccrue", userID));
        //        return false;
        //    }
        //    finally
        //    {
        //        context.Dispose();
        //    }
        //}
        //#endregion

        //#region RevSys Accrue Per Dept

        //public List<vmRevSysAccruePerDept> InsertRevSysAccruePerDept(string userID, List<vmRevSysAccruePerDept> ListdtUpload)
        //{
        //    List<vmRevSysAccruePerDept> list = new List<vmRevSysAccruePerDept>();
        //    var context = new DbContext(Helper.GetConnection("ARSystem"));
        //    var repo = new RevSysRepository(context);

        //    repo.DeleteRevSysAccruePerDept(userID, "DeleteTempAccruePerDept");

        //    try
        //    {
        //        foreach (var dtUpload in ListdtUpload)
        //        {
        //            repo.InsertRevSysAccruePerDept(dtUpload, "InsertAccruePerDept", userID);
        //        }

        //        return list;
        //    }
        //    catch (Exception ex)
        //    {
        //        list.Add(new vmRevSysAccruePerDept((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "RevenueSystemService", "InsertRevSysAccruePerDept", userID)));
        //        return list;
        //    }
        //    finally
        //    {
        //        context.Dispose();
        //    }
        //}

        //public List<vmRevSysAccruePerDept> GetRevSysDataLoadAccruePerDept(string userID, vmRevSysParamMasterListAccrue param)
        //{
        //    var context = new DbContext(Helper.GetConnection("ARSystem"));
        //    var repo = new RevSysRepository(context);
        //    List<vmRevSysAccruePerDept> list = new List<vmRevSysAccruePerDept>();

        //    param.audit_user = userID;

        //    try
        //    {
        //        list = repo.GetRevSysLoadAccruePerDept(param);

        //        return list;
        //    }
        //    catch (Exception ex)
        //    {
        //        list.Add(new vmRevSysAccruePerDept((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "RevenueSystemService", "GetRevSysLoadAccruePerDept", userID)));
        //        return list;
        //    }
        //    finally
        //    {
        //        context.Dispose();
        //    }
        //}

        //public int GetRevSysCountAccruePerDept(string userID, vmRevSysParamMasterListAccrue param)
        //{
        //    var context = new DbContext(Helper.GetConnection("ARSystem"));
        //    var repo = new RevSysRepository(context);


        //    try
        //    {
        //        param.audit_user = userID;
        //        return repo.GetRevSysCountDataAccruePerDept(param);
        //    }
        //    catch (Exception ex)
        //    {
        //        new vmRevSysDataHeader((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "RevenueSystemService", "GetRevSysCountDataAccruePerDept", userID));
        //        return 0;
        //    }
        //    finally
        //    {
        //        context.Dispose();
        //    }
        //}

        //public bool SaveRevSysUpload(string userID, vmRevSysParamMasterListAccrue param)
        //{
        //    var context = new DbContext(Helper.GetConnection("ARSystem"));
        //    var repo = new RevSysRepository(context);


        //    try
        //    {
        //        param.audit_user = userID;

        //        bool st;
        //        if (param.Category == "Accrue")
        //            st = repo.InsertRevSysUploadPerDeptData(param);
        //        else
        //            st = repo.InsertUploadOverAll(param);

        //        return st;
        //    }
        //    catch (Exception ex)
        //    {
        //        new vmRevSysDataHeader((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "RevenueSystemService", "SaveRevSysUpload", userID));
        //        return false;
        //    }
        //    finally
        //    {
        //        context.Dispose();
        //    }
        //}



        //public List<vmRevSysAccruePerDept> InsertTempOverAll(string userID, List<vmRevSysAccruePerDept> ListdtUpload)
        //{
        //    List<vmRevSysAccruePerDept> list = new List<vmRevSysAccruePerDept>();
        //    var context = new DbContext(Helper.GetConnection("ARSystem"));
        //    var repo = new RevSysRepository(context);

        //    repo.DeleteTempOverAll(userID, "DeleteTempOverAll", ListdtUpload[0].type.ToString().Trim());


        //    try
        //    {
        //        foreach (var dtUpload in ListdtUpload)
        //        {
        //            repo.InsertTempOverAll(dtUpload, userID, "InsertTempOverAll");
        //        }

        //        return list;
        //    }
        //    catch (Exception ex)
        //    {
        //        list.Add(new vmRevSysAccruePerDept((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "RevenueSystemService", "InsertTempOverAll", userID)));
        //        return list;
        //    }
        //    finally
        //    {
        //        context.Dispose();
        //    }
        //}

        //public List<vmRevSysAccruePerDept> ShowUploadTempOver(string userID, vmRevSysParamMasterListAccrue param)
        //{
        //    var context = new DbContext(Helper.GetConnection("ARSystem"));
        //    var repo = new RevSysRepository(context);
        //    List<vmRevSysAccruePerDept> list = new List<vmRevSysAccruePerDept>();

        //    param.audit_user = userID;


        //    try
        //    {
        //        param.audit_user = userID;
        //        list = repo.ShowUploadTempOverAll(param);

        //        return list;
        //    }
        //    catch (Exception ex)
        //    {
        //        list.Add(new vmRevSysAccruePerDept((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "RevenueSystemService", "ShowUploadTempOverAll", userID)));
        //        return list;
        //    }
        //    finally
        //    {
        //        context.Dispose();
        //    }
        //}


        //public List<vmRevSysAccruePerPIC> ShowAccruePerDept(string userID, vmRevSysParamMasterListAccrue param)
        //{
        //    var context = new DbContext(Helper.GetConnection("ARSystem"));
        //    var repo = new RevSysRepository(context);
        //    List<vmRevSysAccruePerPIC> list = new List<vmRevSysAccruePerPIC>();
        //    param.audit_user = userID;


        //    try
        //    {
        //        param.audit_user = userID;
        //        list = repo.ShowAccruePerDept(param);

        //        return list;
        //    }
        //    catch (Exception ex)
        //    {
        //        list.Add(new vmRevSysAccruePerPIC((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "RevenueSystemService", "ShowAccruePerDept", userID)));
        //        return list;
        //    }
        //    finally
        //    {
        //        context.Dispose();
        //    }
        //}

        //public int GetRevSysCountTempOver(string userID, vmRevSysParamMasterListAccrue param)
        //{
        //    var context = new DbContext(Helper.GetConnection("ARSystem"));
        //    var repo = new RevSysRepository(context);

        //    try
        //    {
        //        param.audit_user = userID;
        //        return repo.GetRevSysCountTempOverAll(param);
        //    }
        //    catch (Exception ex)
        //    {
        //        new vmRevSysDataHeader((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "RevenueSystemService", "GetRevSysCountTempOverAll", userID));
        //        return 0;
        //    }
        //    finally
        //    {
        //        context.Dispose();
        //    }
        //}


        //#endregion

        #region Revenue Per Sonumb
        public List<vwARRevSysPerSonumb> GetvwARRevSysPerSonumbList(string strAccount, string strPeriode, string strPeriodeTo, string strCompany, string strRegional, string strOperator, string strProduct, string schSoNumber, string strOrderBy, int intRowSkip = 0, int intPageSize = 0)
        {
            var context = new DbContext(Helper.GetConnection("ARSystemDWH"));
            var repo = new vwARRevSysPerSonumbRepository(context);
            List<vwARRevSysPerSonumb> list = new List<vwARRevSysPerSonumb>();

            try
            {
                string strWhereClause = " 1=1 ";
                if (!string.IsNullOrWhiteSpace(strAccount))
                {
                    strWhereClause += " AND Account = '" + strAccount.Trim() + "'";
                }
                if (!string.IsNullOrWhiteSpace(strPeriode))
                {
                    strWhereClause += " AND Periode >= '" + strPeriode.Trim() + "'";
                }
                if (!string.IsNullOrWhiteSpace(strPeriodeTo) &&
                    Convert.ToInt32((strPeriodeTo != null) ? strPeriodeTo : "0") >= Convert.ToInt32((strPeriode != null) ? strPeriode : "0"))
                {
                    strWhereClause += " AND Periode <= '" + strPeriodeTo.Trim() + "'";
                }
                else
                {
                    //default kalau ga ada lebih kecil dari start year
                    strWhereClause += " AND Periode <= '" + strPeriode.Trim() + "'";
                }

                if (!string.IsNullOrWhiteSpace(strCompany))
                {
                    strWhereClause += " AND CompanyID = '" + strCompany + "'";
                }
                if (!string.IsNullOrWhiteSpace(strRegional))
                {
                    strWhereClause += " AND RegionName = '" + strRegional + "'";
                }
                if (!string.IsNullOrWhiteSpace(strOperator))
                {
                    strWhereClause += " AND CustomerID = '" + strOperator + "'";
                }
                if (!string.IsNullOrWhiteSpace(strProduct))
                {
                    strWhereClause += " AND ProductName = '" + strProduct + "'";
                }
                if (!string.IsNullOrWhiteSpace(schSoNumber))
                {
                    strWhereClause += " AND SoNumber LIKE '%" + schSoNumber + "%'";
                }
                if (intPageSize > 0)
                    list = repo.GetPaged(strWhereClause, strOrderBy, intRowSkip, intPageSize);
                else
                    list = repo.GetList(strWhereClause, strOrderBy);

                return list;
            }
            catch (Exception ex)
            {
                list.Add(new vwARRevSysPerSonumb((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "RevenueSystemService", "GetvwARRevSysPerSonumbList", "")));
                return list;
            }
            finally
            {
                context.Dispose();
            }
        }

        public int GetvwARRevSysPerSonumbCount(string strAccount, string strPeriode, string strPeriodeTo, string strCompany, string strRegional, string strOperator, string strProduct, string schSoNumber)
        {
            var context = new DbContext(Helper.GetConnection("ARSystemDWH"));
            var TrxDataPostedRepo = new vwARRevSysPerSonumbRepository(context);

            try
            {
                string strWhereClause = " 1=1 ";
                if (!string.IsNullOrWhiteSpace(strAccount))
                {
                    strWhereClause += " AND Account = '" + strAccount.Trim() + "'";
                }
                if (!string.IsNullOrWhiteSpace(strPeriode))
                {
                    strWhereClause += " AND Periode >= '" + strPeriode.Trim() + "'";
                }
                if (!string.IsNullOrWhiteSpace(strPeriodeTo) &&
                    Convert.ToInt32((strPeriodeTo != null) ? strPeriodeTo : "0") >= Convert.ToInt32((strPeriode != null) ? strPeriode : "0"))
                {
                    strWhereClause += " AND Periode <= '" + strPeriodeTo.Trim() + "'";
                }else
                {
                    //default kalau ga ada lebih kecil dari start year
                    strWhereClause += " AND Periode <= '" + strPeriode.Trim() + "'";
                }
                if (!string.IsNullOrWhiteSpace(strCompany))
                {
                    strWhereClause += " AND CompanyID = '" + strCompany + "'";
                }
                if (!string.IsNullOrWhiteSpace(strRegional))
                {
                    strWhereClause += " AND RegionName = '" + strRegional + "'";
                }
                if (!string.IsNullOrWhiteSpace(strOperator))
                {
                    strWhereClause += " AND CustomerID = '" + strOperator + "'";
                }
                if (!string.IsNullOrWhiteSpace(strProduct))
                {
                    strWhereClause += " AND ProductName = '" + strProduct + "'";
                }
                if (!string.IsNullOrWhiteSpace(schSoNumber))
                {
                    strWhereClause += " AND SoNumber LIKE %'" + schSoNumber + "'%";
                }
                return TrxDataPostedRepo.GetCount(strWhereClause);
            }
            catch (Exception ex)
            {
                Helper.logError(ex.Message.ToString(), "RevenueSystemService", "GetvwARRevSysPerSonumbCount", "");
                return 0;
            }
            finally
            {
                context.Dispose();
            }
        }
        #endregion

        #region Adjustment
        public List<vwARRevSysAdjustment> GetvwARRevSysAdjustmentList(string strSoNumber, string strMonthPeriod, int YearPeriod, string strOrderBy, int intRowSkip = 0, int intPageSize = 0)
        {
            var context = new DbContext(Helper.GetConnection("ARSystemDWH"));
            var repo = new vwARRevSysAdjustmentRepository(context);
            List<vwARRevSysAdjustment> list = new List<vwARRevSysAdjustment>();

            try
            {
                string strWhereClause = "";
                if (!string.IsNullOrWhiteSpace(strSoNumber))
                {
                    strWhereClause += "SoNumber = '" + strSoNumber.Trim() + "' AND ";
                }
                if (!string.IsNullOrWhiteSpace(strMonthPeriod))
                {
                    strWhereClause += "MonthPeriod = '" + strMonthPeriod.Trim() + "' AND ";
                }
                if (YearPeriod != 0)
                {
                    strWhereClause += "YearPeriod = '" + YearPeriod + "' ";
                }

                if (intPageSize > 0)
                    list = repo.GetPaged(strWhereClause, strOrderBy, intRowSkip, intPageSize);
                else
                    list = repo.GetList(strWhereClause, strOrderBy);

                return list;
            }
            catch (Exception ex)
            {
                list.Add(new vwARRevSysAdjustment((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "RevenueSystemService", "GetvwARRevSysAdjustmentList", "")));
                return list;
            }
            finally
            {
                context.Dispose();
            }
        }

        public trxARRevSysAdjustment SaveAdjustment(int id, decimal amount, string userId)
        {
            var contextDwh = new DbContext(Helper.GetConnection("ARSystemDWH"));
            var repo = new trxARRevSysAdjustmentRepository(contextDwh);
            try
            {
                var result = repo.GetByPK(id);
                result.Amount = amount;
                repo.Update(result);
                return result;
            }
            catch (Exception ex)
            {
                return new trxARRevSysAdjustment((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "RevenueSystemService", "SaveAdjustment", userId));
            }
            finally
            {
                contextDwh.Dispose();
            }
        }

        public int GetvwARRevSysAdjustmentCount(string strSoNumber)
        {
            var contextDwh = new DbContext(Helper.GetConnection("ARSystemDWH"));
            var TrxDataPostedRepo = new vwARRevSysAdjustmentRepository(contextDwh);

            try
            {
                string strWhereClause = "";
                if (!string.IsNullOrWhiteSpace(strSoNumber))
                {
                    strWhereClause += "SoNumber = '" + strSoNumber.Trim() + "'";
                }

                return TrxDataPostedRepo.GetCount(strWhereClause);
            }
            catch (Exception ex)
            {
                Helper.logError(ex.Message.ToString(), "RevenueSystemService", "GetvwARRevSysAdjustmentCount", "");
                return 0;
            }
            finally
            {
                contextDwh.Dispose();
            }
        }
        #endregion

        #region revenue Summary
        public List<vmRevenueSummaryAmount> GetSummaryAmount(vmRevenueSummaryParameters param, string userId)
        {
            var context = new DbContext(Helper.GetConnection("ARSystemDWH"));
            var repo = new RevenueSummaryRepository(context);
            var summList = new List<vmRevenueSummaryAmount>();

            try
            {
                return repo.GetRevSummaryOf<vmRevenueSummaryAmount>(param);
            }
            catch (Exception ex)
            {
                summList.Add(new vmRevenueSummaryAmount((int)Helper.ErrorType.Error, Helper.logError(ex.Message, "RevenueSummaryService", "GetSummaryAmount", userId)));
                return summList;
            }
        }

        public List<vmRevenueSummaryUnit> GetSummaryUnit(vmRevenueSummaryParameters param, string userId)
        {
            var context = new DbContext(Helper.GetConnection("ARSystemDWH"));
            var repo = new RevenueSummaryRepository(context);
            var summList = new List<vmRevenueSummaryUnit>();

            try
            {
                return repo.GetRevSummaryOf<vmRevenueSummaryUnit>(param);
            }
            catch (Exception ex)
            {
                summList.Add(new vmRevenueSummaryUnit((int)Helper.ErrorType.Error, Helper.logError(ex.Message, "RevenueSummaryService", "GetSummaryUnit", userId)));
                return summList;
            }
        }

        public List<vmRevenueSoNumberList> GetSoNumberList(vmRevenueSummaryParameters param, string userId)
        {
            var context = new DbContext(Helper.GetConnection("ARSystemDWH"));
            var repo = new RevenueSummaryRepository(context);
            var summList = new List<vmRevenueSoNumberList>();

            try
            {
                return repo.GetRevSummarySoNumberList(param);
            }
            catch (Exception ex)
            {
                summList.Add(new vmRevenueSoNumberList((int)Helper.ErrorType.Error, Helper.logError(ex.Message, "RevenueSummaryService", "GetSoNumberList", userId)));
                return summList;
            }
        }

        public List<trxARRevSysListDetail> GetSoNumberDetail(string soNumber, int? stipSiro, string userId)
        {
            var context = new DbContext(Helper.GetConnection("ARSystemDWH"));
            var repo = new trxARRevSysListDetailRepository(context);
            var summList = new List<trxARRevSysListDetail>();

            try
            {
                return repo.GetList(soNumber);
            }
            catch (Exception ex)
            {
                summList.Add(new trxARRevSysListDetail((int)Helper.ErrorType.Error, Helper.logError(ex.Message, "RevenueSummaryService", "GetSoNumberDetail", userId)));
                return summList;
            }
        }
  #endregion

        #region RevenueReport Description

        public List<dwhARRevSysDescription> GetRevenueReportDescriptionList(dwhARRevSysDescription post, int rowSkip, int pageSize)
        {
            var contextDwh = new DbContext(Helper.GetConnection("ARSystemDWH"));
            var repo = new dwhARRevSysDescriptionRepository(contextDwh);
            List<dwhARRevSysDescription> list = new List<dwhARRevSysDescription>();

            try
            {

                list = repo.GetPaged(GetRevenueReportDescriptionWhereClause(post), "", rowSkip, pageSize);
                return list;
            }
            catch (Exception ex)
            {
                list.Add(new dwhARRevSysDescription((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "RevenueSystemService", "GetReveReportDescriptionList", "")));
                return list;
            }
            finally
            {
                contextDwh.Dispose();
            }
        }

        public List<dwhARRevSysDescription> GetRevenueReportDescriptionList(dwhARRevSysDescription post)
        {
            var contextDwh = new DbContext(Helper.GetConnection("ARSystemDWH"));
            var repo = new dwhARRevSysDescriptionRepository(contextDwh);
            List<dwhARRevSysDescription> list = new List<dwhARRevSysDescription>();

            try
            {

                list = repo.GetList(GetRevenueReportDescriptionWhereClause(post));
                return list;
            }
            catch (Exception ex)
            {
                list.Add(new dwhARRevSysDescription((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "RevenueSystemService", "GetReveReportDescriptionList", "")));
                return list;
            }
            finally
            {
                contextDwh.Dispose();
            }
        }

        public int GetRevenueReportDescriptionCount(dwhARRevSysDescription post)
        {
            var contextDwh = new DbContext(Helper.GetConnection("ARSystemDWH"));
            var repo = new dwhARRevSysDescriptionRepository(contextDwh);
            try
            {
                return repo.GetCount(GetRevenueReportDescriptionWhereClause(post));
            }
            catch (Exception ex)
            {
                Helper.logError(ex.Message.ToString(), "RevenueSystemService", "GetReveReportDescriptionCount", "");
                return 0;
            }
            finally
            {
                contextDwh.Dispose();
            }
        }

        private string GetRevenueReportDescriptionWhereClause(dwhARRevSysDescription post)
        {
            string strWhereClause = "";
            if (!string.IsNullOrEmpty(post.CustomerId))
                strWhereClause += "CustomerId = '" + post.CustomerId + "' AND ";

            if (!string.IsNullOrEmpty(post.CompanyId))
                strWhereClause += "CompanyId = '" + post.CompanyId + "' AND ";

            if (!string.IsNullOrEmpty(post.DepartmentName))
                strWhereClause += "DepartmentName = '" + post.DepartmentName + "' AND ";

            if (post.RegionId != null)
                strWhereClause += "RegionId = " + post.RegionId + " AND ";

            if (!string.IsNullOrEmpty(post.RegionName))
                strWhereClause += "RegionName = '" + post.RegionName + "' AND ";

            if (!string.IsNullOrEmpty(post.SoNumber))
                strWhereClause += "SoNumber like '%" + post.SoNumber + "%' AND ";


            if (!string.IsNullOrEmpty(post.SiteName))
                strWhereClause += "SiteName like '%" + post.SiteName + "%' AND ";


            if (!string.IsNullOrEmpty(post.SiteId))
                strWhereClause += "SiteId like '%" + post.SiteId + "%' AND ";

            if (post.StartSLDDate != null && post.EndSLDDate != null)
                strWhereClause += "SLDDate between '" + post.StartSLDDate + "' AND '" + post.EndSLDDate + "' AND ";
            else if (post.StartSLDDate != null && post.EndSLDDate == null)
                strWhereClause += "SLDDate between '" + post.StartSLDDate + "' AND '" + post.StartSLDDate + "' AND ";
            else if (post.StartSLDDate == null && post.EndSLDDate != null)
                strWhereClause += "SLDDate between '" + post.EndSLDDate + "' AND '" + post.EndSLDDate + "' AND ";


            if (post.RfiStartDate != null && post.RfiEndDate != null)
                strWhereClause += "RfiDate between '" + post.RfiStartDate + "' AND '" + post.RfiEndDate + "' AND ";
            else if (post.RfiStartDate != null && post.RfiEndDate == null)
                strWhereClause += "RfiDate between '" + post.RfiStartDate + "' AND '" + post.RfiStartDate + "' AND ";
            else if (post.RfiStartDate == null && post.RfiEndDate != null)
                strWhereClause += "RfiDate between '" + post.RfiStartDate + "' AND '" + post.RfiEndDate + "' AND ";



            strWhereClause = !string.IsNullOrWhiteSpace(strWhereClause) ? strWhereClause.Substring(0, strWhereClause.Length - 5) : "";

            return strWhereClause;
        }

        #endregion

        #region revenue Movement
        public List<vmRevenueMovementAmount> GetSummaryMovementAmount(vmRevenueSummaryParameters param, string userId)
        {
            var context = new DbContext(Helper.GetConnection("ARSystemDWH"));
            var repo = new RevenueMovementRepository(context);
            var summList = new List<vmRevenueMovementAmount>();

            try
            {
                return repo.GetRevSummaryOf<vmRevenueMovementAmount>(param);
            }
            catch (Exception ex)
            {
                summList.Add(new vmRevenueMovementAmount((int)Helper.ErrorType.Error, Helper.logError(ex.Message, "RevenueSummaryService", "GetSummaryMovementAmount", userId)));
                return summList;
            }
        }

        public List<vmRevenueMovementUnit> GetSummaryMovementUnit(vmRevenueSummaryParameters param, string userId)
        {
            var context = new DbContext(Helper.GetConnection("ARSystemDWH"));
            var repo = new RevenueMovementRepository(context);
            var summList = new List<vmRevenueMovementUnit>();

            try
            {
                return repo.GetRevSummaryOf<vmRevenueMovementUnit>(param);
            }
            catch (Exception ex)
            {
                summList.Add(new vmRevenueMovementUnit((int)Helper.ErrorType.Error, Helper.logError(ex.Message, "RevenueSummaryService", "GetSummaryMovementUnit", userId)));
                return summList;
            }
        }

        public List<vmRevenueDetailMovement> GetDetailMovementByAmount(vmRevenueSummaryParameters param, string userId)
        {
            var context = new DbContext(Helper.GetConnection("ARSystemDWH"));
            var repo = new RevenueMovementRepository(context);
            var summList = new List<vmRevenueDetailMovement>();

            try
            {
                return repo.GetRevenueDetailMovementByAmount(param);
            }
            catch (Exception ex)
            {
                summList.Add(new vmRevenueDetailMovement((int)Helper.ErrorType.Error, Helper.logError(ex.Message, "RevenueSummaryService", "GetSoNumberList", userId)));
                return summList;
            }
        }

        public List<vmRevenueDetailMovementUnit> GetDetailMovementByUnit(vmRevenueSummaryParameters param, string userId)
        {
            var context = new DbContext(Helper.GetConnection("ARSystemDWH"));
            var repo = new RevenueMovementRepository(context);
            var summList = new List<vmRevenueDetailMovementUnit>();

            try
            {
                return repo.GetRevenueDetailMovementByUnit(param);
            }
            catch (Exception ex)
            {
                summList.Add(new vmRevenueDetailMovementUnit((int)Helper.ErrorType.Error, Helper.logError(ex.Message, "RevenueSummaryService", "GetSoNumberList", userId)));
                return summList;
            }
        }

        public int GetCountSummaryMovementAmount(vmRevenueSummaryParameters param)
        {
            var context = new DbContext(Helper.GetConnection("ARSystemDWH"));
            var repo = new RevenueMovementRepository(context);
            var summList = new List<vmRevenueMovementAmount>();
            try
            {

                return repo.GetCountRevSummaryOf(param);
            }
            catch (Exception)
            {
                context.Dispose();
                return 0;
            }
        }

        public int GetCountSummaryMovementUnit(vmRevenueSummaryParameters param)
        {
            var context = new DbContext(Helper.GetConnection("ARSystemDWH"));
            var repo = new RevenueMovementRepository(context);
            var summList = new List<vmRevenueMovementAmount>();
            try
            {

                return repo.GetCountRevSummaryOf(param);
            }
            catch (Exception)
            {
                context.Dispose();
                return 0;
            }
        }
        #endregion

        #region Dashboard revenue
        public List<dwhARRevenueSysSummary> GetListArRevenueSummary(dwhARRevenueSysSummary param, int rowSkip, int pageSize, string strOrderBy = "")
        {
            var list = new List<dwhARRevenueSysSummary>();
            var context = new DbContext(Helper.GetConnection("ARSystemDWH"));
            var repo = new dwhARRevenueSysSummaryRepository(context);
            try
            {
                string strWhereClause = " 1=1 ";
                if (param.fDataYear > 0)
                {
                    strWhereClause += " AND DataYear = '" + param.fDataYear + "'";
                }
                if (!string.IsNullOrWhiteSpace(param.RegionName))
                {
                    strWhereClause += " AND RegionName = '" + param.RegionName + "'";
                }
                if (!string.IsNullOrWhiteSpace(param.CompanyID))
                {
                    strWhereClause += " AND CompanyID = '" + param.CompanyID + "'";
                }
                if (!string.IsNullOrWhiteSpace(param.CustomerID))
                {
                    strWhereClause += " AND CustomerID = '" + param.CustomerID + "'";
                }
                if (!string.IsNullOrWhiteSpace(param.SONumber))
                {
                    strWhereClause += " AND SONumber LIKE '%"  + param.SONumber + "%'";
                }
                if (!string.IsNullOrWhiteSpace(param.SiteName))
                {
                    strWhereClause += " AND SiteName LIKE '%" + param.SiteName + "%'";
                }
                if (pageSize > 0)
                    list = repo.GetPaged(strWhereClause, strOrderBy, rowSkip, pageSize);
                else
                    list = repo.GetList(strWhereClause, strOrderBy);
            }
            catch (Exception ex)
            {
                context.Dispose();
                list.Add(new dwhARRevenueSysSummary { ErrorMessage = ex.Message });
            }
            return list;
        }
        public int GetCountArRevenueSummary(dwhARRevenueSysSummary param)
        {
            var context = new DbContext(Helper.GetConnection("ARSystemDWH"));
            var repo = new dwhARRevenueSysSummaryRepository(context);
            dwhARRevenueSysSummary listDashboardRevenue = new dwhARRevenueSysSummary();
            try
            {
                string strWhereClause = " 1=1 ";
                if (param.fDataYear > 0)
                {
                    strWhereClause += " AND DataYear = '" + param.fDataYear + "'";
                }
                if (!string.IsNullOrWhiteSpace(param.RegionName))
                {
                    strWhereClause += " AND RegionName = '" + param.RegionName + "'";
                }
                if (!string.IsNullOrWhiteSpace(param.CompanyID))
                {
                    strWhereClause += " AND CompanyID = '" + param.CompanyID + "'";
                }
                if (!string.IsNullOrWhiteSpace(param.CustomerID))
                {
                    strWhereClause += " AND CustomerID = '" + param.CustomerID + "'";
                }
                if (!string.IsNullOrWhiteSpace(param.SONumber))
                {
                    strWhereClause += " AND SONumber LIKE '%" + param.SONumber + "%'";
                }
                if (!string.IsNullOrWhiteSpace(param.SiteName))
                {
                    strWhereClause += " AND SiteName LIKE '%" + param.SiteName + "%'";
                }
                return repo.GetCount(strWhereClause);
            }
            catch (Exception)
            {
                context.Dispose();
                return 0;
            }
        }

        #endregion
    }
}
