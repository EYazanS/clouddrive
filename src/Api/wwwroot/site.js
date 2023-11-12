function toggleDarkMode() {
  document.querySelector("html").classList.toggle("dark");
}

function toggleMenu() {
  var el = document.getElementById("nav");
  el.classList.toggle("hidden");
  el.classList.toggle("flex");
}

document.getElementById("toggle-darkmode").onclick = toggleDarkMode;
document.getElementById("toggle-menu").onclick = toggleMenu;
