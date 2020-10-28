<template>
  <div>
    <router-view v-if="isRouterAlive"></router-view>
  </div>
</template>

<script>

export default {
  provide () {
    return {
      reload: this.reload
    }
  },
  components: {

  },
  data () {
    return {
      isRouterAlive: true,
    }
  },
  methods: {
    reload () {
      this.isRouterAlive = false;
      this.$nextTick(() => {
        this.isRouterAlive = true;
      })
    }
  }
}
</script>

<style>
html,
body {
  height: 100%;
}
.q-table--col-auto-width {
  width: 75px;
}
</style>
