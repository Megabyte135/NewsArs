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

        public IActionResult ArticleByCategory(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var obj = _db.Articles
                .Include(u => u.Category)
                .Include(u => u.Author)
                .FirstOrDefault(x => x.CategoryId == id);
            if (obj == null)
            {
                return NotFound();
            }
            return View(obj);
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
            try
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
                article.Author = _db.Users.FirstOrDefault(x => x.Email == HttpContext.User.Identity.Name);
                article.CreationDate = DateTime.Now;
                _db.Articles.Add(article);
                _db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            catch(Exception ex)
            {
                return View(article);
            }
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