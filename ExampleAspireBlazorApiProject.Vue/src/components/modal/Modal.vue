<script setup lang="ts">
import CustomPaper from "@/components/paper/CustomPaper.vue";
import CustomButton from "@/components/button/CustomButton.vue";
import ButtonGroup from "@/components/button/ButtonGroup.vue";
import {onClickOutside} from "@vueuse/core";
import {ref} from "vue";

const props = defineProps({
  open: {
    type: Boolean,
    default: false
  },
  backdropClick: {
    type: Boolean,
    default: true
  }
})

const emit = defineEmits(['abort']);

const target = ref(null);

onClickOutside(target, () => {
  if(!props.open || !props.backdropClick) return;
  emit('abort');
})

</script>

<template>
  <TransitionGroup tag="div" name="fade">
    <div class="modal_backdrop" v-if="props.open">
      <div class="modal" ref="target">
        <div class="header">
          <slot name="header">Modal Title</slot>
          <i class="pi pi-times" @click="emit('abort')" />
        </div>
        <slot name="content">
          <CustomPaper>Modal Content</CustomPaper>
        </slot>
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
    min-width: 500px;
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
  }
}
</style>
