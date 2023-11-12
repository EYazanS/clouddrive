function toggleDarkMode() {
  document.querySelector("html").classList.toggle("dark");
}

function toggleMenu() {
  var el = document.getElementById("nav");
  el.classList.toggle("hidden");
  el.classList.toggle("flex");
}

let liClasses =
  "block transition-all duration-200 cursor-pointer h-16 p-4 border active:bg-primary-800 border-primary-400 shadow-sm rounded-md font-bold hover:bg-primary-400 hover:text-white hover:shadow-2xl";

function handleUpload(event) {
  event.preventDefault();

  var form = document.getElementById("upload-form");

  var formData = new FormData(form);

  axios.post(form.action, formData).then((res) => {
    console.log(res);
    let list = document.getElementById("files-list");
    let newLi = document.createElement("li");
    let newA = document.createElement("a");
    newLi.appendChild(newA);
    list.appendChild(newLi);

    newA.href = `/api/files/Download/${res.data.data.id}`;
    newA.innerText = res.data.data.fileName;

    liClasses.split(" ").forEach((cls) => newA.classList.add(cls));
  });
}

document.getElementById("toggle-darkmode").onclick = toggleDarkMode;
document.getElementById("toggle-menu").onclick = toggleMenu;

document.getElementById("upload-form").onsubmit = handleUpload;
