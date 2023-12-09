using Microsoft.AspNetCore.Mvc;
using Tranning.DataDBContext;

namespace Tranning.Controllers
{
    public class UserController : Controller
    {
        private readonly TranningDBContext _dbContext;
        public UserController(TranningDBContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IActionResult Index()
        {
            List<Users> model = new List<Users>();
            var data = _dbContext.Users.Where(x => x.type == 1).ToList();
            if (data != null && data.Count > 0)
            {
                model = data;
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult CreateOrEdit(long id)
        {
            Users model = new Users();
            var data = _dbContext.Users.Find(id);
            if (data != null)
            {
                model = data;
            }
            return View(model);
        }
        [HttpPost]
        public IActionResult CreateOrEdit(Users model)
        {
            try
            {
                if (model.id == 0)
                {
                    if (string.IsNullOrEmpty(model.password))
                    {
                        ViewBag.Error = "Vui lòng nhập mật khẩu";
                        return View(model);
                    }
                    model.created_at = DateTime.Now;
                    _dbContext.Users.Add(model);
                }
                else
                {
                    var entity = _dbContext.Users.Find(model.id);
                    if (entity != null)
                    {
                        entity.password = model.password;
                        entity.updated_at = DateTime.Now;
                        entity.full_name = model.full_name;
                        entity.email = model.email;
                        entity.phone = model.phone;
                        entity.address = model.address; 
                        entity.gender = model.gender;   
                        entity.birthday = model.birthday;
                    }
                    else
                    {
                        ViewBag.Error = "Cập nhật thông tin không thành công";
                        return View(model);
                    }
                }
                _dbContext.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Có lỗi xảy ra. Vui lòng thử lại";
                return View(model);
            }
        }
        [HttpGet]
        public IActionResult Delete(long id)
        {
            try
            {
                var data = _dbContext.Users.Find(id);
                if (data != null)
                {
                    // Use Remove method to delete the entity
                    _dbContext.Users.Remove(data);
                    _dbContext.SaveChanges();
                    TempData["DeleteUser"] = true;
                }
                else
                {
                    TempData["DeleteUser"] = false;
                }
            }
            catch (Exception ex)
            {
                TempData["DeleteUser"] = false;
            }
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Trainee(string? SearchString)
        {
            List<Users> model = new List<Users>();
            var data = _dbContext.Users.Where(x => x.type == 2).ToList();
            if (data != null && data.Count > 0)
            {
                if (!string.IsNullOrEmpty(SearchString))
                {
                    model = data.Where(x => x.full_name.ToLower().Contains(SearchString.Trim().ToLower()) || x.programming_language.Contains(SearchString.Trim())).ToList();
                }
                else
                {
                    model = data;
                }
                ViewData["CurrentFilter"] = SearchString;
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult CreateOrEditTraniee(long id)
        {
            Users model = new Users();
            var data = _dbContext.Users.Find(id);
            if (data != null)
            {
                model = data;
            }
            return View(model);
        }
        [HttpPost]
        public IActionResult CreateOrEditTraniee(Users model)
        {
            try
            {
                if (model.id == 0)
                {
                    if (string.IsNullOrEmpty(model.password))
                    {
                        ViewBag.Error = "Vui lòng nhập mật khẩu";
                        return View(model);
                    }
                    model.created_at = DateTime.Now;
                    _dbContext.Users.Add(model);
                }
                else
                {
                    var entity = _dbContext.Users.Find(model.id);
                    if (entity != null)
                    {
                        entity.password = model.password;
                        entity.updated_at = DateTime.Now;
                        entity.full_name = model.full_name;
                        entity.email = model.email;
                        entity.phone = model.phone;
                        entity.address = model.address;
                        entity.gender = model.gender;
                        entity.birthday = model.birthday;
                        entity.education = model.education;
                        entity.programming_language = model.programming_language;
                        entity.toeic_score = model.toeic_score;
                        entity.experience = model.experience;
                        entity.department = model.department;
                    }
                    else
                    {
                        ViewBag.Error = "Cập nhật thông tin không thành công";
                        return View(model);
                    }
                }
                _dbContext.SaveChanges();
                return RedirectToAction("Trainee", "User");
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Có lỗi xảy ra. Vui lòng thử lại";
                return View(model);
            }
        }
        [HttpGet]
        public IActionResult DeleteTrainee(long id)
        {
            try
            {
                var data = _dbContext.Users.Find(id);
                if (data != null)
                {
                    // Use Remove method to delete the entity
                    _dbContext.Users.Remove(data);
                    _dbContext.SaveChanges();
                    TempData["DeleteUser"] = true;
                }
                else
                {
                    TempData["DeleteUser"] = false;
                }
            }
            catch (Exception ex)
            {
                TempData["DeleteUser"] = false;
            }
            return RedirectToAction("Trainee", "User");
        }
    }
}
