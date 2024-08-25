using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using DataNexus.Models;

namespace DataNexus.Data
{
    public class BookRepository
    {
        private readonly string _connectionString;

        public BookRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public DataTable GetBooks()
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM Books", conn))
                {
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    return dt;
                }
            }
        }

        public BookViewModel GetBookByID(int id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SELECT * FROM Books WHERE BookID = @BookID", conn))
                {
                    cmd.Parameters.AddWithValue("@BookID", id);
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new BookViewModel
                            {
                                BookID = reader.GetInt32(reader.GetOrdinal("BookID")),
                                Title = reader.GetString(reader.GetOrdinal("Title")),
                                Author = reader.GetString(reader.GetOrdinal("Author")),
                                Price = reader.GetInt32(reader.GetOrdinal("Price"))
                            };
                        }
                    }
                }
            }
            return null;
        }

        public void AddOrEditBook(BookViewModel book)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = conn;
                    if (book.BookID == 0)
                    {
                        cmd.CommandText = "INSERT INTO Books (Title, Author, Price) VALUES (@Title, @Author, @Price)";
                    }
                    else
                    {
                        cmd.CommandText = "UPDATE Books SET Title = @Title, Author = @Author, Price = @Price WHERE BookID = @BookID";
                        cmd.Parameters.AddWithValue("@BookID", book.BookID);
                    }
                    cmd.Parameters.AddWithValue("@Title", book.Title);
                    cmd.Parameters.AddWithValue("@Author", book.Author);
                    cmd.Parameters.AddWithValue("@Price", book.Price);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeleteBook(int id)
        {
            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("DELETE FROM Books WHERE BookID = @BookID", conn))
                {
                    cmd.Parameters.AddWithValue("@BookID", id);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
