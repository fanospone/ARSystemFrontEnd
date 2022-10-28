using System;
using System.Collections.Generic;
using System.Linq;
using ARSystem.Domain.Models;
using ARSystem.Domain.DAL;
using ARSystem.Domain.Repositories;
using ARSystem.Service;
using System.Threading;
using System.Data;
using System.Configuration;

namespace ARSystem.Service
{
    public class FinanceConfirmationService
    {
        public List<List<trxDataAccrue>> list1 = new List<List<trxDataAccrue>>();
        public List<List<trxDataAccrue>> list2 = new List<List<trxDataAccrue>>();
        public List<List<trxDataAccrueLog>> listLog1 = new List<List<trxDataAccrueLog>>();
        public List<List<trxDataAccrueLog>> listLog2 = new List<List<trxDataAccrueLog>>();
        public static string linkARSystem = ConfigurationManager.AppSettings["LinkARSystem"].ToString();
        public int GetFinanceConfirmToListCount(string UserID, vwtrxDataAccrue data, string monthDate, string week)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var repo = new vwtrxDataAccrueRepository(context);


            try
            {
                string strWhereClause = "";
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
                    strWhereClause += "AccrueStatusID = '" + (int)ConstantAccrueStatusHelper.AccrueStatus.WaitingFinanceConfirmation + "' AND ";

                strWhereClause = !string.IsNullOrWhiteSpace(strWhereClause) ? strWhereClause.Substring(0, strWhereClause.Length - 5) : "";

                return repo.GetCount(strWhereClause);
            }
            catch (Exception ex)
            {
                Helper.logError(ex.Message.ToString(), "FinanceConfirmationService", "GetFinanceConfirmToListCount", UserID);
                return 0;
            }
            finally
            {
                context.Dispose();
            }
        }
        public List<vwtrxDataAccrue> GetFinanceConfirmToList(string UserID, vwtrxDataAccrue data, string monthDate, string week, string strOrderBy, int intRowStart = 0, int intPageSize = 0)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var Repo = new vwtrxDataAccrueRepository(context);
            List<vwtrxDataAccrue> list = new List<vwtrxDataAccrue>();

