<template>
  <div class="testcase">
    <q-layout view="hHh lpR fFf">

      <q-header elevated
                class="bg-primary text-white"
                height-hint="98">

        <q-tabs align="left">
          <q-route-tab to="/Directory"
                       label="目录管理" />
          <q-route-tab to="/TestCase"
                       label="测试用例" />
          <q-route-tab to="/TestDataSource"
                       label="测试数据源" />
          <q-route-tab to="/MasterHost"
                       label="主机" />
        </q-tabs>
      </q-header>

      <q-page-container>
        <router-view />
      </q-page-container>

    </q-layout>
  </div>
</template>

<script>
export default {
  name: 'testcase',
  data () {
    return {

    }
  },
  mounted () {

  },
  methods: {

  }
}
</script>

<style lang="scss" scoped>
</style>