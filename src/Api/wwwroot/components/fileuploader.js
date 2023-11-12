console.log("file uploader");

let imageFormats = [".jpg", ".jpeg", ".gif", ".png", ".svg", ".bmp"];

class FileUploader extends HTMLElement {
  emptyLabel = "Please select a file";
  previewAlt = "File preview for current selected file";

  labelClasses =
    "transition-all duration-200 rounded-md border-2 border-dotted border-primary-400 p-10 block cursor-pointer font-bold hover:shadow-lg";

  uploader;
  label;
  preview;

  name;
  id;

  constructor() {
    super();

    this.createUploader();
    this.createLabel();
    this.createPreview();

    this.appendChild(this.label);
    this.appendChild(this.preview);
    this.appendChild(this.uploader);

    this.label.onclick = () => {
      this.uploader.click();
    };

    this.uploader.onchange = this.handleFileOnChange.bind(this);
  }

  static get observedAttributes() {
    return ["id", "name"];
  }

  attributeChangedCallback(property, oldValue, newValue) {
    if (oldValue === newValue) return;

    this[property] = newValue;

    if (property === "id") {
      this.label.for = this.id;
      this.uploader.id = this.id;
      this.label.id = this.id + "-label";
      this.preview.id = this.id + "-preview";
    } else if (property === "name") {
      this.uploader.name = this.name;
    }
  }

  connectedCallback() {
    this.label.for = this.id;
    this.uploader.id = this.id;
    this.label.id = this.id + "-label";
    this.preview.id = this.id + "-preview";
    this.uploader.name = this.name;
  }

  createUploader() {
    this.uploader = document.createElement("input");
    this.uploader.type = "file";
    this.uploader.name = this.name;
    this.uploader.id = this.id;
    this.uploader.classList.add("hidden");
  }

  createLabel() {
    this.label = document.createElement("label");
    this.label.for = this.id;
    this.label.id = this.id + "-label";
    this.label.innerText = this.emptyLabel;

    this.labelClasses
      .split(" ")
      .forEach((classs) => this.label.classList.add(classs));
  }

  createPreview() {
    this.preview = document.createElement("img");
    this.preview.alt = this.previewAlt;
    this.preview.id = this.id + "-preview";
    this.preview.classList.add("hidden");
    this.preview.classList.add("w-24");
    this.preview.classList.add("h-24");
  }

  handleFileOnChange(event) {
    if (event.target.files) {
      let file = event.target.files[0];

      let fileName = file.name;

      this.label.innerText = `Selected file is: ${fileName}`;

      // CREATE PREVIEW
      for (let index = 0; index < imageFormats.length; index++) {
        let img = imageFormats[index];

        if (fileName.endsWith(img)) {
          let blob = new File([file], fileName);
          let url = URL.createObjectURL(blob);
          this.preview.src = url;
          this.preview.classList.remove("hidden");
          break;
        }
      }
    } else {
      this.label.innerText = this.emptyLabel;
    }
  }
}

customElements.define("file-uploader", FileUploader);
