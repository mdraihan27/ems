/** @type {import('tailwindcss').Config} */
module.exports = {
  content: ["./src/**/*.{js,jsx,ts,tsx}"],
  darkMode: "class",
  theme: {
    extend: {
      fontFamily: {
        roboto: ['"Roboto"', 'sans-serif'],
      },
      colors: {
        brand: {
          //  Light-mode surfaces: clearly sage, never white-ish
          50:  "#E1EBE0",  // page bg – warm sage, visually distinct from white
          100: "#D8E9D4",  // active nav / hover bg;  dark-mode primary text   (↑ lighter)
          200: "#B4D0AE",  // card borders (visible on white); dark-mode sub-heading (↑ lighter)
          //  Mid tones
          300: "#8BAA86",  // placeholder, disabled icons;  dark-mode tertiary text (↑ lighter)
          400: "#6FA86A",  // secondary icons; secondary text on dark surfaces   (↑ lighter)
          500: "#4E7249",  // secondary body text on white (≈5.3:1 AA ✓); dark-mode btn bg
          600: "#3D5F38",  // primary buttons / active nav (≈7.1:1 AAA ✓)
          700: "#304F2A",  // button hover, dark accents
          //  Dark-mode surfaces: cool sage-teal – less saturated, gentler on eyes
          800: "#2F4232",  // elevated panels in dark mode
          900: "#243530",  // card bg in dark mode
          950: "#1C2B24",  // page bg in dark mode
        },
      },
    },
  },
  plugins: [],
};


