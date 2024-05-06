using Microsoft.AspNetCore.Mvc;

namespace BookShop.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/Dashboard")]  
    
    public class DashboardController : Controller
    {

        [Route("Dashboard")]
        public IActionResult Dashboard()
        {
            return View();
        }
    }
}
