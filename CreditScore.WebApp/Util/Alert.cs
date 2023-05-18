using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace CreditScore.WebApp.Util
{
    public static class Alert
    {
        static StackTrace _stackTrace = new StackTrace();
        public static void Show()
        {
            System.Web.UI.ScriptManager.RegisterClientScriptBlock(
                HttpContext.Current.CurrentHandler as Page,
                typeof(Alert), "Alert",
                "alert('" + _stackTrace.GetFrame(1).GetMethod().Name + "');",
                true);
        }
        public static void Info(string MSG)
        {
            System.Web.UI.ScriptManager.RegisterClientScriptBlock(
                HttpContext.Current.CurrentHandler as Page,
                typeof(Alert), "Alert",
                "alert('" + MSG + "');",
                true);
        }
    }
}