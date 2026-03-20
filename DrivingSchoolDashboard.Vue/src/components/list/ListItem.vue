<template>
  <div class="list_item">
    <p>
      <slot name="default" />
    </p>
    <span>
      <slot name="description" />
    </span>
  </div>
</template>

<style scoped>
.list_item{
  display: flex;
  flex-direction: column;
  gap: var(--size-4);
  margin: var(--size-8) 0;

  p{
    color: var(--text);
    font-size: var(--fs-300);
  }

  span{
    color: var(--text-muted);
    font-size: var(--fs-200);
  }
}
</style>
