using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class RegisterDto
    {
        [Required] // Data Annotation
        public string UserName { get; set; }

        [Required] // Data Annotation
        public string Password { get; set; }
    }
}