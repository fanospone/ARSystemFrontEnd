using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ARSystem.Domain.Models;
using ARSystem.Domain.Repositories;
using ARSystem.Domain.DAL;

namespace ARSystem.Service
{
    public class TrxRAAmountTargetService
    {
        public List<TrxRAAmountTarget> GetPagedRequest(TrxRAAmountTarget param, int rowSkip, int pageSize)
        {
            return pGetPagedRequest(WhereClause(param), rowSkip, pageSize);
        }
        public int GetCountRequest(TrxRAAmountTarget param)
        {
            return pGetCountRequest(WhereClause(param));
        }
        public TrxRAAmountTarget CreateRequest(TrxRAAmountTarget post, List<TrxRAAmountTargetDetail> postDetail)
        {
            return pCreateRequest(post, postDetail);
        }
        public TrxRAAmountTarget UpdateRequest(TrxRAAmountTarget post, List<TrxRAAmountTargetDetail> postDetail)
        {
            return pUpdateRequest(post, postDetail);
        }

        public List<TrxRAAmountTargetDetail> GetPagedRequestDetail(int ID)
        {
            return pGetPagedRequestDetail(ID);
        }

        public int GetCountRequestDetail(int ID)
        {
            return pGetCountRequestDetail(ID);
        }
        private string WhereClause(TrxRAAmountTarget param)
        {
            string whereClause = "";

            whereClause += "StatusCode <>'APPR' AND ";

            if (!string.IsNullOrEmpty(param.CustomerID))
                whereClause += "CustomerID ='" + param.CustomerID + "' AND ";
            if (param.Year != 0)
                whereClause += "Year =" + param.Year + " AND ";


            whereClause = !string.IsNullOrWhiteSpace(whereClause) ? whereClause.Substring(0, whereClause.Length - 5) : "";
            return whereClause;
        }
        private List<TrxRAAmountTarget> pGetPagedRequest(string whereClause, int rowSkip, int pageSize)
        {
            var context = new DbContext(Helper.GetConnection("ARSystemDWH"));
            var repo = new TrxRAAmountTargetRepository(context);
            List<TrxRAAmountTarget> list = new List<TrxRAAmountTarget>();
            try
            {
                if (pageSize > 0)
                    list = repo.GetPaged(whereClause, "", rowSkip, pageSize);
                else
                    list = repo.GetList(whereClause, "");
                return list;
            }
            catch (Exception ex)
            {
                list.Add(new TrxRAAmountTarget((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "TrxRAAmountTargetService", "GetPaged", "")));
                return list;
            }
            finally
            {
                context.Dispose();
            }
        }
        private int pGetCountRequest(string whereClause)
        {
            var context = new DbContext(Helper.GetConnection("ARSystemDWH"));
            var repo = new TrxRAAmountTargetRepository(context);

            try
            {
                return repo.GetCount(whereClause);
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
        private TrxRAAmountTarget pCreateRequest(TrxRAAmountTarget post, List<TrxRAAmountTargetDetail> postDetail)
        {
            var context = new DbContext(Helper.GetConnection("ARSystemDWH"));
            var repo = new TrxRAAmountTargetRepository(context);
            var repoDetail = new TrxRAAmountTargetDetailRepository(context);
            var repoLog = new TrxRAAmountTargetLogRepository(context);
            TrxRAAmountTarget result = new TrxRAAmountTarget();

            try
            {

                var repoAmountTarget = new MstRAAmountTargetRepository(context);
                var repoAmountTargetMaster = new MstRAAmountTargetRepository(context);
                var mstRAAmountTarget = new MstRAAmountTarget();

                var dataAmountTarget = new List<MstRAAmountTarget>();
                for (int i = 0; i < postDetail.Count; i++)
                {
                    dataAmountTarget.Add(new MstRAAmountTarget { Month = postDetail[i].Month, AmountTarget = postDetail[i].AmountTarget, CreatedBy = post.CreatedBy, UpdatedBy = post.CreatedBy });
                }

                var repoAppr = new mstApprovalStatusRepository(context);
                var apprStatus = new mstApprovalStatus();
               
                /*check status approval*/
                
                  


                var amountTarget2 = new MstRAAmountTarget();
                string whereClause2 = "CustomerID ='" + post.CustomerID + "' AND Year=" + post.Year + "";
                amountTarget2 = repoAmountTarget.GetList(whereClause2).FirstOrDefault();

                if (amountTarget2 == null)
                {
                    for (int i = 0; i < dataAmountTarget.Count; i++)
                    {

                        var amountTarget = new MstRAAmountTarget();
                        string whereClause = "CustomerID ='" + post.CustomerID + "' AND Year=" + post.Year + " AND Month=" + dataAmountTarget[i].Month + "";
                        amountTarget = repoAmountTarget.GetList(whereClause).FirstOrDefault();

                        /*insert data amount target*/
                        if (amountTarget == null)
                        {
                            var postData = new MstRAAmountTarget();
                            postData.ID = amountTarget == null ? 0 : amountTarget.ID;
                            postData.CustomerID = post.CustomerID;
                            postData.Year = post.Year;
                            postData.AmountTarget = dataAmountTarget[i].AmountTarget;
                            postData.Month = dataAmountTarget[i].Month;
                            postData.CreatedBy = dataAmountTarget[i].CreatedBy;
                            postData.UpdatedBy = dataAmountTarget[i].UpdatedBy;

                            repoAmountTarget.Create(postData);


                        }
                    }
                    apprStatus = repoAppr.GetList(" StatusCode = 'APPR' ").FirstOrDefault();
                    post.ApprovalStatusID = apprStatus.ID;
                }
                else
                {
                    apprStatus = repoAppr.GetList(" StatusCode = 'WAIT_APPR' ").FirstOrDefault();
                    post.ApprovalStatusID = apprStatus.ID;
                }

                result = repo.Create(post);
                for (int j = 0; j < postDetail.Count; j++)
                {
                    postDetail[j].TrxRAAmountTargetID = result.ID;
                    postDetail[j].CreatedBy = post.CreatedBy;
                    repoDetail.Create(postDetail[j]);
                    var log = new TrxRAAmountTargetLog();
                    log.TrxRAAmountTargetID = result.ID;
                    log.Month = postDetail[j].Month;
                    log.AmountTarget = postDetail[j].AmountTarget;
                    log.CreatedBy = post.CreatedBy;
                    repoLog.Create(log);
                }



                return result;
            }
            catch (Exception ex)
            {
                return new TrxRAAmountTarget((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "RTIDoneNOverdueService", "Save", post.CreatedBy));
            }
            finally
            {
                context.Dispose();
            }
        }
        private TrxRAAmountTarget pUpdateRequest(TrxRAAmountTarget post, List<TrxRAAmountTargetDetail> postDetail)
        {
            var context = new DbContext(Helper.GetConnection("ARSystemDWH"));
            var repo = new TrxRAAmountTargetRepository(context);
            TrxRAAmountTarget result = new TrxRAAmountTarget();

            try
            {
                /*update status approval*/
                result = repo.Update(post);

                var repoAppr = new mstApprovalStatusRepository(context);
                var apprStatus = new mstApprovalStatus();
                apprStatus.StatusCode = "";
                /*check status approval*/
                if (post.ApprovalStatusID != 0)
                    apprStatus = repoAppr.GetByPK(post.ApprovalStatusID);

                /* jika amount target sudah di approve */
                if (apprStatus.StatusCode.ToUpper() == "APPR")
                {
                    /*get data amount target yang existing berdasarkan customer id dan year */


                    /*get data amount target yang sudah di approve*/
                    var repoViewTrx = new vwTrxAmountTargetRepository(context);
                    var vwTrx = new List<vwTrxAmountTarget>();
                    vwTrx = repoViewTrx.GetList("TargetAmountID =" + post.ID + "");

                    /*set amount target list*/
                    var dataAmountTarget = new List<MstRAAmountTarget>();
                    for (int i = 0; i < vwTrx.Count; i++)
                    {
                        dataAmountTarget.Add(new MstRAAmountTarget { Month = vwTrx[i].Month, AmountTarget = vwTrx[i].AmountTarget, CreatedBy = post.CreatedBy, UpdatedBy = post.CreatedBy });
                    }

                    var repoAmountTarget = new MstRAAmountTargetRepository(context);

                    for (int i = 0; i < dataAmountTarget.Count; i++)
                    {

                        var amountTarget = new MstRAAmountTarget();
                        string whereClause = "CustomerID ='" + post.CustomerID + "' AND Year=" + post.Year + " AND Month=" + dataAmountTarget[i].Month + "";
                        amountTarget = repoAmountTarget.GetList(whereClause).FirstOrDefault();

                        var postData = new MstRAAmountTarget();
                        postData.ID = amountTarget == null ? 0 : amountTarget.ID;
                        postData.CustomerID = post.CustomerID;
                        postData.Year = post.Year;
                        postData.AmountTarget = dataAmountTarget[i].AmountTarget;
                        postData.Month = dataAmountTarget[i].Month;
                        postData.CreatedBy = dataAmountTarget[i].CreatedBy;
                        postData.UpdatedBy = dataAmountTarget[i].UpdatedBy;

                        /*update data amount target existing*/
                        if (amountTarget != null)
                        {
                            repoAmountTarget.Update(postData);
                        }
                        else
                        {
                            repoAmountTarget.Create(postData);
                        }
                    }

                }
                else if (apprStatus.StatusCode.ToUpper() != "REJECT")
                {
                    var repoDetail = new TrxRAAmountTargetDetailRepository(context);
                    var repoLog = new TrxRAAmountTargetLogRepository(context);
                    for (int i = 0; i < postDetail.Count; i++)
                    {

                        postDetail[i].TrxRAAmountTargetID = post.ID;
                        postDetail[i].UpdatedBy = post.CreatedBy;
                        repoDetail.Update(postDetail[i]);

                        var log = new TrxRAAmountTargetLog();
                        log.TrxRAAmountTargetID = post.ID;
                        log.Month = postDetail[i].Month;
                        log.AmountTarget = postDetail[i].AmountTarget;
                        log.CreatedBy = post.CreatedBy;
                        repoLog.Create(log);
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                return new TrxRAAmountTarget((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "RTIDoneNOverdueService", "Update", post.CreatedBy));
            }
            finally
            {
                context.Dispose();
            }
        }

        private List<TrxRAAmountTargetDetail> pGetPagedRequestDetail(int ID)
        {
            var context = new DbContext(Helper.GetConnection("ARSystemDWH"));
            var repo = new TrxRAAmountTargetDetailRepository(context);
            List<TrxRAAmountTargetDetail> list = new List<TrxRAAmountTargetDetail>();
            try
            {
                string whereClause = "";
                whereClause += "TrxRAAmountTargetID =" + ID + "";
                list = repo.GetList(whereClause, "");
                return list;
            }
            catch (Exception ex)
            {
                list.Add(new TrxRAAmountTargetDetail((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "TrxRAAmountTargetService", "GetPaged", "")));
                return list;
            }
            finally
            {
                context.Dispose();
            }
        }

        private int pGetCountRequestDetail(int ID)
        {
            var context = new DbContext(Helper.GetConnection("ARSystemDWH"));
            var repo = new TrxRAAmountTargetDetailRepository(context);
            try
            {
                string whereClause = "";
                whereClause += "TrxRAAmountTargetID =" + ID + "";
                return repo.GetCount(whereClause);
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

    }
}
