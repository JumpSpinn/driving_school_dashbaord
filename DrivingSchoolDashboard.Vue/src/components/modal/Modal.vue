<script setup lang="ts">
import {onClickOutside} from "@vueuse/core";
import type {IModalOptions} from "@/interfaces/IModalOptions.ts";
import {KeyUtil} from "@/utils/KeyUtil.ts";
import {Key} from "@/enums/Key.ts";

const props = defineProps({
  open: {
    type: Boolean,
    default: false
  },
  options: {
    type: Object as () => IModalOptions | null,
    default: null
  },
  error: {
    type: String as PropType<string | null>,
    default: null
  }
})

const emit = defineEmits(['abort']);

const slots = useSlots();

const target = ref(null);

onClickOutside(target, () => {
  if(!props.open) return;

  if(!props.options?.backdropClick) return;
  emit('abort');
})

onMounted(() => {
  KeyUtil.register(Key.ESC, {
    onPress: () => {
      if(!props.open) return;
      if(!props.options?.closeWithEsc) return;
      emit('abort');
    }
  })
})

</script>

<template>
  <TransitionGroup tag="div" name="fade">
    <div class="modal_backdrop" v-if="props.open">
      <div class="modal" ref="target">
        <div class="header">
          <div>
            <slot name="header">Modal Title</slot>
          </div>
          <i class="pi pi-times" @click="emit('abort')" />
        </div>
        <CustomPaper v-if="slots.content">
          <slot name="content">
            Modal Content Paper
          </slot>
        </CustomPaper>
        <slot name="default" v-if="slots.default">
          Modal Content without Paper
        </slot>
        <TransitionGroup tag="div" name="fade">
          <div class="error" v-if="error">{{ error }}</div>
        </TransitionGroup>
        <div class="actions">
          <slot name="actions">
            <ButtonGroup>
              <CustomButton type="primary">Modal Action #1</CustomButton>
              <CustomButton type="primary">Modal Action #2</CustomButton>
            </ButtonGroup>
          </slot>
        </div>
      </div>
    </div>
  </TransitionGroup>
</template>

<style scoped>
.modal_backdrop{
  position: absolute;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  background-color: rgba(0, 0, 0, .2);
  backdrop-filter: blur(2px);
  box-shadow: var(--shadow-combined);
  border: 1px solid var(--border);

  display: flex;
  justify-content: center;
  align-items: center;

  z-index: 100;

  &.fade-enter-active,
  &.fade-leave-active{
    transition: var(--t-default-all);
  }
  &.fade-enter-from,
  &.fade-leave-to{
    opacity: 0;
  }

  .modal{
    min-width: 550px;
    max-width: 550px;
    border-radius: var(--size-8);
    background-color: var(--bg);

    .header, .actions{
      display: flex;
      align-items: center;
      justify-content: space-between;
      padding: var(--size-8) var(--size-16);
    }

    .header{
      border-bottom: 1px solid var(--border);

      i{
        cursor: pointer;
        opacity: .5;
        transition: var(--t-default-all);

        &:hover{
          opacity: 1;
        }
      }
    }

    .actions{
      border-top: 1px solid var(--border);
      justify-content: flex-end;
    }

    .error{
      margin: var(--size-16);
      border-radius: var(--size-8);
      padding: var(--size-16);
      box-shadow: var(--shadow-dark);

      font-size: var(--fs-200);
      line-height: var(--size-20);
      color: var(--text);
      font-weight: 200;

      background-color: rgb(var(--error-color-rgb), 0.6);
      border: 1px solid rgb(var(--error-color-rgb));

      &.fade-enter-active,
      &.fade-leave-active{
        transition: var(--t-default-all);
      }
      &.fade-enter-from,
      &.fade-leave-to{
        opacity: 0;
        transform: translateY(-10px);
      }
    }
  }
}
</style>
