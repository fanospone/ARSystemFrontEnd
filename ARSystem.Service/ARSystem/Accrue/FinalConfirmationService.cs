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
    public class FinalConfirmationService
    {
        public List<List<trxDataAccrueLog>> listLog1 = new List<List<trxDataAccrueLog>>();
        public List<List<trxDataAccrueLog>> listLog2 = new List<List<trxDataAccrueLog>>();
        public static string linkARSystem = ConfigurationManager.AppSettings["LinkARSystem"].ToString();
        public int GetfinalConfirmToListCount(string UserID, vwAccrueFinalConfirm data, string monthDate, string week)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var repo = new vwAccrueFinalConfirmRepository(context);


            try
            {
                //string strWhereClause = "";

                return repo.GetListFinalConfirmSummary(int.Parse(week), int.Parse(DateTime.Parse(monthDate).ToString("MM")), int.Parse(DateTime.Parse(monthDate).ToString("yyyy")), (int)ConstantAccrueStatusHelper.AccrueStatus.Done).Count;
            }
            catch (Exception ex)
            {
                Helper.logError(ex.Message.ToString(), "FinalConfirmationService", "GetFinalConfirmToListCount", UserID);
                return 0;
            }
            finally
            {
                context.Dispose();
            }
        }
        public List<vwAccrueFinalConfirm> GetFinalConfirmToList(string UserID, vwAccrueFinalConfirm data, string monthDate, string week, string strOrderBy, int intRowStart = 0, int intPageSize = 0)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var Repo = new vwAccrueFinalConfirmRepository(context);
            List<vwAccrueFinalConfirm> list = new List<vwAccrueFinalConfirm>();

            try
            {

                list = Repo.GetListFinalConfirmSummary(int.Parse(week), int.Parse(DateTime.Parse(monthDate).ToString("MM")), int.Parse(DateTime.Parse(monthDate).ToString("yyyy")), (int)ConstantAccrueStatusHelper.AccrueStatus.Done);
                
                int page = 1;
                //if (intPageSize != 0 && list.Count > intPageSize)
                //{
                //    page = (intRowStart / intPageSize) + 1;
                //    IList<vwAccrueFinalConfirm> PageList = GetPage(list, page, intPageSize);
                //    list = PageList as List<vwAccrueFinalConfirm>;
                //}

                return list;
            }
            catch (Exception ex)
            {
                list.Add(new vwAccrueFinalConfirm((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "FinalConfirmationService", "GetFinalConfirmToList", UserID)));
                return list;
            }
            finally
            {
                context.Dispose();
            }


        }

        public int GetUserConfirmedFinalToListCount(string UserID, string monthDate, string week)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var Repo = new vwAccrueFinalConfirmRepository(context);


            vmDynamicTable returnModel = new vmDynamicTable();
            DataTable dt = new DataTable();
            DataTable dtMove = new DataTable();
            try
            {

                //string strWhereClause = "";
                dt = Repo.GetDataTableDynamic("dbo.uspAccrueConfirmedUser", int.Parse(week), int.Parse(DateTime.Parse(monthDate).ToString("MM")), int.Parse(DateTime.Parse(monthDate).ToString("yyyy")));
                return dt.Rows.Count;
            }
            catch (Exception ex)
            {
                Helper.logError(ex.Message.ToString(), "FinalConfirmationService", "GetUserConfirmedFinalToListCount", UserID);
                return 0;
            }
            finally
            {
                context.Dispose();
            }


        }
        public vmDynamicTable GetUserConfirmedFinalToList(string UserID, string monthDate, string week, string strOrderBy, int intRowStart = 0, int intPageSize = 0)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var Repo = new vwAccrueFinalConfirmRepository(context);

            //List<Dictionary<string, object>> listDynamic = new List<Dictionary<string, object>>();
            vmDynamicTable returnModel = new vmDynamicTable();
            DataTable dt = new DataTable();
            DataTable dtMove = new DataTable();
            try
            {

                //string strWhereClause = "";
                dt = Repo.GetDataTableDynamic("dbo.uspAccrueConfirmedUser", int.Parse(week), int.Parse(DateTime.Parse(monthDate).ToString("MM")), int.Parse(DateTime.Parse(monthDate).ToString("yyyy")));
                dt.Columns.Add("Total Before Confirmed", typeof(decimal));

                dtMove = Repo.GetDataTableDynamic("dbo.uspAccrueConfirmedUserMoveHistory", int.Parse(week), int.Parse(DateTime.Parse(monthDate).ToString("MM")), int.Parse(DateTime.Parse(monthDate).ToString("yyyy")));
                DataView dv1 = dtMove.DefaultView;
                string thead = "<tr>";
                string tbody = "";
                foreach (DataColumn column in dt.Columns)
                {
                    thead += "<th class='datatable-col-100 text-center'>" + column.ColumnName + "</th>";
                }
                DataRow row = dt.NewRow();
                row["Department"] = "Total After Confirmed";
                dt.Rows.Add(row);
                decimal total = 1;

                decimal totalBeforeConfirmed = 0;
                int flagTotalAfter = 0;

                foreach (DataRow dr in dt.Rows)
                {

                    totalBeforeConfirmed = 0;
                    if (dr["Department"].ToString() == "Total After Confirmed")
                        flagTotalAfter = 1;
                    DataTable dt2 = new DataTable();
                    if (flagTotalAfter == 0)
                        if (dtMove.Rows.Count > 0)
                        {
                            dv1.RowFilter = " DeptFrom = '" + dr["Department"].ToString() + "'";
                            if (dv1.Count > 0)
                                dt2 = dv1.ToTable();

                            foreach (DataColumn column in dt.Columns)
                            {
                                column.ReadOnly = false;
                            }
                            foreach (DataRow rw in dtMove.Rows)
                            {
                                foreach (DataColumn colu in dtMove.Columns)
                                {
                                    colu.ReadOnly = false;
                                    if (colu.ColumnName != "DeptFrom")
                                    {
                                        if (colu.ColumnName == dr["Department"].ToString() && rw[colu.ColumnName].ToString() != "0")
                                        {
                                            if (dr[colu.ColumnName].ToString() != "0")
                                            {
                                                dr[colu.ColumnName] = (Convert.ToInt32(dr[colu.ColumnName].ToString()) - Convert.ToInt32(rw[colu.ColumnName].ToString()));
                                                rw[colu.ColumnName] = 0.ToString();
                                            }

                                        }
                                        if (dr["Department"].ToString() == rw["DeptFrom"].ToString())
                                        {
                                            if (colu.ColumnName != dr["Department"].ToString())
                                            {
                                                DataColumnCollection columns = dt.Columns;
                                                if (columns.Contains(colu.ColumnName))
                                                {
                                                    dr[colu.ColumnName] = rw[colu.ColumnName];
                                                }
                                            }

                                        }
                                    }
                                }
                            }
                        }


                    foreach (DataColumn column in dt.Columns)
                    {
                        column.ReadOnly = false;

                        if (column.ColumnName != "Department")
                        {
                            if (flagTotalAfter == 0)
                            {
                                if (column.ColumnName != "Total Before Confirmed")//Total Before Confirmed
                                    totalBeforeConfirmed += decimal.Parse(dr[column.ColumnName].ToString()); //Get Total before confirmed
                                else
                                {
                                    if (column.ColumnName != "% Before")
                                        dr[column.ColumnName] = totalBeforeConfirmed.ToString();
                                }

                            }
                            else
                            {
                                dr[column.ColumnName] = dt.Compute("Sum([" + column.ColumnName + "])", string.Empty);
                                if (column.ColumnName == "Total Before Confirmed")//Total Before Confirmed
                                    total = decimal.Parse(dr[column.ColumnName].ToString());
                            }
                        }

                    }

                }
                dt.Columns.Add("% Before", typeof(decimal));
                thead += "<th class='datatable-col-100 text-center'>% Before</th>";
                DataRow newrow = dt.NewRow();
                newrow["Department"] = "% After";
                dt.Rows.Add(newrow);
                DataTable dt3 = new DataTable();
                dt3 = dt.Select().Where(p => (p["Department"].ToString()) == "Total After Confirmed").CopyToDataTable();
                foreach (DataRow dr in dt.Rows) //to Get Percentage
                {

                    tbody += "<tr>";
                    foreach (DataColumn column in dt.Columns)
                    {
                        column.ReadOnly = false;
                        if (column.ColumnName == "% Before")
                            if (dr["Department"].ToString() != "Total After Confirmed")
                                if (dr["Total Before Confirmed"].ToString() != "")
                                    dr[column.ColumnName] = Math.Round((decimal.Parse(dr["Total Before Confirmed"].ToString()) / total) * 100, 2);

                        if (column.ColumnName != "Department" && dr["Department"].ToString() == "% After")
                        {
                            if (column.ColumnName != "Total Before Confirmed")
                                if (column.ColumnName == "% Before")
                                    dr["% Before"] = Math.Round(((decimal.Parse(dt.Compute("Sum([Total Before Confirmed])", string.Empty).ToString()) - decimal.Parse(dt3.Rows[0]["Total Before Confirmed"].ToString())) / total) * 100, 2);
                                else
                                    dr[column.ColumnName] = Math.Round(((decimal.Parse(dt.Compute("Sum([" + column.ColumnName + "])", string.Empty).ToString()) - decimal.Parse(dt3.Rows[0][column.ColumnName].ToString())) / total) * 100, 2);

                        }
                        if (column.ColumnName == "Department")
                            tbody += "<td>" + dr[column.ColumnName].ToString() + "</td>";
                        else
                            tbody += "<td align='right'>" + dr[column.ColumnName].ToString() + "</td>";
                    }
                    tbody += "</tr>";
                }

                thead += "</tr>";
                tbody += "";
                returnModel.thead = thead;
                returnModel.tbody = tbody;

                return returnModel;
            }
            catch (Exception ex)
            {
                return new vmDynamicTable((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "FinalConfirmationService", "GetUserConfirmedFinalToList", UserID));
            }
            finally
            {
                context.Dispose();
            }


        }

        public DataTable GetUserConfirmedFinalToDataTable(string UserID, string monthDate, string week, string strOrderBy, int intRowStart = 0, int intPageSize = 0)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var Repo = new vwAccrueFinalConfirmRepository(context);

            DataTable dt = new DataTable();
            DataTable dtMove = new DataTable();
            try
            {

                //string strWhereClause = "";
                dt = Repo.GetDataTableDynamic("dbo.uspAccrueConfirmedUser", int.Parse(week), int.Parse(DateTime.Parse(monthDate).ToString("MM")), int.Parse(DateTime.Parse(monthDate).ToString("yyyy")));
                dt.Columns.Add("Total Before Confirmed", typeof(decimal));

                dtMove = Repo.GetDataTableDynamic("dbo.uspAccrueConfirmedUserMoveHistory", int.Parse(week), int.Parse(DateTime.Parse(monthDate).ToString("MM")), int.Parse(DateTime.Parse(monthDate).ToString("yyyy")));
                DataView dv1 = dtMove.DefaultView;


                DataRow row = dt.NewRow();
                row["Department"] = "Total After Confirmed";
                dt.Rows.Add(row);
                decimal total = 1;

                decimal totalBeforeConfirmed = 0;
                int flagTotalAfter = 0;

                foreach (DataRow dr in dt.Rows)
                {

                    totalBeforeConfirmed = 0;
                    if (dr["Department"].ToString() == "Total After Confirmed")
                        flagTotalAfter = 1;
                    DataTable dt2 = new DataTable();
                    if (flagTotalAfter == 0)
                        if (dtMove.Rows.Count > 0)
                        {
                            dv1.RowFilter = " DeptFrom = '" + dr["Department"].ToString() + "'";
                            if (dv1.Count > 0)
                                dt2 = dv1.ToTable();

                            foreach (DataColumn column in dt.Columns)
                            {
                                column.ReadOnly = false;
                            }
                            foreach (DataRow rw in dtMove.Rows)
                            {
                                foreach (DataColumn colu in dtMove.Columns)
                                {
                                    colu.ReadOnly = false;
                                    if (colu.ColumnName != "DeptFrom")
                                    {
                                        if (colu.ColumnName == dr["Department"].ToString() && rw[colu.ColumnName].ToString() != "0")
                                        {
                                            if (dr[colu.ColumnName].ToString() != "0")
                                            {
                                                dr[colu.ColumnName] = (Convert.ToInt32(dr[colu.ColumnName].ToString()) - Convert.ToInt32(rw[colu.ColumnName].ToString()));
                                                rw[colu.ColumnName] = 0.ToString();
                                            }

                                        }
                                        if (dr["Department"].ToString() == rw["DeptFrom"].ToString())
                                        {
                                            if (colu.ColumnName != dr["Department"].ToString())
                                                dr[colu.ColumnName] = rw[colu.ColumnName];
                                        }
                                    }
                                }
                            }
                        }


                    foreach (DataColumn column in dt.Columns)
                    {
                        column.ReadOnly = false;

                        if (column.ColumnName != "Department")
                        {
                            if (flagTotalAfter == 0)
                            {
                                if (column.ColumnName != "Total Before Confirmed")
                                    totalBeforeConfirmed += decimal.Parse(dr[column.ColumnName].ToString()); //Get Total before confirmed
                                else
                                {
                                    if (column.ColumnName != "% Before")
                                        dr[column.ColumnName] = totalBeforeConfirmed.ToString();
                                }

                            }
                            else
                            {
                                dr[column.ColumnName] = dt.Compute("Sum([" + column.ColumnName + "])", string.Empty);
                                if (column.ColumnName == "Total Before Confirmed")
                                    total = decimal.Parse(dr[column.ColumnName].ToString());
                            }
                        }

                    }

                }
                dt.Columns.Add("% Before", typeof(decimal));

                DataRow newrow = dt.NewRow();
                newrow["Department"] = "% After";
                dt.Rows.Add(newrow);
                DataTable dt3 = new DataTable();
                dt3 = dt.Select().Where(p => (p["Department"].ToString()) == "Total After Confirmed").CopyToDataTable();
                foreach (DataRow dr in dt.Rows) //to Get Percentage
                {


                    foreach (DataColumn column in dt.Columns)
                    {
                        column.ReadOnly = false;
                        if (column.ColumnName == "% Before")
                            if (dr["Department"].ToString() != "Total After Confirmed")
                                if (dr["Total Before Confirmed"].ToString() != "")
                                    dr[column.ColumnName] = Math.Round((decimal.Parse(dr["Total Before Confirmed"].ToString()) / total) * 100, 2);

                        if (column.ColumnName != "Department" && dr["Department"].ToString() == "% After")
                        {
                            if (column.ColumnName != "Total Before Confirmed")
                                if (column.ColumnName == "% Before")
                                    dr["% Before"] = Math.Round(((decimal.Parse(dt.Compute("Sum([Total Before Confirmed])", string.Empty).ToString()) - decimal.Parse(dt3.Rows[0]["Total Before Confirmed"].ToString())) / total) * 100, 2);
                                else
                                    dr[column.ColumnName] = Math.Round(((decimal.Parse(dt.Compute("Sum([" + column.ColumnName + "])", string.Empty).ToString()) - decimal.Parse(dt3.Rows[0][column.ColumnName].ToString())) / total) * 100, 2);

                        }

                    }

                }

                return dt;
            }
            catch (Exception ex)
            {
                return dt;
            }
            finally
            {
                context.Dispose();
            }


        }
        public trxDataAccrue IsExistReConfirm(string UserID, string monthDate, string week)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var Repo = new trxDataAccrueRepository(context);
            trxDataAccrue data = new trxDataAccrue();
            try
            {

                data = Repo.CheckGetExistIsReConfirm(int.Parse(week), int.Parse(DateTime.Parse(monthDate).ToString("MM")), int.Parse(DateTime.Parse(monthDate).ToString("yyyy"))).FirstOrDefault();
                return data;
            }
            catch (Exception ex)
            {
                return new trxDataAccrue((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "FinalConfirmationService", "IsExistReConfirm", UserID));
            }
            finally
            {
                context.Dispose();
            }
        }
        public trxDataAccrue ReConfirm(string UserID, string monthDate, string week)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var contextHC = new DbContext(Helper.GetConnection("HumanCapital"));
            var Repo = new trxDataAccrueRepository(context);

            var RepoAutoConfirm = new mstAccrueSettingAutoConfirmRepository(context);
            var RepoViewAccrue = new vwtrxDataAccrueRepository(context);
            var RepoUser = new vwidxAccrueUserSOWRepository(context);
            var repoSummary = new vwAccrueFinalConfirmRepository(context);
            var repoEmployee = new vwEmployeeRepository(contextHC);


            var RepoLog = new trxDataAccrueLogRepository(context);
            var uow = context.CreateUnitOfWork();

            trxDataAccrue data = new trxDataAccrue();
            List<trxDataAccrue> listAccrue = new List<trxDataAccrue>();
            List<trxDataAccrueLog> ListLog = new List<trxDataAccrueLog>();
            List<trxDataAccrueLog> ListLogOld = new List<trxDataAccrueLog>();
            List<string> paramID = new List<string>();

            try
            {
                if (GetUserConfirmedFinalToListCount(UserID, monthDate, week) == 0)
                {
                    return new trxDataAccrue((int)Helper.ErrorType.Validation, "Data not Available");
                }
                string namaUserID = repoEmployee.GetList("UserID = '" + UserID + "'", "").FirstOrDefault().Name;
                string weekGetDate = RepoViewAccrue.GetAccrueWeekGetDate(DateTime.Now, "GetWeekNow");
                string strWhereClause = "";
                strWhereClause += "YEAR(Period) = '" + int.Parse(DateTime.Parse(monthDate).ToString("yyyy")) + "' AND Month(Period) = '" + int.Parse(DateTime.Parse(monthDate).ToString("MM")) + "' AND ";
                strWhereClause += "Week = '" + int.Parse(week) + "' AND ";
                strWhereClause += "AccrueStatusID = '" + (int)ConstantAccrueStatusHelper.AccrueStatus.WaitingReConfirmConfirmation + "' AND IsActive = 1";
                if (RepoAutoConfirm.GetCount(strWhereClause) == 0)
                {
                    return new trxDataAccrue((int)Helper.ErrorType.Validation, "Please setting auto confirm first");
                }
                DateTime autoConfirmDate = RepoAutoConfirm.GetList(strWhereClause, "").FirstOrDefault().AutoConfirmDate;

                strWhereClause = "";

                #region Send email

                strWhereClause = "";
                if (!string.IsNullOrWhiteSpace(monthDate))
                {
                    strWhereClause += "YEAR(CreatedDate) = '" + int.Parse(DateTime.Parse(monthDate).ToString("yyyy")) + "' AND Month(CreatedDate) = '" + int.Parse(DateTime.Parse(monthDate).ToString("MM")) + "' AND ";
                }
                if (!string.IsNullOrWhiteSpace(week))
                {
                    strWhereClause += "Week = '" + int.Parse(week) + "' AND ";
                }
                strWhereClause += "AccrueStatusID = '" + (int)ConstantAccrueStatusHelper.AccrueStatus.WaitingFinalConfirmation + "' AND ";
                strWhereClause = !string.IsNullOrWhiteSpace(strWhereClause) ? strWhereClause.Substring(0, strWhereClause.Length - 5) : "";
                listAccrue = Repo.GetList(strWhereClause, "");
                string Subject = "Accrue - Re Confirmation User";
                string IDParam = "ID IN ('" + string.Join("','", listAccrue.Select(x => x.ID).Distinct().ToList()) + "')";
                strWhereClause = "SOW IN ('" + string.Join("','", listAccrue.Select(x => x.SOW).Distinct().ToList()) + "')";
                List<vwidxAccrueUserSOW> listUser = RepoUser.GetList(strWhereClause, "");
                string emailTo = string.Join(";", listUser.Select(x => x.Email).ToList());
                emailTo += ";";
                string namaTo = string.Join(",", listUser.Select(x => x.Name).Distinct().ToList());

                #region table versus
                DataTable dt = new DataTable();
                DataTable dtMove = new DataTable();
                dt = repoSummary.GetDataTableDynamic("dbo.uspAccrueConfirmedUser", int.Parse(week), int.Parse(DateTime.Parse(monthDate).ToString("MM")), int.Parse(DateTime.Parse(monthDate).ToString("yyyy")));
                dt.Columns.Add("Grand Total", typeof(decimal)); //Total Before Confirmed

                dtMove = repoSummary.GetDataTableDynamic("dbo.uspAccrueConfirmedUserMoveHistory", int.Parse(week), int.Parse(DateTime.Parse(monthDate).ToString("MM")), int.Parse(DateTime.Parse(monthDate).ToString("yyyy")));
                DataView dv1 = dtMove.DefaultView;
                string tableHTML = "<table border='1' style='border-collapse: collapse'><thead><tr>";

                foreach (DataColumn column in dt.Columns)
                {
                    tableHTML += "<th align='center'><b>" + column.ColumnName + "</b></th>";
                }

                DataRow row = dt.NewRow();
                row["Department"] = "Grand Total"; //Total After Confirmed
                dt.Rows.Add(row);
                decimal total = 1;

                decimal totalBeforeConfirmed = 0;
                int flagTotalAfter = 0;

                foreach (DataRow dr in dt.Rows)
                {

                    totalBeforeConfirmed = 0;
                    if (dr["Department"].ToString() == "Grand Total")
                        flagTotalAfter = 1;
                    DataTable dt2 = new DataTable();
                    if (flagTotalAfter == 0)
                        if (dtMove.Rows.Count > 0)
                        {
                            dv1.RowFilter = " DeptFrom = '" + dr["Department"].ToString() + "'";
                            if (dv1.Count > 0)
                                dt2 = dv1.ToTable();

                            foreach (DataColumn column in dt.Columns)
                            {
                                column.ReadOnly = false;
                            }
                            foreach (DataRow rw in dtMove.Rows)
                            {
                                foreach (DataColumn colu in dtMove.Columns)
                                {
                                    colu.ReadOnly = false;
                                    if (colu.ColumnName != "DeptFrom")
                                    {
                                        if (colu.ColumnName == dr["Department"].ToString() && rw[colu.ColumnName].ToString() != "0")
                                        {
                                            if (dr[colu.ColumnName].ToString() != "0")
                                            {
                                                dr[colu.ColumnName] = (Convert.ToInt32(dr[colu.ColumnName].ToString()) - Convert.ToInt32(rw[colu.ColumnName].ToString()));
                                                rw[colu.ColumnName] = 0.ToString();
                                            }

                                        }
                                        if (dr["Department"].ToString() == rw["DeptFrom"].ToString())
                                        {
                                            DataColumnCollection columnsDt = dt.Columns;
                                            if (colu.ColumnName != dr["Department"].ToString() && columnsDt.Contains(colu.ColumnName))
                                                dr[colu.ColumnName] = rw[colu.ColumnName];
                                        }
                                    }
                                }
                            }
                        }


                    foreach (DataColumn column in dt.Columns)
                    {
                        column.ReadOnly = false;

                        if (column.ColumnName != "Department")
                        {
                            if (flagTotalAfter == 0)
                            {
                                if (column.ColumnName != "Grand Total")
                                    totalBeforeConfirmed += decimal.Parse(dr[column.ColumnName].ToString()); //Get Total before confirmed
                                else
                                {
                                    if (column.ColumnName != "% Before")
                                        dr[column.ColumnName] = totalBeforeConfirmed.ToString();
                                }

                            }
                            else
                            {
                                dr[column.ColumnName] = dt.Compute("Sum([" + column.ColumnName + "])", string.Empty);
                                if (column.ColumnName == "Grand Total")
                                    total = decimal.Parse(dr[column.ColumnName].ToString());
                            }
                        }

                    }

                }
                dt.Columns.Add("% Before", typeof(decimal));
                //tableHTML += "<th align='center'> <b>% Before</b></th>";
                tableHTML += "</tr></thead>";
                DataRow newrow = dt.NewRow();
                newrow["Department"] = "% After";
                dt.Rows.Add(newrow);
                DataTable dt3 = new DataTable();
                dt3 = dt.Select().Where(p => (p["Department"].ToString()) == "Grand Total").CopyToDataTable();
                tableHTML += "<tbody>";
                foreach (DataRow dr in dt.Rows) //to Get Percentage
                {
                    if (dr["Department"].ToString() != "% After")
                    {
                        tableHTML += "<tr>";
                        foreach (DataColumn column in dt.Columns)
                        {
                            column.ReadOnly = false;
                            if (column.ColumnName == "% Before")
                                if (dr["Department"].ToString() != "Grand Total")
                                    if (dr["Grand Total"].ToString() != "")//Total Before Confirmed
                                        dr[column.ColumnName] = Math.Round((decimal.Parse(dr["Grand Total"].ToString()) / total) * 100, 2);//Total Before Confirmed

                            if (column.ColumnName != "Department" && dr["Department"].ToString() == "% After")
                            {
                                if (column.ColumnName != "Grand Total")
                                    if (column.ColumnName == "% Before")
                                        dr["% Before"] = Math.Round(((decimal.Parse(dt.Compute("Sum([Grand Total])", string.Empty).ToString()) - decimal.Parse(dt3.Rows[0]["Grand Total"].ToString())) / total) * 100, 2);//Total Before Confirmed
                                    else
                                        dr[column.ColumnName] = Math.Round(((decimal.Parse(dt.Compute("Sum([" + column.ColumnName + "])", string.Empty).ToString()) - decimal.Parse(dt3.Rows[0][column.ColumnName].ToString())) / total) * 100, 2);

                            }
                            if (column.ColumnName != "% Before")
                            {
                                if (column.ColumnName == "Department")
                                    tableHTML += "<td>" + dr[column.ColumnName].ToString() + "</td>";
                                else
                                {
                                    tableHTML += "<td align='right'>" + " " + String.Format("{0}", Convert.ToInt32(dr[column.ColumnName].ToString())) + "</td>";
                                }

                            }

                        }
                        tableHTML += "</tr>";
                    }

                }
                tableHTML += "</tbody>";
                tableHTML += "</table><br/>";
                #endregion
                string strBody = "<b>Dear All,</b> <br/><br/> Berikut summary untuk REPORT ACCRUE REVENUE CLOSING ";
                if (weekGetDate != "5")
                    strBody += "WK " + weekGetDate + " " + DateTime.Now.ToString("MMM yyyy") + " - <b>VERSI ACCOUNTING SEMENTARA, cut off konfirmasi by system jam  " + autoConfirmDate.ToString("HH:mm") + ".</b>";
                else
                    strBody += DateTime.Now.ToString("MMM yyyy") + " - <b>VERSI ACCOUNTING SEMENTARA, cut off konfirmasi by system jam  " + autoConfirmDate.ToString("HH:mm") + ".</b>";

                strBody += "<br/><br/><center><h3>Movement User Setelah User Konfirmasi & Klik Reconfirm kembali oleh Fin</h3></center>";
                strBody += "<br/><br/> <b>USER VS USER</b><br/>" + tableHTML;

                strBody += "Silahkan dapat dikonfirmasi status progress di system maksimal jam " + autoConfirmDate.ToString("HH:mm");
                strBody += " , apabila ada detail progress disubmit setelah jam " + autoConfirmDate.ToString("HH:mm") + ", <b> Kami anggap ''Confirmed'' atas progress site tersebut.</b>";
                //strBody += "<br/><br/>" + namaTo;
                strBody += "<br/><br/><a href='"+ linkARSystem + "/Accrue/UserConfirmation'>Link Application</a>";
                strBody += "<br/><br/><br/> <b>Regards,</b><br/> " + namaUserID;


                #endregion
                /////////////////////////////////////////////////////
                data = Repo.FinalConfirmBulky(1, (int)ConstantAccrueStatusHelper.AccrueStatus.WaitingReConfirmConfirmation, UserID, int.Parse(week), int.Parse(DateTime.Parse(monthDate).ToString("MM")), int.Parse(DateTime.Parse(monthDate).ToString("yyyy")));
                strWhereClause = "";
                if (!string.IsNullOrWhiteSpace(monthDate))
                {
                    strWhereClause += "YEAR(CreatedDate) = '" + int.Parse(DateTime.Parse(monthDate).ToString("yyyy")) + "' AND Month(CreatedDate) = '" + int.Parse(DateTime.Parse(monthDate).ToString("MM")) + "' AND ";
                }
                if (!string.IsNullOrWhiteSpace(week))
                {
                    strWhereClause += "Week = '" + int.Parse(week) + "' AND ";
                }
                strWhereClause += "AccrueStatusID = '" + (int)ConstantAccrueStatusHelper.AccrueStatus.WaitingReConfirmConfirmation + "' AND ";
                strWhereClause = !string.IsNullOrWhiteSpace(strWhereClause) ? strWhereClause.Substring(0, strWhereClause.Length - 5) : "";
                listAccrue = Repo.GetList(strWhereClause, "");


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
                    paramLog.AccrueStatusID = (int)ConstantAccrueStatusHelper.AccrueStatus.WaitingReConfirmConfirmation;
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
                    paramLog.Remarks = var.Remarks;
                    paramLog.Week = var.Week;
                    paramLog.IsReConfirm = 1;
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

                #endregion

                ////////////////////////////////////////////////
                EmailHelper.SendEmail(emailTo, "", "", Subject, strBody, "", UserID);
                uow.SaveChanges();
                return new trxDataAccrue();
            }
            catch (Exception ex)
            {
                uow.Dispose();
                return new trxDataAccrue((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "FinalConfirmationService", "ReConfirm", UserID));
            }
            finally
            {
                context.Dispose();
                contextHC.Dispose();
            }
        }
        public trxDataAccrue FinalConfirm(string UserID, string monthDate, string week)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var contextHC = new DbContext(Helper.GetConnection("HumanCapital"));
            var Repo = new trxDataAccrueRepository(context);

            var RepoLog = new trxDataAccrueLogRepository(context);
            var RepoAutoConfirm = new mstAccrueSettingAutoConfirmRepository(context);
            var RepoViewAccrue = new vwtrxDataAccrueRepository(context);
            var RepoUser = new vwidxAccrueUserSOWRepository(context);
            var repoSummary = new vwAccrueFinalConfirmRepository(context);
            var repoEmployee = new vwEmployeeRepository(contextHC);
            var uow = context.CreateUnitOfWork();

            trxDataAccrue data = new trxDataAccrue();
            List<trxDataAccrue> listAccrue = new List<trxDataAccrue>();
            List<trxDataAccrueLog> ListLog = new List<trxDataAccrueLog>();
            List<trxDataAccrueLog> ListLogOld = new List<trxDataAccrueLog>();
            List<string> paramID = new List<string>();

            try
            {
                if (GetUserConfirmedFinalToListCount(UserID, monthDate, week) == 0)
                {
                    return new trxDataAccrue((int)Helper.ErrorType.Validation, "Data not Available");
                }
                string namaUserID = repoEmployee.GetList("UserID = '" + UserID + "'", "").FirstOrDefault().Name;
                string weekGetDate = RepoViewAccrue.GetAccrueWeekGetDate(DateTime.Now, "GetWeekNow");
                string strWhereClause = "";
                //strWhereClause += "YEAR(Period) = '" + int.Parse(DateTime.Parse(monthDate).ToString("yyyy")) + "' AND Month(Period) = '" + int.Parse(DateTime.Parse(monthDate).ToString("MM")) + "' AND ";
                //strWhereClause += "Week = '" + int.Parse(week) + "' AND ";
                //strWhereClause += "AccrueStatusID = '" + (int)ConstantAccrueStatusHelper.AccrueStatus.Done + "' AND IsActive = 1";
                //if (RepoAutoConfirm.GetCount(strWhereClause) == 0)
                //{
                //    return new trxDataAccrue((int)Helper.ErrorType.Validation, "Please setting auto confirm first");
                //}
                //DateTime autoConfirmDate = RepoAutoConfirm.GetList(strWhereClause, "").FirstOrDefault().AutoConfirmDate;

                #region Send email

                strWhereClause = "";
                if (!string.IsNullOrWhiteSpace(monthDate))
                {
                    strWhereClause += "YEAR(CreatedDate) = '" + int.Parse(DateTime.Parse(monthDate).ToString("yyyy")) + "' AND Month(CreatedDate) = '" + int.Parse(DateTime.Parse(monthDate).ToString("MM")) + "' AND ";
                }
                if (!string.IsNullOrWhiteSpace(week))
                {
                    strWhereClause += "Week = '" + int.Parse(week) + "' AND ";
                }
                strWhereClause += "AccrueStatusID = '" + (int)ConstantAccrueStatusHelper.AccrueStatus.WaitingFinalConfirmation + "' AND ";
                strWhereClause = !string.IsNullOrWhiteSpace(strWhereClause) ? strWhereClause.Substring(0, strWhereClause.Length - 5) : "";
                listAccrue = Repo.GetList(strWhereClause, "");
                string Subject = "Accrue - Confirmation FINAL";
                string IDParam = "ID IN ('" + string.Join("','", listAccrue.Select(x => x.ID).Distinct().ToList()) + "')";
                strWhereClause = "SOW IN ('" + string.Join("','", listAccrue.Select(x => x.SOW).Distinct().ToList()) + "')";
                List<vwidxAccrueUserSOW> listUser = RepoUser.GetList(strWhereClause, "");
                string emailTo = string.Join(";", listUser.Select(x => x.Email).ToList());
                emailTo += ";";
                string namaTo = string.Join(",", listUser.Select(x => x.Name).Distinct().ToList());

                #region table versus
                DataTable dt = new DataTable();
                DataTable dtMove = new DataTable();
                DataTable dtAttendance = new DataTable();
                dt = repoSummary.GetDataTableDynamic("dbo.uspAccrueConfirmedUser", int.Parse(week), int.Parse(DateTime.Parse(monthDate).ToString("MM")), int.Parse(DateTime.Parse(monthDate).ToString("yyyy")));
                dt.Columns.Add("Total Before Confirmed", typeof(decimal)); //Total Before Confirmed

                dtMove = repoSummary.GetDataTableDynamic("dbo.uspAccrueConfirmedUserMoveHistory", int.Parse(week), int.Parse(DateTime.Parse(monthDate).ToString("MM")), int.Parse(DateTime.Parse(monthDate).ToString("yyyy")));
                dtAttendance = repoSummary.GetListFinalConfirmAttendanceUser(int.Parse(week), int.Parse(DateTime.Parse(monthDate).ToString("MM")), int.Parse(DateTime.Parse(monthDate).ToString("yyyy")), (int)ConstantAccrueStatusHelper.AccrueStatus.WaitingFinalConfirmation);
                DataView dv1 = dtMove.DefaultView;
                string tableHTML = "<table border='1' style='border-collapse: collapse'><thead><tr>";

                foreach (DataColumn column in dt.Columns)
                {
                    tableHTML += "<th align='center'><b>" + column.ColumnName + "</b></th>";
                }

                DataRow row = dt.NewRow();
                row["Department"] = "Total After Confirmed"; //Total After Confirmed
                dt.Rows.Add(row);
                decimal total = 1;

                decimal totalBeforeConfirmed = 0;
                int flagTotalAfter = 0;

                foreach (DataRow dr in dt.Rows)
                {

                    totalBeforeConfirmed = 0;
                    if (dr["Department"].ToString() == "Total After Confirmed")
                        flagTotalAfter = 1;
                    DataTable dt2 = new DataTable();
                    if (flagTotalAfter == 0)
                        if (dtMove.Rows.Count > 0)
                        {
                            dv1.RowFilter = " DeptFrom = '" + dr["Department"].ToString() + "'";
                            if (dv1.Count > 0)
                                dt2 = dv1.ToTable();

                            foreach (DataColumn column in dt.Columns)
                            {
                                column.ReadOnly = false;
                            }
                            foreach (DataRow rw in dtMove.Rows)
                            {
                                foreach (DataColumn colu in dtMove.Columns)
                                {
                                    colu.ReadOnly = false;
                                    if (colu.ColumnName != "DeptFrom")
                                    {
                                        if (colu.ColumnName == dr["Department"].ToString() && rw[colu.ColumnName].ToString() != "0")
                                        {
                                            if (dr[colu.ColumnName].ToString() != "0")
                                            {
                                                dr[colu.ColumnName] = (Convert.ToInt32(dr[colu.ColumnName].ToString()) - Convert.ToInt32(rw[colu.ColumnName].ToString()));
                                                rw[colu.ColumnName] = 0.ToString();
                                            }

                                        }
                                        if (dr["Department"].ToString() == rw["DeptFrom"].ToString())
                                        {
                                            DataColumnCollection columnsDt = dt.Columns;
                                            if (colu.ColumnName != dr["Department"].ToString() && columnsDt.Contains(colu.ColumnName))
                                                dr[colu.ColumnName] = rw[colu.ColumnName];
                                        }
                                        
                                    }
                                }
                            }
                        }


                    foreach (DataColumn column in dt.Columns)
                    {
                        column.ReadOnly = false;

                        if (column.ColumnName != "Department")
                        {
                            if (flagTotalAfter == 0)
                            {
                                if (column.ColumnName != "Total Before Confirmed")
                                    totalBeforeConfirmed += decimal.Parse(dr[column.ColumnName].ToString()); //Get Total before confirmed
                                else
                                {
                                    if (column.ColumnName != "% Before")
                                        dr[column.ColumnName] = totalBeforeConfirmed.ToString();
                                }

                            }
                            else
                            {
                                dr[column.ColumnName] = dt.Compute("Sum([" + column.ColumnName + "])", string.Empty);
                                if (column.ColumnName == "Total Before Confirmed")
                                    total = decimal.Parse(dr[column.ColumnName].ToString());
                            }
                        }

                    }

                }
                dt.Columns.Add("% Before", typeof(decimal));
                dt.Columns.Add("Attendance User", typeof(string)); //Attendance User
                tableHTML += "<th align='center'> <b> % </b></th><th align='center'> <b> Attendance User </b></th>";
                tableHTML += "</tr></thead>";
                DataRow newrow = dt.NewRow();
                newrow["Department"] = "%";
                dt.Rows.Add(newrow);
                DataTable dt3 = new DataTable();
                dt3 = dt.Select().Where(p => (p["Department"].ToString()) == "Total After Confirmed").CopyToDataTable();
                tableHTML += "<tbody>";
                foreach (DataRow dr in dt.Rows) //to Get Percentage
                {
                    //if (dr["Department"].ToString() != "%")
                    //{
                    DataTable dtAtt = new DataTable();
                    if (dr["Department"].ToString() != "Total After Confirmed" && dr["Department"].ToString() != "%")
                        dtAtt = dtAttendance.Select().Where(p => (p["SOW"].ToString()) == dr["Department"].ToString()).CopyToDataTable();
                    tableHTML += "<tr>";
                    foreach (DataColumn column in dt.Columns)
                    {
                        column.ReadOnly = false;
                        if (column.ColumnName == "% Before")
                            if (dr["Department"].ToString() != "Total After Confirmed")
                                if (dr["Total Before Confirmed"].ToString() != "")//Total Before Confirmed
                                    dr[column.ColumnName] = Math.Round((decimal.Parse(dr["Total Before Confirmed"].ToString()) / total) * 100, 2);//Total Before Confirmed

                        if (column.ColumnName == "Attendance User")
                        {
                            if (dr["Department"].ToString() != "Total After Confirmed" && dr["Department"].ToString() != "%")
                                dr[column.ColumnName] = dtAtt.Rows[0]["ConfirmedUser"].ToString();
                        }
                        if (column.ColumnName != "Department" && dr["Department"].ToString() == "%")
                        {
                            if (column.ColumnName != "Total Before Confirmed" && column.ColumnName != "Attendance User")
                                if (column.ColumnName == "% Before")
                                    dr["% Before"] = Math.Round(((decimal.Parse(dt.Compute("Sum([Total Before Confirmed])", string.Empty).ToString()) - decimal.Parse(dt3.Rows[0]["Total Before Confirmed"].ToString())) / total) * 100, 2);//Total Before Confirmed
                                else
                                    dr[column.ColumnName] = Math.Round(((decimal.Parse(dt.Compute("Sum([" + column.ColumnName + "])", string.Empty).ToString()) - decimal.Parse(dt3.Rows[0][column.ColumnName].ToString())) / total) * 100, 2);

                        }

                        //if (column.ColumnName != "% Before")
                        //{
                        if (column.ColumnName == "Department" || column.ColumnName == "Attendance User")
                            tableHTML += "<td>" + dr[column.ColumnName].ToString() + "</td>";
                        else
                        {
                            if (column.ColumnName == "% Before")
                            {
                                if (dr[column.ColumnName].ToString() == "")
                                    tableHTML += "<td align='right'></td>";
                                else
                                    tableHTML += "<td align='right'>" + String.Format("{0:N2}", Convert.ToDouble(dr[column.ColumnName].ToString())) + "</td>";
                            }

                            else
                            {
                                if (dr[column.ColumnName].ToString() == "")
                                    tableHTML += "<td align='right'></td>";
                                else
                                {
                                    if (dr["Department"].ToString() == "%")
                                        tableHTML += "<td align='right'>" + String.Format("{0:N2}", Convert.ToDouble(dr[column.ColumnName].ToString())) + "</td>";
                                    else
                                        tableHTML += "<td align='right'>" + " " + String.Format("{0}", Convert.ToInt32(dr[column.ColumnName].ToString())) + "</td>";
                                }

                            }

                        }

                        //}

                    }
                    tableHTML += "</tr>";
                    //}

                }
                tableHTML += "</tbody>";
                tableHTML += "</table><br/>";
                #endregion

                #region Table Per Department

                DataTable dtA = repoSummary.GetListFinalConfirmSummaryDataTable(int.Parse(week), int.Parse(DateTime.Parse(monthDate).ToString("MM")), int.Parse(DateTime.Parse(monthDate).ToString("yyyy")), (int)ConstantAccrueStatusHelper.AccrueStatus.WaitingFinalConfirmation);
                string tableHTML2 = "<table border='1' style='border-collapse: collapse'><thead><tr><th rowspan='3' align='center'><b>Department</b></th><th colspan='4' align='center'><b>NEW</b></th><th colspan='4' align='center'><b>RENEWAL</b></th>";
                tableHTML2 += "<th colspan='2' rowspan='2' align='center'><b>TOTAL</b></th> <th rowspan='3' align='center'><b> % </b></th></tr>";
                tableHTML2 += "<tr><th colspan='2' align='center'><b>CURR</b></th><th colspan='2' align='center'><b>OD</b></th><th colspan='2' align='center'><b>CURR</b></th><th colspan='2' align='center'><b>OD</b></th></tr>";
                tableHTML2 += "<tr><th align='center'><b>No Of Site</b></th><th align='center'><b>Amount (Mio)</b></th><th align='center'><b>No Of Site</b></th><th align='center'><b>Amount (Mio)</b></th><th align='center'><b>No Of Site</b></th><th align='center'><b>Amount (Mio)</b></th><th align='center'><b>No Of Site</b></th><th align='center'><b>Amount (Mio)</b></th><th align='center'><b>No Of Site</b></th><th align='center'><b>Amount (Mio)</b></th></tr></thead>";
                tableHTML2 += "<tbody>";
                DataRow newrowA = dtA.NewRow();
                newrowA["SOW"] = "Total After Confirmed";
                dtA.Rows.Add(newrowA);
                int flagTotal = 0;
                foreach (DataRow dr in dtA.Rows)
                {
                    if (dr["SOW"].ToString() == "Total After Confirmed")
                        flagTotal = 1;
                    tableHTML2 += "<tr>";
                    foreach (DataColumn column in dtA.Columns)
                    {
                        column.ReadOnly = false;
                        if (column.ColumnName != "SOW")
                        {
                            if (flagTotal == 1)
                            {
                                dr[column.ColumnName] = dtA.Compute("Sum([" + column.ColumnName + "])", string.Empty);
                            }

                        }
                        if (column.ColumnName != "Week" && column.ColumnName != "Month" && column.ColumnName != "Year")
                        {
                            if (column.ColumnName == "SOW")
                                tableHTML2 += "<td>" + dr[column.ColumnName].ToString() + "</td>";
                            else
                            {
                                if (column.ColumnName.Contains("Percen"))
                                    tableHTML2 += "<td align='right'>" + String.Format("{0:N2}", Convert.ToDouble(dr[column.ColumnName].ToString())) + "</td>";
                                else if (column.ColumnName.Contains("Amount"))
                                    tableHTML2 += "<td align='right'>" + String.Format("{0:N2}", Convert.ToDouble(dr[column.ColumnName].ToString()) / 1000000) + "</td>";
                                else
                                    tableHTML2 += "<td align='right'>" + " " + String.Format("{0}", Convert.ToInt32(dr[column.ColumnName].ToString())) + "</td>";
                            }
                        }

                    }
                    tableHTML2 += "</tr>";
                }
                tableHTML2 += "</table><br/><br/>";

                #endregion
                string strBody = "<b>Dear All,</b> <br/><br/> Berikut summary untuk REPORT ACCRUE REVENUE CLOSING ";
                if (weekGetDate != "5")
                    strBody += "WK " + weekGetDate + " " + DateTime.Now.ToString("MMM yyyy") + " - <b>VERSI ACCOUNTING FINAL";//, cut off konfirmasi by system jam  " + autoConfirmDate.ToString("HH:mm") + ".</b>";
                else
                    strBody += DateTime.Now.ToString("MMM yyyy") + " - <b>VERSI ACCOUNTING FINAL";//, cut off konfirmasi by system jam  " + autoConfirmDate.ToString("HH:mm") + ".</b>";

                strBody += "<br/><br/><center><h3>Movement User Setelah User Konfirmasi Final</h3></center>";
                strBody += "<br/><br/> <b>USER VS USER</b><br/>" + tableHTML;
                strBody += "<br/><br/> <b>ACCRUE PER DEPARTMENT</b><br/>" + tableHTML2;


                //strBody += " apabila ada detail progress disubmit setelah jam " + autoConfirmDate.ToString("HH:mm") + ", <b> Kami anggap Confirmed atas progress site tersebut.</b>";
                //strBody += "<br/><br/>" + namaTo;
                strBody += "<br/><br/><a href='" + linkARSystem + "/Accrue/UserConfirmation'>Link Application</a>";
                strBody += "<br/><br/><br/> <b>Regards,</b><br/> " + namaUserID;


                #endregion
                strWhereClause = "";
                //////////////////////////////
                data = Repo.FinalConfirmBulky(null, (int)ConstantAccrueStatusHelper.AccrueStatus.Done, UserID, int.Parse(week), int.Parse(DateTime.Parse(monthDate).ToString("MM")), int.Parse(DateTime.Parse(monthDate).ToString("yyyy")));
                if (!string.IsNullOrWhiteSpace(monthDate))
                {
                    strWhereClause += "YEAR(CreatedDate) = '" + int.Parse(DateTime.Parse(monthDate).ToString("yyyy")) + "' AND Month(CreatedDate) = '" + int.Parse(DateTime.Parse(monthDate).ToString("MM")) + "' AND ";
                }
                if (!string.IsNullOrWhiteSpace(week))
                {
                    strWhereClause += "Week = '" + int.Parse(week) + "' AND ";
                }
                strWhereClause += "AccrueStatusID = '" + (int)ConstantAccrueStatusHelper.AccrueStatus.Done + "' AND ";
                strWhereClause = !string.IsNullOrWhiteSpace(strWhereClause) ? strWhereClause.Substring(0, strWhereClause.Length - 5) : "";

                listAccrue = Repo.GetList(strWhereClause, "");

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
                    paramLog.AccrueStatusID = (int)ConstantAccrueStatusHelper.AccrueStatus.Done;
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
                    paramLog.Remarks = var.Remarks;
                    paramLog.Week = var.Week;
                    paramLog.IsReConfirm = var.IsReConfirm;
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

                #endregion
                /////////////////////////////////////
                EmailHelper.SendEmail(emailTo, "", "", Subject, strBody, "", UserID);
                uow.SaveChanges();
                return new trxDataAccrue();
            }
            catch (Exception ex)
            {
                uow.Dispose();
                return new trxDataAccrue((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "FinalConfirmationService", "FinalConfirm", UserID));
            }
            finally
            {
                context.Dispose();
                contextHC.Dispose();
            }
        }

        public List<vwtrxDataAccrue> GetUserConfirmedToListExcel(string UserID, vwtrxDataAccrue data, string monthDate, string week, string strOrderBy)
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

                if (data.AccrueStatusID > 0 || data.AccrueStatusID != null)
                {
                    strWhereClause += "AccrueStatusID = '" + data.AccrueStatusID + "' AND ";
                }
                else
                    strWhereClause += "AccrueStatusID = " + (int)ConstantAccrueStatusHelper.AccrueStatus.WaitingFinalConfirmation + " AND ";

                strWhereClause = !string.IsNullOrWhiteSpace(strWhereClause) ? strWhereClause.Substring(0, strWhereClause.Length - 5) : "";

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
            }
        }

        IList<vwAccrueFinalConfirm> GetPage(IList<vwAccrueFinalConfirm> list, int page, int pageSize)
        {
            return list.Skip(page * pageSize).Take(pageSize).ToList();
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
        #endregion
    }
}
