using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerEntities
{   
   
    public class CustomerRequestEntity
    {           
        public int CustomerRequestId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
    }
}
