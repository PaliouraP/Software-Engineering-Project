using Microsoft.AspNetCore.Mvc;
using Npgsql;
using Software_Engineering_Project.Models;
using System.Diagnostics;

namespace Software_Engineering_Project.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        // Login page view method
        //GET
        public IActionResult Login()
        {
            return View();
        }

        //POST
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginModel model)
        {
            string username = model.Username;
            string password = model.Password;

            NpgsqlConnection conn = Database.Database.GetConnection();
            NpgsqlDataReader reader = Database.Database.ExecuteQuery(String.Format("select username" +
                ", password , role from users where username = '{0}'", username),conn);

            if (reader.Read())
            {
                string dbPassword = reader.GetString(1);
                string role = reader.GetString(2);

                conn.Close();

                if (dbPassword == password)
                {
                    if(role == "professor")
                    {
                        return View("~/Views/Teacher/TeacherHome.cshtml", model);
                    }
                    else
                    {
                        NpgsqlConnection new_conn = Database.Database.GetConnection();
                        NpgsqlDataReader new_reader = Database.Database.ExecuteQuery(String.Format("select has_ever_connected" + 
                            " from student where student = '{0}'", username), new_conn);
                        if (new_reader.Read())
                        {
                            bool has_connected = new_reader.GetBoolean(0);
                            if (has_connected) 
                            {
                                return View("~/Views/Student/StudentHome.cshtml", model.Username);
                            }
                            else
                            {
                                return View("~/Views/Student/SetPassword.cshtml", model.Username);
                            }
                        }
                       
                    }
                }
            }
            model.IsLoginConfirmed = false;
            return View("Login", model);
        }
    }
}