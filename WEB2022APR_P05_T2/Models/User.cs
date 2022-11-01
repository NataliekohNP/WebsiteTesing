using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WEB2022APR_P05_T2.Models
{
    public class User
    {
        [Required]
        public string Username { get; set; }
        public string UPassword { get; set; }
        public string URole { get; set; }
    }
}
