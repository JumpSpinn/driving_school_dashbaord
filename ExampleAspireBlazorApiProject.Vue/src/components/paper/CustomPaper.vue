<script setup lang="ts">

</script>

<template>
  <div class="custom_paper">
    <slot />
  </div>
</template>

<style scoped>
.custom_paper{
  margin: var(--size-16);
  border-radius: var(--size-8);
  background-color: var(--bg-light);
  border: 1px solid var(--border);
  padding: var(--size-16);
  box-shadow: var(--shadow-dark);
}
</style>
