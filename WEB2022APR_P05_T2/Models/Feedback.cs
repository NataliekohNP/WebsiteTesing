using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WEB2022APR_P05_T2.Models
{
    public class Feedback
    {
        public int FeedbackID { get; set; }
        public string MemberID { get; set; }
        [DataType(DataType.Date)]
        public DateTime? DateTimePosted { get; set; }

        [Display(Name = "Subject:")]
        [Required]
        public string UserSubject { get; set; }
        
        [Display(Name = "Feedback:")]
        [Required]
        public string UserFeedback { get; set; }

        public string? ImageFileName { get; set; }

        public IFormFile? fileToUpload { get; set; }
    }
}
