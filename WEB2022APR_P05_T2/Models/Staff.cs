using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WEB2022APR_P05_T2.Models
{
    public class Staff
    {
        public string StaffID { get; set; }
        public string? StoreID { get; set; }
        public string StaffName { get; set; }
        public char SGender { get; set; }
        public string SAppt { get; set; }
        public string STelNo { get; set; }
        public string SEmailAddr { get; set; }
        public string SPassword { get; set; }
    }
}
