using ElectricBillBooking.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

    public class HomeController : Controller
    {
      
        // Method to determine if footer should be hidden based on the current action
        private bool IsHiddenAction()
        {
            var hiddenActions = new[] { "About", "Privacy", "SignUp", "SignIn" }; // List of actions where footer should be hidden
            var actionName = RouteData.Values["action"]?.ToString(); // Get the current action name
            return hiddenActions.Contains(actionName); // Check if the current action is in the list of hidden actions
        }

        // GET: /Home/Index
        public IActionResult Index()
        {
            return View();
        }

        // GET: /Home/About
        public IActionResult About()
        {
            return View();
        }

        // GET: /Home/Privacy
        public IActionResult Privacy()
        {
            return View();
        }

        // Override the View method to pass data to the view
        public override ViewResult View(object? model)
        {
            // Create a new ViewDataDictionary object
            var viewData = new ViewDataDictionary(ViewData)
            {
                ["IsHiddenFooter"] = IsHiddenAction() // Pass whether to hide the footer based on the current action
            };

            // Call the base View method with the new ViewDataDictionary object
            return base.View(viewData);
        }
    }
