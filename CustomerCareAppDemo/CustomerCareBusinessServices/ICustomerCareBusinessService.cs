using CustomerEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerCareBusinessServices
{
    public interface ICustomerCareBusinessService
    {
        IEnumerable<CustomerRequestEntity> GetAll();
        CustomerRequestEntity Get(int requestId);
        void Insert(CustomerRequestEntity req);
        void Update(CustomerRequestEntity req);
        void Delete(int requestId);
    }
}
