using System;
using System.Collections.Generic;
using System.Linq;
using ARSystem.Domain.Models;
using ARSystem.Domain.DAL;
using ARSystem.Domain.Repositories;
using ARSystem.Service;
using System.Threading;

namespace ARSystem.Service
{
    public class UserConfirmationService
    {
        public List<List<trxDataAccrue>> list1 = new List<List<trxDataAccrue>>();
        public List<List<trxDataAccrue>> list2 = new List<List<trxDataAccrue>>();
        public List<List<trxDataAccrueLog>> listLog1 = new List<List<trxDataAccrueLog>>();
        public List<List<trxDataAccrueLog>> listLog2 = new List<List<trxDataAccrueLog>>();
        public int GetUserConfirmToListCount(string UserID, vwtrxDataAccrue data, string monthDate, string week, bool IsTaskTodo)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var contextHC = new DbContext(Helper.GetConnection("HumanCapital"));
            var repo = new vwtrxDataAccrueRepository(context);
            var repoUserSOW = new vwEmployeeRepository(contextHC);
            var RepoAmount = new vwtrxDataAccrueRepository(context);

            try
            {
                if (repoUserSOW.GetCount("UserID = '" + UserID.Trim() + "'") == 0)
                    return 0;
                string strWhereClause = "";


                string SOW = repoUserSOW.GetList("UserID = '" + UserID.Trim() + "'", "").FirstOrDefault().DepartmentName;
                if (!string.IsNullOrWhiteSpace(SOW))
                {
                    strWhereClause += "SOW = '" + SOW.Trim() + "' AND ";
                }
                if (IsTaskTodo == false)
                {
                    if (!string.IsNullOrWhiteSpace(monthDate))
                    {
                        strWhereClause += "YEAR(CreatedDate) = '" + int.Parse(DateTime.Parse(monthDate).ToString("yyyy")) + "' AND Month(CreatedDate) = '" + int.Parse(DateTime.Parse(monthDate).ToString("MM")) + "' AND ";
                    }
                    if (!string.IsNullOrWhiteSpace(week))
                    {
                        strWhereClause += "Week = '" + int.Parse(week) + "' AND ";
                    }
                }
                else
                {
                    string weekGetDate = RepoAmount.GetAccrueWeekGetDate(DateTime.Now, "flag");
                    strWhereClause += "Week = '" + int.Parse(weekGetDate) + "' AND ";
                    strWhereClause += "YEAR(CreatedDate) = '" + int.Parse(DateTime.Now.ToString("yyyy")) + "' AND Month(CreatedDate) = '" + int.Parse(DateTime.Now.ToString("MM")) + "' AND ";
                }
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
                if (!string.IsNullOrWhiteSpace(data.SOW))
                {
                    strWhereClause += "SOW = '" + data.SOW + "' AND ";
                }

                if (data.EndDatePeriod != null)
                {
                    strWhereClause += "EndDatePeriod <= '" + data.EndDatePeriod + "' AND ";
                }
                if (data.AccrueStatusID > 0 || data.AccrueStatusID != null)
                {
                    strWhereClause += "AccrueStatusID = '" + data.AccrueStatusID + "' AND ";
                }
                else
                    strWhereClause += "(AccrueStatusID = " + (int)ConstantAccrueStatusHelper.AccrueStatus.WaitingUserConfirmation + " OR AccrueStatusID = " + (int)ConstantAccrueStatusHelper.AccrueStatus.WaitingReConfirmConfirmation + ") AND ";

                strWhereClause = !string.IsNullOrWhiteSpace(strWhereClause) ? strWhereClause.Substring(0, strWhereClause.Length - 5) : "";

                return repo.GetCount(strWhereClause);
            }
            catch (Exception ex)
            {
                Helper.logError(ex.Message.ToString(), "UserConfirmationService", "GetUserConfirmToListCount", UserID);
                return 0;
            }
            finally
            {
                context.Dispose();
                contextHC.Dispose();
            }
        }


