using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.Security;
using System.Web.Services.Description;
using System.Web.SessionState;

namespace CreditScore.WebApp
{
    public class Global : HttpApplication
    {
        void Application_Start(object sender, EventArgs e)
        {
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
        void Session_Start(object sender, EventArgs e)
        {
            try
            {
                Session["userCulture"] = "fa-IR";
                Session["userFileID"] = "";
                Session["objectAV"] = null;
                Session["RetailModelSM"] = string.Empty;
                Session["RetailModelJG"] = string.Empty;
                Session["BuisModelSM"] = string.Empty;
                Session["BuisModelJG"] = string.Empty;
                if (Session["user"] != null)
                {
                    Session["userid"] = 0;
                }
            }
            catch (Exception exp)
            {
                Response.Write(exp.ToString());
            }
        }
        public string NormalizePersianCharacters(string text)
        {
            if (text == null)
                return null;

            text = text.Replace("\u0660", "\u06F0"); // ۰
            text = text.Replace("\u0661", "\u06F1"); // ۱
            text = text.Replace("\u0662", "\u06F2"); // ۲
            text = text.Replace("\u0663", "\u06F3"); // ۳
            text = text.Replace("\u0664", "\u06F4"); // ۴
            text = text.Replace("\u0665", "\u06F5"); // ۵
            text = text.Replace("\u0666", "\u06F6"); // ۶
            text = text.Replace("\u0667", "\u06F7"); // ۷
            text = text.Replace("\u0668", "\u06F8"); // ۸
            text = text.Replace("\u0669", "\u06F9"); // ۹
            text = text.Replace("\u0643", "\u06A9"); // ک
            text = text.Replace("\u0649", "\u06CC"); // ی
            text = text.Replace("\u064A", "\u06CC"); // ی
            text = text.Replace("\u06C0", "\u0647\u0654"); // هٔ
            return text;
        }
        public DateTime CheckDate(string date)
        {
            return DateTime.Now;
        }
        public string GetGregorian(string date)
        {
            return string.Empty;
        }
        public string GetGregorianR(string date)
        {
            return string.Empty;
        }
        public string GetPersian(DateTime Date)
        {
            return string.Empty;

        }
    }
}