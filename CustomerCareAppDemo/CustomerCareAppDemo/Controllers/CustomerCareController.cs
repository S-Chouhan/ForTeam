using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;
using CustomerCareAppDemo.BaseCode;
using System.Collections;
using CustomerEntities;
using CustomerCareBusinessServices;
using CustomerCareAppDemo.ActionFilter;
using AttributeRouting.Web.Http;
using CustomerCareAppDemo.ErrorHelper;
using Swashbuckle.Swagger.Annotations;
using System.Web.Http.Description;


namespace CustomerCareAppDemo.Controllers
{
   // [AuthorizationRequired]
    [CustomExceptionFilter]
    public class CustomerCareController : BaseApiController
    {
        public ICustomerCareBusinessService _service;
       
        public CustomerCareController(ICustomerCareBusinessService service)
        {
            _service = service;
        }

        //[CustomActionFilter]
        [HttpGet]
        [Route("GetAll")]
        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        [ResponseType(typeof(IEnumerable<CustomerRequestEntity>))]
        public IHttpActionResult Get()
        {
            var result = _service.GetAll();
            if (result.Any())
                 return Ok(result);
            throw new ApiDataException(1000, "Customer requests not found", HttpStatusCode.NotFound);
        }

        
        [HttpGet]
        [Route("GetById")]
        [SwaggerResponse(HttpStatusCode.OK)]
        [SwaggerResponse(HttpStatusCode.NotFound)]
        [SwaggerResponse(HttpStatusCode.BadRequest)]         
        [ResponseType(typeof(CustomerRequestEntity))]
        public IHttpActionResult GetById([FromUri] int id)
        {
            if (id > 0)
            {
                var result = _service.Get(id);
                if (result != null)
                    return Ok<CustomerRequestEntity>(result);
                throw new ApiDataException(1001, "No customer request found for this id.", HttpStatusCode.NotFound);
            }               
            throw new ApiException() { ErrorCode = (int)HttpStatusCode.BadRequest, ErrorDescription = "Bad Request..." };

        }

        [HttpPost]
        [Route("Create")]
        [SwaggerResponse(HttpStatusCode.Created)]        
        [SwaggerResponse(HttpStatusCode.BadRequest)]
        [ResponseType(typeof(CustomerRequestEntity))]
        public IHttpActionResult Post([FromBody] CustomerRequestEntity customerReq)
        {
            if (customerReq.Description == null)
                 return BadRequest("Invalid customer request");
            _service.Insert(customerReq);
            
            return Created<CustomerRequestEntity>(new Uri(Request.RequestUri.ToString()), customerReq);

           
        }

        [HttpPut]
        [Route("Update")]
        [SwaggerResponse(HttpStatusCode.OK)]        
        [SwaggerResponse(HttpStatusCode.BadRequest)]
        [ResponseType(typeof(CustomerRequestEntity))]
        public IHttpActionResult Put(CustomerRequestEntity customerReq)
        {
            if (customerReq == null)
                return BadRequest("Invalid customer request");
             _service.Update(customerReq);
            
            return Ok();
        }

        [HttpDelete]
        [Route("Remove")]
        [SwaggerResponse(HttpStatusCode.OK)]        
        [SwaggerResponse(HttpStatusCode.BadRequest)]
        public IHttpActionResult Delete(int id)
        {
            if (id > 0)
            {
                _service.Delete(id);

                return Ok();
            }
            throw new ApiException() { ErrorCode = (int)HttpStatusCode.BadRequest, ErrorDescription = "Bad Request..." };
           
        }



        
    }
}
