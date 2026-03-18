<script setup lang="ts">
import {computed, type PropType} from "vue";
import BaseInput from "@/components/input/BaseInput.vue";
import {InputHelper} from "@/helpers/InputHelper.ts";

const props = defineProps({
  id: String,
  label: String,
  modelValue: String,
  placeholder: String,
  disabled: Boolean,
  error: String as PropType<string | null>,
  vertical: Boolean,
  required: Boolean,
});

defineEmits(['update:modelValue']);

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
      type="text"
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
