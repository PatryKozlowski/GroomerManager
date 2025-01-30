import type {
  AddNewClient,
  AddNewClientResponse,
  AddNewNote,
  ClientDetails,
  ClientResponse,
  ClientsResponse,
  EditClient,
  EditClientResponse,
  NotesResponse,
} from "~/types/type";
import { useToast } from "@/components/ui/toast/use-toast";
import type { Client } from "~/types/type";
const { toast } = useToast();

export const useClientsStore = defineStore("clientsStore", {
  state: () => ({
    clients: [] as Client[],
    totalCount: 0,
    pageCount: 0,
    client: {
      name: "",
      phoneNumber: "",
      email: "",
    } as ClientDetails,
    isLoading: false,
  }),
  actions: {
    async loadClients(
      salonId?: string,
      page?: number,
      pageSize?: number,
      searchTerm?: string
    ) {
      this.clients = [];
      if (this.clients.length === 0) {
        this.isLoading = true;

        const params = new URLSearchParams();

        if (salonId) params.append("salonId", salonId);
        if (page !== undefined) params.append("page", page.toString());
        if (pageSize !== undefined)
          params.append("pageSize", pageSize.toString());
        if (searchTerm) params.append("search", searchTerm);

        const url = `/api/Client/GetClients/?${params.toString()}`;

        useApi(url, {
          method: "GET",
        })
          .then((response) => {
            const data = response.data.value as ClientsResponse;
            if (data) {
              this.clients = data.clients;
              this.pageCount = data.pageCount;
              this.totalCount = data.totalCount;
            }
          })
          .finally(() => {
            this.isLoading = false;
          });
      }
    },

    async loadClient() {
      this.client = {
        name: "",
        phoneNumber: "",
        email: "",
      };
      const router = useRoute();
      this.isLoading = true;

      const url = `/api/Client/GetClient/?clientId=${router.params.id}&salonId=${router.query.salonId}`;

      useApi(url, {
        method: "GET",
      })
        .then((response) => {
          const data = response.data.value as ClientResponse;
          if (data) {
            this.client = {
              name: data.firstName + " " + data.lastName,
              phoneNumber: data.phoneNumber,
              email: data.email,
            };
          }
        })
        .finally(() => {
          this.isLoading = false;
        });
    },

    async addNewClient(values: AddNewClient, onSuccess: () => void = () => {}) {
      const router = useRoute();
      this.isLoading = true;
      useApi(`/api/Client/AddNewClient?salonId=${router.query.salonId}`, {
        method: "POST",
        body: values,
      })
        .then((response) => {
          const data = response.data.value as AddNewClientResponse;
          if (data) {
            this.clients.push({
              id: data.clientId,
              ...values,
            });
            toast({
              variant: "success",
              description: "Pomyślnie dodano klienta",
            });
            onSuccess();
          }
        })
        .finally(() => {
          this.isLoading = false;
        });
    },

    async editClient(values: EditClient, onSuccess: () => void = () => {}) {
      const router = useRoute();
      this.isLoading = true;
      useApi(`/api/Client/EditClient?salonId=${router.query.salonId}`, {
        method: "PUT",
        body: values,
      })
        .then((response) => {
          const data = response.data.value as EditClientResponse;
          if (data.clientId) {
            toast({
              variant: "success",
              description: "Pomyślnie zaktualizowano klienta",
            });
            onSuccess();
            this.client = {
              name: values.firstName + " " + values.lastName,
              phoneNumber: values.phoneNumber,
              email: values.email,
            };
            this.clients = this.clients.map((client) => {
              if (client.id === data.clientId) {
                return {
                  ...client,
                  firstName: values.firstName,
                  lastName: values.lastName,
                  phoneNumber: values.phoneNumber,
                  email: values.email,
                };
              }
              return client;
            });
          }
        })
        .finally(() => {
          this.isLoading = false;
        });
    },

    async deleteClient(clientId: string, onSuccess: () => void = () => {}) {
      const router = useRoute();
      this.isLoading = true;
      useApi(
        `/api/Client/DeleteClient?salonId=${router.query.salonId}&clientId=${clientId}`,
        {
          method: "DELETE",
        }
      )
        .then((response) => {
          const data = response.data.value as MessageResponse;
          if (data.message) {
            toast({
              variant: "success",
              description: "Pomyślnie usunięto klienta",
            });
            onSuccess();
            this.clients = this.clients.filter(
              (client) => client.id !== clientId
            );
          }
        })
        .finally(() => {
          this.isLoading = false;
        });
    },

    getStoredClientById(clientId: string) {
      return this.clients.find((client) => client.id === clientId);
    },
  },
});
