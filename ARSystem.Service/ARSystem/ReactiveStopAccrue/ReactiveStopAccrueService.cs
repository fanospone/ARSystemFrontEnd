using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ARSystem.Domain.Models;
using ARSystem.Domain.Repositories;
using ARSystem.Domain.DAL;
using TBGFramework.Workflow;
using TBGFramework.HRData;
using TBGFramework.Common;
using ARSystem.Domain.Repositories.TBIGSYSDB01.TBGARSystem;
using ARSystem.Domain.Models.TBIGSYSDB01.TBGARSystem;
using ARSystem.Domain.Repositories.TBIGSYSDB01;
using WorkflowService;

namespace ARSystem.Service
{
    public class ReactiveStopAccrueService : BaseService
    {
        private readonly Workflow workflow;
        //public string procOID;
        public ReactiveStopAccrueService()
        {
            workflow = new Workflow();
        }


        #region GetSonumList
        public List<vwReactiveStopAccrueSonumbList> GetSonumbList(vwReactiveStopAccrueSonumbList param, int rowSkip, int pageSize)
        {
            return pGetSonumbList(SonumbListWhereClause(param), rowSkip, pageSize);
        }

        public List<vwReactiveStopAccrueSonumbList> GetSonumbList(vwReactiveStopAccrueSonumbList param)
        {
            return pGetSonumbList(SonumbListWhereClause(param));

        }

        public int GetCountHeaderRequest(vwReactiveStopAccrueHeader param)
        {
            return pGetCountHeaderRequest(param);
        }
        
        private List<vwReactiveStopAccrueSonumbList> pGetSonumbList(string whereClause, int rowSkip, int pageSize)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            List<vwReactiveStopAccrueSonumbList> data = new List<vwReactiveStopAccrueSonumbList>();
            try
            {
                var repo = new vwReactiveStopAccrueSonumbListRepository(context);
                data = repo.GetPaged(whereClause, "RegionName ASC", rowSkip, pageSize);
            }
            catch (Exception ex)
            {
                context.Dispose();
                data.Add(new vwReactiveStopAccrueSonumbList { ErrorMessage = ex.Message });
            }
            return data;
        }

        private List<vwReactiveStopAccrueSonumbList> pGetSonumbList(string whereClause)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            List<vwReactiveStopAccrueSonumbList> data = new List<vwReactiveStopAccrueSonumbList>();
            try
            {

                var repo = new vwReactiveStopAccrueSonumbListRepository(context);
                data = repo.GetList(whereClause, "RegionName ASC");
            }
            catch (Exception ex)
            {
                context.Dispose();
                data.Add(new vwReactiveStopAccrueSonumbList { ErrorMessage = ex.Message });
            }

