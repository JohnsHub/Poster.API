﻿using System.ComponentModel.DataAnnotations;

namespace Poster.API.Models.DTOs
{
    public class LoginDto
    {
        [Required] public string Username { get; set; }
        [Required] public string Password { get; set; }
    }
}
