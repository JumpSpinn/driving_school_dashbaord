<script setup lang="ts">
import NavigationItem from "@/components/navigation/NavigationItem.vue";
import {useRoute, useRouter} from "vue-router";
import {RoutePage} from "@/enums/RoutePage.ts";
import {ref} from "vue";
import NavigationAutoClose from "@/components/navigation/NavigationAutoClose.vue";

defineProps({
  isOpen: {
    type: Boolean,
    default: false
  }
})

const emit = defineEmits(['update:isOpen']);
const route = useRoute();
const router = useRouter();
const autoClose = ref(false);

const navigateTo = (path: string) => {
  router.push(path);
  if(autoClose.value)
    emit('update:isOpen', false);
}

const isActive = (name: RoutePage) => route.name === name;

</script>

<template>
  <nav :class="{ closed: !isOpen }">
    <NavigationItem class="nav_selector" prime-icon="pi pi-home" @click="navigateTo(RoutePage.DASHBOARD)" text="Dashboard" :active="isActive(RoutePage.DASHBOARD)" />
    <NavigationItem class="nav_selector" prime-icon="pi pi-car" @click="navigateTo(RoutePage.DRIVING_SCHOOL)" text="Fahrschulen" :active="isActive(RoutePage.DRIVING_SCHOOL)" />
    <NavigationItem class="nav_selector" prime-icon="pi pi-users" @click="navigateTo(RoutePage.STUDENTS)" text="Fahrschüler" :active="isActive(RoutePage.STUDENTS)" />
    <NavigationItem class="nav_selector" prime-icon="pi pi-users" @click="navigateTo(RoutePage.INSTRUCTOR)" text="Fahrlehrer" :active="isActive(RoutePage.INSTRUCTOR)" />
    <NavigationItem class="nav_selector" prime-icon="pi pi-book" @click="navigateTo(RoutePage.THEORY_LESSON)" text="Kurse" :active="isActive(RoutePage.THEORY_LESSON)" />
    <NavigationItem class="nav_selector" prime-icon="pi pi-calendar" @click="navigateTo(RoutePage.COURSE_BOOKING)" text="Kursbuchungen" :active="isActive(RoutePage.COURSE_BOOKING)" />
    <NavigationAutoClose class="auto_close nav_selector" :auto-close-on="autoClose" @update:autoCloseOn="autoClose = $event" />
  </nav>
</template>

<style scoped>
nav{
  --nav-width: 300px;
  display: flex;
  flex-direction: column;
  width: var(--nav-width);

  background-image: linear-gradient(
    to right,
    rgba(var(--primary-rgb), .3),
    rgba(var(--secondary-rgb), .3)
  );
  background-size: 150%;

  border-right: 1px solid rgba(255,255,255,.15);

  overflow: hidden;
  transition: all 750ms ease-in-out;

  .nav_selector {
    transition: transform 1s ease-in-out;
    transform: translateX(0%);
  }

  &.closed{
    --nav-width: 0;
    transform: translateX(-150%);

    .nav_selector {
      transition: transform 250ms ease-in-out, opacity 250ms ease-in-out;
      opacity: 0;
    }
  }

  .auto_close{
    margin-top: auto;
  }
}
</style>
