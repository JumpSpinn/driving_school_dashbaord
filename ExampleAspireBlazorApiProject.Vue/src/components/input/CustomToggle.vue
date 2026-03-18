<script setup lang="ts">

import {computed} from "vue";
import {InputHelper} from "@/helpers/InputHelper.ts";
import BaseInput from "@/components/input/BaseInput.vue";

const props = defineProps({
  id: String,
  label: String,
  modelValue: Boolean,
  disabled: Boolean,
});

defineEmits(['update:modelValue']);

const setId = computed(() => {
  return InputHelper.getInputId(props.id);
});

</script>

<template>
  <BaseInput
    :id="setId"
    :label="label"
    :vertical="false"
  >
    <label class="switch">
      <input
        type="checkbox"
        v-bind="$attrs"
        :id="setId"
        :checked="modelValue"
        :value="modelValue"
        :disabled="disabled"
        @input="$emit('update:modelValue', !props.modelValue)"
      />
      <span class="slider"></span>
    </label>
  </BaseInput>
</template>

<style scoped>
.switch {
  position: relative;
  display: inline-block;
  width: var(--size-52);
  height: var(--size-28);
  margin-left: auto;
  margin-top: 2px;

  input {
    opacity: 0;
    width: 0;
    height: 0;

    &:disabled + .slider {
      filter: grayscale(1);
      cursor: not-allowed;
      background-color: var(--input-disabled-bg-color);
      border: 1px solid var(--input-disabled-bg-color);
    }

    &:checked + .slider {
      background-color: var(--input-toggle-active-bg-color);
    }

    &:checked + .slider::before {
      transform: translateX(var(--size-24));
      background-color: var(--input-toggle-knob-active-color);
    }

    &:hover:not(:disabled) + .slider {
      border: 1px solid var(--input-hover-border-color);
    }
  }

  .slider {
    position: absolute;
    cursor: pointer;
    top: 0;
    left: 0;
    right: 0;
    bottom: 0;
    background-color: var(--input-toggle-inactive-bg-color);
    border: 1px solid var(--input-toggle-inactive-bg-color);
    border-radius: var(--size-32);
    -webkit-transition: 0.4s;
    transition: 0.4s;

    &::before {
      position: absolute;
      content: '';
      height: var(--size-24);
      width: var(--size-24);
      left: 1px;
      top: 1px;
      background-color: white;
      border-radius: 50%;
      -webkit-transition: 0.4s;
      transition: 0.4s;
    }
  }
}
</style>
