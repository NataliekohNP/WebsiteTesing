using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using WEB2022APR_P05_T2.Models;

namespace WEB2022APR_P05_T2.DAL
{
    public class CustomerFeedbackDAL : _FeedbackDAL
    {
        public override List<Feedback> GetFeedback()
        {
            return base.GetFeedback();
        }

        public List<Feedback> GetFeedbackbyID(string memberId)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();

            //Specify the SELECT SQL statement that
            //retrieves all attributes of a customer record.
            cmd.CommandText = @"SELECT * FROM Feedback
                                WHERE MemberID = @memberId";

            //Define the parameter used in SQL statement, value for the
            //parameter is retrieved from the method parameter “customerID”.

            cmd.Parameters.AddWithValue("@memberId", memberId);

            List<Feedback> feedbackList = new List<Feedback>();

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
            }
            //Close data reader
            reader.Close();
            //Close database connection
            conn.Close();
            return feedbackList;
        }

        public List<Response> GetResponsebyFeedbackID(string feedbackID)
        {
            //Create a SqlCommand object from connection object
            SqlCommand cmd = conn.CreateCommand();

            //Specify the SELECT SQL statement that
            //retrieves all attributes of a customer record.
            cmd.CommandText = @"SELECT * FROM Response
                                WHERE FeedbackID = @FeedbackID ORDER BY DateTimePosted DESC";

            //Define the parameter used in SQL statement, value for the
            //parameter is retrieved from the method parameter “customerID”.

            cmd.Parameters.AddWithValue("@FeedbackID", feedbackID);

            List<Response> responseList = new List<Response>();

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
                    responseList.Add(
                       new Response
                       {
                           ResponseID = reader.GetInt32(0),
                           FeedbackID = reader.GetInt32(1),
                           MemberID = !reader.IsDBNull(2) ? reader.GetString(2) : (String?)null,
                           StaffID = !reader.IsDBNull(3) ? reader.GetString(3) : (String?)null,
                           DateTimePosted = reader.GetDateTime(4),
                           Text = reader.GetString(5)
                       }
                   );
                }
            }
            //Close data reader
            reader.Close();
            //Close database connection
            conn.Close();
            return responseList;
        }

        public int addFeedback(Feedback feedback)
        {
            SqlCommand cmd = conn.CreateCommand();

            cmd.CommandText = @"INSERT INTO Feedback (MemberID, DateTimePosted, Title, Text, ImageFileName) VALUES(@memberID, @dateTimePosted, @title, @text, @ImageFileName)";
            cmd.Parameters.AddWithValue("@memberID", feedback.MemberID);
            cmd.Parameters.AddWithValue("@dateTimePosted", feedback.DateTimePosted);
            cmd.Parameters.AddWithValue("@title", feedback.UserSubject);
            cmd.Parameters.AddWithValue("@text", feedback.UserFeedback);
            cmd.Parameters.AddWithValue("@ImageFileName", string.IsNullOrEmpty(feedback.ImageFileName) ? (object)DBNull.Value : feedback.ImageFileName.ToLower());

            conn.Open();

            cmd.ExecuteNonQuery();
            conn.Close();
            return feedback.FeedbackID;
        }
    }
}
