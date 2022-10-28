using ARSystem.Domain.DAL;
using ARSystem.Domain.Models;
using ARSystem.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARSystem.Service
{
    public class trxARReOPenPaymentDateService
    {
        public int GetCountPaymentDateRequest(TrxARReOpenPaymentDate post)
        {
            List<TrxARReOpenPaymentDate> list = new List<TrxARReOpenPaymentDate>();

            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var repo = new TrxARReOpenPaymentDateRepository(context);
            try
            {
                string strWhereClause = "";
                if (!string.IsNullOrWhiteSpace(post.RequestNumber))
                {
                    strWhereClause += "RequestNumber LIKE '%" + post.RequestNumber.TrimStart().TrimEnd() + "%' AND ";
                }

                if (post.PaymentDateRevision != null && post.PaymentDateRevision2 != null)
                {
                    strWhereClause += "PaymentDateRevision between '" + post.PaymentDateRevision + "' AND '" + post.PaymentDateRevision2 + "' AND ";
                }
                else if (post.PaymentDateRevision != null && post.PaymentDateRevision2 == null)
                {
                    strWhereClause += "PaymentDateRevision = '" + post.PaymentDateRevision + "' AND ";
                }

                else if (post.PaymentDateRevision == null && post.PaymentDateRevision2 != null)
                {
                    strWhereClause += "PaymentDateRevision = '" + post.PaymentDateRevision2 + "' AND ";
                }


                if (post.CreateDate != null && post.CreateDate2 != null)
                {
                    strWhereClause += "CAST(a.CreateDate AS Date) between '" + post.CreateDate + "' AND '" + post.CreateDate2 + "' AND ";
                }
                else if (post.CreateDate != null && post.CreateDate2 == null)
                {
                    strWhereClause += "CAST(a.CreateDate AS Date) = '" + post.CreateDate + "' AND ";
                }

                else if (post.CreateDate == null && post.CreateDate2 != null)
                {
                    strWhereClause += "CAST(a.CreateDate AS Date) = '" + post.CreateDate2 + "' AND ";
                }

                strWhereClause = !string.IsNullOrWhiteSpace(strWhereClause) ? strWhereClause.Substring(0, strWhereClause.Length - 5) : "";

                return repo.GetCount(strWhereClause);
            }
            catch (Exception ex)
            {
                list.Add(new TrxARReOpenPaymentDate((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "trxRAReOpenPaymentDate", "GetCountPaymentDateRequest", "")));
                return 0;
            }
            finally
            {
                context.Dispose();
            }

        }

        public List<TrxARReOpenPaymentDate> GetListPaymentDateRequest(TrxARReOpenPaymentDate post, int rowSkip, int pageSize)
        {
            List<TrxARReOpenPaymentDate> list = new List<TrxARReOpenPaymentDate>();

            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var repo = new TrxARReOpenPaymentDateRepository(context);
            try
            {
                string strWhereClause = "";
                if (!string.IsNullOrWhiteSpace(post.RequestNumber))
                {
                    strWhereClause += "RequestNumber LIKE '%" + post.RequestNumber.TrimStart().TrimEnd() + "%' AND ";
                }

                if (post.PaymentDateRevision != null && post.PaymentDateRevision2 != null)
                {
                    strWhereClause += "PaymentDateRevision between '" + post.PaymentDateRevision + "' AND '" + post.PaymentDateRevision2 + "' AND ";
                }
                else if (post.PaymentDateRevision != null && post.PaymentDateRevision2 == null)
                {
                    strWhereClause += "PaymentDateRevision = '" + post.PaymentDateRevision + "' AND ";
                }

                else if (post.PaymentDateRevision == null && post.PaymentDateRevision2 != null)
                {
                    strWhereClause += "PaymentDateRevision = '" + post.PaymentDateRevision2 + "' AND ";
                }


                if (post.CreateDate != null && post.CreateDate2 != null)
                {
                    strWhereClause += "CAST(a.CreateDate AS Date) between '" + post.CreateDate + "' AND '" + post.CreateDate2 + "' AND ";
                }
                else if (post.CreateDate != null && post.CreateDate2 == null)
                {
                    strWhereClause += "CAST(a.CreateDate AS Date) = '" + post.CreateDate + "' AND ";
                }

                else if (post.CreateDate == null && post.CreateDate2 != null)
                {
                    strWhereClause += "CAST(a.CreateDate AS Date) = '" + post.CreateDate2 + "' AND ";
                }


                strWhereClause = !string.IsNullOrWhiteSpace(strWhereClause) ? strWhereClause.Substring(0, strWhereClause.Length - 5) : "";

                return list = repo.GetPaged(strWhereClause, "a.CreateDate DESC", rowSkip, pageSize);
            }
            catch (Exception ex)
            {
                list.Add(new TrxARReOpenPaymentDate((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "trxRAReOpenPaymentDate", "GetListPaymentDateRequest", "")));
                return list;
            }
            finally
            {
                context.Dispose();
            }

        }

        public int GetCountPaymentDateRequestByCompany(TrxARReOpenPaymentDate post, string strUserCompanyCode = "")
        {
            List<TrxARReOpenPaymentDate> list = new List<TrxARReOpenPaymentDate>();

            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var repo = new TrxARReOpenPaymentDateByCompanyRepository(context);
            try
            {
                string strWhereClause = "";
                if (!string.IsNullOrWhiteSpace(post.RequestNumber))
                {
                    strWhereClause += "RequestNumber LIKE '%" + post.RequestNumber.TrimStart().TrimEnd() + "%' AND ";
                }

                if (post.PaymentDateRevision != null && post.PaymentDateRevision2 != null)
                {
                    strWhereClause += "PaymentDateRevision between '" + post.PaymentDateRevision + "' AND '" + post.PaymentDateRevision2 + "' AND ";
                }
                else if (post.PaymentDateRevision != null && post.PaymentDateRevision2 == null)
                {
                    strWhereClause += "PaymentDateRevision = '" + post.PaymentDateRevision + "' AND ";
                }

                else if (post.PaymentDateRevision == null && post.PaymentDateRevision2 != null)
                {
                    strWhereClause += "PaymentDateRevision = '" + post.PaymentDateRevision2 + "' AND ";
                }


                if (post.CreateDate != null && post.CreateDate2 != null)
                {
                    strWhereClause += "CAST(a.CreateDate AS Date) between '" + post.CreateDate + "' AND '" + post.CreateDate2 + "' AND ";
                }
                else if (post.CreateDate != null && post.CreateDate2 == null)
                {
                    strWhereClause += "CAST(a.CreateDate AS Date) = '" + post.CreateDate + "' AND ";
                }

                else if (post.CreateDate == null && post.CreateDate2 != null)
                {
                    strWhereClause += "CAST(a.CreateDate AS Date) = '" + post.CreateDate2 + "' AND ";
                }

                if (!string.IsNullOrEmpty(strUserCompanyCode))
                {
                    strWhereClause += "InvCompanyId ='" + strUserCompanyCode + "' AND ";
                }

                strWhereClause = !string.IsNullOrWhiteSpace(strWhereClause) ? strWhereClause.Substring(0, strWhereClause.Length - 5) : "";

                return repo.GetCount(strWhereClause);
            }
            catch (Exception ex)
            {
                list.Add(new TrxARReOpenPaymentDate((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "trxRAReOpenPaymentDate", "GetCountPaymentDateRequest", "")));
                return 0;
            }
            finally
            {
                context.Dispose();
            }

        }

        public List<TrxARReOpenPaymentDate> GetListPaymentDateRequestByCompany(TrxARReOpenPaymentDate post, int rowSkip, int pageSize, string strUserCompanyCode = "")
        {
            List<TrxARReOpenPaymentDate> list = new List<TrxARReOpenPaymentDate>();

            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var repo = new TrxARReOpenPaymentDateByCompanyRepository(context);
            try
            {
                string strWhereClause = "";
                if (!string.IsNullOrWhiteSpace(post.RequestNumber))
                {
                    strWhereClause += "RequestNumber LIKE '%" + post.RequestNumber.TrimStart().TrimEnd() + "%' AND ";
                }

                if (post.PaymentDateRevision != null && post.PaymentDateRevision2 != null)
                {
                    strWhereClause += "PaymentDateRevision between '" + post.PaymentDateRevision + "' AND '" + post.PaymentDateRevision2 + "' AND ";
                }
                else if (post.PaymentDateRevision != null && post.PaymentDateRevision2 == null)
                {
                    strWhereClause += "PaymentDateRevision = '" + post.PaymentDateRevision + "' AND ";
                }

                else if (post.PaymentDateRevision == null && post.PaymentDateRevision2 != null)
                {
                    strWhereClause += "PaymentDateRevision = '" + post.PaymentDateRevision2 + "' AND ";
                }


                if (post.CreateDate != null && post.CreateDate2 != null)
                {
                    strWhereClause += "CAST(a.CreateDate AS Date) between '" + post.CreateDate + "' AND '" + post.CreateDate2 + "' AND ";
                }
                else if (post.CreateDate != null && post.CreateDate2 == null)
                {
                    strWhereClause += "CAST(a.CreateDate AS Date) = '" + post.CreateDate + "' AND ";
                }

                else if (post.CreateDate == null && post.CreateDate2 != null)
                {
                    strWhereClause += "CAST(a.CreateDate AS Date) = '" + post.CreateDate2 + "' AND ";
                }

                if (!string.IsNullOrEmpty(strUserCompanyCode))
                {
                    strWhereClause += "InvCompanyId ='" + strUserCompanyCode + "' AND ";
                }

                strWhereClause = !string.IsNullOrWhiteSpace(strWhereClause) ? strWhereClause.Substring(0, strWhereClause.Length - 5) : "";

                return list = repo.GetPaged(strWhereClause, "a.CreateDate DESC", rowSkip, pageSize);
            }
            catch (Exception ex)
            {
                list.Add(new TrxARReOpenPaymentDate((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "trxRAReOpenPaymentDate", "GetListPaymentDateRequest", "")));
                return list;
            }
            finally
            {
                context.Dispose();
            }

        }

        public int GetCountPaymentDateDtl(TrxARReOpenPaymentDateDetail post)
        {
            List<TrxARReOpenPaymentDateDetail> list = new List<TrxARReOpenPaymentDateDetail>();
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var repo = new TrxARReOpenPaymentDateDetailRepository(context);
            try
            {
                string strWhereClause = "";
                if (post.TrxARReOpenPaymentDateID != 0)
                {
                    strWhereClause += "TrxARReOpenPaymentDateID = '" + post.TrxARReOpenPaymentDateID + "' AND ";
                }
                if (post.PaymentDate != null || post.PaymentDate.ToString() != "")
                {
                    strWhereClause += "PaymentDate = '" + post.PaymentDate + "' AND ";
                }


                strWhereClause = !string.IsNullOrWhiteSpace(strWhereClause) ? strWhereClause.Substring(0, strWhereClause.Length - 5) : "";

                return repo.GetCount(strWhereClause);
            }
            catch (Exception ex)
            {
                list.Add(new TrxARReOpenPaymentDateDetail((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "trxRAReOpenPaymentDate", "GetListPaymentDateDtl", "")));
                return 0;
            }
            finally
            {
                context.Dispose();
            }
        }


        public List<TrxARReOpenPaymentDateDetail> GetListPaymentDateDtl(TrxARReOpenPaymentDateDetail post, int rowSkip, int pageSize)
        {
            List<TrxARReOpenPaymentDateDetail> list = new List<TrxARReOpenPaymentDateDetail>();

            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var repo = new TrxARReOpenPaymentDateDetailRepository(context);
            try
            {
                string strWhereClause = "";
                if (post.TrxARReOpenPaymentDateID != 0)
                {
                    strWhereClause += "TrxARReOpenPaymentDateID = '" + post.TrxARReOpenPaymentDateID + "' AND ";
                }
                //if (post.PaymentDate != null || post.PaymentDate.ToString() != "")
                //{
                //    strWhereClause += "PaymentDate = '" + post.PaymentDate + "' AND ";
                //}


                strWhereClause = !string.IsNullOrWhiteSpace(strWhereClause) ? strWhereClause.Substring(0, strWhereClause.Length - 5) : "";

                return repo.GetPaged(strWhereClause, "", rowSkip, pageSize);
            }
            catch (Exception ex)
            {
                list.Add(new TrxARReOpenPaymentDateDetail((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "trxRAReOpenPaymentDate", "GetListPaymentDateDtl", "")));
                return list;
            }
            finally
            {
                context.Dispose();
            }
        }

        public int GetCountPaymentDateDtlByCompany(TrxARReOpenPaymentDateDetail post, string strUserCompanyCode = "")
        {
            List<TrxARReOpenPaymentDateDetail> list = new List<TrxARReOpenPaymentDateDetail>();
            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var repo = new TrxARReOpenPaymentDateDetailByCompanyRepository(context);
            try
            {
                string strWhereClause = "";
                if (post.TrxARReOpenPaymentDateID != 0)
                {
                    strWhereClause += "TrxARReOpenPaymentDateID = '" + post.TrxARReOpenPaymentDateID + "' AND ";
                }
                if (post.PaymentDate != null || post.PaymentDate.ToString() != "")
                {
                    strWhereClause += "PaymentDate = '" + post.PaymentDate + "' AND ";
                }
                if (!string.IsNullOrEmpty(strUserCompanyCode))
                {
                    strWhereClause += "InvCompanyId='"+ strUserCompanyCode +"' AND ";
                }

                strWhereClause = !string.IsNullOrWhiteSpace(strWhereClause) ? strWhereClause.Substring(0, strWhereClause.Length - 5) : "";

                return repo.GetCount(strWhereClause);
            }
            catch (Exception ex)
            {
                list.Add(new TrxARReOpenPaymentDateDetail((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "trxRAReOpenPaymentDate", "GetListPaymentDateDtl", "")));
                return 0;
            }
            finally
            {
                context.Dispose();
            }
        }


        public List<TrxARReOpenPaymentDateDetail> GetListPaymentDateDtlByCompany(TrxARReOpenPaymentDateDetail post, int rowSkip, int pageSize, string strUserCompanyCode = "")
        {
            List<TrxARReOpenPaymentDateDetail> list = new List<TrxARReOpenPaymentDateDetail>();

            var context = new DbContext(Helper.GetConnection("ARSystem"));
            var repo = new TrxARReOpenPaymentDateDetailByCompanyRepository(context);
            try
            {
                string strWhereClause = "";
                if (post.TrxARReOpenPaymentDateID != 0)
                {
                    strWhereClause += "TrxARReOpenPaymentDateID = '" + post.TrxARReOpenPaymentDateID + "' AND ";
                }
                //if (post.PaymentDate != null || post.PaymentDate.ToString() != "")
                //{
                //    strWhereClause += "PaymentDate = '" + post.PaymentDate + "' AND ";
                //}

                if (!string.IsNullOrEmpty(strUserCompanyCode))
                {
                    strWhereClause += "InvCompanyId='" + strUserCompanyCode + "' AND ";
                }

                strWhereClause = !string.IsNullOrWhiteSpace(strWhereClause) ? strWhereClause.Substring(0, strWhereClause.Length - 5) : "";

                return repo.GetPaged(strWhereClause, "", rowSkip, pageSize);
            }
            catch (Exception ex)
            {
                list.Add(new TrxARReOpenPaymentDateDetail((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "trxRAReOpenPaymentDate", "GetListPaymentDateDtl", "")));
                return list;
            }
            finally
            {
                context.Dispose();
            }
        }





    }
}
