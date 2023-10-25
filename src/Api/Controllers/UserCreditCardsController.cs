using CloudDrive.Domain.Entities;
using CloudDrive.Services.CreditCards;
using Microsoft.AspNetCore.Mvc;
namespace clouddrive.Controllers
{
    [Route("/api/UserCreditCards")]
    public class UserCreditCardsController : Controller
    {
        private readonly ICreditCardsServices _creditCardsService;
        public UserCreditCardsController(ICreditCardsServices creditCardsService)
        {
            _creditCardsService = creditCardsService;
        }
      
        [HttpGet("{id}")]
        /*
         * Get CreditCard for the user 
         */
        public async Task<IActionResult> GetCreditCard(int id)
        {
            try
            {
                var userCreditCard = await _creditCardsService.GetUserCreditCardAsync(id);
                if (userCreditCard == null)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(userCreditCard);
                }
            } catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserCreditCards(int userId)
        {
            try
            {
                var userCreditCards = await _creditCardsService.GetUserCreditCardsForUserAsync(userId);
                return Ok(userCreditCards);
            }catch (Exception ex)
            {
                return NotFound(ex);
            }
        }

        [HttpPost]
        public async Task<IActionResult>CreateUserCreditCard([FromBody] UserCreditCard userCreditCard)
        {
            try
            {
                await _creditCardsService.CreateUserCreditCard(userCreditCard);
                return Ok(userCreditCard);

            }catch(Exception ex)
            {
                return BadRequest(ex); 
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUserCreditCard(int id, [FromBody] UserCreditCard userCreditCard)
        { 
            try
            {
                return Ok(await _creditCardsService.UpdateUserCreditCardAsync(id, userCreditCard));
            }catch(Exception ex)
            {
                return NotFound(ex);
            }
            
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserCreditCard(int id)
        {
            try
            {
                
                return Ok(await _creditCardsService.DeleteUserCreditCardAsync(id));
            }catch(Exception ex)
            {
                return NotFound(ex);
            }
        }

        
    }
}
