namespace CloudDrive.Domain.Entities
{
    public class UserCreditCard
    {
        public int Id { get; set; }
        public int? UserId { get; set; }
        public string CreditCardNumber { get; set; }
        public string CreditCardSecretCode { get; set; } //CVV
        public string HolderName { get; set; }
        public string ExpireMonth { get; set; } // the month should be like 08, 06 
        public string ExpireYear { get; set; }// the year should be like 2023
    }
}
