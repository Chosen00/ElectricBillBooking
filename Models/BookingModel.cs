using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ElectricBillBooking.Models
{
    public class BookingModel
    {
        public int ClientID { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "First name is required")]
        public string FirstName { get; set; }

        // Assuming MiddleName and SuffixName are optional
        public string MiddleName { get; set; }
        public string SuffixName { get; set; }

        [Required(ErrorMessage = "Booking date is required")]
        [DataType(DataType.DateTime)]
        public DateTime BookDate { get; set; }
        public DateTime BookingDateTime { get; set; }

    }
}
