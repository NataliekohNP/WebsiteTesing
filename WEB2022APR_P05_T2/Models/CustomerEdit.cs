using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WEB2022APR_P05_T2.Models
{
    public class CustomerEdit : _Customer
    {
        [RegularExpression(@"[689]\d{7}|\+65[689]\d{7}$", ErrorMessage = "Enter a valid SG mobile number")]
        [Phone(ErrorMessage = "Please use another mobile number")]
        public override string? MTelNo { get; set; }

        [EmailAddress(ErrorMessage = "Please use another email address")]
        public override string? MEmailAddr { get; set; }

        [Required]
        public override string MPassword { get; set; }
    }
}
