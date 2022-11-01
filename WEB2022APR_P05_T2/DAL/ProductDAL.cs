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
    public class ProductDAL
    {
        private IConfiguration Configuration { get; }
        private SqlConnection conn;

        public ProductDAL()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");
            Configuration = builder.Build();
            string strConn = Configuration.GetConnectionString("FashionConnectionString");
            conn = new SqlConnection(strConn);
        }
        public List<Product> GetProduct()
        {
            SqlCommand cmd = conn.CreateCommand();
            //Read database of staff - sort by StaffID
            cmd.CommandText = @"SELECT * FROM Product ORDER BY ProductID";
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            List<Product> productlist = new List<Product>();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    productlist.Add(
                        new Product
                        {
                            ProductId = reader.GetInt32(0),
                            ProductTitle = reader.GetString(1),
                            ProductImage = !reader.IsDBNull(2) ? reader.GetString(2) : null,
                            Price = reader.GetDecimal(3),
                            EffectiveDate = !reader.IsDBNull(4) ? reader.GetDateTime(4) : (DateTime?)null,
                            Obsolete = reader.GetString(5)
                        }
                        );
                }
            }
            
            reader.Close();
            conn.Close();
            return productlist;
        }
        public Product GetProductDetail(int ProductId)
        {
            Product product = new Product();

            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"SELECT * FROM Product WHERE ProductID = @selectedProductID";
            cmd.Parameters.AddWithValue("@selectedProductID", ProductId);

            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    product.ProductId = ProductId;
                    product.ProductTitle = !reader.IsDBNull(1) ? reader.GetString(1) : null;
                    product.ProductImage = !reader.IsDBNull(2) ? reader.GetString(2) : null;
                    product.Price = !reader.IsDBNull(3) ? reader.GetDecimal(3) : (decimal) 0;
                    product.EffectiveDate = !reader.IsDBNull(4) ? reader.GetDateTime(4) : (DateTime?) null;
                    product.Obsolete = !reader.IsDBNull(4) ? reader.GetString(5) : null;
                }
            
            reader.Close();
            conn.Close();
            return product;
        }
        public int ObsoleteProduct(Product product)
        {
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"UPDATE Product SET Obsolete = 0 WHERE ProductID = @selectedProductID";
            cmd.Parameters.AddWithValue("@selectedProductID", product.ProductId);

            conn.Open();
            int count = cmd.ExecuteNonQuery();
            conn.Close();
            return count;

        }
        public int Add(Product product)
        {
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"INSERT INTO Product(ProductTitle, ProductImage, Price, EffectiveDate, Obsolete)OUTPUT INSERTED.ProductID VALUES(@ProductTitle, @ProductImage, @Price, @EffectiveDate, @Obsolete)";
            String fileext =  product.ProductTitle + Path.GetExtension(product.filetoupload.FileName);
            cmd.Parameters.AddWithValue("ProductTitle", product.ProductTitle);        
            cmd.Parameters.AddWithValue("ProductImage", fileext);
            cmd.Parameters.AddWithValue("Price", product.Price);
            cmd.Parameters.AddWithValue("EffectiveDate", DateTime.Now);
            cmd.Parameters.AddWithValue("Obsolete", product.Obsolete);

            conn.Open();

            product.ProductId = (int)cmd.ExecuteScalar();
            conn.Close();
            return product.ProductId;

        }

        public int Update(Product product)
        {
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"UPDATE Product SET ProductTitle=@producttitle, Price = @price, ProductImage = @productimage WHERE ProductID = @selectedProductID";
            cmd.Parameters.AddWithValue("@producttitle", product.ProductTitle);
            cmd.Parameters.AddWithValue("@price", product.Price);
            String fileext = product.ProductTitle + Path.GetExtension(product.filetoupload.FileName);
            cmd.Parameters.AddWithValue("@productimage", fileext);
            cmd.Parameters.AddWithValue("@selectedProductID", product.ProductId);


            conn.Open();
            int count = cmd.ExecuteNonQuery();
            conn.Close();
            return count;
            

        }
        public int Update2(Product product)
        {
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"UPDATE Product SET ProductTitle=@producttitle, Price = @price WHERE ProductID = @selectedProductID";
            cmd.Parameters.AddWithValue("@producttitle", product.ProductTitle);
            cmd.Parameters.AddWithValue("@price", product.Price);
            cmd.Parameters.AddWithValue("@selectedProductID", product.ProductId);

            conn.Open();
            int count = cmd.ExecuteNonQuery();
            conn.Close();
            return count;


        }

    }
}
