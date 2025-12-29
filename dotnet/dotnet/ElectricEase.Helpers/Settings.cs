using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectricEase.Helpers
{
    public static class Settings
    {
        public static string SenderEmailAddress
        {


            //get { return "arun.l@visentechnosoft.com"; }
            // get;set;

            get { return "electricease.noreply@gmail.com"; }


        }

        public static string SenderEmailPassword
        {
            get { return "visen123"; }


        }



        /// <summary>
        /// Get the Website Url 
        /// </summary>
        /// <returns></returns>
        public static string DomainUrl
        {
            get { return "http://" + DomainName; }

        }

        /// <summary>
        /// Get the DomainName 
        /// </summary>
        /// <returns></returns>
        public static string DomainName
        {
            get
            {

                return "www.electric-ease.com";
            }

            //get;
            //set;
        }


        /// <summary>
        /// Get the company name
        /// </summary>
        /// <returns></returns>
        public static string CompanyName
        {
            get { return "Electric Ease"; }

            //get;
            //set;
        }
        public static string SmtpHost
        {
            //get { return "Visen TechnoSoft Pvt Ltd"; }
            get;
            set;
        }
        public static int? SmtpPort
        {
            // get { return "Visen TechnoSoft Pvt Ltd"; }
            get;
            set;
        }
        public static string copyrightText
        {
            get { return "© " + DateTime.Now.Year + " " + CompanyName + " - All rights reserved."; }
        }

    }
}
