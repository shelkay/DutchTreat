using AutoMapper;
using DutchTreat.Data;
using DutchTreat.Data.Entities;
using DutchTreat.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DutchTreat.Controllers
{
    [Route("api/[Controller]")]
    public class OrderController  : ControllerBase 
    {
        private readonly IDutchRepository repository;
        private readonly ILogger<OrderController> logger;
        private readonly IMapper mapper;

        public OrderController(
            IDutchRepository repository, 
            ILogger<OrderController> logger, 
            IMapper mapper) {
            this.repository = repository;
            this.logger = logger;
            this.mapper = mapper;
        }

        /// <summary>
        /// Get  Orders
        /// </summary>
        /// <param name="includeItems"></param>
        /// <returns>should items be incl with order</returns>
        public IActionResult Get(bool includeItems = true) {
            try {
                var results = repository.GetAllOrders();


                return Ok(mapper.Map<IEnumerable<Order>, 
                    IEnumerable<OrderViewModel>>(repository.GetAllOrders(includeItems)));
            }
            catch(Exception e) {

                logger.LogError($"Failed to get orders: {e}");
                return BadRequest("Failed to get orders");
            }        
        }

        [HttpGet("{id:int}")]
        public IActionResult Get(int id) {
            try {
                var order = repository.GetOrderById(id);
                if(order != null)
                    return Ok(mapper.Map<Order, OrderViewModel>( order));
                else
                    return NotFound();
            }
            catch(Exception e) {

                logger.LogError($"Failed to get orders: {e}");
                return BadRequest("Failed to get orders");
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody]OrderViewModel model) {
            try {
                if(ModelState.IsValid) {
                    var newOrder = mapper.Map<OrderViewModel, Order>(model); 

                    if(newOrder.OrderDate == DateTime.MinValue) {
                        newOrder.OrderDate = DateTime.Now;
                    }

                    repository.AddEntity(model);
                    if(repository.SaveAll()) {
                        var vm = mapper.Map<Order,OrderViewModel>(newOrder);
                        return Created($"/api/orders/{newOrder.Id}", vm);
                    }                    
                }
                else {
                    // Expose errors....
                    return BadRequest(ModelState);
                }
            }
            catch(Exception e) {
                logger.LogError($"Failed to save new order: {e}");
                return BadRequest("Failed to save new order");
            }
            return BadRequest("Failed to save new data");
        }
    }
}
