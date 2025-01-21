<template>
  <div class="px-4 lg:px-6">
    <NuxtLink href="/dashboard" class="flex items-center gap-2 font-semibold">
      <Avatar class="h-12 w-12 rounded-full">
        <img
          :src="salonStore.salons[0].logoPath"
          alt="Logo"
          class="border rounded-full"
        />
      </Avatar>
      <span class="">{{ salonStore.salons[0].name }}</span>
    </NuxtLink>
  </div>
</template>

<script setup lang="ts">
const salonStore = useSalonStore();
</script>
