<template>
  <div class="MasterHost">
    <div class="q-pa-md row masterhost">
      <SSHEndpoint />
      <TestHost />
    </div>

  </div>
</template>

<script>
import SSHEndpoint from "./SSHEndpoint"
import TestHost from "./TestHost"
export default {
  name: 'MasterHost',
  components: {
    TestHost,
    SSHEndpoint
  }
}
</script>

<style lang="scss" scoped>
.masterhost {
  .btn {
    margin-right: 10px;
  }
}
</style>
<style lang="scss">
.q-table {
  .text-left {
    white-space: nowrap;
    overflow: hidden;
    text-overflow: ellipsis;
  }
}
.q-table--col-auto-width {
  width: 75px;
}
.q-textarea .q-field__native {
  resize: none;
}
.new_input {
  width: 100%;
  padding: 10px 30px;
  .input_row {
    margin-bottom: 10px;
  }
}
</style>