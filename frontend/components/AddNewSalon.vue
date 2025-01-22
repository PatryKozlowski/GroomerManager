<template>
  <AlertDialog :open="open">
    <AlertDialogTrigger as-child>
      <Button
        @click="open = true"
        type="button"
        variant="outline"
        class="w-full"
      >
        <PlusCircle />
        Dodaj nowy salon
      </Button>
    </AlertDialogTrigger>
    <AlertDialogContent class="sm:max-w-[425px]">
      <AlertDialogHeader>
        <AlertDialogTitle>Dodaj nowy salon</AlertDialogTitle>
      </AlertDialogHeader>

      <form id="dialogForm" @submit="onSubmit" class="flex flex-col gap-4">
        <FormField
          v-slot="{ componentField }"
          name="name"
          :validate-on-blur="false"
        >
          <FormItem>
            <FormLabel>Nazwa salonu</FormLabel>
            <FormControl>
              <Input type="text" placeholder="..." v-bind="componentField" />
            </FormControl>
            <FormMessage />
          </FormItem>
        </FormField>
        <FormField name="logo" :validate-on-blur="false">
          <FormItem>
            <FormLabel>Logo salonu</FormLabel>
            <FormControl>
              <Input
                type="file"
                accept="image/png"
                @change="
                  form.setFieldValue(
                    'logo',
                    $event.target.files[0] || undefined
                  )
                "
              />
            </FormControl>
            <FormMessage />
          </FormItem>
        </FormField>
      </form>

      <AlertDialogFooter>
        <Button type="submit" form="dialogForm">
          <Spinner v-if="salonStore.isLoading" />
          <Save v-else />
          Zapisz
        </Button>
        <AlertDialogCancel @click="open = false">Anuluj</AlertDialogCancel>
      </AlertDialogFooter>
    </AlertDialogContent>
  </AlertDialog>
</template>

<script setup lang="ts">
import { PlusCircle, Save } from "lucide-vue-next";
import { useForm } from "vee-validate";

const { open } = useDialogHelper();

const form = useForm<AddSalonForm>({
  validationSchema: addSalonFormSchema,
});

const successSubmitting = () => {
  form.resetForm();
  open.value = false;
};

const salonStore = useSalonStore();

const onSubmit = form.handleSubmit(async (values) => {
  const formData = new FormData();
  formData.append("name", values.name);
  formData.append("logo", values.logo);

  await salonStore.addNewSalon(formData, successSubmitting);
});
</script>
