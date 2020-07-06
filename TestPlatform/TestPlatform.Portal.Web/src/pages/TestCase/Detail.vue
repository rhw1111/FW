<template>
  <div class="detail">
    <!-- 选择主机框 -->
    <lookUp :masterHostList="masterHostList"
            :masterSelectIndex='masterSelectIndex'
            :fixed="HostFixed"
            @addMasterHost='addMasterHost'
            @cancelMasterHost='cancelMasterHost'
            ref='lookUp' />
    <!-- SlaveHost选择Host -->
    <lookUp :masterHostList="masterHostList"
            :masterSelectIndex='SlaveHostHostIndex'
            :fixed="SlaveHostFixed"
            @addMasterHost='addSlaveHostHost'
            @cancelMasterHost='cancelSlaveHostHost'
            ref='SlaveHostHostlookUp' />
    <!-- 
             -->
    <!-- EngineType选择框 -->
    <EngineTypeLookUp :fixed="EngineTypeFixed"
                      :EngineTypeIndex="EngineTypeSelect"
                      @cancelEngineType="cancelEngineType"
                      @addEngineType="addEngineType"
                      ref='TypelookUp' />
    <!-- button -->
    <div class="detail_header">
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
             label="查 看 从 主 机 日 志"
             @click="lookSlaveLog" />
      <q-btn class="btn"
             color="primary"
             label="查 看 监 测 地 址"
             @click="lookMonitorUrl" />
    </div>
    <!-- TestCase字段 -->
    <div class="q-pa-md row">

      <div class="new_input">
        <div class="row">
          <q-input v-model="Name"
                   :dense="false"
                   class="col">
            <template v-slot:before>
              <span style="font-size:14px">名称:</span>
            </template>
          </q-input>
          <q-input v-model="EngineType"
                   :dense="false"
                   class="col"
                   readonly
                   style="margin-left:50px;"
                   @dblclick="openEngineType">
            <template v-slot:before>
              <span style="font-size:14px">引擎类型:</span>
            </template>
          </q-input>

          <q-input :dense="false"
                   class="col"
                   readonly
                   v-model="masterHostSelect"
                   @dblclick="masterHost">
            <template v-slot:before>
              <span style="font-size:14px">主机:</span>
            </template>
          </q-input>
        </div>
        <div class="row">
          <q-input v-model="Configuration"
                   :dense="false"
                   class="col-xs-12"
                   type="textarea"
                   outlined>
            <template v-slot:before>
              <span style="font-size:14px">配置:</span>
            </template>
          </q-input>
        </div>
      </div>
    </div>
    <!-- 创建从机 -->
    <q-dialog v-model="createFixed"
              persistent>
      <q-card style="width:100%">
        <q-card-section>
          <div class="text-h6">创建从主机</div>
        </q-card-section>

        <q-separator />
        <div class="new_input">
          <div class="row">
            <q-input v-model="SlaveHostName"
                     :dense="false"
                     class="col">
              <template v-slot:before>
                <span style="font-size:14px">名称:</span>
              </template>
            </q-input>
            <q-input v-model="SlaveCount"
                     :dense="false"
                     class="col"
                     @keyup="SlaveCount=SlaveCount.replace(/[^\d]/g,'')">
              <template v-slot:before>
                <span style="font-size:14px">数量:</span>
              </template>
            </q-input>
            <q-input :dense="false"
                     class="col"
                     readonly
                     v-model="SlaveHostHostSelect"
                     @dblclick="dblSlaveHostHost">
              <template v-slot:before>
                <span style="font-size:14px">主机:</span>
              </template>
            </q-input>
          </div>
          <div class="row">
            <q-input v-model="SlaveExtensionInfo"
                     :dense="false"
                     class="col"
                     type="textarea"
                     outlined>
              <template v-slot:before>
                <span style="font-size:14px">扩展信息:</span>
              </template>
            </q-input>
          </div>
        </div>
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
    <!-- 从机和历史记录列表 -->
    <div class="q-pa-md row HostList">
      <!-- 从机列表 -->
      <q-list bordered
              class="col-xs-12 col-sm-6 col-xl-6">
        <q-table title="从主机列表"
                 :data="SlaveHostList"
                 :columns="columns"
                 row-key="id"
                 selection="multiple"
                 :selected.sync="SlaveHostSelected"
                 @row-dblclick="toSlaveHostDetail"
                 :rows-per-page-options=[0]
                 no-data-label="暂无数据更新">
          <template v-slot:top-right>
            <q-btn class="
                 btn"
                   color="primary"
                   label="新 增"
                   :disable="isNoRun!=1?false:true"
                   @click="openSlaveHost" />
            <q-btn class="btn"
                   style="background: #FF0000; color: white"
                   label="删 除"
                   :disable="isNoRun!=1?false:true"
                   @click="deleteSlaveHost" />
          </template>
          <template v-slot:bottom>
          </template>
        </q-table>
      </q-list>
      <!-- 历史记录列表 -->
      <q-list bordered
              class="col-xs-12 col-sm-6 col-xl-6">
        <q-table title="历史记录列表"
                 :data="HistoryList"
                 :columns="HistoryColumns"
                 row-key="id"
                 selection="multiple"
                 :selected.sync="HistorySelected"
                 @row-dblclick="toHistoryDetail"
                 :rows-per-page-options=[0]
                 no-data-label="暂无数据更新">
          <template v-slot:top-right>
            <q-btn class="btn"
                   style="background: #FF0000; color: white"
                   label="删 除"
                   :disable="isNoRun!=1?false:true"
                   @click="deleteHistory" />
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
    </div>
  </div>
