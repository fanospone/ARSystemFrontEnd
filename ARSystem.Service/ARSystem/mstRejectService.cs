using System;
using System.Collections.Generic;
using System.Linq;
using ARSystem.Domain.DAL;
using ARSystem.Domain.Models;
using ARSystem.Domain.Repositories;

namespace ARSystem.Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "MstRejectService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select MstRejectService.svc or MstRejectService.svc.cs at the Solution Explorer and start debugging.
    public partial class mstRejectService 
    {
        public mstPICADetail CreateRejectDtl(string UserID, mstPICADetail RejectDtl)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var uow = context.CreateUnitOfWork();

            var DtlRepo = new mstPICADetailRepository(context);
            
            try
            {
                if (DtlRepo.GetList("Description = '" + RejectDtl.Description + "' AND mstPICATypeID =" + RejectDtl.mstPICATypeID).Count() > 0)
                {
                    return new mstPICADetail((int)Helper.ErrorType.Validation, "Detail Description Name has been used. Please choose another Detail Description Name!");
                }
                RejectDtl.CreatedDate = Helper.GetDateTimeNow();
                RejectDtl.CreatedBy = UserID;
                DtlRepo.Create(RejectDtl);

                uow.SaveChanges();

                return RejectDtl;
            }
            catch (Exception ex)
            {
                uow.Dispose();
                return new mstPICADetail((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "mstRejectService", "CreatemstPICADetail", UserID));
            }
            finally
            {
                context.Dispose();
            }
        }

        public mstPICAType CreateRejectHdr(string UserID, mstPICAType RejectHdr)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var uow = context.CreateUnitOfWork();
            int mstUserGroupId = 0;
            var HdrRepo = new mstPICATypeRepository(context);
           
            try
            {
                if (UserHelper.GetUserARPosition(UserID) == "DEPT HEAD" || UserHelper.GetUserARPosition(UserID) == "AR DATA")
                    mstUserGroupId = (int)Constants.UserGroup.ARData;
                else if (UserHelper.GetUserARPosition(UserID) == "AR MONITORING" || UserHelper.GetUserARPosition(UserID) == "AR COLLECTION")
                    mstUserGroupId = (int)Constants.UserGroup.ARCollection;
                else
                    mstUserGroupId = RejectHdr.mstUserGroupId.Value;
                if (HdrRepo.GetList("Description = '" + RejectHdr.Description + "' AND mstUserGroupId = "+mstUserGroupId).Count() > 0)
                {
                    return new mstPICAType((int)Helper.ErrorType.Validation, "Header Description Name has been used. Please choose another Header Description Name!");
                }

                RejectHdr.CreatedDate = Helper.GetDateTimeNow();
                RejectHdr.CreatedBy = UserID;
                if (UserHelper.GetUserARPosition(UserID) == "DEPT HEAD" || UserHelper.GetUserARPosition(UserID) == "AR DATA")
                    RejectHdr.mstUserGroupId = (int)Constants.UserGroup.ARData;
                else if (UserHelper.GetUserARPosition(UserID) == "AR MONITORING" || UserHelper.GetUserARPosition(UserID) == "AR COLLECTION")
                    RejectHdr.mstUserGroupId = (int)Constants.UserGroup.ARCollection;

                HdrRepo.Create(RejectHdr);

                uow.SaveChanges();

                return RejectHdr;
            }
            catch (Exception ex)
            {
                uow.Dispose();
                return new mstPICAType((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "mstRejectService", "CreatemstPICAType", UserID));
            }
            finally
            {
                context.Dispose();
            }
        }

        public mstPICADetail DeleteRejectDtl(string UserID, int intDtlId)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var uow = context.CreateUnitOfWork();

            var DtlRepo = new mstPICADetailRepository(context);
            
            try
            {
                DtlRepo.DeleteByPK(intDtlId);

                uow.SaveChanges();

                return new mstPICADetail();
            }
            catch (Exception ex)
            {
                uow.Dispose();
                return new mstPICADetail((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "mstRejectService", "DeletemstPICADetail", UserID));
            }
            finally
            {
                context.Dispose();
            }
        }

        public mstPICAType DeleteRejectHdr(string UserID, int intHdrId)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var uow = context.CreateUnitOfWork();

            var HdrRepo = new mstPICATypeRepository(context);
            var DtlRepo = new mstPICADetailRepository(context);
            
            try
            {
                DtlRepo.DeleteByFilter("mstPICATypeID=" + intHdrId);
                HdrRepo.DeleteByPK(intHdrId);

                uow.SaveChanges();

                return new mstPICAType();
            }
            catch (Exception ex)
            {
                uow.Dispose();
                return new mstPICAType((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "mstRejectService", "DeletemstPICAType", UserID));
            }
            finally
            {
                context.Dispose();
            }
        }


        public List<vmMstRejectHdrDtl> GetMstHdrDtlToList(string UserID, string strHdr, int intIsActive,int mstUserGroupId, string strOrderBy, int intRowSkip = 0, int intPageSize = 0)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var HdrRepo = new mstPICATypeRepository(context);
            var DtlRepo = new mstPICADetailRepository(context);
            List<vmMstRejectHdrDtl> listHdrDtl = new List<vmMstRejectHdrDtl>();
            
            try
            {
                string strWhereClause = "";
                // untuk yang NON AR
                if (UserHelper.GetUserARPosition(UserID) == "DEPT HEAD" || UserHelper.GetUserARPosition(UserID) == "AR DATA")
                    strWhereClause = "mstUserGroupId =" + (int)Constants.UserGroup.ARData + " AND ";
                else if (UserHelper.GetUserARPosition(UserID) == "AR MONITORING" || UserHelper.GetUserARPosition(UserID) == "AR COLLECTION")
                    strWhereClause = "mstUserGroupId =" + (int)Constants.UserGroup.ARCollection + " AND ";
                if (!string.IsNullOrWhiteSpace(strHdr))
                {
                    strWhereClause += "Description LIKE '%" + strHdr + "%' AND ";
                }
                if (intIsActive > -1)
                {
                    strWhereClause += "IsActive = " + intIsActive + " AND ";
                }
                if (mstUserGroupId > -1)
                {
                    strWhereClause += "mstUserGroupId = " + mstUserGroupId + " AND ";
                }
                strWhereClause = !string.IsNullOrWhiteSpace(strWhereClause) ? strWhereClause.Substring(0, strWhereClause.Length - 5) : "";
                List<mstPICAType> RejectHdr = new List<mstPICAType>();
                if (intPageSize > 0)
                    RejectHdr = HdrRepo.GetPaged(strWhereClause, strOrderBy, intRowSkip, intPageSize);
                else
                    RejectHdr = HdrRepo.GetList(strWhereClause, strOrderBy);

                foreach (mstPICAType hdr in RejectHdr)
                {
                    vmMstRejectHdrDtl vmMstRejectHdrDtlData = new vmMstRejectHdrDtl();
                    vmMstRejectHdrDtlData.mstPICATypeID = hdr.mstPICATypeID;
                    vmMstRejectHdrDtlData.Description = hdr.Description;
                    vmMstRejectHdrDtlData.Recipient = hdr.Recipient;
                    vmMstRejectHdrDtlData.CC = hdr.CC;
                    vmMstRejectHdrDtlData.isActive = hdr.IsActive.Value;
                    vmMstRejectHdrDtlData.mstUserGroupId = hdr.mstUserGroupId.Value;
                    vmMstRejectHdrDtlData.DetailList = DtlRepo.GetList("mstPICATypeID = " + vmMstRejectHdrDtlData.mstPICATypeID);
                    listHdrDtl.Add(vmMstRejectHdrDtlData);
                }

                return listHdrDtl;
            }
            catch (Exception ex)
            {
                listHdrDtl.Add(new vmMstRejectHdrDtl((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "mstRejectService", "GetMstRejectHdrDtlToList", UserID)));
                return listHdrDtl;
            }
            finally
            {
                context.Dispose();
            }
        }

        public mstPICADetail UpdateRejectDtl(string UserID, int intDtlId, mstPICADetail RejectDtl)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var uow = context.CreateUnitOfWork();

            var DtlRepo = new mstPICADetailRepository(context);
            
            try
            {
                if (DtlRepo.GetList("Description = '" + RejectDtl.Description + "' AND mstPICADetailID <> " + intDtlId + " AND mstPICATypeID  =" + RejectDtl.mstPICATypeID).Count() > 0)
                {
                    return new mstPICADetail((int)Helper.ErrorType.Validation, "Detail Description Name has been used. Please choose another Detail Description Name!");
                }

                mstPICADetail DtlUpdate = DtlRepo.GetByPK(intDtlId);
                DtlUpdate.Description = RejectDtl.Description;
                DtlUpdate.IsActive = RejectDtl.IsActive;
                DtlUpdate.UpdatedDate = Helper.GetDateTimeNow();
                DtlUpdate.UpdatedBy = UserID;
                DtlRepo.Update(DtlUpdate);

                uow.SaveChanges();

                return DtlUpdate;
            }
            catch (Exception ex)
            {
                uow.Dispose();
                return new mstPICADetail((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "mstRejectService", "UpdatemstPICADetail", UserID));
            }
            finally
            {
                context.Dispose();
            }
        }

        public mstPICAType UpdateRejectHdr(string UserID, int intHdrId, mstPICAType RejectHdr)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var uow = context.CreateUnitOfWork();

            var HdrRepo = new mstPICATypeRepository(context);
            
            try
            {
                if (HdrRepo.GetList("Description = '" + RejectHdr.Description + "' AND mstPICATypeID <> " + intHdrId + " AND mstUserGroupId  =" + RejectHdr.mstUserGroupId).Count() > 0)
                {
                    return new mstPICAType((int)Helper.ErrorType.Validation, "Header Description Name has been used. Please choose another Header Description Name!");
                }

                mstPICAType HdrUpdate = HdrRepo.GetByPK(intHdrId);
                HdrUpdate.Description = RejectHdr.Description;
                HdrUpdate.Recipient = RejectHdr.Recipient;
                HdrUpdate.CC = RejectHdr.CC;
                HdrUpdate.IsActive = RejectHdr.IsActive;
                HdrUpdate.mstUserGroupId = RejectHdr.mstUserGroupId;
                HdrUpdate.UpdatedDate = Helper.GetDateTimeNow();
                HdrUpdate.UpdatedBy = UserID;
                HdrRepo.Update(HdrUpdate);

                uow.SaveChanges();

                return HdrUpdate;
            }
            catch (Exception ex)
            {
                uow.Dispose();
                return new mstPICAType((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "mstRejectService", "UpdatemstPICAType", UserID));
            }
            finally
            {
                context.Dispose();
            }
        }

        public int GetRejectHdrCount(string UserID, string strHdr, int intIsActive,int mstUserGroupId)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var HdrRepo = new mstPICATypeRepository(context);
            var DtlRepo = new mstPICADetailRepository(context);
            List<vmMstRejectHdrDtl> listHdrDtl = new List<vmMstRejectHdrDtl>();
            
            try
            {
                string strWhereClause = "";
                if (UserHelper.GetUserARPosition(UserID) == "DEPT HEAD" || UserHelper.GetUserARPosition(UserID) == "AR DATA")
                    strWhereClause = "mstUserGroupId =" + (int)Constants.UserGroup.ARData + " AND ";
                else if (UserHelper.GetUserARPosition(UserID) == "AR MONITORING" || UserHelper.GetUserARPosition(UserID) == "AR COLLECTION")
                    strWhereClause = "mstUserGroupId =" + (int)Constants.UserGroup.ARCollection + " AND ";
                if (!string.IsNullOrWhiteSpace(strHdr))
                {
                    strWhereClause += "Description LIKE '%" + strHdr + "%' AND ";
                }
                if (intIsActive > -1)
                {
                    strWhereClause += "IsActive = " + intIsActive + " AND ";
                }
                if (mstUserGroupId > -1)
                {
                    strWhereClause += "mstUserGroupId = " + mstUserGroupId + " AND ";
                }
                strWhereClause = !string.IsNullOrWhiteSpace(strWhereClause) ? strWhereClause.Substring(0, strWhereClause.Length - 5) : "";

                return HdrRepo.GetCount(strWhereClause);
            }
            catch (Exception ex)
            {
                Helper.logError(ex.Message.ToString(), "mstRejectService", "GetmstPICATypeCount", UserID);
                return 0;
            }
            finally
            {
                context.Dispose();
            }
        }

        public int GetRejectDtlCount(string UserID, string strHdrId)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var DtlRepo = new mstPICADetailRepository(context);
            List<mstPICADetail> listDtl = new List<mstPICADetail>();
            
            try
            {
                string strWhereClause = "";
                strWhereClause += "mstPICATypeID LIKE '%" + strHdrId + "%' AND ";

                strWhereClause = !string.IsNullOrWhiteSpace(strWhereClause) ? strWhereClause.Substring(0, strWhereClause.Length - 5) : "";

                return DtlRepo.GetCount(strWhereClause);
            }
            catch (Exception ex)
            {
                Helper.logError(ex.Message.ToString(), "mstRejectService", "GetmstPICADetailCount", UserID);
                return 0;
            }
            finally
            {
                context.Dispose();
            }
        }

        public List<mstPICADetail> GetMstRejectDtlToList(string UserID, string strHdrId, string strOrderBy, int intRowSkip = 0, int intPageSize = 0)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var MstRejectDtlRepo = new mstPICADetailRepository(context);
            List<mstPICADetail> listMstRejectDtl = new List<mstPICADetail>();
            
            try
            {
                string strWhereClause = "";
                strWhereClause += "mstPICATypeID LIKE '%" + strHdrId + "%' AND ";

                strWhereClause = !string.IsNullOrWhiteSpace(strWhereClause) ? strWhereClause.Substring(0, strWhereClause.Length - 5) : "";

                if (intPageSize > 0)
                    listMstRejectDtl = MstRejectDtlRepo.GetPaged(strWhereClause, strOrderBy, intRowSkip, intPageSize);
                else

                    listMstRejectDtl = MstRejectDtlRepo.GetList(strWhereClause, strOrderBy);


                return listMstRejectDtl;
            }
            catch (Exception ex)
            {
                listMstRejectDtl.Add(new mstPICADetail((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "mstRejectDtlService", "GetmstPICADetailToList", UserID)));
                return listMstRejectDtl;
            }
            finally
            {
                context.Dispose();
            }
        }

        public mstPICAType GetEmailByPICAType(string UserID, int HdrId)
        {
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var uow = context.CreateUnitOfWork();
            var picaRepo = new mstPICATypeRepository(context);
            mstPICAType pica = new mstPICAType();
            
            try
            {
                pica = picaRepo.GetByPK(HdrId);
                
                return pica;
            }
            catch (Exception ex)
            {
                uow.Dispose();
                return new mstPICAType((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "mstRejectService", "GetEmailByPICAType", UserID));
            }
            finally
            {
                context.Dispose();
            }
        }

    }
}
