<script setup lang="ts">
import type {
  ICourseBooking,
  ICourseBookingBase,
} from "@/interfaces/ICourseBooking.ts";
import {courseBookingApiClient} from "@/apis/CourseBookingApiClient.ts";
import Vue3Datatable from '@bhplugin/vue3-datatable'
import {StudentHelper} from "@/helpers/StudentHelper.ts";
import {TheoryLessonHelper} from "@/helpers/TheoryLessonHelper.ts";
import {TimeHelper} from "@/helpers/TimeHelper.ts";
import {ModalHelper} from "@/helpers/ModalHelper.ts";
import {ApiHelper} from "@/helpers/ApiHelper.ts";
import {ModalType} from "@/enums/ModalType.ts";
import {useCourseBookingStore} from "@/stores/courseBookingStore.ts";
import {useField} from "@/composables/useField.ts";
import {Rules} from "@/helpers/ValidationRules.ts";
import {useForm} from "@/composables/useForm.ts";
import {useStudentStore} from "@/stores/studentStore.ts";
import {useTheoryLessonStore} from "@/stores/theoryLessonStore.ts";

const cols = ref([
  { field: "id", title: "ID", width: "90px", filter: false },
  { field: "bookingDate", title: "Buchungsdatum" },
  { field: "student", title: "Fahrschüler" },
  { field: "theoryLesson", title: "Kurs" },
  { field: "actions", title: "Aktionen", slotMode: true, width: "160px" }
])
const rows = ref<any[]>([])
const isLoading = ref(true);
const courseBookingStore = useCourseBookingStore();
const studentStore = useStudentStore();
const theoryLessonStore = useTheoryLessonStore();
const studentId = useField([
  Rules.required()
]);
const theoryLessonId = useField([
  Rules.required()
]);
const form = useForm([studentId, theoryLessonId]);

onMounted(async () => {
  await loadAllData();
})

const loadAllData = async () => {
  await Promise.all([
    courseBookingStore.fetchAll(),
    studentStore.fetchAll(),
    theoryLessonStore.fetchAll(),
  ]);
  await prepareTable();
  isLoading.value = false;
}

const prepareTable = async () => {
  const data = [];

  for(const courseBooking of courseBookingStore.data) {
    data.push({
      id: courseBooking.id,
      bookingDate: TimeHelper.convert(courseBooking.bookingDate, "date"),
      student: StudentHelper.getFullName(courseBooking.student),
      theoryLesson: TheoryLessonHelper.getName(courseBooking.theoryLesson),
    })
  }

  rows.value = data;
}

// Modals
const modalOpened = ref<ModalType>(ModalType.NONE);
const modalData = ref<ICourseBooking>();
const modalError = ref<string | null>(null);

const showModal = (data: ICourseBooking, type: ModalType) => {
  modalData.value = courseBookingStore.findById(data.id);
  setModalData(modalData.value);
  modalOpened.value = type;
}

const setModalData = (data?: ICourseBooking) => {
  studentId.value = data?.student ?? "";
  theoryLessonId.value = data?.theoryLesson ?? "";
}

const resetModal = () => {
  modalData.value = undefined;
  modalOpened.value = ModalType.NONE;
  modalError.value = null;
  form.reset();
}

// Api Calls
const deleteData = async () => {
  if(!modalData.value) return;

  const deleted = await ApiHelper.delete(modalData.value.id, courseBookingApiClient);
  if(!deleted) return;

  const index = courseBookingStore.findIndexById(modalData.value.id);
  if(index !== -1){
    courseBookingStore.data.splice(index, 1);
    await prepareTable();
  }

  resetModal();
}

const createData = async () => {
  if(!form.validate()) return;

  const data: ICourseBookingBase = {
    studentId: Number(studentId.value),
    theoryLessonId: Number(theoryLessonId.value),
  };

  const createdData = await ApiHelper.create(data, courseBookingApiClient);
  if(!createdData){
    modalError.value = "Eintrag konnte nicht gespeichert werden.";
    return;
  }

  courseBookingStore.data.push(createdData);
  await prepareTable();
  resetModal();
}