            try
            {

                string strWhereClause = "";
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
                    strWhereClause += "AccrueStatusID = '" + (int)ConstantAccrueStatusHelper.AccrueStatus.WaitingFinanceConfirmation + "' AND ";

                strWhereClause = !string.IsNullOrWhiteSpace(strWhereClause) ? strWhereClause.Substring(0, strWhereClause.Length - 5) : "";

                if (intPageSize > 0)
                    list = Repo.GetPaged(strWhereClause, strOrderBy, intRowStart, intPageSize);
                else
                    list = Repo.GetList(strWhereClause, strOrderBy);


                return list;
            }
            catch (Exception ex)
            {
                list.Add(new vwtrxDataAccrue((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "FinanceConfirmationService", "GetFinanceConfirmToList", UserID)));
                return list;
            }
            finally
            {
                context.Dispose();
            }
        }

        public List<string> GettrxDataAccrueListId(string UserID, vwtrxDataAccrue data, string monthDate, string week)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var Repo = new vwtrxDataAccrueRepository(context);
            List<vwtrxDataAccrue> List = new List<vwtrxDataAccrue>();
            List<string> ListId = new List<string>();


            try
            {
                string strWhereClause = "";
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
                    strWhereClause += "AccrueStatusID = '" + (int)ConstantAccrueStatusHelper.AccrueStatus.WaitingFinanceConfirmation + "' AND ";

                strWhereClause = !string.IsNullOrWhiteSpace(strWhereClause) ? strWhereClause.Substring(0, strWhereClause.Length - 5) : "";

                List = Repo.GetList(strWhereClause, "");
                ListId = List.Select(l => l.trxDataAccrueID.ToString()).ToList();
                return ListId;
            }
            catch (Exception ex)
            {
                List.Add(new vwtrxDataAccrue((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "FinanceConfirmationService", "GettrxDataAccrueListId", UserID)));
                return ListId;
            }
            finally
            {
                context.Dispose();
            }

        }

        public trxDataAccrue ValidateTypeMove(string UserID, List<string> id, string paramAllID, vwtrxDataAccrue data, string monthDate, string week)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var Repo = new trxDataAccrueRepository(context);
            var uow = context.CreateUnitOfWork();
            try
            {
                trxDataAccrue result = new trxDataAccrue();
                List<trxDataAccrue> listAccrue = new List<trxDataAccrue>();
                string strWhereClause = "";
                if (paramAllID != "")
                {
                    #region whereClause
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
                        strWhereClause += "AccrueStatusID = '" + (int)ConstantAccrueStatusHelper.AccrueStatus.WaitingFinanceConfirmation + "' AND ";

                    strWhereClause = !string.IsNullOrWhiteSpace(strWhereClause) ? strWhereClause.Substring(0, strWhereClause.Length - 5) : "";
                    #endregion
                }
                else
                {
                    string listID = string.Join("','", id);
                    strWhereClause = "ID IN ('" + listID + "')";
                }

                listAccrue = Repo.GetList(strWhereClause, "");
                string typeStart = listAccrue.Select(l => l.Type.ToString()).FirstOrDefault();
                foreach (trxDataAccrue var in listAccrue)
                {
                    if (typeStart != var.Type)
                    {

                        result.ErrorType = (int)Helper.ErrorType.Info;
                        break;
                    }

                }
                return result;
            }
            catch (Exception ex)
            {
                uow.Dispose();
                return new trxDataAccrue((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "FinanceConfirmationService", "ValidateTypeMove", UserID));
            }
            finally
            {
                context.Dispose();
            }
        }
        public trxDataAccrue ConfirmFinance(string UserID, List<string> id, string remarks, string paramAllID, vwtrxDataAccrue data, string monthDate, string week)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var contextHC = new DbContext(Helper.GetConnection("HumanCapital"));
            var Repo = new trxDataAccrueRepository(context);
            var RepoDataAccrue = new vwAccrueListRepository(context);
            var RepoLog = new trxDataAccrueLogRepository(context);

            var RepoAutoConfirm = new mstAccrueSettingAutoConfirmRepository(context);
            var RepoViewAccrue = new vwtrxDataAccrueRepository(context);
            var RepoUser = new vwidxAccrueUserSOWRepository(context);
            var repoSummary = new vwAccrueFinalConfirmRepository(context);
            var repoEmployee = new vwEmployeeRepository(contextHC);

            var uow = context.CreateUnitOfWork();

            List<trxDataAccrue> listAccrue = new List<trxDataAccrue>();
            List<trxDataAccrue> listAccrueParam = new List<trxDataAccrue>();
            List<vwAccrueList> List = new List<vwAccrueList>();
            List<trxDataAccrueLog> ListLog = new List<trxDataAccrueLog>();
            List<trxDataAccrueLog> ListLogOld = new List<trxDataAccrueLog>();
            List<string> paramID = new List<string>();

            try
            {
                string namaUserID = repoEmployee.GetList("UserID = '" + UserID + "'", "").FirstOrDefault().Name;
                string weekGetDate = RepoViewAccrue.GetAccrueWeekGetDate(DateTime.Now, "GetWeekNow");
                string strWhereClause = "";
                strWhereClause += "YEAR(Period) = '" + int.Parse(DateTime.Parse(monthDate).ToString("yyyy")) + "' AND Month(Period) = '" + int.Parse(DateTime.Parse(monthDate).ToString("MM")) + "' AND ";
                strWhereClause += "Week = '" + int.Parse(week) + "' AND ";
                strWhereClause += "AccrueStatusID = '" + (int)ConstantAccrueStatusHelper.AccrueStatus.WaitingUserConfirmation + "' AND IsActive = 1";
                if (RepoAutoConfirm.GetCount(strWhereClause) == 0)
                {
                    return new trxDataAccrue((int)Helper.ErrorType.Validation, "Please setting auto confirm first");
                }
                DateTime autoConfirmDate = RepoAutoConfirm.GetList(strWhereClause, "").FirstOrDefault().AutoConfirmDate;
                strWhereClause = "";
                if (paramAllID != "")
                {
                    #region whereClause
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
                        strWhereClause += "AccrueStatusID = '" + (int)ConstantAccrueStatusHelper.AccrueStatus.WaitingFinanceConfirmation + "' AND ";

                    strWhereClause = !string.IsNullOrWhiteSpace(strWhereClause) ? strWhereClause.Substring(0, strWhereClause.Length - 5) : "";
                    #endregion
                }
                else
                {
                    string listID = string.Join("','", id);
                    strWhereClause = "ID IN ('" + listID + "')";
                }


                listAccrue = Repo.GetList(strWhereClause, "");
                #region Send email

                strWhereClause = "";
                string Subject = "Accrue - Confirmation User";
                string IDParam = "ID IN ('" + string.Join("','", listAccrue.Select(x => x.ID).Distinct().ToList()) + "')";
                strWhereClause = "SOW IN ('" + string.Join("','", listAccrue.Select(x => x.SOW).Distinct().ToList()) + "')";
                List<vwidxAccrueUserSOW> listUser = RepoUser.GetList(strWhereClause, "");
                string emailTo = string.Join(";", listUser.Select(x => x.Email).ToList());
                emailTo += ";";
                string namaTo = string.Join(",", listUser.Select(x => x.Name).Distinct().ToList());
                #region Table
                DataTable dt = repoSummary.GetListFinalConfirmSummaryEmailFinanceDataTable(IDParam);
                string tableHTML = "<table border='1' style='border-collapse: collapse'><thead><tr><th rowspan='3' align='center'><b>Department</b></th><th colspan='4' align='center'><b>NEW</b></th><th colspan='4' align='center'><b>RENEWAL</b></th>";
                tableHTML += "<th colspan='2' rowspan='2' align='center'><b>TOTAL</b></th> <th rowspan='3' align='center'><b> % </b></th></tr>";
                tableHTML += "<tr><th colspan='2' align='center'><b>CURR</b></th><th colspan='2' align='center'><b>OD</b></th><th colspan='2' align='center'><b>CURR</b></th><th colspan='2' align='center'><b>OD</b></th></tr>";
                tableHTML += "<tr><th align='center'><b>No Of Site</b></th><th align='center'><b>Amount (Mio)</b></th><th align='center'><b>No Of Site</b></th><th align='center'><b>Amount (Mio)</b></th><th align='center'><b>No Of Site</b></th><th align='center'><b>Amount (Mio)</b></th><th align='center'><b>No Of Site</b></th><th align='center'><b>Amount (Mio)</b></th><th align='center'><b>No Of Site</b></th><th align='center'><b>Amount (Mio)</b></th></tr></thead>";
                tableHTML += "<tbody>";
                DataRow newrow = dt.NewRow();
                newrow["SOW"] = "Grand Total";
                dt.Rows.Add(newrow);
                int flagTotal = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    if (dr["SOW"].ToString() == "Grand Total")
                        flagTotal = 1;
                    tableHTML += "<tr>";
                    foreach (DataColumn column in dt.Columns)
                    {
                        column.ReadOnly = false;
                        if (column.ColumnName != "SOW")
                        {
                            if (flagTotal == 1)
                            {
                                dr[column.ColumnName] = dt.Compute("Sum([" + column.ColumnName + "])", string.Empty);
                            }

                        }
                        if (column.ColumnName == "SOW")
                            tableHTML += "<td>" + dr[column.ColumnName].ToString() + "</td>";
                        else
                        {
                            if (column.ColumnName.Contains("Percen"))
                                tableHTML += "<td align='right'>" + String.Format("{0:N2}", Convert.ToDouble(dr[column.ColumnName].ToString())) + "</td>";
                            else if (column.ColumnName.Contains("Amount"))
                                tableHTML += "<td align='right'>" + String.Format("{0:N2}", Convert.ToDouble(dr[column.ColumnName].ToString()) / 1000000) + "</td>";
                            else
                                tableHTML += "<td align='right'>" + " " + String.Format("{0}", Convert.ToInt32(dr[column.ColumnName].ToString())) + "</td>";
                        }
                            
                    }
                    tableHTML += "</tr>";
                }
                tableHTML += "</table><br/>";
                #endregion Table
                string strBody = "<b>Dear All,</b> <br/><br/> Berikut summary untuk REPORT ACCRUE REVENUE CLOSING ";
                if (weekGetDate != "5")
                    strBody += "WK " + weekGetDate + " " + DateTime.Now.ToString("MMM yyyy") + " - <b>VERSI ACCOUNTING, update per closing Weekly Finance " + "WK " + weekGetDate + " " + DateTime.Now.ToString("MMM yyyy") + ".</b>";
                else
                    strBody += DateTime.Now.ToString("MMM yyyy") + " - <b>VERSI ACCOUNTING, update per closing Monthly Finance " + DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month) + " " + DateTime.Now.ToString("MMM yyyy") + ".</b>";
                strBody += "<br/><br/> <b>ACCRUE PER DEPARTMENT</b><br/>" + tableHTML;

                strBody += "Silahkan dapat dikonfirmasi status progress di system maksimal hari " + autoConfirmDate.ToString("dddd", new System.Globalization.CultureInfo("id-ID")) + " Tanggal " + autoConfirmDate.ToString("dd") + " Bulan " + autoConfirmDate.ToString("MMMM") + " Tahun " + autoConfirmDate.ToString("yyyy") + " jam " + autoConfirmDate.ToString("HH:mm");
                strBody += " , apabila ada detail progress disubmit setelah jam " + autoConfirmDate.ToString("HH:mm") + ", <b> Kami anggap ''Confirmed'' atas progress site tersebut.</b>";
                //strBody += "<br/><br/>" + namaTo;
                strBody += "<br/><br/><a href='" + linkARSystem + "/Accrue/UserConfirmation'>Link Application</a>";
                strBody += "<br/><br/><br/> <b>Regards,</b><br/> " + namaUserID;


                #endregion
                ///////////////////////////////////////////////////////////////
                foreach (trxDataAccrue var in listAccrue)
                {
                    trxDataAccrue param = new trxDataAccrue();
                    param.ID = var.ID;
                    param.AccrueStatusID = (int)ConstantAccrueStatusHelper.AccrueStatus.WaitingUserConfirmation;
                    param.Remarks = remarks;
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

                #region Log            
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
                    paramLog.TotalAmount = var.TotalAmount;
                    paramLog.Type = var.Type;
                    paramLog.SOW = var.SOW;
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
                    paramLog.Remarks = remarks;
                    paramLog.Week = var.Week;
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
                //IList<List<trxDataAccrueLog>> ListLogNew = SplitList<trxDataAccrueLog>(ListLog);
                //foreach (List<trxDataAccrueLog> list in ListLogNew)
                //{
                //    RepoLog.CreateBulky(list);
                //}
                #endregion
                ////////////////////////////////////////////////////////////////
                EmailHelper.SendEmail(emailTo, "", "", Subject, strBody, "", UserID);
                uow.SaveChanges();
                return new trxDataAccrue();
            }
            catch (Exception ex)
            {
                uow.Dispose();
                return new trxDataAccrue((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "FinanceConfirmationService", "ConfirmFinance", UserID));
            }
            finally
            {
                context.Dispose();
                contextHC.Dispose();
            }
        }
        public trxDataAccrue Move(string UserID, List<string> id, string department,string Type, string paramAllID, vwtrxDataAccrue data, string monthDate, string week)
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
            List<trxDataAccrueLog> ListLogOld = new List<trxDataAccrueLog>();
            List<string> paramID = new List<string>();

            try
            {
                string strWhereClause = "";
                if (paramAllID != "")
                {
                    #region whereClause
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
                        strWhereClause += "AccrueStatusID = '" + (int)ConstantAccrueStatusHelper.AccrueStatus.WaitingFinanceConfirmation + "' AND ";

                    strWhereClause = !string.IsNullOrWhiteSpace(strWhereClause) ? strWhereClause.Substring(0, strWhereClause.Length - 5) : "";
                    #endregion
                }
                else
                {
                    string listID = string.Join("','", id);
                    strWhereClause = "ID IN ('" + listID + "')";
                }

                listAccrue = Repo.GetList(strWhereClause, "");
                #region Insert to Log Data Old
                //if (RepoLog.GetCount("trxDataAccrueID IN ('" + listID + "')") == 0)
                //{
                //    foreach (trxDataAccrue var in listAccrue)
                //    {

                //        if (RepoLog.GetCount("trxDataAccrueID = '" + var.ID + "'") == 0)
                //        {
                //            trxDataAccrueLog paramLog = new trxDataAccrueLog();
                //            paramLog.trxDataAccrueID = var.ID;
                //            paramLog.SONumber = var.SONumber;
                //            paramLog.RegionID = var.RegionID;
                //            paramLog.EndDatePeriod = var.EndDatePeriod;
                //            paramLog.SiteID = var.SiteID;
                //            paramLog.SiteName = var.SiteName;
                //            paramLog.SiteIDOpr = var.SiteIDOpr;
                //            paramLog.SiteNameOpr = var.SiteNameOpr;
                //            paramLog.CompanyID = var.CompanyID;
                //            paramLog.CustomerID = var.CustomerID;
                //            paramLog.AccrueStatusID = var.AccrueStatusID;
                //            paramLog.BaseLeasePrice = var.BaseLeasePrice;
                //            paramLog.ServicePrice = var.ServicePrice;
                //            paramLog.Currency = var.Currency;
                //            paramLog.StartDateBAPS = var.StartDateBAPS;
                //            paramLog.EndDateBAPS = var.EndDateBAPS;
                //            paramLog.StartDateAccrue = var.StartDateAccrue;
                //            paramLog.EndDateAccrue = var.EndDateAccrue;
                //            paramLog.TotalAmount = var.TotalAmount;
                //            paramLog.Type = var.Type;
                //            paramLog.SOW = var.SOW;
                //            paramLog.CompanyInvID = var.CompanyInvID;
                //            paramLog.StatusMasterList = var.StatusMasterList;
                //            paramLog.TenantType = var.TenantType;
                //            paramLog.RFIDate = var.RFIDate;
                //            paramLog.SldDate = var.SldDate;
                //            paramLog.BAPSDate = var.BAPSDate;
                //            paramLog.Month = var.Month;
                //            paramLog.D = var.D;
                //            paramLog.OD = var.OD;
                //            paramLog.ODCategory = var.ODCategory;
                //            paramLog.MioAccrue = var.MioAccrue;
                //            paramLog.Remarks = var.Remarks;
                //            paramLog.Week = var.Week;
                //            paramLog.CreatedBy = var.CreatedBy;
                //            paramLog.CreatedDate = var.CreatedDate;
                //            ListLogOld.Add(paramLog);
                //        }

                //    }
                //    IList<List<trxDataAccrueLog>> ListLogOldNew = SplitListLog<trxDataAccrueLog>(ListLogOld);
                //    List<List<trxDataAccrueLog>> ListLogOldNewConvert = ListLogOldNew as List<List<trxDataAccrueLog>>;
                //    listLog1 = DivideListLog1(ListLogOldNewConvert);
                //    listLog2 = DivideListLog2(ListLogOldNewConvert);
                //    Thread oThreadoneLog = new Thread(myThreadLog1);
                //    Thread oThreadtwoLog = new Thread(myThreadLog2);

                //    oThreadoneLog.Start();
                //    oThreadoneLog.Join();
                //    oThreadtwoLog.Start();
                //    //IList<List<trxDataAccrueLog>> ListLogOldNew = SplitList<trxDataAccrueLog>(ListLogOld);
                //    //foreach (List<trxDataAccrueLog> list in ListLogOldNew)
                //    //{
                //    //    RepoLog.CreateBulky(list);
                //    //}
                //}
                #endregion
                foreach (trxDataAccrue var in listAccrue)
                {
                    trxDataAccrue param = new trxDataAccrue();
                    param.ID = var.ID;
                    param.SOW = department;
                    param.Type = Type == "" ? var.Type : Type;
                    param.UpdatedBy = UserID;
                    param.UpdatedDate = DateTime.Now;

                    listAccrueParam.Add(param);
                }
                //IList<List<trxDataAccrue>> ListNew = SplitList<trxDataAccrue>(listAccrueParam);
                //foreach (List<trxDataAccrue> list in ListNew)
                //{
                //    Repo.MoveFinanceBulky(list);
                //}
                IList<List<trxDataAccrue>> ListNew = SplitList<trxDataAccrue>(listAccrueParam);
                List<List<trxDataAccrue>> ListNewConvert = ListNew as List<List<trxDataAccrue>>;
                list1 = DivideList1(ListNewConvert);
                list2 = DivideList2(ListNewConvert);
                Thread oThreadone = new Thread(myThreadMove1);
                Thread oThreadtwo = new Thread(myThreadMove2);

                oThreadone.Start();
                oThreadtwo.Start();

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
                    paramLog.AccrueStatusID = (int)ConstantAccrueStatusHelper.AccrueStatus.WaitingFinanceConfirmation;
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
                    paramLog.Remarks = var.Remarks;
                    paramLog.Week = var.Week;
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
                //IList<List<trxDataAccrueLog>> ListLogNew = SplitList<trxDataAccrueLog>(ListLog);
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
                return new trxDataAccrue((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "FinanceConfirmationService", "Move", UserID));
            }
            finally
            {
                context.Dispose();
            }
        }
        public trxDataAccrue UpdateAmount(string UserID, vwtrxDataAccrue var)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var Repo = new trxDataAccrueRepository(context);
            var RepoLog = new trxDataAccrueLogRepository(context);
            var uow = context.CreateUnitOfWork();

            List<trxDataAccrue> listAccrue = new List<trxDataAccrue>();
            List<vwAccrueList> List = new List<vwAccrueList>();
            List<trxDataAccrueLog> ListLog = new List<trxDataAccrueLog>();
            List<string> paramID = new List<string>();

            try
            {

                trxDataAccrue param = new trxDataAccrue();
                param.ID = var.trxDataAccrueID;
                param.SONumber = var.SONumber;
                param.RegionID = var.RegionID;

                param.EndDatePeriod = var.EndDatePeriod;
                param.SiteID = var.SiteID;
                param.SiteName = var.SiteName;
                param.SiteIDOpr = var.SiteIDOpr;
                param.SiteNameOpr = var.SiteNameOpr;
                param.CompanyID = var.CompanyID;
                param.CustomerID = var.CustomerID;
                param.AccrueStatusID = (int)ConstantAccrueStatusHelper.AccrueStatus.WaitingFinanceConfirmation;
                param.BaseLeasePrice = var.BaseLeasePrice;
                param.ServicePrice = var.ServicePrice;
                param.Currency = var.Currency;
                param.StartDateBAPS = var.StartDateBAPS;
                param.EndDateBAPS = var.EndDateBAPS;
                param.StartDateAccrue = var.StartDateAccrue;
                param.EndDateAccrue = var.EndDateAccrue;
                param.CompanyInvID = var.CompanyInvID;
                param.StatusMasterList = var.StatusMasterList;
                param.TenantType = var.TenantType;
                param.RFIDate = var.RFIDate;
                param.SldDate = var.SldDate;
                param.BAPSDate = var.BAPSDate;
                param.Month = var.Month;
                param.D = var.D;
                param.OD = var.OD;
                param.ODCategory = var.ODCategory;
                param.MioAccrue = var.MioAccrue;
                param.TotalAmount = var.TotalAmount;
                param.Type = var.Type;
                param.SOW = var.SOW;
                param.Remarks = var.Remarks;
                param.Week = var.Week;
                param.CreatedBy = var.CreatedBy;
                param.CreatedDate = var.CreatedDate;
                param.UpdatedBy = UserID;
                param.UpdatedDate = DateTime.Now;
                Repo.Update(param);



                trxDataAccrueLog paramLog = new trxDataAccrueLog();
                paramLog.trxDataAccrueID = var.trxDataAccrueID;
                paramLog.SONumber = var.SONumber;
                paramLog.RegionID = var.RegionID;
                paramLog.EndDatePeriod = var.EndDatePeriod;
                paramLog.SiteID = var.SiteID;
                paramLog.SiteName = var.SiteName;
                paramLog.SiteIDOpr = var.SiteIDOpr;
                paramLog.SiteNameOpr = var.SiteNameOpr;
                paramLog.CompanyID = var.CompanyID;
                paramLog.CustomerID = var.CustomerID;
                paramLog.AccrueStatusID = (int)ConstantAccrueStatusHelper.AccrueStatus.WaitingFinanceConfirmation;
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
                paramLog.Remarks = var.Remarks;
                paramLog.Week = var.Week;
                paramLog.CreatedBy = UserID;
                paramLog.CreatedDate = DateTime.Now;


                RepoLog.Create(paramLog);
                uow.SaveChanges();
                return new trxDataAccrue();
            }
            catch (Exception ex)
            {
                uow.Dispose();
                return new trxDataAccrue((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "FinanceConfirmationService", "UpdateAmount", UserID));
            }
            finally
            {
                context.Dispose();
            }
        }



        public decimal GetCalculateAmount(string UserID, vwtrxDataAccrue data)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var Repo = new vwtrxDataAccrueRepository(context);
            decimal sum = 0;
            try
            {
                sum = Repo.GetCalculateAmount(data);
                return sum;
            }
            catch (Exception ex)
            {
                return sum;
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
                    Repo.ConfirmFinanceBulky(list);
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
                    Repo.ConfirmFinanceBulky(list);
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
                    Repo.MoveFinanceBulky(list);
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
                    Repo.MoveFinanceBulky(list);
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
