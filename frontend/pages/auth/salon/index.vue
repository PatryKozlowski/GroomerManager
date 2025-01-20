<template>
  <div class="flex flex-col gap-12">
    <h3 class="text-center text-violet-500 text-2xl">Dodaj pierwszy salon!</h3>
    <Form
      v-slot="{ meta, values, validate }"
      as=""
      keep-values
      :validation-schema="toTypedSchema(addSalonFormSchema[stepIndex - 1])"
    >
      <Stepper
        v-slot="{ isNextDisabled, isPrevDisabled, nextStep, prevStep }"
        v-model="stepIndex"
        class="block w-full"
      >
        <form
          @submit="
            (e) => {
              e.preventDefault();
              validate();

              if (stepIndex === steps.length && meta.valid) {
                onSubmit(values as AddSalonForm);
              }
            }
          "
        >
          <div class="flex w-full flex-start gap-2">
            <StepperItem
              v-for="step in steps"
              :key="step.step"
              v-slot="{ state }"
              class="relative flex w-full flex-col items-center justify-center"
              :step="step.step"
            >
              <StepperSeparator
                v-if="step.step !== steps[steps.length - 1].step"
                class="absolute left-[calc(50%+20px)] right-[calc(-50%+10px)] top-5 block h-0.5 shrink-0 rounded-full bg-muted group-data-[state=completed]:bg-primary"
              />

              <StepperTrigger as-child>
                <Button
                  :variant="
                    state === 'completed' || state === 'active'
                      ? 'default'
                      : 'outline'
                  "
                  size="icon"
                  class="z-10 rounded-full shrink-0"
                  :class="[
                    state === 'active' &&
                      'ring-2 ring-ring ring-offset-2 ring-offset-background',
                  ]"
                  :disabled="state !== 'completed' && !meta.valid"
                >
                  <Check v-if="state === 'completed'" class="size-5" />
                  <Circle v-if="state === 'active'" />
                  <Dot v-if="state === 'inactive'" />
                </Button>
              </StepperTrigger>

              <div class="mt-5 flex flex-col items-center text-center">
                <StepperTitle
                  :class="[state === 'active' && 'text-primary']"
                  class="text-sm font-semibold transition lg:text-base"
                >
                  {{ step.title }}
                </StepperTitle>
                <StepperDescription
                  :class="[state === 'active' && 'text-primary']"
                  class="sr-only text-xs text-muted-foreground transition md:not-sr-only lg:text-sm"
                >
                  {{ step.description }}
                </StepperDescription>
              </div>
            </StepperItem>
          </div>

          <div class="flex flex-col gap-4 mt-4">
            <template v-if="stepIndex === 1">
              <FormField
                v-slot="{ componentField }"
                name="name"
                :validate-on-blur="false"
              >
                <FormItem>
                  <FormLabel>Nazwa salonu</FormLabel>
                  <FormControl>
                    <Input type="text" v-bind="componentField" />
                  </FormControl>
                  <FormMessage />
                </FormItem>
              </FormField>
            </template>

            <template v-if="stepIndex === 2">
              <FormField
                name="logo"
                :validate-on-blur="false"
                v-slot="{ setValue }"
              >
                <FormItem>
                  <FormLabel>Logo salonu</FormLabel>
                  <FormControl>
                    <Input
                      type="file"
                      @change="setValue($event.target.files?.[0] || undefined)"
                      accept="image/png"
                    />
                  </FormControl>
                  <FormMessage />
                </FormItem>
              </FormField>
            </template>
          </div>

          <div class="flex items-center justify-between mt-4">
            <Button
              :disabled="isPrevDisabled"
              variant="outline"
              size="sm"
              @click="prevStep()"
            >
              Cofnij
            </Button>
            <div class="flex items-center gap-3">
              <Button
                v-if="stepIndex !== 2"
                :type="meta.valid ? 'button' : 'submit'"
                :disabled="isNextDisabled"
                size="sm"
                @click="meta.valid && nextStep()"
              >
                Dalej
              </Button>
              <Button v-if="stepIndex === 2" size="sm" type="submit">
                <Plus />
                Dodaj
              </Button>
            </div>
          </div>
        </form>
      </Stepper>
    </Form>
  </div>
</template>

<script setup lang="ts">
import { toTypedSchema } from "@vee-validate/zod";
import { Check, Circle, Dot, Plus } from "lucide-vue-next";

useHead({ title: "Groomer Manager - Dodaj salon" });

definePageMeta({
  middleware: "auth",
});

const stepIndex = ref(1);
const steps = [
  {
    step: 1,
    title: "Nazwa salonu",
    description: "Podaj nazwę swojego salonu",
  },
  {
    step: 2,
    title: "Logo salonu",
    description: "Wgraj logo swojego salonu",
  },
];

function onSubmit(values: AddSalonForm) {
  console.log("add salon ", values);
}
</script>