        public List<vwtrxDataAccrue> GetUserConfirmToList(string UserID, vwtrxDataAccrue data, string monthDate, string week, string strOrderBy, int intRowStart = 0, int intPageSize = 0)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var contextHC = new DbContext(Helper.GetConnection("HumanCapital"));
            var Repo = new vwtrxDataAccrueRepository(context);
            var repoUserSOW = new vwEmployeeRepository(contextHC);
            List<vwtrxDataAccrue> list = new List<vwtrxDataAccrue>();

            try
            {
                if (repoUserSOW.GetCount("UserID = '" + UserID.Trim() + "'") == 0)
                {
                    list.Add(new vwtrxDataAccrue((int)Helper.ErrorType.Info, UserID + " , You dont Have Authorize in this page"));
                    return list;
                }
                string strWhereClause = "";
                string SOW = repoUserSOW.GetList("UserID = '" + UserID.Trim() + "'", "").FirstOrDefault().DepartmentName;
                if (!string.IsNullOrWhiteSpace(SOW))
                {
                    strWhereClause += "SOW = '" + SOW.Trim() + "' AND ";
                }
                if (!string.IsNullOrWhiteSpace(monthDate))
                {
                    strWhereClause += "YEAR(CreatedDate) = '" + int.Parse(DateTime.Parse(monthDate).ToString("yyyy")) + "' AND Month(CreatedDate) = '" + int.Parse(DateTime.Parse(monthDate).ToString("MM")) + "' AND ";
                }
                if (!string.IsNullOrWhiteSpace(week))
                {
                    strWhereClause += "Week = '" + int.Parse(week) + "' AND ";
                }
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
                if (!string.IsNullOrWhiteSpace(data.SOW))
                {
                    strWhereClause += "SOW = '" + data.SOW + "' AND ";
                }

                if (data.EndDatePeriod != null)
                {
                    strWhereClause += "EndDatePeriod <= '" + data.EndDatePeriod + "' AND ";
                }
                if (data.AccrueStatusID > 0 || data.AccrueStatusID != null)
                {
                    strWhereClause += "AccrueStatusID = '" + data.AccrueStatusID + "' AND ";
                }
                else
                    strWhereClause += "(AccrueStatusID = " + (int)ConstantAccrueStatusHelper.AccrueStatus.WaitingUserConfirmation + " OR AccrueStatusID = " + (int)ConstantAccrueStatusHelper.AccrueStatus.WaitingReConfirmConfirmation + ") AND ";

                strWhereClause = !string.IsNullOrWhiteSpace(strWhereClause) ? strWhereClause.Substring(0, strWhereClause.Length - 5) : "";

                if (intPageSize > 0)
                    list = Repo.GetPaged(strWhereClause, strOrderBy, intRowStart, intPageSize);
                else
                    list = Repo.GetList(strWhereClause, strOrderBy);


                return list;
            }
            catch (Exception ex)
            {
                list.Add(new vwtrxDataAccrue((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "UserConfirmationService", "GetUserConfirmToList", UserID)));
                return list;
            }
            finally
            {
                context.Dispose();
                contextHC.Dispose();
            }
        }

        
        public string GetWeekNowSelected(string UserID)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var RepoAmount = new vwtrxDataAccrueRepository(context);
            string weekGetDate = "";
            try
            {
                weekGetDate = RepoAmount.GetAccrueWeekGetDate(DateTime.Now, "flag");
                return weekGetDate;
            }
            catch (Exception ex)
            {
                return "Error";
            }
            finally
            {
                context.Dispose();
            }
        }

        public List<string> GettrxDataAccrueListId(string UserID, vwtrxDataAccrue data, string monthDate, string week)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var contextHC = new DbContext(Helper.GetConnection("HumanCapital"));
            var Repo = new vwtrxDataAccrueRepository(context);
            var repoUserSOW = new vwEmployeeRepository(contextHC);
            List<vwtrxDataAccrue> list = new List<vwtrxDataAccrue>();
            List<string> ListId = new List<string>();


            try
            {
                if (repoUserSOW.GetCount("UserID = '" + UserID.Trim() + "'") == 0)
                {
                    list.Add(new vwtrxDataAccrue((int)Helper.ErrorType.Info, UserID + " , You dont Have Authorize in this page"));
                    return ListId;
                }
                string strWhereClause = "";
                string SOW = repoUserSOW.GetList("UserID = '" + UserID.Trim() + "'", "").FirstOrDefault().DepartmentName;
                if (!string.IsNullOrWhiteSpace(SOW))
                {
                    strWhereClause += "SOW = '" + SOW.Trim() + "' AND ";
                }
                if (!string.IsNullOrWhiteSpace(monthDate))
                {
                    strWhereClause += "YEAR(CreatedDate) = '" + int.Parse(DateTime.Parse(monthDate).ToString("yyyy")) + "' AND Month(CreatedDate) = '" + int.Parse(DateTime.Parse(monthDate).ToString("MM")) + "' AND ";
                }
                if (!string.IsNullOrWhiteSpace(week))
                {
                    strWhereClause += "Week = '" + int.Parse(week) + "' AND ";
                }
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
                if (!string.IsNullOrWhiteSpace(data.SOW))
                {
                    strWhereClause += "SOW = '" + data.SOW + "' AND ";
                }

                if (data.EndDatePeriod != null)
                {
                    strWhereClause += "EndDatePeriod <= '" + data.EndDatePeriod + "' AND ";
                }
                if (data.AccrueStatusID > 0 || data.AccrueStatusID != null)
                {
                    strWhereClause += "AccrueStatusID = '" + data.AccrueStatusID + "' AND ";
                }
                else
                    strWhereClause += "(AccrueStatusID = " + (int)ConstantAccrueStatusHelper.AccrueStatus.WaitingUserConfirmation + " OR AccrueStatusID = " + (int)ConstantAccrueStatusHelper.AccrueStatus.WaitingReConfirmConfirmation + ") AND ";

                strWhereClause = !string.IsNullOrWhiteSpace(strWhereClause) ? strWhereClause.Substring(0, strWhereClause.Length - 5) : "";

                list = Repo.GetList(strWhereClause, "");
                ListId = list.Select(l => l.trxDataAccrueID.ToString()).ToList();
                return ListId;
            }
            catch (Exception ex)
            {
                list.Add(new vwtrxDataAccrue((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "UserConfirmationService", "GettrxDataAccrueListId", UserID)));
                return ListId;
            }
            finally
            {
                context.Dispose();
                contextHC.Dispose();
            }

        }


        public trxDataAccrue ConfirmUser(string UserID, List<string> id, string remarks, trxDataAccrue data)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var Repo = new trxDataAccrueRepository(context);
            var RepoDataAccrue = new vwAccrueListRepository(context);
            var RepoLog = new trxDataAccrueLogRepository(context);
            var uow = context.CreateUnitOfWork();

            List<trxDataAccrue> listAccrue = new List<trxDataAccrue>();
            List<trxDataAccrue> listAccrueParam = new List<trxDataAccrue>();
            List<vwAccrueList> List = new List<vwAccrueList>();
            List<trxDataAccrueLog> ListLog = new List<trxDataAccrueLog>();
            List<string> paramID = new List<string>();

            try
            {
                string listID = string.Join("','", id);
                string strWhereClause = "ID IN ('" + listID + "')";
                listAccrue = Repo.GetList(strWhereClause, "");
                foreach (trxDataAccrue var in listAccrue)
                {
                    trxDataAccrue param = new trxDataAccrue();
                    param.ID = var.ID;
                    param.AccrueStatusID = (int)ConstantAccrueStatusHelper.AccrueStatus.WaitingFinalConfirmation;
                    param.Remarks = remarks;
                    param.WeekTargetUser = data.WeekTargetUser;
                    param.TargetDateUser = data.TargetDateUser;
                    param.RootCauseID = data.RootCauseID;
                    param.PicaID = data.PicaID;
                    param.PicaDetailID = data.PicaDetailID;
                    param.ContentTypeConfirm = data.ContentTypeConfirm;
                    param.FileNameConfirm = data.FileNameConfirm;
                    param.FilePathConfirm = data.FilePathConfirm;
                    param.UpdatedBy = UserID;
                    param.UpdatedDate = DateTime.Now;
                    listAccrueParam.Add(param);

                }
                IList<List<trxDataAccrue>> ListNew = SplitList<trxDataAccrue>(listAccrueParam);
                List<List<trxDataAccrue>> ListNewConvert = ListNew as List<List<trxDataAccrue>>;
                list1 = DivideList1(ListNewConvert);
                list2 = DivideList2(ListNewConvert);
                Thread oThreadone = new Thread(myThreadConfirm1);
                Thread oThreadtwo = new Thread(myThreadConfirm2);

                oThreadone.Start();
                oThreadtwo.Start();
                //IList<List<trxDataAccrue>> ListNew = SplitList<trxDataAccrue>(listAccrueParam);
                //foreach (List<trxDataAccrue> list in ListNew)
                //{
                //    Repo.ConfirmUserBulky(list);
                //}
                //Repo.ConfirmUserBulky(listAccrueParam);
                foreach (trxDataAccrue var in listAccrue)
                {
                    trxDataAccrueLog paramLog = new trxDataAccrueLog();
                    paramLog.trxDataAccrueID = var.ID;
                    paramLog.SONumber = var.SONumber;
                    paramLog.RegionID = var.RegionID;
                    paramLog.EndDatePeriod = var.EndDatePeriod;
                    paramLog.SiteID = var.SiteID;
                    paramLog.SiteName = var.SiteName;
                    paramLog.SiteIDOpr = var.SiteIDOpr;
                    paramLog.SiteNameOpr = var.SiteNameOpr;
                    paramLog.CompanyID = var.CompanyID;
                    paramLog.CustomerID = var.CustomerID;
                    paramLog.AccrueStatusID = (int)ConstantAccrueStatusHelper.AccrueStatus.WaitingFinalConfirmation;
                    paramLog.BaseLeasePrice = var.BaseLeasePrice;
                    paramLog.ServicePrice = var.ServicePrice;
                    paramLog.Currency = var.Currency;
                    paramLog.StartDateBAPS = var.StartDateBAPS;
                    paramLog.EndDateBAPS = var.EndDateBAPS;
                    paramLog.StartDateAccrue = var.StartDateAccrue;
                    paramLog.EndDateAccrue = var.EndDateAccrue;

                    paramLog.CompanyInvID = var.CompanyInvID;
                    paramLog.StatusMasterList = var.StatusMasterList;
                    paramLog.TenantType = var.TenantType;
                    paramLog.RFIDate = var.RFIDate;
                    paramLog.SldDate = var.SldDate;
                    paramLog.BAPSDate = var.BAPSDate;
                    paramLog.Month = var.Month;
                    paramLog.D = var.D;
                    paramLog.OD = var.OD;
                    paramLog.ODCategory = var.ODCategory;
                    paramLog.MioAccrue = var.MioAccrue;
                    paramLog.TotalAmount = var.TotalAmount;
                    paramLog.Type = var.Type;
                    paramLog.SOW = var.SOW;
                    paramLog.Remarks = remarks;
                    paramLog.Week = var.Week;
                    paramLog.WeekTargetUser = data.WeekTargetUser;
                    paramLog.TargetDateUser = data.TargetDateUser;
                    paramLog.RootCauseID = data.RootCauseID;
                    paramLog.PicaID = data.PicaID;
                    paramLog.PicaDetailID = data.PicaDetailID;
                    paramLog.ContentTypeMove = var.ContentTypeMove;
                    paramLog.FileNameMove = var.FileNameMove;
                    paramLog.FilePathMove = var.FilePathMove;
                    paramLog.ContentTypeConfirm = data.ContentTypeConfirm;
                    paramLog.FileNameConfirm = data.FileNameConfirm;
                    paramLog.FilePathConfirm = data.FilePathConfirm;
                    paramLog.CreatedBy = UserID;
                    paramLog.CreatedDate = DateTime.Now;
                    ListLog.Add(paramLog);
                }
                IList<List<trxDataAccrueLog>> ListLogNew = SplitListLog<trxDataAccrueLog>(ListLog);
                List<List<trxDataAccrueLog>> ListLogNewConvert = ListLogNew as List<List<trxDataAccrueLog>>;
                listLog1 = DivideListLog1(ListLogNewConvert);
                listLog2 = DivideListLog2(ListLogNewConvert);
                Thread oThreadoneLogNew = new Thread(myThreadLog1);
                Thread oThreadtwoLogNew = new Thread(myThreadLog2);

                oThreadoneLogNew.Start();
                oThreadtwoLogNew.Start();
                //IList<List<trxDataAccrueLog>> ListLogNew = SplitListLog<trxDataAccrueLog>(ListLog);
                //foreach (List<trxDataAccrueLog> list in ListLogNew)
                //{
                //    RepoLog.CreateBulky(list);
                //}
                uow.SaveChanges();
                return new trxDataAccrue();
            }
            catch (Exception ex)
            {
                uow.Dispose();
                return new trxDataAccrue((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "UserConfirmationService", "ConfirmUser", UserID));
            }
            finally
            {
                context.Dispose();
            }
        }

        public trxDataAccrue Move(string UserID, List<string> id, string department, trxDataAccrue data)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var Repo = new trxDataAccrueRepository(context);
            var RepoDataAccrue = new vwAccrueListRepository(context);
            var RepoLog = new trxDataAccrueLogRepository(context);
            var uow = context.CreateUnitOfWork();

            List<trxDataAccrue> listAccrue = new List<trxDataAccrue>();
            List<trxDataAccrue> listAccrueParam = new List<trxDataAccrue>();
            List<vwAccrueList> List = new List<vwAccrueList>();
            List<trxDataAccrueLog> ListLog = new List<trxDataAccrueLog>();
            List<string> paramID = new List<string>();

            try
            {
                string listID = string.Join("','", id);
                string strWhereClause = "ID IN ('" + listID + "')";
                listAccrue = Repo.GetList(strWhereClause, "");
                foreach (trxDataAccrue var in listAccrue)
                {
                    trxDataAccrue param = new trxDataAccrue();
                    param.ID = var.ID;

                    param.SOW = department;
                    param.Remarks = data.Remarks;
                    param.ContentTypeMove = data.ContentTypeMove;
                    param.FileNameMove = data.FileNameMove;
                    param.FilePathMove = data.FilePathMove;
                    param.UpdatedBy = UserID;
                    param.UpdatedDate = DateTime.Now;

                    listAccrueParam.Add(param);
                    //Repo.Update(param);

                }
                IList<List<trxDataAccrue>> ListNew = SplitList<trxDataAccrue>(listAccrueParam);
                List<List<trxDataAccrue>> ListNewConvert = ListNew as List<List<trxDataAccrue>>;
                list1 = DivideList1(ListNewConvert);
                list2 = DivideList2(ListNewConvert);
                Thread oThreadone = new Thread(myThreadMove1);
                Thread oThreadtwo = new Thread(myThreadMove2);

                oThreadone.Start();
                oThreadtwo.Start();
                //IList<List<trxDataAccrue>> ListNew = SplitList<trxDataAccrue>(listAccrueParam);
                //foreach (List<trxDataAccrue> list in ListNew)
                //{
                //    Repo.MoveUserBulky(list);
                //}
                #region log
                foreach (trxDataAccrue var in listAccrue)
                {
                    trxDataAccrueLog paramLog = new trxDataAccrueLog();
                    paramLog.trxDataAccrueID = var.ID;
                    paramLog.SONumber = var.SONumber;
                    paramLog.RegionID = var.RegionID;
                    paramLog.EndDatePeriod = var.EndDatePeriod;
                    paramLog.SiteID = var.SiteID;
                    paramLog.SiteName = var.SiteName;
                    paramLog.SiteIDOpr = var.SiteIDOpr;
                    paramLog.SiteNameOpr = var.SiteNameOpr;
                    paramLog.CompanyID = var.CompanyID;
                    paramLog.CustomerID = var.CustomerID;
                    paramLog.AccrueStatusID = (int)ConstantAccrueStatusHelper.AccrueStatus.WaitingUserConfirmation;
                    paramLog.BaseLeasePrice = var.BaseLeasePrice;
                    paramLog.ServicePrice = var.ServicePrice;
                    paramLog.Currency = var.Currency;
                    paramLog.StartDateBAPS = var.StartDateBAPS;
                    paramLog.EndDateBAPS = var.EndDateBAPS;
                    paramLog.StartDateAccrue = var.StartDateAccrue;
                    paramLog.EndDateAccrue = var.EndDateAccrue;

                    paramLog.CompanyInvID = var.CompanyInvID;
                    paramLog.StatusMasterList = var.StatusMasterList;
                    paramLog.TenantType = var.TenantType;
                    paramLog.RFIDate = var.RFIDate;
                    paramLog.SldDate = var.SldDate;
                    paramLog.BAPSDate = var.BAPSDate;
                    paramLog.Month = var.Month;
                    paramLog.D = var.D;
                    paramLog.OD = var.OD;
                    paramLog.ODCategory = var.ODCategory;
                    paramLog.MioAccrue = var.MioAccrue;

                    paramLog.TotalAmount = var.TotalAmount;
                    paramLog.Type = var.Type;
                    paramLog.SOW = department;
                    paramLog.SOWMoveFrom = var.SOW;
                    paramLog.Remarks = data.Remarks;
                    paramLog.Week = var.Week;
                    paramLog.WeekTargetUser = var.WeekTargetUser;
                    paramLog.TargetDateUser = var.TargetDateUser;
                    paramLog.RootCauseID = var.RootCauseID;
                    paramLog.PicaID = var.PicaID;
                    paramLog.PicaDetailID = var.PicaDetailID;
                    paramLog.ContentTypeMove = data.ContentTypeMove;
                    paramLog.FileNameMove = data.FileNameMove;
                    paramLog.FilePathMove = data.FilePathMove;
                    paramLog.ContentTypeConfirm = var.ContentTypeConfirm;
                    paramLog.FileNameConfirm = var.FileNameConfirm;
                    paramLog.FilePathConfirm = var.FilePathConfirm;
                    paramLog.CreatedBy = UserID;
                    paramLog.CreatedDate = DateTime.Now;
                    ListLog.Add(paramLog);
                }
                IList<List<trxDataAccrueLog>> ListLogNew = SplitListLog<trxDataAccrueLog>(ListLog);
                List<List<trxDataAccrueLog>> ListLogNewConvert = ListLogNew as List<List<trxDataAccrueLog>>;
                listLog1 = DivideListLog1(ListLogNewConvert);
                listLog2 = DivideListLog2(ListLogNewConvert);
                Thread oThreadoneLogNew = new Thread(myThreadLog1);
                Thread oThreadtwoLogNew = new Thread(myThreadLog2);

                oThreadoneLogNew.Start();
                oThreadtwoLogNew.Start();
                //IList<List<trxDataAccrueLog>> ListLogNew = SplitListLog<trxDataAccrueLog>(ListLog);
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
                return new trxDataAccrue((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "UserConfirmationService", "Move", UserID));
            }
            finally
            {
                context.Dispose();
            }
        }

        public static IList<List<trxDataAccrue>> SplitList<T>(List<trxDataAccrue> source)
        {
            return source
                .Select((x, i) => new { Index = i, Value = x })
                .GroupBy(x => x.Index / 700)
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
        #region Thread

        private List<List<trxDataAccrue>> DivideList1(List<List<trxDataAccrue>> myList)
        {
            List<List<trxDataAccrue>> ListNew = new List<List<trxDataAccrue>>();
            for (int z = 0; z < (myList.Count) / 2; z++)
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

        public void myThreadConfirm1()
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var Repo = new trxDataAccrueRepository(context);
            try
            {
                List<List<trxDataAccrue>> myList = list1;
                foreach (List<trxDataAccrue> list in myList)
                {
                    Repo.ConfirmUserBulky(list);
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
        private void myThreadConfirm2()
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var Repo = new trxDataAccrueRepository(context);
            try
            {
                List<List<trxDataAccrue>> myList = list2;
                foreach (List<trxDataAccrue> list in myList)
                {
                    Repo.ConfirmUserBulky(list);
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

        public void myThreadMove1()
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var Repo = new trxDataAccrueRepository(context);
            try
            {
                List<List<trxDataAccrue>> myList = list1;
                foreach (List<trxDataAccrue> list in myList)
                {
                    Repo.MoveUserBulky(list);
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
        private void myThreadMove2()
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var Repo = new trxDataAccrueRepository(context);
            try
            {
                List<List<trxDataAccrue>> myList = list2;
                foreach (List<trxDataAccrue> list in myList)
                {
                    Repo.MoveUserBulky(list);
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
