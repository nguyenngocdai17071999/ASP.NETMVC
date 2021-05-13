using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HocWeb.Areas.Admin.Code
{
    public static class CommomConstants
    {
        public static string USER_SESSION = "USER_SESSION";

        public static string SESSION_CREDENTIAL = "SESSION_CREDENTIALS";

        public static string CartSession = "CartSession";


        public static string CurrentCulture { set; get; }

        public static string MEMBER_GROUP = "1";
        public static string ADMIN_GROUP = "2";
        public static string MODE_FROUP = "3";
    }
}