using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataModel;

namespace MockHelper
{
    /// <summary>
    /// Data initializer for unit tests
    /// </summary>
    public class DataInitializer
    {
        /// <summary>
        /// Dummy products
        /// </summary>
        /// <returns></returns>
        public static List<CustomerRequest> GetAllCustomerRequests()
        {
            var products = new List<CustomerRequest>
                               {
                                   new CustomerRequest() {CustomerRequestId = 1,Title="Product damaged",Description="Product damaged while delivery"},
                                   new CustomerRequest() {CustomerRequestId = 2,Title="Not delivered",Description="Product not delivered"},
                                   new CustomerRequest() {CustomerRequestId = 13,Title="Damaged product",Description="Damaged product delivered"},
                                   
                               };
            return products;
        }

        /// <summary>
        /// Dummy tokens
        /// </summary>
        /// <returns></returns>
        public static List<Token> GetAllTokens()
        {
            var tokens = new List<Token>
                               {
                                   new Token()
                                       {
                                           AuthToken = "16c20714-948d-4c13-97c2-2d1f13972dee",
                                           ExpiresOn = DateTime.Now.AddHours(2),
                                           IssuedOn = DateTime.Now,
                                           UserId = 1
                                       },
                                   new Token()
                                       {
                                           AuthToken = "a3409135-d2ec-462d-9ba0-dbbb312253e9",
                                           ExpiresOn = DateTime.Now.AddHours(1),
                                           IssuedOn = DateTime.Now,
                                           UserId = 2
                                       }
                               };

            return tokens;
        }

        /// <summary>
        /// Dummy users
        /// </summary>
        /// <returns></returns>
        public static List<User> GetAllUsers()
        {
            var users = new List<User>
                               {
                                   new User()
                                       {
                                           UserName = "user1",
                                           Password = "password1",
                                           Name = "prasad",
                                       },
                                   new User()
                                       {
                                           UserName = "user2",
                                           Password = "password2",
                                           Name = "siva",
                                       }
                                   
                               };

            return users;
        }

    }
}
