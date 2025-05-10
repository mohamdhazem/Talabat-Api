using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Talabat.APIs.DTOs;
using Talabat.APIs.HandlingErrors;
using Talabat.APIs.Helpers;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Contract;
using Talabat.Core.Specifications;
using Talabat.Core.Specifications.ProductSpecs;

namespace Talabat.APIs.Controllers
{
    public class ProductsController : BaseApiController
    { 
        private readonly IGenericRepository<Product> _productRepository;
        private readonly IGenericRepository<ProductBrand> _brandRepository;
        private readonly IGenericRepository<ProductCategory> _categoryRepository;
        private readonly IMapper _mapper;

        public ProductsController(IGenericRepository<Product> productRepository ,
            IGenericRepository<ProductBrand> BrandRepository,
            IGenericRepository<ProductCategory> CategoryRepository, 
            IMapper mapper)
        {
            _productRepository = productRepository;
            _brandRepository = BrandRepository;
            _categoryRepository = CategoryRepository;
            _mapper = mapper;
        }

        //[Authorize] // Bearer Schema
        [CacheAttribute(1000)]
        [HttpGet] // GetAll Products
        //[ProducesResponseType(typeof(ProductToReturnDTO), StatusCodes.Status200OK)]        // to Enhance the Swagger Documentation
        //[ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]        // to Enhance the Swagger Documentation
        public async Task<ActionResult<IEnumerable<ProductToReturnDTO>>> GetProducts([FromQuery] ProductSpecParams productSpecParams)/*string? sort, int? brandId, int? categoryId)*/
        {
            var specifications = new ProductWithCategoryAndBrandSpecifications(productSpecParams);

            var products = await _productRepository.GetAllWithSpecAsync(specifications);

            var data = _mapper.Map<IEnumerable<Product>, IEnumerable<ProductToReturnDTO>>(products);

            var Countspecifications = new ProductWithCountSpecifications(productSpecParams);

            var count = await _productRepository.GetCountWitSpecAsync(Countspecifications);

            return Ok(new Pagination<ProductToReturnDTO>(productSpecParams.pageSize, productSpecParams.pageIndex, count, data));
        }

        // Get By id 
        [HttpGet("{Id:int}")]
        [ProducesResponseType(typeof(ProductToReturnDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiValidationErrorResponse), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<ProductToReturnDTO>> GetProduct(int Id)
        {
            var specifications = new ProductWithCategoryAndBrandSpecifications(Id);

            var Product =  await _productRepository.GetWithSpecAsync(specifications);

            if(Product is null)
                return NotFound(new ApiResponse(404));

            return Ok(_mapper.Map<Product,ProductToReturnDTO>(Product));
        }

        //[Authorize]
        [HttpGet("GetBrands")]
        public async Task<ActionResult<IReadOnlyList<ProductBrand>>> GetBrands()
        {
            var result = await _brandRepository.GetAllAsync();
            if(result is null)
                return NotFound(new ApiResponse(StatusCodes.Status404NotFound));

            return result.ToList();
        }

        [Authorize]
        [HttpGet("GetCategories")]
        public async Task<ActionResult<IReadOnlyList<ProductCategory>>> GetCategories()
        {
            var result = await _categoryRepository.GetAllAsync();
            if (result is null)
                return NotFound(new ApiResponse(StatusCodes.Status404NotFound));

            return result.ToList();
        }
    }
}
