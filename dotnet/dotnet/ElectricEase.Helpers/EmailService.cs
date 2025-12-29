using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net.Mail;

namespace ElectricEase.Helpers
{
    public class EmailService
    {

        public string SendEmail(EmailLog emailLog, List<EmailAttachments> attachments = null)
        {
            string stat = "";
            try
            {
                string EmailId = System.Configuration.ConfigurationSettings.AppSettings["FromEmail"];
                string Password = System.Configuration.ConfigurationSettings.AppSettings["FromPassword"];
                System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();
                message.From = new System.Net.Mail.MailAddress(EmailId);
                message.Subject = emailLog.Subject;
                message.Body = emailLog.BodyContent;
                message.IsBodyHtml = true;

                message.BodyEncoding = System.Text.Encoding.GetEncoding("utf-8");
             

                if (emailLog.EmailTo.ToString().Contains(','))
                {
                    string[] mailids = emailLog.EmailTo.ToString().Split(',');
                    foreach (string item in mailids)
                    {
                        if (item.Trim() != "")
                        {
                            message.Bcc.Add(new System.Net.Mail.MailAddress(item));
                        }
                    }
                }
                else
                    message.To.Add(new System.Net.Mail.MailAddress(emailLog.EmailTo));
                if (emailLog.EmailCC != null && emailLog.EmailCC != "")
                {
                    if (emailLog.EmailCC.ToString().Contains(','))
                    {
                        string[] ccmailids = emailLog.EmailCC.ToString().Split(',');
                        foreach (string item in ccmailids)
                        {
                            if (item.Trim() != "")
                            {
                                message.CC.Add(new System.Net.Mail.MailAddress(item));
                            }
                        }
                    }
                    else
                        message.CC.Add(new System.Net.Mail.MailAddress(emailLog.EmailCC));
                }
                if (attachments != null && attachments.Count > 0)
                {
                    foreach (var item in attachments)
                    {
                        Attachment att = new Attachment(new MemoryStream(item.Attachment), item.AttachmentName);
                        message.Attachments.Add(att);
                    }
                }

                System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient();
                smtp.Port = 587;
                smtp.Host = "smtp.gmail.com";
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new System.Net.NetworkCredential(EmailId, Password);
                smtp.DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network;
                smtp.Send(message);
                stat = "success";
                emailLog.Status = "Sent";
                emailLog.SentDate = DateTime.Now;
                emailLog.isSent = true;
            }
            catch (Exception ex)
            {
                emailLog.Status = "Failed";
                emailLog.isSent = false;
                stat = ex.Message + " and " + ex.InnerException;
            }

            //if (attachments != null && attachments.Count > 0)
            //{
            //    foreach (var item in attachments)
            //    {
            //        item.EmailId = emailLog.Id;

            //    }
            //}
            return stat;
        }
    }
}
