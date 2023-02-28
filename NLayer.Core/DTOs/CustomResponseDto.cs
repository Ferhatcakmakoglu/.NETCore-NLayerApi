using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace NLayer.Core.DTOs
{
    public class CustomResponseDto<T>
    {
        public T Data { get; set; }
        
        [JsonIgnore] //Status Codun Clientlere donmemesi icin
        public int StatusCode { get; set; }

        public List<String> Errors { get; set; }

        public static CustomResponseDto<T> Succes(int statusCode, T data)
        {
            return new CustomResponseDto<T> {Data=data,StatusCode = statusCode}; //Errors = null (default)
        }

        //Update islemleri gibi olayda sadece statusCode u dondurmek icin
        public static CustomResponseDto<T> Succes(int statusCode)
        {
            return new CustomResponseDto<T> {StatusCode = statusCode };
        } 

        //Birden cok hata icin
        public static CustomResponseDto<T> Fail(int statusCode, List<String> errors)
        {
            return new CustomResponseDto<T> {StatusCode=statusCode, Errors = errors  };
        }

        //Tek bir hata icin
        public static CustomResponseDto<T> Fail(int statusCode, string errors)
        {
            return new CustomResponseDto<T> { StatusCode = statusCode, Errors = new List<string> { errors} };
        }

        
    }
}
