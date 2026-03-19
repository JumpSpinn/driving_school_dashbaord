import {defineStore} from "pinia";
import {ApiHelper} from "@/helpers/ApiHelper.ts";
import type {IInstructor} from "@/interfaces/IInstructor.ts";
import {instructorApiClient} from "@/apis/InstructorApiClient.ts";

export const useInstructorStore = defineStore("instructor", () => {
  const data = ref<IInstructor[]>([]);
  const isLoading = ref(false);

  const fetchAll = async () => {
    if(data.value.length) return;

    isLoading.value = true;
    data.value = await ApiHelper.getAll(instructorApiClient);
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
