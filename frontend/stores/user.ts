import { useToast } from "@/components/ui/toast/use-toast";
import type { Salon, SalonsResponse } from "~/types/type";

export const useUserStore = defineStore("userStore", {
  state: () => ({
    user: null as LoggedInUser | null,
    isLoading: false,
  }),
  persist: {
    storage: piniaPluginPersistedstate.localStorage(),
  },
  actions: {
    async loadLoggedInUser() {
      this.isLoading = true;
      useApi("/api/Auth/GetLoggedInUser", {
        method: "GET",
      })
        .then((response) => {
          const data = response.data.value as LoggedInUser;
          this.user = data;
        })
        .finally(() => {
          this.isLoading = false;
        });
    },
  },
});
