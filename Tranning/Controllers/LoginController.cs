using Microsoft.AspNetCore.Mvc;
using Tranning.Models;
using Tranning.Queries;

namespace Tranning.Controllers
{
    public class LoginController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        { 
            LoginModel model = new LoginModel();
            return View(model);
        }

        [HttpPost]
        public IActionResult Index(LoginModel model)
        {
            //model = new LoginQueries().CheckLoginUser(model.Username, model.Password);
            if (string.IsNullOrEmpty(model.Password) || string.IsNullOrEmpty(model.Username))
            {
                // dang nhap linh tinh - khong dung tai khoan trong database
                ViewData["MessageLogin"] = "Account invalid";
                return View(model);
            }
            if(!model.Username.Trim().ToLower().Equals("admin") && !model.Password.Trim().ToLower().Equals("123456"))
            {
                ViewData["MessageLogin"] = "Thông tin tài khoản hoặc mật khẩu không chính xác";
                return View(model);
            }
            model.UserID = model.Username;
            model.RoleID = "admin";
            model.EmailUser = "admin@gmail.com";
            // luu thong tin cua nguoi dung vao session
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("SessionUserID")))
            {
                HttpContext.Session.SetString("SessionUserID",model.UserID);
                HttpContext.Session.SetString("SessionRoleID", model.RoleID);
                HttpContext.Session.SetString("SessionUsername", model.Username);
                HttpContext.Session.SetString("SessionEmail", model.EmailUser);

            }
            // cho chuyen vao trang home
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [HttpPost]
        public IActionResult Logout()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("SessionUserID")))
            {
                // xoa cac session da dc tao ra
                HttpContext.Session.Remove("SessionUserID");
                HttpContext.Session.Remove("SessionRoleID");
                HttpContext.Session.Remove("SessionUsername");
                HttpContext.Session.Remove("SessionEmail");
            }
            return RedirectToAction(nameof(LoginController.Index), "Login");
        }
    }
}
