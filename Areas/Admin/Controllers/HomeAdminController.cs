using Microsoft.AspNetCore.Mvc;

namespace BookShop.Area.Admin.Controllers
{
    [Area("Admin")]
    //[Route("Admin")]
    //[Route("Admin/HomeAdmin")]
    public class HomeAdminController : Controller
    {
        //[Route("")]
        //[Route("Index")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
