using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using ElectricEase.BLL;
using ElectricEase.Models;
using ElectricEase.Helpers;
using System.Web.Security;
using System.IO;
using ElectricEase.BLL.ClientMaster;

namespace ElectricEase.Web
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            log4net.Config.XmlConfigurator.Configure(new FileInfo(Server.MapPath("~/Web.config")));
            AreaRegistration.RegisterAllAreas();
            var s=FormsAuthentication.Timeout.Duration();
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AuthConfig.RegisterAuth();
        }
        protected void Application_PostAuthenticateRequest(Object sender, EventArgs e)
        {
            string roles = string.Empty;
          //  var sd = Session.Timeout;
            var s = FormsAuthentication.Timeout.Duration();
            if (FormsAuthentication.CookiesSupported == true)
            {
                if (Request.Cookies[".ASPXAUTH"] != null)
                {
                    try
                    {
                        
                        //let us take out the username now 
                        string username = FormsAuthentication.Decrypt(Request.Cookies[".ASPXAUTH"].Value).Name;
                    
                        if (username == "Admin")
                        {
                            roles = "Admin";

                            HttpContext.Current.User = new System.Security.Principal.GenericPrincipal(
                          new System.Security.Principal.GenericIdentity("Admin", "Forms"), roles.Split(';'));
                        }
                        else
                        {
                            // ElectricBLL response = new ElectricBLL();
                            ClientMasterBLL ClientResponse = new ClientMasterBLL();
                            Account_MasterInfo modelobj = new Account_MasterInfo();
                            modelobj = ClientResponse.GetClientName(username);

                            if(modelobj.Site_Administrator)
                            {
                                roles += "SiteAdmin;"; 
                            }

                            if (modelobj.Account_Administrator)
                            {
                                roles += "AccountAdmin;";
                            }

                            if (modelobj.Job_Administrator)
                            {
                                roles += "JobAdmin;";
                            }

                            if (modelobj.Part_Administrator)
                            {
                                roles += "PartAdmin;";
                            }

                            if (modelobj.Labor_Administrator)
                            {
                                roles += "LaborAdmin;";
                            }

                            if (modelobj.Legal_Adminstrator)
                            {
                                roles += "LegalAdmin;";
                            }

                            if (modelobj.Assembly_Administrator)
                            {
                                roles += "AssemblyAdmin;";
                            }

                            if (modelobj.Job_Description_Report)
                            {
                                roles += "JobDescriptionReportAdmin;";
                            }

                            if (modelobj.Client_Estimation_Report)
                            {
                                roles += "ClientEstimationReportAdmin;";
                            }

                            HttpContext.Current.Items["MyAccountRights"] = modelobj.Account_Administrator;
                            HttpContext.Current.Items["MyPartsRights"] = modelobj.Part_Administrator;
                            HttpContext.Current.Items["MyLabourMasterRights"] = modelobj.Labor_Administrator;
                            HttpContext.Current.Items["MyPartsMasterRights"] = modelobj.Part_Administrator;
                            HttpContext.Current.Items["MyLegalMasterRights"] = modelobj.Legal_Adminstrator;
                            //Below two lines are modifed by Arunkumar.K on 12/17/2015.
                            HttpContext.Current.Items["MyJobMasterRights"] = modelobj.Job_Administrator;
                            HttpContext.Current.Items["MyAssembliesRights"] = modelobj.Assembly_Administrator;
                            HttpContext.Current.Items["JobDescriptionReportRights"] = modelobj.Job_Description_Report;
                            HttpContext.Current.Items["ClientEstimation_ReportRights"] = modelobj.Client_Estimation_Report;

                            HttpContext.Current.Items["SuperUser"] = modelobj.CreatedBy_SuperUser;

                            HttpContext.Current.Items["SiteAdministrator"] = modelobj.Site_Administrator;

                            //let us extract the roles from our own custom cookie
                            //Let us set the Pricipal with our user specific details
                            HttpContext.Current.User = new System.Security.Principal.GenericPrincipal(
                                new System.Security.Principal.GenericIdentity(username, "Forms"), roles.Split(';'));


                            HttpContext.Current.Items["JobAssemblyOn"] = 0;

                        }


                    }
                    catch (Exception ex)
                    {
                        string url = Request.Url.AbsolutePath;
                        if (url != "/" && url != "/Login/Login" && url != "/Login/ForgotPassword" && url != "/Login/SessionTimout" && url != "/Login/ChangePassword" && url != "/Login/SaveNewPassword" && !url.Contains("/Client_Master/Register?FirstName"))
                            HttpContext.Current.Response.Redirect("/Login/SessionTimout?error="+ex.Message);
                    }
                }
                else
                {
                    string stat = "";
                    string allowanonymous = "";
                    if (Request.Cookies["AllowAnonymous"] != null)
                    {
                        allowanonymous = Request.Cookies["AllowAnonymous"].Value;
                    }
                    if (Request.Cookies[".ASPXAUTH"] != null)
                    {
                        try
                        {
                            stat = FormsAuthentication.Decrypt(Request.Cookies[".ASPXAUTH"].Value).Name;
                        }
                        catch (Exception ex)
                        {
                            stat = ex.Message;
                        }
                    }
                    string url = Request.Url.AbsolutePath;
                    if (url != "/" && url != "/Login/Login" && url != "/Login/ForgotPassword" && url != "/Login/SessionTimout" && url != "/Login/ChangePassword" && url !="/Client_Master/Register" && url != "/Client_Master/Register?FirstName=&LastName=&Phone=&Mobile=&Email=&CompanyName=" && !url.Contains("/Client_Master/Register?FirstName") && url != "/Login/SaveNewPassword" && !url.Contains("/api/MobileApp/") && (allowanonymous == "" || allowanonymous=="false"))
                        HttpContext.Current.Response.Redirect("/Login/SessionTimout?returnurl=" + url + "&stat=" + stat);
                }
            }
            else
            {
                string url = Request.Url.AbsolutePath;
                if (url != "/" && url != "/Login/Login" && url != "/Login/ForgotPassword" && url != "/Login/SessionTimout" && url != "/Login/ChangePassword" && url != "/Login/SaveNewPassword" && url != "/Client_Master/Register" && url != "/Client_Master/Register?FirstName=&LastName=&Phone=&Mobile=&Email=&CompanyName=" && !url.Contains("/Client_Master/Register?FirstName") && !url.Contains("/api/MobileApp/"))
                    HttpContext.Current.Response.Redirect("/Login/SessionTimout?returnurl="+url);
            }
        }
        protected void session_TimeOut(object sender,EventArgs e)
        {
            Session.Timeout = 1200;
        }
        //protected void Application_BeginRequest()
        //{
        //    Response.Cache.SetCacheability(HttpCacheability.NoCache);
        //    Response.Cache.SetExpires(DateTime.UtcNow.AddHours(-1));
        //    Response.Cache.SetNoStore();
        //}
    }
}