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

  return {
    data,
    isLoading,
    fetchAll,
  }
})
