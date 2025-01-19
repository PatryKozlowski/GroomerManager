<template>
  <div class="flex flex-col gap-4 text-center">
    <h1 class="text-6xl text-violet-500">Groomer Manager</h1>
    <h2 class="text-4xl">Twój organizer salonu!</h2>
    <div class="mt-4">
      <NuxtLink href="/auth">
        <Button>
          <PawPrint />
          Zacznij juz teraz !
        </Button>
      </NuxtLink>
    </div>
  </div>
</template>

<script setup lang="ts">
import { PawPrint } from "lucide-vue-next";

useHead({ title: "Groomer Manager - Twój organizer" });

definePageMeta({
  middleware: "guest",
});
</script>
