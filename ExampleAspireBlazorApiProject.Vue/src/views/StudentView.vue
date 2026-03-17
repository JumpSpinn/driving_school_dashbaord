<script setup lang="ts">
import {onMounted, ref} from "vue";
import type {IStudent} from "@/interfaces/IStudent.ts";
import {studentApiClient} from "@/apis/StudentApiClient.ts";
import CustomButton from "@/components/button/CustomButton.vue";
import CustomPaper from "@/components/paper/CustomPaper.vue";
import PageHeader from "@/components/header/PageHeader.vue";
import Vue3Datatable from '@bhplugin/vue3-datatable'
import ButtonGroup from "@/components/button/ButtonGroup.vue";
import {StudentHelper} from "@/helpers/StudentHelper.ts";
import {TimeHelper} from "@/helpers/TimeHelper.ts";

const cols = ref([
  { field: "id", title: "ID", width: "90px", filter: false },
  { field: "fullName", title: "Vor- Nachname" },
  { field: "mail", title: "E-Mail" },
  { field: "phone", title: "Telefon" },
  { field: "license", title: "Klasse" },
  { field: "examDate", title: "Prüfungsdatum" },
  { field: "hasPassed", title: "Bestanden" },
  { field: "drivingSchool", title: "Fahrschule" },
  { field: "actions", title: "Aktionen", slotMode: true, width: "160px" }
])
const rows = ref<any[]>([])
const isLoading = ref(true);
const students = ref<IStudent[]>([]);

onMounted(async () => {
  await loadData();
})

const loadData = async () => {
  const resp = await studentApiClient.getAll();
  students.value = resp ?? [];
  await prepareTable();
  isLoading.value = false;
}

const prepareTable = async () => {
  const data = [];

  for(const student of students.value) {
    data.push({
      id: student.id,
      fullName: StudentHelper.getFullName(student),
      mail: StudentHelper.getMail(student),
      phone: StudentHelper.getPhone(student),
      license: 0,
      examDate: TimeHelper.convert(student.examDate, "date"),
      hasPassed: student.hasPassed ? "Ja" : "in Ausbildung",
      drivingSchool: StudentHelper.getDrivingSchoolName(student),
    })
  }

  rows.value = data;
}

const log = (data: IStudent) => {
  console.log(data);
}
</script>

<template>
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
