<script setup lang="ts">

const props = defineProps({
  size:         { type: Number, default: 160 },
  fillColor:    { type: String, default: 'var(--secondary-color)' },
  borderColor:  { type: String, default: '#ffffff50' },
  strokeWidth:  { type: Number, default: 5.5 },
  duration:     { type: Number, default: 1500 },
  glowStrength: { type: Number, default: 10 },
  glowColor:    { type: String, default: '#80b7ec' },
})

const filterId = `glow-${Math.random().toString(36).slice(2, 7)}`
const resolvedGlowColor = computed(() => props.glowColor || props.fillColor)

const animPath   = ref(null)
const pathLength = ref(0)

const pathD = computed(() => {
  const s       = props.size
  const pad     = s * 0.08
  const bottomY = s - pad
  return `M${s / 2},${bottomY} L${pad},${bottomY} L${s / 2},${pad} L${s - pad},${bottomY}`
})

function measureLength() {
  if (animPath.value) {
    //@ts-ignore
    pathLength.value = Math.round(animPath.value.getTotalLength())
  }
}

onMounted(measureLength)

watch(() => props.size, () => setTimeout(measureLength, 0))

const animStyle = computed(() => ({
  '--len':           `${pathLength.value}px`,
  strokeDasharray:   `${pathLength.value}px`,
  strokeDashoffset:  `${pathLength.value}px`,
  animation:         `triangleDraw ${props.duration}ms linear infinite`,
}))
</script>

<template>
  <svg
    :width="size"
    :height="size"
    :viewBox="`0 0 ${size} ${size}`"
    xmlns="http://www.w3.org/2000/svg"
  >
    <defs>
      <filter :id="filterId" x="-50%" y="-50%" width="200%" height="200%">
        <feGaussianBlur in="SourceGraphic" :stdDeviation="glowStrength" result="blur" />
        <feFlood :flood-color="resolvedGlowColor" flood-opacity="1" result="color" />
        <feComposite in="color" in2="blur" operator="in" result="coloredBlur" />
        <feMerge>
          <feMergeNode in="coloredBlur" />
          <feMergeNode in="coloredBlur" />
          <feMergeNode in="SourceGraphic" />
        </feMerge>
      </filter>
    </defs>
    <path
      :d="pathD"
      fill="none"
      :stroke="borderColor"
      :stroke-width="strokeWidth"
      stroke-linecap="round"
      stroke-linejoin="round"
    />
    <path
      ref="animPath"
      :d="pathD"
      fill="none"
      :stroke="fillColor"
      :stroke-width="strokeWidth"
      stroke-linecap="round"
      stroke-linejoin="round"
      :filter="glowStrength > 0 ? `url(#${filterId})` : undefined"
      :style="animStyle"
    />
  </svg>
</template>

<style>
svg{
  padding: 0 var(--size-32);
}

@keyframes triangleDraw {
  0%   {
    stroke-dashoffset: var(--len);
  }
  100% {
    stroke-dashoffset: 0;
  }
}
</style>
