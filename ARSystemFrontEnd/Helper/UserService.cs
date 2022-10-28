using System;
using System.Collections.Generic;
using ARSystem.Domain.Models;
using ARSystemFrontEnd.Helper;
using ARSystem.Domain.Repositories;
using ARSystem.Domain.DAL;

namespace ARSystemFrontEnd.Helper
{
    public static class UserService
    {
        public static vmUserCredential CheckUserToken(string strToken)
        {
            vmUserCredential userCredential = new vmUserCredential();

            try
            {
                using (var client = new SecureAccessService.UserServiceClient())
                {
                    SecureAccessService.vmUserCredential userCredentialSA = new SecureAccessService.vmUserCredential();
                    userCredentialSA = client.CheckUserToken(strToken);

                    if (userCredentialSA.ErrorType > 0)
                    {
                        return new vmUserCredential(userCredentialSA.ErrorType, userCredentialSA.ErrorMessage);
                    }

                    userCredential.UserID = userCredentialSA.UserID;
                    userCredential.UserRoleID = userCredentialSA.UserRoleID;
                }

                return userCredential;
            }
            catch (Exception ex)
            {
                return new vmUserCredential(0, "");
            }
        }
        public static vmStringResult GetARUserPosition(string strToken)
        {
            var context = new DbContext(ARSystem.Service.Helper.GetConnection("ARSystem"));
            var uow = context.CreateUnitOfWork();
            ARSystem.Domain.Models.vmUserCredential userCredential = CheckUserToken(strToken);
            if (userCredential.ErrorType > 0)
                return new vmStringResult(userCredential.ErrorType, userCredential.ErrorMessage);

            try
            {
                vmStringResult StringModel = new vmStringResult();
                StringModel.Result = UserHelper.GetUserARPosition(userCredential.UserID);
                return StringModel;
            }
            catch (Exception ex)
            {
                uow.Dispose();
                return new vmStringResult(0, "");
            }
            finally
            {
                context.Dispose();
            }

        }

        public static List<vwARUserRole> GetARUserRole(string strToken)
        {
            var context = new DbContext(ARSystem.Service.Helper.GetConnection("ARSystem"));
            var uow = context.CreateUnitOfWork();
            ARSystem.Domain.Models.vmUserCredential userCredential = CheckUserToken(strToken);
            List<vwARUserRole> userRole = new List<vwARUserRole>();
            vwARUserRoleRepository repo = new vwARUserRoleRepository(context);
            if (userCredential.ErrorType > 0)
            {
                userRole.Add(new vwARUserRole(userCredential.ErrorType, userCredential.ErrorMessage));
                return userRole;
            }

            try
            {
                return UserHelper.GetUserRoles(userCredential.UserID);
            }
            catch (Exception ex)
            {
                uow.Dispose();
                userRole.Add(new vwARUserRole(0,""));
                return userRole;
            }
            finally
            {
                context.Dispose();
            }
        }

        public static vmStringResult ValidateUser(string strToken, SecureAccessService.vmLogin login, string role)
        {
            var context = new DbContext(ARSystem.Service.Helper.GetConnection("ARSystem"));
            var uow = context.CreateUnitOfWork();
            ARSystem.Domain.Models.vmUserCredential userCredential = CheckUserToken(strToken);
            vwARUserRoleRepository repo = new vwARUserRoleRepository(context);
            if (userCredential.ErrorType > 0)
            {
                return new vmStringResult(userCredential.ErrorType, userCredential.ErrorMessage);
            }

            try
            {
                SecureAccessService.vmUserLogin result = new SecureAccessService.vmUserLogin();
                vmStringResult stringResult = new vmStringResult();
                stringResult.Result = string.Empty;
                using (var client = new SecureAccessService.UserServiceClient())
                {
                    result = client.LoginValidation(login);
                    if (!string.IsNullOrEmpty(result.ErrorMessage))
                    {
                        stringResult.Result = result.ErrorMessage;
                    }
                }

                if (stringResult.Result == string.Empty)
                {
                    string tempRole = string.Empty;
                    if (role == "SUPERVISOR")
                        tempRole = "Account Receivable Database Section Head";
                    else if (role == "DEPT HEAD")
                        tempRole = "Account Receivable Database Department Head";

                    int countRole = repo.GetCount("UserID = '" + login.UserID + "' AND HCISPosition LIKE '%" + tempRole + "%'");

                    if (countRole <= 0)
                        stringResult.Result = "User ID is incorrect!";
                }

                return stringResult;
            }
            catch (Exception ex)
            {
                uow.Dispose();
                // return new vmStringResult((int)Helper.ErrorType.Error, Helper.logError(ex.Message.ToString(), "UserService", "GetARUserPosition", userCredential.UserID));
                return new vmStringResult(0, "");
            }
            finally
            {
                context.Dispose();
            }
        }
    }
}
