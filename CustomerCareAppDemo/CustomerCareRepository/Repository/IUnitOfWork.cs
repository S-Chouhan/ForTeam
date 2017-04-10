using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerCareAppRepository.Repository
{
    public interface IUnitOfWork
    {
        GenericRepository<CustomerRequest> CustomerRepository { get; }
        GenericRepository<User> UserRepository { get; }
        GenericRepository<Token> TokenRepository { get; } 
        void Save();
    }
}
