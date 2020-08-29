using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace UrlShrt.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)]
    public class ErrorsController : ControllerBase
    {
        private readonly ILogger<ErrorsController> _logger;

        public ErrorsController(ILogger<ErrorsController> logger)
        {
            _logger = logger;
        }


        [Route("error")]
        public ErrorResponse Error()
        {
            var context = HttpContext.Features.Get<IExceptionHandlerFeature>();
            var exception = context?.Error; // Your exception

            _logger.LogError(exception, "An unexpected exception occured at {time} UTC", DateTime.UtcNow);

            Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            return new ErrorResponse { Status = Response.StatusCode, Title = "Internal server error" };
        }
    }

    public class ErrorResponse
    {
        public int Status { get; set; }
        public string Title { get; set; }
    }
}
