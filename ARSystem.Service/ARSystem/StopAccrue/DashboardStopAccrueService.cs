using ARSystem.Domain.DAL;
using ARSystem.Domain.Models;
using ARSystem.Domain.Repositories;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TBGFramework.HRData;

namespace ARSystem.Service
{
    public class DashboardStopAccrueService
    {
        public List<MstInitialDepartment> GetDirectorate()
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            List<MstInitialDepartment> data = new List<MstInitialDepartment>();

            try
            {
                var repo = new MstInitialDepartmentRepository(context);
                data = repo.GetDirectorate();
            }
            catch (Exception ex)
            {
                context.Dispose();
                data.Add(new MstInitialDepartment { ErrorMessage = ex.Message });
            }
            return data;
        }

        public int pGetCountDashboardHeader(vwStopAccrueDashboardHeader param)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));

            try
            {
                var data = new List<vwStopAccrueDashboardHeader>();
                var repo = new vwStopAccrueDashboardHeaderRepository(context);


                //data = repo.GetList(WhereClauseDashboardHeader(param), "ID DESC");
                if (param.DetailCase == "1")
                {
                    data = repo.GetListDashboardHeader(param.SubmissionDateFrom, param.SubmissionDateTo, param.DepartName, 1, param.DetailCase, param.AccrueType, param.DirectorateCode, 0,0,param.RequestNumber);
                }
                else
                {
                    data = repo.GetListDashboardHeader(param.SubmissionDateFrom, param.SubmissionDateTo, param.DepartName, 2, param.DetailCase, param.AccrueType, param.DirectorateCode, 0, 0, param.RequestNumber);
                }

                var contextTBGSys = new DbContext(Helper.GetConnection("TBGFlow"));
                var wfHeaders = new List<WfHeaders_View>();
                    var repoTBGSys = new WfHeaders_ViewRepository(contextTBGSys);
                var vwStopAccrueHeader = new vwStopAccrueHeader();
                vwStopAccrueHeader.ActivityID = param.ActivityID;

                string whereclause = " Code = 'STOP_ACCRUE' AND Status IN('inp','don1') ";
                whereclause += " AND " + (param.Activity.ToLower() == "finish" ? "Label='SA_STOP'" : "Label in ('SA_APPR_DIV','SA_APPR_DEPT_ACC','SA_APPR_DIV_ACC','SA_FEED_DIV_ACC','SA_FEED_DEPT_ACC','SA_SUBMIT_DOC','SA_DOC_RECEIVE_DHAC','SA_DOC_RECEIVE_DHAS','SA_APPR_DIV_CHIEF','SA_APPR_CHIEF_MKT')");

                wfHeaders = repoTBGSys.GetList(whereclause);

                var datalist = (from request in data
                                join wfHeader in wfHeaders on request.AppHeaderID equals wfHeader.AppHeaderID
                                select new
                                {
                                    wfHeader.AppHeaderID
                                    //request.AppHeaderID
                                }).OrderByDescending(x => x.AppHeaderID).ToList();

                return datalist.Count();
                //return repo.GetCount(WhereClauseDashboardHeader(param));
            }
            catch (Exception ex)
            {
                context.Dispose();
                return 0;
            }
        }

        public List<vwStopAccrueDashboardHeader> GetDashboardHeader(vwStopAccrueDashboardHeader param, int rowSkip, int pageSize)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            List<vwStopAccrueDashboardHeader> data = new List<vwStopAccrueDashboardHeader>();
            try
            {
                var repo = new vwStopAccrueDashboardHeaderRepository(context);
                //if (pageSize > 0)
                //    data = repo.GetPaged(WhereClauseDashboardHeader(param), "ID DESC", rowSkip, pageSize);
                //else
                if (param.DetailCase == "1")
                {
                    data = repo.GetListDashboardHeader(param.SubmissionDateFrom, param.SubmissionDateTo, param.DepartName, 1, param.DetailCase, param.AccrueType, param.DirectorateCode, rowSkip, pageSize, param.RequestNumber);
                }
                else
                {
                    data = repo.GetListDashboardHeader(param.SubmissionDateFrom, param.SubmissionDateTo, param.DepartName, 2, param.DetailCase, param.AccrueType, param.DirectorateCode, rowSkip, pageSize, param.RequestNumber);
                }


                var contextTBGSys = new DbContext(Helper.GetConnection("TBGFlow"));
                var wfHeaders = new List<WfHeaders_View>();
                var repoTBGSys = new WfHeaders_ViewRepository(contextTBGSys);

                string whereclause = "";
                if (param.Activity == "ongoing")
                {
                    var vwStopAccrueHeader = new vwStopAccrueHeader();
                    vwStopAccrueHeader.ActivityID = param.ActivityID;
                    whereclause = " Code = 'STOP_ACCRUE' AND Status IN('inp','don1') ";
                    whereclause += " AND " + (param.Activity.ToLower() == "finish" ? "Label='SA_STOP'" : "Label in ('SA_APPR_DIV','SA_APPR_DEPT_ACC','SA_APPR_DIV_ACC','SA_FEED_DIV_ACC','SA_FEED_DEPT_ACC','SA_SUBMIT_DOC','SA_DOC_RECEIVE_DHAC','SA_DOC_RECEIVE_DHAS','SA_APPR_DIV_CHIEF','SA_APPR_CHIEF_MKT')");
                    wfHeaders = repoTBGSys.GetList(whereclause);

                    var datalist = (from request in data
                                    join wfHeader in wfHeaders on request.AppHeaderID equals wfHeader.AppHeaderID
                                    select new
                                    {
                                        request.ID,
                                        wfHeader.AppHeaderID,
                                        request.RequestNumber,
                                        request.DepartName,
                                        request.SoNumberCount,
                                        request.SumRevenue,
                                        request.SumCapex,
                                        request.RequestTypeID,
                                        request.CraetedDate,
                                        request.RequestStatus,
                                        request.FileName,
                                        wfHeader.Activity,
                                        request.AccrueType,
                                        request.IsReHold,
                                        wfHeader.LastModifiedDate

                                    }).OrderByDescending(x => x.AppHeaderID).ToArray();
                    int indexRow = rowSkip + 1;
                    if (pageSize != 0)
                        datalist = datalist.Skip(rowSkip).Take(pageSize).ToArray();

                    data = new List<vwStopAccrueDashboardHeader>();
                    foreach (var item in datalist)
                    {
                        data.Add(new vwStopAccrueDashboardHeader
                        {
                            RowIndex = indexRow.ToString(),
                            ID = item.ID,
                            AppHeaderID = item.AppHeaderID,
                            RequestNumber = item.RequestNumber,
                            DepartName = item.DepartName,
                            SoNumberCount = item.SoNumberCount,
                            SumRevenue = item.SumRevenue,
                            SumCapex = item.SumCapex,
                            RequestTypeID = item.RequestTypeID,
                            CraetedDate = item.CraetedDate,
                            ReqeustStatus = item.RequestStatus,
                            FileName = item.FileName,
                            Activity = item.Activity,
                            AccrueType = item.AccrueType,
                            IsReHold = item.IsReHold,
                            LastModifiedDate = item.LastModifiedDate
                        });
                        indexRow++;
                    }
                    var dataSum = new vwStopAccrueDashboardHeader();
                    dataSum.CountPaging = data.Count();
                    dataSum.RowIndex = "Total";  //"";
                    dataSum.RequestNumber = "";
                    dataSum.DepartName = "";
                    dataSum.SoNumberCount = 0; //data.Sum(x => x.SoNumberCount);
                    dataSum.SumRevenue = 0; //data.Sum(x => x.SumRevenue);
                    dataSum.SumCapex = 0; //data.Sum(x => x.SumCapex);
                    dataSum.RequestTypeID = 0;
                    dataSum.CraetedDate = null;

                    data.Add(dataSum);
                } else
                {
                    var vwStopAccrueHeader = new vwStopAccrueHeader();
                    vwStopAccrueHeader.ActivityID = param.ActivityID;
                    whereclause = " Code = 'STOP_ACCRUE' AND Status IN('inp','don1') ";
                    whereclause += " AND " + (param.Activity.ToLower() == "finish" ? "Label='SA_STOP'" : "Label in ('SA_APPR_DIV','SA_APPR_DEPT_ACC','SA_APPR_DIV_ACC','SA_FEED_DIV_ACC','SA_FEED_DEPT_ACC','SA_SUBMIT_DOC','SA_DOC_RECEIVE_DHAC','SA_DOC_RECEIVE_DHAS','SA_APPR_DIV_CHIEF','SA_APPR_CHIEF_MKT')");
                    wfHeaders = repoTBGSys.GetList(whereclause);

                    var datalist = (from request in data
                                    join wfHeader in wfHeaders on request.AppHeaderID equals wfHeader.AppHeaderID
                                    select new
                                    {
                                        request.ID,
                                        wfHeader.AppHeaderID,
                                        request.RequestNumber,
                                        request.DepartName,
                                        request.SoNumberCount,
                                        request.SumRevenue,
                                        request.SumCapex,
                                        request.RequestTypeID,
                                        request.CraetedDate,
                                        request.RequestStatus,
                                        request.FileName,
                                        wfHeader.Activity,
                                        request.AccrueType,
                                        request.IsReHold,
                                        wfHeader.LastModifiedDate

                                    }).OrderByDescending(x => x.AppHeaderID).ToArray();
                        int indexRow = rowSkip + 1;
                        if (pageSize != 0)
                            datalist = datalist.Skip(rowSkip).Take(pageSize).ToArray();

                        if (datalist.Length > 0)
                        {
                            data = new List<vwStopAccrueDashboardHeader>();
                            foreach (var item in datalist)
                            {
                                data.Add(new vwStopAccrueDashboardHeader
                                {
                                    RowIndex = indexRow.ToString(),
                                    ID = item.ID,
                                    AppHeaderID = item.AppHeaderID,
                                    RequestNumber = item.RequestNumber,
                                    DepartName = item.DepartName,
                                    SoNumberCount = item.SoNumberCount,
                                    SumRevenue = item.SumRevenue,
                                    SumCapex = item.SumCapex,
                                    RequestTypeID = item.RequestTypeID,
                                    CraetedDate = item.CraetedDate,
                                    ReqeustStatus = item.RequestStatus,
                                    FileName = item.FileName,
                                    Activity = item.Activity,
                                    AccrueType = item.AccrueType,
                                    IsReHold = item.IsReHold,
                                    LastModifiedDate = item.LastModifiedDate
                                });
                                indexRow++;
                            }
                            var dataSum = new vwStopAccrueDashboardHeader();
                            dataSum.CountPaging = data.Count();
                            dataSum.RowIndex = "Total"; //"";
                        dataSum.RequestNumber = "";
                            dataSum.DepartName = "";
                        dataSum.SoNumberCount = 0; //data.Sum(x => x.SoNumberCount);
                        dataSum.SumRevenue = 0; //data.Sum(x => x.SumRevenue);
                        dataSum.SumCapex = 0; //data.Sum(x => x.SumCapex);
                        dataSum.RequestTypeID = 0;
                            dataSum.CraetedDate = null;

                            data.Add(dataSum);          
                    } else {
                        int indexRowManual = rowSkip + 1;
                        var datalistManual = new List<vwStopAccrueDashboardHeader>();
                        foreach (var item in data)
                        {
                            datalistManual.Add(new vwStopAccrueDashboardHeader
                            {
                                RowIndex = indexRowManual.ToString(),
                                ID = item.ID,
                                AppHeaderID = item.AppHeaderID,
                                RequestNumber = item.RequestNumber,
                                DepartName = item.DepartName,
                                SoNumberCount = item.SoNumberCount,
                                SumRevenue = item.SumRevenue,
                                SumCapex = item.SumCapex,
                                RequestTypeID = item.RequestTypeID,
                                CraetedDate = item.CraetedDate,
                                ReqeustStatus = item.RequestStatus,
                                FileName = item.FileName,
                                Activity = "Finish - Manual",
                                AccrueType = item.AccrueType,
                                IsReHold = item.IsReHold,
                                LastModifiedDate = item.LastModifiedDate
                            });
                            indexRowManual++;
                        }
                        data = datalistManual;
                    }         
                }
            }
            catch (Exception ex)
            {
                context.Dispose();
                data.Add(new vwStopAccrueDashboardHeader { ErrorMessage = ex.Message });
            }

            return data;
        }

        public List<vwStopAccrueDashboardDetail> GetDashboardDetailList(vwStopAccrueDashboardDetail param, int rowSkip, int pageSize)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            List<vwStopAccrueDashboardDetail> data = new List<vwStopAccrueDashboardDetail>();
            try
            {
                var repo = new vwStopAccrueDashboardDetailRepository(context);
                data = repo.GetListdashboardDetail(param.TrxStopAccrueHeaderID, rowSkip, pageSize, param.DetailCase, param.DepartName, param.DeptOrDetailCase, param.RequestNumber);

                var hrData = new Employee();
                //var hrDataResulut = hrData.GetDetail(data.Select(x => x.Initiator).FirstOrDefault());
                int indexRow = rowSkip + 1;
                indexRow++;
                
                data = data.Select(c => { c.Initiator = hrData.FullName; return c; }).ToList();

                //var dataSum = new vwStopAccrueDashboardDetail();
                //dataSum.RowIndex = "Total";
                //dataSum.CapexAmount = data.Sum(x => x.CapexAmount);
                //dataSum.RevenueAmount = data.Sum(x => x.RevenueAmount);
                //dataSum.CompensationAmount = data.Sum(x => x.CompensationAmount);
                //data.Add(dataSum);
            }
            catch (Exception ex)
            {
                context.Dispose();
                data.Add(new vwStopAccrueDashboardDetail { ErrorMessage = ex.Message });
            }

            return data;
        }

        public List<vwStopAccrueDashboardHeader> GetCountData(vwStopAccrueDashboardHeader param, int rowSkip, int pageSize)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            List<vwStopAccrueDashboardHeader> data = new List<vwStopAccrueDashboardHeader>();
            try
            {
                var repo = new vwStopAccrueDashboardHeaderRepository(context);

                var contextTBGSys = new DbContext(Helper.GetConnection("TBGFlow"));
                var wfHeaders = new List<WfHeaders_View>();
                var repoTBGSys = new WfHeaders_ViewRepository(contextTBGSys);

                var vwStopAccrueHeader = new vwStopAccrueHeader();
                vwStopAccrueHeader.ActivityID = param.ActivityID;
                string whereclause = " Code = 'STOP_ACCRUE' AND Status IN('inp','don1') ";
                whereclause += " AND " + (param.Activity.ToLower() == "finish" ? "Label='SA_STOP'" : "Label in ('SA_APPR_DIV','SA_APPR_DEPT_ACC','SA_APPR_DIV_ACC','SA_FEED_DIV_ACC','SA_FEED_DEPT_ACC','SA_SUBMIT_DOC','SA_DOC_RECEIVE_DHAC','SA_DOC_RECEIVE_DHAS','SA_APPR_DIV_CHIEF','SA_APPR_CHIEF_MKT')");
                wfHeaders = repoTBGSys.GetList(whereclause);

                var appHeader = new ArrayList();
                foreach (var appHeaderID in wfHeaders)
                {
                    appHeader.Add(appHeaderID.AppHeaderID);
                }

                int i = 0;
                string split = "";
                for (i = 0; i < appHeader.Count; i++)
                {
                    split += "'" + appHeader[i].ToString() + "'" + ",";
                }

                if (param.DepartName == "Department")
                {
                    data = repo.GetCountByDepartment(param.SubmissionDateFrom, param.SubmissionDateTo, param.DirectorateCode, "HOLD", split);
                    //var sum = data.Sum(x => x.CountData);
                }
                else
                {
                    data = repo.GetCountByDetailCase(param.SubmissionDateFrom, param.SubmissionDateTo, param.DirectorateCode, "HOLD", split);
                }

                //data = new List<vwStopAccrueDashboardHeader>();


            }
            catch (Exception ex)
            {
                context.Dispose();
                data.Add(new vwStopAccrueDashboardHeader { ErrorMessage = ex.Message });
            }

            return data;
        }

        public List<vwStopAccrueDashboardHeader> GetCountDataSTOP(vwStopAccrueDashboardHeader param, int rowSkip, int pageSize)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            List<vwStopAccrueDashboardHeader> data = new List<vwStopAccrueDashboardHeader>();
            try
            {
                var repo = new vwStopAccrueDashboardHeaderRepository(context);
                var contextTBGSys = new DbContext(Helper.GetConnection("TBGFlow"));
                var wfHeaders = new List<WfHeaders_View>();
                var repoTBGSys = new WfHeaders_ViewRepository(contextTBGSys);

                var vwStopAccrueHeader = new vwStopAccrueHeader();
                vwStopAccrueHeader.ActivityID = param.ActivityID;
                string whereclause = " Code = 'STOP_ACCRUE' AND Status IN('inp','don1') ";
                whereclause += " AND " + (param.Activity.ToLower() == "finish" ? "Label='SA_STOP'" : "Label in ('SA_APPR_DIV','SA_APPR_DEPT_ACC','SA_APPR_DIV_ACC','SA_FEED_DIV_ACC','SA_FEED_DEPT_ACC','SA_SUBMIT_DOC','SA_DOC_RECEIVE_DHAC','SA_DOC_RECEIVE_DHAS','SA_APPR_DIV_CHIEF','SA_APPR_CHIEF_MKT')");
                wfHeaders = repoTBGSys.GetList(whereclause);

                var appHeader = new ArrayList();
                foreach (var appHeaderID in wfHeaders)
                {
                    appHeader.Add(appHeaderID.AppHeaderID);
                }


                int i = 0;
                string split = "";
                //var splitConv = 0;
                for (i = 0; i < appHeader.Count; i++)
                {
                    split += appHeader[i].ToString() + ",";
                    //split += split;
                    //splitConv = Convert.ToInt32(split);
                    //i = i + 1;
                }

                if (param.DepartName == "Department")
                {
                    data = repo.GetCountByDepartment(param.SubmissionDateFrom, param.SubmissionDateTo, param.DirectorateCode, "STOP", split);
                }
                else
                {
                    data = repo.GetCountByDetailCase(param.SubmissionDateFrom, param.SubmissionDateTo, param.DirectorateCode, "STOP", split);
                }



            }
            catch (Exception ex)
            {
                context.Dispose();
                data.Add(new vwStopAccrueDashboardHeader { ErrorMessage = ex.Message });
            }

            return data;
        }

        public List<vwStopAccrueDashboardHeader> GetCountDataFinish(vwStopAccrueDashboardHeader param, int rowSkip, int pageSize)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            List<vwStopAccrueDashboardHeader> data = new List<vwStopAccrueDashboardHeader>();
            try
            {
                var repo = new vwStopAccrueDashboardHeaderRepository(context);

                var contextTBGSys = new DbContext(Helper.GetConnection("TBGFlow"));
                var wfHeaders = new List<WfHeaders_View>();
                var repoTBGSys = new WfHeaders_ViewRepository(contextTBGSys);

                var vwStopAccrueHeader = new vwStopAccrueHeader();
                vwStopAccrueHeader.ActivityID = param.ActivityID;
                string whereclause = " Code = 'STOP_ACCRUE' AND Status IN('inp','don1') ";
                whereclause += " AND " + (param.Activity.ToLower() == "finish" ? "Label='SA_STOP'" : "Label in ('SA_APPR_DIV','SA_APPR_DEPT_ACC','SA_APPR_DIV_ACC','SA_FEED_DIV_ACC','SA_FEED_DEPT_ACC','SA_SUBMIT_DOC','SA_DOC_RECEIVE_DHAC','SA_DOC_RECEIVE_DHAS','SA_APPR_DIV_CHIEF','SA_APPR_CHIEF_MKT')");
                wfHeaders = repoTBGSys.GetList(whereclause);

                var appHeader = new ArrayList();
                foreach (var appHeaderID in wfHeaders)
                {
                    appHeader.Add(appHeaderID.AppHeaderID);
                }

                int i = 0;
                string split = "";
                for (i = 0; i < appHeader.Count; i++)
                {
                    split += "'" + appHeader[i].ToString() + "'" + ",";
                }

                if (param.DepartName == "Department")
                {
                    data = repo.GetCountByDepartmentFinish(param.SubmissionDateFrom, param.SubmissionDateTo, param.DirectorateCode, "HOLD", split);
                    //var sum = data.Sum(x => x.CountData);
                }
                else
                {
                    data = repo.GetCountByDetailCaseFinish(param.SubmissionDateFrom, param.SubmissionDateTo, param.DirectorateCode, "HOLD", split);
                }


            }
            catch (Exception ex)
            {
                context.Dispose();
                data.Add(new vwStopAccrueDashboardHeader { ErrorMessage = ex.Message });
            }

            return data;
        }

        public List<vwStopAccrueDashboardHeader> GetCountDataSTOPFinish(vwStopAccrueDashboardHeader param, int rowSkip, int pageSize)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            List<vwStopAccrueDashboardHeader> data = new List<vwStopAccrueDashboardHeader>();
            try
            {
                var repo = new vwStopAccrueDashboardHeaderRepository(context);
                var contextTBGSys = new DbContext(Helper.GetConnection("TBGFlow"));
                var wfHeaders = new List<WfHeaders_View>();
                var repoTBGSys = new WfHeaders_ViewRepository(contextTBGSys);

                var vwStopAccrueHeader = new vwStopAccrueHeader();
                vwStopAccrueHeader.ActivityID = param.ActivityID;
                string whereclause = " Code = 'STOP_ACCRUE' AND Status IN('inp','don1') ";
                whereclause += " AND " + (param.Activity.ToLower() == "finish" ? "Label='SA_STOP'" : "Label in ('SA_APPR_DIV','SA_APPR_DEPT_ACC','SA_APPR_DIV_ACC','SA_FEED_DIV_ACC','SA_FEED_DEPT_ACC','SA_SUBMIT_DOC','SA_DOC_RECEIVE_DHAC','SA_DOC_RECEIVE_DHAS','SA_APPR_DIV_CHIEF','SA_APPR_CHIEF_MKT')");
                wfHeaders = repoTBGSys.GetList(whereclause);

                var appHeader = new ArrayList();
                foreach (var appHeaderID in wfHeaders)
                {
                    appHeader.Add(appHeaderID.AppHeaderID);
                }


                int i = 0;
                string split = "";
                //var splitConv = 0;
                for (i = 0; i < appHeader.Count; i++)
                {
                    split += appHeader[i].ToString() + ",";
                    //split += split;
                    //splitConv = Convert.ToInt32(split);
                    //i = i + 1;
                }

                if (param.DepartName == "Department")
                {
                    data = repo.GetCountByDepartmentFinish(param.SubmissionDateFrom, param.SubmissionDateTo, param.DirectorateCode, "STOP", split);
                }
                else
                {
                    data = repo.GetCountByDetailCaseFinish(param.SubmissionDateFrom, param.SubmissionDateTo, param.DirectorateCode, "STOP", split);
                }



            }
            catch (Exception ex)
            {
                context.Dispose();
                data.Add(new vwStopAccrueDashboardHeader { ErrorMessage = ex.Message });
            }

            return data;
        }

        public List<vwStopAccrueDashboardDetail> GetListExportAllDashboardDetail(string submissionDateFrom, string submissionDateTo, string directorateCode, string AccrueType, string GroupBy)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            List<vwStopAccrueDashboardDetail> data = new List<vwStopAccrueDashboardDetail>();
            List<vwStopAccrueDashboardDetail> dataList = new List<vwStopAccrueDashboardDetail>();
            vwStopAccrueDashboardDetail detail = new vwStopAccrueDashboardDetail();
            try
            {
                var repo = new vwStopAccrueDashboardDetailRepository(context);

                var contextTBGSys = new DbContext(Helper.GetConnection("TBGFlow"));
                var wfHeaders = new List<WfHeaders_View>();
                var repoTBGSys = new WfHeaders_ViewRepository(contextTBGSys);

                var vwStopAccrueHeader = new vwStopAccrueHeader();
                var Activity = "ongoing";
                //vwStopAccrueHeader.ActivityID = ActivityID;
                string whereclause = " Code = 'STOP_ACCRUE' AND Status IN('inp','don1') ";
                whereclause += " AND " + (Activity.ToLower() == "finish" ? "Label='SA_STOP'" : "Label in ('SA_APPR_DIV','SA_APPR_DEPT_ACC','SA_APPR_DIV_ACC','SA_FEED_DIV_ACC','SA_FEED_DEPT_ACC','SA_SUBMIT_DOC','SA_DOC_RECEIVE_DHAC','SA_DOC_RECEIVE_DHAS','SA_APPR_DIV_CHIEF','SA_APPR_CHIEF_MKT')");
                wfHeaders = repoTBGSys.GetList(whereclause);

                var appHeader = new ArrayList();
                foreach (var appHeaderID in wfHeaders)
                {
                    appHeader.Add(appHeaderID.AppHeaderID);
                }

                int i = 0;
                string split = "";
                for (i = 0; i < appHeader.Count; i++)
                {
                    split += "'" + appHeader[i].ToString() + "'" + ",";
                }


                data = repo.GetListExportAllDashboardDetail(submissionDateFrom, submissionDateTo, directorateCode, "HOLD", split, "ongoing");
                foreach(var data1 in data)
                {
                    detail = new vwStopAccrueDashboardDetail
                    {
                        Activity = data1.Activity,
                        RequestNumber = data1.RequestNumber,
                        Initiator = data1.Initiator,
                        StartEffectiveDate = data1.StartEffectiveDate,
                        EndEffectiveDate = data1.EndEffectiveDate,
                        AccrueType = data1.AccrueType,
                        TrxStopAccrueDetailID = data1.TrxStopAccrueDetailID,
                        TrxStopAccrueHeaderID = data1.TrxStopAccrueHeaderID,
                        SONumber = data1.SONumber,
                        RevenueAmount = data1.RevenueAmount,
                        CapexAmount = data1.CapexAmount,
                        CompensationAmount = data1.CompensationAmount,
                        SiteName = data1.SiteName,
                        Product = data1.Product,
                        CategoryCase = data1.CategoryCase,
                        DetailCase = data1.DetailCase,
                        RFIDone = data1.RFIDone,
                        Company = data1.Company,  
                        Customer = data1.Customer,
                        DepartName = data1.DepartName,
                        IsHold = data1.IsHold
                    };
                    dataList.Add(detail);
                }

                var dataStopOngoing = repo.GetListExportAllDashboardDetail(submissionDateFrom, submissionDateTo, directorateCode, "STOP", split, "ongoing");
                foreach (var data2 in dataStopOngoing)
                {
                    detail = new vwStopAccrueDashboardDetail
                    {
                        Activity = data2.Activity,
                        RequestNumber = data2.RequestNumber,
                        Initiator = data2.Initiator,
                        StartEffectiveDate = data2.StartEffectiveDate,
                        EndEffectiveDate = data2.EndEffectiveDate,
                        AccrueType = data2.AccrueType,
                        TrxStopAccrueDetailID = data2.TrxStopAccrueDetailID,
                        TrxStopAccrueHeaderID = data2.TrxStopAccrueHeaderID,
                        SONumber = data2.SONumber,
                        RevenueAmount = data2.RevenueAmount,
                        CapexAmount = data2.CapexAmount,
                        CompensationAmount = data2.CompensationAmount,
                        SiteName = data2.SiteName,
                        Product = data2.Product,
                        CategoryCase = data2.CategoryCase,
                        DetailCase = data2.DetailCase,
                        RFIDone = data2.RFIDone,
                        Company = data2.Company,
                        Customer = data2.Customer,
                        DepartName = data2.DepartName,
                        IsHold = data2.IsHold
                    };
                    dataList.Add(detail);
                }


                var ActivityFinish = "finish";
                //vwStopAccrueHeader.ActivityID = ActivityID;
                whereclause = " Code = 'STOP_ACCRUE' AND Status IN('inp','don1') ";
                whereclause += " AND " + (ActivityFinish.ToLower() == "finish" ? "Label='SA_STOP'" : "Label in ('SA_APPR_DIV','SA_APPR_DEPT_ACC','SA_APPR_DIV_ACC','SA_FEED_DIV_ACC','SA_FEED_DEPT_ACC','SA_SUBMIT_DOC','SA_DOC_RECEIVE_DHAC','SA_DOC_RECEIVE_DHAS','SA_APPR_DIV_CHIEF','SA_APPR_CHIEF_MKT')");
                wfHeaders = repoTBGSys.GetList(whereclause);

                appHeader = new ArrayList();
                foreach (var appHeaderID in wfHeaders)
                {
                    appHeader.Add(appHeaderID.AppHeaderID);
                }

                i = 0;
                split = "";
                for (i = 0; i < appHeader.Count; i++)
                {
                    split += "'" + appHeader[i].ToString() + "'" + ",";
                }

                var dataListHoldFinish = repo.GetListExportAllDashboardDetail(submissionDateFrom, submissionDateTo, directorateCode, "HOLD", split, "finish");
                foreach (var dataHoldFinish in dataListHoldFinish)
                {
                    detail = new vwStopAccrueDashboardDetail
                    {
                        Activity = dataHoldFinish.Activity,
                        RequestNumber = dataHoldFinish.RequestNumber,
                        Initiator = dataHoldFinish.Initiator,
                        StartEffectiveDate = dataHoldFinish.StartEffectiveDate,
                        EndEffectiveDate = dataHoldFinish.EndEffectiveDate,
                        AccrueType = dataHoldFinish.AccrueType,
                        TrxStopAccrueDetailID = dataHoldFinish.TrxStopAccrueDetailID,
                        TrxStopAccrueHeaderID = dataHoldFinish.TrxStopAccrueHeaderID,
                        SONumber = dataHoldFinish.SONumber,
                        RevenueAmount = dataHoldFinish.RevenueAmount,
                        CapexAmount = dataHoldFinish.CapexAmount,
                        CompensationAmount = dataHoldFinish.CompensationAmount,
                        SiteName = dataHoldFinish.SiteName,
                        Product = dataHoldFinish.Product,
                        CategoryCase = dataHoldFinish.CategoryCase,
                        DetailCase = dataHoldFinish.DetailCase,
                        RFIDone = dataHoldFinish.RFIDone,
                        Company = dataHoldFinish.Company,
                        Customer = dataHoldFinish.Customer,
                        DepartName = dataHoldFinish.DepartName,
                        IsHold = dataHoldFinish.IsHold
                    };
                    dataList.Add(detail);
                }

                var dataListStopFinish = repo.GetListExportAllDashboardDetail(submissionDateFrom, submissionDateTo, directorateCode, "STOP", split, "finish");
                foreach (var dataStopFinish in dataListStopFinish)
                {
                    detail = new vwStopAccrueDashboardDetail
                    {
                        Activity = dataStopFinish.Activity,
                        RequestNumber = dataStopFinish.RequestNumber,
                        Initiator = dataStopFinish.Initiator,
                        StartEffectiveDate = dataStopFinish.StartEffectiveDate,
                        EndEffectiveDate = dataStopFinish.EndEffectiveDate,
                        AccrueType = dataStopFinish.AccrueType,
                        TrxStopAccrueDetailID = dataStopFinish.TrxStopAccrueDetailID,
                        TrxStopAccrueHeaderID = dataStopFinish.TrxStopAccrueHeaderID,
                        SONumber = dataStopFinish.SONumber,
                        RevenueAmount = dataStopFinish.RevenueAmount,
                        CapexAmount = dataStopFinish.CapexAmount,
                        CompensationAmount = dataStopFinish.CompensationAmount,
                        SiteName = dataStopFinish.SiteName,
                        Product = dataStopFinish.Product,
                        CategoryCase = dataStopFinish.CategoryCase,
                        DetailCase = dataStopFinish.DetailCase,
                        RFIDone = dataStopFinish.RFIDone,
                        Company = dataStopFinish.Company,
                        Customer = dataStopFinish.Customer,
                        DepartName = dataStopFinish.DepartName,
                        IsHold = dataStopFinish.IsHold
                    };
                    dataList.Add(detail);
                }

                var dataListFinishManual = repo.GetListExportAllDashboardDetail(submissionDateFrom, submissionDateTo, directorateCode, "", "", "finish");
                foreach (var dataFinishManual in dataListFinishManual)
                {
                    detail = new vwStopAccrueDashboardDetail
                    {
                        Activity = dataFinishManual.Activity,
                        RequestNumber = dataFinishManual.RequestNumber,
                        Initiator = dataFinishManual.Initiator,
                        StartEffectiveDate = dataFinishManual.StartEffectiveDate,
                        EndEffectiveDate = dataFinishManual.EndEffectiveDate,
                        AccrueType = dataFinishManual.AccrueType,
                        TrxStopAccrueDetailID = dataFinishManual.TrxStopAccrueDetailID,
                        TrxStopAccrueHeaderID = dataFinishManual.TrxStopAccrueHeaderID,
                        SONumber = dataFinishManual.SONumber,
                        RevenueAmount = Convert.ToDecimal(dataFinishManual.RevenueAmount),
                        CapexAmount = Convert.ToDecimal(dataFinishManual.CapexAmount),
                        CompensationAmount = Convert.ToDecimal(dataFinishManual.CompensationAmount),
                        SiteName = dataFinishManual.SiteName,
                        Product = dataFinishManual.Product,
                        CategoryCase = dataFinishManual.CategoryCase,
                        DetailCase = dataFinishManual.DetailCase,
                        RFIDone = dataFinishManual.RFIDone,
                        Company = dataFinishManual.Company,
                        Customer = dataFinishManual.Customer,
                        DepartName = dataFinishManual.DepartName,
                        IsHold = dataFinishManual.IsHold
                    };
                    dataList.Add(detail);
                }
            }
            catch (Exception ex)
            {

            }

            return dataList;
        }
        public List<vwStopAccrueExportToExcel> GetListExportDetailByDate(string submissionDateFrom, string submissionDateTo, string directorateCode, string AccrueType, string GroupBy)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            List<vwStopAccrueExportToExcel> data = new List<vwStopAccrueExportToExcel>();
            List<vwStopAccrueExportToExcel> dataList = new List<vwStopAccrueExportToExcel>();
            vwStopAccrueExportToExcel detail = new vwStopAccrueExportToExcel();
            try
            {
                //var repo = new vwStopAccrueDashboardDetailRepository(context);
                var repo = new vwStopAccrueExportToExcelRepository(context);
                var wfHeaders = new List<WfHeaders_View>();
                var repoTBGSys = new WfHeaders_ViewRepository(context);

                var vwStopAccrueHeader = new vwStopAccrueHeader();
                string start = string.Format("{0:yyyy-MM-dd}", submissionDateFrom);
                string end = string.Format("{0:yyyy-MM-dd}", submissionDateTo);
                data = repo.GetListExportAllDetailByExcel(start, end, directorateCode);
                foreach (var data1 in data)
                {
                    detail = new vwStopAccrueExportToExcel
                    {
                        ActualStatus = data1.ActualStatus,
                        InputReqBy = data1.InputReqBy,
                        RequestNumber = data1.RequestNumber,
                        DepartmentName = data1.DepartmentName,
                        DirectorateCode = data1.DirectorateCode,
                        DirectorateName = data1.DirectorateName,
                        AppHeaderID = data1.AppHeaderID,
                        Submit = data1.Submit,
                        VerificationDivHead = data1.VerificationDivHead,
                        ActivtyDivHead = data1.ActivtyDivHead,
                        VerifiedByChief = data1.VerifiedByChief,
                        ActivtyChief = data1.ActivtyChief,
                        RecomendedByChiefOfMarketing = data1.RecomendedByChiefOfMarketing,
                        ActivtyChiefMKT = data1.ActivtyChiefMKT,
                        FeedbackDepHeadREALatest = data1.FeedbackDepHeadREALatest,
                        ActivtyDeptREALatest = data1.ActivtyDeptREALatest,
                        FeedbackDepHeadREA2ndLatest = data1.FeedbackDepHeadREA2ndLatest,
                        ActivtyDeptREA2ndlatest = data1.ActivtyDeptREA2ndlatest,
                        FeedbackFromDeptAcc = data1.FeedbackFromDeptAcc,
                        ActivtyDeptACC = data1.ActivtyDeptACC,
                        FeedbackVerificationDivControllershipLatest = data1.FeedbackVerificationDivControllershipLatest,
                        ActivtyDivCONTLatest = data1.ActivtyDivCONTLatest,
                        FeedbackVerificationDivControllership2ndlatest = data1.FeedbackVerificationDivControllership2ndlatest,
                        ActivtyDivCONT2ndLatest = data1.ActivtyDivCONT2ndLatest,
                        FeedbackFromDivAcc = data1.FeedbackFromDivAcc,
                        ActivtyDivACC = data1.ActivtyDivACC,
                        SubmitDocument = data1.SubmitDocument,
                        ActivtySubmitDoc = data1.ActivtySubmitDoc,
                        DocumentReceiveByAccounting = data1.DocumentReceiveByAccounting,
                        ActivtyDocACC = data1.ActivtyDocACC,
                        DocumentReceiveByAset = data1.DocumentReceiveByAset,
                        ActivtyDocAST = data1.ActivtyDocAST,
                        Finish = data1.Finish,
                        ActivtyFinish = data1.ActivtyFinish,
                        Rejected = data1.Rejected,
                        ActivtyReject = data1.Rejected
                    };
                    dataList.Add(detail);
                }
            }
            catch(Exception ex)
            {

            }
            return dataList;
        }
    }   
}
