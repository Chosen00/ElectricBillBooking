using Microsoft.AspNetCore.Mvc;
using ElectricBillBooking.Models;
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace ElectricBillBooking.Controllers
{
    public class BookingController : Controller
    {
        private readonly BookingContext _bookingContext;
        private readonly ILogger<BookingController> _logger;
        

        public BookingController(BookingContext bookingContext, ILogger<BookingController> logger)
        {
            _bookingContext = bookingContext;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult BookingSuccesfulForm()
        {
            return View();
        }

        [HttpGet]
        public IActionResult UpdateSuccessfulForm()
        {
            return View();
        }


        
        [HttpGet]
        public IActionResult DeleteRequestForm()
        {
            // Retrieve bookings from the database
            List<BookingModel> bookings = _bookingContext.GetBookings();

            // Pass the bookings to the view
            return View(bookings);
        }

        [HttpGet]
        public IActionResult DashboardForm()
        {
            return View();
        }

        [HttpGet]
        public IActionResult BookingForm()
        {
            var booking = new BookingModel();
            return View(booking);
        }

        //******************************************************************************************************************************************************//
        //Create
        [HttpPost]
        public IActionResult BookingForm(BookingModel bookclient)
        {
            try
            {
                // Assign the value of BookingDateTime to BookDate
                bookclient.BookDate = bookclient.BookingDateTime;

                // Check if the booking date and time is unique
                if (!_bookingContext.IsBookingDateTimeUnique(bookclient.BookDate))
                {
                    // If not unique, return error message
                    TempData["ErrorMessage"] = "Booking date and time must be unique.";
                    return View(bookclient);
                }

                // If the booking date and time is unique, proceed with insertion
                bool isSuccess = _bookingContext.InsertBooking(bookclient);
                if (isSuccess)
                {
                    TempData["SuccessMessage"] = "Booking Form is Successful!";
                    return RedirectToAction("BookingSuccesfulForm");
                }
                else
                {
                    TempData["ErrorMessage"] = "Error occurred while saving data.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessageCatch"] = $"Error: {ex.Message}";
            }
            return View(bookclient);
        }




        //Update
        [HttpGet]
        public IActionResult UpdateBooktableForm(int clientId)
        {
            var clientIds = _bookingContext.GetClientById(clientId);
            return View(clientIds);
        }

        [HttpPost]
        public IActionResult UpdateBooktableForm(BookingModel updatedclients)
        {
            try
            {
                int ClientID = updatedclients.ClientID;
                bool isSuccess = _bookingContext.UpdateClient(updatedclients);

                if (isSuccess)
                {
                    TempData["SuccessMessage"] = "Client booking updated successfully!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Failed to update client booking.";
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessageCatch"] = $"Error: {ex.Message}";
            }
            return RedirectToAction("UpdateBooktableForm", new { clientId = updatedclients.ClientID }); // Redirect to display the message
        }



        //Delete
        [HttpPost]
        public IActionResult DeleteRequestForm(string LastName, string FirstName, string MiddleName, string SuffixName)
        {
            try
            {
                bool isSuccess = _bookingContext.DeleteBookingByFullname(LastName, FirstName, MiddleName, SuffixName);
                if (isSuccess)
                {
                    TempData["SuccessMessage"] = "Booking deleted successfully!";
                }
                else
                {
                    TempData["ErrorMessage"] = "Error occurred while deleting booking.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while deleting booking by fullname: {ex.Message}", ex);
                TempData["ErrorMessage"] = $"Error: {ex.Message}";
            }

            // Redirect back to the booking dashboard page
            return RedirectToAction("DeleteRequestForm");
        }

    }
}