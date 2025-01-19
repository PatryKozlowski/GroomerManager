export const useAuthStore = defineStore("authStore", {
  state: () => ({
    isAuthenticated: false,
    isLoading: false,
  }),
  persist: {
    storage: piniaPluginPersistedstate.localStorage(),
  },
  actions: {
    async loginUser(values: LoginRequestDto) {
      this.isLoading = true;
      useApi("/api/Auth/Login", {
        method: "POST",
        body: values,
      })
        .then((response) => {
          const data = response.data.value as LoginResponse;
          if (data) {
            this.setIsAuthenticated();
            // router.push("/dashboard");
            // navigateTo("/dashboard");
          }
        })
        .finally(() => {
          this.isLoading = false;
        });
    },
    setIsAuthenticated() {
      this.isAuthenticated = true;
    },

    clearIsAuthenticated() {
      this.isAuthenticated = false;
    },
  },
});
