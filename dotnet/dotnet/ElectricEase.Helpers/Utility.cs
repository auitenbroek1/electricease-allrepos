using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;
using System.IO;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Data;
using System.Text;
using System.Globalization;
using System.Net;

using System.Web.Http;
using System.Net.Mail;
namespace ElectricEase.Helpers
{
    public static class Utility
    {

        public static string SendEmailForgotPassword(string toEmail, string name, string Sender_EmailAddress, string Sender_EmailPassword, string DomainName, string SMTP_Host, int? SMTP_Port)
        {
            string mailBody = System.IO.File.ReadAllText(System.Web.HttpContext.Current.Server.MapPath("~/EmailTemplates/ForgetPassowrd.html"));
            mailBody = mailBody.Replace("{username}", name);
            mailBody = mailBody.Replace("{domainname}", Settings.DomainName);
            mailBody = mailBody.Replace("{domainurl}", ConfigurationManager.AppSettings["websiteURL"].ToString() + "Login/ChangePassword" + "?Email=" + ElectricHelper.Encrypt(toEmail) + "&Expire=" + ElectricHelper.Encrypt(DateTime.Now.AddDays(1).ToString()));
            // mailBody = mailBody.Replace("{domainurl}", "http://localhost:49782/Login/ChangePassword" + "?Email=" + ElectricHelper.Encrypt(toEmail) + "&Expire=" + ElectricHelper.Encrypt(DateTime.Now.AddDays(1).ToString()));
            mailBody = mailBody.Replace("{companyname}", Settings.CompanyName);

            //  mailBody = mailBody.Replace("{token}", token);
            string[] toMail = new string[] { toEmail };
            return SendMail(toMail, "ElectricEase Password Reset", mailBody);
        }

        public static string SendMail(string[] toEmailAddress, string subjectText, string bodyText, string[] bccEmailAddress = null, string[] ccEmailAddress = null)
        {
            try
            {
                System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();

                //emailBody = ReadApprovalTemplate().Replace("{Approver}", Basics.GetString(dt.Rows[0]["DISPLAY_NAME"])).Replace("{DocumentNo}", DocumentNo);
                //emailBody = emailBody.Replace("{DocumentDate}", DocumentDate).Replace("{Owner}", Owner);
                //emailBody = emailBody.Replace("{SalesEmployee}", SalesEmployee).Replace("{TotalAmount}", TotalAmount);
                //emailBody = emailBody.Replace("{Customer}", CustomerName);

                // Add To Email Addresses
                if (toEmailAddress == null)
                    return "Receiver email address can't be empty";

                foreach (var emailAddress in toEmailAddress)
                    message.To.Add(new System.Net.Mail.MailAddress(emailAddress));

                // Add BCC Email Addresses
                if (bccEmailAddress != null)
                {
                    foreach (var emailAddress in bccEmailAddress)
                        message.Bcc.Add(new System.Net.Mail.MailAddress(emailAddress));
                }

                // Add CC Email Addresses
                if (ccEmailAddress != null)
                {
                    foreach (var emailAddress in ccEmailAddress)
                        message.CC.Add(new System.Net.Mail.MailAddress(emailAddress));
                }

                // Define Mail message
                message.From = new System.Net.Mail.MailAddress(Settings.SenderEmailAddress);
                message.Subject = subjectText;
                message.Body = bodyText;
                message.IsBodyHtml = true;

                // Smtp configuration
                System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient();
                smtp.Port = 587;
                // int port=Settings.SmtpPort==null?25:25;
                // smtp.Port = port;
                //smtp.Host = Settings.SmtpHost;
                smtp.Host = "smtp.gmail.com";
                //smtp.Host = "relay-hosting.secureserver.net";
                smtp.EnableSsl = true;
                // smtp.EnableSsl = false;
                smtp.UseDefaultCredentials = false;
                //smtp.UseDefaultCredentials = true;
                smtp.Credentials = new System.Net.NetworkCredential(Settings.SenderEmailAddress, Settings.SenderEmailPassword);
                smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                smtp.Send(message);

                return "";
            }
            catch (Exception ex)
            {
                return "Sending Email failure!..." + ex.Message;
            }
        }

