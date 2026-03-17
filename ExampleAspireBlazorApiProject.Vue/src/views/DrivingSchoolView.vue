<script setup lang="ts">
import {onMounted, ref} from "vue";
import type {IDrivingSchool} from "@/interfaces/IDrivingSchool.ts";
import {drivingSchoolApiClient} from "@/apis/DrivingSchoolApiClient.ts";
import CustomPaper from "@/components/paper/CustomPaper.vue";
import PageHeader from "@/components/header/PageHeader.vue";
import CustomButton from "@/components/button/CustomButton.vue";
import Vue3Datatable from '@bhplugin/vue3-datatable'
import {InstructorHelper} from "@/helpers/InstructorHelper.ts";
import ButtonGroup from "@/components/button/ButtonGroup.vue";

const cols = ref([
  { field: "id", title: "ID", width: "90px", filter: false },
  { field: "name", title: "Bezeichnung" },
  { field: "owner", title: "Besitzer" },
  { field: "students", title: "Fahrschüler" },
  { field: "actions", title: "Aktionen", slotMode: true, width: "160px" }
])
const rows = ref<any[]>([])
const isLoading = ref(true);
const drivingSchools = ref<IDrivingSchool[]>([]);

onMounted(async () => {
  await loadData();
})

const loadData = async () => {
  const resp = await drivingSchoolApiClient.getAll();
  drivingSchools.value = resp ?? [];
  await prepareTable();
  isLoading.value = false;
}

const prepareTable = async () => {
  const data = [];

  for(const drivingSchool of drivingSchools.value) {
    data.push({
      id: drivingSchool.id,
      name: drivingSchool.name,
      owner: InstructorHelper.getFullName(drivingSchool.owner),
      students: drivingSchool.students?.length ?? 0,
    })
  }

  rows.value = data;
}

const log = (data: IDrivingSchool) => {
  console.log(data);
}

</script>

<template>
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

  <CustomPaper>
    <Vue3Datatable
      :loading="isLoading"
      noDataContent="Keine Daten vorhanden"
      :rows="rows"
      :columns="cols"
    >
      <template #actions="data">
        <ButtonGroup>
          <CustomButton type="neutral" @click="log(data.value)">
            <i class="pi pi-eye"></i>
          </CustomButton>
          <CustomButton type="neutral" @click="log(data.value)">
            <i class="pi pi-pencil"></i>
          </CustomButton>
          <CustomButton type="neutral" @click="log(data.value)">
            <i class="pi pi-trash"></i>
          </CustomButton>
        </ButtonGroup>
      </template>
    </Vue3Datatable>
  </CustomPaper>

</template>

<style scoped>

</style>
