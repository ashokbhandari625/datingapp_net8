using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class RegisterDTO
    {
        [Required]
        public  string Username { get; set; } = string.Empty; 
        [Required]
        [StringLength(8 ,MinimumLength =1)]
        public   string Password { get; set; } = string.Empty;
    }
}