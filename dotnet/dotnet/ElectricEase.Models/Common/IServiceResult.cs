using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectricEase.Models
{
    interface IServiceResult
    {
        int ResultCode { get; set; }
        string Message { get; set; }
    }
}
