using CleanArchitecture.Infrastructure.Common;
using System;
using System.ComponentModel.DataAnnotations;

namespace CleanArchitecture.Infrastructure.Services
{
    public class RegisterDto
    {
        public bool IsRegistered { get; set; }
    }

    public class RegisterCuDto
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        public string CompanyName { get; set; }
        [Required]
        public string MobileNumber { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }

    public class CodeWithMobileNumber
    {
        public string Code { get; set; }
        public string MobileNumber { get; set; }
    }

    public class MobileNumberRegister : BaseViewModel
    {
        public MobileNumberRegister()
        {
            IsRegistered = false;
        }
        public bool IsRegistered { get; set; }
        [Required]
        public string MobileNumber { get; set; }
    }
}

