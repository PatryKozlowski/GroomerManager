import { useToast } from "@/components/ui/toast/use-toast";
import { useSalonStore } from "./salon";

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
      const { toast } = useToast();
      const salonStore = useSalonStore();
      this.isLoading = true;
      useApi("/api/Auth/Login", {
        method: "POST",
        body: values,
      })
        .then(async (response) => {
          const data = response.data.value as LoginResponse;
          if (data) {
            this.setIsAuthenticated();
            await salonStore.loadSalons();
            toast({
              variant: "success",
              description: "Pomyślnie zalogowano !",
            });
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
