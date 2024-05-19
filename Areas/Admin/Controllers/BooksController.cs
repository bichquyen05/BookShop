using BookShop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;


namespace TiemSachChill.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/Books")]
    public class BooksController : Controller
    {
        private readonly DataContext _context;        
        public BooksController(DataContext context)
        {
            _context = context;
        }

        [Route("")]

        [Route("ListBook")]
        public async Task<IActionResult> ListBook()
        {
            var books = _context.Books.Include(b => b.Category).Include(b => b.Supplier);
            return View(await books.ToListAsync());
        }
        
        // GET: Books/Details/5
        [Route("Details")]
        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .Include(b => b.Category)
                .Include(b => b.Supplier)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        //GET: Books/Edit/5
        [Route("Edit")]
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            ViewData["Categories"] = new SelectList(_context.Categories, "Id", "Name");
            ViewData["Suppliers"] = new SelectList(_context.Suppliers, "Id", "Name");
            return View(book);
        }
        // POST: Books/Edit/5
        [Route("Edit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Book book)
        {
            //ModelState.Remove("BookFile");
            if (book.bookFile==null)
            {
                ModelState.Remove("BookFile");
            }
            else
            {
                string fileName = UploadImage(book);
                book.Image = fileName;
            }            
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(book);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["Message"] = "Update successful!";
                return RedirectToAction("ListBook");
            }
            ViewData["Categories"] = new SelectList(_context.Categories, "Id", "Name", book.CategoryId);
            ViewData["Suppliers"] = new SelectList(_context.Suppliers, "Id", "Name", book.SupplierId);
            TempData["Message"] = "Update Failed!";
            return View(book);
        }


        [Route("Create")]
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name");
            ViewBag.Suppliers = new SelectList(_context.Suppliers, "Id", "Name");
            //Book book = new Book();
            return View();
        }
        [Route("Create")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Book book)
        {
            string fileName = UploadImage(book);
            book.Image = fileName;
            if (ModelState.IsValid)
            {
                _context.Add(book);
                await _context.SaveChangesAsync();
                TempData["Message"] = "Create successful!";
                return RedirectToAction("ListBook");
            }
            ViewBag.Categories = new SelectList(_context.Categories, "Id", "Name", book.CategoryId);
            ViewBag.Suppliers = new SelectList(_context.Suppliers, "Id", "Name", book.SupplierId);
            TempData["Message"] = "Create Failed!";
            return View(book);
        }

        [Route("Delete")]
        [HttpGet]      
        public async Task<IActionResult> Delete(int? id)
        {
            if(id == null)
            {
                TempData["Message"] = "No book was found to be deleted!";
                return NotFound();
            }
            var book = await _context.Books.Include(b => b.Category).Include(b => b.Supplier).FirstOrDefaultAsync(m => m.Id == id);
            if (book != null)
            {
                _context.Books.Remove(book);
               await _context.SaveChangesAsync();
            }
            TempData["Message"] = "Delete successful!";
            return RedirectToAction("ListBook");
        }        
        private bool BookExists(int id)
        {
            return _context.Books.Any(e => e.Id == id);
        }

        private string UploadImage(Book book)
        {
            string uniqueName = null;
            if (book.bookFile != null)
            {
                var myPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Books");
                if (!Directory.Exists(myPath))
                {
                    Directory.CreateDirectory(myPath);
                }
                var fullPath = Path.Combine(myPath, book.bookFile.FileName);
                using (var file = new FileStream(fullPath, FileMode.Create))
                {
                    book.bookFile.CopyTo(file);
                }
                uniqueName = Path.GetFileName(fullPath);
            }
            return uniqueName;
        }
    }
}
