using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.DTOs.IdentityDTO_s
{
    public class RegisterDTO
    {

        [Required]
        public string PersonName { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        //this tell to view that render as inputType=tel -> not for validation 
        [DataType(DataType.PhoneNumber)]
        [RegularExpression(@"^(?:\+201|01)[0-2,5][0-9]{8}$",
            ErrorMessage = "Invalid Egyptian phone number format.")]
        public string PhoneNumber { get; set; }


        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [Compare("Password" , ErrorMessage ="Password does not match")]
        public string ConfirmationPassword { get; set; }

        
    }
}
//Remote validation for email 
//required package view .....
//[Remote(Action:"IsEmailAlreadyRegister",controller:"Account",ErrorMessage ="Email is already used")]