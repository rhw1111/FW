<template>
  <div class="detail">
    <!-- button -->
    <div class="detail_header">
      <q-btn class="btn"
             color="primary"
             label="返 回 目 录"
             v-if="$route.name=='DirectoryTestCaseDetail'"
             @click="returnDirectory" />
      <q-btn class="btn"
             color="primary"
             label="保 存"
             :disable="isNoRun!=1?false:true"
             @click="putTestCase" />
      <q-btn class="btn"
             style="background: #FF0000; color: white"
             label="删 除"
             :disable="isNoRun!=1?false:true"
             @click="deleteTestCase" />
      <q-btn class="btn"
             color="primary"
             label="运 行"
             :disable="isNoRun!=1?false:true"
             @click="run" />
      <q-btn class="btn"
             color="primary"
             label="停 止"
             :disable='isNoRun==1?false:true'
             @click="stop" />
      <q-btn class="btn"
             color="primary"
             label="查 看 状 态"
             @click="lookStatus" />
      <q-btn class="btn"
             color="primary"
             label="查 看 主 机 日 志"
             @click="lookMasterLog" />
      <q-btn class="btn"
             color="primary"
             label="性 能 监 测"
             @click="lookMonitorUrl" />
      <q-btn class="btn"
             color="primary"
             label="复 制"
             @click="CopyTestCase" />
    </div>
    <!-- 新建测试用例 -->
    <div class="q-pa-md">
      <CreateShowTestCase :masterHostList="masterHostList"
                          ref="CSTestCase"
                          :detailData="detailData" />
    </div>
    <!-- 复制创建TestCase -->
    <q-dialog v-model="CopyTestCaseFixed"
              persistent>
      <q-card style="width: 100%; max-width: 60vw;">
        <q-card-section>
          <div class="text-h6">创建测试用例</div>
        </q-card-section>

        <q-separator />
        <div class="new_input">
          <div class="row input_row">
            <q-input v-model="CopyTestCaseName"
                     :dense="false"
                     class="col">
              <template v-slot:before>
                <span style="font-size:14px">测试用例名称:</span>
              </template>
            </q-input>
            <q-select v-model="CopyTestCaseSlaveFlag"
                      :options="['是','否']"
                      class="col"
                      :dense="false">
              <template v-slot:before>
                <span style="font-size:14px">是否复制从主机:</span>
              </template>
              <template v-slot:prepend>
              </template>
            </q-select>
          </div>
        </div>
        <q-separator />

        <q-card-actions align="right">
          <q-btn flat
                 label="取消"
                 color="primary"
                 @click="CopyTestCaseCancel" />
          <q-btn flat
                 label="创建"
                 color="primary"
                 @click="CopyTestCaseCreate" />
        </q-card-actions>
      </q-card>
    </q-dialog>
    <!-- 从机和历史记录列表 -->
    <div class="q-pa-md row HostList">
      <!-- 从主机列表 -->
      <SlaveHost :isNoRun="isNoRun"
                 :detailData="detailData"
                 ref="TestCaseSlaveHost" />
      <!-- 历史记录列表 -->
      <History :isNoRun="isNoRun"
               :detailData="detailData"
               ref="TestCaseHistory" />
    </div>
    <!-- 主机日志提示 -->
    <q-dialog v-model="lookMasterLogFlag">
      <q-card class="q-dialog-plugin full-height"
              style="width: 100%; max-width: 80vw;height:800px;overflow:hidden;">
        <q-card-section class="row">
          <div class="text-h6 col-11">主机日志</div>
          <q-btn color="primary"
                 label="关 闭"
                 class="col-1"
                 @click="lookMasterLogFlag = false" />
        </q-card-section>

        <q-separator />
        <q-card-section style="height:85%;overflow:hidden scroll; white-space: pre-line; word-break: break-all;">
          {{lookMasterLogText}}
        </q-card-section>
        <q-separator />
      </q-card>
    </q-dialog>
  </div>
</template>

