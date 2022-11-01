using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WEB2022APR_P05_T2.Models
{
    public class CashVoucher
    {
        public int IssuingID { get; set; }

        [StringLength(9)]
        public string MemberID { get; set; }

        public decimal Amount { get; set; }

        public int MonthIssuedFor { get; set; }

        public int YearIssuedFor { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime DateTimeIssued { get; set; }

        [StringLength(30)]
        [Display(Name = "Voucher No.")]
        public string? VoucherSN { get; set; }

        [StringLength(1)]
        public char Status { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? DateTimeRedeemed { get; set; }
    }
}
