using Microsoft.AspNetCore.Mvc;

namespace PetSpace.Server.Controllers
{
    public class UserProfileController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
