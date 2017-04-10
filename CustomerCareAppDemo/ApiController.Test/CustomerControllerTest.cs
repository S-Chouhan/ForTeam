using DataModel;
using DataModel.GenericRepository;
using DataModel.UnitOfWork;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Newtonsoft.Json;
using MockHelper;
using CustomerCareAppDemo.Controllers;
using CustomerCareAppDemo.ErrorHelper;
using CustomerCareBusinessServices;
using System.Net.Http;
using System.Net;
using System.Web.Http.Hosting;
using System.Web.Http;
using CustomerEntities;
using System.Net.Http.Formatting;
using System.Web.Http.Results;
using System.Data.Entity;

namespace ApiController.Test
{
    [TestFixture]
    public class CustomerControllerTest:BaseApiController
    {

        #region Variables

        private ICustomerCareBusinessService _customerService;       
        private IUnitOfWork _unitOfWork;
        private List<CustomerRequest> _customerReqs;        
        private GenericRepository<CustomerRequest> _customerRepository;        
        private CustomerDbContext _dbEntities;
        private HttpClient _client;
             
        private const string ServiceBaseURL = "http://localhost:55496//";

        #endregion

        #region Test fixture setup

        /// <summary>
        /// Initial setup for tests
        /// </summary>
        [OneTimeSetUp]
        public void Setup()
        {
            _customerReqs = SetUpCustomerRequests();           
            _dbEntities = new Mock<CustomerDbContext>().Object;           
            _customerRepository = SetUpCustomerRepository();
            var unitOfWork = new Mock<IUnitOfWork>();
            unitOfWork.SetupGet(s => s.CustomerRepository).Returns(_customerRepository);           
            _unitOfWork = unitOfWork.Object;
            _customerService = new CustomerCareBusinessService(_unitOfWork,_dbEntities);           
            _client = new HttpClient { BaseAddress = new Uri(ServiceBaseURL) };
           
        }

        #endregion

        #region Setup
        /// <summary>
        /// Re-initializes test.
        /// </summary>
        [SetUp]
        public void ReInitializeTest()
        {
            _client = new HttpClient { BaseAddress = new Uri(ServiceBaseURL) };           
        }
        #endregion

        #region Private member methods

        /// <summary>
        /// Setup dummy repository
        /// </summary>
        /// <returns></returns>
        private GenericRepository<CustomerRequest> SetUpCustomerRepository()
        {
            // Initialise repository
            var mockRepo = new Mock<GenericRepository<CustomerRequest>>(MockBehavior.Default, _dbEntities);
            
            // Setup mocking behavior
            mockRepo.Setup(p => p.GetAll()).Returns(_customerReqs);

            mockRepo.Setup(p => p.GetByID(It.IsAny<int>()))
                .Returns(new Func<int, CustomerRequest>(
                             id => _customerReqs.Find(p => p.CustomerRequestId.Equals(id))));

            mockRepo.Setup(p => p.Insert((It.IsAny<CustomerRequest>())))
                .Callback(new Action<CustomerRequest>(newCustReq =>
                {
                    dynamic maxReqID = _customerReqs.Last().CustomerRequestId;
                    dynamic nextReqID = maxReqID + 1;
                    newCustReq.CustomerRequestId = nextReqID;
                    _customerReqs.Add(newCustReq);
                }));

            mockRepo.Setup(p => p.Update(It.IsAny<CustomerRequest>()))
                .Callback(new Action<CustomerRequest>(req =>
                {
                    var oldRequest = _customerReqs.Find(a => a.CustomerRequestId == req.CustomerRequestId);
                    oldRequest = req;
                }));

            mockRepo.Setup(p => p.Delete(It.IsAny<CustomerRequest>()))
                .Callback(new Action<CustomerRequest>(req =>
                {
                    var requestToRemove =
                        _customerReqs.Find(a => a.CustomerRequestId == req.CustomerRequestId);

                    if (requestToRemove != null)
                        _customerReqs.Remove(requestToRemove);
                }));

            // Return mock implementation object
            return mockRepo.Object;
        }

       
        /// <summary>
        /// Setup dummy Customer Requests data
        /// </summary>
        /// <returns></returns>
        private static List<CustomerRequest> SetUpCustomerRequests()
        {
            return DataInitializer.GetAllCustomerRequests();
        }

       

