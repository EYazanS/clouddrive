
class UserCreditCards extends HTMLElement
{
    table;
    row;
    constructor() {
        super();
        this.createRow();
    }
    createRow() {

    }
    set card(cardData) {
        this.updateRowContent(cardData);
    }
    updateRowContent(cardData) {
        // Update cell content based on cardData
        this.cells[0].textContent = cardData.Id;
        this.cells[1].textContent = cardData.HolderName;
        this.cells[2].textContent = cardData.CreditCardNumber;
        this.cells[3].textContent = `${cardData.ExpireMonth} / ${cardData.ExpireYear}`;
        // Add action links/buttons update if needed
    }

    
}
customElements.define("user-credit", UserCreditCards);
