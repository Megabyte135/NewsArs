using Microsoft.AspNetCore.Mvc;
using NewsArs.Data;

namespace Components
{
    public class CategoriesListViewComponent : ViewComponent
    {
        private readonly AppDbContext _db;
        public CategoriesListViewComponent(AppDbContext db)
        {
            _db = db;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            IEnumerable<NewsArs.Models.Category> CategoriesList = _db.Categories;
            return View(CategoriesList);
        }
    }
}