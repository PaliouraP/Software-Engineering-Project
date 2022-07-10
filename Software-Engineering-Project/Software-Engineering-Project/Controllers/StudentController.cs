using Microsoft.AspNetCore.Mvc;
using Npgsql;
using Software_Engineering_Project.Models;
using System.IO.Compression;

namespace Software_Engineering_Project.Controllers
{
    public class StudentController : Controller
    {
        public IActionResult StudentHome(string Username)
        {
            ViewBag.Username = Username;
            return View();
        }

        public IActionResult Upload(string Username) 
        {
            ViewBag.Username = Username;
            ViewBag.Success = false;
            NpgsqlConnection conn = Database.Database.GetConnection();
            NpgsqlDataReader result = Database.Database.ExecuteQuery(String.Format("select upload1,upload2,upload3 from student where student='{0}';", Username), conn);
            if (result.Read())
            {
                if ((byte[])result[0] != null)
                {
                    ViewBag.version1 = false;
                }
                else if ((byte[])result[1] != null)
                {
                    ViewBag.version2 = false;
                }
                else if((byte[])result[2] != null)
                {
                    ViewBag.version3 = false;
                }
            }
                return View();
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upload(FileModel model)
        { 
            MemoryStream target = new MemoryStream();
            model.File.CopyTo(target); 
            byte[] bytes = target.ToArray();
            NpgsqlConnection conn1 = Database.Database.GetConnection();
            
            if (model.Title != null)
            {

            }
            


            int result1 = Database.Database.ExecuteUpdate(String.Format("update student set upload"+ model.version +"= '{0}' where student = '{1}';", bytes, model.Username), conn1);
            if (result1 != 0)
            {
                conn1.Close();
                ViewBag.Success = true;
                ViewBag.Username = model.Username;
                NpgsqlConnection conn = Database.Database.GetConnection();
                NpgsqlDataReader result = Database.Database.ExecuteQuery(String.Format("select upload1,upload2,upload3 from student where student='{0}';", model.Username), conn);
                if (result.Read())
                {
                    if ((byte[])result[0] != null)
                    {
                        ViewBag.version1 = false;
                    }
                    else if ((byte[])result[1] != null)
                    {
                        ViewBag.version2 = false;
                    }
                    else if ((byte[])result[2] != null)
                    {
                        ViewBag.version3 = false;
                    }
                }
                return View();
            }
            conn1.Close();
            ViewBag.Success = false;
            ViewBag.Username = model.Username;

            NpgsqlConnection conn2 = Database.Database.GetConnection();
            NpgsqlDataReader result2 = Database.Database.ExecuteQuery(String.Format("select upload1,upload2,upload3 from student where student='{0}';", model.Username), conn2);
            if (result2.Read())  
            {
                if ((byte[]) result2[0] != null)
                {
                    ViewBag.version1 = false;
                }
                else if ((byte[])result2[0] != null)
                {
                    ViewBag.version2 = false;
                }
                else if ((byte[])result2[0] != null)
                {
                    ViewBag.version3 = false;
                }
            }

            return View();
        }

        public IActionResult ThesisStatus(string Username)
        {
            StudentModel model = new();
            NpgsqlConnection conn = Database.Database.GetConnection();
            NpgsqlDataReader reader = Database.Database.ExecuteQuery(String.Format("select title," +
                " thesis_start_date, professor, grade, language, technology from thesis where student = '{0}'; ", Username), conn);

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

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ChangePassword(string oldPassword, string newPassword, string newPassword1, string Username)
        {
            ViewBag.wrongPassword = 0;
            ViewBag.notSamePasswords = 0;
            ViewBag.success = 0;
            ViewBag.failure = 0;
            ViewBag.emptyPassword = 0;

            string dbPassword = "";

            NpgsqlConnection conn = Database.Database.GetConnection();
            NpgsqlDataReader reader = Database.Database.ExecuteQuery(String.Format("select username, password " +
                                                        "from users where username='{0}'", Username), conn);
            while (reader.Read())
            {
                dbPassword = reader.GetString(1);
            }
            conn.Close();

            if (dbPassword != oldPassword)
            {
                ViewBag.wrongPassword = 1;
            }
            else if (newPassword == null || newPassword == "" || newPassword.Length == newPassword.Count(f => (f == (char)32)))
            {
                ViewBag.emptyPassword = 1;
            }
            else
            {
                if (newPassword != newPassword1)
                {
                    ViewBag.notSamePasswords = 1;
                }
                else
                {
                    int result = Database.Database.ExecuteUpdate(String.Format("update users set password='{0}'" +
                        " where username='{1}'", newPassword.Trim(), Username), conn);
                    if (result == 1)
                    {
                        ViewBag.success = 1;
                    }
                    else
                    {
                        ViewBag.failure = 1;
                    }
                }
            }
            conn.Close();
            StudentModel model = new();
            model.Username = Username;
            return View("Profile", model);
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ChangePhone(string phone, string Username)
        {
            if (phone == null)
            {
                ViewBag.failure = 1;
            }
            else
            {
                NpgsqlConnection conn = Database.Database.GetConnection();
                int result = Database.Database.ExecuteUpdate(String.Format("update users set phone='{0}' " +
                                                        "where username='{1}'", phone, Username), conn);
                if (result == 1)
                {
                    ViewBag.success = 1;
                }
                else
                {
                    ViewBag.failure = 1;
                }
                conn.Close();
            }
            StudentModel model = new();
            model.Username = Username;
            return View("Profile", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SetPassword(string oldPassword, string newPassword, string newPassword1, string Username)
        {
            ViewBag.wrongPassword = 0;
            ViewBag.notSamePasswords = 0;
            ViewBag.success = 0;
            ViewBag.failure = 0;
            ViewBag.emptyPassword = 0;

            string dbPassword = "";

            NpgsqlConnection conn = Database.Database.GetConnection();
            NpgsqlDataReader reader = Database.Database.ExecuteQuery(String.Format("select username, password " +
                                                        "from users where username='{0}'", Username), conn);
            while (reader.Read())
            {
                dbPassword = reader.GetString(1);
            }
            conn.Close();

            if (dbPassword != oldPassword)
            {
                ViewBag.wrongPassword = 1;
            }
            else if (newPassword == null || newPassword == "" || newPassword.Length == newPassword.Count(f => (f == (char)32)))
            {
                ViewBag.emptyPassword = 1;
            }
            else
            {
                if (newPassword != newPassword1)
                {
                    ViewBag.notSamePasswords = 1;
                }
                else
                {
                    int result = Database.Database.ExecuteUpdate(String.Format("update users set password='{0}'" +
                        " where username='{1}'", newPassword.Trim(), Username), conn);
                    conn.Close();
                    NpgsqlConnection conn1 = Database.Database.GetConnection();
                    int result1 = Database.Database.ExecuteUpdate(String.Format("update student set has_ever_connected='true'" +
                        " where student='{1}'", newPassword.Trim(), Username), conn1);
                    conn.Close();

                    if (result == 1 && result1 ==1)
                    {
                        return View("StudentHome", Username);
                    }
                    else
                    {
                        ViewBag.failure = 1;
                    }
                }
            }
                        return View("SetPassword", Username);
        }
    }
}
