<script setup lang="ts">

const props = defineProps({
  autoCloseOn: {
    type: Boolean,
    default: false
  }
})

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
    <p>Autom. schließen</p>
    <input type="checkbox" :checked="autoCloseOn" @click="toggleAutoClose" />
  </div>
</template>

<style scoped>
.nav_auto_close{
  display: flex;
  align-items: center;
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

  input{
    margin-left: auto;
    margin-right: var(--size-8);
  }
}
</style>
