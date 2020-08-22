<template>
  <div class="TestCase">
    <!-- TestCase列表 -->
    <div class="q-pa-md">

      <q-table title="测试用例列表"
               :data="TestCaseList"
               :columns="columns"
               row-key="id"
               selection="multiple"
               :selected.sync="selected"
               :rows-per-page-options=[0]
               table-style="max-height: 500px"
               no-data-label="暂无数据更新">

        <template v-slot:top-right>
          <q-btn class="btn"
                 color="primary"
                 label="运 行"
                 @click="runTestCase" />
          <q-btn class="btn"
                 color="primary"
                 label="新 增"
                 @click="openCrateTestCase" />
        </template>
        <template v-slot:bottom
                  class="row">
          <q-pagination v-model="pagination.page"
                        :max="pagination.rowsNumber"
                        :input="true"
                        class="col offset-md-10"
                        @input="nextPage">
          </q-pagination>
        </template>
        <template v-slot:body-cell-id="props">
          <q-td class=""
                :props="props">
            <q-btn class="btn"
                   color="primary"
                   v-show="props.row.status=='正在运行'"
                   label="查看主机日志"
                   @click="lookMasterLog(props)" />
            <q-btn class="btn"
                   color="primary"
                   label="更 新"
                   @click="toDetail(props)" />
            <q-btn class="btn"
                   style="background: #FF0000; color: white"
                   label="删 除"
                   @click="deleteTestCase(props)" />
          </q-td>
        </template>
      </q-table>

      <!-- <q-splitter v-model="splitterModel"
                  style="height: 400px">

        <template v-slot:before>
          <div class="q-pa-md">
            <q-tree :nodes="simple"
                    node-key="label"
                    selected-color="primary"
                    :selected.sync="selectedf"
                    default-expand-all
                    @update:selected="getNodeByKey" />
          </div>
        </template>

        <template v-slot:after>
          <q-tab-panels v-model="selectedf"
                        animated
                        transition-prev="jump-up"
                        transition-next="jump-up">
            <q-tab-panel name="Relax Hotel">
              <div class="text-h4 q-mb-md">Welcome</div>
              <p>Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quis praesentium cumque magnam odio iure quidem, quod illum numquam possimus obcaecati commodi minima assumenda consectetur culpa fuga nulla ullam. In, libero.</p>
              <p>Lorem ipsum dolor sit, amet consectetur adipisicing elit. Quis praesentium cumque magnam odio iure quidem, quod illum numquam possimus obcaecati commodi minima assumenda consectetur culpa fuga nulla ullam. In, libero.</p>
            </q-tab-panel>
          </q-tab-panels>
        </template>
      </q-splitter> -->

    </div>

    <!-- 新增TestCase框 -->
    <q-dialog v-model="createFixed"
              persistent>
      <q-card style="width: 100%; max-width: 70vw;">
        <q-card-section>
          <div class="text-h6">创建测试用例</div>
        </q-card-section>

        <q-separator />

        <CreateShowTestCase :masterHostList="masterHostList"
                            :dataSourceName="dataSourceName"
                            ref="CSTestCase" />

        <q-separator />

        <q-card-actions align="right">
          <q-btn flat
                 label="取消"
                 color="primary"
                 @click="newCancel" />
          <q-btn flat
                 label="创建"
                 color="primary"
                 @click="newCreate" />
        </q-card-actions>
      </q-card>
    </q-dialog>

  </div>
</template>

