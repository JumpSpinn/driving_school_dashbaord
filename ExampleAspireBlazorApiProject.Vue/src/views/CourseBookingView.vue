<script setup lang="ts">
import {onMounted, ref} from "vue";
import type {ICourseBooking} from "@/interfaces/ICourseBooking.ts";
import {courseBookingApiClient} from "@/apis/CourseBookingApiClient.ts";
import CustomButton from "@/components/button/CustomButton.vue";
import CustomPaper from "@/components/paper/CustomPaper.vue";
import PageHeader from "@/components/header/PageHeader.vue";
import ButtonGroup from "@/components/button/ButtonGroup.vue";
import Vue3Datatable from '@bhplugin/vue3-datatable'
import {StudentHelper} from "@/helpers/StudentHelper.ts";
import {TheoryLessonHelper} from "@/helpers/TheoryLessonHelper.ts";
import {TimeHelper} from "@/helpers/TimeHelper.ts";

const cols = ref([
  { field: "id", title: "ID", width: "90px", filter: false },
  { field: "bookingDate", title: "Buchungsdatum" },
  { field: "student", title: "Fahrschüler" },
  { field: "theoryLesson", title: "Kurs" },
  { field: "actions", title: "Aktionen", slotMode: true, width: "160px" }
])
const rows = ref<any[]>([])
const isLoading = ref(true);
const courseBookings = ref<ICourseBooking[]>([]);

onMounted(async () => {
  await loadData();
})

const loadData = async () => {
  const resp = await courseBookingApiClient.getAll();
  courseBookings.value = resp ?? [];
  await prepareTable();
  isLoading.value = false;
}

const prepareTable = async () => {
  const data = [];

  for(const courseBooking of courseBookings.value) {
    data.push({
      id: courseBooking.id,
      bookingDate: TimeHelper.convert(courseBooking.bookingDate, "date"),
      student: StudentHelper.getFullName(courseBooking.student),
      theoryLesson: TheoryLessonHelper.getName(courseBooking.theoryLesson),
    })
  }

  rows.value = data;
}

const log = (data: ICourseBooking) => {
  console.log(data);
}

</script>

<template>
  <CustomPaper>
    <PageHeader>
      <h3>Kursbuchungen</h3>
      <template #description>
        <p>Lorem ipsum dolor sit amet</p>
      </template>
      <template #actions>
        <CustomButton type="secondary" :outline="true" :disabled="isLoading">Buchung eintragen</CustomButton>
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
