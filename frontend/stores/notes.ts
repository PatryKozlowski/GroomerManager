import type { Notes, NotesResponse } from "~/types/type";

export const useNoteStore = defineStore("notesStore", {
  state: () => ({
    clientNotes: [] as Notes,
    isLoading: false,
  }),
  actions: {
    async loadClientNotes() {
      this.clientNotes = [];
      const router = useRoute();
      this.isLoading = true;

      useApi(`/api/Note/GetClientNote/?clientId=${router.params.id}&salonId=${router.query.salonId}`, {
        method: "GET",
      })
        .then((response) => {
          const data = response.data.value as NotesResponse[];
          if (data) {
            this.clientNotes = data;
          }
        })
        .finally(() => {
          this.isLoading = false;
        });
    },
    async addNewClientNote(values: any, onSuccess: () => void = () => {}) {
      const router = useRoute();
      this.isLoading = true;

      useApi(`/api/Note/AddNewClientNote/?clientId=${router.params.id}&salonId=${router.query.salonId}`, {
        method: "POST",
        body: values,
      })
        .then((response) => {
          const data = response.data.value as NotesResponse;
          if (data) {
            this.clientNotes = [data, ...this.clientNotes];
            onSuccess();
          }
        })
        .finally(() => {
          this.isLoading = false;
        });
    },
  },
});
