using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WEB2022APR_P05_T2.Models
{
    public class MonthlySpendingViewModel
    {
        public string selMonth {get; set;}
        public string selYear { get; set; }
        public List<MonthlySpending> monthlySpendingList { get; set; }
        public MonthlySpendingViewModel()
        {
            monthlySpendingList = new List<MonthlySpending>();
        }
        public List<SelectListItem> voucherSelect { get; set; }
    }
}
