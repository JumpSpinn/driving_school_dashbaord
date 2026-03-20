<script setup lang="ts">
import {instructorApiClient} from "@/apis/InstructorApiClient.ts";
import type {IInstructor, IInstructorBase, IInstructorUpdate} from "@/interfaces/IInstructor.ts";
import Vue3Datatable from '@bhplugin/vue3-datatable'
import {InstructorHelper} from "@/helpers/InstructorHelper.ts";
import {ModalType} from "@/enums/ModalType.ts";
import {ModalHelper} from "@/helpers/ModalHelper.ts";
import {ApiHelper} from "@/helpers/ApiHelper.ts";
import {useInstructorStore} from "@/stores/instructorStore.ts";
import {useField} from "@/composables/useField.ts";
import {Rules} from "@/helpers/ValidationRules.ts";
import {useForm} from "@/composables/useForm.ts";
import ListWrapper from "@/components/list/ListWrapper.vue";
import ListItem from "@/components/list/ListItem.vue";

const cols = ref([
  { field: "id", title: "ID", width: "90px", filter: false },
  { field: "fullName", title: "Vor- Nachname" },
  { field: "mail", title: "E-Mail" },
  { field: "phone", title: "Telefon" },
  { field: "theoryLessons", title: "Zugewiesene Kurse" },
  { field: "actions", title: "Aktionen", slotMode: true, width: "160px" }
])
const rows = ref<any[]>([])
const isLoading = ref(true);
const instructorStore = useInstructorStore();
const firstName = useField([
  Rules.required(),
  Rules.minLength(3),
  Rules.maxLength(50),
]);
const lastName = useField([
  Rules.required(),
  Rules.minLength(3),
  Rules.maxLength(50),
]);
const mail = useField([
  Rules.required(),
  Rules.email(),
]);
const phone = useField();
const form = useForm([firstName, lastName, mail, phone]);

onMounted(async () => {
  await loadAllData();
})

const loadAllData = async () => {
  await Promise.all([
    instructorStore.fetchAll(),
  ]);
  await prepareTable();
  isLoading.value = false;
}

const prepareTable = async () => {
  const data = [];

  for(const instructor of instructorStore.data) {
    data.push({
      id: instructor.id,
      fullName: InstructorHelper.getFullName(instructor),
      mail: InstructorHelper.getMail(instructor),
      phone: InstructorHelper.getPhone(instructor),
      theoryLessons: instructor.theoryLessons?.length ?? 0,
    })
  }

  rows.value = data;
}

// Modals
const modalOpened = ref<ModalType>(ModalType.NONE);
const modalData = ref<IInstructor>();
const modalError = ref<string | null>(null);

const showModal = (data: IInstructor, type: ModalType) => {
  modalData.value = instructorStore.findById(data.id);
  setModalData(modalData.value);
  modalOpened.value = type;
}

const setModalData = (data?: IInstructor) => {
  firstName.value = data?.firstName ?? "";
  lastName.value = data?.lastName ?? "";
  mail.value = data?.mail ?? "";
  phone.value = data?.phone ?? "";
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

  const deleted = await ApiHelper.delete(modalData.value.id, instructorApiClient);
  if(!deleted) return;

  const index = instructorStore.findIndexById(modalData.value.id);
  if(index !== -1){
    instructorStore.data.splice(index, 1);
    await prepareTable();
  }

  resetModal();
}

const createData = async () => {
  if(!form.validate()) return;

  const data: IInstructorBase = {
    firstName: firstName.value,
    lastName: lastName.value,
    mail: mail.value,
    phone: phone.value,
  };

  const createdData = await ApiHelper.create(data, instructorApiClient);
  if(!createdData){
    modalError.value = "Eintrag konnte nicht gespeichert werden.";
    return;
  }

  instructorStore.data.push(createdData);
  await prepareTable();
  resetModal();
}

