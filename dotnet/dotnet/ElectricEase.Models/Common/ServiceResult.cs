using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectricEase.Models
{
    public class ServiceResult<T> : Result
    {
        
        public T Data { get; set; }
    }
}