</template>

<script>
import * as Apis from "@/api/index"
import lookUp from "@/components/lookUp.vue"
import EngineTypeLookUp from "@/components/EngineTypeLookUp.vue"
export default {
  name: 'TestCaseDetail',
  components: {
    lookUp,
    EngineTypeLookUp
  },
  data () {
    return {
      createFixed: false,//createslave Flag
      isNoRun: 0,//判断是否在运行
      timerOut: null, //定时器
      detailData: '',//详情数据
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
      HostFixed: false,   //主机lookUp Flag
      masterHostList: [],//主机列表
      masterHostSelect: '',//主机已选择列表
      masterSelectIndex: '',  //主机已选择下标


      EngineTypeFixed: false,//EngineTypeFlag
      EngineTypeSelect: -1,//EngineType选择

      Name: '',           //Name
      Configuration: '',  //Configuration
      EngineType: '',     //EngineType
      MasterHostID: '',   //MasterHostID 主机ID


      SlaveHostFixed: false,//从机Host LookUP Flag

      SlaveHostName: '',   //从机名称
      SlaveCount: '',      //从机数量
      SlaveExtensionInfo: '',   //从机拓展信息


      SlaveHostHostId: '',   //SlaveHostHost ID
      SlaveHostHostSelect: '',//主机已选择列表
      SlaveHostHostIndex: -1,  //主机已选择下标

      SlaveHostList: [],       //从机列表
      SlaveHostSelected: [],   //从机表格选择
      //从机表格配置
      columns: [
        {
          name: 'slaveName',
          required: true,
          label: '名称',
          align: 'left',
          field: row => row.slaveName,
          format: val => `${val}`,
        },
        { name: 'count', align: 'left', label: '数量', field: 'count', },
        { name: 'extensionInfo', label: '扩展信息', align: 'left', field: 'extensionInfo', style: 'width:100px;' },
      ],


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
      ],
      //历史记录分页配置
      pagination: {
        page: 1,          //页码
        rowsNumber: 1     //总页数
      },
    }
  },
  mounted () {
    this.getTestCaseDetail();
    this.timerOut = window.setInterval(() => {
      setTimeout(this.getTestCaseStatus(), 0);
    }, 3000);
  },
  beforeDestroy () {
    clearInterval(this.timerOut);
    this.timerOut = null;
  },
  methods: {
    //获得TestCase详情
    getTestCaseDetail () {
      this.$q.loading.show()
      Apis.getTestCaseDetail({ id: this.$route.query.id }).then((res) => {
        console.log(res)
        this.masterHostSelect = res.data.masterHostAddress;
        this.MasterHostID = res.data.masterHostID;
        this.detailData = res.data;
        this.Name = res.data.name;
        this.Configuration = res.data.configuration;
        this.EngineType = res.data.engineType;
        this.EngineTypeSelect = this.EngineType == 'Tcp' ? 1 : 0;
        this.getMasterHostList();
        this.getSlaveHostsList();
        this.getHistoryList();
        this.getTestCaseStatus();
      })
    },
    //获得从机列表
    getSlaveHostsList () {
      Apis.getSlaveHostsList({ caseId: this.$route.query.id }).then((res) => {
        console.log(res)
        this.SlaveHostList = res.data;
        this.SlaveHostSelected = [];
        this.$q.loading.hide()
      })
    },
    //获得主机列表
    getMasterHostList () {
      Apis.getMasterHostList().then((res) => {
        console.log(res)
        this.masterHostList = res.data;
        for (let i = 0; i < res.data.length; i++) {
          if (res.data[i].id == this.detailData.masterHostID) {
            this.masterSelectIndex = i;
            break;
          }
        }
        this.$q.loading.hide()
      })
    },
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
    //查看TestCase是否运行
    getTestCaseStatus () {
      Apis.getTestCaseStatus({ caseId: this.$route.query.id }).then((res) => {
        this.isNoRun = res.data;
      })
    },


    //双击主机
    masterHost () {
      this.HostFixed = true;
    },
    //添加主机
    addMasterHost (value) {
      if (value == undefined) {
        return false;
      }
      this.masterHostSelect = this.masterHostList[value].address;
      this.MasterHostID = this.masterHostList[value].id;
      this.masterSelectIndex = value;
      this.HostFixed = false;
    },
    //取消主机选择
    cancelMasterHost () {
      this.HostFixed = false;
      this.$refs.lookUp.selectIndex = this.masterSelectIndex;
    },

    // 打开EngineType框
    openEngineType () {
      this.EngineTypeFixed = true;
    },
    //添加EngineType
    addEngineType (value, index) {
      if (value == undefined) {
        return false;
      }
      console.log(value, index)
      this.EngineType = value[index];
      this.EngineTypeSelect = index;
      this.EngineTypeFixed = false;
    },
    //取消EngineType框
    cancelEngineType () {
      this.EngineTypeFixed = false;
      this.$refs.TypelookUp.selectIndex = this.EngineTypeSelect;
    },


    //双击从机host
    dblSlaveHostHost () {
      this.createFixed = false;
      this.SlaveHostFixed = true;
    },
    //添加从机Host
    addSlaveHostHost (value) {
      if (value == undefined) {
        return false;
      }
      this.SlaveHostHostSelect = this.masterHostList[value].address;
      this.SlaveHostHostId = this.masterHostList[value].id;
      this.SlaveHostHostIndex = value;
      this.SlaveHostFixed = false;
      this.createFixed = true;
      console.log(this.SlaveHostHostSelect, this.SlaveHostHostId, this.SlaveHostHostIndex)
    },
    //取消添加从机Host
    cancelSlaveHostHost () {
      this.SlaveHostFixed = false;
      this.createFixed = true;
      this.$refs.SlaveHostHostlookUp.selectIndex = this.SlaveHostHostIndex;
    },


    //保存更新TestCase
    putTestCase () {
      let para = {
        ID: this.detailData.id,
        Name: this.Name,
        Configuration: this.Configuration,
        EngineType: this.EngineType,
        MasterHostID: this.MasterHostID
      }
      if (this.detailData.id && this.Name && this.Configuration && this.EngineType && this.MasterHostID) {
        this.$q.loading.show()
        Apis.putTestCase(para).then((res) => {
          console.log(res)
          this.$q.notify({
            position: 'top',
            message: '提示',
            caption: '保存成功',
            color: 'secondary',
          })
          this.$q.loading.hide()
        })
      } else {
        this.$q.notify({
          position: 'top',
          message: '提示',
          caption: '请填写完整信息',
          color: 'red',
        })
      }
    },
    //删除当条TestCase
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
        let para = `?id=${this.detailData.id}`
        Apis.deleteTestCase(para).then((res) => {
          console.log(res)
          this.$router.push({ name: 'TestCase' })
        })
      }).onCancel(() => {
      })
    },

    //打开从机
    openSlaveHost () {
      this.createFixed = true;
    },
    //新建从机
    newCreate () {
      let para = {
        "HostID": this.SlaveHostHostId,
        "TestCaseID": this.detailData.id,
        "SlaveName": this.SlaveHostName,
        "Count": Number(this.SlaveCount),
        "ExtensionInfo": this.SlaveExtensionInfo
      }
      if (this.detailData.id && this.SlaveHostName && this.SlaveCount && this.SlaveExtensionInfo && this.SlaveHostHostId) {
        this.$q.loading.show()
        Apis.postCreateSlaveHost(para).then((res) => {
          console.log(res)
          this.$q.notify({
            position: 'top',
            message: '提示',
            caption: '创建成功',
            color: 'secondary',
          })
          this.newCancel();
          this.getSlaveHostsList();
        })
      } else {
        this.$q.notify({
          position: 'top',
          message: '提示',
          caption: '请填写完整信息',
          color: 'red',
        })
      }
    },
    //取消新建从机
    newCancel () {
      this.createFixed = false;
      this.SlaveHostName = '';
      this.SlaveCount = '';
      this.SlaveExtensionInfo = '';
      this.SlaveHostHostSelect = '';
      this.SlaveHostHostId = '';
      this.SlaveHostHostIndex = '';
    },
    //删除从机
    deleteSlaveHost () {
      if (this.SlaveHostSelected.length == 0) {
        this.$q.notify({
          position: 'top',
          message: '提示',
          caption: '请选择从主机',
          color: 'red',
        })
        return;
      }
      this.$q.dialog({
        title: '提示',
        message: '您确定要删除当前选择的从主机吗',
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
        if (this.SlaveHostSelected.length == 1) {
          // 单个删除slaveHost列表
          let para = `?caseId=${this.detailData.id}&id=${this.SlaveHostSelected[0].id}`
          Apis.deleteSlaveHost(para).then((res) => {
            console.log(res)
            this.getSlaveHostsList();
          })
        } else if (this.SlaveHostSelected.length > 1) {
          // 批量删除slaveHost列表
          let delIdArr = [];
          for (let i = 0; i < this.SlaveHostSelected.length; i++) {
            delIdArr.push(this.SlaveHostSelected[i].id)
          }
          console.log(delIdArr)
          let para = {
            CaseID: this.detailData.id,
            IDS: delIdArr
          }
          Apis.deleteSlaveHostArr(para).then((res) => {
            console.log(res)
            this.getSlaveHostsList();
          })
        }
      }).onCancel(() => {
      })
    },
    //跳转从机详情
    toSlaveHostDetail (evt, row) {
      if (this.isNoRun == 1) {
        return;
      }
      sessionStorage.setItem('SlaveHostDetailData', JSON.stringify(row))
      this.$router.push({
        name: 'SlaveHostDetail'
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
    toHistoryDetail (evt, row) {
      console.log(this.isNoRun)
      if (this.isNoRun == 1) {
        return;
      }
      console.log(evt, row)
      this.$router.push({
        name: 'HistoryDetail',
        query: {
          historyId: row.id,
          caseId: row.caseID
        }
      })
    },
    //运行
    run () {
      this.$q.loading.show()
      let para = `?caseId=${this.$route.query.id}`
      Apis.postTestCaseRun(para).then((res) => {
        console.log(res)
        this.getTestCaseDetail()
      })
    },
    //停止
    stop () {
      this.$q.loading.show()
      let para = `?caseId=${this.$route.query.id}`
      Apis.postTestCaseStop(para).then((res) => {
        console.log(res)
        this.getTestCaseDetail()
      })
    },
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
        this.$q.dialog({
          title: '提示',
          message: res.data
        })
      })
    },
    //查看Slave日志
    lookSlaveLog () {
      if (this.SlaveHostSelected.length == 0) {
        this.$q.notify({
          position: 'top',
          message: '提示',
          caption: '请选择从主机',
          color: 'red',
        })
      } else if (this.SlaveHostSelected.length == 1) {
        //选择单个slavehost
        this.$q.loading.show()
        let para = {
          caseId: this.$route.query.id,
          slaveHostId: this.SlaveHostSelected[0].id
        }
        Apis.getSlaveLog(para).then((res) => {
          this.$q.loading.hide()
          this.$q.dialog({
            title: '提示',
            message: res.data
          })
        })
      } else if (this.SlaveHostSelected.length > 1) {
        this.$q.notify({
          position: 'top',
          message: '提示',
          caption: '只能选择一个从主机',
          color: 'red',
        })
      }
    },
    //查看MonitorUrl
    lookMonitorUrl () {
      this.$q.dialog({
        title: '提示',
        message: this.detailData.monitorUrl
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
  table-layout: fixed;
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
