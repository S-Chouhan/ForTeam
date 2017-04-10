using DataModel.GenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataModel.UnitOfWork
{
    public interface IUnitOfWork
    {
        #region Properties
        GenericRepository<CustomerRequest> CustomerRepository { get; }
        GenericRepository<User> UserRepository { get; }
        GenericRepository<Token> TokenRepository { get; }
        #endregion

        #region Public methods
        /// <summary>
        /// Save method.
        /// </summary>
        void Save();
        #endregion
    }
}
