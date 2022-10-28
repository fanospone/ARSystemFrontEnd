using System;
using System.Collections.Generic;
using System.Linq;
using ARSystem.Domain.Models;
using ARSystem.Domain.DAL;
using ARSystem.Domain.Repositories;
using ARSystem.Service;
using System.Globalization;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using System.Threading;

namespace ARSystem.Service
{
    public class ListDataAccrueService
    {
       
        public List<List<trxDataAccrue>> list1 = new List<List<trxDataAccrue>>();
        public List<List<trxDataAccrue>> list2 = new List<List<trxDataAccrue>>();

        public List<List<trxDataAccrueLog>> listLog1 = new List<List<trxDataAccrueLog>>();
        public List<List<trxDataAccrueLog>> listLog2 = new List<List<trxDataAccrueLog>>();

        #region Get for Dropdown
        public List<mstCompany> GetCompanyList(string strToken)
        {
            var context = new DbContext(Helper.GetConnection("TBIGSys"));
            var companyRepo = new mstCompanyRepository(context);
            List<mstCompany> companyList = new List<mstCompany>();

            
            try
            {
                return companyRepo.GetList("IsActive = 1", "Company");
            }
            catch (Exception ex)
            {
                companyList.Add(new mstCompany((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "ListDataAccrueService", "GetCompanyList", "")));
                return companyList;
            }
            finally
            {
                context.Dispose();
            }
        }
        public List<mstCustomer> GetCustomerList(string strToken, bool? bolIsTelco)
        {
            var context = new DbContext(Helper.GetConnection("TBIGSys"));
            var customerRepo = new mstCustomerRepository(context);
            List<mstCustomer> customerList = new List<mstCustomer>();
            
            try
            {
                string strWhereClause = "1=1 AND IsActive = 1";
                if (bolIsTelco != null)
                {
                    int intIsTelco = ((bool)bolIsTelco) ? 1 : 0;
                    strWhereClause += " AND IsTelco = " + intIsTelco;

                }
                return customerRepo.GetList(strWhereClause, "CustomerName");
            }
            catch (Exception ex)
            {
                customerList.Add(new mstCustomer((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "ListDataAccrueService", "GetCustomerList", "")));
                return customerList;
            }
            finally
            {
                context.Dispose();
            }
        }
        public List<mstRegion> GetRegionList(string strToken, int? intRegionID, string strOrderBy)
        {
            var context = new DbContext(Helper.GetConnection("TBIGSys"));
            var regionRepo = new mstRegionRepository(context);
            List<mstRegion> regionList = new List<mstRegion>();
            
            try
            {
                string strWhereClause = "1=1";
                if (intRegionID != null && intRegionID > 0)
                {
                    strWhereClause += " AND RegionID = " + intRegionID;
                }

                return regionRepo.GetList(strWhereClause, strOrderBy);
            }
            catch (Exception ex)
            {
                regionList.Add(new mstRegion((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "ListDataAccrueService", "GetRegionList", "")));
                return regionList;
            }
            finally
            {
                context.Dispose();
            }
        }
        public List<vwAccrueMstSOW> GetDepartmentToList(string UserID)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var Repo = new vwAccrueMstSOWRepository(context);
            List<vwAccrueMstSOW> list = new List<vwAccrueMstSOW>();

            try
            {
                string strWhereClause = "";
                list = Repo.GetList(strWhereClause, "");

                return list;
            }
            catch (Exception ex)
            {
                list.Add(new vwAccrueMstSOW((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "ListDataAccrueService", "GetDepartmentToList", UserID)));
                return list;
            }
            finally
            {
                context.Dispose();
            }
        }

        public List<mstAccrueDepartment> GetDepartmentHCToList(string UserID)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var Repo = new mstAccrueDepartmentRepository(context);
            List<mstAccrueDepartment> list = new List<mstAccrueDepartment>();

