// import { useToast } from "@/components/ui/toast/use-toast";
import type { Salon, SalonsResponse } from "~/types/type";

export const useSalonStore = defineStore("salonStore", {
  state: () => ({
    salons: [] as Salon[],
    isLoading: false,
  }),
  actions: {
    async loadSalons() {
      this.isLoading = true;
      useApi("/api/Salon/GetUserSalons", {
        method: "GET",
      })
        .then((response) => {
          const data = response.data.value as SalonsResponse;
          console.log("data ", data);
          if (data.length !== 0) {
            this.salons = data;
            navigateTo("/dashboard");
          } else {
            navigateTo("/auth/salon");
          }
        })
        .finally(() => {
          this.isLoading = false;
        });
    },
  },
});
