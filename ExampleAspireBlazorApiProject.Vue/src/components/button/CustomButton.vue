<script setup lang="ts">

interface CustomButtonProps {
  type?: "primary" | "secondary" | "neutral",
  outline?: boolean
  disabled?: boolean
}

withDefaults(defineProps<CustomButtonProps>(), {
  type: "primary",
  outline: true,
});

</script>

<template>
  <button :class="[type, { outline: outline, disabled: disabled }]" :disabled="disabled">
    <slot />
  </button>
</template>

<style scoped>
button{
  border: none;
  outline: none;
  cursor: pointer;

  background-color: unset;
  padding: var(--size-8) var(--size-16);
  border-radius: var(--size-8);
  transition: var(--t-default-all);

  color: var(--text);
  font-size: var(--fs-200);

  user-select: none;
  width: fit-content;

  &.primary{
    border: 1px solid var(--primary-color);
    color: var(--primary-color);

    &:not(.outline){
      background-color: var(--primary-color);
      color: hsl(0, 0%, 95%);
    }

    &:hover:not(.disabled){
      background-color: var(--primary-color);
      border-color: var(--primary-color);
      color: hsl(0, 0%, 95%);

      &:not(.outline){
        box-shadow: 0 0 5px var(--primary-color);
      }
    }
  }

  &.secondary{
    border: 1px solid var(--secondary-color);
    color: var(--secondary-color);

    &:not(.outline){
      background-color: var(--secondary-color);
      color: hsl(0, 0%, 95%);
    }

    &:hover:not(.disabled){
      background-color: var(--secondary-color);
      border-color: var(--secondary-color);
      color: hsl(0, 0%, 95%);

      &:not(.outline){
        box-shadow: 0 0 5px var(--secondary-color);
      }
    }
  }

  &.neutral{
    border: 1px solid var(--text);
    color: var(--text);

    &:not(.outline){
      background-color: var(--text);
      color: var(--bg);
    }

    &:hover:not(.disabled){
      background-color: var(--text);
      border-color: var(--text);
      color: var(--bg);

      &:not(.outline){
        box-shadow: 0 0 5px var(--text);
      }
    }
  }

  &.disabled{
    filter: grayscale(1);
    opacity: .75;
    cursor: not-allowed;
  }
}
</style>
