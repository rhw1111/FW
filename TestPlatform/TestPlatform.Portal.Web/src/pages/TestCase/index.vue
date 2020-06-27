<template>
  <div class="TestCase">
    <div class="TestCase_header">
      <q-btn class="btn"
             color="primary"
             label="新 增"
             @click="fixed=true" />
      <q-btn class="btn"
             style="background: #FF0000; color: white"
             label="删 除" />
    </div>

    <div class="q-pa-md">
      <q-list bordered
              separator>
        <q-item clickable
                v-ripple
                v-for="(val,ind) in data"
                :key="ind"
                tag="section"
                @dblclick="$router.push({path:'/TestCase/Detail'})">
          <q-item-section avatar>
            <q-checkbox v-model="value"
                        :val="val" />
          </q-item-section>
          <q-item-section>
            <q-item-label>{{val.name}}</q-item-label>
            <q-item-label caption>{{val.EngineType}}</q-item-label>
          </q-item-section>

        </q-item>

      </q-list>
    </div>

    <q-dialog v-model="fixed">
      <q-card>
        <q-card-section>
          <div class="text-h6">Terms of Agreement</div>
        </q-card-section>

        <q-separator />
        <div class="new_input">
          <q-input v-model="text"
                   label="Name" />
          <q-input v-model="text"
                   label="EngineType" />
          <q-input v-model="text"
                   label="EngineType" />
        </div>

        <q-separator />

        <q-card-actions align="right">
          <q-btn flat
                 label="Decline"
                 color="primary"
                 v-close-popup />
          <q-btn flat
                 label="Accept"
                 color="primary"
                 v-close-popup />
        </q-card-actions>
      </q-card>
    </q-dialog>

  </div>
</template>

<script>
import * as Apis from "@/api/index"
export default {
  name: 'TestCase',
  data () {
    return {
      fixed: false,
      data: [
        {
          name: '2020/6/20Test',
          EngineType: '1',
          Configuration: 'A',
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
      value: [],
      text: ''
    }
  },
  mounted () {
    console.log(this.$route)
    let para = {
      matchName: 'aaa',
      page: 1
    }
    Apis.getbypage(para).then((res) => {
      console.log(res)
    })
  },
  methods: {

  }
}
</script>

<style lang="scss" scoped>
.TestCase {
  width: 100%;
  overflow: hidden;
  .TestCase_header {
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
<style lang="scss">
.new_input {
  width: 500px;
  padding: 5px 10px;
}
</style>