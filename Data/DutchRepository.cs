using DutchTreat.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DutchTreat.Data
{
    public class DutchRepository : IDutchRepository
    {
        private readonly DutchContext context;
        private readonly ILogger<DutchRepository> logger;

        public DutchRepository(DutchContext context, ILogger<DutchRepository> logger) {
            this.context = context;
            this.logger = logger;
        }

        public void AddEntity(object model) {
            context.Add(model);
        }

        public IEnumerable<Order> GetAllOrders(bool includeItems) {
            if(includeItems) {
                return context.Orders
                    .Include(o => o.Items)
                    .ThenInclude(i => i.Product)
                    .ToList();
            }
            else {
                return context.Orders.ToList();
            }

        }

        public IEnumerable<Product> GetAllProducts() {
            try {
                logger.LogInformation("GetAllProducts was called");
                return context.Products
                              .OrderBy(p => p.Title)
                              .ToList();
            }
            catch(Exception ex) {

                logger.LogError($"Failed to get all products: {ex}");
                return null;
            }
        }

        /// <summary>
        /// Get order by id
        /// </summary>
        /// <param name="id"></param>
        /// <remarks>
        /// By including the product object from the orderitem relationship we can make 
        /// product properties available to the model view simple by prefixing the properties with "product" 
        /// in the OrderItemViewModel
        /// </remarks>
        /// <returns></returns>
        public Order GetOrderById(int id) {
            return context.Orders
                .Include(o => o.Items)
                .ThenInclude(i => i.Product)
                .Where(o => o.Id == id )
                .FirstOrDefault();
        }

        public IEnumerable<Product> GetProductsByCategory(string category) {
            return context.Products
                           .Where(p => p.Category == category)
                           .ToList();
        }

        public bool SaveAll() {
            return context.SaveChanges() > 0;
        }
    }
}
