using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Web;
using ElectricEase.Data.DataBase;

namespace ElectricEase.Data
{
    public class LogsMaster
    {
        public void WriteLog(string message, int clientid, string username)
        {
            string cliename="";
           string filepath = "~/Logs";
           //string filepath = @"C:\ElectricEase_TFS\ElectricEase\Electric Ease Latest\ElectricEase.Web\Logs";
           if (!Directory.Exists(System.Web.HttpContext.Current.Server.MapPath(filepath)))
           {
               Directory.CreateDirectory(System.Web.HttpContext.Current.Server.MapPath(filepath));
           }
            if (clientid != 0)
            {
                using (var db = new ElectricEaseEntitiesContext())
                {
                    cliename = db.Client_Master.Where(x => x.Client_ID == clientid).Select(x => x.Client_Company).FirstOrDefault();
                }
            }
            else
            {
                cliename = "Admin";
            }
            var sv = System.Web.HttpContext.Current.Server.MapPath(filepath);
            if (cliename != null && cliename != "" && cliename.Contains('.'))
            {
                cliename = cliename.Replace('.',' ');
            }
            filepath =sv+"\\" + cliename;
            if (!Directory.Exists(filepath))
            {
                Directory.CreateDirectory(filepath);
            }
            string month = DateTime.Now.ToString("MMM-yyyy");
            filepath += "/" + month + ".txt";
            if (!File.Exists(filepath))
            {
                var newfile=File.Create(filepath);
                newfile.Close();
            }
            try
            {
                string currentContent = File.ReadAllText(filepath);
                string newContent = DateTime.Now + " (User: " + username + ") :- " + message;
                File.WriteAllText(filepath, newContent + Environment.NewLine + currentContent);
            }
            catch
            {
            }
        }
    }
}
