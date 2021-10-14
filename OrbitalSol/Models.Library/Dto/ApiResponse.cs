using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Models.Library.Dto
{
    public class ApiResponse
    {
        public ApiResponse(string error = null)
        {
            Message = error;
        }

        public string Message { get; set; } = null;
        public string Data { get; set; } = null;
    }

    public class ApiErrorResponse
    {
        [JsonConstructor]
        public ApiErrorResponse(
            List<string> errors = null,
            string description = "",
            object data = null,
            string correlationId = null)
        {
            Data = data;
            Error = errors;
            RequestIdentifier = correlationId;
            ErrorDescription = description;
        }

        public ApiErrorResponse(
            string error = null,
            string description = "",
            object data = null)
        {
            Error = new List<string> { error };
            ErrorDescription = description;
            Data = data;
        }

        public string RequestIdentifier { get; set; }
        public bool RequiresTranslation { get; set; } = true;
        public object Data { get; set; }
        public string ErrorDescription { get; set; }
        public List<string> Error { get; set; }
    }

    public static class ApiResponseExtensions
    {
        public static string ToJson(this ApiErrorResponse apiErrorResponse)
        {
            return JsonConvert.SerializeObject(apiErrorResponse);
        }

        public static string ToJson(this ApiResponse apiResponse)
        {
            return JsonConvert.SerializeObject(apiResponse);
        }
    }

    public enum ErrorTypes
    {
        ModelValidation,
        BadRequest,
        EmailExist,
        None
    }
}