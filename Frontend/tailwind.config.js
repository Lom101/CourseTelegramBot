/** @type {import('tailwindcss').Config} */
module.exports = {
  content: ["./src/**/*.{js,jsx,ts,tsx}"],
  theme: {
    extend: {
      colors: {
        bg: '#F9F7F4',
        accentBlue: '#BFD7ED',
        accentPink: '#F5C6D6',
        accentViolet: '#D6CDEA',
        accentGreen: '#C6E2C9',
        darkText: '#2E2E2E',
        'page-gray': '#B8C0D9',
        'menu-yellow': '#FFFACD',
      },
    },
  },
  plugins: [],
};