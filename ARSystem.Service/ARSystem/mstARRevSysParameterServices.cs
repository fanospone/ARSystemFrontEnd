using System;
using System.Collections.Generic;
using System.Linq;
using ARSystem.Domain.DAL;
using ARSystem.Domain.Models;
using ARSystem.Domain.Repositories;
using System.Reflection;

namespace ARSystem.Service
{
    public partial class mstARRevSysParameterServices
    {
        #region 1

        public static string ServiceName = "mstARRevSysParameterServices";
        public List<mstARRevSysParameter> GetmstARRevSysParameterList(string UserID,  string strOrderBy, int intRowSkip = 0, int intPageSize = 0)
        {
            var context = new DbContext(Helper.GetConnection("ARSystemDWH"));
            var mstARRevSysParameterRepo = new mstARRevSysParameterRepository(context);
            List<mstARRevSysParameter> mstARRevSysParameterList = new List<mstARRevSysParameter>();
            string role = UserHelper.GetUserARPosition(UserID);
           
            try
            {
                string strWhereClause = "1=1";
                                 
               
                if (intPageSize > 0)
                    mstARRevSysParameterList = mstARRevSysParameterRepo.GetPaged(strWhereClause, strOrderBy, intRowSkip, intPageSize);
                else
                    mstARRevSysParameterList = mstARRevSysParameterRepo.GetList(strWhereClause, strOrderBy);

                return mstARRevSysParameterList;
            }
            catch (Exception ex)
            {
                mstARRevSysParameterList.Add(new mstARRevSysParameter((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), ServiceName, MethodBase.GetCurrentMethod().Name.ToString(), UserID)));
                return mstARRevSysParameterList;
            }
            finally
            {
                context.Dispose();
            }
        }

        public int CountmstARRevSysParameterList(string UserID)            
        {
            var context = new DbContext(Helper.GetConnection("ARSystemDWH"));
            var mstARRevSysParameterRepo = new mstARRevSysParameterRepository(context);
            List<mstARRevSysParameter> mstARRevSysParameterList = new List<mstARRevSysParameter>();
            
            try
            {
                string strWhereClause = "1=1";                              
                return mstARRevSysParameterRepo.GetCount(strWhereClause);
            }
            catch (Exception ex)
            {
                mstARRevSysParameterList.Add(new mstARRevSysParameter((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), ServiceName, MethodBase.GetCurrentMethod().Name.ToString(), UserID)));
                return 0;
            }
            finally
            {
                context.Dispose();
            }
        }
        
        public mstARRevSysParameter mstARRevSysParameterCreate(string UserID, mstARRevSysParameter Input)
        {
            var context = new DbContext(Helper.GetConnection("ARSystemDWH"));
            mstARRevSysParameterRepository repo = new mstARRevSysParameterRepository(context);
            var uow = context.CreateUnitOfWork();

            
            try
            {
                Input.CreatedBy = UserID;
                Input.CreatedDate = Helper.GetDateTimeNow();

                uow.SaveChanges();
                return Input = repo.Create(Input);

            }
            catch (Exception ex)
            {
                uow.Dispose();
                return new mstARRevSysParameter((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), ServiceName, MethodBase.GetCurrentMethod().Name.ToString(), UserID));
            }
            finally
            {
                context.Dispose();
            }

        }

        public List<mstARRevSysParameter> mstARRevSysParameterCreateBulky(string UserID, List<mstARRevSysParameter> Input)
        {
            var context = new DbContext(Helper.GetConnection("ARSystemDWH"));
            mstARRevSysParameterRepository repo = new mstARRevSysParameterRepository(context);
            List<mstARRevSysParameter> datalist = new List<mstARRevSysParameter>();
            var uow = context.CreateUnitOfWork();

           
            try
            {
                datalist = repo.CreateBulky(Input);
                uow.SaveChanges();
                return datalist;

            }
            catch (Exception ex)
            {
                uow.Dispose();
                datalist.Add(new mstARRevSysParameter((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), ServiceName, System.Reflection.MethodBase.GetCurrentMethod().Name.ToString(), UserID)));
                return datalist;
            }
            finally
            {
                context.Dispose();
            }

        }

        public mstARRevSysParameter mstARRevSysParameterUpdate(string UserID, mstARRevSysParameter Input)
        {
            var context = new DbContext(Helper.GetConnection("ARSystemDWH"));
            mstARRevSysParameterRepository repo = new mstARRevSysParameterRepository(context);
            var uow = context.CreateUnitOfWork();
            
            try
            {
                Input.UpdatedBy = UserID;
                Input.UpdatedDate = Helper.GetDateTimeNow();
                Input = repo.Update(Input);
                uow.SaveChanges();
                return Input;

            }
            catch (Exception ex)
            {
                uow.Dispose();
                return new mstARRevSysParameter((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), ServiceName, MethodBase.GetCurrentMethod().Name.ToString(), UserID));
            }
            finally
            {
                context.Dispose();
            }

        }
        public mstARRevSysParameter mstARRevSysParameterStartJob(string Flag, mstARRevSysParameter Input)
        {
            var context = new DbContext(Helper.GetConnection("HTBGSSIS"));
            mstARRevSysParameterRepository repo = new mstARRevSysParameterRepository(context);
            var uow = context.CreateUnitOfWork();

            try
            {
                Input.ParamValue = "OK";
                if (Flag == "start")
                { repo.StartJob(); }
                else
                { repo.StopJob(); }
               
                return Input;

            }
            catch (Exception ex)
            {
                uow.Dispose();
                return new mstARRevSysParameter((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), ServiceName, MethodBase.GetCurrentMethod().Name.ToString(), "JOB"));
            }
            finally
            {
                context.Dispose();
            }

        }
        #endregion
    }
}
