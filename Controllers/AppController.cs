using DutchTreat.Data;
using DutchTreat.Services;
using DutchTreat.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace DutchTreat.Controllers
{

    public class AppController : Controller
    {
        private readonly IMailService mailService;
        private readonly IDutchRepository respository;
    

        public AppController(IMailService mailService, IDutchRepository respository) {
            this.mailService = mailService;
            this.respository = respository;      
            //_mailService = mailService;
        }



        public IActionResult Index() {
            //var results  = respository.GetAllProducts();
            return View();
        }

        [HttpGet("contact")]
        public IActionResult Contact() {
            ViewBag.Title = "Contact Us";

            //throw new InvalidOperationException("Bad things happened!");

            return View();
        }

        [HttpPost("contact")]
        public IActionResult Contact(ContactViewModel model) {
            try {
                if(model == null)
                    throw new ArgumentNullException(nameof(model));

                if(ModelState.IsValid) {
                    mailService.SendMessage("abc@mymail.com", model.Subject, $"From: {model.Name} - {model.Email}, Message: {model.Message}");
                    ViewBag.UserMessage = "Mail Sent";
                    ModelState.Clear();
                }
            }
            catch(ArgumentNullException e) {
                Console.WriteLine(e.Message);
            }
            return View();
        }


        public IActionResult About() {
            ViewBag.Title = "About Us";

            return View();
        }

        [Authorize]
        public IActionResult Shop() {
            var results = respository.GetAllProducts();

            return View(results);
        }
    }
}