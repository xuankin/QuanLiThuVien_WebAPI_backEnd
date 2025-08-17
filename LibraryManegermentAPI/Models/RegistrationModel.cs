﻿using System.ComponentModel.DataAnnotations;

namespace LibraryManagementApi.Models
{
    public class RegistrationModel
    {
        [Required(ErrorMessage = "Username is required")]
        public string Username { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Password is required")]
        [MinLength(6, ErrorMessage = "Password must be at least 6 characters long")]
        public string Password { get; set; } = string.Empty;

        public string? Initials { get; set; } // Optional

        public string? Role { get; set; } // Optional - assign a role if needed
    }
}