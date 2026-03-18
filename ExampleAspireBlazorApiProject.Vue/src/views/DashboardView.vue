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
import {useField} from "@/composables/useField.ts";
import {Rules} from "@/helpers/ValidationRules.ts";
import {useForm} from "@/composables/useForm.ts";
import CustomButton from "@/components/button/CustomButton.vue";
import ButtonGroup from "@/components/button/ButtonGroup.vue";

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

// Form tests
const dropdownItems = ref<IDropdownItem[]>([
  { label: "Option 1", value: "1" },
  { label: "Option 2", value: "2" },
  { label: "Option 3", value: "3" },
])

const username = useField([
  Rules.required(),
  Rules.minLength(3),
  Rules.maxLength(50),
])

const category = useField([
  Rules.required(),
])

const { validate, reset } = useForm([username, category]);

const handleSubmit = () => {
  if(!validate()) {
    console.log("Validation failed");
    return;
  }

  console.log("Benutzername: ", username.value);
  console.log("Kategorie: ", category.value);
}

</script>

<template>
  <CustomPaper>
    <PageHeader>
      <h3>Dashboard</h3>
    </PageHeader>
  </CustomPaper>

  <Modal :open="false">
    <template #header>Input Test</template>
    <template #content>
      <form @submit.prevent="handleSubmit">
        <CustomTextInput label="Benutzername:" :required="true" id="username" v-model="username.value" :error="username.error" />
        <CustomDropdown label="Kategorie wählen:" :options="dropdownItems" v-model="category.value" :required="true" :error="category.error" />
      </form>
    </template>
    <template #actions>
      <ButtonGroup>
        <CustomButton type="primary" @click="handleSubmit">primary</CustomButton>
        <CustomButton type="secondary" @click="handleSubmit">secondary</CustomButton>
        <CustomButton type="neutral" @click="handleSubmit">neutral</CustomButton>
        <CustomButton type="success" @click="handleSubmit">success</CustomButton>
        <CustomButton type="info" @click="handleSubmit">info</CustomButton>
        <CustomButton type="warn" @click="handleSubmit">warn</CustomButton>
        <CustomButton type="error" @click="handleSubmit">error</CustomButton>
      </ButtonGroup>
    </template>
  </Modal>

</template>

<style scoped>

</style>
