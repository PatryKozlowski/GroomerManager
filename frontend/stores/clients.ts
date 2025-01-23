import type {
  AddNewClient,
  AddNewClientResponse,
  AddNewNote,
  ClientResponse,
  ClientsResponse,
  NotesResponse,
} from "~/types/type";
import { useToast } from "@/components/ui/toast/use-toast";
import type { Client } from "~/types/type"; // Ensure Client type is imported
const { toast } = useToast();

export const useClientsStore = defineStore("clientsStore", {
  state: () => ({
    clients: [] as Client[],
    totalCount: 0,
    pageCount: 0,
    client: {
      name: "",
      phone: "",
      email: "",
      notes: [],
    } as ClientResponse,
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

    getStoredClientById(clientId: number) {
      return this.clients.find((client) => client.id === clientId);
    },

    async loadNotes(clientId: number) {
      this.isLoading = true;
      useApi(`/api/Client/GetClient?ClientId=${clientId}`, {
        method: "GET",
      })
        .then((response) => {
          const data = response.data.value as ClientResponse;
          if (data) {
            (this.client.name = data.name), (this.client.phone = data.phone);
            this.client.email = data.email;
            this.client.notes = data.notes;
          }
        })
        .finally(() => {
          this.isLoading = false;
        });
    },

    async addNewNote(values: AddNewNote, onSuccess: () => void = () => {}) {
      this.isLoading = true;
      useApi("/api/Client/AddNoteForClient", {
        method: "POST",
        body: values,
      })
        .then((response) => {
          const data = response.data.value as NotesResponse;
          if (data) {
            toast({
              variant: "success",
              description: "Pomyślnie dodano notatkę",
            });
            this.client.notes.push({
              id: data.id,
              text: values.text,
              createdBy: data.createdBy,
              created: data.created,
            });
            onSuccess();
          }
        })
        .finally(() => {
          this.isLoading = false;
        });
    },
  },
});
