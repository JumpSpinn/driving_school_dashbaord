<script setup lang="ts">
import LoadingSpinner from "@/components/loading/LoadingSpinner.vue";
import {onMounted, ref} from "vue";
import type {IDrivingSchool} from "@/interfaces/IDrivingSchool.ts";
import {drivingSchoolApiClient} from "@/apis/DrivingSchoolApiClient.ts";
import CustomPaper from "@/components/paper/CustomPaper.vue";
import PageHeader from "@/components/header/PageHeader.vue";
import CustomButton from "@/components/button/CustomButton.vue";

const isLoading = ref(true);
const drivingSchools = ref<IDrivingSchool[]>([]);

onMounted(async () => {
  await loadData();
})

const loadData = async () => {
  const resp = await drivingSchoolApiClient.getAll();

  if(resp == null)
    console.error("Error while fetching driving schools");
  else
    drivingSchools.value = resp;

  isLoading.value = false;
}
</script>

<template>
  <LoadingSpinner v-if="isLoading" text="Fahrschulen werden geladen.." />
  <CustomPaper>
    <PageHeader>
      <h3>Fahrschulen</h3>
      <template #description>
        <p>Lorem ipsum dolor sit amet</p>
      </template>
      <template #actions>
        <CustomButton type="secondary" :outline="true" :disabled="isLoading">Fahrschule eintragen</CustomButton>
      </template>
    </PageHeader>
  </CustomPaper>
</template>

<style scoped>

</style>
