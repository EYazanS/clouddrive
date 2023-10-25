using CloudDrive.Domain;
using CloudDrive.Domain.Entities;
using CloudDrive.Persistence;
using Microsoft.EntityFrameworkCore;
using Services.CreditCards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CloudDrive.Services.CreditCards
{
    public interface ICreditCardsServices
    {
        Task<Result<CreditCardsDto>> GetUserCreditCardAsync(int id);
        Task<Result<List<CreditCardsDto>>> GetUserCreditCardsForUserAsync(int id);
        Task<Result<CreditCardsDto>> CreateUserCreditCard(UserCreditCard userCreditCard);
        Task<Result> DeleteUserCreditCardAsync(int id);
        Task<Result<CreditCardsDto>> UpdateUserCreditCardAsync(int id, UserCreditCard userCreditCard);
    }

    public class UserCreditCardsImpl : ICreditCardsServices
    {
        //Connect the Database 
        private readonly AppDbContext _appDbContext;
        public UserCreditCardsImpl(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }


        public async Task<Result<CreditCardsDto>> CreateUserCreditCard(UserCreditCard userCreditCard)
        {
            _appDbContext.UserCreditCards.Add(userCreditCard);
            await _appDbContext.SaveChangesAsync();
            return new Result<CreditCardsDto>
            {
                IsSuccssfull = true,
                Data = new CreditCardsDto
                {
                    HolderName = userCreditCard.HolderName,
                    CreditCardNumber = userCreditCard.CreditCardNumber,
                    CardSecretCode = userCreditCard.CreditCardSecretCode,
                    ExpireMonth = userCreditCard.ExpireMonth,
                    ExpireYear = userCreditCard.ExpireYear,
                }
            };

        }

        public async Task<Result> DeleteUserCreditCardAsync(int id)
        {
            var CardToDelete = _appDbContext.UserCreditCards.Where(x=>x.Id == id).FirstOrDefault();
            if (CardToDelete != null)
            {
                _appDbContext.UserCreditCards.Remove(CardToDelete);
                await _appDbContext.SaveChangesAsync();
                return new Result
                {
                    IsSuccssfull = true,
                    Message = "successfuly removed credit card # "+id+" from database"
                };
            }
            else
            {
                return new Result
                {
                    IsSuccssfull = false,
                    Message = "card not found in database"
                };
            }
            
            
        }
        /*
         Gets Credit Card for one user by the Cards Id 
         */
        public async Task<Result<CreditCardsDto>> GetUserCreditCardAsync(int id)
        {
            var userCredit = await _appDbContext.UserCreditCards.FindAsync(id);
            if (userCredit == null)
            {
                return new Result<CreditCardsDto> { IsSuccssfull = false, Message = "No Credit Card found !" };
            }
            else
            {
                return new Result<CreditCardsDto>
                {
                    IsSuccssfull = true,
                    Data = new CreditCardsDto
                    {
                        CardSecretCode = userCredit.CreditCardSecretCode,
                        CreditCardNumber = userCredit.CreditCardNumber,
                        ExpireMonth = userCredit.ExpireMonth,
                        ExpireYear = userCredit.ExpireYear,
                        HolderName = userCredit.HolderName
                    }
                };
            }
        }
        /*
         Gets All the Credit Cards for one user
         */
        public async Task< Result<List<CreditCardsDto>> > GetUserCreditCardsForUserAsync(int id)
        {
            List<UserCreditCard> cards = await _appDbContext.UserCreditCards.Where(x => x.UserId == id).ToListAsync();
            if(cards.Count == 0 | cards == null)
            {
                return new Result<List<CreditCardsDto>>
                {
                    IsSuccssfull = false,
                    Message = "User Doesn't Have any cards !"
                };
            }
            List<CreditCardsDto> cardsDtos = new List<CreditCardsDto>();
            foreach(var Card in cards)
            {
                
                cardsDtos.Add(new CreditCardsDto().ToCreditCardsDto(Card));
            }
            return new Result<List< CreditCardsDto>> { IsSuccssfull = true, Data = cardsDtos };
        }

        public async Task<Result<CreditCardsDto>> UpdateUserCreditCardAsync(int id, UserCreditCard userCreditCard)
        {
            try
            {
                var card = await _appDbContext.UserCreditCards.SingleOrDefaultAsync(x => x.Id == id);
                card.CreditCardNumber = userCreditCard.CreditCardNumber;
                card.CreditCardSecretCode = userCreditCard.CreditCardSecretCode;
                card.ExpireMonth = userCreditCard.ExpireMonth;
                card.ExpireYear = userCreditCard.ExpireYear;
                card.HolderName = userCreditCard.HolderName;
                _appDbContext.UserCreditCards.Update(card);
                await _appDbContext.SaveChangesAsync();
                var data = new CreditCardsDto().ToCreditCardsDto(card);
                return new Result<CreditCardsDto>
                {
                    IsSuccssfull = true,
                    Data = data
                };
            }
            catch (Exception ex)
            {
                return new Result<CreditCardsDto> { IsSuccssfull = false, Message = ex.Message };
            }
            
        }
    }
}
