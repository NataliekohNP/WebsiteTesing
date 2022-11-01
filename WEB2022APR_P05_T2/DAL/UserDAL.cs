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
    public class UserDAL
    {
        private IConfiguration Configuration { get; }
        private SqlConnection conn;
        private List<string> memberID = new List<string>();

        public UserDAL()
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json");
            Configuration = builder.Build();
            string strConn = Configuration.GetConnectionString("FashionConnectionString");
            conn = new SqlConnection(strConn);
        }

        public Staff GetStaffDetail(string StaffID)
        {
            Staff staff = new Staff();

            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"SELECT * FROM Staff WHERE StaffID = @selectedStaffID";
            cmd.Parameters.AddWithValue("@selectedStaffID", StaffID);

            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                while (reader.Read())
                {
                    staff.StaffID = StaffID;
                    staff.StoreID = !reader.IsDBNull(1) ? reader.GetString(1) : null;
                    staff.StaffName = !reader.IsDBNull(2) ? reader.GetString(2) : null;
                    staff.SGender = !reader.IsDBNull(3) ? reader.GetChar(3) : (char)0;
                    staff.SAppt = !reader.IsDBNull(4) ? reader.GetString(4) : null;
                    staff.STelNo = !reader.IsDBNull(5) ? reader.GetString(5) : null;
                    staff.SEmailAddr = !reader.IsDBNull(6) ? reader.GetString(6) : null;
                    staff.SPassword = !reader.IsDBNull(7) ? reader.GetString(7) : null;

                }
            }
            reader.Close();
            conn.Close();

            return staff;
        }
        public List<Customer> GetCustomer(string MemberID)
        {
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"SELECT * FROM Customer WHERE MemberID = @member";
            cmd.Parameters.AddWithValue("@member", MemberID);

            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            List<Customer> customerList = new List<Customer>();

            while (reader.Read())
            {
                customerList.Add(
                    new Customer 
                    {
                        MemberID = reader.GetString(0),
                        MName = reader.GetString(1),
                        MGender = !reader.IsDBNull(2) ? reader.GetString(2).First() : (Char)0,
                        MBirthDate = reader.GetDateTime(3),
                        MAddress = !reader.IsDBNull(4) ? reader.GetString(4) : null,
                        MCountry = reader.GetString(5),
                        MTelNo = !reader.IsDBNull(6) ? reader.GetString(6) : null,
                        MEmailAddr = !reader.IsDBNull(7) ? reader.GetString(7) : null,
                    }
                );
            }
            reader.Close();
            conn.Close();
            return customerList;
        }

        public CustomerEdit GetCustomerDetails(string customerID)
        {
            CustomerEdit customer = new CustomerEdit();
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();

            //Specify the SELECT SQL statement that
            //retrieves all attributes of a customer record.
            cmd.CommandText = @"SELECT * FROM Customer
                                WHERE MemberID = @customerID";

            //Define the parameter used in SQL statement, value for the
            //parameter is retrieved from the method parameter “customerID”.

            cmd.Parameters.AddWithValue("@customerID", customerID);
            //Open a database connection
            conn.Open();

            //Execute SELCT SQL through a DataReader
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                //Read the record from database
                while (reader.Read())
                {
                    // Fill staff object with values from the data reader
                    customer.MemberID = customerID.ToString();
                    customer.MName = !reader.IsDBNull(1) ? reader.GetString(1) : null;
                    customer.MGender = !reader.IsDBNull(2) ? reader.GetString(2)[0] : (char)0;
                    customer.MBirthDate = reader.GetDateTime(3);
                    customer.MAddress = !reader.IsDBNull(4) ? reader.GetString(4) : null;
                    customer.MCountry = reader.GetString(5);
                    customer.MTelNo = !reader.IsDBNull(6) ? reader.GetString(6) : null;
                    customer.MEmailAddr = !reader.IsDBNull(7) ? reader.GetString(7) : null;
                    customer.MPassword = reader.GetString(8);
                }
            }
            //Close data reader
            reader.Close();
            //Close database connection
            conn.Close();
            return customer;
        }

        public int Update(CustomerEdit customer)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();

            //Specify an UPDATE SQL statement
            cmd.CommandText = @"UPDATE Customer SET MAddress = @address, MTelNo = @telno, 
                                                    MEmailAddr = @emailaddr, MPassword = @password
                                                    WHERE MemberID = @memberID";

            cmd.Parameters.AddWithValue("@memberID", customer.MemberID);
            //Define the parameters used in SQL statement, value for each parameter
            //is retrieved from respective class's property.
            if (customer.MAddress != null && customer.MAddress != "")
            {
                cmd.Parameters.AddWithValue("@address", customer.MAddress);
            }
            else
            {
                cmd.Parameters.AddWithValue("@address", (object)DBNull.Value);
            }
            if (customer.MTelNo != null && customer.MTelNo != "")
            {
                cmd.Parameters.AddWithValue("@telno", customer.MTelNo);
            }
            else
            {
                cmd.Parameters.AddWithValue("@telno", (object)DBNull.Value);
            }
            if (customer.MEmailAddr != null && customer.MEmailAddr != "")
            {
                cmd.Parameters.AddWithValue("@emailaddr", customer.MEmailAddr);
            }
            else
            {
                cmd.Parameters.AddWithValue("@emailaddr", (object)DBNull.Value);
            }
            if (customer.MPassword != null && customer.MPassword != "")
            {
                cmd.Parameters.AddWithValue("@password", customer.MPassword);
            }
            else
            {
                cmd.Parameters.AddWithValue("@password", "AbC@123#");
            }

            conn.Open();
            int count = cmd.ExecuteNonQuery();
            conn.Close();

            return count;
        }

        public List<User> GetUsers()
        {
            SqlCommand cmd = conn.CreateCommand();
            //Read database of staff - sort by StaffID
            cmd.CommandText = @"SELECT * FROM Staff ORDER BY StaffID";
            conn.Open();

            SqlDataReader staffReader = cmd.ExecuteReader();
            List<User> userList = new List<User>();

            //Add Staff - username, password and role into User List
            while (staffReader.Read())
            {
                userList.Add(
                    new User
                    {
                        Username = staffReader.GetString(0),
                        UPassword = staffReader.GetString(7),
                        URole = staffReader.GetString(4)
                    }
                    );
            }
            staffReader.Close();
            conn.Close();
            
            //Read database of customer - sort by MemberID
            cmd.CommandText = @"SELECT * FROM Customer ORDER BY MemberID";
            conn.Open();
            SqlDataReader customerReader = cmd.ExecuteReader();
            while (customerReader.Read())
            {
                userList.Add(
                    new User
                    {
                        Username = customerReader.GetString(0),
                        UPassword = customerReader.GetString(8),
                        URole = "Customer"
                    });
            }
            customerReader.Close();
            conn.Close();
            return userList;
        }

        public List<Customer> GetAllCustomer()
        {
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"SELECT * FROM Customer";
            conn.Open();

            SqlDataReader reader = cmd.ExecuteReader();

            List<Customer> customerList = new List<Customer>();
            while (reader.Read())
            {
                customerList.Add(new Customer
                {
                    MemberID = reader.GetString(0),
                    MName = reader.GetString(1),
                    MGender = reader.GetString(2)[0],
                    MBirthDate = reader.GetDateTime(3),
                    MAddress = !reader.IsDBNull(4) ? reader.GetString(4) : null,
                    MCountry = reader.GetString(5),
                    MTelNo = !reader.IsDBNull(6) ? reader.GetString(6) : null,
                    MEmailAddr = !reader.IsDBNull(7) ? reader.GetString(7) : null,
                    MPassword = reader.GetString(8)
                });
            }
            reader.Close();
            conn.Close();
            return customerList;
        }

        public List<CashVoucher> GetCashVouchersbyMemberID(string id)
        {
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"SELECT * FROM CashVoucher WHERE MemberID = @memberid";

            cmd.Parameters.AddWithValue("@memberID", id);
            conn.Open();

            SqlDataReader reader = cmd.ExecuteReader();

            List<CashVoucher> voucherList = new List<CashVoucher>();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    voucherList.Add(new CashVoucher
                    {
                        IssuingID = reader.GetInt32(0),
                        MemberID = reader.GetString(1),
                        Amount = reader.GetDecimal(2),
                        MonthIssuedFor = reader.GetInt32(3),
                        YearIssuedFor = reader.GetInt32(4),
                        DateTimeIssued = reader.GetDateTime(5),
                        VoucherSN = !reader.IsDBNull(6) ? reader.GetString(6) : null,
                        Status = reader.GetString(7)[0],
                        DateTimeRedeemed = reader.IsDBNull(8) ? (DateTime?)null : (DateTime?)reader.GetDateTime(8)
                    });
                }
            }
            reader.Close();
            conn.Close();
            return voucherList;
        }

        public List<CashVoucher> GetAllCollectible()
        {
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"SELECT * FROM CashVoucher WHERE Status = 0";
            conn.Open();

            SqlDataReader reader = cmd.ExecuteReader();

            List<CashVoucher> voucherList = new List<CashVoucher>();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    voucherList.Add(new CashVoucher
                    {
                        IssuingID = reader.GetInt32(0),
                        MemberID = reader.GetString(1),
                        Amount = reader.GetDecimal(2),
                        MonthIssuedFor = reader.GetInt32(3),
                        YearIssuedFor = reader.GetInt32(4),
                        DateTimeIssued = reader.GetDateTime(5),
                        VoucherSN = !reader.IsDBNull(6) ? reader.GetString(6) : null,
                        Status = reader.GetString(7)[0],
                        DateTimeRedeemed = reader.IsDBNull(8) ? (DateTime?)null : (DateTime?)reader.GetDateTime(8)
                });
                }
            }
            reader.Close();
            conn.Close();
            return voucherList;
        }

        public List<CashVoucher> GetAllRedeemable()
        {
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"SELECT * FROM CashVoucher WHERE Status = 1";
            conn.Open();

            SqlDataReader reader = cmd.ExecuteReader();

            List<CashVoucher> voucherList = new List<CashVoucher>();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    voucherList.Add(new CashVoucher
                    {
                        IssuingID = reader.GetInt32(0),
                        MemberID = reader.GetString(1),
                        Amount = reader.GetDecimal(2),
                        MonthIssuedFor = reader.GetInt32(3),
                        YearIssuedFor = reader.GetInt32(4),
                        DateTimeIssued = reader.GetDateTime(5),
                        VoucherSN = !reader.IsDBNull(6) ? reader.GetString(6) : null,
                        Status = reader.GetString(7)[0],
                        DateTimeRedeemed = reader.IsDBNull(8) ? (DateTime?)null : (DateTime?)reader.GetDateTime(8)
                    });
                }
            }
            reader.Close();
            conn.Close();
            return voucherList;
        }

        public string Add(Customer customer)
        {

            // Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();

            // Specify an INSERT SQL Statement which will return the auto-generated password
            cmd.CommandText = @"INSERT INTO Customer (MemberID, MName, MGender, MBirthDate, MAddress, MCountry, MTelNo, MEmailAddr, MPassword)
                                VALUES(@memberid, @name, @gender, @dob, @address, @country, @phoneNo, @email, @pass)";

            // Define the parameters used in SQL statement, value for each parameter is retrieved from respective class's property
            cmd.Parameters.AddWithValue("@memberid", customer.MemberID);
            cmd.Parameters.AddWithValue("@name", customer.MName);
            cmd.Parameters.AddWithValue("@gender", customer.MGender);
            cmd.Parameters.AddWithValue("@dob", customer.MBirthDate);
            cmd.Parameters.AddWithValue("@address", string.IsNullOrEmpty(customer.MAddress) ? (object)DBNull.Value : customer.MAddress);
            cmd.Parameters.AddWithValue("@country", customer.MCountry);
            cmd.Parameters.AddWithValue("@phoneNo", string.IsNullOrEmpty(customer.MTelNo) ? (object)DBNull.Value : customer.MTelNo);
            cmd.Parameters.AddWithValue("@email", string.IsNullOrEmpty(customer.MEmailAddr) ? (object)DBNull.Value : customer.MEmailAddr);
            cmd.Parameters.AddWithValue("@pass", "AbC@123#");

            // A connection to database must be opened before any operations made
            conn.Open();

            cmd.ExecuteScalar();

            // A connection should be closed after operations
            conn.Close();

            return customer.MemberID;
        }

        public bool IsMemberIdExist(string memberID)
        {
            bool memberIdFound = false;

            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"SELECT MemberID FROM Customer WHERE MemberID = @selectedMemberID";
            cmd.Parameters.AddWithValue("@selectedMemberID", memberID);

            conn.Open();

            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                memberIdFound = true;
            }
            else
            {
                memberIdFound = false;
            }
            reader.Close();
            conn.Close();
            return memberIdFound;
        }

        public bool IsEmailExist(string email)
        {
            bool emailFound = false;

            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"SELECT MemberID FROM Customer WHERE MEmailAddr = @selectedEmail";
            cmd.Parameters.AddWithValue("@selectedEmail", email);

            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                emailFound = true;
            }
            else
            {
                emailFound = false;
            }
            reader.Close();
            conn.Close();
            return emailFound;
        }

        public bool IsTelNoExist(string telNo)
        {
            bool telNoFound = false;

            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"SELECT MemberID FROM Customer WHERE MTelNo = @selectedTelNo";
            cmd.Parameters.AddWithValue("@selectedTelNo", telNo);

            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.HasRows)
            {
                telNoFound = true;
            }
            else
            {
                telNoFound = false;
            }
            reader.Close();
            conn.Close();
            return telNoFound;
        }
    }
}