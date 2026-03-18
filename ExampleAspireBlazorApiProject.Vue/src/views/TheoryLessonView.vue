<script setup lang="ts">
import type {ITheoryLesson} from "@/interfaces/ITheoryLesson.ts";
import {theoryLessonApiClient} from "@/apis/TheoryLessonApiClient.ts";
import Vue3Datatable from '@bhplugin/vue3-datatable'
import {InstructorHelper} from "@/helpers/InstructorHelper.ts";
import {ModalType} from "@/enums/ModalType.ts";
import {ModalHelper} from "@/helpers/ModalHelper.ts";
import {ApiHelper} from "@/helpers/ApiHelper.ts";
import {useTheoryLessonStore} from "@/stores/theoryLessonStore.ts";

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
const theoryLessonStore = useTheoryLessonStore();

onMounted(async () => {
  await loadAllData();
})

const loadAllData = async () => {
  await Promise.all([
    theoryLessonStore.fetchAll(),
  ]);
  await prepareTable();
  isLoading.value = false;
}

const prepareTable = async () => {
  const data = [];

  for(const theoryLesson of theoryLessonStore.data) {
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

const getDataFromTable = (data: ITheoryLesson) : ITheoryLesson | undefined => {
  return theoryLessonStore.data.find(cb => cb.id === data.id);
}

const showModal = (data: ITheoryLesson, type: ModalType) => {
  modalData.value = getDataFromTable(data);
  modalOpened.value = type;
}

const resetModal = () => {
  modalData.value = undefined;
  modalOpened.value = ModalType.NONE;
}

// Api Calls
const deleteData = async () => {
  if(!modalData.value) return;

  const deleted = await ApiHelper.delete(modalData.value.id, theoryLessonApiClient);
  if(!deleted) return;

  const index = theoryLessonStore.data.findIndex(x => x.id === modalData.value?.id);
  if(index !== -1){
    theoryLessonStore.data.splice(index, 1);
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
    <template #header>Kurs löschen</template>
    <template #content>
      Du bist dabei den Kurs
      <span class="modal_highlight">{{ modalData?.name }} ({{ modalData?.topic }})</span>
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
      <h3>Kurse</h3>
      <template #actions>
        <CustomButton type="primary" :outline="true" :disabled="isLoading" @click="modalOpened = ModalType.CREATE">Kurs eintragen</CustomButton>
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
