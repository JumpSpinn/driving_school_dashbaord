<script setup lang="ts">
import {onMounted, ref} from "vue";
import {theoryLessonApiClient} from "@/apis/TheoryLessonApiClient.ts";
import type {IDrivingSchool} from "@/interfaces/IDrivingSchool.ts";
import type {IStudent} from "@/interfaces/IStudent.ts";
import type {IInstructor} from "@/interfaces/IInstructor.ts";
import type {ITheoryLesson} from "@/interfaces/ITheoryLesson.ts";
import type {ICourseBooking} from "@/interfaces/ICourseBooking.ts";
import {drivingSchoolApiClient} from "@/apis/DrivingSchoolApiClient.ts";
import {studentApiClient} from "@/apis/StudentApiClient.ts";
import {instructorApiClient} from "@/apis/InstructorApiClient.ts";
import {courseBookingApiClient} from "@/apis/CourseBookingApiClient.ts";
import {ApiHelper} from "@/helpers/ApiHelper.ts";
import CustomPaper from "@/components/paper/CustomPaper.vue";
import PageHeader from "@/components/header/PageHeader.vue";
import CustomTextInput from "@/components/input/CustomTextInput.vue";
import Modal from "@/components/modal/Modal.vue";
import CustomToggle from "@/components/input/CustomToggle.vue";
import CustomDropdown from "@/components/input/CustomDropdown.vue";
import type {IDropdownItem} from "@/interfaces/IDropdownItem.ts";

const isLoading = ref(true);
const drivingSchools = ref<IDrivingSchool[]>([]);
const students = ref<IStudent[]>([]);
const instructors = ref<IInstructor[]>([]);
const theoryLessons = ref<ITheoryLesson[]>([]);
const courseBookings = ref<ICourseBooking[]>([]);

onMounted(async () => {
  await loadAllData();
})

const loadAllData = async () => {
  drivingSchools.value = await ApiHelper.getAll(drivingSchoolApiClient);
  students.value = await ApiHelper.getAll(studentApiClient);
  instructors.value = await ApiHelper.getAll(instructorApiClient);
  theoryLessons.value = await ApiHelper.getAll(theoryLessonApiClient);
  courseBookings.value = await ApiHelper.getAll(courseBookingApiClient);
  isLoading.value = false;
}

const dropdownItems = ref<IDropdownItem[]>([
  { label: "Option 1", value: "1" },
  { label: "Option 2", value: "2" },
  { label: "Option 3", value: "3" },
])
const dropdownSelected = ref();

</script>

<template>
  <CustomPaper>
    <PageHeader>
      <h3>Dashboard</h3>
    </PageHeader>
  </CustomPaper>

  <Modal :open="true">
    <template #header>Input Test</template>
    <template #content>
      <CustomTextInput :vertical="true" label="Benutzername:" />
      <CustomTextInput :vertical="false" label="Benutzername:" />
      <CustomToggle label="Prüfung bestanden" />
      <CustomDropdown label="Klasse wählen" :options="dropdownItems" v-model="dropdownSelected" :search-on="true"></CustomDropdown>
    </template>
  </Modal>

  <CustomPaper>
    <div>
    </div>
  </CustomPaper>

</template>

<style scoped>

</style>
