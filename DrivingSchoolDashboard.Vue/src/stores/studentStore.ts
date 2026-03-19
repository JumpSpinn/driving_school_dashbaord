import {defineStore} from "pinia";
import {ApiHelper} from "@/helpers/ApiHelper.ts";
import type {IStudent} from "@/interfaces/IStudent.ts";
import {studentApiClient} from "@/apis/StudentApiClient.ts";

export const useStudentStore = defineStore("student", () => {
  const data = ref<IStudent[]>([]);
  const isLoading = ref(false);

  const fetchAll = async () => {
    if(data.value.length) return;

    isLoading.value = true;
    data.value = await ApiHelper.getAll(studentApiClient);
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
