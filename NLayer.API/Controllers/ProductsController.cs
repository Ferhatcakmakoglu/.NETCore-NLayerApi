using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NLayer.Core.DTOs;
using NLayer.Core.Models;
using NLayer.Core.Services;

namespace NLayer.API.Controllers
{
    public class ProductsController : CustomBaseController
    {
        private readonly IMapper _mapper;
        private readonly IProductService _service;

        public ProductsController(IMapper mapper, IService<Product> service, IProductService productService)
        {
            _mapper = mapper;
            _service = productService;
        }


        //Birden fazla GET oldugu için cakısmaması icin boyle yaptık. Anlamı;
        //GET --> www.mysite.com/api/products/GetProductsWithCategory
        [HttpGet("[action]")] // ANLAMI --> [HttpGet("GetProductsWithCategory")]
        public async Task<IActionResult> GetProductsWithCategory() 
        {
            //var res = await _productService.GetProductsWithCategory();
            //aynı
            return CreateActionResult(await _service.GetProductsWithCategory());
        }

        //GET --> www.mysite.com/api/products => tum productlar
        [HttpGet]
        public async Task<IActionResult> All() 
        {
            var products = await _service.GetAllAsync();
            var productsDto = _mapper.Map<List<ProductDto>>(products.ToList());
            return CreateActionResult(CustomResponseDto<List<ProductDto>>.Succes(200, productsDto));
            //Her seferinde Ok methodunu donmemek icin CustomBaseControllerden Ok methodu
            //otomatik olarak cekilcek
        }

        //GET --> www.mysite.com/api/products/5 => id si 5 olan urun
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var product = await _service.GetByIdAsync(id);
            var productDto = _mapper.Map<ProductDto>(product);
            return CreateActionResult(CustomResponseDto<ProductDto>.Succes(200, productDto));
        }

        [HttpPost]
        public async Task<IActionResult> Save(ProductDto productDto)
        {
            var product = await _service.AddAsync(_mapper.Map<Product>(productDto));
            var productsDto = _mapper.Map<ProductDto>(product);
            return CreateActionResult(CustomResponseDto<ProductDto>.Succes(201, productDto));
        }

        [HttpPut]
        public async Task<IActionResult> Update(ProductUpdateDto productDto)
        {
            await _service.UpdateAsync(_mapper.Map<Product>(productDto));
            return CreateActionResult(CustomResponseDto<NoContentDto>.Succes(204));
        }

        //DELETE --> www.mysite.com/api/products/5 => id si 5 olan silincek
        [HttpDelete("{id}")]
        public async Task<IActionResult> Remove(int id)
        {
            var product = await _service.GetByIdAsync(id);
            await _service.RemoveAsync(product);
            return CreateActionResult(CustomResponseDto<NoContentDto>.Succes(204));
        }
    }
}