<script>
import * as Apis from "@/api/index"
import CreateShowTestCase from './component/CreateShowTestCase.vue'
import History from "./History.vue"
import SlaveHost from "./SlaveHost"
export default {
  name: 'TestCaseDetail',
  components: {
    CreateShowTestCase,
    History,
    SlaveHost
  },
  data () {
    return {
      isNoRun: 0,//判断是否在运行
      timerOut: null, //查看当前测试用例是否运行的定时器
      detailData: '',//详情数据

      masterHostList: [],//主机列表

      //------------------------- 复制 --------------------------
      CopyTestCaseFixed: false,//复制创建TestCaseFlag
      CopyTestCaseName: '',//复制创建TestCase名称
      CopyTestCaseSlaveFlag: '否',//复制创建是否复制从主机


      //------------------------- 主机 --------------------------
      lookMasterLogFlag: false,//主机日志Flag
      lookMasterLogText: ''//主机日志内容

    }
  },
  mounted () {
    this.getTestCaseDetail();
  },
  beforeDestroy () {
    //清除定时器
    clearInterval(this.timerOut);
    this.timerOut = null;
  },
  methods: {
    //获得TestCase详情
    getTestCaseDetail () {
      this.$q.loading.show()
      Apis.getTestCaseDetail({ id: this.$route.query.id }).then((res) => {
        console.log(res)
        this.detailData = res.data;
        this.$refs.TestCaseSlaveHost.getSlaveHostsList();
        this.$refs.TestCaseHistory.getHistoryList();
        //判断当前测试用例是否在运行当中（运行中：循环查看测试用例是否运行，运行完毕停止。）
        if (res.data.status == 1) {
          this.timerOut = window.setInterval(() => {
            setTimeout(this.getTestCaseStatus(), 0);
          }, 3000);
        } else {
          Apis.getTestCaseStatus({ caseId: this.$route.query.id }).then((res) => {
            this.isNoRun = res.data;
            if (!res.data) {
              clearInterval(this.timerOut);
              this.timerOut = null;
            }
          })
        }
      })
    },
    //查看TestCase是否运行
    getTestCaseStatus () {
      Apis.getTestCaseStatus({ caseId: this.$route.query.id }).then((res) => {
        this.isNoRun = res.data;
        if (!res.data) {
          clearInterval(this.timerOut);
          this.timerOut = null;
          this.getTestCaseDetail();
        }
      })
    },
    //保存更新测试用例
    putTestCase () {
      if (!this.$refs.CSTestCase.newCreate()) { return; }
      let para = this.$refs.CSTestCase.newCreate();
      para.ID = this.detailData.id;
      this.$q.loading.show()
      Apis.putTestCase(para).then((res) => {
        console.log(res)
        this.$q.notify({
          position: 'top',
          message: '提示',
          caption: '保存成功',
          color: 'secondary',
        })
        this.getTestCaseDetail();
      })
    },
    //删除当条测试用例
    deleteTestCase () {
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
        //判断当前的测试用例是否存在目录管理里面，执行不同的删除方法
        if (this.detailData.treeID == null) {
          let para = `?id=${this.detailData.id}`
          Apis.deleteTestCase(para).then((res) => {
            console.log(res)
            this.$router.push({ name: 'TestCase' })
          })
        } else {
          let para = `?id=${this.detailData.treeID}`
          Apis.deleteTreeEntity(para).then((res) => {
            console.log(res)
            this.$router.push({ name: 'TestCase' })
          })
        }
      })
    },
    //-------------------------------------------- 运行停止测试用例 --------------------------------------
    //运行测试用例
    run () {
      this.$q.loading.show()
      let para = `?caseId=${this.$route.query.id}`
      Apis.postTestCaseRun(para).then((res) => {
        console.log(res)
        this.getTestCaseDetail()
        this.$q.notify({
          position: 'top',
          message: '提示',
          caption: '运行成功',
          color: 'secondary',
        })
      })
    },
    //停止测试用例
    stop () {
      this.$q.loading.show()
      let para = `?caseId=${this.$route.query.id}`
      Apis.postTestCaseStop(para).then((res) => {
        console.log(res)
        this.getTestCaseDetail()
        this.$q.notify({
          position: 'top',
          message: '提示',
          caption: '停止成功',
          color: 'secondary',
        })
      })
    },
    //-------------------------------------------- 查看当前测试用例状态和日志 --------------------------------------
    //查看状态
    lookStatus () {
      this.$q.loading.show()
      Apis.getCheckStatus({ caseId: this.$route.query.id }).then((res) => {
        this.$q.dialog({
          title: '提示',
          message: res.data ? '当前测试用例正在运行' : '当前测试用例为停止状态'
        })
        this.$q.loading.hide()
      })
    },
    //查看master日志
    lookMasterLog () {
      this.$q.loading.show()
      Apis.getMasterLog({ caseId: this.$route.query.id }).then((res) => {
        this.$q.loading.hide()
        this.lookMasterLogFlag = true;
        this.lookMasterLogText = res.data;
      })
    },
    //查看MonitorUrl
    lookMonitorUrl () {
      window.open(this.detailData.monitorUrl);
    },
    //-------------------------------------------- 复制 --------------------------------------
    //复制创建TestCase打开
    CopyTestCase () {
      this.CopyTestCaseFixed = true;
      this.CopyTestCaseName = this.detailData.name + '_1';
    },
    //复制创建TestCase取消
    CopyTestCaseCancel () {
      this.CopyTestCaseFixed = false;
      this.CopyTestCaseName = '';
      this.CopyTestCaseSlaveFlag = '否'
    },
    //复制创建TestCase创建
    CopyTestCaseCreate () {
      let _this = this;
      this.$q.loading.show()
      let para = {
        Name: this.CopyTestCaseName,
        Configuration: this.detailData.configuration,
        EngineType: this.detailData.engineType,
        MasterHostID: this.detailData.masterHostID
      }
      Apis.postCreateTestCase(para).then((res) => {
        console.log(res)
        //是否复制从主机
        if (this.CopyTestCaseSlaveFlag == '是') {
          if (this.SlaveHostList.length != 0) {
            //复制从主机
            CopyCreateSlaveHost(res, 0)
          } else {
            this.$q.notify({
              position: 'top',
              message: '提示',
              caption: '创建成功',
              color: 'secondary',
            })
            this.CopyTestCaseFixed = false;
            this.$q.loading.hide()
            this.$router.push({
              name: 'TestCaseDetail',
              query: {
                id: res.data.id
              },
            });
          }
        } else {
          //跳转到刚刚复制的页面
          this.$q.notify({
            position: 'top',
            message: '提示',
            caption: '创建成功',
            color: 'secondary',
          })
          this.CopyTestCaseFixed = false;
          this.$q.loading.hide()
          this.$router.push({
            name: 'TestCaseDetail',
            query: {
              id: res.data.id
            },
          });
        }
        this.getTestCaseDetail();
      })

      //复制从主机
      function CopyCreateSlaveHost (res, SalveHostNum) {
        console.log(SalveHostNum, _this.SlaveHostList.length)
        if (SalveHostNum == _this.SlaveHostList.length) {
          _this.$q.notify({
            position: 'top',
            message: '提示',
            caption: '创建成功',
            color: 'secondary',
          })
          _this.CopyTestCaseFixed = false;
          _this.$q.loading.hide()
          _this.$router.push({
            name: 'TestCaseDetail',
            query: {
              id: res.data.id
            },
          });
          _this.getTestCaseDetail();
        } else {
          console.log(_this.SlaveHostList, SalveHostNum, _this.SlaveHostList[SalveHostNum], _this.SlaveHostList[SalveHostNum].HostID)
          let para = {
            "HostID": _this.SlaveHostList[SalveHostNum].hostID,
            "TestCaseID": res.data.id,
            "SlaveName": _this.SlaveHostList[SalveHostNum].slaveName,
            "Count": _this.SlaveHostList[SalveHostNum].count,
            "ExtensionInfo": _this.SlaveHostList[SalveHostNum].extensionInfo
          }
          Apis.postCreateSlaveHost(para).then(() => {
            CopyCreateSlaveHost(res, SalveHostNum + 1)
          })
        }
      }


    },
    // -------------------- 目录管理 ----------------------
    //返回目录管理页面
    returnDirectory () {
      this.$router.push({
        path: '/Directory'
      })
    }
  }
}
</script>

<style lang="scss" scoped>
.detail {
  position: relative;
  width: 100%;
  overflow: hidden;
  .mask {
    position: absolute;
    left: 0;
    top: 60px;
    width: 100%;
    height: 100%;
    z-index: 100;
  }
  .detail_header {
    padding: 10px 16px 5px;
    width: 100%;
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
<style lang="scss">
.q-table {
  .cursor-pointer {
    .text-left {
      white-space: nowrap;
      overflow: hidden;
      text-overflow: ellipsis;
    }
    .q-table--col-auto-width {
      width: 75px;
    }
  }
}
.q-table--col-auto-width {
  width: 75px;
}
.new_input {
  width: 100%;
  padding: 10px 30px;

  .input_row {
    margin-bottom: 20px;
  }
}
.q-textarea .q-field__native {
  height: 400px;
  resize: none;
}
</style>