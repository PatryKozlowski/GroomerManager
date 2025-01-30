<template>
  <div class="grid grid-cols-2 gap-6">
   <ClientCard />
  </div>
  <Notes
      title="Notatki o kliencie"
      description="Notatki dotyczące Twojego klienta"
      emptyMessage="Brak notatek o kliencie"
      :isLoading="notesStore.isLoading"
      :notes="notesStore.clientNotes"
      :addNewNoteCallback="notesStore.addNewClientNote"
  />
</template>

<script setup lang="ts">

definePageMeta({
  layout: "dashboard",
  middleware: "auth",
});

const router = useRouter();
const clientStore = useClientsStore();
const salonStore = useSalonStore();
const notesStore = useNoteStore();

onMounted(async () => {
  await Promise.all([clientStore.loadClient(), notesStore.loadClientNotes()]);
});

watch(
  [() => salonStore.activeSalonId],
  async ([newSalonId]: [string | null], [oldSalonId]: [string | null]) => {
    if (newSalonId && newSalonId !== oldSalonId) {
      await router.push({
        path: "/dashboard/clients",
        query: {
          ...router.currentRoute.value.query,
          salonId: newSalonId,
        },
      });
    }
  }
);
</script>
