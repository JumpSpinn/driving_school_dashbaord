<script setup lang="ts">
import {useDrivingSchoolStore} from "@/stores/drivingSchoolStore.ts";
import {useStudentStore} from "@/stores/studentStore.ts";
import {useInstructorStore} from "@/stores/instructorStore.ts";
import {useTheoryLessonStore} from "@/stores/theoryLessonStore.ts";
import {useCourseBookingStore} from "@/stores/courseBookingStore.ts";
import { Pie, Line } from 'vue-chartjs';
import {Chart as ChartJS, CategoryScale, LinearScale, PointElement, ArcElement, LineElement, Title, Tooltip, Legend} from 'chart.js';
import Vue3Datatable from '@bhplugin/vue3-datatable'
import {TimeHelper} from "@/helpers/TimeHelper.ts";
import {StudentHelper} from "@/helpers/StudentHelper.ts";
import {TheoryLessonHelper} from "@/helpers/TheoryLessonHelper.ts";
import { Building2, LucideGraduationCap, LibraryBig, BookCheck, UserStar } from "lucide-vue-next";

const isLoading = ref(true);
const drivingSchoolStore = useDrivingSchoolStore();
const studentStore = useStudentStore();
const instructorStore = useInstructorStore();
const theoryLessonStore = useTheoryLessonStore();
const courseBookingStore = useCourseBookingStore();
ChartJS.register(ArcElement, PointElement, CategoryScale, LinearScale, LineElement, Title, Tooltip, Legend);

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
  prepareCharts();
  prepareTable();
  isLoading.value = false;
}

// Charts
const chartDefaultOptions = {
  responsive: true,
  maintainAspectRatio: true
}

const donutPassedData = ref({
  labels: ["Bestanden", "in Ausbildung"],
  datasets: [
    {
      backgroundColor: ['rgb(21, 213, 85)', 'rgb(235, 125, 52)'],
      data: [0,0]
    }
  ]
})

const donutDrivingSchoolData = ref({
  labels: ["Zugewiesen", "ohne Zuweisung"],
  datasets: [
    {
      backgroundColor: ['rgb(21, 213, 85)', 'rgb(235, 125, 52)'],
      data: [0,0]
    }
  ]
})

const lineData = {
  labels: ["Mo", "Di", "Mi", "Do", "Fr", "Sa", "So"],
  datasets: [
    {
      label: 'Buchungen',
      backgroundColor: 'rgba(253, 73, 168, 1)',
      borderColor: 'rgba(253, 73, 168, .3)',
      data: [0, 10, 5, 2, 20, 30, 45],
    }
  ]
}

const prepareCharts = () => {
  // Data (Passed, Non-Passed)
  donutPassedData.value.datasets[0]!.data[0] = studentStore.data.filter(s => s.hasPassed).length;
  donutPassedData.value.datasets[0]!.data[1] = studentStore.data.filter(s => !s.hasPassed).length;

  // Data (Driving School, Non-Driving School)
  donutDrivingSchoolData.value.datasets[0]!.data[0] = studentStore.data.filter(s => s.drivingSchool).length;
  donutDrivingSchoolData.value.datasets[0]!.data[1] = studentStore.data.filter(s => !s.drivingSchool).length;

  // Data (Booking per Day)
  const dayMap: Record<number, number> = {
    1: 0,
    2: 1,
    3: 2,
    4: 3,
    5: 4,
    6: 5,
    0: 6,
  }

  lineData.datasets[0]!.data = [0, 0, 0, 0, 0, 0, 0];

  for(const booking of courseBookingStore.data){
    if(!booking.bookingDate) return;

    const dayIdx = dayMap[new Date(booking.bookingDate).getDay()];
    if(dayIdx !== undefined)
      lineData.datasets[0]!.data[dayIdx]!++;
  }
}

// Table
const cols = ref([
  { field: "bookingDate", title: "Buchungsdatum" },
  { field: "student", title: "Fahrschüler" },
  { field: "theoryLesson", title: "Kurs" }
]);
const rows = ref<any[]>([]);