const updateData = async () => {
  if(!modalData.value) return;
  if(!form.validate()) return;

  const data: IInstructorUpdate = {
    id: modalData.value.id,
    firstName: firstName.value,
    lastName: lastName.value,
    mail: mail.value,
    phone: phone.value,
  };

  const updateData = await ApiHelper.update(data, instructorApiClient);
  if(!updateData){
    modalError.value = "Änderungen konnten nicht übernommen werden.";
    return;
  }

  const index = instructorStore.data.findIndex(x => x.id === modalData.value?.id);
  if(index !== -1){
    instructorStore.data[index] = updateData;
    await prepareTable();
    resetModal();
  }
}

</script>

<template>
  <Modal :open="modalOpened === ModalType.CREATE || modalOpened === ModalType.EDIT" @abort="resetModal" :options="ModalHelper.DefaultOptions" :error="modalError">
    <template #header>{{ modalOpened === ModalType.CREATE ? "Fahrlehrer eintragen" : "Fahrlehrer bearbeiten" }}</template>
    <template #content>
      <form @submit.prevent="modalOpened === ModalType.CREATE ? createData() : updateData()">
        <CustomTextInput label="Vorname:"
                         :required="true"
                         v-model="firstName.value"
                         :error="firstName.error"
        />
        <CustomTextInput label="Nachname:"
                         :required="true"
                         v-model="lastName.value"
                         :error="lastName.error"
        />
        <CustomTextInput label="E-Mail:"
                         :required="true"
                         v-model="mail.value"
                         :error="mail.error"
        />
        <CustomTextInput label="Telefon:"
                         v-model="phone.value"
                         :error="phone.error"
        />
        <button type="submit" style="display:none" />
      </form>
    </template>
    <template #actions>
      <ButtonGroup>
        <CustomButton @click="resetModal" type="neutral">Abbrechen</CustomButton>
        <CustomButton :outline="false" type="success" @click="modalOpened === ModalType.CREATE ? createData() : updateData()">
          {{ modalOpened === ModalType.CREATE ? "Eintragen" : "Änderungen übernehmen" }}
        </CustomButton>
      </ButtonGroup>
    </template>
  </Modal>

  <Modal :open="modalOpened === ModalType.INFO" @abort="modalOpened = ModalType.NONE" :options="ModalHelper.InfoOptions">
    <template #header>{{ modalData?.firstName }} {{ modalData?.lastName }} | Information</template>
    <CustomPaper>
      <p class="modal_info_highlight">Zugewiesene Kurse:</p>
      <ListWrapper v-if="modalData?.theoryLessons">
        <ListItem v-for="lesson in modalData?.theoryLessons">
          {{ lesson.name }}
          <template #description>
            {{ lesson.topic }}
          </template>
        </ListItem>
      </ListWrapper>
      <p v-else>Keine Kurse zugewiesen.</p>
    </CustomPaper>
    <template #actions>
      <CustomButton @click="modalOpened = ModalType.NONE" :outline="false" type="neutral">Schließen</CustomButton>
    </template>
  </Modal>

  <Modal :open="modalOpened === ModalType.DELETE" @abort="modalOpened = ModalType.NONE" :options="ModalHelper.DefaultOptions" :error="modalError">
    <template #header>Fahrlehrer löschen</template>
    <template #content>
      Du bist dabei den Fahrlehrer
      <span class="modal_highlight">{{ InstructorHelper.getFullName(modalData) }}</span>
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
      <h3>Fahrlehrer</h3>
      <template #description>
        <p>Hier sind alle Fahrlehrer aufgelistet, die unterrichten dürfen.</p>
      </template>
      <template #actions>
        <CustomButton type="primary" :outline="true" :disabled="isLoading" @click="modalOpened = ModalType.CREATE; setModalData()">Fahrlehrer eintragen</CustomButton>
      </template>
    </PageHeader>
  </CustomPaper>

  <CustomPaper>
    <Vue3Datatable
      :loading="isLoading"
      noDataContent="Es wurden noch keine Fahrlehrer eingetragen."
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
