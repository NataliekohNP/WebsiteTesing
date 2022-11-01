using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using WEB2022APR_P05_T2.Models;

namespace WEB2022APR_P05_T2.DAL
{
    public abstract class _FeedbackDAL
    {
        private IConfiguration Configuration { get; }
        public SqlConnection conn;

        public _FeedbackDAL()
        {
            var builder = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json");
            Configuration = builder.Build();
            string strConn = Configuration.GetConnectionString("FashionConnectionString");
            conn = new SqlConnection(strConn);
        }

        public virtual List<Feedback> GetFeedback()
        {
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"SELECT * FROM Feedback ORDER BY FeedbackID";

            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            List<Feedback> feedbackList = new List<Feedback>();
            while (reader.Read())
            {
                feedbackList.Add(
                    new Feedback
                    {
                        FeedbackID = reader.GetInt32(0),
                        MemberID = reader.GetString(1),
                        DateTimePosted = !reader.IsDBNull(2) ? reader.GetDateTime(2) : (DateTime?)null,
                        UserSubject = reader.GetString(3),
                        UserFeedback = reader.GetString(4),
                        ImageFileName = !reader.IsDBNull(5) ? reader.GetString(5) : null
                    }
                );
            }
            reader.Close();
            conn.Close();
            return feedbackList;
        }

        public virtual List<Response> GetResponse()
        {

            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"SELECT * FROM Response ORDER BY ResponseID";

            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            List<Response> responseList = new List<Response>();
            while (reader.Read())
            {
                responseList.Add(
                    new Response
                    {
                        ResponseID = Convert.ToInt32(0),
                        FeedbackID = reader.GetInt32(1),
                        MemberID = !reader.IsDBNull(2) ? reader.GetString(2) : null,
                        StaffID = !reader.IsDBNull(3) ? reader.GetString(3) : null,
                        DateTimePosted = reader.GetDateTime(4),
                        Text = reader.GetString(5)
                    }
                );
            }
            reader.Close();
            conn.Close();
            return responseList;
        }

        public virtual Feedback getFeedbackDetail(int FeedbackID)
        {
            Feedback feedback = new Feedback();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"SELECT * FROM Feedback WHERE FeedbackID = @selectedFeedbackID";
            cmd.Parameters.AddWithValue("@selectedFeedbackID", FeedbackID);
            conn.Open();

            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    feedback.FeedbackID = reader.GetInt32(0);
                    feedback.MemberID = reader.GetString(1);
                    feedback.DateTimePosted = reader.GetDateTime(2);
                    feedback.UserSubject = reader.GetString(3);
                    feedback.UserFeedback = reader.GetString(4);
                    feedback.ImageFileName = !reader.IsDBNull(5) ? reader.GetString(5) : null;
                }
            }
            reader.Close();
            conn.Close();
            return feedback;
        }
        public virtual Response getResponseDetails(int ResponseID)
        {
            Response response = new Response();
            SqlCommand cmd = conn.CreateCommand();
            cmd.CommandText = @"SELECT * FROM Response WHERE ResponseID = @selectedResponseID";
            cmd.Parameters.AddWithValue("@selectedResponseID", ResponseID);
            conn.Open();

            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    response.ResponseID = reader.GetInt32(0);
                    response.FeedbackID = reader.GetInt32(1);
                    response.MemberID = !reader.IsDBNull(2) ? reader.GetString(2) : null;
                    response.StaffID = reader.GetString(3);
                    response.DateTimePosted = reader.GetDateTime(4);
                    response.Text = reader.GetString(5);

                }
            }
            reader.Close();
            conn.Close();
            return response;
        }
        public virtual int addResponse(Response response)
        {
            SqlCommand cmd = conn.CreateCommand();

            cmd.CommandText = @"INSERT INTO Response (FeedbackID, MemberID, StaffID, DateTimePosted, Text) VALUES(@feedbackID, @memberID, @staffID, @dateTimePosted, @text)";
            cmd.Parameters.AddWithValue("@feedbackID", response.FeedbackID);
            cmd.Parameters.AddWithValue("@memberID", string.IsNullOrEmpty(response.MemberID) ? (object)DBNull.Value : response.MemberID);
            cmd.Parameters.AddWithValue("@staffID", string.IsNullOrEmpty(response.StaffID) ? (object)DBNull.Value : response.StaffID);
            cmd.Parameters.AddWithValue("@dateTimePosted", response.DateTimePosted);
            cmd.Parameters.AddWithValue("@text", response.Text);

            conn.Open();

            cmd.ExecuteNonQuery();
            conn.Close();
            return response.ResponseID;
        }
    }
}
