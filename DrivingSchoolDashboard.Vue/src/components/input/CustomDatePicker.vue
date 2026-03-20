<script setup lang="ts">
import {InputHelper} from "@/helpers/InputHelper.ts";

const props = defineProps({
  id: String,
  label: String,
  modelValue: String,
  placeholder: String,
  disabled: Boolean,
  error: String,
  vertical: Boolean,
  required: Boolean,
});

const setPlaceholder = computed(() => {
  return InputHelper.getInputPlaceholder(
    props.placeholder,
    props.label,
    props.disabled,
    props.required
  );
});

const setId = computed(() => {
  return InputHelper.getInputId(props.id);
});
</script>

<template>
  <BaseInput
    :id="setId"
    :label="label"
    :error="error"
    :vertical="vertical"
    :required="required"
  >
    <input
      type="date"
      v-bind="$attrs"
      :id="setId"
      :value="modelValue"
      :placeholder="setPlaceholder"
      :disabled="disabled"
      :class="{ error: error }"
      @input="$emit('update:modelValue', ($event.target as HTMLInputElement).value)"
    />
  </BaseInput>
</template>
