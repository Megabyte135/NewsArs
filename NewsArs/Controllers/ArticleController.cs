using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.DotNet.Scaffolding.Shared;
using Microsoft.EntityFrameworkCore;
using NewsArs.Data;
using NewsArs.Models;
using System.Drawing.Printing;

namespace NewsArs.Controllers
{
    public class ArticleController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ArticleController(AppDbContext db, IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Article(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var obj = _db.Articles
                .Include(u => u.Category)
                .Include(u => u.Author)
                .FirstOrDefault(x => x.Id == id);
            if (obj == null)
            {
                return NotFound();
            }
            return View(obj);
        }

        public IActionResult List(string sortOrder="", int page = 1)
        {
            int pageSize = 10;
            ViewBag.Pages = (_db.Articles.Count() + pageSize - 1) / pageSize;
            ViewBag.CurrentPage = page;
            IEnumerable<Article> model = _db.Articles
                .Include(u => u.Category)
                .Include(u => u.Author);
            if (page > 1)
            {
                model = model
                .Skip((page - 1) * pageSize)
                .Take(pageSize);
            }
            else
            {
                model = model
                .Take(pageSize);
            }
            switch (sortOrder)
            {
                case "Category":
                    model = model.OrderByDescending(u => u.Category.Name);
                    break;
                case "Author":
                    model = model.OrderByDescending(u => u.Author.FullName);
                    break;
                case "Date":
                    model = model.OrderByDescending(u => u.CreationDate);
                    break;
                default:
                    break;
            }
            return View(model);
        }

        [Authorize]
        public IActionResult Create()
        {
            ViewBag.Categories = _db.Categories;
            ViewBag.Authors = _db.Users;
            return View();
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Article article)
        {
            var files = HttpContext.Request.Form.Files;
            string webRootPath = _webHostEnvironment.WebRootPath;
            string upload = webRootPath + WC.ImagePath;
            string fileName = Guid.NewGuid().ToString();
            string extension = Path.GetExtension(files[0].FileName);
            using (var fileStream = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
            {
                files[0].CopyTo(fileStream);
            }
            article.Image = fileName + extension;
            article.CreationDate = DateTime.Now;
            _db.Articles.Add(article);
            _db.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var obj = _db.Articles
                .Include(u => u.Category)
                .Include(u => u.Author)
                .FirstOrDefault(x => x.Id == id);
            if (obj == null)
            {
                return NotFound();
            }
            return View(obj);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int id)
        {
            var obj = _db.Articles.Find(id);
            if (obj == null)
            {
                return NotFound();
            }
            string upload = _webHostEnvironment.WebRootPath + WC.ImagePath;
            var oldFile = Path.Combine(upload, obj.Image);
            if (System.IO.File.Exists(oldFile))
            {
                System.IO.File.Delete(oldFile);
            }
            _db.Articles.Remove(obj);
            _db.SaveChanges();
            return RedirectToAction("Index", "Home");
        }
    }
}