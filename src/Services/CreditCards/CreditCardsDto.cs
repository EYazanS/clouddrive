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
        public string HolderName { get; set; }
        public string CreditCardNumber { get; set; }
        public string CardSecretCode { get; set; }
        public string ExpireMonth { get; set; }
        public string ExpireYear { get; set; }

        public CreditCardsDto ToCreditCardsDto(UserCreditCard userCreditCard)
        {
            return new CreditCardsDto
            {
                HolderName = userCreditCard.HolderName,
                ExpireMonth = userCreditCard.ExpireMonth,
                ExpireYear = userCreditCard.ExpireYear,
                CreditCardNumber = userCreditCard.CreditCardNumber,
                CardSecretCode = userCreditCard.CreditCardSecretCode
            };
        }

    }
}
