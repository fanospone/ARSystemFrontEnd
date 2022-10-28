using ARSystem.Domain.DAL;
using ARSystem.Domain.Models;
using ARSystem.Domain.Models.Models.RevenueAssurance;
using ARSystem.Domain.Models.TBIGSYSDB01.TBGARSystem;
using ARSystem.Domain.Repositories;
using ARSystem.Domain.Repositories.Repositories.RevenueAssurance;
using ARSystem.Domain.Repositories.TBIGSYSDB01.TBGARSystem;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Service.RevenueAssurance
{
    public class BAPSBulkyService
    {
        public mstBAPSRecurring SaveBAPSBulky(vmSaveBAPSBulky ModelSaveBAPSBulky, string userID)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var uow = context.CreateUnitOfWork();

            trxReconcile result = new trxReconcile();

            var mstBAPSRecurringRepo = new mstBAPSRecurringRepository(context);
            var mstBAPSRecurring = new mstBAPSRecurring();

            var trxRAUploadDocumentRepo = new trxRAUploadDocumentRepository(context);
            var trxRAUploadDocument = new trxRAUploadDocument();

            var trxReconcileRepo = new trxReconcileRepository(context);
            var trxReconcile = new List<trxReconcile>();

            var trxRAPurchaseOrderRepo = new trxRAPurchaseOrderRepository(context);
            var trxRAPurchaseOrder = new trxRAPurchaseOrder();

            try
            {

                #region 'Insert mstBAPSRecurring'

                mstBAPSRecurring = new mstBAPSRecurring();
                mstBAPSRecurring.BAPSNumber = ModelSaveBAPSBulky.BAPSNumber;
                mstBAPSRecurring.CompanyID = ModelSaveBAPSBulky.CompanyID;
                mstBAPSRecurring.CustomerID = ModelSaveBAPSBulky.CustomerID;
                mstBAPSRecurring.TotalTenant = ModelSaveBAPSBulky.TotalTenant;
                mstBAPSRecurring.TotalAmount = ModelSaveBAPSBulky.TotalAmount;
                mstBAPSRecurring.BAPSSignDate = ModelSaveBAPSBulky.BAPSSignDate;
                mstBAPSRecurring.Remarks = ModelSaveBAPSBulky.Remarks;
                mstBAPSRecurring.mstRAActivityID = (int)Constants.RAActivity.BAPS_DONE;
                mstBAPSRecurring.CreatedDate = Helper.GetDateTimeNow();
                mstBAPSRecurring.CreatedBy = userID;
                mstBAPSRecurring = mstBAPSRecurringRepo.Create(mstBAPSRecurring);

                #endregion

                #region 'Update trxReconcile'

                int mstBAPSRecurringID = mstBAPSRecurring.ID;
                trxReconcile = trxReconcileRepo.UpdateBAPSBulky(ModelSaveBAPSBulky.ListTrxBAPS, mstBAPSRecurringID, mstBAPSRecurring.mstRAActivityID);

                #endregion

                #region 'Update trxRAPurchaseOrder'

                if (ModelSaveBAPSBulky.CustomerID.Trim() != Constants.CustomerID["EXCELCOMINDO"] && ModelSaveBAPSBulky.CustomerID.Trim() != Constants.CustomerID["MITEL"] && ModelSaveBAPSBulky.CustomerID.Trim() != Constants.CustomerID["PAB"] && ModelSaveBAPSBulky.CustomerID.Trim() != Constants.CustomerID["TELKOM"])
                {
                    List<string> ID = ModelSaveBAPSBulky.ListTrxBAPS.Select(l => l.trxRAPurchaseOrderID.ToString()).Distinct().ToList();
                    foreach (string trxRAPurchaseOrderID in ID)
                    {
                        trxRAPurchaseOrder = trxRAPurchaseOrderRepo.GetByPK(Convert.ToInt64(trxRAPurchaseOrderID));
                        trxRAPurchaseOrder.mstRAActivityID = (int)Constants.RAActivity.BAPS_DONE;
                        trxRAPurchaseOrder = trxRAPurchaseOrderRepo.Update(trxRAPurchaseOrder);
                    }
                }
                #endregion

                uow.SaveChanges();

                return mstBAPSRecurring;

            }
            catch (Exception ex)
            {
                uow.Dispose();
                return new mstBAPSRecurring((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "BAPSBulkyService", "SaveBAPSBulky", userID));
            }
            finally
            {
                context.Dispose();
            }
        }

        public List<mstOperator> GetOperatorList(string token, string orderBy, string userID)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var OperatorRepo = new mstOperatorRepository(context);
            List<mstOperator> listOperator = new List<mstOperator>();

            try
            {
                string strWhereClause = "";

                strWhereClause += "OperatorID IN (" + string.Join(",", Constants._Operator) + ") AND ";
                strWhereClause = !string.IsNullOrWhiteSpace(strWhereClause) ? strWhereClause.Substring(0, strWhereClause.Length - 5) : "";
                listOperator = OperatorRepo.GetList(strWhereClause, orderBy);


                return listOperator;
            }
            catch (Exception ex)
            {
                listOperator.Add(new mstOperator((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "BAPSBulkyService", "GetMstOperatorToList", userID)));
                return listOperator;
            }
            finally
            {
                context.Dispose();
            }
        }

        public mstBAPSRecurring BacktoBAPSInput(string token, vmSaveBAPSBulky ModelSaveBAPSBulky, string userID)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var uow = context.CreateUnitOfWork();

            var repo = new RevenueAssuranceRepository(context);

            var mstBapsRecurringRepo = new mstBAPSRecurringRepository(context);
            var trxReconcileRepo = new trxReconcileRepository(context);
            var trxRALogActivityRepo = new trxRALogActivityRepository(context);
            var trxRAPurchaseOrderRepo = new trxRAPurchaseOrderRepository(context);

            var BAPS = new mstBAPSRecurring();
            var trxReconcile = new List<trxReconcile>();
            var trxRAPurchaseOrder = new trxRAPurchaseOrder();

            try
            {
                string IDs;
                string where;

                IDs = string.Join(",", ModelSaveBAPSBulky.detailIDs);

                List<mstBAPSRecurring> ListmstBaps = new List<mstBAPSRecurring>();
                where = "ID IN (" + IDs + ")";
                ListmstBaps = mstBapsRecurringRepo.GetList(where);

                foreach (mstBAPSRecurring mst in ListmstBaps)
                {

                    //INSERT INTO LOGRAACTIVITY BEFORE UPDATE
                    trxRALogActivity logHeaderRA = new trxRALogActivity();
                    logHeaderRA.UserID = userID;
                    logHeaderRA.mstRAActivityID = BAPS.mstRAActivityID;
                    logHeaderRA.TransactionID = ModelSaveBAPSBulky.ID;
                    logHeaderRA.Remarks = BAPS.Remarks;
                    logHeaderRA.LogDate = Helper.GetDateTimeNow();
                    logHeaderRA.LogState = true;
                    logHeaderRA.Label = "BacktoBAPSInput";
                    trxRALogActivityRepo.Create(logHeaderRA);

                    // Update mstBapsRecurring
                    mstBAPSRecurring mstBaps = mstBapsRecurringRepo.GetByPK(mst.ID);
                    //mstBaps.mstRAActivityID = 0;
                    mstBaps.mstRAActivityID = (int)Constants.RAActivity.HOLD;
                    mstBaps = mstBapsRecurringRepo.Update(mstBaps);

                    int mstRAActivityID;

                    if (mstBaps.CustomerID.Trim() == "TSEL")
                    {
                        mstRAActivityID = (int)Constants.RAActivity.PO_DONE;
                    }
                    else
                    {
                        mstRAActivityID = (int)Constants.RAActivity.RECONCILE_DONE;
                    }

                    where = "mstBAPSRecurringID = '" + mst.ID + "'";
                    trxReconcile = trxReconcileRepo.GetList(where);
                    List<int> ID = trxReconcile.Select(l => Convert.ToInt32(l.trxRAPurchaseOrderID)).Distinct().ToList();


                    if (ID.Count != 0)
                    {
                        foreach (int trxRAPurchaseOrderID in ID)
                        {
                            // Update trxReconcile
                            string msg = RejectBAPSBulky(token, mstRAActivityID, mst.ID, trxRAPurchaseOrderID);
                            if (msg != "")
                            {
                                uow.Dispose();
                                return new mstBAPSRecurring((int)Helper.ErrorType.Error, Helper.logError(msg.ToString(), "BAPSBulkyService", "RejectBAPSBulky", userID));
                            }

                            where = "trxRAPurchaseOrderID = '" + trxRAPurchaseOrderID + "' AND mstBAPSRecurringID IS NOT NULL";
                            trxReconcile = trxReconcileRepo.GetList(where);

                            #region 'Update trxRAPurchaseOrder'

                            // Update trxRAPurchaseOrder
                            if (trxReconcile.Count == 0)
                            {
                                if (mst.CustomerID.Trim() != "XL" && mst.CustomerID.Trim() != "MITEL" && mst.CustomerID.Trim() != "PAB" && mst.CustomerID.Trim() != "TELKOM")
                                {
                                    trxRAPurchaseOrder = trxRAPurchaseOrderRepo.GetByPK(Convert.ToInt64(trxRAPurchaseOrderID));
                                    trxRAPurchaseOrder.mstRAActivityID = (int)Constants.RAActivity.PO_DONE;
                                    trxRAPurchaseOrder = trxRAPurchaseOrderRepo.Update(trxRAPurchaseOrder);
                                }
                            }


                            #endregion


                            //// Update trxRAPurchaseOrder
                            //if (trxReconcile.Count == 0)
                            //{
                            //    trxRAPurchaseOrder = trxRAPurchaseOrderRepo.GetByPK(Convert.ToInt64(trxRAPurchaseOrderID));
                            //    trxRAPurchaseOrder.mstRAActivityID = (int)Constants.RAActivity.PO_DONE;
                            //    trxRAPurchaseOrder = trxRAPurchaseOrderRepo.Update(trxRAPurchaseOrder);
                            //}
                        }
                    }
                    else
                    {
                        // Update trxReconcile
                        string msg = RejectBAPSBulky(token, mstRAActivityID, mst.ID, 0);
                        if (msg != "")
                        {
                            uow.Dispose();
                            return new mstBAPSRecurring((int)Helper.ErrorType.Error, Helper.logError(msg.ToString(), "BAPSBulkyService", "RejectBAPSBulky", userID));
                        }

                    }

                }

                uow.SaveChanges();
                return BAPS;
            }
            catch (Exception ex)
            {
                uow.Dispose();
                return new mstBAPSRecurring((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "BAPSBulkyService", "BacktoBAPSInput", userID));
            }
            finally
            {
                context.Dispose();
            }

        }

        public string RejectBAPSBulky(string strToken, int mstRAActivityID, int Id, int Po)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));

            try
            {
                var command = context.CreateCommand();
                command.CommandType = CommandType.Text;

                if (Po != 0)
                {
                    command.CommandText = "UPDATE TBGARSystem.dbo.trxReconcile SET mstRAActivityID = " + mstRAActivityID + " , mstBAPSRecurringID = null WHERE mstBAPSRecurringID = " + Id + " AND trxRAPurchaseOrderID = " + Po + "";
                }
                else
                {
                    command.CommandText = "UPDATE TBGARSystem.dbo.trxReconcile SET mstRAActivityID = " + mstRAActivityID + " , mstBAPSRecurringID = null WHERE mstBAPSRecurringID = " + Id + "";
                }
                command.ExecuteNonQuery();

                return "";
            }
            catch (Exception ex)
            {
                Helper.logError(ex.Message.ToString(), "BAPSBulkyService", "RejectBAPSBulky", "");
                return ex.Message;
            }
            finally
            {
                context.Dispose();
            }
        }
    }
}
