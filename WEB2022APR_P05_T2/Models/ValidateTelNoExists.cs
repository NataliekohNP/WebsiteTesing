using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using WEB2022APR_P05_T2.DAL;

namespace WEB2022APR_P05_T2.Models
{
    public class ValidateTelNoExists : ValidationAttribute
    {
        private UserDAL userContext = new UserDAL();

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // Get the email value to validate
            string telNo = Convert.ToString(value);

            if (userContext.IsTelNoExist(telNo))
            {
                // Validation failed
                return new ValidationResult("Phone Number already exists!");
            }
            else
            {
                // Validation passed
                return ValidationResult.Success;
            }
        }
    }
}
