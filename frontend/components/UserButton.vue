<template>
  <DropdownMenu>
    <DropdownMenuTrigger as-child>
      <Button variant="outline" size="icon" class="h-10 w-10">
        <User />
      </Button>
    </DropdownMenuTrigger>
    <DropdownMenuContent
      class="w-[--radix-dropdown-menu-trigger-width] min-w-56 rounded-lg"
      side="bottom"
      align="end"
      :side-offset="4"
    >
      <DropdownMenuLabel class="p-0 font-normal">
        <div class="flex items-center gap-2 px-1 py-1.5 text-left text-sm">
          <Avatar class="h-8 w-8 rounded-lg">
            <!-- <AvatarFallback class="rounded-lg">{{
              userStore.user?.initials
            }}</AvatarFallback> -->
            <AvatarFallback class="rounded-lg">PK</AvatarFallback>
          </Avatar>
          <div class="grid flex-1 text-left text-sm leading-tight">
            <span class="truncate font-semibold">{{
              userStore.user?.fullName
            }}</span>
            <span class="truncate text-xs">{{ userStore.user?.email }}</span>
          </div>
        </div>
      </DropdownMenuLabel>
      <DropdownMenuSeparator />
      <DropdownMenuGroup>
        <DropdownMenuItem>
          <BadgeCheck />
          Konto
        </DropdownMenuItem>
      </DropdownMenuGroup>
      <DropdownMenuSeparator />
      <DropdownMenuItem
        @click="authStore.logoutUser()"
        class="hover:cursor-pointer"
      >
        <LogOut />
        Wyloguj się
      </DropdownMenuItem>
    </DropdownMenuContent>
  </DropdownMenu>
</template>

<script setup lang="ts">
import { BadgeCheck, LogOut, User } from "lucide-vue-next";

const authStore = useAuthStore();
const userStore = useUserStore();
</script>
