using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Web_153501_Kiselev.Domain.Models
{
    public class BaseResponse<T>
    {
        public BaseResponse() { }
        public BaseResponse(bool success, T? data) => (Success, Data) = (success, data);
        public BaseResponse(bool success, string message) => (Success, Message) = (success, message);
        public T? Data { get; set; }
        public bool Success { get; set; }
        public string? Message { get; set; }
    }
}
