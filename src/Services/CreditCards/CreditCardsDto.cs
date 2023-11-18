using CloudDrive.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.CreditCards
{
    public class CreditCardsDto
    {
        public int Id {  get; set; }
        public int userId { get; set; }
        public string HolderName { get; set; }
        public string CreditCardNumber { get; set; }
        public string CardSecretCode { get; set; }
        public int ExpireMonth { get; set; }
        public int ExpireYear { get; set; }


        public CreditCardsDto (UserCreditCard userCreditCard)
        {
            this.Id = userCreditCard.Id;
            this.HolderName = userCreditCard.HolderName;
            this.ExpireMonth = userCreditCard.ExpireMonth;
            this.ExpireYear = userCreditCard.ExpireYear;
            this.CreditCardNumber = userCreditCard.CreditCardNumber;
            this.CardSecretCode = userCreditCard.CreditCardSecretCode;
            this.userId = userCreditCard.UserId;
            
        }

        public CreditCardsDto()
        {
        }
    }
}
