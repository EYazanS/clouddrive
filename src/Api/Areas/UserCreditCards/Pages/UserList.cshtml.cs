using CloudDrive.Domain;
using CloudDrive.Services.CreditCards;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.CreditCards;

namespace clouddrive.Areas.UserCreditCards.Pages
{
    public class UserListModel : PageModel
    {
        private readonly ICreditCardsServices _service;

        public UserListModel(ICreditCardsServices service)
        {
            _service = service;
        }
        [BindProperty]
        public Result<List<CreditCardsDto>> users { get; set; }
        


        public async Task OnGet()
        {
            users = await _service.ListAllUsersAsync();

        }
    }
}
