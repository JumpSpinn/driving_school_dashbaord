<template>
  <div class="chip">
    <div class="avatar">
      <slot name="short"></slot>
    </div>
    <p>
      <slot name="long"></slot>
    </p>
  </div>
</template>

<style scoped>
.chip{
  border-radius: var(--size-16);
  padding: var(--size-4) var(--size-8) var(--size-4) var(--size-4);

  display: flex;
  align-items: center;
  gap: var(--size-8);

  background-color: var(--bg);
  width: max-content;

  .avatar{
    aspect-ratio: 1/1;
    height: var(--size-28);
    border-radius: 50%;
    display: flex;
    justify-content: center;
    align-items: center;
    font-size: var(--fs-200);
    color: var(--text);
    background-color: gray;
  }

  p{
    color: var(--text);
    font-size: var(--fs-200);
  }
}
</style>
