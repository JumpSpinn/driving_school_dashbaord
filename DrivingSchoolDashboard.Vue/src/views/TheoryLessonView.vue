<script setup lang="ts">
import type {
  ITheoryLesson,
  ITheoryLessonBase,
  ITheoryLessonUpdate
} from "@/interfaces/ITheoryLesson.ts";
import {theoryLessonApiClient} from "@/apis/TheoryLessonApiClient.ts";
import Vue3Datatable from '@bhplugin/vue3-datatable'
import {InstructorHelper} from "@/helpers/InstructorHelper.ts";
import {ModalType} from "@/enums/ModalType.ts";
import {ModalHelper} from "@/helpers/ModalHelper.ts";
import {ApiHelper} from "@/helpers/ApiHelper.ts";
import {useTheoryLessonStore} from "@/stores/theoryLessonStore.ts";
import {useInstructorStore} from "@/stores/instructorStore.ts";
import {useField} from "@/composables/useField.ts";
import {BaseUtil} from "@/utils/BaseUtil.ts";
import {useForm} from "@/composables/useForm.ts";
import {Rules} from "@/helpers/ValidationRules.ts";
import {DayOfWeek} from "@/enums/DayOfWeek.ts";
import {TimeHelper} from "@/helpers/TimeHelper.ts";

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
const instructorStore = useInstructorStore();
const name = useField([
  Rules.required(),
  Rules.minLength(3),
  Rules.maxLength(50),
]);
const topic = useField([
  Rules.required(),
  Rules.minLength(3),
  Rules.maxLength(50),
]);
const dayOfWeek = useField();
const startTime = useField([
  Rules.required(),
  Rules.timeHHmmss()
]);
const durationMinutes = useField([
  Rules.required(),
  Rules.minValue(45)
]);
const maxStudents = useField([
  Rules.required(),
  Rules.minValue(1)
]);
const price = useField([
  Rules.required(),
  Rules.minValue(1)
]);
const instructorId = useField();
const isBasic = useField();
const form = useForm([name, topic, dayOfWeek, startTime, durationMinutes, maxStudents, price, instructorId, isBasic]);

onMounted(async () => {
  await loadAllData();
})

const loadAllData = async () => {
  await Promise.all([
    theoryLessonStore.fetchAll(),
    instructorStore.fetchAll(),
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
const modalError = ref<string | null>(null);

const showModal = (data: ITheoryLesson, type: ModalType) => {
  modalData.value = theoryLessonStore.findById(data.id);
  setModalData(modalData.value);
  modalOpened.value = type;
}

const setModalData = (data?: ITheoryLesson) => {
  name.value = data?.name ?? '';
  topic.value = data?.topic ?? '';
  dayOfWeek.value = data?.dayOfWeek ?? 0;
  startTime.value = data?.startTime ?? '';
  durationMinutes.value = data?.durationMinutes ?? 45;
  maxStudents.value = data?.maxStudents ?? 1;
  price.value = data?.price ?? 1;
  instructorId.value = data?.instructorId ?? '';
  isBasic.value = data?.isBasic ?? true;
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

  const deleted = await ApiHelper.delete(modalData.value.id, theoryLessonApiClient);
  if(!deleted) return;

  const index = theoryLessonStore.findIndexById(modalData.value.id);
  if(index !== -1){
    theoryLessonStore.data.splice(index, 1);
    await prepareTable();
  }

  resetModal();
}

const createData = async () => {
  if(!form.validate()) return;

  const data: ITheoryLessonBase = {
    name: name.value,
    topic: topic.value,
    dayOfWeek: Number(dayOfWeek.value),
    startTime: startTime.value,
    durationMinutes: Number(durationMinutes.value),
    maxStudents: Number(maxStudents.value),
    price: Number(price.value),
    isBasic: Boolean(isBasic.value),
    instructorId: Number(instructorId.value),
  };

  const createdData = await ApiHelper.create(data, theoryLessonApiClient);
  if(!createdData){
    modalError.value = "Eintrag konnte nicht gespeichert werden.";
    return;
  }

  theoryLessonStore.data.push(createdData);
  await prepareTable();
  resetModal();
}

const updateData = async () => {
  if(!modalData.value) return;
  if(!form.validate()) return;

  const data: ITheoryLessonUpdate = {
    id: modalData.value.id,
    name: name.value,
    topic: topic.value,
    dayOfWeek: Number(dayOfWeek.value),
    startTime: startTime.value,
    durationMinutes: Number(durationMinutes.value),
    maxStudents: Number(maxStudents.value),
    price: Number(price.value),
    isBasic: Boolean(isBasic.value),
    instructorId: Number(instructorId.value),
  };

  const updateData = await ApiHelper.update(data, theoryLessonApiClient);
  if(!updateData){
    modalError.value = "Änderungen konnten nicht übernommen werden.";
    return;
  }

  const index = theoryLessonStore.data.findIndex(x => x.id === modalData.value?.id);
  if(index !== -1){
    theoryLessonStore.data[index] = updateData;
    await prepareTable();
    resetModal();
  }
}

</script>

<template>
  <Modal :open="modalOpened === ModalType.CREATE || modalOpened === ModalType.EDIT" @abort="resetModal" :options="ModalHelper.DefaultOptions">
    <template #header>{{ modalOpened === ModalType.CREATE ? "Kurs eintragen" : "Kurs bearbeiten" }}</template>
    <template #content>
      <form @submit.prevent="modalOpened === ModalType.CREATE ? createData() : updateData()">
        <CustomTextInput label="Kursname:"
                         :required="true"
                         v-model="name.value"
                         :error="name.error"
        />
        <CustomTextInput label="Thema:"
                         :required="true"
                         v-model="topic.value"
                         :error="topic.error"
        />
        <CustomDropdown label="Wochentag:"
                        :options="BaseUtil.enumToArray(DayOfWeek).map(d => ({label: TimeHelper.getDayOfWeekName(d), value: d}))"
                        v-model="dayOfWeek.value"
                        :error="dayOfWeek.error"
        />
        <CustomTextInput label="Startzeit:"
                         v-model="startTime.value"
                         :error="startTime.error"
                         placeholder="HH:mm:ss"
        />
        <CustomTextInput label="Dauer (Minuten):"
                         :required="true"
                         v-model="durationMinutes.value"
                         :error="durationMinutes.error"
        />
        <CustomTextInput label="Max. Teilnehmer:"
                         :required="true"
                         v-model="maxStudents.value"
                         :error="maxStudents.error"
        />
        <CustomTextInput label="Preis (€):"
                         :required="true"
                         v-model="price.value"
                         :error="price.error"
        />
        <CustomDropdown label="Fahrlehrer zuweisen:"
                        v-model="instructorId.value"
                        :error="instructorId.error"
                        :options="instructorStore.data.map(i => ({label: InstructorHelper.getFullName(i), value: i.id}))"
        />
        <CustomToggle label="Basisstoff (Grundwissen)?"
                      v-model="isBasic.value"
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
    <template #header>Kurs | Information</template>
    <template #content>coming soon.. {{ modalData?.id }}</template>
    <template #actions>
      <CustomButton @click="modalOpened = ModalType.NONE" :outline="false" type="neutral">Schließen</CustomButton>
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
      noDataContent="Es wurden noch keine Kurse eingetragen."
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