</script>

<template>
  <Modal :open="modalOpened === ModalType.CREATE" @abort="resetModal" :options="ModalHelper.DefaultOptions">
    <template #header>Erstellen</template>
    <template #content>
      <form @submit.prevent="createData">
        <CustomDropdown label="Fahrschüler:"
                        :required="true"
                        :searchOn="true"
                        v-model="studentId.value"
                        :error="studentId.error"
                        :options="studentStore.data.map(x => ({label: StudentHelper.getFullName(x), value: x.id.toString()}))"
        />
        <CustomDropdown label="Kurs:"
                        :required="true"
                        :searchOn="true"
                        v-model="theoryLessonId.value"
                        :error="theoryLessonId.error"
                        :options="theoryLessonStore.data.map(x => ({label: TheoryLessonHelper.getName(x), value: x.id.toString()}))"
        />
        <button type="submit" style="display:none" />
      </form>
    </template>
    <template #actions>
      <ButtonGroup>
        <CustomButton @click="resetModal" type="neutral">Abbrechen</CustomButton>
        <CustomButton :outline="false" type="success" @click="createData">Erstellen</CustomButton>
      </ButtonGroup>
    </template>
  </Modal>

  <Modal :open="modalOpened === ModalType.INFO" @abort="modalOpened = ModalType.NONE" :options="ModalHelper.InfoOptions">
    <template #header>Information</template>
    <template #content>coming soon.. {{ modalData?.id }}</template>
    <template #actions>
      <CustomButton @click="modalOpened = ModalType.NONE" :outline="false" type="neutral">Schließen</CustomButton>
    </template>
  </Modal>

  <Modal :open="modalOpened === ModalType.DELETE" @abort="modalOpened = ModalType.NONE" :options="ModalHelper.DefaultOptions">
    <template #header>Kursbuchung löschen</template>
    <template #content>
      Du bist dabei die Kursbuchung von
      <span class="modal_highlight">{{ StudentHelper.getFullName(modalData?.student) }}</span>
      zu löschen. Möchtest du fortfahren?
    </template>
    <template #actions>
      <ButtonGroup>
        <CustomButton @click="modalOpened = ModalType.NONE" type="neutral">Abbrechen</CustomButton>
        <CustomButton @click="deleteData" :outline="false" type="error">Löschen</CustomButton>
      </ButtonGroup>
    </template>
  </Modal>

  <CustomPaper>
    <PageHeader>
      <h3>Kursbuchungen</h3>
      <template #description>
        <p>Hier sind alle Kurse aufgelistet, die eine Buchung aufweisen.</p>
      </template>
      <template #actions>
        <CustomButton type="primary" :outline="true" :disabled="isLoading" @click="modalOpened = ModalType.CREATE">Buchung eintragen</CustomButton>
      </template>
    </PageHeader>
  </CustomPaper>

  <CustomPaper>
    <Vue3Datatable
      :loading="isLoading"
      noDataContent="Es wurden noch keine Buchungen vorgenommen."
      :rows="rows"
      :columns="cols"
      paginationInfo="Es werden die Einträge {0} bis {1} von {2} angezeigt."
    >
      <template #actions="data">
        <ButtonGroup>
          <CustomButton :min-width="0" type="neutral" :outline="true" @click="showModal(data.value, ModalType.INFO)">
            <i class="pi pi-eye"></i>
          </CustomButton>
          <CustomButton :min-width="0" type="error" :outline="true" @click="showModal(data.value, ModalType.DELETE)">
            <i class="pi pi-trash"></i>
          </CustomButton>
        </ButtonGroup>
      </template>
    </Vue3Datatable>
  </CustomPaper>

</template>

<style scoped>

</style>
