<template>
  <div class="list_wrapper">
    <slot />
  </div>
</template>

<style scoped>
.list_wrapper{
  display: flex;
  flex-direction: column;
  gap: var(--size-4);

  overflow-y: auto;
  max-height: 400px;
}
</style>
