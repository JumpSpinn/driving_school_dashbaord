<script setup lang="ts">
import LoadingSpinner from "@/components/loading/LoadingSpinner.vue";
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
  await loadDrivingSchoolData();
  await loadStudentsData();
  await loadInstructorsData();
  await loadTheoryLessonsData();
  await loadCourseBookingsData();
  isLoading.value = false;
}

const loadDrivingSchoolData = async () => {
  const resp = await drivingSchoolApiClient.getAll();

  if(resp == null)
    console.error("Error while fetching driving schools");
  else
    drivingSchools.value = resp;
}

const loadStudentsData = async () => {
  const resp = await studentApiClient.getAll();

  if(resp == null)
    console.error("Error while fetching students");
  else
    students.value = resp;
}

const loadInstructorsData = async () => {
  const resp = await instructorApiClient.getAll();

  if(resp == null)
    console.error("Error while fetching instructors");
  else
    instructors.value = resp;
}

const loadTheoryLessonsData = async () => {
  const resp = await theoryLessonApiClient.getAll();

  if(resp == null)
    console.error("Error while fetching theory lessons");
  else
    theoryLessons.value = resp;
}

const loadCourseBookingsData = async () => {
  const resp = await courseBookingApiClient.getAll();

  if(resp == null)
    console.error("Error while fetching course bookings");
  else
    courseBookings.value = resp;
}
</script>

<template>
  <LoadingSpinner v-if="isLoading" text="Dashboard wird geladen.." />
  <h1>Dashbaord</h1>
</template>

<style scoped>

</style>
