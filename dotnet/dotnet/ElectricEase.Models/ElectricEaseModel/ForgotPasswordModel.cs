using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectricEase.Models
{
   public  class ForgotPasswordModel
    {
       public int Client_ID { get; set; }
       public string EMAIL { get; set; }

       public string PASSWORDTOKEN { get; set; }

       public string NAME { get; set; }
       public string User_ID { get; set; }

       public string PASWORD { get; set; }

       public string CONFIRM_PASSWORD { get; set; }
    }
}