<script>
import * as Apis from "@/api/index"
import CreateShowTestCase from './component/CreateShowTestCase.vue'
export default {
  name: 'TestCase',
  components: {
    CreateShowTestCase
  },
  data () {
    return {
      createFixed: false,   //新建flag
      TestCaseList: [],     //TeseCase列表
      masterHostList: [],//主机列表
      dataSourceName: [],//数据源名称列表

      selected: [],   //表格选择
      //表格配置
      columns: [
        {
          name: 'name',
          required: true,
          label: '名称',
          align: 'left',
          field: row => row.name,
          format: val => `${val}`,
        },
        { name: 'engineType', align: 'left', label: '引擎类型', field: 'engineType', style: 'max-width: 50px', headerStyle: 'max-width: 50px' },
        { name: 'configuration', label: '配置', align: 'left', field: 'configuration', style: 'max-width: 250px', headerStyle: 'max-width: 250px' },
        { name: 'status', label: '状态', align: 'left', field: 'status', },
        { name: 'id', label: '操作', align: 'right', field: 'id', headerStyle: 'text-align:center' },
      ],
      //分页配置
      pagination: {
        page: 1,          //页码
        rowsNumber: 1     //总页数
      },
      dismiss: null,




      splitterModel: 15,
      selectedf: '',
      simple: [
        {
          label: '测试用例',
          children: [
          ]
        }
      ]



    }
  },
  mounted () {
    this.getTestCaseList();
  },
  computed: {

  },
  methods: {
    //新增
    openCrateTestCase () {
      this.createFixed = true;
    },
    //获得主机列表
    getMasterHostList () {
      Apis.getMasterHostList().then((res) => {
        console.log(res)
        this.masterHostList = res.data;
        this.$q.loading.hide()
      })
    },
    //获得TeseCase列表
    getTestCaseList (page) {
      this.$q.loading.show()
      let para = {
        matchName: '',
        //pageSize: 50,
        page: page || 1
      }
      Apis.getTestCaseList(para).then((res) => {
        console.log(res)
        this.TestCaseList = res.data.results;
        this.pagination.page = page || 1;
        this.pagination.rowsNumber = Math.ceil(res.data.totalCount / 50);
        //this.getMasterHostList();
        this.getDataSourceName();

        for (let i = 0; i < res.data.results.length; i++) {
          //this.simple[0].children.push(res.data.results[i])
          this.simple[0].children.push({ label: res.data.results[i].name })
        }
        console.log(this.simple)
      })
    },
    //获得数据源名称
    getDataSourceName () {
      let para = {}
      Apis.getDataSourceName(para).then((res) => {
        console.log(res)
        this.dataSourceName = res.data;
        this.$q.loading.hide()
      })
    },
    //列表下一页
    nextPage (value) {
      this.getTestCaseList(value)
    },
    //删除Testcase
    deleteTestCase (value) {
      console.log(value)
      this.$q.dialog({
        title: '提示',
        message: '您确定要删除当前的测试用例吗',
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
        let para = `?id=${value.row.id}`
        Apis.deleteTestCase(para).then((res) => {
          console.log(res)
          this.getTestCaseList();
        })
      }).onCancel(() => {
      })
    },
    //新增弹窗取消按钮
    newCancel () {
      this.$refs.CSTestCase.newCancel();
      this.createFixed = false;
    },
    //新增弹窗创建按钮
    newCreate () {
      if (!this.$refs.CSTestCase.newCreate()) { return; }
      let para = this.$refs.CSTestCase.newCreate();
      this.$q.loading.show()
      Apis.postCreateTestCase(para).then((res) => {
        console.log(res)
        this.getTestCaseList();
        this.$q.notify({
          position: 'top',
          message: '提示',
          caption: '创建成功',
          color: 'secondary',
        })
        this.newCancel()
      })
    },
    //跳转TestCase详情
    toDetail (evt) {
      this.$router.push({
        name: 'TestCaseDetail',
        query: {
          id: evt.row.id
        },
      })
    },
    //查看主机日志
    lookMasterLog (value) {
      this.$q.loading.show()
      Apis.getMasterLog({ caseId: value.row.id }).then((res) => {
        this.$q.loading.hide()
        this.$q.dialog({
          title: '提示',
          message: res.data,
          style: { 'width': '100%', 'max-width': '65vw', "white-space": "pre-line", "overflow-x": "hidden", "word-break": "break-all" }
        })
      })
    },
    //运行TestCase
    runTestCase () {
      if (this.selected.length != 0) {
        let runArray = [];
        for (let i = 0; i < this.selected.length; i++) {
          if (this.selected[i].status == '正在运行') {
            runArray.push(this.selected[i].name)
          }
        }
        if (runArray.length != 0) {
          this.$q.notify({
            position: 'top',
            message: '提示',
            caption: `当前测试用例${runArray.join('，')}正在运行当中,请重新选择`,
            color: 'red',
          })
          return;
        }
        this.$q.loading.show()
        for (let i = 0; i < this.selected.length; i++) {
          this.getSlaveHostsList(this.selected[i].id, (flag) => {
            if (flag) { runArray.push(this.selected[i].name) }
            if (i == this.selected.length - 1) {
              if (runArray.length != 0) {
                this.$q.notify({
                  position: 'top',
                  message: '提示',
                  caption: `当前测试用例${runArray.join('，')}下没有从主机，请添加从主机再进行运行。`,
                  color: 'red',
                })
                this.$q.loading.hide()
              } else {
                this.run(0);
              }
            }
          })
        }

      } else {
        this.$q.notify({
          position: 'top',
          message: '提示',
          caption: '请选择测试用例',
          color: 'red',
        })
      }
    },
    //运行
    run (index) {
      console.log(index)
      if (this.dismiss) {
        this.dismiss();
      }
      let runNum = index;
      this.$q.loading.show()
      let para = `?caseId=${this.selected[runNum].id}`
      Apis.postTestCaseRun(para).then((res) => {
        console.log(res)
        this.dismiss = this.$q.notify({
          position: 'top-right',
          caption: `当前测试用例${this.selected[runNum].name}正在运行当中。${runNum + 1}/${this.selected.length}`,
          color: 'teal',
          timeout: '0'
        })
        this.timerOut = window.setInterval(() => {
          setTimeout(this.getTestCaseStatus(runNum), 0);
        }, 3000);
      })

    },
    //查看TestCase是否运行
    getTestCaseStatus (index) {
      Apis.getTestCaseStatus({ caseId: this.selected[index].id }).then((res) => {
        if (!res.data) {
          if (index == this.selected.length - 1) {
            clearInterval(this.timerOut);
            this.timerOut = null;
            this.dismiss();
            this.getMasterHostList();
            this.selected = [];
            return;
          }
          clearInterval(this.timerOut);
          this.timerOut = null;
          setTimeout(() => {
            this.run(index + 1)
          }, 120000)
        }
      })
    },
    //获得从机列表
    getSlaveHostsList (id, callback) {
      Apis.getSlaveHostsList({ caseId: id }).then((res) => {
        if (res.data.length == 0) {
          callback(true)
        } else {
          callback(false)
        }
      })
    },
    // ---------------------------- 列表新样式 ----------------------------------
    getNodeByKey (key) {
      console.log(key)
    }
  }
}
</script>

<style lang="scss" scoped>
.TestCase {
  width: 100%;
  overflow: hidden;
  .btn {
    margin-right: 10px;
  }
  .q-pa-md {
    margin-top: 40px;
  }
}
</style>
<style lang="scss">
.q-table {
  //width: 100%;
  //table-layout: fixed;
  .text-left {
    white-space: nowrap;
    overflow: hidden;
    text-overflow: ellipsis;
  }
}
.q-table--col-auto-width {
  width: 75px;
}
.new_input {
  width: 100%;
  padding: 10px 30px;
  .input_row {
    margin-bottom: 30px;
  }
}
</style>