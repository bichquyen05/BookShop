using BookShop.Models;
using BookShop.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using X.PagedList;



namespace BookShop.Controllers
{
    [Route("Shop")]   
    
    public class ShopController : Controller
    {        
        private readonly DataContext _context;
        public static int pageSize { get; set; } = 15;
        public ShopController(DataContext context)
        {
            _context = context;
        }
        [Route("Shop")]
        public IActionResult Shop(int? page, int? categoryId, int? sortOption)
        {
            //Book sale off
            var saleOff = _context.Books.Where(s => s.Discount > 0).ToList();
            ViewData["SaleOff"] = saleOff;

            var books = _context.Books.AsNoTracking().OrderBy(b => b.Name).ToList();
            //Book by category
            if (categoryId.HasValue)
            {
                books = books.Where(b => b.CategoryId == categoryId.Value).ToList();                
            }

            //Book sort by special or latest
            if (sortOption.HasValue)
            {
                if (sortOption.Value == 0)
                {
                    books = books.Where(b => (bool)b.Special).ToList();
                }
                else
                {
                    books = books.Where(b => (bool)b.Latest).ToList();
                }
            }

            //list book
            var listBook = Page(books, page);
            int total = books.Count;
            ViewBag.Total = total;
            return View(listBook);
        }
        [Route("BookDetail")]
        public IActionResult BookDetail(int id)
        {
            var book = _context.Books
                .Include(b=>b.Category)
                .Include(b=>b.Supplier)
                .SingleOrDefault(b=>b.Id==id);
            var bookDetail = new BookVM
            {
                Id = book.Id,
                Name = book.Name,
                Author = book.Author,
                Dimensions = string.Format("{0} x {1} cm", book.Length, book.Width),
                Pages = book.Pages,
                UnitPrice = book.UnitPrice,
                DiscountedPrice = book.UnitPrice - (book.UnitPrice * book.Discount),
                Image = book.Image,
                Description = book.Description,
                Category = book.Category.Name,
                Supplier = book.Supplier.Name,
                Quantity = book.Quantity,
                Discount = (int)(book.Discount * 100),
                Review = "12"
            };

            var relatedBooks = _context.Books.Where(b => b.CategoryId == book.CategoryId && b.Id != book.Id).Take(4).ToList();
            ViewData["RelatedBooks"] = relatedBooks;
            return View(bookDetail);
        }

        [Route("Search")]
        public IActionResult Search(string? keyword, int? page)
        {
            IQueryable<Book> bookQuery = _context.Books;
            if (!string.IsNullOrWhiteSpace(keyword))
            {
               bookQuery = bookQuery.Where(b=>b.Name.Contains(keyword));
            }
            var books = bookQuery.ToList();
            var listBook = Page(books, page);
            return View(listBook);
        }  
        
        
        //[Route("Shop")]
        //public IActionResult Shop(int? page, int? categoryId, int? sortOption)
        //{
        //    var saleOff = _context.Books.Where(s=>s.Discount > 0).ToList();
        //    ViewData["SaleOff"] = saleOff;           
        //    var books = _context.Books.AsNoTracking().OrderBy(b => b.Name).ToList();

        //    if (categoryId.HasValue)
        //    {
        //        books = books.Where(b => b.CategoryId == categoryId.Value).ToList();
        //        int count = books.Count();
        //        ViewBag.Count = count;
        //    }

        //    if (sortOption.HasValue)
        //    {
        //        if (sortOption.Value == 0)
        //        {
        //            books = books.Where(b => (bool)b.Special).ToList();
        //        }
        //        else
        //        {
        //            books = books.Where(b => (bool)b.Latest).ToList();
        //        }
        //    }

        //    var listBook = Page(books, page);
        //    int total = books.Count;
        //    ViewBag.Total = total;
        //    return View(listBook);
        //}


        //public IActionResult BookByCategory(int? categoryId, int? page)
        //{
        //    var saleOff = _context.Books.Where(s => s.Discount > 0).ToList();
        //    ViewData["SaleOff"] = saleOff;
        //    var books = _context.Books.AsNoTracking().ToList();
        //    if(categoryId.HasValue)
        //    {
        //        books = books.Where(b=>b.CategoryId== categoryId.Value).ToList();
        //        int count = books.Count();
        //        ViewBag.Count = count;                
        //    }
        //    var listBook = Page(books, page);
        //    return View(listBook);
        //}

        //public IActionResult FilterBooks(int? sortOption)
        //{
        //    var books = _context.Books.ToList();
        //    if (sortOption.HasValue)
        //    {
        //        if (sortOption.Value == 0)
        //        {
        //            books = books.Where(b => (bool)b.Special).ToList();
        //        }
        //        else
        //        {
        //            books = books.Where(b => (bool)b.Latest).ToList();
        //        }
        //    }
        //    return View(books);
        //}

        private PagedList<Book> Page(List<Book> books,int? page)
        {
            int pageNumber = (page ?? 1);
            PagedList<Book> listBook = new PagedList<Book>(books,pageNumber, pageSize);
            return listBook;
        }
    }
}
