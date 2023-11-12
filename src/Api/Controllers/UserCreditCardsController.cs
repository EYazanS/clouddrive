/*using CloudDrive.Domain;
using CloudDrive.Domain.Entities;
using CloudDrive.Services.CreditCards;
using Microsoft.AspNetCore.Mvc;
using Services.CreditCards;

namespace clouddrive.Controllers
{
    [Route("UserCreditCards")]
    public class UserCreditCardsController : Controller
    {
        private readonly ICreditCardsServices _creditCardsServices;

        public UserCreditCardsController(ICreditCardsServices creditCardsServices)
        {
            _creditCardsServices = creditCardsServices;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Index(int id)
        {
            Result<List<CreditCardsDto>> result = await _creditCardsServices.GetAllAsync(id);

            return View(result);
        }
        [HttpGet("Create")]
        public IActionResult Create()
        {
            return View();
        }
    }
}
*/