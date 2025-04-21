using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Services.Abstractions;

namespace Presentation
{
    [ApiController]
    [Route("api/[controller]")]
   public  class ProductsController (IServiceManager servicManager) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAllProduct()
        {

            var result = await servicManager.ProductService.GetAllproductAsync();
            if (result is null) return BadRequest();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            var result = await servicManager.ProductService.GetproductAsync(id);
            if (result is null) return NotFound();
            return Ok(result);
        }


    }
}
























