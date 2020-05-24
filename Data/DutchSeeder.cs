using DutchTreat.Data.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DutchTreat.Data
{
    public class DutchSeeder
    {
        private readonly DutchContext ctx;
        private readonly IHostingEnvironment hosting;
        private readonly UserManager<StoreUser> userManager;

        public DutchSeeder(
            DutchContext ctx, 
            IHostingEnvironment hosting,
            UserManager<StoreUser> userManager) {

            this.ctx = ctx;
            this.hosting = hosting;
            this.userManager = userManager;
        }

        public async Task SeedAsync() {
            ctx.Database.EnsureCreated();

            StoreUser user = await userManager.FindByEmailAsync("abc@gmail.com");

            if(user == null) {
                user = new StoreUser() {
                    FirstName = "Billy",
                    LastName = "Shelton",
                    Email = "abc@gmail.com",
                    UserName = "abc@gmail.com"
                };

                // TODO: Learn how to configure identity user...
                var result = await userManager.CreateAsync(user, "P@ssw0rd!");
                if(result != IdentityResult.Success) {
                    throw new InvalidOperationException("User not found");
                }            
            }

            if(!ctx.Products.Any()) {
                // Need to create sample data
                var filePath = Path.Combine(hosting.ContentRootPath, "Data/art.json");
                var json = File.ReadAllText(filePath);
                var products = JsonConvert.DeserializeObject<IEnumerable<Product>>(json);

                ctx.Products.AddRange(products);

                var order = ctx.Orders.Where(o => o.Id == 1).FirstOrDefault();

                if(order != null) {
                    order.User = user;
                    order.Items = new List<OrderItem>() {
                        new OrderItem(){ 
                            Product = products.First(),
                            Quantity = 5,
                            UnitPrice = products.First().Price
                        }
                    };
                }

                ctx.SaveChanges();
            }
        }
        
    }
}
