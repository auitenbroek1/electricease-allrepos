using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectricEase.Models
{
    public class ServiceResultList<T> : Result
    {
        /// <summary>
        /// User might be assign this value
        /// </summary>
        
        public int TotalItemCount { get; set; }

        /// <summary>
        /// Output as LIST<object> 
        /// </summary>
       
        public IList<T> ListData { get; set; }
    }
}
