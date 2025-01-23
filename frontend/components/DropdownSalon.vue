<template>
  <DropdownMenu>
    <DropdownMenuTrigger
      class="w-full h-full hover:bg-muted transition-all flex justify-center items-center"
    >
      <div class="w-full flex items-center lg:gap-4 gap-2 lg:ml-4 ml-2">
        <Avatar class="h-10 w-10 rounded-full">
          <img
            :src="activeSalon.logoPath"
            alt="Salon logo"
            class="border rounded-full"
          />
        </Avatar>
        <span class="truncate">{{ activeSalon.name }}</span>
      </div>
      <div class="flex w-full justify-end">
        <ChevronsUpDown class="mr-1" />
      </div>
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
        :class="{
          'bg-violet-500 text-white': salon.name === activeSalon.name,
          'hover:bg-gray-100': salon.name !== activeSalon.name,
        }"
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
      <AddNewSalon />
    </DropdownMenuContent>
  </DropdownMenu>
</template>
<script setup lang="ts">
import { ChevronsUpDown } from "lucide-vue-next";

const salonStore = useSalonStore();

const activeSalon = ref(salonStore.salons[0]);

function setActiveSalon(salon: (typeof salonStore.salons)[number]) {
  activeSalon.value = salon;
  salonStore.activeSalonId = activeSalon.value.id;
  salonStore.activeSalonName = activeSalon.value.name;
}

onMounted(() => {
  if (salonStore.activeSalonId) {
    activeSalon.value =
      salonStore.salons.find(
        (salon) => salon.id === salonStore.activeSalonId
      ) || salonStore.salons[0];
  }
});
</script>
