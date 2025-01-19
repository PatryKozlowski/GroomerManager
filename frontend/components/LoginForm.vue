<template>
  <form @submit="onSubmit" class="w-full max-w-sm">
    <Card>
      <CardHeader>
        <CardTitle class="text-2xl text-center mb-2 text-violet-500">
          Groomer Manager
        </CardTitle>
        <CardDescription>
          Podaj email oraz hasło aby się zalogować
        </CardDescription>
      </CardHeader>
      <CardContent class="grid gap-4">
        <FormField v-slot="{ componentField }" name="email">
          <FormItem>
            <FormLabel>Email</FormLabel>
            <FormControl>
              <Input
                type="email"
                placeholder="m@example.com"
                v-bind="componentField"
              />
            </FormControl>
            <FormMessage />
          </FormItem>
        </FormField>
        <FormField v-slot="{ componentField }" name="password">
          <FormItem>
            <FormLabel>Hasło</FormLabel>
            <FormControl>
              <Input type="password" v-bind="componentField" />
            </FormControl>
            <FormMessage />
          </FormItem>
        </FormField>
      </CardContent>
      <CardFooter>
        <Button type="submit" class="w-full" :disabled="authStore.isLoading">
          <Spinner v-if="authStore.isLoading" />
          <span v-else>Zaloguj się</span>
        </Button>
      </CardFooter>
    </Card>
  </form>
</template>

<script setup lang="ts">
import { useForm } from "vee-validate";

const authStore = useAuthStore();

const form = useForm<LoginRequestDto>({
  validationSchema: loginFormSchema,
  initialValues: {
    email: "",
    password: "",
  },
});

const onSubmit = form.handleSubmit((values) => {});
</script>
