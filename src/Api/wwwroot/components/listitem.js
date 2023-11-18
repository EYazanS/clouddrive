console.log("list item");


class ListItem extends HTMLElement {

	listClass = "my-8 list-decimal ml-10 text-l font-bold font-mono dark:text-primary-100";
	divClass = "inline-flex ml-3";
	aClass = "text-primary-600 hover:underline dark:text-primary-400";

	div;
	list;
	aEdit;
	aDelete;

	area = "Notebooks";
	deletePage = "/Delete";
	editPage = "/Form";


	constructor() {
		super();
		this.createList();
		this.createDiv();
		this.createDelete();
		this.createEdit();

		this.appendChild(this.list);
		this.list.appendChild(this.div);
		this.div.appendChild(this.aDelete);
		this.div.appendChild(this.aEdit);

		this.aEdit.addEventListener('click', this.handleEditClick.bind(this));
		this.aDelete.addEventListener('click', this.handleDeleteClick.bind(this));

	}
	static get observedAttributes() {
		return ["name", "item-id"];
	}

	connectedCallback() {

	}

	createList() {
		this.list = document.createElement("li");
		const itemName = this.getAttribute("name");

		if (itemName) {
			this.list.textContent = itemName;
		} else {
			// Provide a default text or handle the case when 'name' attribute is not set
			this.list.textContent = "Default Item Name";
		}

		this.listClass
			.split(" ")
			.forEach((classs) => this.list.classList.add(classs));
	}

	createDiv() {
		this.div = document.createElement("div");
		this.divClass
			.split(" ")
			.forEach((classs) => this.div.classList.add(classs));
	}
	createDelete() {
		this.aDelete = document.createElement("a");
		this.aDelete.textContent = "Delete"
		this.aDelete.setAttribute('asp-area', this.area);
		this.aDelete.setAttribute('asp-route-id', this.getAttribute('item-id'));
		this.aDelete.setAttribute('asp-page', this.deletePage);

		this.aDelete.classList.add("mr-3");

		this.aClass
			.split(" ")
			.forEach((classs) => this.aDelete.classList.add(classs));
	}

	createEdit() {
		this.aEdit = document.createElement("a");
		this.aEdit.textContent = "Edit"
		this.aEdit.setAttribute('asp-area', this.area);
		this.aEdit.setAttribute("asp-route-id", this.getAttribute('item-id'));
		this.aEdit.setAttribute('asp-page', this.editPage);
		const pageHandler = this.getAttribute('asp-page-handler');

		if (pageHandler) {
			this.aEdit.setAttribute('asp-page-handler', pageHandler);
		}

		this.aClass
			.split(" ")
			.forEach((classs) => this.aEdit.classList.add(classs));
	}

	handleEditClick() {
		const itemId = this.getAttribute('item-id');
		const handler = this.getAttribute('asp-page-handler')
		const editPageUrl = `${this.area}/${handler}/${itemId}`;

		// Navigate to the Edit page
		window.location.href = editPageUrl;
	}
	handleDeleteClick() {
		const itemId = this.getAttribute('item-id');
		const editPageUrl = `Notebooks/Delete/${itemId}`;

		// Navigate to the Edit page
		window.location.href = editPageUrl;
	}

}
customElements.define("list-item", ListItem);
