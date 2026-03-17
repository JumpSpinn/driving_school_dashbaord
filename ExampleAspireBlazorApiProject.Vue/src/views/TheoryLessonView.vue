<script setup lang="ts">
import {onMounted, ref} from "vue";
import type {ITheoryLesson} from "@/interfaces/ITheoryLesson.ts";
import {theoryLessonApiClient} from "@/apis/TheoryLessonApiClient.ts";
import CustomButton from "@/components/button/CustomButton.vue";
import CustomPaper from "@/components/paper/CustomPaper.vue";
import PageHeader from "@/components/header/PageHeader.vue";
import Vue3Datatable from '@bhplugin/vue3-datatable'
import ButtonGroup from "@/components/button/ButtonGroup.vue";
import {InstructorHelper} from "@/helpers/InstructorHelper.ts";
import {ModalType} from "@/enums/ModalType.ts";
import Modal from "@/components/modal/Modal.vue";
import {ModalHelper} from "@/helpers/ModalHelper.ts";

const cols = ref([
  { field: "id", title: "ID", width: "90px", filter: false },
  { field: "name", title: "Kursname" },
  { field: "topic", title: "Thema" },
  { field: "startTime", title: "Startzeit" },
  { field: "instructor", title: "Fahrlehrer" },
  { field: "actions", title: "Aktionen", slotMode: true, width: "160px" }
])
const rows = ref<any[]>([])
const isLoading = ref(true);
const theoryLessons = ref<ITheoryLesson[]>([]);

onMounted(async () => {
  await loadData();
})

const loadData = async () => {
  const resp = await theoryLessonApiClient.getAll();
  theoryLessons.value = resp ?? [];
  await prepareTable();
  isLoading.value = false;
}

const prepareTable = async () => {
  const data = [];

  for(const theoryLesson of theoryLessons.value) {
    data.push({
      id: theoryLesson.id,
      name: theoryLesson.name,
      topic: theoryLesson.topic,
      startTime: theoryLesson.startTime,
      instructor: InstructorHelper.getFullName(theoryLesson.instructor),
    })
  }

  rows.value = data;
}

// Modals
const modalOpened = ref<ModalType>(ModalType.NONE);
const modalData = ref<ITheoryLesson>();

const showModal = (data: ITheoryLesson, type: ModalType) => {
  modalData.value = data;
  modalOpened.value = type;
}

</script>

<template>
  <Modal :open="modalOpened === ModalType.INFO" @abort="modalOpened = ModalType.NONE" :options="ModalHelper.InfoOptions">
    <template #header>Information</template>
    <template #content>coming soon.. {{ modalData?.id }}</template>
    <template #actions>
      <CustomButton @click="modalOpened = ModalType.NONE">Schließen</CustomButton>
    </template>
  </Modal>

  <Modal :open="modalOpened === ModalType.EDIT" @abort="modalOpened = ModalType.NONE" :options="ModalHelper.DefaultOptions">
    <template #header>Bearbeiten</template>
    <template #content>coming soon.. {{ modalData?.id }}</template>
    <template #actions>
      <CustomButton>Änderungen übernehmen</CustomButton>
      <CustomButton @click="modalOpened = ModalType.NONE">Nein</CustomButton>
    </template>
  </Modal>

  <Modal :open="modalOpened === ModalType.DELETE" @abort="modalOpened = ModalType.NONE" :options="ModalHelper.DefaultOptions">
    <template #header>Löschen</template>
    <template #content>Möchtest du for real löschen alla? {{ modalData?.id }}</template>
    <template #actions>
      <CustomButton>Ja</CustomButton>
      <CustomButton @click="modalOpened = ModalType.NONE">Nein</CustomButton>
    </template>
  </Modal>

  <CustomPaper>
    <PageHeader>
      <h3>Kurse</h3>
      <template #description>
        <p>Lorem ipsum dolor sit amet</p>
      </template>
      <template #actions>
        <CustomButton type="secondary" :outline="true" :disabled="isLoading">Kurs eintragen</CustomButton>
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
          <CustomButton type="neutral" @click="showModal(data.value, ModalType.INFO)">
            <i class="pi pi-eye"></i>
          </CustomButton>
          <CustomButton type="neutral" @click="showModal(data.value, ModalType.EDIT)">
            <i class="pi pi-pencil"></i>
          </CustomButton>
          <CustomButton type="neutral" @click="showModal(data.value, ModalType.DELETE)">
            <i class="pi pi-trash"></i>
          </CustomButton>
        </ButtonGroup>
      </template>
    </Vue3Datatable>
  </CustomPaper>

</template>

<style scoped>

</style>
