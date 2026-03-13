<script setup lang="ts">
import NavigationItem from "@/components/navigation/NavigationItem.vue";
import {useRoute, useRouter} from "vue-router";
import {RoutePage} from "@/enums/RoutePage.ts";

defineProps({
  isOpen: {
    type: Boolean,
    default: false
  }
})

const route = useRoute();
const router = useRouter();
const navigateTo = (path: string) => router.push(path);
const isActive = (name: RoutePage) => route.name === name;

</script>

<template>
  <nav :class="{ closed: !isOpen }">
    <NavigationItem @click="navigateTo(RoutePage.DASHBOARD)" text="Dashboard" :active="isActive(RoutePage.DASHBOARD)" />
    <NavigationItem @click="navigateTo(RoutePage.DRIVING_SCHOOL)" text="Fahrschulen" :active="isActive(RoutePage.DRIVING_SCHOOL)" />
    <NavigationItem @click="navigateTo(RoutePage.STUDENTS)" text="Fahrschüler" :active="isActive(RoutePage.STUDENTS)" />
    <NavigationItem @click="navigateTo(RoutePage.INSTRUCTOR)" text="Fahrlehrer" :active="isActive(RoutePage.INSTRUCTOR)" />
    <NavigationItem @click="navigateTo(RoutePage.THEORY_LESSON)" text="Kurse" :active="isActive(RoutePage.THEORY_LESSON)" />
    <NavigationItem @click="navigateTo(RoutePage.COURSE_BOOKING)" text="Kursbuchungen" :active="isActive(RoutePage.COURSE_BOOKING)" />
  </nav>
</template>

<style scoped>
nav{
  --nav-width: 300px;
  width: var(--nav-width);

  background-image: linear-gradient(
    to right,
    rgba(var(--primary-rgb), .3),
    rgba(var(--secondary-rgb), .3)
  );
  background-size: 150%;

  border-right: 1px solid rgba(255,255,255,.15);

  overflow: hidden;
  transition: var(--t-default-all);

  &.closed{
    --nav-width: 0;
    transform: translateX(-100%);
  }
}
</style>
