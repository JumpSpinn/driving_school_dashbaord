<script setup lang="ts">
import type {IStudent} from "@/interfaces/IStudent.ts";
import {studentApiClient} from "@/apis/StudentApiClient.ts";
import Vue3Datatable from '@bhplugin/vue3-datatable'
import {StudentHelper} from "@/helpers/StudentHelper.ts";
import {TimeHelper} from "@/helpers/TimeHelper.ts";
import {ModalType} from "@/enums/ModalType.ts";
import {ModalHelper} from "@/helpers/ModalHelper.ts";
import {ApiHelper} from "@/helpers/ApiHelper.ts";
import {useStudentStore} from "@/stores/studentStore.ts";
import {LicenseHelper} from "@/helpers/LicenseHelper.ts";

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
const studentStore = useStudentStore();

onMounted(async () => {
  await loadAllData();
})

const loadAllData = async () => {
  await Promise.all([
    studentStore.fetchAll(),
  ]);
  await prepareTable();
  isLoading.value = false;
}

const prepareTable = async () => {
  const data = [];

  for(const student of studentStore.data) {
    data.push({
      id: student.id,
      fullName: StudentHelper.getFullName(student),
      mail: StudentHelper.getMail(student),
      phone: StudentHelper.getPhone(student),
      license: LicenseHelper.getName(student.license),
      examDate: TimeHelper.convert(student.examDate, "date"),
      hasPassed: student.hasPassed ? "Ja" : "in Ausbildung",
      drivingSchool: StudentHelper.getDrivingSchoolName(student),
    })
  }

  rows.value = data;
}

// Modals
const modalOpened = ref<ModalType>(ModalType.NONE);
const modalData = ref<IStudent>();

const showModal = (data: IStudent, type: ModalType) => {
  modalData.value = studentStore.findById(data.id);
  modalOpened.value = type;
}

const resetModal = () => {
  modalData.value = undefined;
  modalOpened.value = ModalType.NONE;
}

// Api Calls
const deleteData = async () => {
  if(!modalData.value) return;

  const deleted = await ApiHelper.delete(modalData.value.id, studentApiClient);
  if(!deleted) return;

  const index = studentStore.findIndexById(modalData.value.id);
  if(index !== -1){
    studentStore.data.splice(index, 1);
    await prepareTable();
  }

  resetModal();
}

</script>

<template>
  <Modal :open="modalOpened === ModalType.CREATE || modalOpened === ModalType.EDIT" @abort="modalOpened = ModalType.NONE" :options="ModalHelper.DefaultOptions">
    <template #header>{{ modalOpened === ModalType.CREATE ? "Erstellen" : "Bearbeiten" }}</template>
    <template #content>coming soon..</template>
    <template #actions>
      <ButtonGroup>
        <CustomButton @click="modalOpened = ModalType.NONE" type="neutral">Abbrechen</CustomButton>
        <CustomButton :outline="false" type="success">
          {{ modalOpened === ModalType.CREATE ? "Erstellen" : "Änderungen übernehmen" }}
        </CustomButton>
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
    <template #header>Fahrschüler löschen</template>
    <template #content>
      Du bist dabei den Fahrschüler
      <span class="modal_highlight">{{ StudentHelper.getFullName(modalData) }}</span>
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
      <h3>Fahrschüler</h3>
      <template #description>
        <p>Hier sind alle Fahrschüler aufgelistet, die im System eingetragen wurden.</p>
      </template>
      <template #actions>
        <CustomButton type="primary" :outline="true" :disabled="isLoading" @click="modalOpened = ModalType.CREATE">Fahrschüler eintragen</CustomButton>
      </template>
    </PageHeader>
  </CustomPaper>

  <CustomPaper>
    <Vue3Datatable
      :loading="isLoading"
      noDataContent="Es wurden noch keine Fahrschüler eingetragen."
      :rows="rows"
      :columns="cols"
      paginationInfo="Es werden die Einträge {0} bis {1} von {2} angezeigt."
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
