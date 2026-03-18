<script setup lang="ts">

import CustomToggle from "@/components/input/CustomToggle.vue";
import {computed} from "vue";

const props = defineProps({
  autoCloseOn: {
    type: Boolean,
    default: false
  }
})

const isToggled = computed(() => props.autoCloseOn);

const emit = defineEmits(['update:autoCloseOn']);

const toggleAutoClose = () => {
  emit('update:autoCloseOn', !props.autoCloseOn);
}

</script>

<template>
  <div class="nav_auto_close">
    <div class="nav_icon">
      <i class="pi pi-cog" />
    </div>
    <CustomToggle class="toggle" label="Autom. schließen" v-model="isToggled" @update:modelValue="toggleAutoClose"  />
  </div>
</template>

<style scoped>
.nav_auto_close{
  display: flex;
  border-top: 1px solid rgba(255,255,255,.075);
  padding: var(--size-8);

  .nav_icon{
    display: flex;
    justify-content: center;
    align-items: center;
    padding: var(--size-8);
    font-size: var(--fs-400);
    color: var(--icon-color);
  }

  .toggle{
    display: flex;
    justify-content: center;
    align-items: center;
    width: 100%;
    height: 100%;
  }
}
</style>
