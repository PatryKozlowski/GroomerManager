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
      <div class="flex flex-col gap-4">
        <Button variant="outline" class="border-none">
          <NuxtLink
            :href="`/dashboard/clients/${client.id}?salonId=${$route.query.salonId}`"
            class="flex gap-2 items-center"
          >
            <Eye class="w-4 h-4 text-cyan-500" />
            Pokaz klienta
          </NuxtLink>
        </Button>
        <EditClientDialog
          :clientId="client.id"
          button-variant="outline"
          ,
          button-class="border-none"
        />
        <DeleteClientDialog
          :clientId="client.id"
          button-variant="outline"
          button-class="border-none"
        />
      </div>
    </DropdownMenuContent>
  </DropdownMenu>
</template>

<script setup lang="ts">
import { MoreHorizontal, Eye, Pencil, Trash2 } from "lucide-vue-next";

defineProps<{
  client: {
    id: string;
  };
}>();
</script>
