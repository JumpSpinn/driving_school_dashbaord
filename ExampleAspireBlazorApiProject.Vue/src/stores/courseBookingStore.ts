import {defineStore} from "pinia";
import {ApiHelper} from "@/helpers/ApiHelper.ts";
import type {ICourseBooking} from "@/interfaces/ICourseBooking.ts";
import {courseBookingApiClient} from "@/apis/CourseBookingApiClient.ts";

export const useCourseBookingStore = defineStore("courseBooking", () => {
  const data = ref<ICourseBooking[]>([]);
  const isLoading = ref(false);

  const fetchAll = async () => {
    if(data.value.length) return;

    isLoading.value = true;
    data.value = await ApiHelper.getAll(courseBookingApiClient);
    isLoading.value = false;
  }

  return {
    data,
    isLoading,
    fetchAll,
  }
})
