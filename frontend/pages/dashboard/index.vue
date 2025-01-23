<template>
  <div class="flex items-center text-lg font-semibold md:text-2xl gap-2">
    <h1>Dashboard</h1>
    <p class="text-violet-500">{{ salonStore.activeSalonName }}</p>
  </div>
</template>

<script setup lang="ts">
useHead({ title: "Groomer Manager - Dashboard" });
const router = useRouter();
const salonStore = useSalonStore();

definePageMeta({
  layout: "dashboard",
  middleware: "auth",
});

onMounted(() => {
  router.push({
    query: {
      ...router.currentRoute.value.query,
      salonId: salonStore.activeSalonId,
    },
  });
});

watch(
  [() => salonStore.activeSalonId],
  async ([newSalonId]: [string | null], [oldSalonId]: [string | null]) => {
    if (newSalonId && newSalonId !== oldSalonId) {
      router.push({
        query: {
          ...router.currentRoute.value.query,
          salonId: newSalonId,
        },
      });
      console.log("salonId changed");
    }
  }
);
</script>
