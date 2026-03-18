<script setup lang="ts">
import {computed, ref} from "vue";
import {InputHelper} from "@/helpers/InputHelper.ts";
import {onClickOutside} from "@vueuse/core";
import BaseInput from "@/components/input/BaseInput.vue";
import { ChevronDown } from 'lucide-vue-next';

const props = defineProps({
  id: String,
  label: String,
  modelValue: [String, Number, Boolean],
  options: {
    type: Array as () => {
      label: string;
      value: any;
    }[],
    required: true,
  },
  placeholder: String,
  error: String,
  required: Boolean,
  disabled: Boolean,
  vertical: Boolean,
  searchOn: Boolean,
});

const emit = defineEmits(['update:modelValue']);

const isOpen = ref(false);
const target = ref(null);
const search = ref('');

const setId = computed(() => {
  return InputHelper.getInputId(props.id);
});

const selectedLabel = computed(() => {
  const option = props.options.find((x) => x.value === props.modelValue);
  return option ? option.label : props.placeholder || '..';
});

const getOptions = computed(() => {
  if (!props.options) return [];

  if (!props.searchOn) return props.options;

  return props.options.filter((x) => x.label.toLowerCase().includes(search.value.toLowerCase()));
});

const selectOption = (value: any) => {
  emit('update:modelValue', value);
  isOpen.value = false;
  search.value = '';
};

const toggleOpen = () => {
  if (props.disabled) return;
  isOpen.value = !isOpen.value;
};

onClickOutside(target, () => {
  isOpen.value = false;
  search.value = '';
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
    <div
      class="custom_select"
      ref="target"
      :class="{ disabled: disabled, error: error }"
    >
      <div
        class="select_trigger"
        @click="toggleOpen"
        :class="{ open: isOpen, error: error }"
      >
        <span :class="{ placeholder: !modelValue }">{{ selectedLabel }}</span>
        <ChevronDown
          class="icon"
          :class="{ rotate: isOpen }"
          :size="18"
        />
      </div>

      <Transition name="fade-slide">
        <div
          class="select_options"
          v-if="isOpen"
          :class="{ search: searchOn }"
        >
          <input
            v-if="searchOn"
            class="select_search"
            type="text"
            placeholder="Suchen.."
            v-model="search"
          />

          <div class="options_list">
            <div
              v-for="option in getOptions"
              :key="option.label"
              :class="{ selected: option.value === modelValue }"
              class="option_item"
              @click="selectOption(option.value)"
            >
              {{ option.label }}
            </div>
            <div
              class="option_item no_search"
              v-if="getOptions.length <= 0"
            >
              keine Ergebnisse
            </div>
          </div>
        </div>
      </Transition>
    </div>
  </BaseInput>
</template>

<style scoped>
.custom_select {
  position: relative;
  cursor: pointer;

  .select_trigger {
    position: relative;
    anchor-name: --dropdown-trigger;
    height: var(--size-32);
    color: var(--text);
    background-color: var(--input-bg-color);
    border: 1px solid var(--input-border-color);
    outline: none;
    padding: var(--size-8);
    border-radius: var(--size-4);

    display: flex;
    align-items: center;
    justify-content: space-between;
    font-size: var(--fs-200);

    transition: var(--t-default-all);

    .placeholder {
      font-size: var(--fs-200);
      color: var(--input-placeholder-text-color);
      opacity: var(--input-placeholder-text-opacity);
    }

    .icon {
      transition: var(--t-default-all);

      &.rotate {
        transform: rotate(180deg);
      }
    }

    &:hover:not(.open, .error) {
      border: 1px solid var(--input-hover-border-color);
    }

    &.open {
      border-radius: var(--size-4) var(--size-4) 0 0;
    }
  }

  .select_options {
    position: absolute;
    width: 100%;
    z-index: 2;

    position-anchor: --dropdown-trigger;
    position-area: bottom;
    backdrop-filter: blur(var(--size-4));

    background-color: var(--input-bg-color);
    border: 1px solid var(--input-border-color);
    border-top: unset;
    border-bottom-left-radius: var(--size-4);
    border-bottom-right-radius: var(--size-4);

    .options_list {
      display: flex;
      flex-direction: column;

      max-height: calc(var(--size-28) * 5);
      overflow-y: auto;
    }

    .select_search {
      border-radius: unset;
      width: 100%;
    }

    .option_item {
      color: var(--text-muted);
      padding: var(--size-4);
      transition: var(--t-default-all);
      font-size: var(--fs-200);

      &:hover:not(.selected, .no_search) {
        background-color: rgba(255, 255, 255, 0.2);
        color: var(--text);
        background-color: rgb(var(--secondary-rgb), 0.3);
      }

      &.selected {
        background-color: rgb(var(--secondary-rgb));
        color: var(--text);
      }

      &.no_search {
        cursor: default;
        font-style: italic;
      }
    }
  }

  &.disabled {
    filter: grayscale(1);
    cursor: not-allowed;
    --input-bg-color: var(--input-disabled-bg-color);
    --input-border-color: var(--input-disabled-bg-color);
  }

  &.error {
    .select_trigger {
      border-radius: var(--size-4) var(--size-4) 0 0;
      --input-border-color: var(--input-error-border-color);
      border-bottom: unset;
    }
  }

  .fade-slide-enter-active,
  .fade-slide-leave-active {
    transition: var(--t-default-all);
  }

  .fade-slide-enter-from,
  .fade-slide-leave-to {
    opacity: 0;
    transform: translateY(-10px);
  }
}
</style>
