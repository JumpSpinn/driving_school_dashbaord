import {defineStore} from "pinia";
import {ApiHelper} from "@/helpers/ApiHelper.ts";
import type {ITheoryLesson} from "@/interfaces/ITheoryLesson.ts";
import {theoryLessonApiClient} from "@/apis/TheoryLessonApiClient.ts";

export const useTheoryLessonStore = defineStore("theoryLesson", () => {
  const data = ref<ITheoryLesson[]>([]);
  const isLoading = ref(false);

  const fetchAll = async () => {
    if(data.value.length) return;

    isLoading.value = true;
    data.value = await ApiHelper.getAll(theoryLessonApiClient);
    isLoading.value = false;
  }

  return {
    data,
    isLoading,
    fetchAll,
  }
})
