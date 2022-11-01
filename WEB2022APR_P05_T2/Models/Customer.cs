using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WEB2022APR_P05_T2.Models
{
    public class Customer : _Customer
    {
        [Required]
        [ValidateMemberIDExists]
        public override string MemberID { get; set; }

        [Required]
        public override string MName { get; set; }

        public override char MGender { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd-MMM-yyyy}")]
        public override DateTime MBirthDate { get; set; }

        public override string? MAddress { get; set; }

        public override string MCountry { get; set; }

        [ValidateTelNoExists]
        [RegularExpression(@"[689]\d{7}|\+65[689]\d{7}$")]
        [Phone]
        public override string? MTelNo { get; set; }

        [ValidateEmailExists]
        [EmailAddress]
        public override string? MEmailAddr { get; set; }

        public override string MPassword { get; set; }
    }
}
