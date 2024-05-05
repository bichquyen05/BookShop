using BookShop.ViewModels;
using Microsoft.AspNetCore.Mvc;
using BookShop.Models;
using BookShop.ViewModels;

namespace TiemSachChill.ViewComponents
{
    public class MenuViewComponent : ViewComponent
    {
        private readonly DataContext _context;
        public MenuViewComponent(DataContext context)
        {
            _context = context;
        }

        public IViewComponentResult Invoke()
        {
            var categories = _context.Categories.Select(c => new Menu
            {
                Id = c.Id,
                Name = c.Name,
            }).OrderBy(c => c.Name);
            return View(categories);
        }
    }
}
