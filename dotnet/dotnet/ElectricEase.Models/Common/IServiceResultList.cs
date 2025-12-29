using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectricEase.Models.Common
{
    interface IServiceResultList
    {
        int ResultCode { get; set; }
        string Message { get; set; }
        int MEMBER_ID { get; set; }
    }
}