const prepareTable = () => {
  const data = [];
  const bookings = courseBookingStore.data;
  bookings.sort((a,b) => a.bookingDate! > b.bookingDate! ? -1 : 1);
  bookings.splice(5, bookings.length - 5);

  for(const booking of bookings) {
    data.push({
      bookingDate: TimeHelper.convert(booking.bookingDate, "date"),
      student: StudentHelper.getFullName(booking.student),
      theoryLesson: TheoryLessonHelper.getName(booking.theoryLesson),
    })
  }

  rows.value = data;
}

</script>

<template>
  <div class="grid">
    <CustomPaper class="paper">
      <PageHeader>
        <div class="header">
          <h3>Fahrschulen</h3>
          <Building2 :size="50" :stroke-width="1" class="icon" />
        </div>
        <h1>{{ drivingSchoolStore.data.length }}</h1>
      </PageHeader>
    </CustomPaper>

    <CustomPaper class="paper">
      <PageHeader>
        <div class="header">
          <h3>Fahrlehrer</h3>
          <UserStar :size="50" :stroke-width="1" class="icon" />
        </div>
        <h1>{{ instructorStore.data.length }}</h1>
      </PageHeader>
    </CustomPaper>

    <CustomPaper class="paper">
      <PageHeader>
        <div class="header">
          <h3>Fahrschüler</h3>
          <LucideGraduationCap :size="50" :stroke-width="1" class="icon" />
        </div>
        <h1>{{ studentStore.data.length }}</h1>
      </PageHeader>
    </CustomPaper>

    <CustomPaper class="paper">
      <PageHeader>
        <div class="header">
          <h3>Kurse</h3>
          <LibraryBig :size="50" :stroke-width="1" class="icon" />
        </div>
        <h1>{{ theoryLessonStore.data.length }}</h1>
      </PageHeader>
    </CustomPaper>

    <CustomPaper class="paper">
      <PageHeader>
        <div class="header">
          <h3>Kurs-Buchungen</h3>
          <BookCheck :size="50" :stroke-width="1" class="icon" />
        </div>
        <h1>{{ courseBookingStore.data.length }}</h1>
      </PageHeader>
    </CustomPaper>
  </div>

  <div class="wrapper">
    <CustomPaper class="paper chart">
      <Line :data="lineData" :options="chartDefaultOptions" v-if="!isLoading" />
    </CustomPaper>
    <CustomPaper class="paper chart">
      <Pie :data="donutDrivingSchoolData" :options="chartDefaultOptions" v-if="!isLoading" />
    </CustomPaper>
    <CustomPaper class="paper chart">
      <Pie :data="donutPassedData" :options="chartDefaultOptions" v-if="!isLoading" />
    </CustomPaper>
  </div>

  <CustomPaper>
    <PageHeader>
      <h3>Letzten Buchungen</h3>
    </PageHeader>
    <Vue3Datatable
      :loading="isLoading"
      noDataContent="Es wurden noch keine Buchungen vorgenommen."
      :rows="rows"
      :columns="cols"
      :showPageSize="false"
      :showNumbers="false"
      :showNumbersCount="false"
      paginationInfo="Es werden die Einträge {0} bis {1} von {2} angezeigt."
     />
  </CustomPaper>

</template>

<style scoped>
.grid{
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(250px, 1fr));
  margin-bottom: -16px;
}

.wrapper{
  display: flex;
  flex-wrap: wrap;
  margin-bottom: -16px;
}

.paper{
  flex-grow: 1;
  margin-right: 0;

  &:last-child{
    margin-right: var(--size-16);
  }

  &.chart{
    height: 300px;
    display: flex;
    justify-content: center;
    align-items: center;
    flex: 1;
  }
}

h1{
  width: fit-content;
  min-width: 50px;
  color: var(--primary-color);
  margin: unset;
}

.header{
  width: 100%;
  display: flex;
  align-items: center;
  justify-content: space-between;
  gap: var(--size-4);

  .icon{
    margin-top: calc(var(--size-8) * -1);
    opacity: .2;
  }
}
</style>
