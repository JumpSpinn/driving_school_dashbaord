<script setup lang="ts">
import type {ICourseBooking} from "@/interfaces/ICourseBooking.ts";
import {courseBookingApiClient} from "@/apis/CourseBookingApiClient.ts";
import Vue3Datatable from '@bhplugin/vue3-datatable'
import {StudentHelper} from "@/helpers/StudentHelper.ts";
import {TheoryLessonHelper} from "@/helpers/TheoryLessonHelper.ts";
import {TimeHelper} from "@/helpers/TimeHelper.ts";
import {ModalHelper} from "@/helpers/ModalHelper.ts";
import {ApiHelper} from "@/helpers/ApiHelper.ts";
import {ModalType} from "@/enums/ModalType.ts";
import {useCourseBookingStore} from "@/stores/courseBookingStore.ts";

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

onMounted(async () => {
  await loadAllData();
})

const loadAllData = async () => {
  await Promise.all([
    courseBookingStore.fetchAll(),
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

const showModal = (data: ICourseBooking, type: ModalType) => {
  modalData.value = courseBookingStore.findById(data.id);
  modalOpened.value = type;
}

const resetModal = () => {
  modalData.value = undefined;
  modalOpened.value = ModalType.NONE;
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

</script>

<template>
  <Modal :open="modalOpened === ModalType.CREATE" @abort="modalOpened = ModalType.NONE" :options="ModalHelper.DefaultOptions">
    <template #header>Erstellen</template>
    <template #content>coming soon..</template>
    <template #actions>
      <ButtonGroup>
        <CustomButton @click="modalOpened = ModalType.NONE" type="neutral">Abbrechen</CustomButton>
        <CustomButton :outline="false" type="success">Erstellen</CustomButton>
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

  <Modal :open="modalOpened === ModalType.EDIT" @abort="modalOpened = ModalType.NONE" :options="ModalHelper.DefaultOptions">
    <template #header>Bearbeiten</template>
    <template #content>coming soon.. {{ modalData?.id }}</template>
    <template #actions>
      <ButtonGroup>
        <CustomButton @click="modalOpened = ModalType.NONE" type="neutral">Abbrechen</CustomButton>
        <CustomButton :outline="false" type="success">Änderungen übernehmen</CustomButton>
      </ButtonGroup>
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
      noDataContent="Keine Daten vorhanden"
      :rows="rows"
      :columns="cols"
    >
      <template #actions="data">
        <ButtonGroup>
          <CustomButton :min-width="0" type="neutral" :outline="true" @click="showModal(data.value, ModalType.INFO)">
            <i class="pi pi-eye"></i>
          </CustomButton>
          <CustomButton :min-width="0" type="primary" :outline="true" @click="showModal(data.value, ModalType.EDIT)">
            <i class="pi pi-pencil"></i>
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
