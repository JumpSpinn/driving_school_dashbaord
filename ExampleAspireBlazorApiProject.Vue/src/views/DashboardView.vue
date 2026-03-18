<script setup lang="ts">
import type {IDropdownItem} from "@/interfaces/IDropdownItem.ts";
import {useField} from "@/composables/useField.ts";
import {Rules} from "@/helpers/ValidationRules.ts";
import {useForm} from "@/composables/useForm.ts";
import {useDrivingSchoolStore} from "@/stores/drivingSchoolStore.ts";
import {useStudentStore} from "@/stores/studentStore.ts";
import {useInstructorStore} from "@/stores/instructorStore.ts";
import {useTheoryLessonStore} from "@/stores/theoryLessonStore.ts";
import {useCourseBookingStore} from "@/stores/courseBookingStore.ts";

const isLoading = ref(true);
const drivingSchoolStore = useDrivingSchoolStore();
const studentStore = useStudentStore();
const instructorStore = useInstructorStore();
const theoryLessonStore = useTheoryLessonStore();
const courseBookingStore = useCourseBookingStore();

onMounted(async () => {
  await loadAllData();
})

const loadAllData = async () => {
  await Promise.all([
    drivingSchoolStore.fetchAll(),
    studentStore.fetchAll(),
    instructorStore.fetchAll(),
    theoryLessonStore.fetchAll(),
    courseBookingStore.fetchAll(),
  ]);

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
        <CustomButton type="primary" @click="handleSubmit">Senden</CustomButton>
      </ButtonGroup>
    </template>
  </Modal>

</template>

<style scoped>

</style>
