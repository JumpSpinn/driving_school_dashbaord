<script setup lang="ts">
import LoadingSpinner from "@/components/loading/LoadingSpinner.vue";
import {onMounted, ref} from "vue";
import type {IStudent} from "@/interfaces/IStudent.ts";
import {studentApiClient} from "@/apis/StudentApiClient.ts";
import CustomButton from "@/components/button/CustomButton.vue";
import CustomPaper from "@/components/paper/CustomPaper.vue";
import PageHeader from "@/components/header/PageHeader.vue";

const isLoading = ref(true);
const students = ref<IStudent[]>([]);

onMounted(async () => {
  await loadData();
})

const loadData = async () => {
  const resp = await studentApiClient.getAll();

  if(resp == null)
    console.error("Error while fetching students");
  else
    students.value = resp;

  isLoading.value = false;
}
</script>

<template>
  <LoadingSpinner v-if="isLoading" text="Fahrschüler werden geladen.." />
  <CustomPaper>
    <PageHeader>
      <h3>Fahrschüler</h3>
      <template #description>
        <p>Lorem ipsum dolor sit amet</p>
      </template>
      <template #actions>
        <CustomButton type="secondary" :outline="true" :disabled="isLoading">Fahrschüler eintragen</CustomButton>
      </template>
    </PageHeader>
  </CustomPaper>
</template>

<style scoped>

</style>