        public static List<State> GetStates()
        {
            List<State> lststate = new List<State>();
            State state = new State();
            state.Value = "AL";
            state.Name = "AL- ALABAMA";
            lststate.Add(state);
            state = new State();
            state.Value = "AK";
            state.Name = "AK- ALASKA";
            lststate.Add(state);
            state = new State();
            state.Value = "AZ";
            state.Name = "AZ- ARIZONA";
            lststate.Add(state);
            state = new State();
            state.Value = "AR";
            state.Name = "AR- ARKANSAS";
            lststate.Add(state);
            state = new State();
            state.Value = "CA";
            state.Name = "CA- CALIFORNIA";
            lststate.Add(state);
            state = new State();
            state.Value = "CO";
            state.Name = "CO- COLORADO";
            lststate.Add(state);
            state = new State();
            state.Value = "CT";
            state.Name = "CT- CONNECTICUT";
            lststate.Add(state);
            state = new State();
            state.Value = "DE";
            state.Name = "DE- DELAWARE";
            lststate.Add(state);
            state = new State();
            state.Value = "DC";
            state.Name = "DC- DISTRICT OF COLUMBIA";
            lststate.Add(state);
            state = new State();
            state.Value = "FM";
            state.Name = "FM- FEDERATED STATES OF MICRONESIA";
            lststate.Add(state);
            state = new State();
            state.Value = "FL";
            state.Name = "FL- FLORIDA";
            lststate.Add(state);
            state = new State();
            state.Value = "GA";
            state.Name = "GA- GEORGIA";
            lststate.Add(state);
            state = new State();
            state.Value = "GU";
            state.Name = "GU- GUAM";
            lststate.Add(state);
            state = new State();
            state.Value = "HI";
            state.Name = "HI- HAWAII";
            lststate.Add(state);
            state = new State();
            state.Value = "ID";
            state.Name = "ID- IDAHO";
            lststate.Add(state);
            state = new State();
            state.Value = "IL";
            state.Name = "IL- ILLINOIS";
            lststate.Add(state);
            state = new State();
            state.Value = "IN";
            state.Name = "IN- INDIANA";
            lststate.Add(state);
            state = new State();
            state.Value = "IA";
            state.Name = "IA- IOWA";
            lststate.Add(state);
            state = new State();
            state.Value = "KS";
            state.Name = "KS- KANSAS";
            lststate.Add(state);
            state = new State();
            state.Value = "KY";
            state.Name = "KY- KENTUCKY";
            lststate.Add(state);
            state = new State();
            state.Value = "LA";
            state.Name = "LA- LOUISIANA";
            lststate.Add(state);
            state = new State();
            state.Value = "ME";
            state.Name = "ME- MAINE";
            lststate.Add(state);
            state = new State();
            state.Value = "MH";
            state.Name = "MH- MARSHALL ISLANDS";
            lststate.Add(state);
            state = new State();
            state.Value = "MD";
            state.Name = "MD- MARYLAND";
            lststate.Add(state);
            state = new State();
            state.Value = "MA";
            state.Name = "MA- MASSACHUSETTS";
            lststate.Add(state);
            state = new State();
            state.Value = "MI";
            state.Name = "MI- MICHIGAN";
            lststate.Add(state);
            state = new State();
            state.Value = "MN";
            state.Name = "MN- MINNESOTA";
            lststate.Add(state);
            state = new State();
            state.Value = "MS";
            state.Name = "MS- MISSISSIPPI";
            lststate.Add(state);
            state = new State();
            state.Value = "MO";
            state.Name = "MO- MISSOURI";
            lststate.Add(state);
            state = new State();
            state.Value = "MT";
            state.Name = "MT- MONTANA";
            lststate.Add(state);
            state = new State();
            state.Value = "NE";
            state.Name = "NE- NEBRASKA";
            lststate.Add(state);
            state = new State();
            state.Value = "NV";
            state.Name = "NV- NEVADA";
            lststate.Add(state);
            state = new State();
            state.Value = "NH";
            state.Name = "NH- NEW HAMPSHIRE";
            lststate.Add(state);
            state = new State();
            state.Value = "NJ";
            state.Name = "NJ- NEW JERSEY";
            lststate.Add(state);
            state = new State();
            state.Value = "NM";
            state.Name = "NM- NEW MEXICO";
            lststate.Add(state);
            state = new State();
            state.Value = "NY";
            state.Name = "NY- NEW YORK";
            lststate.Add(state);
            state = new State();
            state.Value = "NC";
            state.Name = "NC- NORTH CAROLINA";
            lststate.Add(state);
            state = new State();
            state.Value = "ND";
            state.Name = "ND- NORTH DAKOTA";
            lststate.Add(state);
            state = new State();
            state.Value = "MP";
            state.Name = "MP- NORTHERN MARIANA ISLANDS";
            lststate.Add(state);
            state = new State();
            state.Value = "OH";
            state.Name = "OH- OHIO";
            lststate.Add(state);
            state = new State();
            state.Value = "OK";
            state.Name = "OK- OKLAHOMA";
            lststate.Add(state);
            state = new State();
            state.Value = "OR";
            state.Name = "OR- OREGON";
            lststate.Add(state);
            //state = new State();
            //state.Value = "PW";
            //state.Name = "OR- OREGON";
            //lststate.Add(state);
            state = new State();
            state.Value = "PA";
            state.Name = "PA- PENNSYLVANIA";
            lststate.Add(state);
            state = new State();
            state.Value = "PR";
            state.Name = "PR- PUERTO RICO";
            lststate.Add(state);
            state = new State();
            state.Value = "RI";
            state.Name = "RI- RHODE ISLAND";
            lststate.Add(state);
            state = new State();
            state.Value = "SC";
            state.Name = "SC- SOUTH CAROLINA";
            lststate.Add(state);
            state = new State();
            state.Value = "SD";
            state.Name = "SD- SOUTH DAKOTA";
            lststate.Add(state);
            state = new State();
            state.Value = "TN";
            state.Name = "TN- TENNESSEE";
            lststate.Add(state);
            state = new State();
            state.Value = "TX";
            state.Name = "TX- TEXAS";
            lststate.Add(state);
            state = new State();
            state.Value = "UT";
            state.Name = "UT- UTAH";
            lststate.Add(state);
            state = new State();
            state.Value = "VT";
            state.Name = "VT- VERMONT";
            lststate.Add(state);
            state = new State();
            state.Value = "VI";
            state.Name = "VI- VIRGIN ISLANDS";
            lststate.Add(state);
            state = new State();
            state.Value = "VA";
            state.Name = "VA- VIRGINIA";
            lststate.Add(state);
            state = new State();
            state.Value = "WA";
            state.Name = "WA- WASHINGTON";
            lststate.Add(state);
            state = new State();
            state.Value = "WV";
            state.Name = "WV- WEST VIRGINIA";
            lststate.Add(state);
            state = new State();
            state.Value = "WI";
            state.Name = "WI- WISCONSIN";
            lststate.Add(state);
            state = new State();
            state.Value = "WY";
            state.Name = "WY- WYOMING";
            lststate.Add(state);
            return lststate;
        }
        //public static void SendEmail(string email, string subject, string body)
        //{
        //    System.Net.Mail.SmtpClient client = new System.Net.Mail.SmtpClient();
        //    client.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
        //    client.EnableSsl = true;
        //    client.Host = "smtp.gmail.com";
        //    client.Port = 587;

        //    System.Net.NetworkCredential credentials = new System.Net.NetworkCredential("","");
        //    client.Credentials = credentials;

        //    System.Net.Mail.MailMessage msg = new System.Net.Mail.MailMessage();
        //    msg.From= new MailAddress("xxxxx@gmail.com");
        //    msg.To.Add(new MailAddress(email));

        //    msg.Subject = subject;
        //    msg.IsBodyHtml = true;
        //    msg.Body = body;



        //   client.Send(msg);


        //}
    }
    public class State
    {
        public string Value { get; set; }
        public string Name { get; set; }
    }
}

