<script setup lang="ts">
import LoadingSpinner from "@/components/loading/LoadingSpinner.vue";
import {onMounted, ref} from "vue";
import type {ITheoryLesson} from "@/interfaces/ITheoryLesson.ts";
import {theoryLessonApiClient} from "@/apis/TheoryLessonApiClient.ts";
import CustomButton from "@/components/button/CustomButton.vue";
import CustomPaper from "@/components/paper/CustomPaper.vue";
import PageHeader from "@/components/header/PageHeader.vue";

const isLoading = ref(true);
const theoryLessons = ref<ITheoryLesson[]>([]);

onMounted(async () => {
  await loadData();
})

const loadData = async () => {
  const resp = await theoryLessonApiClient.getAll();

  if(resp == null)
    console.error("Error while fetching theory lessons");
  else
    theoryLessons.value = resp;

  isLoading.value = false;
}
</script>

<template>
  <LoadingSpinner v-if="isLoading" text="Kurse werden geladen.." />
  <CustomPaper>
    <PageHeader>
      <h3>Kurse</h3>
      <template #description>
        <p>Lorem ipsum dolor sit amet</p>
      </template>
      <template #actions>
        <CustomButton type="secondary" :outline="true" :disabled="isLoading">Kurs eintragen</CustomButton>
      </template>
    </PageHeader>
  </CustomPaper>
</template>

<style scoped>

</style>
