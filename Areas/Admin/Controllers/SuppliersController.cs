using BookShop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.AccessControl;

namespace BookShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/Suppliers")]
    public class SuppliersController : Controller
    {
        private readonly DataContext _context;
        public SuppliersController(DataContext context)
        {
            _context = context;
        }

        [Route("SupplierList")]
        [HttpGet]
        public async Task<IActionResult> SupplierList()
        {
            var suppliers = await _context.Suppliers.ToListAsync();
            return View(suppliers);
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
        public async Task<IActionResult> Create(Supplier supplier)
        {
            var existingSupplier = await _context.Suppliers.FirstOrDefaultAsync(s=>s.Name==supplier.Name || s.Id == supplier.Id);
            if (existingSupplier != null)
            {
                TempData["Message"] = "This supplier already exists!";
            }
            else
            {
                if (ModelState.IsValid)
                {
                    _context.Suppliers.Add(supplier);
                    await _context.SaveChangesAsync();
                    TempData["Message"] = "Create successful!";
                    return RedirectToAction("SupplierList");
                }
            }
            return View(supplier);
        }

        [Route("Edit")]
        [HttpGet]
        public async Task<IActionResult> Edit(string? id)
        {
            var supplier = await _context.Suppliers.FindAsync(id);
            if (supplier == null)
            {
                TempData["Message"] = "This supplier does not exist!";
                return RedirectToAction("SupplierList");
            }
            return View(supplier);
        }
        [Route("Edit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, Supplier supplier)
        {
            if (id != supplier.Id)
            {
                TempData["Message"] = "Find not found!";
            }            
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(supplier);
                    await _context.SaveChangesAsync();
                    TempData["Message"] = "Update Successful!";
                    return RedirectToAction("SupplierList");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SupplierExists(supplier))
                    {
                        TempData["Message"] = "This supplier already exists!";
                    }
                    else
                    {
                        TempData["Message"] = "Update Failed!";
                    }
                }
            }
            return View(supplier);
        }

        [Route("Delete")]
        [HttpGet]
        public async Task<IActionResult> Delete(string id)
        {
            // Kiểm tra xem có cuốn sách nào thuộc thể loại đó hay không
            var supplierHasBooks = await _context.Books.AnyAsync(b => b.SupplierId == id);

            if (supplierHasBooks)
            {
                TempData["Message"] = "This supplier cannot be deleted because there are books in it!";
                return RedirectToAction("SupplierList");
            }

            // Xóa nhà xuất bản nếu không có cuốn sách nào thuộc nxb đó
            var supplierToDelete = await _context.Suppliers.FindAsync(id);
            if (supplierToDelete == null)
            {
                TempData["Message"] = "No supplier was found to delete!";
                return RedirectToAction("SupplierList");
            }

            _context.Suppliers.Remove(supplierToDelete);
            await _context.SaveChangesAsync();

            TempData["Message"] = "Delete successful!";
            return RedirectToAction("SupplierList");
        }

        private bool SupplierExists(Supplier supplier)
        {
            return _context.Suppliers.Any(s=>s.Id == supplier.Id || s.Name == supplier.Name);
        }
    }
}
