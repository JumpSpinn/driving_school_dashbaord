<template>
  <div class="page_header">
    <div class="content">
      <slot name="default" />
      <div class="desc">
        <slot name="description" />
      </div>
    </div>
    <div class="actions">
      <slot name="actions" />
    </div>
  </div>
</template>

<style scoped>
.page_header{
  display: flex;
  justify-content: space-between;
  align-items: center;

  .desc{
    color: var(--text-muted);
  }

  .actions{
    display: flex;
    align-items: center;
    gap: var(--size-8);
  }
}
</style>
