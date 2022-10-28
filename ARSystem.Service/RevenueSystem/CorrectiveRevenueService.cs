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
    public class CorrectiveRevenueService
    {

        public ArCorrectiveRevenuePeriod GetPeriod()
        {
            var context = new DbContext(Helper.GetConnection("ARSystemDWH"));

            var result = new ArCorrectiveRevenuePeriod();
            try
            {
                var repo = new ArCorrectiveRevenuePeriodRepository(context);
                result = repo.GetPeriod();
            }
            catch (Exception ex)
            {
                result.ErrorMessage = Helper.logError(ex.Message.ToString(), "CorrectiveRevenueService", "GetPeriod", "");
                result.ErrorType = (int)Helper.ErrorType.Error;

            }
            finally
            {
                context.Dispose();
            }
            return result;
        }

        public ArCorrectiveRevenueTemp GenerateData(List<ArCorrectiveRevenueTemp> data)
        {
            var context = new DbContext(Helper.GetConnection("ARSystemDWH"));

            var result = new ArCorrectiveRevenueTemp();
            try
            {
                var repo = new ArCorrectiveRevenueTempRepository(context);
                repo.CreateBulky(data);
            }
            catch (Exception ex)
            {
                string error = Helper.logError(ex.Message.ToString(), "CorrectiveRevenueService", "GenerateData", "");
                if (ex.Message.ToLower().Contains("duplicate"))
                {
                    result.ErrorMessage = "Please cek data, because there are duplicate data";
                    result.ErrorType = 2;
                }
                else
                {
                    result.ErrorMessage = error;
                    result.ErrorType = 1;
                }


            }
            finally
            {
                context.Dispose();
            }
            return result;
        }

        //public ArCorrectiveRevenueTemp ProcessData(List<ArCorrectiveRevenueFinalTemp> data)
        //{
        //    var context = new DbContext(Helper.GetConnection("ARSystemDWH"));

        //    var result = new ArCorrectiveRevenueTemp();
        //    try
        //    {
        //        var repo = new ArCorrectiveRevenueFinalTempRepository(context);
        //        repo.ProcessData(data);
        //    }
        //    catch (Exception ex)
        //    {
        //        result.ErrorMessage = Helper.logError(ex.Message.ToString(), "CorrectiveRevenueService", "GenerateData", "");
        //        result.ErrorType = (int)Helper.ErrorType.Error;

        //    }
        //    finally
        //    {
        //        context.Dispose();
        //    }
        //    return result;
        //}

        //public List<vwArCorrectiveRevenue> GetDataGenerated(string UserId, int rowSkip, int pageSize)
        //{
        //    var context = new DbContext(Helper.GetConnection("ARSystemDWH"));

        //    var result = new List<vwArCorrectiveRevenue>();
        //    try
        //    {

        //        var repo = new vwArCorrectiveRevenueRepository(context);
        //        string whereClause = "CreatedBy='" + UserId + "'";
        //        if (pageSize > 0)
        //            result = repo.GetPaged(whereClause, "", rowSkip, pageSize);
        //        else
        //            result = repo.GetList(whereClause, "");


        //    }
        //    catch (Exception ex)
        //    {
        //        result.Add(new vwArCorrectiveRevenue
        //        {
        //            ErrorMessage = Helper.logError(ex.Message.ToString(), "CorrectiveRevenueService", "GetDataGenerated", ""),
        //            ErrorType = (int)Helper.ErrorType.Error
        //        });

        //    }
        //    finally
        //    {
        //        context.Dispose();
        //    }
        //    return result;
        //}

        public List<ArCorrectiveRevenueFinalTemp> GetDataGenerated(string UserId)
        {
            var context = new DbContext(Helper.GetConnection("ARSystemDWH"));

            var result = new List<ArCorrectiveRevenueFinalTemp>();
            try
            {

                var repo = new ArCorrectiveRevenueFinalTempRepository(context);
                string whereClause = "CreatedBy='" + UserId + "'";

                result = repo.GetList(whereClause, "");


            }
            catch (Exception ex)
            {
                result.Add(new ArCorrectiveRevenueFinalTemp
                {
                    ErrorMessage = Helper.logError(ex.Message.ToString(), "CorrectiveRevenueService", "GetDataGenerated", ""),
                    ErrorType = (int)Helper.ErrorType.Error
                });

            }
            finally
            {
                context.Dispose();
            }
            return result;
        }

        //public int GetCountDataGenerated(string UserId)
        //{
        //    var context = new DbContext(Helper.GetConnection("ARSystemDWH"));

        //    var result = new List<vwArCorrectiveRevenue>();
        //    try
        //    {

        //        var repo = new vwArCorrectiveRevenueRepository(context);
        //        string whereClause = "CreatedBy='" + UserId + "'";
        //        return repo.GetCount(whereClause);

        //    }
        //    catch (Exception ex)
        //    {
        //        return 0;

        //    }
        //    finally
        //    {
        //        context.Dispose();
        //    }

        //}

        public ArCorrectiveRevenueTemp ProcessData(string userId)
        {
            var context = new DbContext(Helper.GetConnection("ARSystemDWH"));

            var result = new ArCorrectiveRevenueTemp();
            try
            {
                var repo = new ArCorrectiveRevenueFinalTempRepository(context);
                string message = repo.ProcessData(userId);
                if (message != "")
                {
                    result.ErrorMessage = Helper.logError(message, "CorrectiveRevenueService", "ProcessData", "");
                    result.ErrorType = (int)Helper.ErrorType.Error;
                }

            }
            catch (Exception ex)
            {
                result.ErrorMessage = Helper.logError(ex.Message.ToString(), "CorrectiveRevenueService", "ProcessData", "");
                result.ErrorType = (int)Helper.ErrorType.Error;
            }
            finally
            {
                context.Dispose();
            }
            return result;
        }

        public ArCorrectiveRevenueTemp Delete(List<ArCorrectiveRevenueFinalTemp> data, bool isDeleteAll, string userId)
        {
            var context = new DbContext(Helper.GetConnection("ARSystemDWH"));

            var result = new ArCorrectiveRevenueTemp();
            try
            {
                var repo = new ArCorrectiveRevenueFinalTempRepository(context);
                repo.DeleteBulky(data, isDeleteAll, userId);
            }
            catch (Exception ex)
            {
                result.ErrorMessage = Helper.logError(ex.Message.ToString(), "CorrectiveRevenueService", "Delete", "");
                result.ErrorType = (int)Helper.ErrorType.Error;

            }
            finally
            {
                context.Dispose();
            }
            return result;
        }
    }
}
