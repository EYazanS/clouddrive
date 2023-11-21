
class UserCreditCards extends HTMLElement
{
 
    constructor() {
        super();

    }
    connectedCallback() {
        const itemId = this.getAttribute('itemid');
        const cardAttribute = this.getAttribute('card');
        console.log('cardAttribute:', cardAttribute);
        const card = JSON.parse(this.getAttribute('card'));
        const row = document.createElement('tr');
        row.classList.add("bg-white", "border-b", "dark:bg-gray-800", "dark:border-gray-700", "hover:bg-gray-50", "dark:hover:bg-gray-600");

        const idCell = document.createElement('td');
        idCell.textContent = card.id;
        row.appendChild(idCell);
        const holderNameCell = document.createElement('td');
        holderNameCell.textContent = card.holderName;
        row.appendChild(holderNameCell);

        const cardNumberCell = document.createElement('td');
        cardNumberCell.textContent = card.creditCardNumber;
        row.appendChild(cardNumberCell);

        const expireDateCell = document.createElement('td');
        expireDateCell.textContent = `${card.expireMonth} / ${card.expireYear}`;
        row.appendChild(expireDateCell);

        const actionCell = document.createElement('td');
        const editLink = document.createElement('a');
        editLink.href = `/userCredit/Update/${itemId}`;
        editLink.textContent = 'Edit';

        const deleteLink = document.createElement('a');
        deleteLink.href = `/userCredit/Delete/${itemId}`;
        deleteLink.textContent = 'Delete';

        actionCell.appendChild(editLink);
        actionCell.appendChild(document.createTextNode(' | '));
        actionCell.appendChild(deleteLink);

        row.appendChild(actionCell);

        // Append the row to the table body
        const tableBody = document.querySelector('table tbody');
        tableBody.appendChild(row);
    }

    
}
customElements.define("user-credit", UserCreditCards);
