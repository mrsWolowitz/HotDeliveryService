using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace HotDeliveryModel
{
    public class ResponseDTO
    {
        public ResponseType Type { get; set; }
        public string Message { get; set; }

        public ResponseDTO(ResponseType responseType, string message = "")
        {
            Type = responseType;
            Message = message;
        }
        
    }

    public enum ResponseType
    {
        Ok = 200,
        NotFound = 404,
        NotAvailable = 422
    }
}