            try
            {
                string strWhereClause = "";
                list = Repo.GetList(strWhereClause, "");

                return list;
            }
            catch (Exception ex)
            {
                list.Add(new mstAccrueDepartment((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "ListDataAccrueService", "GetDepartmentHCToList", UserID)));
                return list;
            }
            finally
            {
                context.Dispose();
            }
        }
        public List<mstAccrueStatus> GetStatusAccrueToList(string UserID)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var Repo = new mstAccrueStatusRepository(context);
            List<mstAccrueStatus> list = new List<mstAccrueStatus>();

            try
            {
                string strWhereClause = "ID >= 2 AND IsActive = 1";
                list = Repo.GetList(strWhereClause, "");

                return list;
            }
            catch (Exception ex)
            {
                list.Add(new mstAccrueStatus((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "ListDataAccrueService", "GetStatusAccrueToList", UserID)));
                return list;
            }
            finally
            {
                context.Dispose();
            }
        }
        public List<mstAccrueStatus> GetStatusListDataAccrueToList(string UserID)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var Repo = new mstAccrueStatusRepository(context);
            List<mstAccrueStatus> list = new List<mstAccrueStatus>();

            try
            {
                string strWhereClause = "IsActive = 1";
                list = Repo.GetList(strWhereClause, "");

                return list;
            }
            catch (Exception ex)
            {
                list.Add(new mstAccrueStatus((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "ListDataAccrueService", "GetStatusAccrueToList", UserID)));
                return list;
            }
            finally
            {
                context.Dispose();
            }
        }
        public List<mstAccrueMappingSOW> GetMappingSOWToList(string UserID, string typeSOW)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var Repo = new mstAccrueMappingSOWRepository(context);
            List<mstAccrueMappingSOW> list = new List<mstAccrueMappingSOW>();

            try
            {
                string strWhereClause = "Type = '" + typeSOW + "'";
                list = Repo.GetList(strWhereClause, "");

                return list;
            }
            catch (Exception ex)
            {
                list.Add(new mstAccrueMappingSOW((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "ListDataAccrueService", "GetMappingSOWToList", UserID)));
                return list;
            }
            finally
            {
                context.Dispose();
            }
        }
        public List<mstAccrueRootCause> GetRootCauseToList(string UserID)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var Repo = new mstAccrueRootCauseRepository(context);
            List<mstAccrueRootCause> list = new List<mstAccrueRootCause>();

            try
            {
                string strWhereClause = "IsActive = 1";
                list = Repo.GetList(strWhereClause, "");

                return list;
            }
            catch (Exception ex)
            {
                list.Add(new mstAccrueRootCause((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "ListDataAccrueService", "GetRootCauseToList", UserID)));
                return list;
            }
            finally
            {
                context.Dispose();
            }
        }

        public List<mstAccruePICA> GetPICAToList(string UserID, string RootCauseID)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var Repo = new mstAccruePICARepository(context);
            List<mstAccruePICA> list = new List<mstAccruePICA>();

            try
            {
                string strWhereClause = "AccrueRootCauseID = '" + RootCauseID + "' AND IsActive = 1";
                list = Repo.GetList(strWhereClause, "");

                return list;
            }
            catch (Exception ex)
            {
                list.Add(new mstAccruePICA((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "ListDataAccrueService", "GetPICAToList", UserID)));
                return list;
            }
            finally
            {
                context.Dispose();
            }
        }
        public List<mstAccruePICADetail> GetPICADetailToList(string UserID, string PicaID)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var Repo = new mstAccruePICADetailRepository(context);
            List<mstAccruePICADetail> list = new List<mstAccruePICADetail>();

            try
            {
                string strWhereClause = "AccruePICAID = '" + PicaID + "' AND IsActive = 1";
                list = Repo.GetList(strWhereClause, "");

                return list;
            }
            catch (Exception ex)
            {
                list.Add(new mstAccruePICADetail((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "ListDataAccrueService", "GetPICADetailToList", UserID)));
                return list;
            }
            finally
            {
                context.Dispose();
            }
        }
        public static int GetWeekOfMonth(DateTime date)
        {
            DateTime beginningOfMonth = new DateTime(date.Year, date.Month, 1);

            while (date.Date.AddDays(1).DayOfWeek != CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek)
                date = date.AddDays(1);

            return (int)Math.Truncate((double)date.Subtract(beginningOfMonth).TotalDays / 7f) + 1;
        }

        public List<vmWeek> GetWeekNumberOfMonthList(string param)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var RepoAmount = new vwtrxDataAccrueRepository(context);
            List<vmWeek> ListWeek = new List<vmWeek>();
            try
            {
                DateTime date = DateTime.Parse(param);                
                date = date.Date;                
                string weekGetDate = RepoAmount.GetAccrueWeekGetDate(date, "");
                int weeksInMonth = Convert.ToInt32(weekGetDate);
                for (int i = 0; i < weeksInMonth; i++)
                {
                    vmWeek var = new vmWeek();
                    var.ID = i + 1;
                    var.Week = "W " + (i + 1).ToString();
                    ListWeek.Add(var);
                }
                return ListWeek;
            }
            catch(Exception ex)
            {
                return ListWeek;
            }
            finally
            {
                context.Dispose();
            }
            
            
        }
        public List<vmWeek> GetWeekNumberOfMonthListGetDate()
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var RepoAmount = new vwtrxDataAccrueRepository(context);
            List<vmWeek> ListWeek = new List<vmWeek>();
            try
            {                
                string weekGetDate = RepoAmount.GetAccrueWeekGetDate(DateTime.Now, "");
                int weeksInMonth = Convert.ToInt32(weekGetDate);
                for (int i = 0; i < weeksInMonth; i++)
                {
                    vmWeek var = new vmWeek();
                    var.ID = i + 1;
                    var.Week = "W " + (i + 1).ToString();
                    ListWeek.Add(var);
                }
                return ListWeek;
            }
            catch (Exception ex)
            {
                return ListWeek;
            }
            finally
            {
                context.Dispose();
            }
        }
        #endregion
        public int GetDataAccrueToListCount(string UserID, vwAccrueList data)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var contextDWH = new DbContext(Helper.GetConnection("ARSystemDWH"));
            var repo = new dwhAccrueListDataRepository(contextDWH);
            var repotrxDataAccrue = new vwtrxDataAccrueTempRepository(context);

            List<dwhAccrueListData> list = new List<dwhAccrueListData>();
            List<vwtrxDataAccrueTemp> listTrxAccrue = new List<vwtrxDataAccrueTemp>();

            try
            {
                int returns = 0;
                string strWhereClause = "";
                if (!string.IsNullOrWhiteSpace(data.CustomerID))
                {
                    strWhereClause += "CustomerID = '" + data.CustomerID + "' AND ";
                }
                if (!string.IsNullOrWhiteSpace(data.SOW))
                {
                    strWhereClause += "SOW = '" + data.SOW + "' AND ";
                }
                

                if (!string.IsNullOrWhiteSpace(data.CompanyID))
                {
                    strWhereClause += "CompanyID = '" + data.CompanyID + "' AND ";
                }
                if (!string.IsNullOrWhiteSpace(data.SONumber))
                {
                    strWhereClause += "SONumber LIKE '%" + data.SONumber + "%' AND ";
                }
                if (!string.IsNullOrWhiteSpace(data.SiteName))
                {
                    strWhereClause += "SiteName LIKE '%" + data.SiteName + "%' AND ";
                }
                if (data.RegionID > 0 || data.RegionID != null)
                {
                    strWhereClause += "RegionID = " + data.RegionID + " AND ";
                }

                

                strWhereClause = !string.IsNullOrWhiteSpace(strWhereClause) ? strWhereClause.Substring(0, strWhereClause.Length - 5) : "";
                if (repotrxDataAccrue.GetCount("") == 0)
                    returns = repo.GetCount(strWhereClause);
                else
                {
                    listTrxAccrue = repotrxDataAccrue.GetList("");
                    list = repo.GetList(strWhereClause, "");

                    if (!string.IsNullOrWhiteSpace(data.StatusAccrue) && data.StatusAccrue != "Not Yet")
                    {
                        #region LINQ
                        var listNew = (from lst in list
                                       join listTrx in listTrxAccrue on lst.IDTemp equals listTrx.KeyDataAccrue
                                       select new
                                       {
                                           lst.IDTemp,
                                           listTrx.StatusAccrue
                                       }
                                       ).ToList();

                        #endregion
                        if (!string.IsNullOrWhiteSpace(data.StatusAccrue))
                        {
                            listNew = listNew.Where(x => x.StatusAccrue.ToString() == data.StatusAccrue).ToList();
                        }
                        returns = listNew.Count();
                    }
                    else
                    {
                        list.RemoveAll(c => listTrxAccrue.ToList().Exists(n => n.KeyDataAccrue == c.IDTemp));
                        returns = list.Count();
                    }
                }
                return returns;

            }
            catch (Exception ex)
            {
                Helper.logError(ex.Message.ToString(), "ListDataAccrueService", "GetDataAccrueToListCount", UserID);
                return 0;
            }
            finally
            {
                context.Dispose();
            }
        }
        public List<dwhAccrueListData> GetDataAccrueToList(string UserID, vwAccrueList data, string strOrderBy, int intRowStart = 0, int intPageSize = 0)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var contextDWH = new DbContext(Helper.GetConnection("ARSystemDWH"));
            var Repo = new dwhAccrueListDataRepository(contextDWH);
            var repotrxDataAccrue = new vwtrxDataAccrueTempRepository(context);
            var repoAccrueLogRootCause = new vmAccrueLogUserConfirmRepository(context);

            List<dwhAccrueListData> list = new List<dwhAccrueListData>();
            List<vwtrxDataAccrueTemp> listTrxAccrue = new List<vwtrxDataAccrueTemp>();
            List<vmAccrueLogUserConfirm> listAccrueLogRootCause = new List<vmAccrueLogUserConfirm>();

            try
            {
                string strWhereClause = "";
                if (!string.IsNullOrWhiteSpace(data.CustomerID))
                {
                    strWhereClause += "CustomerID = '" + data.CustomerID + "' AND ";
                }
                if (!string.IsNullOrWhiteSpace(data.SOW))
                {
                    strWhereClause += "SOW = '" + data.SOW + "' AND ";
                }
                //if (!string.IsNullOrWhiteSpace(data.StatusAccrue))
                //{
                //    strWhereClause += "StatusAccrue LIKE '%" + data.StatusAccrue + "%' AND ";
                //}
                //else
                //{
                //    strWhereClause += "StatusAccrue LIKE '%Not Yet%' AND ";
                //}
                if (!string.IsNullOrWhiteSpace(data.CompanyID))
                {
                    strWhereClause += "CompanyID = '" + data.CompanyID + "' AND ";
                }
                if (!string.IsNullOrWhiteSpace(data.SONumber))
                {
                    strWhereClause += "SONumber LIKE '%" + data.SONumber + "%' AND ";
                }
                if (!string.IsNullOrWhiteSpace(data.SiteName))
                {
                    strWhereClause += "SiteName LIKE '%" + data.SiteName + "%' AND ";
                }
                if (data.RegionID > 0 || data.RegionID != null)
                {
                    strWhereClause += "RegionID = " + data.RegionID + " AND ";
                }
                //if (data.EndDatePeriod != null)
                //{
                //    strWhereClause += "EndDatePeriod <= '" + data.EndDatePeriod + "' AND ";
                //}
                strWhereClause = !string.IsNullOrWhiteSpace(strWhereClause) ? strWhereClause.Substring(0, strWhereClause.Length - 5) : "";

                if (repotrxDataAccrue.GetCount("") == 0)
                {
                    if (intPageSize > 0)
                        list = Repo.GetPaged(strWhereClause, strOrderBy, intRowStart, intPageSize);
                    else
                        list = Repo.GetList(strWhereClause, strOrderBy);
                }
                else
                {
                    listTrxAccrue = repotrxDataAccrue.GetList(""); 
                    list = Repo.GetList(strWhereClause, strOrderBy);
                    

                    if (!string.IsNullOrWhiteSpace(data.StatusAccrue) && data.StatusAccrue != "Not Yet")
                    {
                        #region LINQ
                        var listNew = (from lst in list
                                       join listTrx in listTrxAccrue on lst.IDTemp equals listTrx.KeyDataAccrue
                                       select new
                                       {
                                           lst.IDTemp,
                                           lst.SONumber,
                                           lst.RegionID,
                                           lst.RegionName,
                                           lst.EndDatePeriod,
                                           lst.CompanyID,
                                           lst.Company,
                                           lst.SiteID,
                                           lst.CustomerID,
                                           lst.CustomerName,
                                           lst.SiteName,
                                           lst.CustomerSiteID,
                                           lst.CustomerSiteName,
                                           lst.STIPNumber,
                                           lst.PONumber,
                                           lst.BAUK_No,
                                           lst.BAPS,
                                           lst.InvoiceSONumber,
                                           lst.TypeSOW,
                                           listTrx.StatusAccrue,
                                           lst.BaseLeasePrice,
                                           lst.ServicePrice,
                                           lst.StartBapsDate,
                                           lst.EndBapsDate,
                                           lst.StartDateAccrue,
                                           lst.EndDateAccrue,
                                           lst.Currency,
                                           lst.StatusMasterList,
                                           lst.CompanyInvoiceID,
                                           lst.BAPSDate,
                                           lst.RFIDate,
                                           lst.SldDate,
                                           lst.Type,
                                           lst.SOW,
                                           lst.Accrue,
                                           lst.MioAccrue,
                                           lst.Month,
                                           lst.D,
                                           lst.OD,
                                           lst.ODCATEGORY,
                                           lst.Unearned,
                                           lst.TenantType,
                                           lst.AmountTotal
                                       }).ToList();
                        list = listNew.Select(t => new dwhAccrueListData
                        {
                            IDTemp = t.IDTemp,
                            SONumber = t.SONumber,
                            RegionID = t.RegionID,
                            RegionName = t.RegionName,
                            EndDatePeriod = t.EndDatePeriod,
                            CompanyID = t.CompanyID,
                            Company = t.Company,
                            SiteID = t.SiteID,
                            CustomerID = t.CustomerID,
                            CustomerName = t.CustomerName,
                            SiteName = t.SiteName,
                            CustomerSiteID = t.CustomerSiteID,
                            CustomerSiteName = t.CustomerSiteName,
                            STIPNumber = t.STIPNumber,
                            PONumber = t.PONumber,
                            BAUK_No = t.BAUK_No,
                            BAPS = t.BAPS,
                            InvoiceSONumber = t.InvoiceSONumber,
                            TypeSOW = t.TypeSOW,
                            StatusAccrue = t.StatusAccrue,
                            BaseLeasePrice = t.BaseLeasePrice,
                            ServicePrice = t.ServicePrice,
                            StartBapsDate = t.StartBapsDate,
                            EndBapsDate = t.EndBapsDate,
                            StartDateAccrue = t.StartDateAccrue,
                            EndDateAccrue = t.EndDateAccrue,
                            Currency = t.Currency,
                            StatusMasterList = t.StatusMasterList,
                            CompanyInvoiceID = t.CompanyInvoiceID,
                            BAPSDate = t.BAPSDate,
                            RFIDate = t.RFIDate,
                            SldDate = t.SldDate,
                            Type = t.Type,
                            SOW = t.SOW,
                            Accrue = t.Accrue,
                            MioAccrue = t.MioAccrue,
                            Month = t.Month,
                            D = t.D,
                            OD = t.OD,
                            ODCATEGORY = t.ODCATEGORY,
                            Unearned = t.Unearned,
                            TenantType = t.TenantType,
                            AmountTotal = t.AmountTotal
                        }).ToList();
                        #endregion
                        if (!string.IsNullOrWhiteSpace(data.StatusAccrue))
                        {
                            list = list.Where(x => x.StatusAccrue.ToString() == data.StatusAccrue).ToList();
                        }
                    }
                    else
                    {
                        list.RemoveAll(c => listTrxAccrue.ToList().Exists(n => n.KeyDataAccrue == c.IDTemp));
                        listAccrueLogRootCause = repoAccrueLogRootCause.GetList("", "");
                        if(listAccrueLogRootCause.Count > 0)
                        {
                            #region LINQ join Log

                            var newList = (from lst in list
                                                      join listLog in listAccrueLogRootCause on lst.SONumber equals listLog.SONumber into temp
                                                      from ed in temp.DefaultIfEmpty()
                                                      select new
                                                      {
                                                          lst.IDTemp,
                                                          lst.SONumber,
                                                          lst.RegionID,
                                                          lst.RegionName,
                                                          lst.EndDatePeriod,
                                                          lst.CompanyID,
                                                          lst.Company,
                                                          lst.SiteID,
                                                          lst.CustomerID,
                                                          lst.CustomerName,
                                                          lst.SiteName,
                                                          lst.CustomerSiteID,
                                                          lst.CustomerSiteName,
                                                          lst.STIPNumber,
                                                          lst.PONumber,
                                                          lst.BAUK_No,
                                                          lst.BAPS,
                                                          lst.InvoiceSONumber,
                                                          lst.TypeSOW,
                                                          lst.StatusAccrue,
                                                          lst.BaseLeasePrice,
                                                          lst.ServicePrice,
                                                          lst.StartBapsDate,
                                                          lst.EndBapsDate,
                                                          lst.StartDateAccrue,
                                                          lst.EndDateAccrue,
                                                          lst.Currency,
                                                          lst.StatusMasterList,
                                                          lst.CompanyInvoiceID,
                                                          lst.BAPSDate,
                                                          lst.RFIDate,
                                                          lst.SldDate,
                                                          lst.Type,
                                                          lst.SOW,
                                                          lst.Accrue,
                                                          lst.MioAccrue,
                                                          lst.Month,
                                                          lst.D,
                                                          lst.OD,
                                                          lst.ODCATEGORY,
                                                          lst.Unearned,
                                                          lst.TenantType,
                                                          lst.AmountTotal,
                                                          Remarks = ed == null ? "" : ed.Remarks,
                                                          Target = ed == null ? "" : ed.TargetUser,
                                                          RootCause = ed == null ? "" : ed.RootCause,
                                                          PicaDetail = ed == null ? "" : ed.PICADetail,
                                                          Pica = ed == null ? "" : ed.PICA,

                                                      }).ToList();
                            list = newList.Select(t => new dwhAccrueListData
                            {
                                IDTemp = t.IDTemp,
                                SONumber = t.SONumber,
                                RegionID = t.RegionID,
                                RegionName = t.RegionName,
                                EndDatePeriod = t.EndDatePeriod,
                                CompanyID = t.CompanyID,
                                Company = t.Company,
                                SiteID = t.SiteID,
                                CustomerID = t.CustomerID,
                                CustomerName = t.CustomerName,
                                SiteName = t.SiteName,
                                CustomerSiteID = t.CustomerSiteID,
                                CustomerSiteName = t.CustomerSiteName,
                                STIPNumber = t.STIPNumber,
                                PONumber = t.PONumber,
                                BAUK_No = t.BAUK_No,
                                BAPS = t.BAPS,
                                InvoiceSONumber = t.InvoiceSONumber,
                                TypeSOW = t.TypeSOW,
                                StatusAccrue = t.StatusAccrue,
                                BaseLeasePrice = t.BaseLeasePrice,
                                ServicePrice = t.ServicePrice,
                                StartBapsDate = t.StartBapsDate,
                                EndBapsDate = t.EndBapsDate,
                                StartDateAccrue = t.StartDateAccrue,
                                EndDateAccrue = t.EndDateAccrue,
                                Currency = t.Currency,
                                StatusMasterList = t.StatusMasterList,
                                CompanyInvoiceID = t.CompanyInvoiceID,
                                BAPSDate = t.BAPSDate,
                                RFIDate = t.RFIDate,
                                SldDate = t.SldDate,
                                Type = t.Type,
                                SOW = t.SOW,
                                Accrue = t.Accrue,
                                MioAccrue = t.MioAccrue,
                                Month = t.Month,
                                D = t.D,
                                OD = t.OD,
                                ODCATEGORY = t.ODCATEGORY,
                                Unearned = t.Unearned,
                                TenantType = t.TenantType,
                                AmountTotal = t.AmountTotal,
                                Remarks = t.Remarks,
                                TargetUser = t.Target,
                                RootCause = t.RootCause,
                                PICADetail = t.PicaDetail,
                                PICA = t.Pica
                            }).ToList();
                            #endregion
                        }

                    }
                    int page = 0;                   
                    if(intPageSize != 0 && list.Count > intPageSize)
                    {                        
                        page = (intRowStart / intPageSize);
                        IList<dwhAccrueListData> PageList = GetPage(list, page, intPageSize);
                        list = PageList as List<dwhAccrueListData>;
                    }
                    
                }




                return list;
            }
            catch (Exception ex)
            {
                list.Add(new dwhAccrueListData((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "ListDataAccrueService", "ListDataAccrueToList", UserID)));
                return list;
            }
            finally
            {
                context.Dispose();
                contextDWH.Dispose();
            }
        }

        public List<dwhAccrueListData> GetDataAccrueListId(string UserID, vwAccrueList data)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var contextDWH = new DbContext(Helper.GetConnection("ARSystemDWH"));
            var Repo = new dwhAccrueListDataRepository(contextDWH);
            List<string> ListId = new List<string>();
            List<dwhAccrueListData> list = new List<dwhAccrueListData>();
            var repotrxDataAccrue = new vwtrxDataAccrueRepository(context);
            List<vwtrxDataAccrue> listTrxAccrue = new List<vwtrxDataAccrue>();

            try
            {
                string strWhereClause = "";
                if (!string.IsNullOrWhiteSpace(data.CustomerID))
                {
                    strWhereClause += "CustomerID = '" + data.CustomerID + "' AND ";
                }
                if (!string.IsNullOrWhiteSpace(data.CompanyID))
                {
                    strWhereClause += "CompanyID = '" + data.CompanyID + "' AND ";
                }
                if (!string.IsNullOrWhiteSpace(data.SONumber))
                {
                    strWhereClause += "SONumber LIKE '%" + data.SONumber + "%' AND ";
                }
                if (!string.IsNullOrWhiteSpace(data.SiteName))
                {
                    strWhereClause += "SiteName LIKE '%" + data.SiteName + "%' AND ";
                }
                if (data.RegionID > 0 || data.RegionID != null)
                {
                    strWhereClause += "RegionID = " + data.RegionID + " AND ";
                }
                if (!string.IsNullOrWhiteSpace(data.SOW))
                {
                    strWhereClause += "SOW = '" + data.SOW + "' AND ";
                }
                //if (!string.IsNullOrWhiteSpace(data.StatusAccrue))
                //{
                //    strWhereClause += "StatusAccrue LIKE '%" + data.StatusAccrue + "%' AND ";
                //}
                //if (data.EndDatePeriod != null)
                //{
                //    strWhereClause += "EndDatePeriod <= '" + data.EndDatePeriod + "' AND ";
                //}

                strWhereClause = !string.IsNullOrWhiteSpace(strWhereClause) ? strWhereClause.Substring(0, strWhereClause.Length - 5) : "";

                if (repotrxDataAccrue.GetCount("") == 0)
                    list = Repo.GetList(strWhereClause, "");
                else
                {
                    listTrxAccrue = repotrxDataAccrue.GetList("");
                    list = Repo.GetList(strWhereClause, "");
                    if (!string.IsNullOrWhiteSpace(data.StatusAccrue) && data.StatusAccrue != "Not Yet")
                    {
                        #region LINQ
                        var listNew = (from lst in list
                                       join listTrx in listTrxAccrue on lst.IDTemp equals listTrx.KeyDataAccrue
                                       select new
                                       {
                                           lst.IDTemp
                                           
                                       }).ToList();
                        list = listNew.Select(t => new dwhAccrueListData
                        {
                            IDTemp = t.IDTemp
                        }).ToList();
                        #endregion
                    }
                    else
                    {
                        list.RemoveAll(c => listTrxAccrue.ToList().Exists(n => n.KeyDataAccrue == c.IDTemp));

                    }
                    
                }
                //ListId = list.Select(l => l.IDTemp).ToList();
                return list;
            }
            catch (Exception ex)
            {
                //return ListId;
                list.Add(new dwhAccrueListData((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "ListDataAccrueService", "GetDataAccrueListId", UserID)));
                return list;
            }
            finally
            {
                context.Dispose();
                contextDWH.Dispose();
            }

        }

        public int GetAddAccrueDataListCount(string UserID, List<string> id)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var repo = new dwhAccrueListDataRepository(context);

            try
            {
                string listID = string.Join("','", id);
                string strWhereClause = "IDTemp IN ('" + listID + "')";
                return repo.GetCount(strWhereClause);
            }
            catch (Exception ex)
            {
                Helper.logError(ex.Message.ToString(), "ListDataAccrueService", "GetAddAccrueDataListCount", UserID);
                return 0;
            }
            finally
            {
                context.Dispose();
            }
        }
        public List<dwhAccrueListData> GetAddAccrueDataList(string UserID, List<string> id, string strOrderBy, int intRowStart = 0, int intPageSize = 0)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var Repo = new dwhAccrueListDataRepository(context);
            List<dwhAccrueListData> List = new List<dwhAccrueListData>();
            try
            {
                string listID = string.Join("','", id);
                string strWhereClause = "IDTemp IN ('" + listID + "')";

                if (intPageSize > 0)
                    List = Repo.GetPaged(strWhereClause, strOrderBy, intRowStart, intPageSize);
                else
                    List = Repo.GetList(strWhereClause, strOrderBy);
                return List;
            }
            catch (Exception ex)
            {
                List.Add(new dwhAccrueListData((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "ListDataAccrueService", "GetAddAccrueDataList", UserID)));
                return List;
            }
            finally
            {
                context.Dispose();
            }
        }

        public trxDataAccrue SubmitDataAccrue(string UserID, List<string> id, string paramAllID, vwAccrueList data)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var contextDWH = new DbContext(Helper.GetConnection("ARSystemDWH"));
            var Repo = new trxDataAccrueRepository(context);
            
            var RepovwAccrueList = new dwhAccrueListDataRepository(contextDWH);
            //var RepoDataAccrue = new vwAccrueListRepository(contextDWH);
            var RepoLog = new trxDataAccrueLogRepository(context);
            var RepoAmount = new vwtrxDataAccrueRepository(context);
            var repotrxDataAccrue = new vwtrxDataAccrueTempRepository(context);
            var uow = context.CreateUnitOfWork();

            List<trxDataAccrue> listAccrue = new List<trxDataAccrue>();
            List<dwhAccrueListData> List = new List<dwhAccrueListData>();
            List<trxDataAccrueLog> ListLog = new List<trxDataAccrueLog>();
            List<string> paramID = new List<string>();
            List<vwtrxDataAccrueTemp> listTrxAccrue = new List<vwtrxDataAccrueTemp>();
            string idNew = string.Empty;
            //var data = new ConcurrentBag<trxDataAccrue>();
            //var dataParamID = new ConcurrentBag<string>();
            trxDataAccrue param = new trxDataAccrue();
            vwtrxDataAccrue acc = new vwtrxDataAccrue();
            trxDataAccrueLog paramLog = new trxDataAccrueLog();
            try
            {
                string strWhereClause = "";
                
                if (paramAllID != "")
                {
                    #region WhereClause
                    if (!string.IsNullOrWhiteSpace(data.CustomerID))
                    {
                        strWhereClause += "CustomerID = '" + data.CustomerID + "' AND ";
                    }
                   
                    if (!string.IsNullOrWhiteSpace(data.CompanyID))
                    {
                        strWhereClause += "CompanyID = '" + data.CompanyID + "' AND ";
                    }
                    if (!string.IsNullOrWhiteSpace(data.SONumber))
                    {
                        strWhereClause += "SONumber LIKE '%" + data.SONumber + "%' AND ";
                    }
                    if (!string.IsNullOrWhiteSpace(data.SiteName))
                    {
                        strWhereClause += "SiteName LIKE '%" + data.SiteName + "%' AND ";
                    }
                    if (data.RegionID > 0 || data.RegionID != null)
                    {
                        strWhereClause += "RegionID = " + data.RegionID + " AND ";
                    }
                    if (!string.IsNullOrWhiteSpace(data.SOW))
                    {
                        strWhereClause += "SOW = '" + data.SOW + "' AND ";
                    }
                    #endregion
                    strWhereClause = !string.IsNullOrWhiteSpace(strWhereClause) ? strWhereClause.Substring(0, strWhereClause.Length - 5) : "";
                    List = RepovwAccrueList.GetList(strWhereClause, "");

                    //strWhereClause = "StatusAccrue LIKE '%" + paramAllID + "%'";
                }
                else
                {
                    string listID = string.Join("','", id);
                    strWhereClause = "IDTemp IN ('" + listID + "')";
                    List = RepovwAccrueList.GetList(strWhereClause, "");
                }
               
                if (repotrxDataAccrue.GetCount("") != 0)
                {
                    listTrxAccrue = repotrxDataAccrue.GetList("");                    
                    List.RemoveAll(c => listTrxAccrue.ToList().Exists(n => n.KeyDataAccrue == c.IDTemp)); //filter not exist in Data Transaction
                }
                string weekGetDate = RepoAmount.GetAccrueWeekGetDate(DateTime.Now, "GetWeekNow");

               
                #region foreach Transaction
                foreach (dwhAccrueListData var in List)
                {
                    param = new trxDataAccrue();
                    acc = new vwtrxDataAccrue();

                    acc.CustomerID = var.CustomerID;
                    acc.StartDateAccrue = var.StartDateAccrue;
                    acc.EndDateAccrue = var.EndDateAccrue;
                    acc.BaseLeasePrice = var.BaseLeasePrice;
                    acc.ServicePrice = var.ServicePrice;

                    param.SONumber = var.SONumber;
                    param.RegionID = var.RegionID;
                    param.EndDatePeriod = var.EndDatePeriod;
                    param.SiteID = var.SiteID;
                    param.SiteName = var.SiteName;
                    param.SiteIDOpr = var.CustomerSiteID;
                    param.SiteNameOpr = var.CustomerSiteName;
                    param.CompanyID = var.CompanyID;
                    param.CustomerID = var.CustomerID;
                    param.AccrueStatusID = (int)ConstantAccrueStatusHelper.AccrueStatus.WaitingFinanceConfirmation;
                    param.BaseLeasePrice = var.BaseLeasePrice;
                    param.ServicePrice = var.ServicePrice;
                    //param.TotalAmount = RepoAmount.GetCalculateAmount(acc);
                    param.TotalAmount = var.AmountTotal; //replace di SP
                    param.Type = var.Type;
                    param.SOW = var.SOW;
                    param.StartDateBAPS = var.StartBapsDate;
                    param.EndDateBAPS = var.EndBapsDate;
                    param.StartDateAccrue = var.StartDateAccrue;
                    param.EndDateAccrue = var.EndDateAccrue;

                    param.CompanyInvID = var.CompanyInvoiceID;
                    param.StatusMasterList = var.StatusMasterList;
                    param.TenantType = var.TenantType;
                    param.RFIDate = var.RFIDate;
                    param.SldDate = var.SldDate;
                    param.BAPSDate = var.BAPSDate;
                    param.Month = var.Month;
                    param.D = var.D;
                    param.OD = var.OD;
                    param.ODCategory = var.ODCATEGORY;
                    param.MioAccrue = var.MioAccrue;

                    param.Currency = var.Currency;
                    param.Week = Convert.ToInt32(weekGetDate);
                    //param.Week = 1; // di replace di SP
                    param.CreatedBy = UserID;
                    param.CreatedDate = DateTime.Now;

                    idNew = var.SONumber + weekGetDate;
                    paramID.Add(idNew);
                    listAccrue.Add(param);


                }
                #endregion
                
                IList<List<trxDataAccrue>> ListNew = SplitList<trxDataAccrue>(listAccrue);
                List<List<trxDataAccrue>> ListNewConvert = ListNew as List<List<trxDataAccrue>>;
                list1 = DivideList1(ListNewConvert);
                list2 = DivideList2(ListNewConvert);
                Thread oThreadone = new Thread(myThread1);
                Thread oThreadtwo = new Thread(myThread2);

                oThreadone.Start();
                //oThreadone.Join();
                oThreadtwo.Start();


                #region Foreach Old for Log
                //foreach (List<trxDataAccrue> list in ListNew)
                //{
                //    Repo.CreateBulky(list);
                //}


                //string listIDInsert = string.Join("','", paramID);
                //string strWhereClauseOld = "SONumber + CONVERT(VARCHAR(2), Week)+ CONVERT(VARCHAR(3), MONTH(CreatedDate)) + CONVERT(VARCHAR(4), YEAR(CreatedDate)) IN ('" + listIDInsert + "')";
                //List<trxDataAccrue> ListOld = Repo.GetList(strWhereClauseOld, "");
                ////List<vwtrxDataAccrue> ListOld = RepoAmount.GetList("", "");
                ////ListOld.RemoveAll(c => paramID.ToList().Exists(n => n != c.KeyDataAccrue));
                //foreach (vwtrxDataAccrue var in ListOld)
                //{
                //    paramLog = new trxDataAccrueLog();
                //    paramLog.trxDataAccrueID = var.trxDataAccrueID;
                //    paramLog.SONumber = var.SONumber;
                //    paramLog.RegionID = var.RegionID;
                //    paramLog.EndDatePeriod = var.EndDatePeriod;
                //    paramLog.SiteID = var.SiteID;
                //    paramLog.SiteName = var.SiteName;
                //    paramLog.SiteIDOpr = var.SiteIDOpr;
                //    paramLog.SiteNameOpr = var.SiteNameOpr;
                //    paramLog.CompanyID = var.CompanyID;
                //    paramLog.CustomerID = var.CustomerID;
                //    paramLog.AccrueStatusID = (int)ConstantAccrueStatusHelper.AccrueStatus.WaitingFinanceConfirmation;
                //    paramLog.BaseLeasePrice = var.BaseLeasePrice;
                //    paramLog.ServicePrice = var.ServicePrice;
                //    paramLog.TotalAmount = var.TotalAmount;
                //    paramLog.Type = var.Type;
                //    paramLog.SOW = var.SOW;
                //    paramLog.StartDateBAPS = var.StartDateBAPS;
                //    paramLog.EndDateBAPS = var.EndDateBAPS;
                //    paramLog.StartDateAccrue = var.StartDateAccrue;
                //    paramLog.EndDateAccrue = var.EndDateAccrue;

                //    paramLog.CompanyInvID = var.CompanyInvID;
                //    paramLog.StatusMasterList = var.StatusMasterList;
                //    paramLog.TenantType = var.TenantType;
                //    paramLog.RFIDate = var.RFIDate;
                //    paramLog.SldDate = var.SldDate;
                //    paramLog.BAPSDate = var.BAPSDate;
                //    paramLog.Month = var.Month;
                //    paramLog.D = var.D;
                //    paramLog.OD = var.OD;
                //    paramLog.ODCategory = var.ODCategory;
                //    paramLog.MioAccrue = var.MioAccrue;

                //    paramLog.Currency = var.Currency;
                //    paramLog.Week = 1; //replace in SP
                //    paramLog.CreatedBy = UserID;
                //    paramLog.CreatedDate = DateTime.Now;
                //    ListLog.Add(paramLog);
                //}
                //#endregion
                //IList<List<trxDataAccrueLog>> ListLogNew = SplitListLog<trxDataAccrueLog>(ListLog);
                //List<List<trxDataAccrueLog>> ListLogNewConvert = ListLogNew as List<List<trxDataAccrueLog>>;
                //listLog1 = DivideListLog1(ListLogNewConvert);
                //listLog2 = DivideListLog2(ListLogNewConvert);
                //Thread oThreadoneLog = new Thread(myThreadLog1);
                //Thread oThreadtwoLog = new Thread(myThreadLog2);

                //oThreadoneLog.Start();
                //oThreadtwoLog.Start();

                //foreach (List<trxDataAccrueLog> list in ListLogNew)
                //{
                //    RepoLog.CreateBulky(list);
                //}


                #endregion
                uow.SaveChanges();
                return new trxDataAccrue();
            }
            catch (Exception ex)
            {
                uow.Dispose();
                return new trxDataAccrue((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "ListDataAccrueService", "SubmitDataAccrue", UserID));
            }
            finally
            {
                context.Dispose();
                contextDWH.Dispose();
            }
        }


        public static IList<List<trxDataAccrue>> SplitList<T>(List<trxDataAccrue> source)
        {
            return source
                .Select((x, i) => new { Index = i, Value = x })
                .GroupBy(x => x.Index / 300)
                .Select(x => x.Select(v => v.Value).ToList())
                .ToList();
        }
        public static IList<List<trxDataAccrueLog>> SplitListLog<T>(List<trxDataAccrueLog> source)
        {
            return source
                .Select((x, i) => new { Index = i, Value = x })
                .GroupBy(x => x.Index / 300)
                .Select(x => x.Select(v => v.Value).ToList())
                .ToList();
        }

        IList<dwhAccrueListData> GetPage(IList<dwhAccrueListData> list, int page, int pageSize)
        {
            return list.Skip(page * pageSize).Take(pageSize).ToList();
        }

        #region Thread
        private List<List<trxDataAccrue>> DivideList1(List<List<trxDataAccrue>> myList)
        {
            List<List<trxDataAccrue>> ListNew = new List<List<trxDataAccrue>>(); 
            for (int z = 0; z < (myList.Count)/2; z++)
            {
                ListNew.Add(myList[z]);
            }

            return ListNew;
        }

        private List<List<trxDataAccrue>> DivideList2(List<List<trxDataAccrue>> myList)
        {
            List<List<trxDataAccrue>> ListNew = new List<List<trxDataAccrue>>();
            for (int z = (myList.Count) / 2; z < myList.Count; z++)
            {
                ListNew.Add(myList[z]);
            }
            return ListNew;
        }

        private List<List<trxDataAccrueLog>> DivideListLog1(List<List<trxDataAccrueLog>> myList)
        {
            List<List<trxDataAccrueLog>> ListNew = new List<List<trxDataAccrueLog>>();
            for (int z = 0; z < (myList.Count) / 2; z++)
            {
                ListNew.Add(myList[z]);
            }

            return ListNew;
        }

        private List<List<trxDataAccrueLog>> DivideListLog2(List<List<trxDataAccrueLog>> myList)
        {
            List<List<trxDataAccrueLog>> ListNew = new List<List<trxDataAccrueLog>>();
            for (int z = (myList.Count) / 2; z < myList.Count; z++)
            {
                ListNew.Add(myList[z]);
            }
            return ListNew;
        }

        public void myThread1()
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var Repo = new trxDataAccrueRepository(context);            
            try
            {
                List<List<trxDataAccrue>> myList = list1;
                foreach (List<trxDataAccrue> list in myList)
                {
                    Repo.CreateBulky(list);
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
            finally
            {
                context.Dispose();
            }
        }
        private void myThread2()
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var Repo = new trxDataAccrueRepository(context);
            try
            {
                List<List<trxDataAccrue>> myList = list2;
                foreach (List<trxDataAccrue> list in myList)
                {
                    Repo.CreateBulky(list);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                context.Dispose();
            }
        }

        public void myThreadLog1()
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var Repo = new trxDataAccrueLogRepository(context);
            try
            {
                List<List<trxDataAccrueLog>> myList = listLog1;
                foreach (List<trxDataAccrueLog> list in myList)
                {
                    Repo.CreateBulky(list);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                context.Dispose();
            }
        }
        private void myThreadLog2()
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var Repo = new trxDataAccrueLogRepository(context);
            try
            {
                List<List<trxDataAccrueLog>> myList = listLog2;
                foreach (List<trxDataAccrueLog> list in myList)
                {
                    Repo.CreateBulky(list);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                context.Dispose();
            }
        }
        #endregion
    }
}
