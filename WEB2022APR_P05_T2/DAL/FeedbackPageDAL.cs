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
    public class FeedbackPageDAL : _FeedbackDAL
    {
        private IConfiguration Configuration { get; }
        private SqlConnection conn;

        public FeedbackPageDAL()
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json");
            Configuration = builder.Build();
            string strConn = Configuration.GetConnectionString("FashionConnectionString");
            conn = new SqlConnection(strConn);
        }
    }
}
