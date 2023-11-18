console.log("list item");

class NotebookItem extends HTMLElement {
  divClass = "flex space-x-2";
  aClass = "text-primary-600 hover:underline dark:text-primary-400";

  div;
  span;
  aEdit;
  aDelete;

  area = "Notebooks";
  deletePage = "/Delete";
  editPage = "/Form";

  constructor() {
    super();
    this.createDiv();
    this.createDelete();
    this.createEdit();

    this.appendChild(this.div);
    this.div.appendChild(this.span);
    this.div.appendChild(this.aEdit);
    this.div.appendChild(this.aDelete);
  }
  static get observedAttributes() {
    return ["name", "item-id"];
  }

  createDiv() {
    this.div = document.createElement("div");
    this.span = document.createElement("span");

    const itemName = this.getAttribute("name");

    if (itemName) {
      this.span.textContent += itemName;
    } else {
      // Provide a default text or handle the case when 'name' attribute is not set
      this.span.textContent = "Default Item Name";
    }

    this.divClass
      .split(" ")
      .forEach((classs) => this.div.classList.add(classs));
  }

  createDelete() {
    const itemId = this.getAttribute("item-id");

    this.aDelete = document.createElement("a");
    this.aDelete.textContent = "Delete";
    this.aDelete.href = `/Notebooks/Delete/${itemId}`;

    this.aDelete.classList.add("mr-3");

    this.aClass
      .split(" ")
      .forEach((classs) => this.aDelete.classList.add(classs));
  }

  createEdit() {
    const itemId = this.getAttribute("item-id");

    this.aEdit = document.createElement("a");
    this.aEdit.textContent = "Edit";
    this.aEdit.href = `/Notebooks/Update/${itemId}`;

    this.aClass
      .split(" ")
      .forEach((classs) => this.aEdit.classList.add(classs));
  }
}

customElements.define("notebook-item", NotebookItem);
