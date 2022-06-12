using Microsoft.AspNetCore.Mvc;
using Npgsql;
using Software_Engineering_Project.Models;

namespace Software_Engineering_Project.Controllers
{
    public class StudentController : Controller
    {
        //public IActionResult StudentHome(string Username)
        //{
        //    ViewBag.Username = Username;
        //    return View();
        //}

        public IActionResult Upload(string Username) 
        { 
            return View();
        }

        public IActionResult ThesisStatus(string Username)
        {
            StudentModel model = new();
            NpgsqlConnection conn = Database.Database.GetConnection();
            NpgsqlDataReader reader = Database.Database.ExecuteQuery(String.Format("select title, thesis_start_date, professor, grade, language, technology from thesis where student = '{0}'; ", Username), conn);

            while (reader.Read())
            {
                model.Username = Username;
                model.FirstName = reader.GetString(0);
                model.LastName = reader.GetString(1);
                model.Gender = reader.GetString(2);
                model.Email = reader.GetString(3);
                model.Phone = reader.GetDecimal(4).ToString();
                model.StartYear = reader.GetInt32(5);
                model.Professor = reader.GetString(6);
            }
            conn.Close();
            return View();
        }

        public IActionResult Profile(string Username) 
        {
            StudentModel model = new();
            NpgsqlConnection conn = Database.Database.GetConnection();
            NpgsqlDataReader reader = Database.Database.ExecuteQuery(String.Format("select users.first_name, users.last_name," + 
                " users.gender, users.email, users.phone, student.start_year, student.professor" + 
                " from student join users on student.student = users.username and users.username = '{0}'; ", Username), conn);

            while (reader.Read())
            {
                model.Username = Username;
                model.FirstName = reader.GetString(0);
                model.LastName = reader.GetString(1);
                model.Gender = reader.GetString(2);
                model.Email = reader.GetString(3);
                model.Phone = reader.GetDecimal(4).ToString();
                model.StartYear = reader.GetInt32(5);
                model.Professor = reader.GetString(6);
            }
            conn.Close();
            return View(model);
        }

    }
}
