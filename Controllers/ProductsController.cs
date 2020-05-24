using DutchTreat.Data;
using DutchTreat.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DutchTreat.Controllers
{
    // Extra annotation for documentation
    [Route("api/[Controller]")]
    //[ApiController]// See compatible version for MVC in startup for these tags
    //[Produces("application/json")]  
    public class ProductsController : ControllerBase
    {
        private readonly IDutchRepository repository;
        private readonly ILogger<ProductsController> logger;

        public ProductsController(IDutchRepository repository, 
            ILogger<ProductsController> logger) {
            this.repository = repository;
            this.logger = logger;
        }

        [HttpGet]
        //[ProducesResponseType(200)] 
       // [ProducesResponseType(400)]  // See compatible version for MVC in startup for wiring these tags
        public ActionResult<IEnumerable<Product>> Get() {
            try {
                return Ok(repository.GetAllProducts());
            }
            catch(Exception e) {
                logger.LogError($"Failed to get product: {e}");
                return BadRequest("Failed to get products");
            }

        } 
    }
}
