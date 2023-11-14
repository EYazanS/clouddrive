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
  var button = document.getElementById("upload-form-button");

  var formData = new FormData(form);

  button.setAttribute("disabled", true);
  button.innerText = "Uploading";

  axios.post(form.action, formData).then((res) => {
    let list = document.getElementById("files-list");
    let newLi = document.createElement("li");
    let newA = document.createElement("a");
    newLi.appendChild(newA);
    list.appendChild(newLi);

    newA.href = `/api/files/Download/${res.data.data.id}`;
    newA.innerText = res.data.data.fileName;
    button.removeAttribute("disabled");
    button.innerText = "Save";

    liClasses.split(" ").forEach((cls) => newA.classList.add(cls));
  });
}

function ToggleLanguage(expDays) {
  // Default at 365 days.
  days = expDays || 365;

  // Get unix milliseconds at current time plus number of days
  let date = new Date();
  date.setTime(+date + days * 86400000); //24 * 60 * 60 * 1000
  const expires = "expires=" + date;

  // READ COOKIE
  let cookie = document.cookie;

  // PARSE COOKIE
  let cookeiValue = "";

  let index = 0;

  while (index < cookie.length - 3) {
    if (
      cookie[index] == "u" &&
      cookie[index + 1] == "i" &&
      cookie[index + 2] == "c"
    ) {
      let newIndex = index + 4;

      while (newIndex < cookie.length && cookie[newIndex] !== ";") {
        cookeiValue += cookie[newIndex++];
      }

      break;
    }

    index++;
  }

  // UPDATE COOKE
  if (cookeiValue === "en") {
    let newValue = `.AspNetCore.Culture=c=en|uic=ar;${expires}; path=/; `;
    document.cookie = newValue;
  } else if (cookeiValue == "ar") {
    let newValue = `.AspNetCore.Culture=c=en|uic=en;${expires}; path=/; `;
    document.cookie = newValue;
  } else {
    let newValue = `.AspNetCore.Culture=c=en|uic=ar;${expires}; path=/; `;
    document.cookie = newValue;
  }

  // Refresh page
  location.reload(true);
}

document.getElementById("toggle-darkmode").onclick = toggleDarkMode;
document.getElementById("toggle-language").onclick = ToggleLanguage;
document.getElementById("toggle-menu").onclick = toggleMenu;
document.getElementById("upload-form").onsubmit = handleUpload;
