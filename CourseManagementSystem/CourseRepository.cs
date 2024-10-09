using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CourseManagementSystem
{
    public class CourseRepository
    {
        private string connectionString = @"Server=(localdb)\MSSQLLocalDB;Database=master;Trusted_Connection=True;";

        public void InitializeDatabase()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string createDatabaseQuery = "IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'CourseManagement') " +
                                              "BEGIN CREATE DATABASE CourseManagement END";
                string createTableQuery = @"
                    USE CourseManagement;
                    IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Courses')
                    BEGIN
                        CREATE TABLE Courses (
                            CourseId INT IDENTITY(1,1) PRIMARY KEY,
                            Title NVARCHAR(255) NOT NULL,
                            Duration NVARCHAR(100) NOT NULL,
                            Price DECIMAL(18, 2) NOT NULL
                        );
                    END";

                conn.Open();

                using (SqlCommand cmd = new SqlCommand(createDatabaseQuery, conn))
                {
                    cmd.ExecuteNonQuery();
                }

                using (SqlCommand cmd = new SqlCommand(createTableQuery, conn))
                {
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void AddCourse(Course course)
        {
            using (SqlConnection conn = new SqlConnection(@"Server=(localdb)\MSSQLLocalDB;Database=CourseManagement;Trusted_Connection=True;"))
            {
                string query = "INSERT INTO Courses (Title, Duration, Price) VALUES (@Title, @Duration, @Price)";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Title", CapitalizeTitle(course.Title));
                    cmd.Parameters.AddWithValue("@Duration", course.Duration);
                    cmd.Parameters.AddWithValue("@Price", course.Price);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<Course> GetAllCourses()
        {
            List<Course> courses = new List<Course>();
            using (SqlConnection conn = new SqlConnection(@"Server=(localdb)\MSSQLLocalDB;Database=CourseManagement;Trusted_Connection=True;"))
            {
                string query = "SELECT * FROM Courses";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            courses.Add(new Course(
                                reader.GetInt32(0),
                                reader.GetString(1),
                                reader.GetString(2),
                                reader.GetDecimal(3)));
                        }
                    }
                }
            }
            return courses;
        }

        public void UpdateCourse(Course course)
        {
            using (SqlConnection conn = new SqlConnection(@"Server=(localdb)\MSSQLLocalDB;Database=CourseManagement;Trusted_Connection=True;"))
            {
                string query = "UPDATE Courses SET Title = @Title, Duration = @Duration, Price = @Price WHERE CourseId = @CourseId";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@CourseId", course.CourseId);
                    cmd.Parameters.AddWithValue("@Title", CapitalizeTitle(course.Title));
                    cmd.Parameters.AddWithValue("@Duration", course.Duration);
                    cmd.Parameters.AddWithValue("@Price", course.Price);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void DeleteCourse(int courseId)
        {
            using (SqlConnection conn = new SqlConnection(@"Server=(localdb)\MSSQLLocalDB;Database=CourseManagement;Trusted_Connection=True;"))
            {
                string query = "DELETE FROM Courses WHERE CourseId = @CourseId";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@CourseId", courseId);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public Course GetCourseById(int courseId)
        {
            using (SqlConnection conn = new SqlConnection(@"Server=(localdb)\MSSQLLocalDB;Database=CourseManagement;Trusted_Connection=True;"))
            {
                string query = "SELECT * FROM Courses WHERE CourseId = @CourseId";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@CourseId", courseId);
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Course(
                                reader.GetInt32(0),
                                reader.GetString(1),
                                reader.GetString(2),
                                reader.GetDecimal(3));
                        }
                    }
                }
            }
            return null;
        }

        public string CapitalizeTitle(string title)
        {
            if (string.IsNullOrEmpty(title))
                return title;

            var words = title.Split(' ');
            for (int i = 0; i < words.Length; i++)
            {
                if (words[i].Length > 0)
                {
                    words[i] = char.ToUpper(words[i][0]) + words[i].Substring(1).ToLower();
                }
            }
            return string.Join(" ", words);
        }

        public bool ValidateCoursePrice(decimal price)
        {
            if (price <= 0)
            {
                Console.WriteLine("Price must be a positive value. Please enter a valid price:");
                return false;
            }
            return true;
        }
    }
}
