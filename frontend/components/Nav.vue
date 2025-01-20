<template>
  <nav class="grid items-start px-2 text-sm font-medium lg:px-4 gap-4">
    <NuxtLink
      v-for="link in links"
      :key="link.href"
      :href="link.href"
      :class="[
        'flex items-center gap-3 rounded-lg px-3 py-2 text-muted-foreground transition-all hover:text-primary',
        route.path === link.href ? 'bg-muted' : '',
      ]"
    >
      <component :is="link.icon" class="h-4 w-4" />
      <span>{{ link.label }}</span>
    </NuxtLink>
  </nav>
</template>

<script setup lang="ts">
defineProps({
  links: {
    type: Array as PropType<Array<{ href: string; label: string; icon: any }>>,
    required: true,
  },
});

const route = useRoute();
</script>
