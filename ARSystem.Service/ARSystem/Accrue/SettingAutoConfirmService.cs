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
    public class SettingAutoConfirmService
    {
        public int GetSettingAutoConfirmToListCount(string UserID, vwmstAccrueSettingAutoConfirm data, string monthDate, string week)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var repo = new vwmstAccrueSettingAutoConfirmRepository(context);

            try
            {
                string strWhereClause = "";
                if (!string.IsNullOrWhiteSpace(monthDate))
                {
                    strWhereClause += "YEAR(Period) = '" + int.Parse(DateTime.Parse(monthDate).ToString("yyyy")) + "' AND Month(Period) = '" + int.Parse(DateTime.Parse(monthDate).ToString("MM")) + "' AND ";
                }
                if (!string.IsNullOrWhiteSpace(week))
                {
                    strWhereClause += "Week = '" + int.Parse(week) + "' AND ";
                }

                if (data.AccrueStatusID > 0)
                {
                    strWhereClause += "AccrueStatusID = '" + data.AccrueStatusID + "' AND ";
                }


                strWhereClause = !string.IsNullOrWhiteSpace(strWhereClause) ? strWhereClause.Substring(0, strWhereClause.Length - 5) : "";

                return repo.GetCount(strWhereClause);
            }
            catch (Exception ex)
            {
                Helper.logError(ex.Message.ToString(), "SettingAutoConfirmService", "GetSettingAutoConfirmToListCount", UserID);
                return 0;
            }
            finally
            {
                context.Dispose();
            }
        }

        public List<vwmstAccrueSettingAutoConfirm> GetSettingAutoConfirmToList(string UserID, vwmstAccrueSettingAutoConfirm data, string monthDate, string week, string strOrderBy, int intRowStart = 0, int intPageSize = 0)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var Repo = new vwmstAccrueSettingAutoConfirmRepository(context);
            List<vwmstAccrueSettingAutoConfirm> list = new List<vwmstAccrueSettingAutoConfirm>();

            try
            {

                string strWhereClause = "";
                if (!string.IsNullOrWhiteSpace(monthDate))
                {
                    strWhereClause += "YEAR(CreatedDate) = '" + int.Parse(DateTime.Parse(monthDate).ToString("yyyy")) + "' AND Month(CreatedDate) = '" + int.Parse(DateTime.Parse(monthDate).ToString("MM")) + "' AND ";
                }
                if (!string.IsNullOrWhiteSpace(week))
                {
                    if (week != "null")
                        strWhereClause += "Week = '" + int.Parse(week) + "' AND ";
                }

                if (data.AccrueStatusID > 0)
                {
                    strWhereClause += "AccrueStatusID = '" + data.AccrueStatusID + "' AND ";
                }

                strWhereClause = !string.IsNullOrWhiteSpace(strWhereClause) ? strWhereClause.Substring(0, strWhereClause.Length - 5) : "";
                if (string.IsNullOrWhiteSpace(strOrderBy))
                {
                    strOrderBy = "UpdatedDate DESC";
                }
                if (intPageSize > 0)
                    list = Repo.GetPaged(strWhereClause, strOrderBy, intRowStart, intPageSize);
                else
                    list = Repo.GetList(strWhereClause, strOrderBy);


                return list;
            }
            catch (Exception ex)
            {
                list.Add(new vwmstAccrueSettingAutoConfirm((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "SettingAutoConfirmService", "GetSettingAutoConfirmToList", UserID)));
                return list;
            }
            finally
            {
                context.Dispose();
            }
        }

        public mstAccrueSettingAutoConfirm Save(string UserID, vwmstAccrueSettingAutoConfirm data, string monthDate, string week, string autoConfirmDate)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var Repo = new mstAccrueSettingAutoConfirmRepository(context);
            var uow = context.CreateUnitOfWork();
            mstAccrueSettingAutoConfirm param = new mstAccrueSettingAutoConfirm();

            try
            {
                string strWhereClause = "";
                strWhereClause += "YEAR(Period) = '" + int.Parse(DateTime.Parse(monthDate).ToString("yyyy")) + "' AND Month(Period) = '" + int.Parse(DateTime.Parse(monthDate).ToString("MM")) + "' AND ";
                strWhereClause += "Week = '" + int.Parse(week) + "' AND ";
                strWhereClause += "AccrueStatusID = '" + data.AccrueStatusID + "' AND IsActive = 1";
                if (Repo.GetCount(strWhereClause) > 0)//Update
                {
                    mstAccrueSettingAutoConfirm paramOld = new mstAccrueSettingAutoConfirm();
                    paramOld = Repo.GetList(strWhereClause, "").FirstOrDefault();
                    param.ID = paramOld.ID;
                    param.Week = Convert.ToInt32(week);
                    param.Period = DateTime.Parse(monthDate);
                    param.AccrueStatusID = data.AccrueStatusID;
                    param.AutoConfirmDate = DateTime.Parse(autoConfirmDate);
                    param.CreatedBy = paramOld.CreatedBy;
                    param.CreatedDate = paramOld.CreatedDate;
                    param.UpdatedBy = UserID;
                    param.UpdatedDate = DateTime.Now;
                    param.IsActive = true;
                    Repo.Update(param);
                }
                else//Insert New
                {
                    param.Week = Convert.ToInt32(week);
                    param.Period = DateTime.Parse(monthDate);
                    param.AccrueStatusID = data.AccrueStatusID;
                    param.AutoConfirmDate = DateTime.Parse(autoConfirmDate);
                    param.CreatedBy = UserID;
                    param.CreatedDate = DateTime.Now;
                    param.UpdatedBy = UserID;
                    param.UpdatedDate = DateTime.Now;
                    param.IsActive = true;
                    Repo.Create(param);
                }


                uow.SaveChanges();
                return new mstAccrueSettingAutoConfirm();
            }
            catch (Exception ex)
            {
                uow.Dispose();
                return new mstAccrueSettingAutoConfirm((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "SettingAutoConfirmService", "Save", UserID));
            }
            finally
            {
                context.Dispose();

            }
        }

        public mstAccrueSettingAutoConfirm Delete(string UserID, vwmstAccrueSettingAutoConfirm data)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var Repo = new mstAccrueSettingAutoConfirmRepository(context);
            var uow = context.CreateUnitOfWork();
            mstAccrueSettingAutoConfirm param = new mstAccrueSettingAutoConfirm();
            try
            {
                param = Repo.GetByPK(data.ID);
                param.Week = param.Week;
                param.Period = param.Period;
                param.AccrueStatusID = param.AccrueStatusID;
                param.AutoConfirmDate = param.AutoConfirmDate;
                param.UpdatedBy = UserID;
                param.UpdatedDate = DateTime.Now;
                param.IsActive = false;
                Repo.Update(param);

                uow.SaveChanges();
                return new mstAccrueSettingAutoConfirm();
            }
            catch (Exception ex)
            {
                uow.Dispose();
                return new mstAccrueSettingAutoConfirm((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "SettingAutoConfirmService", "Delete", UserID));
            }
            finally
            {
                context.Dispose();

            }
        }
    }
}
