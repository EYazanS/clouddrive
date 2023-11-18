using CloudDrive.Domain;
using CloudDrive.Domain.Entities;
using CloudDrive.Services.CreditCards;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.CreditCards;

namespace clouddrive.Areas.UserCreditCards.Pages
{
    public class EditModel : PageModel
    {
        private readonly ICreditCardsServices _service;
        [BindProperty]
        public CreditCardsDto cardsDto { get; set; }
        [BindProperty]
        public int Id { get; set; }
        public Result<CreditCardsDto> creditCards { get; set; }
        public EditModel(ICreditCardsServices service)
        {
            _service = service;
        }

        public async Task OnGetUpdateAsync(int Id)
        {
            cardsDto = new CreditCardsDto();
            if (Id == 0)
            {
                cardsDto = null;
            }
            else
            {
                var result = await _service.GetAsync(Id);
                if (result.IsSuccssfull)
                {
                    ViewData["PageTitle"] = "Update";
                }
                else
                {
                    ViewData["PageTitle"] = "Insert";
                }
                cardsDto = result.Data;

            }
            
            
           

        }
        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            else
            {
                if (Id > 0)
                {
                    creditCards = await _service.UpdateAsync(Id, cardsDto);
                }
                else
                {
                    UserCreditCard card = new UserCreditCard();
                    card.UserId = cardsDto.userId;
                    card.CreditCardNumber = cardsDto.CreditCardNumber;
                    card.CreditCardSecretCode = cardsDto.CardSecretCode;
                    card.HolderName = cardsDto.HolderName;
                    card.ExpireYear = cardsDto.ExpireYear;
                    card.ExpireMonth = cardsDto.ExpireMonth;
                    creditCards = await _service.InsertAsync(card);

                }
                return Page();
            }

        }
    }
}
