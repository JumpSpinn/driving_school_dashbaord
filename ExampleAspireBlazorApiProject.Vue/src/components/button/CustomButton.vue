<script setup lang="ts">

interface CustomButtonProps {
  type?: "primary" | "secondary" | "neutral" | "success" | "info" | "warn" | "error",
  outline?: boolean
  minWidth?: number
  disabled?: boolean
}

withDefaults(defineProps<CustomButtonProps>(), {
  type: "primary",
  outline: true,
  minWidth: 100,
});

</script>

<template>
  <button :style="{ 'min-width': `${minWidth ?? 0}px` }" :class="[type, { outline: outline, disabled: disabled }]" :disabled="disabled">
    <slot />
  </button>
</template>

<style scoped>
button{
  --button-color: var(--primary-color);

  outline: none;
  cursor: pointer;
  background-color: unset;
  padding: var(--size-8) var(--size-8);
  border-radius: var(--size-8);
  transition: var(--t-default-all);
  font-size: var(--fs-200);
  user-select: none;
  width: fit-content;
  border: 1px solid var(--button-color);
  color: var(--button-color);

  &:not(.outline){
    background-color: var(--button-color);
    color: hsl(0, 0%, 95%);
  }

  &:hover:not(.disabled){
    background-color: var(--button-color);
    border-color: var(--button-color);
    color: hsl(0, 0%, 95%);

    &:not(.outline){
      box-shadow: 0 0 5px var(--button-color);
    }
  }

  &.primary{
    --button-color: var(--primary-color);
  }

  &.secondary{
    --button-color: var(--secondary-color);
  }

  &.secondary{
    --button-color: var(--secondary-color);
  }

  &.success{
    --button-color: var(--success-color);
  }

  &.info{
    --button-color: var(--info-color);
  }

  &.warn{
    --button-color: var(--warn-color);
  }

  &.error{
    --button-color: var(--error-color);
  }

  &.neutral{
    --button-color: var(--text);

    &:not(.outline){
      color: var(--bg);
    }

    &:hover:not(.disabled){
      color: var(--bg);
    }
  }

  &.disabled{
    filter: grayscale(1);
    opacity: .75;
    cursor: not-allowed;
  }
}
</style>
