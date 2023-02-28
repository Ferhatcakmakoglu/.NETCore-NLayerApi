using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NLayer.Core.DTOs;

namespace NLayer.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomBaseController : ControllerBase
    {
        /*
            Burada her seferinde status code olarak
            OK, NoContent gibi seyler donmek yerine bir base
            classtan kontrol etmek icin bu kodu yazdık.
            Gelen respons a gore kod donecek
         */
        [NonAction] // Get veya Post olmadıgını belirttik yoksa hata!
        public IActionResult CreateActionResult<T>(CustomResponseDto<T> response)
        {
            if (response.StatusCode == 204)
                return new ObjectResult(null)
                {
                    StatusCode = response.StatusCode
                };

            return new ObjectResult(response)
            {
                StatusCode = response.StatusCode
            };
        }
           
    }
}
