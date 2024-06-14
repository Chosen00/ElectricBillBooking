using Microsoft.AspNetCore.Mvc;
using ElectricBillBooking.Models;
using System;

public class SignUpController : Controller
{
    private readonly SignUpContext _clientContext;
    

    public SignUpController(SignUpContext clientContext)
    {
        _clientContext = clientContext;
    }

    [HttpGet]
    public IActionResult SignUpForm()
    {
        var client = new SignUpModel();
        return View(client);
    }

    [HttpPost]
    public IActionResult SignUpForm(SignUpModel client)
    {
        try
        {
            // Check if username and password already exist
            if (_clientContext.IsUserExists(client.UserName, client.Password))
            {
                ModelState.AddModelError(string.Empty, "Username or password already exists.");
                return View(client);
            }

            bool isSuccess = _clientContext.InsertClient(client);
            if (isSuccess)
            {
                TempData["SuccessMessage"] = "Client Successfully Added!";
                return RedirectToAction("SignUpSuccessForm");
            }
            else
            {
                TempData["ErrorMessage"] = "Error occurred while saving client data.";
            }
        }
        catch (Exception ex)
        {
            TempData["ErrorMessageCatch"] = $"Error: {ex.Message}";
        }
        return View(client);
    }


    [HttpGet]
    public IActionResult SignUpSuccessForm()
    {
        return View();
    }
}
