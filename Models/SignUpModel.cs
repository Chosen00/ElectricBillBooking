using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ElectricBillBooking.Models
{
    public class SignUpModel
    {
      
        public string UserName { get; set; }

        public string Password { get; set; }

        public string LastName { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string SuffixName { get; set; }

        public DateTime Birthdate { get; set; }

        public string Gender { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "Phone number must be exactly 11 digits long")]
        public string CellphoneNumber { get; set; }
        public string StreetAddress { get; set; }

        public string City { get; set; }

        public string StateProvince { get; set; }
    }
}
