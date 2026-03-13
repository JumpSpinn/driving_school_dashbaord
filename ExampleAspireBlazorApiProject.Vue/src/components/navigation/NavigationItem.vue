<script setup lang="ts">

defineProps({
  text: {
    type: String,
    default: "Placeholder"
  },
  primeIcon: {
    type: String,
    default: "pi pi-home"
  },
  active: {
    type: Boolean,
    default: false
  }
})

</script>

<template>
  <div class="nav_item" :class="{ 'active': active }">
    <div class="nav_icon">
      <i :class="[primeIcon]" />
    </div>
    <p>{{ text }}</p>
  </div>
</template>

<style scoped>
.nav_item{
  --text-color: var(--text-muted);
  --icon-color: var(--text-muted);

  display: flex;
  align-items: center;
  gap: var(--size-8);
  border-bottom: 1px solid rgba(255,255,255,.075);
  padding: var(--size-8);
  cursor: pointer;
  transition: all var(--t-duration) ease;

  p{
    color: var(--text-color);
    font-weight: 100;
  }

  .nav_icon{
    display: flex;
    justify-content: center;
    align-items: center;
    padding: var(--size-8);
    font-size: var(--fs-400);
    color: var(--icon-color);
  }

  &:hover:not(.active){
    background-color: rgba(255,255,255,.075);
    --text-color: var(--text);
    --icon-color: var(--text);
  }

  &.active{
    background-color: rgba(255,255,255,.15);
    background-image: linear-gradient(
      to right,
      rgba(var(--primary-rgb), .15),
      rgba(var(--secondary-rgb), .05)
    );
    --text-color: var(--text-nav-active);
    --icon-color: var(--text-nav-active);
  }
}
</style>
