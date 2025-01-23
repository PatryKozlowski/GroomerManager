<template>
  <AlertDialog :open="open">
    <AlertDialogTrigger as-child>
      <Button @click="open = true" type="button">
        <PlusCircle />
        Dodaj klienta
      </Button>
    </AlertDialogTrigger>
    <AlertDialogContent class="sm:max-w-[425px]">
      <AlertDialogHeader>
        <AlertDialogTitle>Dodaj nowego klineta</AlertDialogTitle>
        <AlertDialogDescription>
          Kolejny klient? Super! Dodaj go
        </AlertDialogDescription>
      </AlertDialogHeader>

      <form id="dialogForm" @submit="onSubmit" class="flex flex-col gap-4">
        <FormField
          v-slot="{ componentField }"
          name="firstName"
          :validate-on-blur="false"
        >
          <FormItem>
            <FormLabel>Imię klienta</FormLabel>
            <FormControl>
              <Input type="text" placeholder="Jan" v-bind="componentField" />
            </FormControl>
            <FormMessage />
          </FormItem>
        </FormField>
        <FormField
          v-slot="{ componentField }"
          name="lastName"
          :validate-on-blur="false"
        >
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
        <FormField
          v-slot="{ componentField }"
          name="phoneNumber"
          :validate-on-blur="false"
        >
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
        <FormField
          v-slot="{ componentField }"
          name="email"
          :validate-on-blur="false"
        >
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
        <Button type="submit" form="dialogForm">
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
import { toTypedSchema } from "@vee-validate/zod";
import { PlusCircle, Save } from "lucide-vue-next";
import { useForm } from "vee-validate";
import * as z from "zod";
import type { AddNewClient } from "~/types/type";

const { open } = useDialogHelper();

const formSchema = toTypedSchema(
  z.object({
    firstName: z
      .string({ message: "Imię jest wymagane" })
      .min(1, { message: "Imię musi mieć co najmniej 2 znaki" })
      .max(50, { message: "Imię nie może mieć więcej niż 50 znaków" }),
    lastName: z
      .string({ message: "Nazwisko jest wymagane" })
      .min(2, { message: "Nazwisko musi mieć co najmniej 2 znaki" })
      .max(50, { message: "Nazwisko nie może mieć więcej niż 50 znaków" }),
    phoneNumber: z
      .string({ message: "Numer telefonu jest wymagane" })
      .min(9, { message: "Numer telefonu musi mieć co najmniej 9 cyfry" })
      .max(9, { message: "Numer telefonu nie może mieć więcej niż 9 cyfr" }),
    email: z
      .string()
      .email({ message: "Podaj poprawny adres email" })
      .optional(),
  })
);

const form = useForm<AddNewClient>({
  validationSchema: formSchema,
});

const clientsStore = useClientsStore();

const successSubmitting = () => {
  form.resetForm();
  open.value = false;
};

const onSubmit = form.handleSubmit(async (values) => {
  await clientsStore.addNewClient(values, successSubmitting);
});
</script>
