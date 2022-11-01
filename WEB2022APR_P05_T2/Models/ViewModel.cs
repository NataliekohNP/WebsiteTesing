using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WEB2022APR_P05_T2.Models
{
    public class ViewModel
    {
        public IEnumerable<Feedback> Feedbacks { get; set; }
        public IEnumerable<Response> Responses { get; set; }
    }
}
