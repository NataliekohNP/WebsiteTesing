using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Data.SqlClient;
using WEB2022APR_P05_T2.Models;

namespace WEB2022APR_P05_T2.DAL
{
    public class UserTransactionDAL
    {
        private IConfiguration Configuration { get; }
        private SqlConnection conn;

        public UserTransactionDAL()
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json");
            Configuration = builder.Build();
            string strConn = Configuration.GetConnectionString("FashionConnectionString");
            conn = new SqlConnection(strConn);
        }

        public List<TransactionItem> ItemTransactions()
        {
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"SELECT * FROM TransactionItem ORDER BY TranactionID";

            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            List<TransactionItem> itemTransactionList = new List<TransactionItem>();
            while (reader.Read())
            {
                itemTransactionList.Add(
                    new TransactionItem
                    {
                        TransactionID = Convert.ToInt32(0),
                        ProductID = Convert.ToInt32(1),
                        Price = Convert.ToDecimal(2),
                        Quantity = Convert.ToInt32(3)
                    }) ;
            }
            reader.Close();
            conn.Close();
            return itemTransactionList;
        }

        public List<SalesTransaction> SalesTransactions()
        {
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"SELECT * FROM SalesTransaction ORDER BY DateCreated";

            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            List<SalesTransaction> salesTransactionList = new List<SalesTransaction>();
            while (reader.Read())
            {
                salesTransactionList.Add(
                    new SalesTransaction
                    {
                        TransactionID = Convert.ToInt32(0),
                        StoreID = Convert.ToString(1),
                        MemberID = Convert.ToChar(2),
                        Subtotal = Convert.ToDouble(3),
                        Tax = Convert.ToDouble(4),
                        DiscountPercent = Convert.ToInt32(5),
                        DiscountAmt = Convert.ToDouble(6),
                        Total = Convert.ToDouble(7),
                        DateCreated = Convert.ToDateTime(8)
            });
            }
            reader.Close();
            conn.Close();
            return salesTransactionList;
        }

        public TransactionItem GetTransactionItem(string TransactionID)
        {
            TransactionItem item = new TransactionItem();

            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"SELECT * FROM TransactionItem WHERE TransactionID = @selectedTransactionID";
            cmd.Parameters.AddWithValue("@selectedTransactionID", TransactionID);

            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                item.TransactionID = Convert.ToInt32(TransactionID);
                item.ProductID = reader.GetInt32(1);
                item.Price = reader.GetDecimal(2);
                item.Quantity = reader.GetInt32(3);
            }
            reader.Close();
            conn.Close();
            return item;
        }

        public SalesTransaction GetSalesTransaction(string TransactionID)
        {
            SalesTransaction sales = new SalesTransaction();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"SELECT * FROM SalesTransaction WHERE TransactionID = @selectedTransactionID";
            cmd.Parameters.AddWithValue("@selectedTransactionID", TransactionID);
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                sales.TransactionID = Convert.ToInt32(TransactionID);
                sales.StoreID = reader.GetString(1);
                sales.MemberID = !reader.IsDBNull(2)? reader.GetChar(2): (char)0;
                sales.Subtotal = reader.GetDouble(3);
                sales.Tax = reader.GetDouble(4);
                sales.DiscountPercent = reader.GetInt32(5);
                sales.DiscountAmt = reader.GetDouble(6);
                sales.Total = reader.GetDouble(7);
                sales.DateCreated = reader.GetDateTime(8);
            }
            reader.Close();
            conn.Close();
            return sales;
        }

        public List<CashVoucher> getVouchers()
        {
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"SELECT * FROM CashVoucher ORDER BY IssuingID";

            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            List<CashVoucher> voucherList = new List<CashVoucher>();
            while (reader.Read())
            {
                voucherList.Add(
                    new CashVoucher
                    {
                        IssuingID = Convert.ToInt32(0),
                        MemberID = Convert.ToString(1),
                        Amount = Convert.ToDecimal(2),
                        MonthIssuedFor = Convert.ToInt32(3),
                        YearIssuedFor = Convert.ToInt32(4),
                        DateTimeIssued = Convert.ToDateTime(5),
                        VoucherSN = Convert.ToString(6),
                        Status = Convert.ToChar(7),
                        DateTimeRedeemed = Convert.ToDateTime(8)
                    });
            }
            reader.Close();
            conn.Close();
            return voucherList;
        }
        
        public List<MonthlySpending> GetMonthlySpendingsByMonth(string month, string year)
        {
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"Select * From  SalesTransaction Where MONTH(DateCreated) = @Month and YEAR(DateCreated) = @Year and MemberID IS NOT NULL";
            cmd.Parameters.AddWithValue("@Month", month);
            cmd.Parameters.AddWithValue("@Year", year);
            List<MonthlySpending> monthlySpending = new List<MonthlySpending>();
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    monthlySpending.Add(
                        new MonthlySpending
                        {
                            MemberID = reader.GetString(2),
                            TotalAmtSpent = reader.GetDecimal(7)
                        });
                }
            }
            reader.Close();
            conn.Close();
            return monthlySpending;
        }
        public List<CashVoucher> GetCashVouchersbyMonth(string month, string year)
        {
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"Select* From  CashVoucher Where MONTH(DateTimeIssued) = @Month + 1 and YEAR(DateTimeIssued) = @Year and MemberID IS NOT NULL";
            cmd.Parameters.AddWithValue("@Month", month);
            cmd.Parameters.AddWithValue("@Year", year);
            List<CashVoucher> cashVouchers = new List<CashVoucher>();
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    cashVouchers.Add(
                        new CashVoucher
                        {
                            IssuingID = reader.GetInt32(0),
                            MemberID = reader.GetString(1),
                            Amount = reader.GetDecimal(2),
                            MonthIssuedFor = reader.GetInt32(3),
                            YearIssuedFor = reader.GetInt32(4),
                            DateTimeIssued = reader.GetDateTime(5),
                            VoucherSN = !reader.IsDBNull(6) ?reader.GetString(6): null,
                            Status = reader.GetString(7)[0],
                            DateTimeRedeemed = !reader.IsDBNull(8) ?reader.GetDateTime(8): (DateTime?)null
                        });
                }
            }
            reader.Close();
            conn.Close();
            return cashVouchers;
        }
        
        public int assignVoucher(CashVoucher cv)
        {
            SqlCommand cmd = conn.CreateCommand();

            cmd.CommandText = @"INSERT INTO CashVoucher (MemberID, Amount, MonthIssuedFor, YearIssuedFor, DateTimeIssued, VoucherSN, Status, DateTimeRedeemed) VALUES(@memberID,@amount, @monthIssuedFor, @yearIssuedFor, @dateTimeIssued, @voucherSN, @status, @dateTimeRedeemed)";
            cmd.Parameters.AddWithValue("@memberID", cv.MemberID);
            cmd.Parameters.AddWithValue("@amount", cv.Amount);
            cmd.Parameters.AddWithValue("@monthIssuedFor", cv.MonthIssuedFor);
            cmd.Parameters.AddWithValue("@yearIssuedFor", cv.YearIssuedFor);
            cmd.Parameters.AddWithValue("@dateTimeIssued", cv.DateTimeIssued);
            cmd.Parameters.AddWithValue("@voucherSN", cv.VoucherSN?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@status", cv.Status);
            cmd.Parameters.AddWithValue("@dateTimeRedeemed", cv.DateTimeRedeemed?? (object)DBNull.Value);
            conn.Open();

            cmd.ExecuteNonQuery();
            conn.Close();
            return cv.IssuingID;
        }

    }
}
