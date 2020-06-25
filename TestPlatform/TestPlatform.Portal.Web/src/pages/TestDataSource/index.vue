<template>
  <div class="TestDataSource">
    <div class="TestDataSource_header"
         v-if="$route.name=='TestDataSource'">
      <q-btn class="btn"
             color="primary"
             label="新 增"
             @click="fixed=true" />
      <q-btn class="btn"
             style="background: #FF0000; color: white"
             label="删 除" />
    </div>
    <div class="q-pa-md"
         v-if="$route.name=='TestDataSource'">
      <q-list bordered
              separator>
        <q-item clickable
                v-ripple
                v-for="(val,ind) in 3"
                :key="ind"
                tag="section"
                @dblclick="$router.push({path:'/TestDataSource/Detail'})">
          <q-item-section avatar>
            <q-checkbox v-model="value"
                        :val="val" />
          </q-item-section>
          <q-item-section>
            <q-item-label>TestDataSource</q-item-label>
            <q-item-label caption>Hello world</q-item-label>
          </q-item-section>

        </q-item>

      </q-list>
    </div>
    <router-view v-if="$route.name=='TestDataSourceDetail'"></router-view>
  </div>
</template>

<script>
export default {
  name: 'TestDataSource',
  data () {
    return {
      value: []
    }
  }
}
</script>

<style lang="scss" scoped>
.TestDataSource {
  width: 100%;
  overflow: hidden;
  .TestDataSource_header {
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
    }
  }
  .q-pa-md {
    margin-top: 40px;
  }
}
</style>