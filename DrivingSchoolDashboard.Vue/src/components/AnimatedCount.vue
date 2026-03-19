<script setup lang="ts">

const props = defineProps<{
  value: number
}>();

const displayValue = ref(0)
let animationFrame: number | null = null

const animateTo = (target: number) => {
  if (animationFrame) cancelAnimationFrame(animationFrame)

  const duration = Math.min(800 + target * 10, 2400)
  const startTime = performance.now()
  const startValue = displayValue.value

  const ease = (t: number): number => 1 - Math.pow(1 - t, 3)

  const step = (currentTime: number) => {
    const elapsed = currentTime - startTime
    const progress = Math.min(elapsed / duration, 1)
    displayValue.value = Math.round(startValue + (target - startValue) * ease(progress))

    if (progress < 1) {
      animationFrame = requestAnimationFrame(step)
    } else {
      displayValue.value = target
    }
  }

  animationFrame = requestAnimationFrame(step)
}

onMounted(() => animateTo(props.value))

watch(() => props.value, (newVal) => animateTo(newVal))

</script>

<template>
  {{ displayValue }}
</template>

<style scoped>

</style>
