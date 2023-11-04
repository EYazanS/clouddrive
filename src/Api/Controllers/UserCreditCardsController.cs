using Microsoft.AspNetCore.Mvc;

namespace clouddrive.Controllers
{
    [Route("UserCreditCards")]
    public class UserCreditCardsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
