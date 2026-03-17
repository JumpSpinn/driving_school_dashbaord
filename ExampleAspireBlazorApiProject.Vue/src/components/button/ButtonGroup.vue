<template>
  <div class="button_group">
    <slot />
  </div>
</template>

<style scoped>
.button_group{
  display: flex;
  align-items: center;
  gap: var(--size-8);
}
</style>
