<template>
  <div class="flex items-center text-lg font-semibold md:text-2xl gap-2">
    <h1>Klienci</h1>
    <p class="text-violet-500">{{ salonStore.activeSalonName }}</p>
  </div>
  <div class="flex w-full justify-between">
    <Input
      v-model="searchTerm"
      class="max-w-sm"
      placeholder="Znajdź klienta"
      @input="debouncedSearchInput"
    />
    <AddNewClientDialog />
  </div>
  <Spinner v-if="clientsStore.isLoading" />
  <DataTable
    v-else
    :data="clientsStore.clients"
    :columns="columns"
    :pagination="pagination"
    :paginationChange="{
      onPaginationChange: (updater) => {
        typeof updater === 'function'
          ? setPagination(
              updater({
                pageIndex: pagination.pageIndex,
                pageSize: pagination.pageSize,
              })
            )
          : setPagination(updater);
      },
    }"
    :pageCount="clientsStore.pageCount"
  />
</template>

<script setup lang="ts">
import type { PaginationState } from "@tanstack/vue-table";
import { columns } from "./columns";

const INITIAL_PAGE_INDEX = 0;
const INITIAL_PAGE_SIZE = 10;

definePageMeta({
  layout: "dashboard",
  middleware: "auth",
});

const clientsStore = useClientsStore();
const salonStore = useSalonStore();
const searchTerm = ref("");
const router = useRouter();

const debouncedSearchInput = debounce(
  async () =>
    await clientsStore.loadClients(
      salonStore.activeSalonId!,
      pagination.value.pageIndex,
      pagination.value.pageSize,
      searchTerm.value
    ),
  650
);

const pagination = ref<PaginationState>({
  pageIndex: INITIAL_PAGE_INDEX,
  pageSize: INITIAL_PAGE_SIZE,
});

function setPagination({
  pageIndex,
  pageSize,
}: PaginationState): PaginationState {
  pagination.value.pageIndex = pageIndex;
  pagination.value.pageSize = pageSize;

  return { pageIndex, pageSize };
}

const loadClients = async () => {
  await clientsStore.loadClients(
    salonStore.activeSalonId!,
    pagination.value.pageIndex,
    pagination.value.pageSize,
    searchTerm.value
  );
};

onMounted(async () => {
  router.push({
    query: {
      ...router.currentRoute.value.query,
      salonId: salonStore.activeSalonId,
    },
  });

  await loadClients();
});

watch(
  [
    () => salonStore.activeSalonId,
    () => pagination.value.pageIndex,
    () => pagination.value.pageSize,
  ],
  async (
    [newSalonId, newPageIndex, newPageSize]: [string | null, number, number],
    [oldSalonId, oldPageIndex, oldPageSize]: [string | null, number, number]
  ) => {
    if (newSalonId && newSalonId !== oldSalonId) {
      router.push({
        query: {
          ...router.currentRoute.value.query,
          salonId: newSalonId,
        },
      });
      await loadClients();
    }
    if (newPageIndex !== oldPageIndex) {
      router.push({
        query: {
          ...router.currentRoute.value.query,
          page: newPageIndex + 1,
        },
      });
      await loadClients();
    }
    if (newPageSize !== oldPageSize) {
      router.push({
        query: {
          ...router.currentRoute.value.query,
          pageSize: newPageSize,
        },
      });
      await loadClients();
    }
  }
);
</script>
