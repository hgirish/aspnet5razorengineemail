using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.Extensions.PlatformAbstractions;
using RazorEngine;
using RazorEngine.Configuration;
using RazorEngine.Templating;
using RazorEngine.Text;
using RazorEngineEmail.Models;

namespace RazorEngineEmail.Controllers
{
    public class HomeController : Controller
    {
        private IApplicationEnvironment _appEnv;
        private IEmailService _emailService;
        public HomeController(IApplicationEnvironment appEnv, IEmailService emailService)
        {
            _appEnv = appEnv;
            _emailService = emailService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            var config = new TemplateServiceConfiguration();
            config.Debug = true;
            config.EncodedStringFactory = new RawStringFactory();
            var service = RazorEngineService.Create(config);
            Engine.Razor = service;
            var model = new UserModel {Name = "Sarah", Email = "sarah@eaxmaple.com", IsPremiumUser = false};
            var basePath = _appEnv.ApplicationBasePath;
            var templateFolderPath =Path.Combine(basePath,"Views/EmailTemplates");
            var emailPath = Path.Combine(templateFolderPath, "Welcome.cshtml");
            var emailString = System.IO.File.ReadAllText(emailPath);
            string template = "Hello @Model.Name, welcome to RazorEngine!";
          
             var result = Engine.Razor.RunCompile(emailString, "welcome", null, model);
     //       var result =
     //Engine.Razor.RunCompile(template, "templateKey", null, new { Name = "World" });
            ViewData["Message"] = result;
            _emailService.SendEmail("test@example.com","RazorEngine test",result);
            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
