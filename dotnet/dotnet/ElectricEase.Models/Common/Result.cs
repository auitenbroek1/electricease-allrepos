using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectricEase.Models
{
    public abstract class Result
    {
        public Result()
        {
        }

        /// <summary>
        /// Reponse message
        /// </summary>
       
        public string Message { get; set; }

        /// <summary>
        /// Success = 1, Failed = 0, Unknown Error = -1
        /// </summary>
      
        public int ResultCode { get; set; }

        // [DataMember]
        //public int MEMBER_ID { get; set; }

    }
}
