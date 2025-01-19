// https://nuxt.com/docs/api/configuration/nuxt-config
export default defineNuxtConfig({
  compatibilityDate: "2024-11-01",
  devtools: { enabled: false },
  runtimeConfig: {
    API_URL: process.env.API_URL,
    public: {
      API_URL: process.env.API_URL,
    },
  },
  css: ["~/assets/css/tailwind.css"],
  postcss: {
    plugins: {
      tailwindcss: {},
      autoprefixer: {},
    },
  },
  colorMode: {
    classSuffix: "",
  },
  modules: ["@nuxtjs/tailwindcss", "shadcn-nuxt", "@nuxtjs/color-mode"],
});