            return data;
        }

        private string SonumbListWhereClause(vwReactiveStopAccrueSonumbList param)
        {
            string whereClause = "";
            if (!string.IsNullOrEmpty(param.CustomerID))
                whereClause += "CustomerID ='" + param.CustomerID + "' AND ";
            if (!string.IsNullOrEmpty(param.CompanyID))
                whereClause += "CompanyID ='" + param.CompanyID + "' AND ";
            if (param.ProductID != null && param.ProductID != 0)
                whereClause += "ProductID ='" + param.ProductID + "' AND ";
            if (param.RegionID != 0)
                whereClause += "RegionID ='" + param.RegionID + "' AND ";
            if (!string.IsNullOrEmpty(param.SiteID))
                whereClause += "SiteID like '%" + param.SiteID + "%' AND ";
            if (!string.IsNullOrEmpty(param.SiteName))
                whereClause += "SiteName like '%" + param.SiteName + "%' AND "; 
            if (!string.IsNullOrEmpty(param.SoNumber))
                whereClause += "SONumber like '%" + param.SoNumber + "%' AND ";
            if (!string.IsNullOrEmpty(param.RFIDone.ToString()))
                whereClause += "CAST(RFIDone as date) =CAST('" + String.Format("{0:yyyy/MM/dd}", param.RFIDone) + "' as date) AND ";
            whereClause = !string.IsNullOrWhiteSpace(whereClause) ? whereClause.Substring(0, whereClause.Length - 5) : "";
            return whereClause;
        }
        
        private int pGetCountHeaderRequest(vwReactiveStopAccrueHeader param)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var repo = new vwReactiveStopAccrueHeaderRepository(context);
            try
            {
                var contextTBGSys = new DbContext(Helper.GetConnection("TBGFlow"));
                List<vwReactiveStopAccrueHeader> data = new List<vwReactiveStopAccrueHeader>();
                var repoTBGSys = new WfHeaders_ViewRepository(contextTBGSys);
                var wfHeaders = new List<WfHeaders_View>();
                data = repo.GetList(HeaderRequestWhereClause(param), "CreatedDate DESC");
                wfHeaders = repoTBGSys.GetList(WfHeaderWhereClause(param));
                var list = (from request in data
                            join wfHeader in wfHeaders on request.AppHeaderID equals wfHeader.AppHeaderID
                            select new { request.AppHeaderID }).ToArray();
                return list.Count();
            }
            catch (Exception ex)
            {
                context.Dispose();
                return 0;
            }

        }

        private string HeaderRequestWhereClause(vwReactiveStopAccrueHeader param)
        {

            string Initiator = "";
            Initiator = param.Initiator;

            string whereClause = "";
            if (param.RequestTypeID != 0)
                whereClause += "RequestTypeID =" + param.RequestTypeID + " AND ";

            if (param.UserRole == "DEPT_HEAD")
            {
                if (!string.IsNullOrEmpty(Initiator))
                    whereClause += "Initiator = '" + Initiator + "' OR ResourceRequesterId = '" + Initiator  + "' AND ";
            }

            if (!string.IsNullOrEmpty(param.RequestNumber))
                whereClause += "RequestNumber like '%" + param.RequestNumber + "%' AND ";
            if (!string.IsNullOrEmpty(param.StartEffectiveDate.ToString()))
                whereClause += "StartEffectiveDate ='" + String.Format("{0:yyyy-MM-dd}", param.StartEffectiveDate) + "' AND ";
            if (!string.IsNullOrEmpty(param.EndEffectiveDate.ToString()))
                whereClause += "EndEffectiveDate ='" + String.Format("{0:yyyy-MM-dd}", param.EndEffectiveDate) + "' AND ";
            if (!string.IsNullOrEmpty(param.CreatedDate.ToString()))
                whereClause += "CAST(CreatedDate as date) =CAST('" + String.Format("{0:yyyy-MM-dd}", param.CreatedDate) + "' as Date) AND ";
            if (!string.IsNullOrEmpty(param.AccrueType))
                whereClause += "AccrueType = '" + param.AccrueType + "' AND ";
            if (!string.IsNullOrEmpty(param.StartDate.ToString()))
            {
                if (!string.IsNullOrEmpty(param.EndDate.ToString()))
                {
                    whereClause += "CAST(CreatedDate as date) between CAST('" + String.Format("{0:yyyy-MM-dd}", param.StartDate) + "' as Date) AND CAST('" + String.Format("{0:yyyy-MM-dd}", param.EndDate) + "' as Date) AND ";
                }
                else
                {
                    whereClause += " CAST(CreatedDate as date) = CAST('" + String.Format("{0:yyyy-MM-dd}", param.StartDate) + "') AND ";
                }
            }
            whereClause = !string.IsNullOrWhiteSpace(whereClause) ? whereClause.Substring(0, whereClause.Length - 5) : "";
            return whereClause;
        }
        private string WfHeaderWhereClause(vwReactiveStopAccrueHeader param)
        {
            string ActivityOwner = "";
            string Initiator = "";
            
            string whereClause = "";
            Initiator = param.Initiator;
            ActivityOwner = param.Initiator;

            whereClause += "Code ='STOP_ACCRUE' AND ";

            if (param.ActivityID != 0)
                whereClause += "ActivityID =" + param.ActivityID + " AND ";

            if (param.UserRole == "DEPT_HEAD")
            {
                if (!string.IsNullOrEmpty(Initiator))
                    whereClause += "Initiator = '" + Initiator + "' AND ";
                if (!string.IsNullOrEmpty(param.ActivityOwnerName))
                    whereClause += "ActivityOwnerName like '%" + param.ActivityOwnerName + "%' AND ";
            }
            else
            {
                if (!string.IsNullOrEmpty(param.InitiatorName))
                    whereClause += "InitiatorName like '%" + param.InitiatorName + "%' AND ";

                if (!string.IsNullOrEmpty(ActivityOwner))
                    whereClause += "ActivityOwner = '" + ActivityOwner + "' AND ";
            }
            whereClause = !string.IsNullOrWhiteSpace(whereClause) ? whereClause.Substring(0, whereClause.Length - 5) : "";
            return whereClause;
        }
        
        public int GetCountSonumb(vwReactiveStopAccrueSonumbList param)
        {
            return pGetCountSonumb(SonumbListWhereClause(param));
        }

        private int pGetCountSonumb(string whereClause)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            try
            {
                var repo = new vwReactiveStopAccrueSonumbListRepository(context);
                return repo.GetCount(whereClause);
            }
            catch (Exception ex)
            {
                context.Dispose();
                return 0;
            }

        }

        private string SonumbListWhereClause(vwStopAccrueSonumbList param)
        {
            string whereClause = "";
            if (!string.IsNullOrEmpty(param.CustomerID))
                whereClause += "CustomerID ='" + param.CustomerID + "' AND ";
            if (!string.IsNullOrEmpty(param.CompanyID))
                whereClause += "CompanyID ='" + param.CompanyID + "' AND ";
            if (param.ProductID != null && param.ProductID != 0)
                whereClause += "ProductID ='" + param.ProductID + "' AND ";
            if (param.RegionID != 0)
                whereClause += "RegionID ='" + param.RegionID + "' AND ";
            if (!string.IsNullOrEmpty(param.SiteID))
                whereClause += "SiteID like '%" + param.SiteID + "%' AND ";
            if (!string.IsNullOrEmpty(param.SiteName))
                whereClause += "SiteName like '%" + param.SiteName + "%' AND ";
            if (!string.IsNullOrEmpty(param.SONumber))
                whereClause += "SONumber like '%" + param.SONumber + "%' AND ";
            if (!string.IsNullOrEmpty(param.RFIDone.ToString()))
                whereClause += "CAST(RFIDone as date) =CAST('" + String.Format("{0:yyyy/MM/dd}", param.RFIDone) + "' as date) AND ";
            whereClause = !string.IsNullOrWhiteSpace(whereClause) ? whereClause.Substring(0, whereClause.Length - 5) : "";
            return whereClause;
        }

        public List<vwTrxStopAccrueDraft> GetDraftList(string initiatorID)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            List<vwTrxStopAccrueDraft> data = new List<vwTrxStopAccrueDraft>();
            try
            {
                var repo = new vwTrxStopAccrueDraftRepository(context);
                data = repo.GetList($"Initiator='{initiatorID}'");
            }
            catch (Exception ex)
            {
                context.Dispose();
                data.Add(new vwTrxStopAccrueDraft { ErrorMessage = ex.Message });
            }

            return data;
        }

        public List<vwTrxReactiveStopAccrueHeaderReqNo> GetSoNumberList(string requestNo)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            List<vwTrxReactiveStopAccrueHeaderReqNo> data = new List<vwTrxReactiveStopAccrueHeaderReqNo>();
            try
            {
                var get = new vwTrxReactiveStopAccrueHeaderReqNoRepository(context);
                data = get.GetList($"RequestNumber='{requestNo}'");
            }
            catch (Exception ex)
            {
                context.Dispose();
                data.Add(new vwTrxReactiveStopAccrueHeaderReqNo { ErrorMessage = ex.Message });
            }

            return data;
        }

        #endregion

        #region Header Request
        public trxReactiveStopAccrueHeader SubmitRequest(List<trxReactiveStopAccrueDetail> detail, trxReactiveStopAccrueHeader header, string submitType, List<trxStopAccrueDetailDraft> detailDraft)
        {
            return pSubmitRequest(detail, header, submitType, detailDraft);
        }

        // Clear Draft
        public trxReactiveStopAccrueHeader ClearDraft(trxReactiveStopAccrueHeader header)
        {
            return pClearDraft(header);
        }
        
        // Submit Edit
        public trxReactiveStopAccrueHeader SubmitEdit(List<trxReactiveStopAccrueDetail> detail, trxReactiveStopAccrueHeader header, string submitType)
        {
            return pSubmitEdit(detail, header, submitType);
        }

        // update Request Header
        public trxReactiveStopAccrueHeader UpdateRequest(trxReactiveStopAccrueHeader header)
        {
            return pUpdateRequest(header);
        }

        //
        private trxReactiveStopAccrueHeader pSubmitRequest(List<trxReactiveStopAccrueDetail> detail, trxReactiveStopAccrueHeader header, string submitType, List<trxStopAccrueDetailDraft> detailDraft)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var result = new trxReactiveStopAccrueHeader();
            var hrData = new Employee();
            var hrDataResult = hrData.GetDetailByUserID(header.Initiator);
            header.Initiator = hrDataResult.NIP;
            try
            {

                string requestNumber = "";
                //var repoHeader = new trxStopAccrueHeaderRepository(context);
                var repoHeader = new trxReactiveStopAccrueHeaderRepository(context);
                var repoHeaderDraft = new vwtrxStopAccrueHeaderDraftRepository(context);
                var StopAccrueHeader = new trxReactiveStopAccrueHeader();
                var StopAccrueHeaderDraft = new trxStopAccrueHeaderDraft();

                //// generate rrquest number

                string deptCode = "";

                //var contextDepart = new DbContext(Helper.GetConnection("TBIGSys"));
                //var initDepartRepo = new MstInitialDepartmentRepository(contextDepart);
                //var initDepart = new MstInitialDepartment();
                //initDepart = initDepartRepo.GetList("DepartmentCode='" + hrDataResult.DepartmentCode + "'").FirstOrDefault();
                //deptCode = initDepart.Code;

                var mstDepartService = new MstDepartmentService();
                deptCode = mstDepartService.GetDepartment(new vwMstDepartment { DepartmentCode = hrDataResult.DepartmentCode }).DepartmentInitial;


                if (submitType == "NEW")
                {
                    string requetNo = "";
                    StopAccrueHeader = repoHeader.GetList("", "ID DESC").FirstOrDefault();
                    requetNo = StopAccrueHeader == null ? "" : StopAccrueHeader.RequestNumber;


                    if (requetNo == "" || requetNo == null)
                    {
                        requetNo = "0001";
                    }
                    else
                    {
                        requetNo = requetNo.Substring(0, 4);
                        int counter = int.Parse(requetNo) + 1;
                        if (counter.ToString().Length == 1)
                            requetNo = "000" + counter.ToString();
                        else if (counter.ToString().Length == 2)
                            requetNo = "00" + counter.ToString();
                        else if (counter.ToString().Length == 3)
                            requetNo = "0" + counter.ToString();
                        else
                            requetNo = counter.ToString();
                    }
                    string accrueType;
                    if (header.Remarks == "DRAFT")
                    {
                        accrueType = "DRAFT";
                    }
                    else
                    {
                        accrueType = header.RequestType == "STOP" ? "RESAC" : "REHAC";
                    }

                    string year = System.DateTime.Now.Year.ToString();
                    string month = MonthRomawi(System.DateTime.Now.Month);
                    requestNumber = requetNo + "/" + accrueType + "/" + deptCode + "/" + month + "/" + year;

                }
                else
                {
                    requestNumber = header.RequestNumber;
                }
                // get department code

                var activity = new Activity();
                var activityID = activity.GetDetail("STOP_ACCRUE", "SA_START");

                // job
                Job job = new Job();
                var datajob = job.GetDetail("STOP_ACCRUE");

                // wf header
                var workFlowService = new WorkFlowService();
                var wfHeader = new WfHeaders();
                wfHeader.JobID = datajob.JobID;
                // save wfheader
                var wf = new Header();
                wf.Initiator = header.Initiator;
                wf.Status = "rtr";
                wf.ActivityID = activityID.ActivityID;
                wf.CreateDate = System.DateTime.Now;
                wf.PrevActivityID = 0;
                wf.NextFlag = "submit";
                wf.JobID = datajob.JobID;
                wf.LastModifiedDate = System.DateTime.Now;
                wf.InitiatorJobTitle = hrDataResult.JobTitle;
                wf.InitiatorLocation = hrDataResult.PRL01;
                wf.ActivityOwner = "Routing";
                wf.AppHeaderID = 0;
                wf.PRL = hrDataResult.PRL;
                wf.LastModifiedBy = header.Initiator;
                wf.RequestNo = "";
                wf.Summary = header.Remarks;
                wf.IsApproved = false;
                wf.PotentialActivityOwner = "";
                wf.Save();
                wf.GetDetail(wf.AppHeaderID);
                TBGFramework.Common.Helper.SaveLog(wf.AppHeaderID, header.Remarks, "STOP_ACCRUE", wf.CreateDate, header.Initiator, "", "", wf.Label, "submit");

                /* create header data */
                /*start*/

                header.AppHeaderID = wf.AppHeaderID;
                header.RequestNumber = requestNumber;
                header.Status = "SUBMITED";
                header.DepartInitial = deptCode;
                header.DeptCode = hrDataResult.DepartmentCode;

                //var headerDraft = new vwtrxStopAccrueHeaderDraft();
                //headerDraft.Equals(header);

                /*Add workflow*/

                //var contextTbgSA = new DbContext(Helper.GetConnection("TBIGSYSDB01_TBGSecureAccess"));
                var variable = new Dictionary<string, object>();
                variable.Add("status", "submit");
                variable.Add("param", wf.Initiator);
                    var ss = wf.Initiator;
                var wfl = workflow.InitiateWorkflow(wf.Initiator, true, "app_reactivestopaccrue", "proc_ReActiveStopAccrue", variable);

                header.ProcessOID = Convert.ToString(wfl.processOID);
                //header.Approver = wfl.assignment;
                StopAccrueHeader = repoHeader.Create(header);
                
                /*end*/

                /* create detail data */
                /*start*/

                var repoDetailDraft = new vwTrxStopAccrueDetailDraftRepository(context);
                //var repoDetail = new trxStopAccrueDetailRepository(context);
                var repoDetail = new trxReactiveStopAccrueDetailRepository(context);

                var StopAccrueDetail = new List<trxReactiveStopAccrueDetail>();
                var StopAccrueDetailDraft = new List<trxStopAccrueDetailDraft>();
                detail = detail.Select(x => { x.TrxStopAccrueHeaderID = StopAccrueHeader.ID; return x; }).ToList();
                detailDraft = detailDraft.Select(x => { x.TrxStopAccrueHeaderID = StopAccrueHeader.ID; return x; }).ToList();
                if (header.Remarks == "DRAFT")
                {
                    StopAccrueDetail = repoDetail.CreateBulkyDraft(detail);
                }
                else
                {
                    StopAccrueDetail = repoDetail.CreateBulky(detail);
                }
                /*end*/

                /* create detail file data */
                /*start*/

                if (header.Remarks == "DRAFT")
                {
                    StopAccrueDetail = repoDetail.GetList("TrxStopAccrueHeaderID = " + StopAccrueHeader.ID, "Draft");
                }
                else
                {
                    StopAccrueDetail = repoDetail.GetList("TrxStopAccrueHeaderID = " + StopAccrueHeader.ID, "");
                }
                var repoDetailFile = new trxReactiveStopAccrueDetailFileRepository(context);
                var repoDetailFileDraft = new vwTrxStopAccrueDetailFileDraftRepository(context);

                var StopAccrueDetailFile = new List<trxReactiveStopAccrueDetailFile>();
                var StopAccrueDetailFileDraft = new List<vwTrxStopAccrueDetailFileDraft>();

                var queryDetail = from dtl1 in detail
                                  join dtl2 in StopAccrueDetail on dtl1.SONumber equals dtl2.SONumber
                                  select new
                                  {
                                      dtl2.ID,
                                      dtl1.FileName,
                                      dtl1.FilePath,
                                      dtl1.Remarks
                                  };
                
                foreach (var item in queryDetail)
                {
                    StopAccrueDetailFile.Add(new trxReactiveStopAccrueDetailFile
                    {
                        trxStopAccrueDetailID = item.ID,
                        FileName = item.FileName,
                        FilePath = item.FilePath,
                        CreatedDate = System.DateTime.Now,
                        Remarks = item.Remarks
                    });
                }
                if (header.Remarks == "DRAFT")
                {
                    StopAccrueDetailFile = repoDetailFile.CreateBulkyDraft(StopAccrueDetailFile);
                }
                else
                {
                    StopAccrueDetailFile = repoDetailFile.CreateBulky(StopAccrueDetailFile);
                    StopAccrueDetail = repoDetail.DeleteDraft(header.Initiator);
                }
                /*end*/
                
                return result;
            }
            catch (Exception ex)
            {
                //context.Dispose();
                return new trxReactiveStopAccrueHeader((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "StopAccrueService", "pSubmitRequest", hrData.NIP));
            }
            finally
            {
                context.Dispose();
            }
        }

        private trxReactiveStopAccrueHeader pClearDraft(trxReactiveStopAccrueHeader header)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var result = new trxReactiveStopAccrueHeader();
            var hrData = new Employee();
            var hrDataResult = hrData.GetDetailByUserID(header.Initiator);
            header.Initiator = hrDataResult.NIP;
            try
            {
                var repoDetail = new trxReactiveStopAccrueDetailRepository(context);
                var StopAccrueDetail = new List<trxReactiveStopAccrueDetail>();

                StopAccrueDetail = repoDetail.DeleteDraft(header.Initiator);
                return result;
            }
            catch (Exception ex)
            {
                return new trxReactiveStopAccrueHeader((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "StopAccrueService", "pClearDraft", hrData.NIP));
            }
            finally
            {
                context.Dispose();
            }
        }

        private trxReactiveStopAccrueHeader pSubmitEdit(List<trxReactiveStopAccrueDetail> detail, trxReactiveStopAccrueHeader header, string submitType)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var result = new trxReactiveStopAccrueHeader();
            var hrData = new Employee();
            var hrDataResult = hrData.GetDetailByUserID(header.Initiator);
            header.Initiator = hrDataResult.NIP;
            try
            {
                var repoHeader = new trxReactiveStopAccrueHeaderRepository(context);
                var StopAccrueHeader = new trxReactiveStopAccrueHeader();
                string deptCode = "";
                var mstDepartService = new MstDepartmentService();
                deptCode = mstDepartService.GetDepartment(new vwMstDepartment { DepartmentCode = hrDataResult.DepartmentCode }).DepartmentInitial;
                string requestNumber = header.RequestNumber;

                var activity = new Activity();
                var activityID = activity.GetDetail("STOP_ACCRUE", "SA_START");

                // job
                Job job = new Job();
                var datajob = job.GetDetail("STOP_ACCRUE");

                // wf header
                var workFlowService = new WorkFlowService();
                var wfHeader = new WfHeaders();
                wfHeader.JobID = datajob.JobID;
                // save wfheader
                var wf = new Header();
                wf.Initiator = header.Initiator;
                wf.Status = "rtr";
                wf.ActivityID = activityID.ActivityID;
                wf.CreateDate = System.DateTime.Now;
                wf.PrevActivityID = 0;
                wf.NextFlag = "submit";
                wf.JobID = datajob.JobID;
                wf.LastModifiedDate = System.DateTime.Now;
                wf.InitiatorJobTitle = hrDataResult.JobTitle;
                wf.InitiatorLocation = hrDataResult.PRL01;
                wf.ActivityOwner = "Routing";
                wf.AppHeaderID = 0;
                wf.PRL = hrDataResult.PRL;
                wf.LastModifiedBy = header.Initiator;
                wf.RequestNo = "";
                wf.Summary = header.Remarks;
                wf.IsApproved = false;
                wf.PotentialActivityOwner = "";
                wf.Save();
                wf.GetDetail(wf.AppHeaderID);
                TBGFramework.Common.Helper.SaveLog(wf.AppHeaderID, header.Remarks, "STOP_ACCRUE", wf.CreateDate, header.Initiator, "", "", wf.Label, "submit");
                
                /* create header data */
                /*start*/

                header.AppHeaderID = wf.AppHeaderID;
                header.RequestNumber = requestNumber;
                header.Status = "SUBMITED";
                header.DepartInitial = deptCode;
                header.DeptCode = hrDataResult.DepartmentCode;

                var headerDraft = new vwtrxStopAccrueHeaderDraft();
                headerDraft.Equals(header);

                StopAccrueHeader = repoHeader.UpdateHeader(header);

                StopAccrueHeader = repoHeader.CheckHeaderID(header);
                /*end*/

                /* create detail data */
                /*start*/

                var repoDetailDraft = new vwTrxStopAccrueDetailDraftRepository(context);
                var repoDetail = new trxReactiveStopAccrueDetailRepository(context);

                var StopAccrueDetail = new List<trxReactiveStopAccrueDetail>();
                detail = detail.Select(x => { x.TrxStopAccrueHeaderID = StopAccrueHeader.ID; return x; }).ToList();

                StopAccrueDetail = repoDetail.CreateBulkyEdit(detail);

                /*end*/

                /* create detail file data */
                /*start*/

                StopAccrueDetail = repoDetail.GetList("TrxStopAccrueHeaderID = " + StopAccrueHeader.ID, "");

                var repoDetailFile = new trxReactiveStopAccrueDetailFileRepository(context);
                var repoDetailFileDraft = new vwTrxStopAccrueDetailFileDraftRepository(context);

                var StopAccrueDetailFile = new List<trxReactiveStopAccrueDetailFile>();
                var StopAccrueDetailFileDraft = new List<vwTrxStopAccrueDetailFileDraft>();

                var queryDetail = from dtl1 in detail
                                  join dtl2 in StopAccrueDetail on dtl1.SONumber equals dtl2.SONumber
                                  select new
                                  {
                                      dtl2.ID,
                                      dtl1.FileName,
                                      dtl1.FilePath,
                                      dtl1.Remarks
                                  };
                
                foreach (var item in queryDetail)
                {
                    StopAccrueDetailFile.Add(new trxReactiveStopAccrueDetailFile
                    {
                        trxStopAccrueDetailID = item.ID,
                        FileName = item.FileName,
                        FilePath = item.FilePath,
                        CreatedDate = System.DateTime.Now,
                        Remarks = item.Remarks
                    });
                }
                StopAccrueDetailFile = repoDetailFile.CreateBulkyEdit(StopAccrueDetailFile);

                return result;

            }
            catch (Exception ex)
            {
                return new trxReactiveStopAccrueHeader((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "StopAccrueService", "pSubmitRequest", hrData.NIP));
            }
            finally
            {
                context.Dispose();
            }

        }

        // update Request Header
        private trxReactiveStopAccrueHeader pUpdateRequest(trxReactiveStopAccrueHeader header)
        {
            var result = new trxReactiveStopAccrueHeader();
            var hrdata = new Employee();
            string ActivityOwner = hrdata.GetDetailByUserID(header.ActivityOwner).NIP;

            try
            {
                var wfHeader = new Header();
                wfHeader.GetDetail(header.AppHeaderID);

                // validasio amount capex and amount revenue
                if (wfHeader.Label == "SA_APPR_DIV")
                {
                    if (!AmountCapexRevenueValidate(header.ID))
                    {
                        result.ErrorMessage = "Amount capex and revenue not complate!";
                        result.ErrorType = 1;
                        return result;
                    }
                }

                if (wfHeader.Label.ToUpper() == "SA_APPR_DIV_CHIEF" && header.NextFlag.ToUpper() == "APPROVE")
                {
                    header.NextFlag = header.NextFlag + " " + header.RequestType;
                }

                if (wfHeader.Label.ToUpper() == "SA_DOC_RECEIVE_DHAC" && header.NextFlag.ToUpper() == "RECEIVE")
                {
                    header.NextFlag = header.NextFlag + " " + header.RequestType;
                }

                if (wfHeader.Label == "SA_FEED_DIV_ACC" || wfHeader.Label == "SA_FEED_DEPT_ACC")
                    wfHeader.NextFlag = "Submit";
                else
                    wfHeader.NextFlag = header.NextFlag;

                if (wfHeader.Label.ToUpper() == "SA_DOC_RECEIVE_DHAS")
                    wfHeader.NextFlag = "Receive";




                if (string.IsNullOrEmpty(ActivityOwner))
                    ActivityOwner = header.ActivityOwner;

                wfHeader.Status = "rtr";
                wfHeader.ActivityOwner = "Routing";
                wfHeader.PotentialActivityOwner = "";
                wfHeader.Summary = header.Remarks;
                wfHeader.LastModifiedBy = ActivityOwner;
                wfHeader.Save();
                TBGFramework.Common.Helper.SaveLog(wfHeader.AppHeaderID, header.Remarks, "STOP_ACCRUE", System.DateTime.Now, ActivityOwner, "", "", wfHeader.Label, wfHeader.NextFlag); 

                var context = new DbContext(Helper.GetConnection("ARSystem"));
                var repoHeader = new trxReactiveStopAccrueHeaderRepository(context);
                if (header.NextFlag.ToLower() == "reject")
                {
                    header.Status = "REJECTED";
                    header = repoHeader.UpdateStatus(header);
                }

                if (header.NextFlag.ToUpper().Contains("RECEIVE") && header.RequestType.ToUpper() == "HOLD" && wfHeader.Label.ToUpper() == "SA_DOC_RECEIVE_DHAC")
                {
                    header.Status = "APPROVED";
                    header = repoHeader.UpdateStatus(header);
                }



                if (wfHeader.Label.ToUpper() == "SA_DOC_RECEIVE_DHAS")
                {
                    header.Status = "APPROVED";
                    header = repoHeader.UpdateStatus(header);
                }

                if (wfHeader.Label.ToUpper() == "SA_SUBMIT_DOC")
                {
                    header = repoHeader.UpdateStatusSubmitDoc(header);
                }

                return result;
            }
            catch (Exception ex)
            {
                return new trxReactiveStopAccrueHeader((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "StopAccrueService", "pUpdateRequest", ActivityOwner));

            }
        }

        private bool AmountCapexRevenueValidate(long ID)
        {
            var result = new List<vwReactiveStopAccrueDetail>();
            string whereClause = "";
            whereClause = "TrxStopAccrueHeaderID =" + ID + " AND   (RevenueAmount is null OR CapexAmount is null) AND IsHold=1 ";
            result = pGetDetailRequestList(whereClause);
            if (result.Count > 0)
                return false;
            else
                return true;

        }

        // get header request list
        public List<vwReactiveStopAccrueHeader> GetHeaderRequestList(vwReactiveStopAccrueHeader param, int rowSkip, int pageSize)
        {
            return pGetHeaderRequestList(param, rowSkip, pageSize);
        }

        // get header request list
        public List<vwReactiveStopAccrueHeader> GetHeaderRequestList(vwReactiveStopAccrueHeader param)
        {
            return pGetHeaderRequestList(param);
        }

        // get header request list
        public List<vwStopAccrueHeader> GetRequestUpdateAmountList(vwStopAccrueHeader param, int rowSkip, int pageSize)
        {
            return pGetRequestUpdateAmountList(param, rowSkip, pageSize);
        }

        // get count header
        public int GetCountHeaderRequest(vwStopAccrueHeader param)
        {
            return pGetCountHeaderRequest(param);
        }


        public int GetCountRequestUpdateAmountList(vwStopAccrueHeader param)
        {
            return pGetCountRequestUpdateAmountList(param);
        }

        public List<vwStopAccrueDetailPrint> GetStopAccrueDetailPrint(int ID)
        {
            return pGetStopAccrueDetailPrint(ID);
        }

        // get header request
        private List<vwReactiveStopAccrueHeader> pGetHeaderRequestList(vwReactiveStopAccrueHeader param, int rowSkip, int pageSize)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var contextTBGSys = new DbContext(Helper.GetConnection("TBGFlow"));
            List<vwReactiveStopAccrueHeader> data = new List<vwReactiveStopAccrueHeader>();
            try
            {

                var repo = new vwReactiveStopAccrueHeaderRepository(context);
                //data = repo.GetPaged(whereClause, "CreatedDate DESC", rowSkip, pageSize);

                data = repo.GetList(HeaderRequestWhereClause(param), "");
                var repoTBGSys = new WfHeaders_ViewRepository(contextTBGSys);
                var wfHeaders = new List<WfHeaders_View>();


                wfHeaders = repoTBGSys.GetList(WfHeaderWhereClause(param));
                var list = (from request in data
                            join wfHeader in wfHeaders on request.AppHeaderID equals wfHeader.AppHeaderID
                            select new
                            {
                                request.ID,
                                wfHeader.AppHeaderID,
                                request.RequestNumber,
                                request.Initiator,
                                request.InitiatorName,
                                request.RequestTypeID,
                                request.StartEffectiveDate,
                                request.EndEffectiveDate,
                                wfHeader.ActivityID,
                                request.CreatedDate,
                                wfHeader.Activity,
                                wfHeader.Label,
                                request.AccrueType,
                                wfHeader.ActivityOwnerName,
                                wfHeader.ActivityOwner,
                                request.IsReHold,
                                request.IsReHoldReady,
                                request.FileName
                            }).OrderByDescending(x => x.AppHeaderID).ToArray();


                int indexRow = rowSkip + 1;
                if (pageSize != 0)
                    list = list.Skip(rowSkip).Take(pageSize).ToArray();

                data = new List<vwReactiveStopAccrueHeader>();
                foreach (var item in list)
                {
                    data.Add(new vwReactiveStopAccrueHeader
                    {
                        RowIndex = indexRow,
                        ID = item.ID,
                        AppHeaderID = item.AppHeaderID,
                        RequestNumber = item.RequestNumber,
                        Initiator = item.Initiator,
                        InitiatorName = item.InitiatorName,
                        ActivityOwner = item.ActivityOwner,
                        ActivityOwnerName = item.ActivityOwnerName,
                        RequestTypeID = item.RequestTypeID,
                        StartEffectiveDate = item.StartEffectiveDate,
                        EndEffectiveDate = item.EndEffectiveDate,
                        ActivityID = item.ActivityID,
                        ActivityName = item.Activity,
                        CreatedDate = item.CreatedDate,
                        ActivityLabel = item.Label,
                        AccrueType = item.AccrueType,
                        IsReHold = item.IsReHold,
                        IsReHoldReady = item.IsReHoldReady,
                        FileName = item.FileName
                    });
                    indexRow++;
                }
            }
            catch (Exception ex)
            {
                context.Dispose();
                data.Add(new vwReactiveStopAccrueHeader { ErrorMessage = ex.Message });
            }

            return data;
        }

        private List<vwStopAccrueHeader> pGetRequestUpdateAmountList(vwStopAccrueHeader param, int rowSkip, int pageSize)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var contextTBGSys = new DbContext(Helper.GetConnection("TBGFlow"));
            List<vwStopAccrueHeader> data = new List<vwStopAccrueHeader>();
            try
            {

                var repo = new vwStopAccrueHeaderRepository(context);
                //data = repo.GetPaged(whereClause, "CreatedDate DESC", rowSkip, pageSize);

                data = repo.GetList(HeaderRequestUpdateAmountWhereClause(param), "");
                var repoTBGSys = new WfHeaders_ViewRepository(contextTBGSys);
                var wfHeaders = new List<WfHeaders_View>();


                wfHeaders = repoTBGSys.GetList(WfHeaderUpdateAmountWhereClause(param));
                var list = (from request in data
                            join wfHeader in wfHeaders on request.AppHeaderID equals wfHeader.AppHeaderID
                            select new
                            {
                                request.ID,
                                wfHeader.AppHeaderID,
                                request.RequestNumber,
                                request.Initiator,
                                request.InitiatorName,
                                request.RequestTypeID,
                                request.StartEffectiveDate,
                                request.EndEffectiveDate,
                                wfHeader.ActivityID,
                                request.CreatedDate,
                                wfHeader.Activity,
                                wfHeader.Label,
                                request.AccrueType,
                                wfHeader.ActivityOwnerName,
                                request.IsReHold
                            }).OrderByDescending(x => x.AppHeaderID).ToArray();


                int indexRow = rowSkip + 1;

                if (pageSize != 0)
                    list = list.Skip(rowSkip).Take(pageSize).ToArray();

                data = new List<vwStopAccrueHeader>();
                foreach (var item in list)
                {
                    data.Add(new vwStopAccrueHeader
                    {
                        RowIndex = indexRow,
                        ID = item.ID,
                        AppHeaderID = item.AppHeaderID,
                        RequestNumber = item.RequestNumber,
                        Initiator = item.Initiator,
                        InitiatorName = item.InitiatorName,
                        ActivityOwner = item.ActivityOwnerName,
                        RequestTypeID = item.RequestTypeID,
                        StartEffectiveDate = item.StartEffectiveDate,
                        EndEffectiveDate = item.EndEffectiveDate,
                        ActivityID = item.ActivityID,
                        ActivityName = item.Activity,
                        CreatedDate = item.CreatedDate,
                        ActivityLabel = item.Label,
                        AccrueType = item.AccrueType,
                        IsReHold = item.IsReHold
                    });
                    indexRow++;
                }
            }
            catch (Exception ex)
            {
                context.Dispose();
                data.Add(new vwStopAccrueHeader { ErrorMessage = ex.Message });
            }

            return data;
        }

        // get header request
        private List<vwReactiveStopAccrueHeader> pGetHeaderRequestList(vwReactiveStopAccrueHeader param)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var contextTBGSys = new DbContext(Helper.GetConnection("TBGFlow"));
            List<vwReactiveStopAccrueHeader> data = new List<vwReactiveStopAccrueHeader>();
            try
            {
                var repo = new vwReactiveStopAccrueHeaderRepository(context);
                data = repo.GetList(HeaderRequestWhereClause(param), "CreatedDate DESC");

                var repoTBGSys = new WfHeaders_ViewRepository(contextTBGSys);
                var wfHeaders = new List<WfHeaders_View>();
                wfHeaders = repoTBGSys.GetList(WfHeaderWhereClause(param));
                var list = (from request in data
                            join wfHeader in wfHeaders on request.AppHeaderID equals wfHeader.AppHeaderID
                            select new
                            {
                                request.ID,
                                wfHeader.AppHeaderID,
                                request.RequestNumber,
                                request.Initiator,
                                request.InitiatorName,
                                request.RequestTypeID,
                                request.StartEffectiveDate,
                                request.EndEffectiveDate,
                                wfHeader.ActivityID,
                                request.CreatedDate,
                                wfHeader.Activity,
                                wfHeader.Label,
                                request.AccrueType,
                                wfHeader.ActivityOwnerName,
                                wfHeader.ActivityOwner
                            }).ToArray();


                data = new List<vwReactiveStopAccrueHeader>();
                foreach (var item in list)
                {
                    data.Add(new vwReactiveStopAccrueHeader
                    {

                        ID = item.ID,
                        AppHeaderID = item.AppHeaderID,
                        RequestNumber = item.RequestNumber,
                        Initiator = item.Initiator,
                        InitiatorName = item.InitiatorName,
                        ActivityOwner = item.ActivityOwner,
                        ActivityOwnerName = item.ActivityOwnerName,
                        RequestTypeID = item.RequestTypeID,
                        StartEffectiveDate = item.StartEffectiveDate,
                        EndEffectiveDate = item.EndEffectiveDate,
                        ActivityID = item.ActivityID,
                        ActivityName = item.Activity,
                        CreatedDate = item.CreatedDate,
                        ActivityLabel = item.Label,
                        AccrueType = item.AccrueType,

                    });
                }



            }
            catch (Exception ex)
            {
                context.Dispose();
                data.Add(new vwReactiveStopAccrueHeader { ErrorMessage = ex.Message });
            }

            return data;
        }

        // get count header
        private int pGetCountHeaderRequest(vwStopAccrueHeader param)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var repo = new vwStopAccrueHeaderRepository(context);
            try
            {
                var contextTBGSys = new DbContext(Helper.GetConnection("TBGFlow"));
                List<vwStopAccrueHeader> data = new List<vwStopAccrueHeader>();
                var repoTBGSys = new WfHeaders_ViewRepository(contextTBGSys);
                var wfHeaders = new List<WfHeaders_View>();
                data = repo.GetList(HeaderRequestWhereClause(param), "CreatedDate DESC");
                wfHeaders = repoTBGSys.GetList(WfHeaderWhereClause(param));
                var list = (from request in data
                            join wfHeader in wfHeaders on request.AppHeaderID equals wfHeader.AppHeaderID
                            select new { request.AppHeaderID }).ToArray();

                //foreach (var item in list)
                //{
                //    var app = item.AppHeaderID;
                //}
                return list.Count();
            }
            catch (Exception ex)
            {
                context.Dispose();
                return 0;
            }

        }

        private int pGetCountRequestUpdateAmountList(vwStopAccrueHeader param)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var repo = new vwStopAccrueHeaderRepository(context);
            try
            {
                var contextTBGSys = new DbContext(Helper.GetConnection("TBGFlow"));
                List<vwStopAccrueHeader> data = new List<vwStopAccrueHeader>();
                var repoTBGSys = new WfHeaders_ViewRepository(contextTBGSys);
                var wfHeaders = new List<WfHeaders_View>();
                data = repo.GetList(HeaderRequestUpdateAmountWhereClause(param), "CreatedDate DESC");
                wfHeaders = repoTBGSys.GetList(WfHeaderUpdateAmountWhereClause(param));
                var list = (from request in data
                            join wfHeader in wfHeaders on request.AppHeaderID equals wfHeader.AppHeaderID
                            select new { request.AppHeaderID }).ToArray();

                foreach (var item in list)
                {
                    var app = item.AppHeaderID;
                }
                return list.Count();
            }
            catch (Exception ex)
            {
                context.Dispose();
                return 0;
            }

        }

        // where clause condition
        private string HeaderRequestWhereClause(vwStopAccrueHeader param)
        {

            string Initiator = "";
            //var hrdata = new Employee();
            //Initiator = hrdata.GetDetailByUserID(param.Initiator).NIP;

            //if (string.IsNullOrEmpty(Initiator))
            Initiator = param.Initiator;


            string whereClause = "";
            if (param.RequestTypeID != 0)
                whereClause += "RequestTypeID =" + param.RequestTypeID + " AND ";

            if (param.UserRole == "DEPT_HEAD")
            {
                if (!string.IsNullOrEmpty(Initiator))
                    whereClause += "Initiator = '" + Initiator + "' AND ";



            }

            if (!string.IsNullOrEmpty(param.RequestNumber))
                whereClause += "RequestNumber like '%" + param.RequestNumber + "%' AND ";
            if (!string.IsNullOrEmpty(param.StartEffectiveDate.ToString()))
                whereClause += "StartEffectiveDate ='" + String.Format("{0:yyyy-MM-dd}", param.StartEffectiveDate) + "' AND ";
            if (!string.IsNullOrEmpty(param.EndEffectiveDate.ToString()))
                whereClause += "EndEffectiveDate ='" + String.Format("{0:yyyy-MM-dd}", param.EndEffectiveDate) + "' AND ";
            if (!string.IsNullOrEmpty(param.CreatedDate.ToString()))
                whereClause += "CAST(CreatedDate as date) =CAST('" + String.Format("{0:yyyy-MM-dd}", param.CreatedDate) + "' as Date) AND ";

            whereClause = !string.IsNullOrWhiteSpace(whereClause) ? whereClause.Substring(0, whereClause.Length - 5) : "";
            return whereClause;
        }

        private string HeaderRequestUpdateAmountWhereClause(vwStopAccrueHeader param)
        {
            string Initiator = "";
            //var hrdata = new Employee();
            //Initiator = hrdata.GetDetailByUserID(param.Initiator).NIP;

            //if (string.IsNullOrEmpty(Initiator))
            Initiator = param.Initiator;

            string whereClause = "Status ='SUBMITED' AND ";

            if (param.UserRole != null && param.UserRole.Equals("DEPT_HEAD"))
            {
                whereClause += "Initiator='" + Initiator + "' AND ";
            }

            if (param.RequestTypeID != 0)
                whereClause += "RequestTypeID =" + param.RequestTypeID + " AND ";

            if (!string.IsNullOrEmpty(param.RequestNumber))
                whereClause += "RequestNumber like '%" + param.RequestNumber + "%' AND ";


            if (!string.IsNullOrEmpty(param.StartEffectiveDate.ToString()))
                whereClause += "StartEffectiveDate ='" + String.Format("{0:yyyy-MM-dd}", param.StartEffectiveDate) + "' AND ";

            if (!string.IsNullOrEmpty(param.CreatedDate.ToString()))
                whereClause += "CreatedDate ='" + String.Format("{0:yyyy-MM-dd}", param.CreatedDate) + "' AND ";

            whereClause = !string.IsNullOrWhiteSpace(whereClause) ? whereClause.Substring(0, whereClause.Length - 5) : "";
            return whereClause;
        }

        // where clause condition
        private string WfHeaderWhereClause(vwStopAccrueHeader param)
        {

            string ActivityOwner = "";
            string Initiator = "";

            //var hrdata = new Employee();
            //ActivityOwner = hrdata.GetDetailByUserID(param.ActivityOwner).NIP;
            //Initiator = hrdata.GetDetailByUserID(param.Initiator).NIP;
            string whereClause = "";


            //if (string.IsNullOrEmpty(Initiator))
            Initiator = param.Initiator;
            ActivityOwner = param.Initiator;

            whereClause += "Code ='STOP_ACCRUE' AND ";

            if (param.ActivityID != 0)
                whereClause += "ActivityID =" + param.ActivityID + " AND ";

            if (param.UserRole == "DEPT_HEAD")
            {
                if (!string.IsNullOrEmpty(Initiator))
                    whereClause += "Initiator = '" + Initiator + "' AND ";


                if (!string.IsNullOrEmpty(param.ActivityOwnerName))
                    whereClause += "ActivityOwnerName like '%" + param.ActivityOwnerName + "%' AND ";
            }
            else
            {
                if (!string.IsNullOrEmpty(param.InitiatorName))
                    whereClause += "InitiatorName like '%" + param.InitiatorName + "%' AND ";

                if (!string.IsNullOrEmpty(ActivityOwner))
                    whereClause += "ActivityOwner = '" + ActivityOwner + "' AND ";
            }

            //if (param.UserRole != "DEPT_HEAD")
            //{
            //    if (!string.IsNullOrEmpty(param.ActivityOwner))
            //        whereClause += "ActivityOwner ='" + param.ActivityOwner + "' AND ";
            //}                                  }
            //else
            //{
            //    if (!string.IsNullOrEmpty(param.Initiator))
            //        whereClause += "Initiator ='" + param.Initiator + "' AND ";

            //    //if (!string.IsNullOrEmpty(param.ActivityOwner))
            //    //    whereClause += "ActivityOwnerName like '%" + param.ActivityOwner + "%' AND ";
            //}


            whereClause = !string.IsNullOrWhiteSpace(whereClause) ? whereClause.Substring(0, whereClause.Length - 5) : "";
            return whereClause;
        }

        private string WfHeaderUpdateAmountWhereClause(vwStopAccrueHeader param)
        {

            //string whereClause = "Label = 'SA_APPR_DIV' ";

            string whereClause = "Label IN ('SA_APPR_DIV', 'SA_APPR_DIV_CHIEF') ";

            // whereClause = !string.IsNullOrWhiteSpace(whereClause) ? whereClause.Substring(0, whereClause.Length - 5) : "";
            return whereClause;
        }


        // header request save
        public trxReactiveStopAccrueHeader HeaderRequestSave(trxReactiveStopAccrueHeader data)
        {
            return pHeaderRequestSave(data);
        }

        // header request save
        private trxReactiveStopAccrueHeader pHeaderRequestSave(trxReactiveStopAccrueHeader data)
        {
            var result = new trxReactiveStopAccrueHeader();
            try
            {
                var context = new DbContext(Helper.GetConnection("ARSystem"));
                var repo = new trxReactiveStopAccrueHeaderRepository(context);

                if (data.ID == 0)
                {
                    result = repo.Create(data);
                }
                else
                {
                    result = repo.Update(data);
                }
            }
            catch (Exception ex)
            {

                result.ErrorMessage = ex.Message;
                return new trxReactiveStopAccrueHeader((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "StopAccrueService", "pUpdateRequest", data.ActivityOwner));
            }

            return result;
        }

        private List<vwStopAccrueDetailPrint> pGetStopAccrueDetailPrint(int ID)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            List<vwStopAccrueDetailPrint> data = new List<vwStopAccrueDetailPrint>();
            try
            {
                var repo = new vwStopAccrueDetailPrintRepository(context);

                data = repo.GetList("TrxStopAccrueHeaderID=" + ID, "");


            }
            catch (Exception ex)
            {
                context.Dispose();
                data.Add(new vwStopAccrueDetailPrint { ErrorMessage = ex.Message });
            }

            return data;
        }

        #endregion

        #region  Request Detail
        public List<vwReactiveStopAccrueDetail> GetDetailRequestList(int HeaderID)
        {
            return pGetDetailRequestList(DetailRequestWhereClause(HeaderID));
        }
        public int GetCountDetailRequest(int HeaderID)
        {
            return pGetCountDetailRequest(DetailRequestWhereClause(HeaderID));
        }
        private List<vwReactiveStopAccrueDetail> pGetDetailRequestList(string whereClause)
        {
            List<vwReactiveStopAccrueDetail> data = new List<vwReactiveStopAccrueDetail>();
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            try
            {

                var repo = new vwReactiveStopAccrueDetailRepository(context);
                data = repo.GetList(whereClause, "ID ASC");
            }
            catch (Exception ex)
            {
                context.Dispose();
                data.Add(new vwReactiveStopAccrueDetail { ErrorMessage = ex.Message });
            }

            return data;
        }
        private int pGetCountDetailRequest(string whereClause)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            try
            {

                var repo = new trxReactiveStopAccrueDetailRepository(context);
                return repo.GetCount(whereClause);
            }
            catch (Exception ex)
            {
                context.Dispose();
                return 0;
            }

        }
        private string DetailRequestWhereClause(int headerID)
        {
            string whereClause = "";
            whereClause = "TrxStopAccrueHeaderID =" + headerID + " ";
            return whereClause;
        }
        public trxReactiveStopAccrueDetail DetailRequestSave(trxReactiveStopAccrueDetail data)
        {
            return pDetailRequestSave(data);
        }
        private trxReactiveStopAccrueDetail pDetailRequestSave(trxReactiveStopAccrueDetail data)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var result = new trxReactiveStopAccrueDetail();
            try
            {

                var repo = new trxReactiveStopAccrueDetailRepository(context);

                if (data.ID == 0)
                {
                    result = repo.Create(data);
                }
                else
                {
                    result = repo.Update(data);
                }
            }
            catch (Exception ex)
            {
                context.Dispose();
                result.ErrorMessage = ex.Message;
                return new trxReactiveStopAccrueDetail((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "StopAccrueService", "pDetailRequestSave", data.TrxStopAccrueHeaderID.ToString()));
            }
            finally
            {
                context.Dispose();
            }

            return result;
        }

        public trxReactiveStopAccrueDetail UpdateAmountCapexRevenue(List<trxReactiveStopAccrueDetail> data)
        {
            return pUpdateAmountCapexRevenue(data);
        }
        private trxReactiveStopAccrueDetail pUpdateAmountCapexRevenue(List<trxReactiveStopAccrueDetail> data)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var result = new trxReactiveStopAccrueDetail();
            try
            {

                var repo = new trxReactiveStopAccrueDetailRepository(context);

                foreach (var item in data)
                {
                    var dataItem = new trxReactiveStopAccrueDetail();
                    dataItem.TrxStopAccrueHeaderID = item.TrxStopAccrueHeaderID;
                    dataItem.SONumber = item.SONumber;
                    dataItem.RevenueAmount = item.RevenueAmount;
                    dataItem.CapexAmount = item.CapexAmount;
                    result = repo.UpdateCapexRevenue(dataItem);
                }

            }
            catch (Exception ex)
            {
                context.Dispose();
                //    result.ErrorMessage = ex.Message;
                return new trxReactiveStopAccrueDetail((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "StopAccrueService", "pUpdateAmountCapexRevenue", data[0].TrxStopAccrueHeaderID.ToString()));
            }

            return result;
        }



        #endregion

        #region  Request Detail  File
        public List<trxReactiveStopAccrueDetailFile> GetDetailFileRequestList(int DetailID, int rowSkip, int pageSize)
        {
            return pGetDetailFileRequestList(DetailFileRequestWhereClause(DetailID), rowSkip, pageSize);
        }
        public int GetCountDetailFileRequest(int DetailID)
        {
            return pGetCountDetailFileRequest(DetailFileRequestWhereClause(DetailID));
        }
        private List<trxReactiveStopAccrueDetailFile> pGetDetailFileRequestList(string whereClause, int rowSkip, int pageSize)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            List<trxReactiveStopAccrueDetailFile> data = new List<trxReactiveStopAccrueDetailFile>();
            try
            {

                var repo = new trxReactiveStopAccrueDetailFileRepository(context);
                data = repo.GetPaged(whereClause, "CreatedDate DESC", rowSkip, pageSize);
            }
            catch (Exception ex)
            {
                context.Dispose();
                data.Add(new trxReactiveStopAccrueDetailFile { ErrorMessage = ex.Message });
            }

            return data;
        }
        private int pGetCountDetailFileRequest(string whereClause)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            try
            {

                var repo = new trxStopAccrueDetailFileRepository(context);
                return repo.GetCount(whereClause);
            }
            catch (Exception ex)
            {
                context.Dispose();
                return 0;
            }

        }
        private string DetailFileRequestWhereClause(int DetailID)
        {
            string whereClause = "";
            whereClause = "trxStopAccrueDetailID ='" + DetailID + "' AND ";
            return whereClause;
        }
        public trxReactiveStopAccrueDetailFile DetailFileRequestSave(trxReactiveStopAccrueDetailFile data)
        {
            return pDetailFileRequestSave(data);
        }
        private trxReactiveStopAccrueDetailFile pDetailFileRequestSave(trxReactiveStopAccrueDetailFile data)
        {
            var result = new trxReactiveStopAccrueDetailFile();
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            try
            {
                var repo = new trxReactiveStopAccrueDetailFileRepository(context);
                if (data.ID == 0)
                {
                    result = repo.Create(data);
                }
                else
                {
                    result = repo.Update(data);
                }
            }
            catch (Exception ex)
            {
                context.Dispose();
                //  result.ErrorMessage = ex.Message;
                return new trxReactiveStopAccrueDetailFile((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "StopAccrueService", "pDetailFileRequestSave", data.trxStopAccrueDetailID.ToString()));
            }

            return result;
        }
        #endregion

        #region Log Activity

        public List<vwWfHeaderActivityLogs> GetListActivityLogs(int headerID)
        {
            return pGetListActivityLogs(headerID);
        }
        private List<vwWfHeaderActivityLogs> pGetListActivityLogs(int headerID)
        {
            var context = new DbContext(Helper.GetConnection("TBGFlow"));
            List<vwWfHeaderActivityLogs> data = new List<vwWfHeaderActivityLogs>();
            try
            {

                var repo = new vwWfHeaderActivityLogsRepository(context);
                string whereClause = "AppHeaderID =" + headerID + " AND EventCode='STOP_ACCRUE' ";
                data = repo.GetList(whereClause, "LogID DESC");
            }
            catch (Exception ex)
            {
                context.Dispose();
                data.Add(new vwWfHeaderActivityLogs { ErrorMessage = ex.Message });
            }

            return data;
        }

        #endregion

        #region Master Data
        public List<mstStopAccrueDetailCase> StopAccrueDetailCase(int ID)
        {
            return pStopAccrueDetailCase(ID);
        }
        private List<mstStopAccrueDetailCase> pStopAccrueDetailCase(int ID)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            List<mstStopAccrueDetailCase> data = new List<mstStopAccrueDetailCase>();
            try
            {

                var repo = new mstStopAccrueDetailCaseRepository(context);
                data = repo.GetList("CategoryCaseID = " + ID);
                data = data.Where(x => x.IsActive == true).ToList();
            }
            catch (Exception ex)
            {
                context.Dispose();
                data.Add(new mstStopAccrueDetailCase { ErrorMessage = ex.Message });
            }

            return data;
        }
        public List<mstStopAccrueCategoryCase> StopAccrueCategoryCase()
        {
            return pStopAccrueCategoryCase();
        }
        private List<mstStopAccrueCategoryCase> pStopAccrueCategoryCase()
        {
            List<mstStopAccrueCategoryCase> data = new List<mstStopAccrueCategoryCase>();
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            try
            {

                var repo = new mstStopAccrueCategoryCaseRepository(context);
                data = repo.GetList("", "");
            }
            catch (Exception ex)
            {
                context.Dispose();
                data.Add(new mstStopAccrueCategoryCase { ErrorMessage = ex.Message });
            }

            return data;
        }
        public List<mstStopAccrueType> StopAccrueType()
        {
            return pStopAccrueType();
        }
        private List<mstStopAccrueType> pStopAccrueType()
        {
            List<mstStopAccrueType> data = new List<mstStopAccrueType>();
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            try
            {

                var repo = new mstStopAccrueTypeRepository(context);
                data = repo.GetList("", "");
            }
            catch (Exception ex)
            {
                context.Dispose();
                data.Add(new mstStopAccrueType { ErrorMessage = ex.Message });
            }

            return data;
        }
        public List<mstStopAccrueApprovalStatus> ApprovalStatus(mstStopAccrueApprovalStatus param)
        {
            return pApprovalStatus(param);
        }
        private List<mstStopAccrueApprovalStatus> pApprovalStatus(mstStopAccrueApprovalStatus param)
        {
            var data = new List<mstStopAccrueApprovalStatus>();
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            try
            {
                var repo = new mstStopAccrueApprovalStatusRepository(context);
                data = repo.GetList("RoleLabel ='" + param.RoleLabel + "' AND ActivityLabel='" + param.ActivityLabel + "' ", "Sort ASC ");
                data = data.Where(x => x.IsShowUp == true).ToList();
            }
            catch (Exception ex)
            {
                context.Dispose();
                data.Add(new mstStopAccrueApprovalStatus { ErrorMessage = ex.Message });
            }

            return data;
        }


        //public vwMstStopAccrueNextActivity MstStopAccrueNextActivity(vwMstStopAccrueNextActivity param)
        //{
        //    return pMstStopAccrueNextActivity(param);
        //}

        //public List<mstStopAccrueNextFlag> StopAccrueNextFlag(int param)
        //{
        //    return pStopAccrueNextFlag(param);
        //}
        //private List<mstStopAccrueNextFlag> pStopAccrueNextFlag(int param)
        //{
        //    List<mstStopAccrueNextFlag> data = new List<mstStopAccrueNextFlag>();
        //    var context = new DbContext(Helper.GetConnection("ARSystem"));
        //    try
        //    {

        //        var repo = new mstStopAccrueNextFlagRepository(context);
        //        string whereClause = "RoleID = " + param;
        //        data = repo.GetList(whereClause, "");
        //    }
        //    catch (Exception ex)
        //    {
        //        context.Dispose();
        //        data.Add(new mstStopAccrueNextFlag { ErrorMessage = ex.Message });
        //    }

        //    return data;
        //}
        public mstStopAccrueRoleLabel StopAccrueRoleLabel(int param)
        {
            return pStopAccrueRoleLabel(param);
        }
        private mstStopAccrueRoleLabel pStopAccrueRoleLabel(int param)
        {
            mstStopAccrueRoleLabel data = new mstStopAccrueRoleLabel();
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            try
            {

                var repo = new mstStopAccrueRoleLabelRepository(context);
                string whereClause = "RoleID = " + param;
                data = repo.GetList(whereClause, "").FirstOrDefault();
            }
            catch (Exception ex)
            {
                context.Dispose();
                data.ErrorMessage = ex.Message;
            }

            return data;
        }
        public List<WfPrDef_NextFlag> NextFlag(int param)
        {
            return pNextFlag(param);
        }
        private List<WfPrDef_NextFlag> pNextFlag(int param)
        {
            var data = new List<WfPrDef_NextFlag>();
            var context = new DbContext(Helper.GetConnection("TBGFlow"));
            try
            {
                var repo = new WfPrDef_NextFlagRepository(context);
                string whereClause = "FromActivityID = " + param;
                data = repo.GetList(whereClause, "");
            }
            catch (Exception ex)
            {
                context.Dispose();
                data.Add(new WfPrDef_NextFlag { ErrorMessage = ex.Message });
            }

            return data;
        }
        #endregion

        #region Dashboard

        public List<vwReactiveStopAccrueDashboardHeader> GetDashboardHeaderList(vwReactiveStopAccrueDashboardHeader param, int rowSkip, int pageSize)
        {
            return pGetDashboardHeaderList(param, rowSkip, pageSize);
        }
        public List<vwReactiveStopAccrueDashboardHeader> pGetDashboardHeaderListToExcel(vwReactiveStopAccrueDashboardHeader param, int rowSkip, int pageSize)
        {
            return pGetDashboardHeaderListToExcel(param, rowSkip, pageSize);
        }
        public int GetCountDashboardHeader(vwReactiveStopAccrueDashboardHeader param)
        {
            return pGetCountDashboardHeader(param);
        }
        public List<vwReactiveStopAccrueDashboardDetail> GetDashboardDetailList(vwReactiveStopAccrueDashboardDetail param, int rowSkip, int pageSize)
        {
            return pGetDashboardDetailList(param, rowSkip, pageSize);
        }
        public int GetCountDashboardDetail(vwReactiveStopAccrueDashboardDetail param)
        {
            return pGetCountDashboardDetail(param);
        }
        private List<vwReactiveStopAccrueDashboardHeader> pGetDashboardHeaderList(vwReactiveStopAccrueDashboardHeader param, int rowSkip, int pageSize)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            List<vwReactiveStopAccrueDashboardHeader> data = new List<vwReactiveStopAccrueDashboardHeader>();
            try
            {
                var repo = new vwReactiveStopAccrueDashboardHeaderRepository(context);
                //if (pageSize > 0)
                //    data = repo.GetPaged(WhereClauseDashboardHeader(param), "ID DESC", rowSkip, pageSize);
                //else
                data = repo.GetList(WhereClauseDashboardHeader(param), "ID DESC");

                var contextTBGSys = new DbContext(Helper.GetConnection("TBGFlow"));
                var wfHeaders = new List<WfHeaders_View>();
                var repoTBGSys = new WfHeaders_ViewRepository(contextTBGSys);

                var vwStopAccrueHeader = new vwReactiveStopAccrueHeader();
                vwStopAccrueHeader.ActivityID = param.ActivityID;
                string whereclause = " Code = 'STOP_ACCRUE' AND Status IN('inp','don1') ";
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

                data = new List<vwReactiveStopAccrueDashboardHeader>();
                foreach (var item in datalist)
                {
                    data.Add(new vwReactiveStopAccrueDashboardHeader
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
                var dataSum = new vwReactiveStopAccrueDashboardHeader();
                dataSum.RowIndex = "Total";
                dataSum.RequestNumber = "";
                dataSum.DepartName = "";
                dataSum.SoNumberCount = data.Sum(x => x.SoNumberCount);
                dataSum.SumRevenue = data.Sum(x => x.SumRevenue);
                dataSum.SumCapex = data.Sum(x => x.SumCapex);
                dataSum.RequestTypeID = 0;
                dataSum.CraetedDate = null;

                data.Add(dataSum);

            }
            catch (Exception ex)
            {
                context.Dispose();
                data.Add(new vwReactiveStopAccrueDashboardHeader { ErrorMessage = ex.Message });
            }

            return data; 
        }

        private List<vwReactiveStopAccrueDashboardHeader> pGetDashboardHeaderListToExcel(vwReactiveStopAccrueDashboardHeader param)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            List<vwReactiveStopAccrueDashboardHeader> data = new List<vwReactiveStopAccrueDashboardHeader>();
            try
            {
                var repo = new vwReactiveStopAccrueDashboardHeaderRepository(context);
                //if (pageSize > 0)
                //    data = repo.GetPaged(WhereClauseDashboardHeader(param), "ID DESC", rowSkip, pageSize);
                //else
                data = repo.GetList(WhereClauseDashboardHeader(param), "ID DESC");

                var contextTBGSys = new DbContext(Helper.GetConnection("TBGFlow"));
                var wfHeaders = new List<WfHeaders_View>();
                var repoTBGSys = new WfHeaders_ViewRepository(contextTBGSys);

                var vwStopAccrueHeader = new vwReactiveStopAccrueHeader();
                vwStopAccrueHeader.ActivityID = param.ActivityID;
                string whereclause = " Code = 'STOP_ACCRUE' AND Status IN('inp','don1') ";
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


                data = new List<vwReactiveStopAccrueDashboardHeader>();
                foreach (var item in datalist)
                {
                    data.Add(new vwReactiveStopAccrueDashboardHeader
                    {

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
                }
                var dataSum = new vwReactiveStopAccrueDashboardHeader();
                dataSum.RowIndex = "Total";
                dataSum.RequestNumber = "";
                dataSum.DepartName = "";
                dataSum.SoNumberCount = data.Sum(x => x.SoNumberCount);
                dataSum.SumRevenue = data.Sum(x => x.SumRevenue);
                dataSum.SumCapex = data.Sum(x => x.SumCapex);
                dataSum.RequestTypeID = 0;
                dataSum.CraetedDate = null;

                data.Add(dataSum);

            }
            catch (Exception ex)
            {
                context.Dispose();
                data.Add(new vwReactiveStopAccrueDashboardHeader { ErrorMessage = ex.Message });
            }

            return data;
        }

        private int pGetCountDashboardHeader(vwReactiveStopAccrueDashboardHeader param)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));

            try
            {
                var data = new List<vwReactiveStopAccrueDashboardHeader>();
                var repo = new vwReactiveStopAccrueDashboardHeaderRepository(context);


                data = repo.GetList(WhereClauseDashboardHeader(param), "ID DESC");
                 
                var contextTBGSys = new DbContext(Helper.GetConnection("TBGFlow"));
                var wfHeaders = new List<WfHeaders_View>();
                var repoTBGSys = new WfHeaders_ViewRepository(contextTBGSys);
                var vwStopAccrueHeader = new vwReactiveStopAccrueHeader();
                vwStopAccrueHeader.ActivityID = param.ActivityID;

                string whereclause = " Code = 'STOP_ACCRUE' AND Status IN('inp','don1') ";
                whereclause += " AND " + (param.Activity.ToLower() == "finish" ? "Label='SA_STOP'" : "Label in ('SA_APPR_DIV','SA_APPR_DEPT_ACC','SA_APPR_DIV_ACC','SA_FEED_DIV_ACC','SA_FEED_DEPT_ACC','SA_SUBMIT_DOC','SA_DOC_RECEIVE_DHAC','SA_DOC_RECEIVE_DHAS','SA_APPR_DIV_CHIEF','SA_APPR_CHIEF_MKT')");

                wfHeaders = repoTBGSys.GetList(whereclause);

                var datalist = (from request in data
                                join wfHeader in wfHeaders on request.AppHeaderID equals wfHeader.AppHeaderID
                                select new
                                {
                                    wfHeader.AppHeaderID
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
        private string WhereClauseDashboardHeader(vwReactiveStopAccrueDashboardHeader param)
        {
            string whereClause = "";

            if (param.RequestTypeID != 0 && param.RequestTypeID != null)
                whereClause += "RequestTypeID ='" + param.RequestTypeID + "' AND ";


            if (!string.IsNullOrEmpty(param.RequestNumber))
                whereClause += "RequestNumber like '%" + param.RequestNumber + "%' AND ";

            if (!string.IsNullOrEmpty(param.DepartName))
                whereClause += "DepartName like '%" + param.DepartName + "%' AND ";

            if (!string.IsNullOrEmpty(param.DepartName))
                whereClause += "DepartName like '%" + param.DepartName + "%' AND ";

            if (!string.IsNullOrEmpty(param.CraetedDate.ToString()) && !string.IsNullOrEmpty(param.CraetedDate2.ToString()))
                whereClause += "CAST(CraetedDate as date) between CAST('" + String.Format("{0:yyyy/MM/dd}", param.CraetedDate) + "' as date) AND CAST('" + String.Format("{0:yyyy/MM/dd}", param.CraetedDate2) + "' as date) AND ";

            if (!string.IsNullOrEmpty(param.CraetedDate.ToString()) && string.IsNullOrEmpty(param.CraetedDate2.ToString()))
                whereClause += "CAST(CraetedDate as date) =CAST('" + String.Format("{0:yyyy/MM/dd}", param.CraetedDate) + "' as date) AND ";

            if (string.IsNullOrEmpty(param.CraetedDate.ToString()) && !string.IsNullOrEmpty(param.CraetedDate2.ToString()))
                whereClause += "CAST(CraetedDate as date) =CAST('" + String.Format("{0:yyyy/MM/dd}", param.CraetedDate2) + "' as date) AND ";


            whereClause = !string.IsNullOrWhiteSpace(whereClause) ? whereClause.Substring(0, whereClause.Length - 5) : "";
            return whereClause;
        }
        private List<vwReactiveStopAccrueDashboardDetail> pGetDashboardDetailList(vwReactiveStopAccrueDashboardDetail param, int rowSkip, int pageSize)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            List<vwReactiveStopAccrueDashboardDetail> data = new List<vwReactiveStopAccrueDashboardDetail>();
            try
            {
                var repo = new vwReactiveStopAccrueDashboardDetailRepository(context);
                data = repo.GetList("TrxStopAccrueHeaderID =" + param.TrxStopAccrueHeaderID, "RequestNumber DESC");

                var hrData = new Employee();
                var hrDataResult = hrData.GetDetail(data.Select(x => x.Initiator).FirstOrDefault());
                data = data.Select(c => { c.Initiator = hrData.FullName; return c; }).ToList();
            } 
            catch (Exception ex)
            {
                context.Dispose();
                data.Add(new vwReactiveStopAccrueDashboardDetail { ErrorMessage = ex.Message });
            }

            return data;
        }
        private int pGetCountDashboardDetail(vwReactiveStopAccrueDashboardDetail param)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));

            try
            {
                var repo = new vwReactiveStopAccrueDashboardDetailRepository(context);
                return repo.GetCount("TrxStopAccrueHeaderID =" + param.TrxStopAccrueHeaderID);
            }
            catch (Exception ex)
            {
                context.Dispose();
                return 0;
            }
        }
        #endregion

        private string MonthRomawi(int month)
        {
            string monthRowami = "";
            switch (month)
            {
                case 1:
                    monthRowami = "I"; break;
                case 2:
                    monthRowami = "II"; break;
                case 3:
                    monthRowami = "III"; break;
                case 4:
                    monthRowami = "IV"; break;
                case 5:
                    monthRowami = "V"; break;
                case 6:
                    monthRowami = "VI"; break;
                case 7:
                    monthRowami = "VII"; break;
                case 8:
                    monthRowami = "VIII"; break;
                case 9:
                    monthRowami = "IX"; break;
                case 10:
                    monthRowami = "X"; break;
                case 11:
                    monthRowami = "XI"; break;
                case 12:
                    monthRowami = "XII"; break;

            }
            return monthRowami;
        }

    }
}
