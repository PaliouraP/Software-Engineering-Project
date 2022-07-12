using Microsoft.AspNetCore.Mvc;
using Npgsql;
using Software_Engineering_Project.Models;

namespace Software_Engineering_Project.Controllers
{
    public class TeacherController : Controller
    {
        public IActionResult TeacherHome(string username)
        {
            ViewBag.Username = username;
            return View();
        }

        //GET
        public IActionResult StudentHandler(string username)
        {
            ViewBag.Username = username;
            return View();
        }

        public IActionResult AddMeeting(string username)
        {
            ViewBag.Username = username;
            return View();
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddMeeting(MeetingModel model, string proffesorName)
        {
            model.Professor = proffesorName;
            if (ModelState.IsValid)
            {
                NpgsqlConnection conn = Database.Database.GetConnection();
                int result = Database.Database.ExecuteUpdate(String.Format("insert into meeting (professor, student, type" +
                    ", duration, title, meet_date) values ('{0}','{1}','{2}','{3}','{4}','{5}');",
                   model.Professor, model.Student, model.Type, model.Duration, model.Title, model.DateTime.ToString("yyyy-M-dd hh:mm:ss")), conn);
                if (result != 0)
                {
                    conn.Close();
                    ViewBag.Success = true;
                    return View();
                }
                conn.Close();
            }
            ViewBag.Success = false;
            return View();

        }

        //GET
        public IActionResult AddStudent(string username)
        {
            ViewBag.Username = username;
            return View();
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddStudent(StudentModel model, string proffesorName)
        {
            model.Professor = proffesorName;
            if (ModelState.IsValid)
            {
                NpgsqlConnection conn = Database.Database.GetConnection();
                int result = Database.Database.ExecuteUpdate(String.Format("insert into users (username , password,first_name,last_name" +
                    ",gender,email,phone,role) values ('{0}','{1}','{2}','{3}','{4}','{5}',{6},'{7}');" +
                    " insert into student (student , start_year, professor) values ('{8}', {9}, '{10}');" +
                    " insert into thesis (professor, student) values ('{10}', '{0}');",
                    model.Username, model.Password, model.FirstName, model.LastName, model.Gender, model.Email, model.Phone,
                    model.Role, model.Username, model.StartYear, model.Professor), conn);
                if (result != 0)
                {
                    conn.Close();
                    ViewBag.Success = true;
                    return View();
                }
                conn.Close();
            }
            ViewBag.Success = false;
            return View();

        }

        public IActionResult SearchMeetingStudent(string Username)
        {
            List<SearchModel> searchModels = new List<SearchModel>();

            NpgsqlConnection conn = Database.Database.GetConnection();

            NpgsqlDataReader reader = Database.Database.ExecuteQuery(String.Format("SELECT student, first_name, last_name" +
                "FROM student NATURAL JOIN users where professor = 'professor'"), conn);

            while (reader.Read())
            {
                SearchModel model = new SearchModel();
                model.Student = reader.GetString(0);
                model.FirstName = reader.GetString(1);
                model.LastName = reader.GetString(2);

                searchModels.Add(model);
            }
            conn.Close();
            return View("AddMeeting", searchModels);

        }

        //GET
        public IActionResult Meeting(string Username)
        {
            ViewBag.Username = Username;
            return View("Meeting");
        }


        //POST
        public IActionResult SearchMeeting(string selected_month, string selected_day, string Username)
        {
            List<MeetingModel> meetingModels = new List<MeetingModel>();

            NpgsqlConnection conn = Database.Database.GetConnection();

            NpgsqlDataReader reader = Database.Database.ExecuteQuery(String.Format("SELECT student, " +
                "type, duration, title, meet_date FROM meeting WHERE professor='{0}' "
                + "and EXTRACT(month FROM meet_date)='{2}' and EXTRACT(day FROM meet_date)='{1}'", Username, selected_day, selected_month), conn);

            while (reader.Read())
            {
                MeetingModel model = new MeetingModel();
                model.Student = reader.GetString(0);
                model.Type = reader.GetString(1);
                model.Duration = reader.GetString(2);
                model.Title = reader.GetString(3);
                model.DateTime = reader.GetDateTime(4);

                meetingModels.Add(model);
            }
            ViewBag.popup = true;
            ViewBag.Username = Username;
            conn.Close();
            return View("Meeting", meetingModels);
        }
    


    public IActionResult StudentSearch(string queryString, string professorName)
        {
            List<SearchModel> searchModels = new List<SearchModel>();

            NpgsqlConnection conn = Database.Database.GetConnection();

            if (queryString.Contains(' '))
            {
                string firstName = queryString.Substring(0, queryString.IndexOf(' '));
                string lastName = queryString.Substring(queryString.IndexOf(' ') + 1);

                NpgsqlDataReader reader = Database.Database.ExecuteQuery(String.Format("select student" +
                    ", start_year, first_name, last_name, email, phone, title, thesis_start_date, " +
                    "grade, language, technology from thesis natural join (select student, start_year," +
                    " first_name, last_name, email, phone from student as s join users as u on " +
                    "s.student = u.username where first_name='{0}' and last_name='{1}' " +
                    "and professor='{2}') as student", firstName, lastName, professorName), conn);

                while (reader.Read())
                {
                    SearchModel model = new SearchModel();
                    model.Student = reader.GetString(0);
                    model.StartYear = reader.GetInt32(1);
                    model.FirstName = reader.GetString(2);
                    model.LastName = reader.GetString(3);
                    model.Email = reader.GetString(4);
                    model.Phone = reader.GetDecimal(5).ToString();
                    model.Title = reader.GetString(6);
                    model.StartDate = (DateOnly)reader.GetDate(7);
                    model.Grade = reader.GetInt32(8);
                    model.Language = reader.GetString(9);
                    model.Technology = reader.GetString(10);
                    searchModels.Add(model);
                }
                conn.Close();
            }
            else
            {
                NpgsqlDataReader reader = Database.Database.ExecuteQuery(String.Format("select student" +
                    ", start_year, first_name, last_name, email, phone, title, thesis_start_date, " +
                    "grade, language, technology from thesis natural join (select student, start_year," +
                    " first_name, last_name, email, phone from student as s join users as u on " +
                    "s.student = u.username where student='{0}' " +
                    "and professor='{1}') as student", queryString, professorName), conn);

                while (reader.Read())
                {
                    SearchModel model = new SearchModel();
                    model.Student = reader.GetString(0);
                    model.StartYear = reader.GetInt32(1);
                    model.FirstName = reader.GetString(2);
                    model.LastName = reader.GetString(3);
                    model.Email = reader.GetString(4);
                    model.Phone = reader.GetDecimal(5).ToString();
                    model.Title = reader.GetString(6);
                    model.StartDate = (DateOnly)reader.GetDate(7);
                    model.Grade = reader.GetInt32(8);
                    model.Language = reader.GetString(9);
                    model.Technology = reader.GetString(10);
                    searchModels.Add(model);
                }
                conn.Close();
            }
            return View(searchModels);
        }

        //GET
        public IActionResult Profile(string username)
        {
            ViewBag.Username = username;
            return View();
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ChangePassword(string oldPassword, string newPassword, string newPassword1, string professorName)
        {
            ViewBag.wrongPassword = 0;
            ViewBag.notSamePasswords = 0;
            ViewBag.success = 0;
            ViewBag.failure = 0;
            ViewBag.emptyPassword = 0;

            string dbPassword = "";

            NpgsqlConnection conn = Database.Database.GetConnection();
            NpgsqlDataReader reader = Database.Database.ExecuteQuery(String.Format("select username, password " +
                                                        "from users where username='{0}'", professorName), conn);
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
                        " where username='{1}'", newPassword.Trim(), professorName), conn);
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
            ViewBag.Username = professorName;
            return View("Profile");
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ChangePhone(string phone, string professorName)
        {
            if (phone == null)
            {
                ViewBag.failure = 1;
            }
            else
            {
                NpgsqlConnection conn = Database.Database.GetConnection();
                int result = Database.Database.ExecuteUpdate(String.Format("update users set phone='{0}' " +
                                                        "where username='{1}'", phone, professorName), conn);
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
            ViewBag.Username = professorName;
            return View("Profile");
        }

        public IActionResult GradeList(string username)
        {
            List<GradeModel> models = new();

            NpgsqlConnection conn = Database.Database.GetConnection();
            NpgsqlDataReader reader = Database.Database.ExecuteQuery(String.Format("select first_name, last_name, grade " +
                "from users as u join(select student, grade from thesis where professor='{0}' order by grade desc) as foo" +
                " on u.username = foo.student", username), conn);

            while (reader.Read())
            {
                GradeModel model = new();
                model.Professor = username;
                model.FirstName = reader.GetString(0);
                model.LastName = reader.GetString(1);
                model.Grade = reader.GetInt32(2);
                models.Add(model);
            }
            conn.Close();
            return View(models);
        }

        public IActionResult ThesisStartList(string username)
        {
            List<ThesisModel> models = new();

            NpgsqlConnection conn = Database.Database.GetConnection();
            NpgsqlDataReader reader = Database.Database.ExecuteQuery(String.Format("select first_name, last_name, thesis_start_date " +
                "from users as u join(select student, thesis_start_date from thesis where professor='{0}' " +
                "order by thesis_start_date) as foo on u.username = foo.student", username), conn);

            while (reader.Read())
            {
                ThesisModel model = new();
                model.Professor = username;
                model.FirstName = reader.GetString(0);
                model.LastName = reader.GetString(1);
                if (model.ThesisStartDate != null)
                {
                    model.ThesisStartDate = (DateOnly)reader.GetDate(2);
                }
                else
                {
                    model.ThesisStartDate = new DateOnly();
                }
                
                models.Add(model);
            }
            conn.Close();
            return View(models);
        }
    }
}
