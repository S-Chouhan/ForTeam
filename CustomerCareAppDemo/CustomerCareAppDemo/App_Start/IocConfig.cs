using CustomerCareAppDemo.Resolver;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using CustomerCareBusinessServices;
using DataModel.UnitOfWork;
using DataModel;
using System.Data.Entity;

namespace CustomerCareAppDemo.App_Start
{
    public class IocConfig
    {
        public static void Register(HttpConfiguration config)
        {
            var container = new UnityContainer();
            container.RegisterType<ICustomerCareBusinessService, CustomerCareBusinessService>(new HierarchicalLifetimeManager());
            container.RegisterType<IUserServices, UserServices>(new HierarchicalLifetimeManager());
            container.RegisterType<ITokenServices, TokenServices>(new HierarchicalLifetimeManager());
            container.RegisterType<IUnitOfWork, UnitOfWork>(new HierarchicalLifetimeManager());
            container.RegisterType<CustomerDbContext>(new HierarchicalLifetimeManager());
            

            config.DependencyResolver = new UnityResolver(container);
        }


    }
}