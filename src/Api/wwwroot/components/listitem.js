console.log("list item");

class MyListItem extends HTMLElement {
	constructor() {
		super();

		// Create a shadow root
		this.attachShadow({ mode: 'open' });

		// Clone the template content and attach it to the shadow root
		this.shadowRoot.innerHTML = `
      <style>
        :host {
          display: block;
          margin-top: 8px;
          list-style-type: decimal;
          margin-left: 10px;
          font-size: 1rem;
          font-weight: bold;
          font-family: 'Monospace', monospace; /* Adjust font-family as needed */
          color: #000; /* Adjust text color */
        }

        .inline-flex {
          display: inline-flex;
          margin-left: 3px;
        }

        a {
          margin-right: 3px;
          color: #0066cc; /* Adjust link color */
          text-decoration: none;
        }

        a:hover {
          text-decoration: underline;
        }

        .dark-text {
          color: #A0AEC0; /* Adjust dark mode text color */
        }
      </style>
      <li class="my-8 list-decimal ml-10 text-l font-bold font-mono dark-text">
        ${this.getAttribute('name')}
        <div class="inline-flex ml-3">
          <a class="delete-link">Delete</a>
          <a  class="edit-link">Edit</a>
        </div>
      </li>
    `;

		// Set up event listeners or any additional logic here
		this.shadowRoot.querySelector('.delete-link').addEventListener('click', this.handleDelete.bind(this));
		this.shadowRoot.querySelector('.edit-link').addEventListener('click', this.handleEdit.bind(this));
	}

	// Define methods or event handlers here
	handleDelete() {
		// Implement delete logic
		console.log('Delete clicked');
	}

	handleEdit() {
		// Implement edit logic
		console.log('Edit clicked');
		const itemId = this.getAttribute('item-id');
		if (itemId) {
			// Create an anchor element dynamically
			const editLink = document.createElement('a');

			// Set the necessary attributes
			editLink.setAttribute('asp-area', 'Notebooks');
			editLink.setAttribute('asp-page', '/Form');
			editLink.setAttribute('class', 'text-primary-600 hover:underline dark:text-primary-400');
			editLink.setAttribute('asp-page-handler', 'Update');
			editLink.setAttribute('asp-route-id', itemId);

			// Append the anchor element to the body
			document.body.appendChild(editLink);

			// Simulate a click on the anchor element to navigate to the specified page
			editLink.click();

			document.body.removeChild(editLink);
		}
	}

	// Called when the element is connected to the DOM
	connectedCallback() {
		console.log('Element added to the DOM');
	}

	// Called when the element is disconnected from the DOM
	disconnectedCallback() {
		console.log('Element removed from the DOM');
	}
}

// Define the custom element
customElements.define('my-list-item', MyListItem);

// class ListItem extends HTMLElement {

// 	listClass = "my-8 list-decimal ml-10 text-l font-bold font-mono dark:text-primary-100";
// 	divClass = "inline-flex ml-3";
// 	aClass = "text-primary-600 hover:underline dark:text-primary-400";

// 	div;
// 	list;
// 	a;

// 	area = "Notebooks";
// 	deletePage = "/Delete";
// 	editPage = "/Form";


// 	constructor() {
// 		super();
// 		this.createList();
// 		this.createDiv();
// 		this.createDelete();
// 		this.createDiv();

// 		this.appendChild(this.list);
// 		this.appendChild(this.div);
// 		this.appendChild(this.a);
// 		this.appendChild(this.a);

// 	}

// 	connectedCallback() {
// 	}
// 	createList() {
// 		this.list = document.createElement("li");
// 		this.listClass
// 			.split(" ")
// 			.forEach((classs) => this.label.classList.add(classs));
// 	}

// 	createDiv() {
// 		this.div = document.createElement("div");
// 		this.divClass
// 			.split(" ")
// 			.forEach((classs) => this.label.classList.add(classs));
// 	}
// 	createDelete() {
// 		this.a = document.createElement("a");
// 		this.a.area=area;
// 		this.a.classList.add("mr-3");
// 		this.aClass
// 			.split(" ")
// 			.forEach((classs) => this.label.classList.add(classs));
// 	}
// 	createEdit() {
// 		this.a = document.createElement("a");
// 		this.a.area=area;
// 		this.a.
// 		this.aClass
// 			.split(" ")
// 			.forEach((classs) => this.label.classList.add(classs));
// 	}


// }
// customElements.define("list-item", ListItem);
