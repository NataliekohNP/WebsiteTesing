using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WEB2022APR_P05_T2.Models
{
    public class Response
    {
        public int ResponseID { get; set; }
        public int FeedbackID { get; set; }
        public string? MemberID { get; set; }

        public string StaffID { get; set; }
        public DateTime DateTimePosted { get; set; }
        [Required(ErrorMessage ="You must enter a response")]
        public string Text { get; set; }
        public string UserFeedback { get; set; }
        public string UserSubject { get; set; }

    }
}
