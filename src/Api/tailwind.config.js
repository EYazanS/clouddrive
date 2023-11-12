/** @type {import('tailwindcss').Config} */
module.exports = {
  darkMode: "class",
  content: ["./**/*.{html,js,cshtml}"],
  theme: {
    fontFamily: {
      sans: ["Open Sans", "sans-serif"],
    },
    extend: {
      colors: {
        primary: {
          50: "#e7f5ff",
          100: "#d4eaff",
          200: "#b2d7ff",
          300: "#84bbff",
          400: "#5390ff",
          500: "#2d64ff",
          600: "#0933ff",
          700: "#0029ff",
          800: "#0428cf",
          900: "#0f2eaa",
          950: "#09185d",
        },
        secondery: {
          50: "#fcf9ea",
          100: "#f9f1c8",
          200: "#f4e094",
          300: "#edc957",
          400: "#e8b83f",
          500: "#d69a1c",
          600: "#b87716",
          700: "#935515",
          800: "#7a4419",
          900: "#68391b",
          950: "#3d1d0b",
        },
      },
    },
  },
  plugins: [],
};
