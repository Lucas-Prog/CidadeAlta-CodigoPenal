using CidadeAlta_CodigoPenal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CidadeAlta_CodigoPenal.Wrappers
{
    public class Response<T>
    {
        public List<ResponseCriminalCode> Data { get; set; }
        public bool Succeeded { get; set; }
        public string[] Errors { get; set; }
        public string Message { get; set; }
        public Response()
        {

        }
        public Response(List<ResponseCriminalCode> data)
        {
            Succeeded = true;
            Message = string.Empty;
            Errors = null;
            Data = data;
        }
    }
}
