using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WEB2022APR_P05_T2.Models
{
    public class MonthlySpending
    {
        public string? MemberID { get; set; }

        public decimal TotalAmtSpent { get; set; }

        public int noTransactions { get; set; }

        public decimal Voucher { get; set; }
        public bool VoucherAssigned { get; set; }
    }
}
