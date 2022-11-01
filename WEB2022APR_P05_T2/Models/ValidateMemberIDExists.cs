using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using WEB2022APR_P05_T2.DAL;

namespace WEB2022APR_P05_T2.Models
{
    public class ValidateMemberIDExists : ValidationAttribute
    {
        private UserDAL userContext = new UserDAL();

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // Get the memberId value to validate
            string memberId = Convert.ToString(value);

            if (userContext.IsMemberIdExist(memberId))
            {
                // Validation failed
                return new ValidationResult("Member ID already exists!");
            }
            else
            {
                // Validation passed
                return ValidationResult.Success;
            }
        }
    }
}