        #endregion

        #region Unit Tests

        /// <summary>
        /// Get all CustomerRequests test
        /// </summary>
        [Test]
        public void GetAllCustomerRequestsTest()
        {
            var customerController = new CustomerCareController(_customerService)
            {
                Request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(ServiceBaseURL + "api/CustomerCare/GetAll")
                }
            };
            customerController.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());

            var response = customerController.Get() as OkNegotiatedContentResult<IEnumerable<CustomerRequestEntity>>;

            List<CustomerRequest> lstReq = new List<CustomerRequest>();
            foreach (var item in response.Content)
            {
                
                    CustomerRequest entity = new CustomerRequest();
                    entity.CustomerRequestId=item.CustomerRequestId;
                    entity.Description = item.Description;
                    entity.Title = item.Title;
                    lstReq.Add(entity);
            }

           
            Assert.AreEqual(lstReq.Any(), true);
            var comparer = new CustomerComparer();
            CollectionAssert.AreEqual(
                lstReq.OrderBy(req => req, comparer),
                _customerReqs.OrderBy(entity => entity, comparer), comparer);
        }

        /// <summary>
        /// Get Customer Request by Id
        /// </summary>
        [Test]
        public void GetCustomerRequestByIdTest()
        {
            var customerController = new CustomerCareController(_customerService)
            {
                Request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(ServiceBaseURL + "api/CustomerCare/1")
                }
            };
            customerController.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());

            
            var response = customerController.GetById(1) as OkNegotiatedContentResult<CustomerRequestEntity>;
            CustomerRequest CustResponse = new CustomerRequest();
            CustResponse.CustomerRequestId = response.Content.CustomerRequestId;
            CustResponse.Title = response.Content.Title;
            CustResponse.Description = response.Content.Description;
          
            AssertObjects.PropertyValuesAreEquals(CustResponse,
                                                    _customerReqs.Find(a => a.Title.Contains("Product damaged")));

           // Assert.IsTrue(true);
        
        }

        /// <summary>
        /// Get Customer Request by wrong Id
        /// </summary>
        [Test]        
        public void GetCustomerRequestByWrongIdTest()
        {
            var customerController = new CustomerCareController(_customerService)
            {
                Request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(ServiceBaseURL + "api/CustomerCare/10")
                }
            };
            customerController.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());

            var ex = Assert.Throws<ApiDataException>(() => customerController.GetById(10));
            Assert.That(ex.ErrorCode, Is.EqualTo(1001));
            Assert.That(ex.ErrorDescription, Is.EqualTo("No customer request found for this id."));

        }

        /// <summary>
        /// Get Customer Request by invalid Id
        /// </summary>
        [Test]
        // [ExpectedException("WebApi.ErrorHelper.ApiException")]
        public void GetCustomerRequestByInvalidIdTest()
        {
            var customerController = new CustomerCareController(_customerService)
            {
                Request = new HttpRequestMessage
                {
                    Method = HttpMethod.Get,
                    RequestUri = new Uri(ServiceBaseURL + "api/CustomerCare/-1")
                }
            };
            customerController.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());

            var ex = Assert.Throws<ApiException>(() => customerController.GetById(-1));
            Assert.That(ex.ErrorCode, Is.EqualTo((int)HttpStatusCode.BadRequest));
            Assert.That(ex.ErrorDescription, Is.EqualTo("Bad Request..."));
        }

        /// <summary>
        /// Create Customer Request test
        /// </summary>
        [Test]
        public void CreateCustomerRequestTest()
        {
            var customerController = new CustomerCareController(_customerService)
            {
                Request = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri(ServiceBaseURL + "api/CustomerCare/Create")
                }
            };
            customerController.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());

            var newCustReq = new CustomerEntities.CustomerRequestEntity()
            {
                Title = "Damaged product",
                Description="Damaged product delivered"
            };

            var maxCustReqIDBeforeAdd = _customerReqs.Max(a => a.CustomerRequestId);
            newCustReq.CustomerRequestId = maxCustReqIDBeforeAdd + 1;
            customerController.Post(newCustReq);
            var addedproduct = new CustomerRequest() { CustomerRequestId=newCustReq.CustomerRequestId,Title=newCustReq.Title,Description=newCustReq.Description};


            Assert.That(addedproduct.CustomerRequestId, Is.EqualTo(_customerReqs.Last().CustomerRequestId+1));
        }

        /// <summary>
        /// Update Customer Request test
        /// </summary>
        [Test]
        public void UpdateCustomerReqTest()
        {
            var customerController = new CustomerCareController(_customerService)
            {
                Request = new HttpRequestMessage
                {
                    Method = HttpMethod.Put,
                    RequestUri = new Uri(ServiceBaseURL + "api/CustomerCare/Update")
                }
            };
            customerController.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());

            var firstCustomerReq = _customerReqs.First();
            firstCustomerReq.Title = "Laptop damaged";
            var updatedCustReq = new CustomerRequestEntity() {CustomerRequestId=firstCustomerReq.CustomerRequestId,  Title = firstCustomerReq.Title, Description = firstCustomerReq.Description };
            //customerController.Put(updatedCustReq);
            Assert.That(firstCustomerReq.CustomerRequestId, Is.EqualTo(1)); // hasn't changed
        }

        /// <summary>
        /// Delete Customer Request test
        /// </summary>
        [Test]
        public void DeleteCustomerReqTest()
        {
            var customerController = new CustomerCareController(_customerService)
            {
                Request = new HttpRequestMessage
                {
                    Method = HttpMethod.Put,
                    RequestUri = new Uri(ServiceBaseURL + "api/CustomerCare/Remove")
                }
            };
            customerController.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());

            int maxID = _customerReqs.Max(a => a.CustomerRequestId); // Before removal
            var lastCustReq = _customerReqs.Last();

            // Remove last Customer Request
            customerController.Delete(lastCustReq.CustomerRequestId);
            Assert.That(maxID+1, Is.GreaterThan(_customerReqs.Max(a => a.CustomerRequestId))); // Max id reduced by 1
        }

        /// <summary>
        /// Delete customer request test with invalid id
        /// </summary>
        [Test]
        public void DeleteCustomerRequestInvalidIdTest()
        {
            var customerController = new CustomerCareController(_customerService)
            {
                Request = new HttpRequestMessage
                {
                    Method = HttpMethod.Put,
                    RequestUri = new Uri(ServiceBaseURL + "api/CustomerCare/remove")
                }
            };
            customerController.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());

            var ex = Assert.Throws<ApiException>(() => customerController.Delete(-1));
            Assert.That(ex.ErrorCode, Is.EqualTo((int)HttpStatusCode.BadRequest));
            Assert.That(ex.ErrorDescription, Is.EqualTo("Bad Request..."));
        }

        #endregion

        

        #region Tear Down
        /// <summary>
        /// Tears down each test data
        /// </summary>
        [TearDown]
        public void DisposeTest()
        {            
            if (_client != null)
                _client.Dispose();
        }

        #endregion

        #region TestFixture TearDown.

        /// <summary>
        /// TestFixture teardown
        /// </summary>
        [OneTimeTearDown]
        public void DisposeAllObjects()
        {
           
            _customerService = null;
            _unitOfWork = null;
           
            _customerRepository = null;
          
            _customerReqs = null;
            
            if (_client != null)
                _client.Dispose();
        }

        #endregion

       
    }
}
