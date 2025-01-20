<template>
  <div class="flex flex-col min-h-screen">
    <header
      class="flex h-14 items-center gap-4 border-b bg-muted/40 px-4 lg:h-[60px] lg:px-6 fixed top-0 left-0 w-full z-50"
    >
      <div class="flex justify-end w-full gap-4">
        <ThemeButton />
        <UserButton v-if="authStore.isAuthenticated" />
      </div>
    </header>
    <main
      class="flex flex-1 flex-col justify-center items-center w-full pt-14 lg:pt-[60px] pb-14 lg:pb-[60px]"
    >
      <slot />
    </main>
    <footer
      class="flex h-14 items-center gap-4 border-t bg-muted/40 px-4 lg:h-[60px] lg:px-6 fixed bottom-0 left-0 w-full"
    >
      <div class="flex gap-4 w-full justify-center">
        <Heart class="text-red-500" />
        <PawPrint class="text-violet-500" />
      </div>
    </footer>
  </div>
</template>

<script setup lang="ts">
import { Heart, PawPrint } from "lucide-vue-next";
import UserButton from "~/components/UserButton.vue";

const authStore = useAuthStore();
</script>
