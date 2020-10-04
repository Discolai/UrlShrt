using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UrlShrt.Dtos
{
    public class CommonResponse
    {
        public int Status { get; set; } = 200;
        public object Data { get; set; } = null;
        public IDictionary<string, string[]> Errors { get; set; } = null;

        public CommonResponse() { }
        public CommonResponse(ValidationProblemDetails problemDetails)
        {
            Errors = problemDetails.Errors;
        }
        public CommonResponse(ModelStateDictionary modelState) : this(new ValidationProblemDetails(modelState)) { }

        //public static ObjectResult CreateResponse(object data = null, IDictionary<string, string[]> errors = null, int status = 200)
        //{
        //    return new ObjectResult(new CommonResponse() { Data = data, Errors = errors, Status = status }) { StatusCode = status };
        //}

        public static ObjectResult CreateResponse(object data = null, ModelStateDictionary modelState = null, int status = 200)
        {
            var commonResponse = modelState == null ? new CommonResponse { Data= data, Status = status} : new CommonResponse(modelState) { Data = data, Status = status };
            return new ObjectResult(commonResponse) { StatusCode = status };
        }

    }
}
