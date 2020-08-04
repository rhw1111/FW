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
                 color="primary"
                 style="margin-right:20px;"
                 label="比 较"
                 :disable="isNoRun!=1?false:true"
                 @click="compareLog" />
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
    <!-- 历史记录比较列表 -->
    <q-dialog v-model="HistoryCompareLogFlag"
              persistent>
      <q-card style="width: 100%; max-width: 85vw;">
        <q-card-section>
          <div class="text-h6">日志比较</div>
        </q-card-section>

        <q-separator />
        <q-table :data="HistoryCompareLogList"
                 :columns="HistoryCompareColumns"
                 row-key="id"
                 :rows-per-page-options=[0]
                 table-style="max-height: 500px"
                 no-data-label="暂无数据更新">
          <template v-slot:bottom
                    style="height:20px;">
          </template>
        </q-table>
        <q-separator />
        <q-card-actions align="right">
          <q-btn flat
                 label="关闭"
                 color="primary"
                 @click="cancelHistoryCompare" />
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
      HistoryCompareLogFlag: false,
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

      createTime: '',   //历史记录创建时间
      summary: '',       //历史记录总结 

      HistoryCompareLogList: [],//比较日志列表
      HistoryCompareColumns: [
        {
          name: 'createTime',
          required: true,
          label: '创建时间',
          align: 'left',
          field: row => row.createTime,
          format: val => `${val}`,
          sortable: true
        },
        { name: 'ConnectCount', label: '连接数', align: 'left', field: 'ConnectCount', sortable: true },
        { name: 'ConnectFailCount', label: '连接失败数', align: 'left', field: 'ConnectFailCount', sortable: true },
        { name: 'ReqCount', label: '请求数', align: 'left', field: 'ReqCount', sortable: true },
        { name: 'ReqFailCount', label: '请求失败数', align: 'left', field: 'ReqFailCount', sortable: true },
        { name: 'MaxQPS', label: '最大QPS', align: 'left', field: 'MaxQPS', sortable: true },
        { name: 'MinQPS', label: '最小QPS', align: 'left', field: 'MinQPS', sortable: true },
        { name: 'AvgQPS', label: '平均QPS', align: 'left', field: 'AvgQPS', sortable: true },
        { name: 'MaxDuration', label: '最大响应时间（微秒）', align: 'left', field: 'MaxDuration', sortable: true },
        { name: 'MinDurartion', label: '最小响应时间（微秒）', align: 'left', field: 'MinDurartion', sortable: true },
        { name: 'AvgDuration', label: '平均响应时间（微秒）', align: 'left', field: 'AvgDuration', sortable: true },
      ]
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
    //------------------------------日志比较--------------------------
    //打开比较日志
    compareLog () {
      console.log(this.HistorySelected)
      //let _this = this;
      if (this.HistorySelected.length == 0 || this.HistorySelected.length == 1) {
        this.$q.notify({
          position: 'top',
          message: '提示',
          caption: '请选择两个或两个以上的历史记录进行比较。',
          color: 'red',
        })
        return;
      }
      let idsArr = [];
      for (let i = 0; i < this.HistorySelected.length; i++) {
        idsArr.push(this.HistorySelected[i].id)
      }
      this.$q.loading.show()
      let para = {
        CaseID: this.$route.query.id,
        IDS: idsArr
      }
      Apis.postSelectedHistories(para).then((res) => {
        console.log(res)
        for (let i = 0; i < res.data.length; i++) {
          this.HistoryCompareLogList.push(JSON.parse(res.data[i].summary));
          this.HistoryCompareLogList[i].createTime = res.data[i].createTime;
        }
        this.$q.loading.hide()
        this.HistoryCompareLogFlag = true;
      })
      // getHistoryDetail(this.HistorySelected, 0)
      // function getHistoryDetail (value, num) {
      //   console.log(num)
      //   if (num != _this.HistorySelected.length) {
      //     let para = {
      //       caseId: _this.$route.query.id,
      //       historyId: value[num].id
      //     }
      //     Apis.getHistoryDetail(para).then((res) => {
      //       console.log(res)
      //       _this.HistoryCompareLogList.push(JSON.parse(res.data.summary));
      //       _this.HistoryCompareLogList[num].createTime = res.data.createTime;
      //       getHistoryDetail(_this.HistorySelected, num + 1)
      //     })
      //   } else {
      //     console.log(_this.HistoryCompareLogList)
      //     _this.$q.loading.hide()
      //     _this.HistoryCompareLogFlag = true;
      //   }
      // }
    },
    //关闭比较日志
    cancelHistoryCompare () {
      this.HistoryCompareLogFlag = false;
      this.HistoryCompareLogList = [];
    }
  }
}
</script>

<style lang="scss" scoped>
</style>