using FluentValidation;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Product.Data;
using Product.Dtos;
using Product.Model;
using System.ComponentModel.DataAnnotations;

namespace Product.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly ILogger<ProductsController> logger;

        public ProductsController(ApplicationDbContext context,ILogger<ProductsController> logger)
        {
            this.context = context;
            this.logger = logger;
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAllAsync()
        {
            var prod = await context.products.ToListAsync();
            var response = prod.Adapt<IEnumerable<GetAllProductDtos>>();
            return Ok(response);
        }

        [HttpGet("Details") ]
        public async Task<IActionResult> GetAsync(int id)
        {
            var prod = await context.products.FindAsync(id);
            if(prod == null)
            {
                return NotFound("the Product is not Found");
            }
            return Ok(prod.Adapt<ProductDtos>());

        }


        [HttpPost("Create")]
        public async Task<IActionResult> CreateAsync(ProductDtos Requset, [FromServices] IValidator<ProductDtos> validator)
        {
            logger.LogWarning("Pleaze Make Sure To Create Correct Product..");
            var validationResult = validator.Validate(Requset);

            if (!validationResult.IsValid)  // Check if the validation result is invalid
            {
                // Initialize the ModelStateDictionary
                var modelState = new ModelStateDictionary();

                // Populate the ModelState with validation errors
                validationResult.Errors.ForEach(error =>
                {
                    modelState.AddModelError(error.PropertyName, error.ErrorMessage);
                });

                // Return a validation problem result
                return ValidationProblem(modelState);
            }

            // Proceed with adding the product
            await context.products.AddAsync(Requset.Adapt<Products>());
            await context.SaveChangesAsync();

            return Ok("The product was added successfully.");
        }



        [HttpDelete("delete")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            var prod = await context.products.FindAsync(id);
            if (prod == null)
            {
                return NotFound("Employee Not Found");
            }
           context.products.Remove(prod);
           await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPut("Edite")]
        public async Task<IActionResult> EditeAsync(int id,ProductDtos request)
        {
            var prod =  await context.products.FindAsync(id);
            if (prod == null)
            {
                return NotFound();
            }
            request.Adapt(prod);
          await context.SaveChangesAsync();
            return NoContent();
        }
    }
}
