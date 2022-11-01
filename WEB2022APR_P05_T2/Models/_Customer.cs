using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WEB2022APR_P05_T2.Models
{
    public abstract class _Customer
    {
        [Display(Name = "Member ID:")]
        public virtual string MemberID { get; set; }

        [Display(Name = "Name:")]
        public virtual string MName { get; set; }

        [Display(Name = "Gender:")]
        public virtual char MGender { get; set; }

        [Display(Name = "Birth Date:")]
        public virtual DateTime MBirthDate { get; set; }

        [Display(Name = "Address:")]
        public virtual string? MAddress { get; set; }

        [Display(Name = "Country:")]
        public virtual string MCountry { get; set; }

        [Display(Name = "Phone Number:")]
        public virtual string? MTelNo { get; set; }

        [Display(Name = "Email Address:")]
        public virtual string? MEmailAddr { get; set; }
        
        [Display(Name = "Password")]
        public virtual string MPassword { get; set; }
    }
}
