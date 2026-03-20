<script setup lang="ts">
import type {IStudent, IStudentBase, IStudentUpdate} from "@/interfaces/IStudent.ts";
import {studentApiClient} from "@/apis/StudentApiClient.ts";
import Vue3Datatable from '@bhplugin/vue3-datatable'
import {StudentHelper} from "@/helpers/StudentHelper.ts";
import {TimeHelper} from "@/helpers/TimeHelper.ts";
import {ModalType} from "@/enums/ModalType.ts";
import {ModalHelper} from "@/helpers/ModalHelper.ts";
import {ApiHelper} from "@/helpers/ApiHelper.ts";
import {useStudentStore} from "@/stores/studentStore.ts";
import {LicenseHelper} from "@/helpers/LicenseHelper.ts";
import {useField} from "@/composables/useField.ts";
import {Rules} from "@/helpers/ValidationRules.ts";
import {useForm} from "@/composables/useForm.ts";
import {useDrivingSchoolStore} from "@/stores/drivingSchoolStore.ts";
import {LicenseClass} from "@/enums/LicenseClass.ts";
import {BaseUtil} from "@/utils/BaseUtil.ts";

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
const drivingSchoolStore = useDrivingSchoolStore();
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
])
const phone = useField();
const birthday = useField();
const license = useField();
const examDate = useField();
const hasPassed = useField();
const drivingSchoolId = useField();
const form = useForm([
  firstName, lastName, mail, phone, birthday, license,
  examDate, hasPassed, drivingSchoolId
]);

onMounted(async () => {
  await loadAllData();
})

const loadAllData = async () => {
  await Promise.all([
    studentStore.fetchAll(),
    drivingSchoolStore.fetchAll(),
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
const modalError = ref<string | null>(null);

const showModal = (data: IStudent, type: ModalType) => {
  modalData.value = studentStore.findById(data.id);
  setModalData(modalData.value);
  modalOpened.value = type;
}

const setModalData = (data?: IStudent) => {
  firstName.value = data?.firstName ?? '';
  lastName.value = data?.lastName ?? '';
  mail.value = data?.mail ?? '';
  phone.value = data?.phone ?? '';
  license.value = data?.license ?? '';
  drivingSchoolId.value = data?.drivingSchoolId ?? '';
  hasPassed.value = data?.hasPassed ?? '';
  examDate.value = (data?.examDate as string)?.split("T")[0] ?? '';
  birthday.value = (data?.birthday as string)?.split("T")[0] ?? '';
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

  const deleted = await ApiHelper.delete(modalData.value.id, studentApiClient);
  if(!deleted) return;

  const index = studentStore.findIndexById(modalData.value.id);
  if(index !== -1){
    studentStore.data.splice(index, 1);
    await prepareTable();
  }

  resetModal();
}

const createData = async () => {
  if(!form.validate()) return;

  const data: IStudentBase = {
    firstName: firstName.value,
    lastName: lastName.value,
    mail: mail.value,
    phone: phone.value,
    birthday: (birthday.value as string)?.trim().length > 0 ? (birthday.value as string) : null,
    license: Number(license.value),
    examDate: (examDate.value as string)?.trim().length > 0 ? (examDate.value as string) : null,
    drivingSchoolId: drivingSchoolId.value,
    hasPassed: false // default false
  };

  const createdData = await ApiHelper.create(data, studentApiClient);
  if(!createdData){
    modalError.value = "Eintrag konnte nicht gespeichert werden.";
    return;
  }

  studentStore.data.push(createdData);
  await prepareTable();
  resetModal();
}

const updateData = async () => {
  if(!modalData.value) return;
  if(!form.validate()) return;

  const data: IStudentUpdate = {
    id: modalData.value.id,
    firstName: firstName.value,
    lastName: lastName.value,
    mail: mail.value,
    phone: phone.value,
    birthday: (birthday.value as string)?.trim().length > 0 ? (birthday.value as string) : null,
    license: Number(license.value),
    examDate: (examDate.value as string)?.trim().length > 0 ? (examDate.value as string) : null,
    drivingSchoolId: drivingSchoolId.value,
    hasPassed: Boolean(hasPassed.value),
  };

  const updateData = await ApiHelper.update(data, studentApiClient);
  if(!updateData){
    modalError.value = "Änderungen konnten nicht übernommen werden.";
    return;
  }

  const index = studentStore.data.findIndex(x => x.id === modalData.value?.id);
  if(index !== -1){
    studentStore.data[index] = updateData;
    await prepareTable();
    resetModal();
  }
}

</script>

<template>
  <Modal :open="modalOpened === ModalType.CREATE || modalOpened === ModalType.EDIT" @abort="resetModal" :options="ModalHelper.DefaultOptions" :error="modalError">
    <template #header>{{ modalOpened === ModalType.CREATE ? "Fahrschüler eintragen" : "Fahrschüler bearbeiten" }}</template>
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
        <CustomDatePicker label="Geburtstag:"
                          v-model="birthday.value"
                          :error="birthday.error"
        />
        <CustomDropdown label="Fahrschule zuweisen"
                        :search-on="true"
                        v-model="drivingSchoolId.value"
                        :error="drivingSchoolId.error"
                        :options="drivingSchoolStore.data.map(x => ({label: x.name, value: x.id}))"
        />
        <CustomDropdown label="Fahrzeug-Klasse wählen"
                        :search-on="true"
                        v-model="license.value"
                        :error="license.error"
                        :options="BaseUtil.enumToArray(LicenseClass).map(d => ({label: `${LicenseHelper.getName(d)} (${LicenseHelper.getDescription(d)})`, value: d}))"
        />
        <CustomDatePicker label="Prüfungstermin:"
                          v-model="examDate.value"
                          :error="examDate.error"
        />
        <CustomToggle label="Bestanden?"
                      v-if="modalOpened == ModalType.EDIT"
                      v-model="hasPassed.value"
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

  <Modal :open="modalOpened === ModalType.INFO" @abort="resetModal" :options="ModalHelper.InfoOptions">
    <template #header>Fahrschüler | Information</template>
    <template #content>coming soon.. {{ modalData?.id }}</template>
    <template #actions>
      <CustomButton @click="resetModal" :outline="false" type="neutral">Schließen</CustomButton>
    </template>
  </Modal>

  <Modal :open="modalOpened === ModalType.DELETE" @abort="resetModal" :options="ModalHelper.DefaultOptions" :error="modalError">
    <template #header>Fahrschüler löschen</template>
    <template #content>
      Du bist dabei den Fahrschüler
      <span class="modal_highlight">{{ StudentHelper.getFullName(modalData) }}</span>
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
