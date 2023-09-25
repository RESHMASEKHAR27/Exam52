using Exam52.Models;
using Microsoft.AspNetCore.Mvc;
using MimeKit;
using System.Diagnostics;
using MailKit.Net.Smtp;
using MailKit.Security;
using System.Net.Mail;

namespace _52final_ques.Controllers
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
        [HttpPost]
        public IActionResult Index(Login login)
        {
            Random rnd = new Random();
            string OTP = rnd.Next(100000, 999999).ToString();
            TempData["DataToPass"] = OTP;
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse("sruthimahadevan03@gmail.com"));
            email.To.Add(MailboxAddress.Parse(login.Email));
            email.Subject = "Automated Password";
            email.Body = new TextPart(MimeKit.Text.TextFormat.Plain)
            {
                Text = TempData["DataToPass"] as string
            };
            var smtp = new MailKit.Net.Smtp.SmtpClient();
            smtp.Connect("smtp.gmail.com", 587, SecureSocketOptions.StartTls);
            smtp.Authenticate("reshmasekhar27@gmail.com", "isdaelbbtbrgfkun");
            smtp.Send(email);
            smtp.Disconnect(true);
            return RedirectToAction("Privacy");

        }

        public IActionResult Privacy()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Privacy(Login login)
        {
            string tempspass = TempData["DataToPass"] as string;
            if (tempspass != null)
            {
                if (login.Password == tempspass && login.UserName != null)
                {
                    CookieOptions options = new CookieOptions();
                    options.Secure = true;
                    options.Expires = DateTimeOffset.Now.AddMinutes(5);
                    HttpContext.Response.Cookies.Append("cookie", login.UserName.ToString(), options);
                    return RedirectToAction("Success");

                }
                else
                {
                    return View();
                }
            }
            return View();
        }
        public IActionResult Success()
        {
            string cookie = HttpContext.Request.Cookies["Cookie"];
            if (cookie != null)
            {
                Login login = new Login();
                login.UserName = cookie;
                if (cookie != null)
                {
                    ViewBag.status = login.UserName;
                    return View();
                }
            }
            return View();
        }
        public IActionResult Logout()
        {
            HttpContext.Response.Cookies.Delete("Cookie");
            return View("Index");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
