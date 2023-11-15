console.log("list item");

class ListItem extends HTMLElement {

	listClass = "my-8 list-decimal ml-10 text-l font-bold font-mono dark:text-primary-100";
	divClass = "inline-flex ml-3";
	aClass = "text-primary-600 hover:underline dark:text-primary-400";

	div;
	list;
	a;

	area = "Notebooks";
	deletePage = "/Delete";
	editPage = "/Form";


	constructor() {
		super();
		this.createList();
		this.createDiv();
		this.createDelete();
		this.createDiv();

		this.appendChild(this.list);
		this.appendChild(this.div);
		this.appendChild(this.a);
		this.appendChild(this.a);

	}

	connectedCallback() {
	}
	createList() {
		this.list = document.createElement("li");
		this.listClass
			.split(" ")
			.forEach((classs) => this.label.classList.add(classs));
	}

	createDiv() {
		this.div = document.createElement("div");
		this.divClass
			.split(" ")
			.forEach((classs) => this.label.classList.add(classs));
	}
	createDelete() {
		this.a = document.createElement("a");
		this.a.area=area;
		this.a.classList.add("mr-3");
		this.aClass
			.split(" ")
			.forEach((classs) => this.label.classList.add(classs));
	}
	createEdit() {
		this.a = document.createElement("a");
		this.a.area=area;
		this.a.
		this.aClass
			.split(" ")
			.forEach((classs) => this.label.classList.add(classs));
	}


}
customElements.define("list-item", ListItem);
