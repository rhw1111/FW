<template>
  <div class="detail">
    <div class="detail_header">
      <q-btn class="btn"
             color="primary"
             label="保 存" />
      <q-btn class="btn"
             style="background: #FF0000; color: white"
             label="删 除" />
    </div>
  </div>
</template>

<script>
export default {
  name: 'SlaveHostDetail'
}
</script>

<style lang="scss" scoped>
.detail {
  width: 100%;
  overflow: hidden;
  .detail_header {
    position: fixed;
    left: 0;
    right: 0;
    padding: 10px 16px 5px;
    width: 100%;
    z-index: 10;
    box-sizing: border-box;
    background: #ffffff;
    .btn {
      margin-right: 10px;
      margin-bottom: 5px;
    }
  }
}
</style>
