import {defineStore} from "pinia";
import {ApiHelper} from "@/helpers/ApiHelper.ts";
import type {IDrivingSchool} from "@/interfaces/IDrivingSchool.ts";
import {drivingSchoolApiClient} from "@/apis/DrivingSchoolApiClient.ts";

export const useDrivingSchoolStore = defineStore("drivingSchool", () => {
  const data = ref<IDrivingSchool[]>([]);
  const isLoading = ref(false);

  const fetchAll = async () => {
    if(data.value.length) return;

    isLoading.value = true;
    data.value = await ApiHelper.getAll(drivingSchoolApiClient);
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
