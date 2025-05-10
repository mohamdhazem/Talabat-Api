using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Talabat.APIs.HandlingErrors;
using Talabat.Repository.Data;

namespace Talabat.APIs.Controllers
{
    public class BugyyController : BaseApiController
    {
        private readonly StoreContext _dbContext;

        public BugyyController(StoreContext dbContext)
        {
            _dbContext = dbContext;
        }


        // We have Type of Error Handling

        // 1- 404 Not Found
        [HttpGet("NotFound")]
        public ActionResult GetNotFound()
        {
            var Product = _dbContext.Products.Find(42);
            if(Product == null) return NotFound(new ApiResponse(404));  

            return Ok(Product);
        }

        //2- 400 Bad Request
        [HttpGet("BadRequest")]
        public ActionResult GetBadRequest()
        {
            return BadRequest(new ApiResponse(400));
        }

        //3- 500 Internal Server Error
        [HttpGet("ServerError")]
        public ActionResult GetServerError()
        {
            var Product = _dbContext.Products.Find(42);
            var ProductToReturn = Product.ToString(); // this will throw exception [NullReferenceException]
            return Ok(ProductToReturn);
        }

        //4- validation Error
        [HttpGet("ValidationError/{id}")]
        public ActionResult GetValidationError(int id)
        {
            // the user enter string instead of int
            return Ok();
        }

        //5- Endpoint Not Found

        //6- Unauthorized
        [HttpGet("Unauthorized")]
        public ActionResult GetUnauthorized()
        {
            return Unauthorized(new ApiResponse(401));
        }
    }
}
