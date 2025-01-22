<template>
  <p>Poczekaj az nadamy Ci odpowiednie uprawnienia</p>
</template>

<script setup lang="ts">
useHead({ title: "Groomer Manager - Twój organizer" });

definePageMeta({
  middleware: "auth",
});
</script>
