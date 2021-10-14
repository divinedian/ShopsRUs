using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopsRUs.Data.Models
{
    public class BaseResponse<T>
    {
        public string Message { get; set; } = string.Empty;
        public T Data { get; set; }

        public BaseResponse()
        {
        }

        public BaseResponse( string message)
        {
            Message = message;
        }

        public BaseResponse(string message, T data)
        {
            Message = message;
            Data = data;
        }
    }

    public class BaseResponse
    {
        public string Status { get; set; }
        public string StatusDescription { get; set; }
        public string Message { get; set; }
        public List<string> Errors { get; set; } = new List<string>();

        public BaseResponse()
        {
        }

        public BaseResponse(string status, string message)
        {
            Status = status;
            Message = message;
        }

        public BaseResponse(string status, string statusDescription, string message)
        {
            Status = status;
            StatusDescription = statusDescription;
            Message = message;
        }

        public BaseResponse(string status, string statusDescription, string message, List<string> errors)
        {
            Status = status;
            StatusDescription = statusDescription;
            Message = message;
            Errors = errors;
        }
    }

    public enum Status
    {
        Success = 1,
        Failed = 2
    }
}
