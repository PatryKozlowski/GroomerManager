<template>
  <Card class="w-[350px]">
    <CardHeader>
      <CardTitle>Klient</CardTitle>
      <CardDescription>Informacje o Twoim kliencie</CardDescription>
    </CardHeader>
    <template v-if="clientStore.isLoading">
      <Spinner />
    </template>
    <template v-else>
      <CardContent>
        <div class="grid items-center w-full gap-4">
          <div class="flex flex-col space-y-1.5">
            <Label for="name">Imię i nazwisko</Label>
            <Input
                id="name"
                :defaultValue="clientStore.client.name"
                disabled
            />
          </div>
          <div class="flex flex-col space-y-1.5">
            <Label for="phone">Numer telefonu</Label>
            <Input
                id="phone"
                :defaultValue="clientStore.client.phoneNumber"
                disabled
            />
          </div>
          <div class="flex flex-col space-y-1.5">
            <Label for="email">Adres email</Label>
            <Input
                id="email"
                :defaultValue="
                  clientStore.client.email ?? 'Edytuj aby dodać email'
                "
                disabled
            />
          </div>
        </div>
      </CardContent>
      <CardFooter class="flex justify-between px-6 pb-6">
        <EditClientDialog
            :clientId="
              Array.isArray(route.params.id)
                ? route.params.id[0]
                : route.params.id
            "
        />
        <DeleteClientDialog
            :clientId="
              Array.isArray(route.params.id)
                ? route.params.id[0]
                : route.params.id
            "
            :onSuccessCallback="redirectedToClientsPageAfterDeleting"
        />
      </CardFooter>
    </template>
  </Card>
</template>

<script setup lang="ts">
const route = useRoute();
const router = useRouter();
const salonStore = useSalonStore();
const clientStore = useClientsStore();


const redirectedToClientsPageAfterDeleting = () => {
  router.push({
    path: "/dashboard/clients",
    query: {
      ...router.currentRoute.value.query,
      salonId: salonStore.activeSalonId,
    },
  });
};
</script>