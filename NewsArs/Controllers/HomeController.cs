using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NewsArs.Data;
using NewsArs.Models;
using System.Diagnostics;

namespace NewsArs.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public HomeController(AppDbContext db, IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            IEnumerable<Article> articles = _db.Articles.Include(u => u.Category).Take(6);
            return View(articles);
        }

        public IActionResult About()
        {
            return View();
        }
    }
}