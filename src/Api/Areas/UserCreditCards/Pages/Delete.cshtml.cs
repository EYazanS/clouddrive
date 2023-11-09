using CloudDrive.Services.CreditCards;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.CreditCards;

namespace clouddrive.Areas.UserCreditCards.Pages
{
    public class DeleteModel : PageModel
    {
        private readonly ICreditCardsServices _service;
        [BindProperty]
        public CloudDrive.Domain.Result<CreditCardsDto> credirCard { get; set; }
        [BindProperty]
        public int CardId { get; set; }

        public DeleteModel(ICreditCardsServices service)
        {
            _service = service;
        }

        public async Task OnGet(int Id)
        {
            var result = await _service.GetAsync(Id);
            CardId = Id;
            credirCard = result;

        }
        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            else
            {
                var result = await _service.DeleteAsync(credirCard.Data.Id);
                credirCard.IsSuccssfull = result.IsSuccssfull;
                credirCard.Message = result.Message;
                return LocalRedirect("/Index");
            }
        }
    }
}
