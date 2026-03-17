<script setup lang="ts">
import LoadingSpinner from "@/components/loading/LoadingSpinner.vue";
import {onMounted, ref} from "vue";
import {instructorApiClient} from "@/apis/InstructorApiClient.ts";
import type {IInstructor} from "@/interfaces/IInstructor.ts";
import CustomButton from "@/components/button/CustomButton.vue";
import CustomPaper from "@/components/paper/CustomPaper.vue";
import PageHeader from "@/components/header/PageHeader.vue";

const isLoading = ref(true);
const instructors = ref<IInstructor[]>([]);

onMounted(async () => {
  await loadData();
})

const loadData = async () => {
  const resp = await instructorApiClient.getAll();

  if(resp == null)
    console.error("Error while fetching instructors");
  else
    instructors.value = resp;

  isLoading.value = false;
}
</script>

<template>
  <LoadingSpinner v-if="isLoading" text="Fahrlehrer werden geladen.." />
  <CustomPaper>
    <PageHeader>
      <h3>Fahrlehrer</h3>
      <template #description>
        <p>Lorem ipsum dolor sit amet</p>
      </template>
      <template #actions>
        <CustomButton type="secondary" :outline="true" :disabled="isLoading">Fahrlehrer eintragen</CustomButton>
      </template>
    </PageHeader>
  </CustomPaper>
</template>

<style scoped>

</style>
