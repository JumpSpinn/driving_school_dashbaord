<script setup lang="ts">
import LoadingSpinner from "@/components/loading/LoadingSpinner.vue";
import {onMounted, ref} from "vue";
import type {ICourseBooking} from "@/interfaces/ICourseBooking.ts";
import {courseBookingApiClient} from "@/apis/CourseBookingApiClient.ts";
import CustomButton from "@/components/button/CustomButton.vue";
import CustomPaper from "@/components/paper/CustomPaper.vue";
import PageHeader from "@/components/header/PageHeader.vue";

const isLoading = ref(true);
const courseBookings = ref<ICourseBooking[]>([]);

onMounted(async () => {
  await loadData();
})

const loadData = async () => {
  const resp = await courseBookingApiClient.getAll();

  if(resp == null)
    console.error("Error while fetching course bookings");
  else
    courseBookings.value = resp;

  isLoading.value = false;
}
</script>

<template>
  <LoadingSpinner v-if="isLoading" text="Kursbuchungen werden geladen.." />
  <CustomPaper>
    <PageHeader>
      <h3>Kursbuchungen</h3>
      <template #description>
        <p>Lorem ipsum dolor sit amet</p>
      </template>
      <template #actions>
        <CustomButton type="secondary" :outline="true" :disabled="isLoading">Buchung eintragen</CustomButton>
      </template>
    </PageHeader>
  </CustomPaper>
</template>

<style scoped>

</style>
