<template>
  <LoginForm />
</template>

<script setup lang="ts">
useHead({ title: "Groomer Manager - Auth" });

definePageMeta({
  middleware: "guest",
});
</script>
