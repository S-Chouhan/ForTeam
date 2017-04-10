using CustomerCareAppDemo.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;

namespace CustomerCareAppDemo.BaseCode
{
    public class NoContentResult : BaseApiController,IHttpActionResult
    {
        private readonly HttpRequestMessage _request;
        private readonly string _reason;

        public NoContentResult(HttpRequestMessage request, string reason)
        {
            _request = request;
            _reason = reason;
        }

        public NoContentResult(HttpRequestMessage request)
        {
            _request = request;
            _reason = "No Content";
        }

        public Task<HttpResponseMessage> ExecuteAsync(CancellationToken cancellationToken)
        {
            var response = _request.CreateResponse(HttpStatusCode.NoContent, _reason);
            return Task.FromResult(response);
        }

    }
}