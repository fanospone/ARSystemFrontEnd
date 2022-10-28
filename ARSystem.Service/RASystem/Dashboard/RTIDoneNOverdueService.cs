using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ARSystem.Domain.Models;
using ARSystem.Domain.Repositories;
using ARSystem.Domain.DAL;

using System.Threading.Tasks;

namespace ARSystem.Service
{
    public class RTIDoneNOverdueService
    {
        #region public methode
        public async Task<vwRTINOverdueModel> GetDataChart(int Year, string customerid, string userID, string groupBy)
        {
            return await pGetDataChart(Year, customerid, userID, groupBy);
        }

        public async Task<List<vwRTINOverdueModel>> GetDataChartByGroup(int Year, string customerid, string userID, string groupby)
        {
            return await pGetDataChartByGroup(Year, customerid, userID, groupby);
        }


        public async Task<List<RTINOverdueDetailModel>> GetDataDetail(string DataType, int Year, int Month, string CustomerID, int RowSkip, int PageSize, string userID)
        {
            return await pGetDataDetail(DataType, Year, Month, CustomerID, RowSkip, PageSize, userID);
        }

        public int GetCountDataDetail(string DataType, int Year, int Month, string CustomerID)
        {
            return pGetCountDataDetail(DataType, Year, Month, CustomerID);
        }
        #endregion

        #region private methode
        private async Task<List<vwRTINOverdueModel>> pGetDataChartByGroup(int Year, string customerid, string userID, string groupby)
        {
            var context = new DbContext(Helper.GetConnection("ARSystemDWH"));
            var repo2 = new RTIDoneNOverdueByGroupRepository(context);
            List<vwRTINOverdueModel> vwModel = new List<vwRTINOverdueModel>();
            try
            {
                
                vwModel = await repo2.GetChartDataByGroup(Year, customerid, groupby);

                return vwModel;
            }
            catch (Exception ex)
            {
                vwModel.Add(new vwRTINOverdueModel((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "RTIDoneNOverdueService", "pGetDataChart", userID)));
                return vwModel;

            }
            finally
            {
                context.Dispose();
            }
        }
        #endregion

        #region private methode
        private async Task<vwRTINOverdueModel> pGetDataChart(int Year, string customerid, string userID, string groupBy)
        {
            var context = new DbContext(Helper.GetConnection("ARSystemDWH"));
            var repo = new RTIDoneNOverdueRepository(context);  
            //var repo2 = new RTIDoneNOverdueByStatusRepository(context);
            var repo2 = new RTIDoneNOverdueByGroupRepository(context);
            List<RTINOverdueModel> dataRTIList = new List<RTINOverdueModel>();
            vwRTINOverdueModel vwModel = new vwRTINOverdueModel();
            List<SummaryData> summaryDataList = new List<SummaryData>();
            List<vwRTINOverdueModel> vwModelList = new List<vwRTINOverdueModel>();
            try
            {

              
                //List<Operator> sumList = new List<Operator>();

                dataRTIList = await repo.GetChartData(Year, customerid);
                vwModel.dataChart = dataRTIList;

                vwModelList =await repo2.GetChartDataByGroup(Year, customerid, groupBy);
                foreach (var item in vwModelList)
                {
                    summaryDataList.Add(new SummaryData { CustomerID = item.CustomerID, NearlyOverDue = item.NearlyOverDue, RTI = item.RTI, OverDue = item.OverDue });
                }
                vwModel.vwSumData = summaryDataList;

                //vwModel.RTI = dataRTIList.Sum(x => x.RTI);
                //vwModel.OverDue = dataRTIList.Sum(x => x.OverDue);
                //vwModel.NearlyOverDue = dataRTIList.Sum(x => x.NearlyOverDue);

                //string month = GetMontName((System.DateTime.Now.Month) - 1);
                //string month2 = GetMontName((System.DateTime.Now.Month) - 2);

                //  vwModel.RTINearly = dataRTIList.Where(x => x.Month == month || x.Month == month2).Sum(x => x.RTI);



                //sumList = await repo2.GetChartDataByStatus(Year, customerid);
                //var data = sumList.Where(x => x.Status.ToLower() == "rti").Select(x => new Operator { CustomerID = x.CustomerID, Amount = x.Amount }).ToList();

                //vwModel.vwSumData.Add(new SummaryData
                //{
                //    Status = "RTI",
                //    TotalAmount = vwModel.RTI,
                //    Operators = data
                //});

                //data = sumList.Where(x => x.Status.ToLower() == "overdue").Select(x => new Operator { CustomerID = x.CustomerID, Amount = x.Amount }).ToList();
                //vwModel.vwSumData.Add(new SummaryData
                //{
                //    Status = "Overdue",
                //    TotalAmount = vwModel.OverDue,
                //    Operators = data
                //});

                //data = sumList.Where(x => x.Status.ToLower() == "nearlyoverdue").Select(x => new Operator { CustomerID = x.CustomerID, Amount = x.Amount }).ToList();
                //vwModel.vwSumData.Add(new SummaryData
                //{
                //    Status = "NearlyOverDue",
                //    TotalAmount = vwModel.NearlyOverDue,
                //    Operators = data
                //});


                return vwModel;
            }
            catch (Exception ex)
            {
                return new vwRTINOverdueModel((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "RTIDoneNOverdueService", "pGetDataChart", userID));

            }
            finally
            {
                context.Dispose();
            }
        }

        private async Task<List<RTINOverdueDetailModel>> pGetDataDetail(string DataType, int Year, int Month, string CustomerID, int RowSkip, int PageSize, string userID)
        {
            var context = new DbContext(Helper.GetConnection("ARSystemDWH"));
            var repo = new RTIDoneNOverdueDetailRepository(context);
            List<RTINOverdueDetailModel> list = new List<RTINOverdueDetailModel>();
            try
            {
                if (PageSize > 0)
                    list = await repo.GetPaged(DataType, Year, Month, CustomerID, RowSkip, PageSize);
                else
                    list = await repo.GetList(DataType, Year, Month, CustomerID);

                return list;
            }
            catch (Exception ex)
            {
                list.Add(new RTINOverdueDetailModel((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "RTIDoneNOverdueService", "pGetDataChart", userID)));
                return list;
            }
            finally
            {
                context.Dispose();
            }
        }


        private int pGetCountDataDetail(string DataType, int Year, int Month, string CustomerID)
        {
            var context = new DbContext(Helper.GetConnection("ARSystemDWH"));
            var repo = new RTIDoneNOverdueDetailRepository(context);

            try
            {
                return repo.GetCount(DataType, Year, Month, CustomerID);
            }
            catch (Exception ex)
            {
                return 0;
            }
            finally
            {
                context.Dispose();
            }
        }

        protected string GetMontName(int name)
        {
            string[] mlist = { "JAN", "FEB", "MAR", "APR", "MEI", "JUN", "JUL", "AUG", "SEP", "OCT", "NOV", "DEC" };
            return mlist[name].ToString();
        }

        #endregion

    }
}
