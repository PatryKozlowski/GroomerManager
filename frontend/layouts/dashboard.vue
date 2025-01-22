<template>
  <div
    class="grid min-h-screen w-full md:grid-cols-[220px_1fr] lg:grid-cols-[280px_1fr]"
  >
    <div class="hidden border-r bg-muted/40 md:block">
      <div class="flex h-full max-h-screen flex-col gap-2">
        <div class="flex h-14 items-center border-b lg:h-[60px]">
          <DropdownSalon v-if="userStore.user?.role === 'Owner'" />
          <SalonLogo v-else />
        </div>
        <div class="flex-1">
          <Nav :links="navLinks" />
        </div>
      </div>
    </div>
    <div class="flex flex-col">
      <header
        class="flex h-14 items-center gap-4 border-b bg-muted/40 px-4 lg:h-[60px] lg:px-6"
      >
        <MobileNav :links="navLinks" />
        <div class="flex justify-end w-full gap-4">
          <ThemeButton />
          <UserButton />
        </div>
      </header>
      <main class="flex flex-1 flex-col gap-4 p-4 lg:gap-6 lg:p-6">
        <div class="flex items-center">
          <h1 class="text-lg font-semibold md:text-2xl">{{ route.name }}</h1>
        </div>
        <slot />
      </main>
    </div>
  </div>
</template>

<script setup lang="ts">
import { Home, Users } from "lucide-vue-next";

const navLinks = [
  { href: "/dashboard", label: "Dashboard", icon: Home },
  { href: "/clients", label: "Klienci", icon: Users },
];

const route = useRoute();

const userStore = useUserStore();
</script>
