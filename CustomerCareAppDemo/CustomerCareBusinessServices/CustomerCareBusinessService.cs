//using CustomerCareAppRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CustomerEntities;
using DataModel.UnitOfWork;
using DataModel;
//using CustomerCareAppRepository.Repository;


namespace CustomerCareBusinessServices
{
    public class CustomerCareBusinessService : ICustomerCareBusinessService
    {
        public IUnitOfWork _unitOfWork;
        public CustomerCareBusinessService(IUnitOfWork unitOfWork,CustomerDbContext context)
        {
            _unitOfWork = new UnitOfWork(context);
        }
        public IEnumerable<CustomerRequestEntity> GetAll()
        {
            var custReqList = _unitOfWork.CustomerRepository.GetAll();
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CustomerRequest, CustomerRequestEntity>();
                Mapper.AssertConfigurationIsValid();

               
            });
            IMapper mapper = config.CreateMapper();

            var custReqModel = mapper.Map<IEnumerable<CustomerRequest>, IEnumerable<CustomerRequestEntity>>(custReqList);
            return custReqModel;
        }

        public CustomerRequestEntity Get(int requestId)
        {
            var custReq = _unitOfWork.CustomerRepository.GetByID(requestId);
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CustomerRequest, CustomerRequestEntity>();
                Mapper.AssertConfigurationIsValid();
            });
            IMapper mapper = config.CreateMapper();
            var custReqModel = mapper.Map<CustomerRequest, CustomerRequestEntity>(custReq);
            return custReqModel;
        }

        public void Insert(CustomerRequestEntity req)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CustomerRequestEntity,CustomerRequest>();
                Mapper.AssertConfigurationIsValid();
            });
            IMapper mapper = config.CreateMapper();
            var custReqModel = mapper.Map<CustomerRequestEntity, CustomerRequest>(req);
            _unitOfWork.CustomerRepository.Insert(custReqModel);
            _unitOfWork.Save();
        }

        public void Update(CustomerRequestEntity req)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CustomerRequestEntity, CustomerRequest>();
                Mapper.AssertConfigurationIsValid();
            });
            IMapper mapper = config.CreateMapper();
            var custReqModel = mapper.Map<CustomerRequestEntity, CustomerRequest>(req);
            _unitOfWork.CustomerRepository.Update(custReqModel);
            _unitOfWork.Save();
        }

        public void Delete(int requestId)
        {
            _unitOfWork.CustomerRepository.Delete(requestId);
            _unitOfWork.Save();
        }
    }
}
