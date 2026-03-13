<script setup lang="ts">
import LoadingSpinner from "@/components/loading/LoadingSpinner.vue";
import {onMounted, ref} from "vue";

const isLoading = ref(true);

onMounted(async () => {
  await new Promise(resolve => setTimeout(resolve, 2000));
  isLoading.value = false;
})
</script>

<template>
  <h1>DrivingSchoolView</h1>
  <LoadingSpinner v-if="isLoading" text="Fahrschulen werden geladen.." />
</template>

<style scoped>

</style>
