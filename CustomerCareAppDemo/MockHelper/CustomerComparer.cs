using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataModel;

namespace MockHelper
{
    public class CustomerComparer : IComparer, IComparer<CustomerRequest>
    {
        public int Compare(object expected, object actual)
        {
            var lhs = expected as CustomerRequest;
            var rhs = actual as CustomerRequest;
            if (lhs == null || rhs == null) throw new InvalidOperationException();
            return Compare(lhs, rhs);
        }

        public int Compare(CustomerRequest expected, CustomerRequest actual)
        {
            int temp;
            return (temp = expected.CustomerRequestId.CompareTo(actual.CustomerRequestId)) != 0 ? temp : expected.Title.CompareTo(actual.Title);
        }
    }
}
