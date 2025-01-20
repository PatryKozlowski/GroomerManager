<template>
  <Sheet>
    <SheetTrigger as-child>
      <Button variant="outline" size="icon" class="shrink-0 md:hidden">
        <Menu class="h-5 w-5" />
        <span class="sr-only">Toggle menu</span>
      </Button>
    </SheetTrigger>
    <SheetContent side="left" class="flex flex-col">
      <nav class="grid gap-4 text-lg font-medium">
        <NuxtLink
          href="/dashboard"
          class="flex items-center gap-2 font-semibold"
        >
          <!-- <Logo /> -->
          <span class="">Pies Na Glanc</span>
        </NuxtLink>
        <NuxtLink
          v-for="link in links"
          :key="link.href"
          :href="link.href"
          :class="[
            'mx-[-0.65rem] flex items-center gap-4 rounded-xl px-3 py-2 text-muted-foreground hover:text-foreground',
            route.path === link.href ? 'bg-muted' : '',
          ]"
        >
          <component :is="link.icon" class="h-4 w-4" />
          <span>{{ link.label }}</span>
        </NuxtLink>
      </nav>
    </SheetContent>
  </Sheet>
</template>

<script setup lang="ts">
import { Menu } from "lucide-vue-next";

defineProps({
  links: {
    type: Array as PropType<Array<{ href: string; label: string; icon: any }>>,
    required: true,
  },
});

const route = useRoute();
</script>
