<script setup lang="ts">
import LoadingSpinnerIcon from "@/components/loading/LoadingSpinnerIcon.vue";

defineProps({
  text: {
    type: String,
    default: "Daten werden geladen.."
  }
})

</script>

<template>
  <div class="loading_spinner">
    <LoadingSpinnerIcon :size="160" :strokeWidth="8" />
    <p>{{ text }}</p>
  </div>
</template>

<style scoped>
.loading_spinner{
  position: absolute;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;

  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;

  p{
    font-size: var(--fs-200);
    margin-top: var(--size-16);
    animation: loadingText 1.5s infinite;
  }
}

@keyframes loadingText {
  30%, 100% {
    opacity: 1;
  }
  50%{
    opacity: .4;
  }
}
</style>
