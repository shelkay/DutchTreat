using DutchTreat.Services;
using DutchTreat.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DutchTreat.Controllers {

    public class AppController : Controller
    {
        private readonly IMailService mailService;

        public AppController(IMailService mailService)
        {
            this.mailService = mailService;
            //_mailService = mailService;
        }



        public IActionResult Index() {
            return View();
        }

        [HttpGet("contact")]
        public IActionResult Contact() {
            ViewBag.Title = "Contact Us";

            //throw new InvalidOperationException("Bad things happened!");

            return View();        
        }

        [HttpPost("contact")]
        public IActionResult Contact(ContactViewModel model)
        {
            try {
                if(model == null)  
                    throw new ArgumentNullException(nameof(model));
          
                if (ModelState.IsValid)
                {
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


        public IActionResult About()
        {
            ViewBag.Title = "About Us";

            return View();
        }
    }
}