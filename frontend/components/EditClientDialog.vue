<template>
  <AlertDialog>
    <AlertDialogTrigger as-child>
      <Button
        @click="open = true"
        type="button"
        :variant="buttonVariant"
        :class="buttonClass"
      >
        <Pencil class="w-4 h-4 text-violet-500" />
        Edytuj klienta
      </Button>
    </AlertDialogTrigger>
    <AlertDialogContent class="sm:max-w-[425px]">
      <AlertDialogHeader>
        <AlertDialogTitle>Edytuj klineta</AlertDialogTitle>
        <AlertDialogDescription>Chcesz coś zmienic?</AlertDialogDescription>
      </AlertDialogHeader>

      <form :id="'dialogForm-' + props.clientId" @submit="onSubmit" class="flex flex-col gap-4">
        <FormField v-slot="{ componentField }" name="firstName">
          <FormItem>
            <FormLabel>Imię klienta</FormLabel>
            <FormControl>
              <Input type="text" placeholder="Jan" v-bind="componentField" />
            </FormControl>
            <FormMessage />
          </FormItem>
        </FormField>
        <FormField v-slot="{ componentField }" name="lastName">
          <FormItem>
            <FormLabel>Nazwisko klienta</FormLabel>
            <FormControl>
              <Input
                type="text"
                placeholder="Kowalski"
                v-bind="componentField"
              />
            </FormControl>
            <FormMessage />
          </FormItem>
        </FormField>
        <FormField v-slot="{ componentField }" name="phoneNumber">
          <FormItem>
            <FormLabel>Numer telefonu klienta</FormLabel>
            <FormControl>
              <Input
                type="text"
                placeholder="123 444 555"
                v-bind="componentField"
              />
            </FormControl>
            <FormMessage />
          </FormItem>
        </FormField>
        <FormField v-slot="{ componentField }" name="email">
          <FormItem>
            <FormLabel>Adres email klienta</FormLabel>
            <FormControl>
              <Input
                type="email"
                placeholder="jankowalski@examle.com"
                v-bind="componentField"
              />
            </FormControl>
            <FormMessage />
          </FormItem>
        </FormField>
      </form>

      <AlertDialogFooter>
        <Button type="submit" :form="'dialogForm-' + props.clientId">
          <Spinner v-if="clientsStore.isLoading" />
          <Save v-else />
          Zapisz
        </Button>
        <AlertDialogCancel @click="open = false">Anuluj</AlertDialogCancel>
      </AlertDialogFooter>
    </AlertDialogContent>
  </AlertDialog>
</template>

<script setup lang="ts">
import { useForm } from "vee-validate";
import type { EditClient } from "~/types/type";
import { Pencil, Save } from "lucide-vue-next";
import { editClientSchema } from "~/utils/schemas";

const form = useForm<EditClient>({
  validationSchema: editClientSchema,
});

const { open } = useDialogHelper();

const clientsStore = useClientsStore();

const successSubmitting = () => {
  form.resetForm();
  open.value = false;
};

const onSubmit = form.handleSubmit(async (values) => {
  const valuesWithId = {
    ...values,
    id: props.clientId!,}
  await clientsStore.editClient(valuesWithId , successSubmitting);
});

const props = defineProps({
  clientId: {
    type: String,
  },
  buttonVariant: {
    type: String as PropType<
      "link" | "default" | "destructive" | "outline" | "secondary" | "ghost"
    >,
    default: "default",
  },
  buttonClass: {
    type: String,
    default: "",
  },
});

onMounted(() => {
  if (props.clientId !== undefined) {
    const client = clientsStore.getStoredClientById(props.clientId);

    form.setFieldValue("id", props.clientId ?? "");
    form.setFieldValue("firstName", client?.firstName ?? "");
    form.setFieldValue("lastName", client?.lastName ?? "");
    form.setFieldValue("phoneNumber", client?.phoneNumber ?? "");
    form.setFieldValue("email", client?.email ?? "", false);
  }
});
</script>
