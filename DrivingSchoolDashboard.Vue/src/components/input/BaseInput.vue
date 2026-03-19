<script setup lang="ts">

defineProps({
  id: String,
  label: String,
  error: String as PropType<string | null>,
  vertical: Boolean,
  required: Boolean,
})

</script>

<template>
  <div class="base_input" :class="{ vertical: vertical }">
    <label :for="id" v-if="label" :class="{ required: required }">
      {{ label }}
    </label>
    <div class="base_input_wrapper">
      <slot :id="id" />
      <Transition name="error">
        <span v-if="error" class="error">{{ error }}</span>
      </Transition>
    </div>
  </div>
</template>

<style scoped>
.base_input{
  position: relative;
  display: flex;
  align-items: flex-start;
  justify-content: space-between;
  anchor-name: --base-input-anchor;
  margin-bottom: var(--size-8);

  label {
    anchor-name: --label-anchor;
    font-weight: 400;
    font-size: var(--fs-200);
    white-space: nowrap;
    line-height: var(--size-32);
    color: var(--text-muted);
    text-shadow: 0 1px 0 black;
    flex: 1 1 0;

    &.required {
      &::after {
        content: '*';
        color: rgb(var(--error-color-rgb));
        margin-left: var(--size-4);
      }
    }
  }

  .base_input_wrapper {
    anchor-name: --input-anchor;
    position: relative;
    display: flex;
    flex-direction: column;
    flex: 2 1 0;
  }

  &.vertical {
    flex-direction: column;

    label {
      width: 100%;
    }

    .base_input_wrapper {
      width: 100%;
    }
  }

  .error {
    padding: var(--size-4);

    font-size: 0.7rem;
    color: var(--text);
    font-weight: 200;

    background-color: rgb(var(--error-color-rgb), 0.6);
    border-left: 1px solid rgb(var(--error-color-rgb));
    border-right: 1px solid rgb(var(--error-color-rgb));
    border-bottom: 1px solid rgb(var(--error-color-rgb));
  }

  .error-enter-active,
  .error-leave-active {
    transition: all var(--t-duration) ease;
  }

  .error-enter-from,
  .error-leave-to {
    opacity: 0;
  }
}
</style>
