using CloudDrive.Domain;
using CloudDrive.Services.CreditCards;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services.CreditCards;

namespace clouddrive.Areas.UserCreditCards.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ICreditCardsServices _service;

        public Result<List<CreditCardsDto>> Cards { get; set; }
        
        [BindProperty]
        public int Id { get; set; }
        public IndexModel(ICreditCardsServices service)
        {
            _service = service;
            Cards = new Result<List<CreditCardsDto>>();
        }

        public async void OnGet(int Id)
        {
            if (Id > 0)
            {
                var result = await _service.GetAllAsync(Id);
                if (result.IsSuccssfull)
                {
                    Cards = result;
                }
                else
                {
                    Cards.Message = result.Message;
                    Cards.IsSuccssfull = result.IsSuccssfull;
                }
            }
           
        }
    }
}
