<script setup lang="ts">
import type {
  IDrivingSchool,
  IDrivingSchoolBase,
  IDrivingSchoolUpdate
} from "@/interfaces/IDrivingSchool.ts";
import {drivingSchoolApiClient} from "@/apis/DrivingSchoolApiClient.ts";
import Vue3Datatable from '@bhplugin/vue3-datatable'
import {InstructorHelper} from "@/helpers/InstructorHelper.ts";
import {ModalType} from "@/enums/ModalType.ts";
import {ModalHelper} from "@/helpers/ModalHelper.ts";
import {ApiHelper} from "@/helpers/ApiHelper.ts";
import {useDrivingSchoolStore} from "@/stores/drivingSchoolStore.ts";
import {useField} from "@/composables/useField.ts";
import {Rules} from "@/helpers/ValidationRules.ts";
import {useInstructorStore} from "@/stores/instructorStore.ts";
import {useForm} from "@/composables/useForm.ts";

const cols = ref([
  { field: "id", title: "ID", width: "90px", filter: false },
  { field: "name", title: "Bezeichnung" },
  { field: "owner", title: "Besitzer" },
  { field: "students", title: "Fahrschüler" },
  { field: "actions", title: "Aktionen", slotMode: true, width: "160px" }
])
const rows = ref<any[]>([])
const isLoading = ref(true);
const drivingSchoolStore = useDrivingSchoolStore();
const instructorStore = useInstructorStore();
const name = useField([
  Rules.required(),
  Rules.minLength(3),
  Rules.maxLength(50),
])
const ownerId = useField();
const form = useForm([name, ownerId]);

onMounted(async () => {
  await loadAllData();
})

const loadAllData = async () => {
  await Promise.all([
    drivingSchoolStore.fetchAll(),
    instructorStore.fetchAll(),
  ]);
  await prepareTable();
  isLoading.value = false;
}

const prepareTable = async () => {
  const data = [];

  for(const drivingSchool of drivingSchoolStore.data) {
    data.push({
      id: drivingSchool.id,
      name: drivingSchool.name,
      owner: InstructorHelper.getFullName(drivingSchool.owner),
      students: drivingSchool.students?.length ?? 0,
    })
  }

  rows.value = data;
}

// Modals
const modalOpened = ref<ModalType>(ModalType.NONE);
const modalData = ref<IDrivingSchool>();

const getDataFromTable = (data: IDrivingSchool) : IDrivingSchool | undefined => {
  return drivingSchoolStore.data.find(cb => cb.id === data.id);
}

const showModal = (data: IDrivingSchool, type: ModalType) => {
  modalData.value = getDataFromTable(data);
  if(modalData.value) setModalData(modalData.value);
  modalOpened.value = type;
}

const setModalData = (data: IDrivingSchool) => {
  name.value = data.name;
  ownerId.value = data.ownerId ?? '';
}

const resetModal = () => {
  modalData.value = undefined;
  modalOpened.value = ModalType.NONE;
  form.reset();
}

// Api Calls
const deleteData = async () => {
  if(!modalData.value) return;

  const deleted = await ApiHelper.delete(modalData.value.id, drivingSchoolApiClient);
  if(!deleted) return;

  const index = drivingSchoolStore.data.findIndex(x => x.id === modalData.value?.id);
  if(index !== -1){
    drivingSchoolStore.data.splice(index, 1);
    await prepareTable();
  }

  resetModal();
}

const createData = async () => {
  if(!form.validate()) return;

  const data: IDrivingSchoolBase = {
    name: name.value,
    ownerId: Number(ownerId.value),
  };

  const createdData = await ApiHelper.create(data, drivingSchoolApiClient);
  if(!createdData) return;

  drivingSchoolStore.data.push(createdData);
  await prepareTable();
  resetModal();
}

const updateData = async () => {
  if(!modalData.value) return;
  if(!form.validate()) return;

  const data: IDrivingSchoolUpdate = {
    id: modalData.value.id,
    name: name.value,
    ownerId: Number(ownerId.value),
  };

  const updateData = await ApiHelper.update(data, drivingSchoolApiClient);
  if(!updateData) return;

  const index = drivingSchoolStore.data.findIndex(x => x.id === modalData.value?.id);
  if(index !== -1){
    drivingSchoolStore.data[index] = updateData;
    await prepareTable();
    resetModal();
  }
}

</script>

<template>
  <Modal :open="modalOpened === ModalType.CREATE" @abort="modalOpened = ModalType.NONE" :options="ModalHelper.DefaultOptions">
    <template #header>Fahrschule erstellen</template>
    <template #content>
      <form @submit.prevent="createData">
        <CustomTextInput label="Bezeichnung:"
                         :required="true"
                         v-model="name.value"
                         :error="name.error"
        />
        <CustomDropdown label="Besitzer festlegen:"
                        v-model="ownerId.value"
                        :error="ownerId.error"
                        :options="instructorStore.data.map(x => ({label: InstructorHelper.getFullName(x), value: x.id}))"
        />
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
      <CustomButton @click="resetModal" :outline="false" type="neutral">Schließen</CustomButton>
    </template>
  </Modal>

  <Modal :open="modalOpened === ModalType.EDIT" @abort="modalOpened = ModalType.NONE" :options="ModalHelper.DefaultOptions">
    <template #header>Fahrschule <span class="modal_highlight">{{ modalData?.name }}</span> bearbeiten</template>
    <template #content>
      <form @submit.prevent="updateData">
        <CustomTextInput label="Bezeichnung:"
                         :required="true"
                         v-model="name.value"
                         :error="name.error"
        />
        <CustomDropdown label="Besitzer festlegen:"
                        v-model="ownerId.value"
                        :error="ownerId.error"
                        :options="instructorStore.data.map(x => ({label: InstructorHelper.getFullName(x), value: x.id}))"
        />
      </form>
    </template>
    <template #actions>
      <ButtonGroup>
        <CustomButton @click="resetModal" type="neutral">Abbrechen</CustomButton>
        <CustomButton :outline="false" type="success" @click="updateData">Änderungen übernehmen</CustomButton>
      </ButtonGroup>
    </template>
  </Modal>

  <Modal :open="modalOpened === ModalType.DELETE" @abort="modalOpened = ModalType.NONE" :options="ModalHelper.DefaultOptions">
    <template #header>Fahrschule löschen</template>
    <template #content>
      Du bist dabei die Fahrschule
      <span class="modal_highlight">{{ modalData?.name }}</span>
      zu löschen. Möchtest du fortfahren?
    </template>
    <template #actions>
      <ButtonGroup>
        <CustomButton @click="resetModal" type="neutral">Abbrechen</CustomButton>
        <CustomButton @click="deleteData" :outline="false" type="error">Löschen</CustomButton>
      </ButtonGroup>
    </template>
  </Modal>

  <CustomPaper>
    <PageHeader>
      <h3>Fahrschulen</h3>
      <template #description>
        <p>Hier sind alle Fahrschulen aufgelistet, die im System registriert wurden.</p>
      </template>
      <template #actions>
        <CustomButton type="primary" :outline="true" :disabled="isLoading" @click="modalOpened = ModalType.CREATE">Fahrschule eintragen</CustomButton>
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
