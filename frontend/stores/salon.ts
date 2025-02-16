import { useToast } from "@/components/ui/toast/use-toast";
import type { Salon, SalonsResponse } from "~/types/type";

export const useSalonStore = defineStore("salonStore", {
  state: () => ({
    salons: [] as Salon[],
    isLoading: false,
    activeSalonId: null as string | null,
    activeSalonName: null as string | null,
  }),
  persist: {
    storage: piniaPluginPersistedstate.localStorage(),
  },
  actions: {
    async loadSalons() {
      this.isLoading = true;
      useApi("/api/Salon/GetUserSalons", {
        method: "GET",
      })
        .then((response) => {
          const data = response.data.value as SalonsResponse;
          this.salons = data;
          this.activeSalonId = data[0]?.id;
          this.activeSalonName = data[0]?.name;
          if (data.length !== 0) {
            navigateTo("/dashboard");
          } else {
            navigateTo("/auth/salon");
          }
        })
        .finally(() => {
          this.isLoading = false;
        });
    },
    async addNewSalon(values: FormData, onSuccess: () => void = () => {}) {
      const { toast } = useToast();
      const router = useRouter();
      this.isLoading = true;
      useApi("/api/Salon/CreateSalon", {
        method: "POST",
        body: values,
      })
        .then((response) => {
          const data = response.data.value as AddSalonResponse;
          if (data) {
            this.salons.push({
              id: data.salonId,
              logoPath: data.logoPath,
              name: data.name,
            });
            toast({
              variant: "success",
              description: "Pomyślnie dodano salon",
            });
            router.push({
              path: "/dashboard/clients",
              query: {
                ...router.currentRoute.value.query,
                salonId: data.salonId,
              },
            });
            onSuccess();
          }
        })
        .finally(() => {
          this.isLoading = false;
        });
    },
    clearSalonState() {
      this.salons = [];
      this.activeSalonId = null;
      this.activeSalonName = null;
    },
  },
});
