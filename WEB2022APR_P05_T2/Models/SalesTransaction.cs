using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WEB2022APR_P05_T2.Models
{
    public class SalesTransaction
    {
        public int TransactionID { get; set; }

        [StringLength(25)]
        public string StoreID { get; set; }

        [StringLength(9)]
        public char? MemberID { get; set; }

        public double Subtotal { get; set; }

        public double Tax { get; set; }

        public int DiscountPercent { get; set; }

        public double DiscountAmt { get; set; }

        public double Total { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime DateCreated { get; set; }
    }
}
