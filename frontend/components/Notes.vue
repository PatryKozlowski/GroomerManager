<template>
  <Card>
    <CardHeader>
      <CardTitle>{{ title }}</CardTitle>
      <CardDescription>{{ description }}</CardDescription>
    </CardHeader>
    <CardContent>
      <template v-if="isLoading">
        <Spinner />
      </template>
      <template v-else-if="notes.length === 0">
        <div class="flex flex-col items-center justify-center h-full gap-4">
          <PawPrint class="w-16 h-16 text-violet-500" />
          <p class="text-violet-500">{{ emptyMessage }}</p>
        </div>
      </template>
      <template v-else>
        <Accordion type="single" class="max-h-64 overflow-auto w-full" collapsible>
          <AccordionItem v-for="note in notes" :key="note.id" :value="note.id.toString()">
            <AccordionTrigger>
              {{ dayjs(note.created).format("DD.MM.YYYY") }}
            </AccordionTrigger>
            <div class="flex flex-col gap-4">
              <AccordionContent>
                {{ note.text }}
              </AccordionContent>
              <AccordionContent class="text-violet-500 text-xs">
                Stworzona przez: {{ note.createdBy }}
              </AccordionContent>
            </div>
          </AccordionItem>
        </Accordion>
      </template>
      <form class="mt-8" @submit="onSubmit">
        <FormField v-slot="{ componentField }" name="note" :validate-on-blur="false">
          <FormItem>
            <FormLabel>Dodaj notatkę</FormLabel>
            <FormControl>
              <Textarea type="text" placeholder="..." v-bind="componentField" />
            </FormControl>
            <FormMessage />
          </FormItem>
        </FormField>
        <Button type="submit" class="w-full mt-6" :disabled="isLoading">
          <Spinner v-if="isLoading" />
          <span v-else class="flex gap-2 items-center">
            <PlusCircle />
            Dodaj notatkę
          </span>
        </Button>
      </form>
    </CardContent>
  </Card>
</template>

<script setup lang="ts">
import { PawPrint, PlusCircle } from "lucide-vue-next";
import { useForm } from "vee-validate";
import type {Note} from "~/types/type";

const dayjs = useDayjs();

const form = useForm<AddNewNote>({
  validationSchema: addNewClientNoteSchema,
});

const props = defineProps<{
  title: string;
  description: string;
  emptyMessage: string;
  isLoading: boolean;
  notes: Array<Note>;
  addNewNoteCallback: (values: any, callback: () => void) => Promise<void>;
}>();

const onSubmit = form.handleSubmit(async (values) => {
  await props.addNewNoteCallback(values, successSubmitting);
});

const successSubmitting = () => {
  form.resetForm();
};
</script>