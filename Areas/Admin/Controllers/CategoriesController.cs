using BookShop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/Categories")]
    public class CategoriesController : Controller
    {
        private readonly DataContext _context;
        public CategoriesController(DataContext context)
        {
            _context = context;
        }
        [Route("CategoryList")]
        public async Task<IActionResult> CategoryList()
        {
            var categories = await _context.Categories.ToListAsync();
            var bookCount = new Dictionary<string, int>();
            foreach(var item in categories)
            {
                int counts = await _context.Books.Where(b => b.CategoryId == item.Id).CountAsync();
                bookCount.Add(item.Name, counts);
            }
            ViewData["bookCounts"] = bookCount;
            return View(categories);
        }

        [Route("Create")]
        [HttpGet]   
        public IActionResult Create()
        {
            return View();
        }               
        [Route("Create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            var existingTheLoai = await _context.Categories.FirstOrDefaultAsync(t => t.Id != category.Id && t.Name == category.Name);
            if (existingTheLoai != null)
            {
                TempData["Message"] = "This category already exists!";
            }
            else
            {
                if (ModelState.IsValid)
                {
                    _context.Categories.Add(category);
                    await _context.SaveChangesAsync();
                    TempData["Message"] = "Create successful!";
                    return RedirectToAction("CategoryList");
                }
            }            
            return View(category);
        }

        [Route("Edit")]
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {           
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                TempData["Message"] = "This category does not exist!";
            }
            return View(category);
        }

        [Route("Edit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Category category)
        {
            if(id!=category.Id)
            {
                TempData["Message"] = "Find not found!";
            }
            var existingTheLoai = await _context.Categories.FirstOrDefaultAsync(t => t.Id != id && t.Name == category.Name);
            if(existingTheLoai != null)
            {
                TempData["Message"] = "This category already exists!";
            }
            else
            {
                _context.Entry(category).State = EntityState.Modified;
                try
                {
                    _context.Update(category);
                    await _context.SaveChangesAsync();
                    TempData["Message"] = "Update successful!";
                    return RedirectToAction("CategoryList");
                }
                catch (DbUpdateConcurrencyException)
                {
                    TempData["Message"] = "Update Failed!";
                }
            }           
            return View(category);
        }

        [Route("Delete")]
        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            // Kiểm tra xem có cuốn sách nào thuộc thể loại đó hay không
            var categoryHasBooks = await _context.Books.AnyAsync(b => b.CategoryId == id);

            if (categoryHasBooks)
            {
                TempData["Message"] = "This category cannot be deleted because there are books in it!";
                return RedirectToAction("CategoryList");
            }

            // Xóa thể loại nếu không có cuốn sách nào thuộc thể loại đó
            var categoryToDelete = await _context.Categories.FindAsync(id);
            if (categoryToDelete == null)
            {
                TempData["Message"] = "No category was found to be deleted!";
                return RedirectToAction("CategoryList");
            }

            _context.Categories.Remove(categoryToDelete);
            await _context.SaveChangesAsync();

            TempData["Message"] = "Delete successful!";
            return RedirectToAction("CategoryList");
        }

    }
}
