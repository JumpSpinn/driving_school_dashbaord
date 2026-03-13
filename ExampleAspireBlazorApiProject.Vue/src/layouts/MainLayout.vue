<script setup lang="ts">
import Navigation from "@/components/navigation/Navigation.vue";
import {ref} from "vue";

const isNavOpened = ref(true);

const toggleNavigation = () => {
  isNavOpened.value = !isNavOpened.value;
}

const setNavigationOpenState = (isOpen: boolean) => {
  isNavOpened.value = isOpen;
}

</script>

<template>
  <div class="main_layout">
    <header>
      <div class="navbar_toggle" :class="{ 'opened': isNavOpened }" @click="toggleNavigation">
        <i class="pi pi-bars" />
      </div>
      <h3>Fahrschul Manager</h3>
    </header>
    <main>
      <Navigation :isOpen="isNavOpened" @update:isOpen="setNavigationOpenState" />
      <div class="main_content">
        <RouterView />
      </div>
    </main>
  </div>
</template>

<style scoped>
.main_layout{
  display: flex;
  flex-direction: column;
  min-height: 100vh;
  max-height: 100vh;

  main{
    height: 0;
    flex: 1;
    display: flex;

    .main_content{
      flex: 1;
      overflow-y: auto;
    }
  }

  header{
    display: flex;
    align-items: center;
    min-height: 60px;
    padding: var(--size-16);
    border-bottom: 1px solid rgba(255,255,255,.15);
    background-image: linear-gradient(
      to right,
      rgba(var(--primary-rgb), .4),
      rgba(var(--secondary-rgb), .4)
    );

    h3{
      margin: 0;
      padding: 0;
    }

    .navbar_toggle{
      display: flex;
      justify-content: center;
      align-items: center;
      font-size: var(--fs-400);
      margin-right: var(--size-16);
      cursor: pointer;
      color: var(--text-muted);
      transition: var(--t-default-all);

      &.opened{
        color: var(--primary-color);
        opacity: 1;
      }
    }
  }
}
</style>
