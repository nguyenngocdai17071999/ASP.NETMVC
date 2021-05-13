using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HocWeb.Areas.Admin.Code;

namespace HocWeb.Areas.Admin.Code
{
    public class HasCredentialAttribute : AuthorizeAttribute
    {
        public string RoleID { set; get; }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {          
            var session = (UserSession)HttpContext.Current.Session[CommomConstants.USER_SESSION];
            if(session==null)
            {
                return false;
            }
            List<string> privilegeLevels =this.GetCredentialByLoggedInUser(session.TenTK);

            if(privilegeLevels.Contains(this.RoleID) || session.ChucVu==CommomConstants.ADMIN_GROUP)
            {
                return true;
            }
            else
            {
                return false;
            }
           
        }
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new ViewResult
            {
                ViewName = "~/Areas/Admin/Views/Shared/View404.cshtml"
            };
        }
        private List<string> GetCredentialByLoggedInUser(string username)
        {
            var cerdentials = (List<string>)HttpContext.Current.Session[CommomConstants.SESSION_CREDENTIAL];
            return cerdentials;
        }
    }
}