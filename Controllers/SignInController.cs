using Microsoft.AspNetCore.Mvc;
using ElectricBillBooking.Models;
using System;

namespace ElectricBillBooking.Controllers
{
    public class SignInController : Controller
    {
        private readonly SignInContext _signInContext;
        private readonly BookingContext _bookingContext;
        public SignInController(SignInContext signInContext, BookingContext bookingContext)
        {
            _signInContext = signInContext;
            _bookingContext = bookingContext;
        }

        [HttpGet]
        public IActionResult SignInForm()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SignInForm(string username, string password)
        {
            try
            {
                if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
                {
                    if (_signInContext.ValidateUser(username, password, out string errorMessage))
                    {
                        // Redirect to the appropriate action after successful login (e.g., dashboard)
                        return RedirectToAction("DashboardForm");
                    }
                    else
                    {
                        TempData["ErrorMessage"] = errorMessage ?? "An error occurred while validating the user.";
                    }
                }
                else
                {
                    TempData["ErrorMessage"] = "Username and password cannot be empty.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessageCatch"] = $"Error: {ex.Message}";
            }
            return RedirectToAction("SignInForm"); // Redirect back to the sign-in form if authentication fails or an error occurs
        }

        [HttpGet]
        public IActionResult DashboardForm()
        {
            return View();
        }
    }
}
