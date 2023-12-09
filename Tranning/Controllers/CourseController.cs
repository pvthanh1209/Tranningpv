using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Tranning.DataDBContext;
using Tranning.Models;

namespace Tranning.Controllers
{
    public class CourseController : Controller
    {
        private readonly TranningDBContext _dbContext;

        public CourseController(TranningDBContext context)
        {
            _dbContext = context;
        }

        [HttpGet]
        public IActionResult Index(string SearchString)
        {
            List<CourseMapping> model = new List<CourseMapping>();
            var data = _dbContext.Courses.Join(_dbContext.Categories,
                 c => c.category_id,
                 ca => ca.id,
                 (c, ca) => new CourseMapping
                 {
                     id = c.id,
                     category_id = c.category_id,
                     name = c.name,
                     description = c.description,
                     avatar = c.avatar,
                     status = c.status,
                     start_date = c.start_date,
                     end_date = c.end_date,
                     created_at = c.created_at,
                     updated_at = c.updated_at,
                     CateName = ca.name
                 }).ToList();

            data = data.Where(x => x.deleted_at == null).ToList();
            if (!string.IsNullOrEmpty(SearchString))
            {
                data = data.Where(m => m.name.Contains(SearchString) || m.description.Contains(SearchString)).ToList();
            }
            model = data;
            ViewData["CurrentFilter"] = SearchString;
            return View(model);
        }

        [HttpGet]
        public IActionResult Add()
        {
            CourseDetail course = new CourseDetail();
            var categoryList = _dbContext.Categories
                .Where(m => m.deleted_at == null)
                .Select(m => new SelectListItem { Value = m.id.ToString(), Text = m.name }).ToList();
            ViewBag.Stores = categoryList;
            return View(course);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(CourseDetail course)
        {

            if (ModelState.IsValid)
            {
                try
                {
                    string uniqueFileName = UploadFile(course.Photo);
                    var courseData = new Course()
                    {
                        name = course.name,
                        description = course.description,
                        category_id = course.category_id,
                        start_date = course.start_date,
                        end_date = course.end_date,
                        status = course.status,
                        avatar = uniqueFileName,
                        created_at = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
                    };
                
                    _dbContext.Courses.Add(courseData);
                    _dbContext.SaveChanges(true);
                    TempData["saveStatus"] = true;
                }
                catch(Exception ex)
                {
                    TempData["saveStatus"] = false;
                }
                return RedirectToAction(nameof(CourseController.Index), "Course");
            }

            var categoryList = _dbContext.Categories
              .Where(m => m.deleted_at == null)
              .Select(m => new SelectListItem { Value = m.id.ToString(), Text = m.name }).ToList();
            ViewBag.Stores = categoryList;
            Console.WriteLine(ModelState.IsValid);
            return View(course);
        }

        private string UploadFile(IFormFile file)
        {
            string filePath = string.Empty;
            try
            {
                if (file != null)
                {
                    string pathUploadImage = "wwwroot\\uploads\\images";
                    string fileName = file.FileName;
                    fileName = Path.GetFileName(fileName);
                    string uniqueStr = Guid.NewGuid().ToString();
                    fileName = uniqueStr + "-" + fileName;
                    if (!Directory.Exists(pathUploadImage))
                    {
                        Directory.CreateDirectory(pathUploadImage);
                    }
                    string uploadPath = Path.Combine(Directory.GetCurrentDirectory(), pathUploadImage, fileName);
                    var stream = new FileStream(uploadPath, FileMode.Create);
                    file.CopyTo(stream);
                    filePath = fileName;
                }
            }
            catch (Exception ex)
            {

            }
            return filePath;
        }
        [HttpGet]
        public IActionResult Update(int id)
        {
            CourseDetail course = new CourseDetail();
            ViewBag.Category = _dbContext.Categories.ToList();
            var data = _dbContext.Courses.Where(m => m.id == id).FirstOrDefault();
            if (data != null)
            {
                course.id = data.id;
                course.name = data.name;
                course.description = data.description;
                course.avatar = data.avatar;
                course.start_date = data.start_date;
                course.end_date = data.end_date;
                course.status = data.status;
                course.category_id = data.category_id;
            }
            return View(course);
        }
        [HttpPost]
        public IActionResult Update(CourseDetail course)
        {
            try
            {
                ViewBag.Category = _dbContext.Categories.ToList();
                var data = _dbContext.Courses.Where(x => x.id == course.id).FirstOrDefault();
                string imageAvatar = string.Empty;
                if (course.Photo != null)
                {
                    imageAvatar =  UploadFile(course.Photo);
                }

                if (data != null)
                {
                    data.name = course.name;
                    data.description = course.description;
                    data.start_date = course.start_date;
                    data.end_date = course.end_date;
                    data.status = course.status;
                    data.category_id = course.category_id;
                    data.updated_at = DateTime.Now;
                    if (!string.IsNullOrEmpty(imageAvatar))
                    {
                        data.avatar = imageAvatar;
                    }
                }
                else
                {
                    ViewBag.Error = "Cập nhật thông tin không thành công";
                    return View(course);
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = "Có lỗi xảy ra!";
                return View(course);
            }
            _dbContext.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public IActionResult Delete(int id = 0)
        {
            try
            {
                var data = _dbContext.Courses.Where(m => m.id == id).FirstOrDefault();
                if (data != null)
                {
                    // Use Remove method to delete the entity
                    _dbContext.Courses.Remove(data);
                    _dbContext.SaveChanges();
                    TempData["DeleteCourse"] = true;
                }
                else
                {
                    TempData["DeleteCourse"] = false;
                }
            }
            catch (Exception ex)
            {
                TempData["DeleteCourse"] = false;
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
