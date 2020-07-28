<template>
  <div class="col-xs-12 col-sm-5 col-xl-5">
    <!-- 历史记录列表 -->
    <q-list bordered>
      <q-table title="历史记录列表"
               :data="HistoryList"
               :columns="HistoryColumns"
               row-key="id"
               selection="multiple"
               :selected.sync="HistorySelected"
               :rows-per-page-options=[0]
               table-style="max-height: 500px"
               no-data-label="暂无数据更新">
        <template v-slot:top-right>
          <q-btn class="btn"
                 style="background: #FF0000; color: white"
                 label="删 除"
                 :disable="isNoRun!=1?false:true"
                 @click="deleteHistory" />
        </template>
        <template v-slot:body-cell-id="props">
          <q-td :props="props">
            <q-btn class="btn"
                   color="primary"
                   label="查 看"
                   :disable="isNoRun!=1?false:true"
                   @click="getHistoryDetail(props)" />
          </q-td>
        </template>
        <template v-slot:bottom>
          <q-pagination v-model="pagination.page"
                        :max="pagination.rowsNumber"
                        :input="true"
                        class="col offset-md-8"
                        @input="switchPage">
          </q-pagination>
        </template>
      </q-table>
    </q-list>
    <!-- 查看历史记录列表 -->
    <q-dialog v-model="lookHistoryDetailFlag"
              persistent>
      <q-card style="width: 100%; max-width: 60vw;">
        <q-card-section>
          <div class="text-h6">历史记录</div>
        </q-card-section>

        <q-separator />
        <div class="new_input">
          <div class="row input_row">
            <q-input v-model="createTime"
                     :dense="false"
                     class="col">
              <template v-slot:before>
                <span style="font-size:14px">创建时间:</span>
              </template>
            </q-input>
          </div>
          <div class="row input_row">
            <q-input v-model="summary"
                     :dense="false"
                     class="col"
                     type="textarea"
                     outlined>
              <template v-slot:before>
                <span style="font-size:14px">总结:</span>
              </template>
            </q-input>
          </div>
        </div>
        <q-separator />

        <q-card-actions align="right">
          <q-btn flat
                 label="关闭"
                 color="primary"
                 @click="lookHistoryDetailFlag = false" />
        </q-card-actions>
      </q-card>
    </q-dialog>
  </div>
</template>

<script>
import * as Apis from "@/api/index"
export default {
  name: 'History',
  props: ['isNoRun', 'detailData'],
  data () {
    return {
      lookHistoryDetailFlag: false,
      HistoryList: [],//历史记录列表
      HistorySelected: [],//历史记录选择
      //历史记录表格配置
      HistoryColumns: [
        {
          name: 'createTime',
          required: true,
          label: '创建时间',
          align: 'left',
          field: row => row.createTime,
          format: val => `${val}`,
        },
        { name: 'id', label: '操作', align: 'right', field: 'id', headerStyle: 'text-align:center' },
      ],
      //历史记录分页配置
      pagination: {
        page: 1,          //页码
        rowsNumber: 1     //总页数
      },

      createTime: '',
      summary: ''
    }
  },
  methods: {
    //获得历史记录列表
    getHistoryList (page) {
      let para = {
        caseId: this.$route.query.id,
        page: page || 1,
        pageSize: 50
      }
      Apis.getHistoryList(para).then((res) => {
        console.log(res)
        this.pagination.page = page || 1;
        this.pagination.rowsNumber = Math.ceil(res.data.totalCount / 50);
        this.HistoryList = res.data.results;
        this.HistorySelected = [];
        this.$q.loading.hide()
      })
    },
    //获得历史记录详情
    getHistoryDetail (value) {
      this.$q.loading.show()
      let para = {
        caseId: this.$route.query.id,
        historyId: value.row.id
      }
      Apis.getHistoryDetail(para).then((res) => {
        console.log(res)
        this.createTime = res.data.createTime;
        this.summary = JSON.stringify(JSON.parse(res.data.summary), null, 2);
        this.$q.loading.hide();
        this.lookHistoryDetailFlag = true;
      })
    },
    //删除历史记录
    deleteHistory () {
      if (this.HistorySelected.length == 0) {
        this.$q.notify({
          position: 'top',
          message: '提示',
          caption: '请选择历史记录',
          color: 'red',
        })
        return;
      }
      this.$q.dialog({
        title: '提示',
        message: '您确定要删除当前选择的历史记录吗',
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
        if (this.HistorySelected.length == 1) {
          // 单个删除slaveHost列表
          let para = `?caseId=${this.detailData.id}&historyId=${this.HistorySelected[0].id}`
          Apis.deleteHistory(para).then(() => {
            this.HistorySelected = [];
            this.getHistoryList();
          })
        } else if (this.HistorySelected.length > 1) {
          // 批量删除slaveHost列表
          let delIdArr = [];
          for (let i = 0; i < this.HistorySelected.length; i++) {
            delIdArr.push(this.HistorySelected[i].id)
          }
          console.log(delIdArr)
          let para = {
            CaseID: this.detailData.id,
            IDS: delIdArr
          }
          Apis.deleteHistoryArr(para).then(() => {
            this.HistorySelected = [];
            this.getHistoryList();
          })
        }
      })
    },
    //切换历史记录页码
    switchPage (value) {
      this.$q.loading.show()
      this.getHistoryList(value)
    },
    //跳转到历史记录详情
    toHistoryDetail (evt) {
      this.$router.push({
        name: 'HistoryDetail',
        query: {
          historyId: evt.row.id,
          caseId: evt.row.caseID
        }
      })
    },
  }
}
</script>

<style lang="scss" scoped>
</style>