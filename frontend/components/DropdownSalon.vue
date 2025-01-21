<template>
  <DropdownMenu>
    <DropdownMenuTrigger
      class="w-full h-full hover:bg-muted transition-all flex justify-center items-center p-4"
    >
      <div class="w-full flex items-center gap-4">
        <Avatar class="h-10 w-10 rounded-full">
          <img
            :src="activeSalon.logoPath"
            alt="Salon logo"
            class="border rounded-full"
          />
        </Avatar>
        <span class="truncate font-semibold">{{ activeSalon.name }}</span>
      </div>
      <ChevronsUpDown class="mr-4" />
    </DropdownMenuTrigger>
    <DropdownMenuContent
      class="min-w-56 rounded-lg"
      align="start"
      side="bottom"
      :side-offset="4"
    >
      <DropdownMenuLabel class="text-xs text-muted-foreground"
        >Twoje salony</DropdownMenuLabel
      >
      <DropdownMenuSeparator />
      <DropdownMenuItem
        v-for="(salon, index) in salonStore.salons"
        :key="salon.name"
        class="gap-2 p-2"
        @click="setActiveSalon(salon)"
      >
        <div class="flex size-6 items-center justify-center">
          <Avatar class="h-4 w-4 rounded-full">
            <img :src="activeSalon.logoPath" alt="Mini salon logo" />
          </Avatar>
        </div>
        {{ salon.name }}
      </DropdownMenuItem>
      <DropdownMenuSeparator />
      <DropdownMenuItem class="gap-2 p-2">
        <div
          class="flex size-6 items-center justify-center rounded-md border bg-background"
        >
          <Plus class="size-4" />
        </div>
        <div class="font-medium text-muted-foreground">Dodaj nowy salon</div>
      </DropdownMenuItem>
    </DropdownMenuContent>
  </DropdownMenu>
</template>
<script setup lang="ts">
import { ChevronsUpDown, Plus } from "lucide-vue-next";

const salonStore = useSalonStore();

const activeSalon = ref(salonStore.salons[0]);

function setActiveSalon(salon: (typeof salonStore.salons)[number]) {
  activeSalon.value = salon;
  salonStore.activeSalonId = activeSalon.value.id;
}
</script>
