<template>
  <div class="detail">
    <div class="detail_header">
      <q-btn class="btn"
             color="primary"
             label="保 存" />
      <q-btn class="btn"
             style="background: #FF0000; color: white"
             label="删 除" />
      <q-btn class="btn"
             color="primary"
             label="运 行" />
      <q-btn class="btn"
             color="primary"
             label="停 止" />
      <q-btn class="btn"
             color="primary"
             label="查 看 状 态" />
      <q-btn class="btn"
             color="primary"
             label="查 看 Master 日 志" />
      <q-btn class="btn"
             color="primary"
             label="查 看 Slave 日 志" />
    </div>
    <div class="q-pa-md row">
      <q-input outlined
               readonly
               v-model="text"
               label="主机："
               stack-label
               :dense="false"
               class="col-xs-12 col-sm-4 col-xl-4" />
    </div>
    <div class="q-pa-md row HostList">
      <q-list bordered
              class="col-xs-12 col-sm-6 col-xl-6">
        <q-btn class="btn"
               color="primary"
               label="新 增" />
        <q-btn class="btn"
               style="background: #FF0000; color: white"
               label="删 除" />
        <q-item clickable
                v-ripple
                v-for="(val,ind) in data"
                :key="ind"
                tag="section"
                @dblclick="$router.push({path:'/TestCase/Detail/SlaveHostDetail'})">
          <q-item-section avatar>
            <q-checkbox v-model="datavalue"
                        :val="val" />
          </q-item-section>
          <q-item-section>
            <q-item-label>{{val.name}}</q-item-label>
            <q-item-label caption>{{val.EngineType}}</q-item-label>
          </q-item-section>
        </q-item>
      </q-list>
      <q-list bordered
              class="col-xs-12 col-sm-6 col-xl-6">
        <q-btn class="btn"
               style="background: #FF0000; color: white"
               label="删 除" />
        <q-item clickable
                v-ripple
                v-for="(val,ind) in data"
                :key="ind"
                tag="section"
                @dblclick="$router.push({path:'/TestCase/Detail/HistoryDetail'})">
          <q-item-section avatar>
            <q-checkbox v-model="datavalue"
                        :val="val" />
          </q-item-section>
          <q-item-section>
            <q-item-label>{{val.name}}</q-item-label>
            <q-item-label caption>{{val.EngineType}}</q-item-label>
          </q-item-section>
        </q-item>
      </q-list>
    </div>
  </div>
</template>

<script>
export default {
  name: 'TestCaseDetail',
  data () {
    return {
      value: '',
      data: [
        {
          name: '2020/6/20Test',
          EngineType: '1',
        },
        {
          name: '2020/6/21Test',
          EngineType: '2',
        },
        {
          name: '2020/6/22Test',
          EngineType: '3',
        },
      ],
      datavalue: [],
      text: '11 22 33 33  3 3 3 3 3 3 3 333 3 3s 4'
    }
  }
}
</script>

<style lang="scss" scoped>
.detail {
  width: 100%;
  overflow: hidden;
  .detail_header {
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
  .HostList {
    .btn {
      margin: 5px 10px;
    }
  }
}
</style>
