<template>
  <h1>Dashboard</h1>
</template>

<script setup lang="ts">
useHead({ title: "Groomer Manager - Dashboard" });

definePageMeta({
  layout: "dashboard",
  middleware: "auth",
});

const salonStore = useSalonStore();

onMounted(async () => {
  // await salonStore.loadSalons();
});
</script>
