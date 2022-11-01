using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WEB2022APR_P05_T2.Models
{
    public class Product
    {

        public int ProductId { get; set; }

        [Required]
        [Display (Name ="Product Title")]
        public string ProductTitle { get; set; }
        [Display (Name = "Product Image")]
        public string? ProductImage { get; set; }
        [Required]
        [Display(Name = "Price($)")]
        [Range(20,500)]
        public decimal Price { get; set; }
        [Required]
        [Display (Name ="Effective Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString ="(0:dd-MM-yyyy)")]
        public DateTime? EffectiveDate { get; set; }
        public String Obsolete { get; set; }

        public IFormFile filetoupload { get; set; }
      
    }
}
