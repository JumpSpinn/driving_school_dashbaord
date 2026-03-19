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

  const findById = (id: number) => {
    return data.value.find(cb => cb.id === id);
  }

  const findIndexById = (id: number) => {
    return data.value.findIndex(cb => cb.id === id);
  }

  return {
    data,
    isLoading,
    fetchAll,
    findById,
    findIndexById
  }
})
