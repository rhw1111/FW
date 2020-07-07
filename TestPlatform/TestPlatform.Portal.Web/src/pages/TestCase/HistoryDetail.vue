<template>
  <div class="detail">
    <div class="detail_header">
      <q-btn class="btn"
             style="background: #FF0000; color: white"
             label="删 除"
             @click="deleteHistory" />
    </div>
    <div class="q-pa-md row">

      <div class="new_input">
        <div class="row">
          <q-input v-model="createTime"
                   :dense="false"
                   class="col"
                   readonly>
            <template v-slot:before>
              <span style="font-size:14px">创建时间:</span>
            </template>
          </q-input>
        </div>

        <div class="row">
          <q-input v-model="summary"
                   :dense="false"
                   class="col-xs-12"
                   type="textarea"
                   outlined
                   readonly>
            <template v-slot:before>
              <span style="font-size:14px">总结:</span>
            </template>
          </q-input>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import * as Apis from "@/api/index"
export default {
  name: 'HistoryDetail',
  data () {
    return {
      caseId: '',
      historyId: '',
      createTime: '',
      summary: ''
    }
  },
  mounted () {
    this.caseId = this.$route.query.caseId;
    this.historyId = this.$route.query.historyId;
    this.getHistoryDetail();
  },
  methods: {
    //获得历史记录详情
    getHistoryDetail () {
      this.$q.loading.show()
      let para = {
        caseId: this.caseId,
        historyId: this.historyId
      }
      Apis.getHistoryDetail(para).then((res) => {
        console.log(res)
        this.createTime = res.data.createTime;
        this.summary = res.data.summary;
        this.$q.loading.hide()
      })
    },
    //获得
    deleteHistory () {
      this.$q.dialog({
        title: '提示',
        message: '您确定要删除当前的历史记录吗',
        persistent: true,
        ok: {
          push: true,
          label: '确定'
        },
        cancel: {
          push: true,
          label: '取消'
        },
      }).onOk(() => {
        this.$q.loading.show()
        // 单个删除slaveHost列表
        let para = `?caseId=${this.caseId}&historyId=${this.historyId}`
        Apis.deleteHistory(para).then(() => {
          this.$router.push({
            name: 'TestCaseDetail',
            query: {
              id: this.caseId
            }
          })

          this.$q.loading.hide();
        })
      })
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
}
</style>
<style lang="scss">
.new_input {
  width: 100%;
  padding: 10px 30px;

  .row {
    margin-bottom: 10px;
  }
}
.q-textarea .q-field__native {
  resize: none;
}
@media (min-width: 600px) {
  .q-dialog__inner--minimized > div {
    max-width: 700px;
  }
}
</style>

