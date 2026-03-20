<script setup lang="ts">

</script>

<template>
  <div class="chip_wrapper">
    <slot />
  </div>
</template>

<style scoped>
.chip_wrapper{
  display: flex;
  flex-wrap: wrap;
  gap: var(--size-4);
  align-content: flex-start;
}
</style>
