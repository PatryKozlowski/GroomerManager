<template>
  <DropdownMenu>
    <DropdownMenuTrigger as-child>
      <Button
        variant="ghost"
        class="w-full h-8 p-0 flex justify-center items-center"
      >
        <span class="sr-only">Open menu</span>
        <MoreHorizontal class="w-4 h-4" />
      </Button>
    </DropdownMenuTrigger>
    <DropdownMenuContent align="end">
      <DropdownMenuLabel>Co chcesz zrobić?</DropdownMenuLabel>
      <DropdownMenuSeparator />
      <div class="flex flex-col gap-2">
        <Button variant="outline" class="border-none">
          <NuxtLink
            :href="`/client/${client.id}`"
            class="flex gap-2 items-center"
          >
            <Eye class="w-4 h-4 text-cyan-500" />
            Pokaz klienta
          </NuxtLink>
        </Button>
        <!-- <EditClientDialog :clientId="client.id">
          <Button variant="outline" class="border-none">
            <Pencil class="w-4 h-4 text-violet-500" />
            Edytuj klienta
          </Button>
        </EditClientDialog> -->
        <Button variant="outline" class="border-none">
          <Trash2 class="w-4 h-4 text-red-500" />
          Usuń klienta
        </Button>
      </div>
    </DropdownMenuContent>
  </DropdownMenu>
</template>

<script setup lang="ts">
import { MoreHorizontal, Eye, Pencil, Trash2 } from "lucide-vue-next";

defineProps<{
  client: {
    id: number;
  };
}>();
</script>
