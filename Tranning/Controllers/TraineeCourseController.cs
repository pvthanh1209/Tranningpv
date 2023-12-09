using Microsoft.AspNetCore.Mvc;
using Tranning.DataDBContext;
using Tranning.Models;

namespace Tranning.Controllers
{

    public class TraineeCourseController : Controller
    {
        private readonly TranningDBContext _dbContext;
        public TraineeCourseController(TranningDBContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IActionResult Index()
        {
            List<TraneeCourseMapping> model = new List<TraneeCourseMapping>();
            var data = _dbContext.trainee_Courses.Join(_dbContext.Courses,
                tc => tc.course_id,
                c => c.id,
                (tc, c) => new TraneeCourseMapping
                {
                    trainee_id = tc.trainee_id,
                    course_id = tc.course_id,
                    created_at = tc.created_at,
                    updated_at = tc.updated_at,
                    courseName = c.name,
                    userid = tc.userid
                }).ToList();
            if (data.Count > 0 && data != null)
            {
                foreach(var item in data)
                {
                    var user = _dbContext.Users.Find(item.userid);
                    if(user != null)
                    {
                        model.Add(new TraneeCourseMapping
                        {
                            trainee_id = item.trainee_id,
                            course_id = item.course_id,
                            created_at = item.created_at,
                            updated_at = item.updated_at,
                            courseName = item.courseName,
                            fullName = user.full_name
                        });
                    }
                    else
                    {
                        model.Add(item);
                    }
                }
            }
            return View(model);
        }
        [HttpGet]
        public IActionResult CreateOrEdit(long id)
        {
            Trainee_course model = new Trainee_course();
            ViewBag.Course = _dbContext.Courses.ToList();
            ViewBag.Trainee = _dbContext.Users.Where(x => x.type == 2).ToList();
            var data = _dbContext.trainee_Courses.Find(id);
            if (data != null)
            {
                model = data;
            }
            return View(model);
        }
        [HttpPost]
        public IActionResult CreateOrEdit(Trainee_course model)
        {
            ViewBag.Course = _dbContext.Courses.ToList();
            ViewBag.Trainee = _dbContext.Users.Where(x => x.type == 2).ToList();
            try
            {
                var ckUserid = _dbContext.trainee_Courses.Where(x => x.userid == model.userid && x.trainee_id != model.trainee_id).FirstOrDefault();
                if(ckUserid != null)
                {
                    ViewBag.Error = "Đã tồn tại thông tin trainee";
                    return View(model);
                }
                if(model.trainee_id == 0)
                {
                    model.created_at = DateTime.Now;
                    _dbContext.trainee_Courses.Add(model);
                }
                else
                {
                    var enity = _dbContext.trainee_Courses.Find(model.trainee_id);
                    if(enity != null)
                    {
                        enity.course_id = model.course_id;
                        enity.updated_at = DateTime.Now;
                        enity.userid = model.userid;    
                    }
                    else
                    {
                        ViewBag.Error = "Cập nhật không thành công";
                        return View(model);
                    }
                }
                _dbContext.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
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
                var data = _dbContext.trainee_Courses.Find(id);
                if (data != null)
                {
                    // Use Remove method to delete the entity
                    _dbContext.trainee_Courses.Remove(data);
                    _dbContext.SaveChanges();
                    TempData["DeleteTrainee"] = true;
                }
                else
                {
                    TempData["DeleteTrainee"] = false;
                }
            }
            catch (Exception ex)
            {
                TempData["DeleteTrainee"] = false;
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
