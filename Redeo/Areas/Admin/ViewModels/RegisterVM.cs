﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Redeo.Areas.Admin.ViewModels
{
    public class RegisterVM
    {
        [Display(Name = "Full Name")]
        [Required(ErrorMessage = "Name is Required!")]
        public string FullName { get; set; }

        [Required(ErrorMessage = "Birthdate is Required!")]
        [Column(TypeName = "date")]
        public DateTime Birthdate { get; set; }

        [Display(Name = "Username")]
        [Required(ErrorMessage = "Username is required")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Email is Required!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
