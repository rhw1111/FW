<template>
  <div class="detail">
    <!-- 选择主机框 -->
    <lookUp :masterHostList="masterHostList"
            :masterSelectIndex='masterSelectIndex'
            :fixed="HostFixed"
            @addMasterHost='addMasterHost'
            @cancelMasterHost='cancelMasterHost'
            ref='lookUp' />
    <!-- button -->
    <div class="detail_header">
      <q-btn class="btn"
             color="primary"
             label="保 存"
             @click="putTestCase" />
      <q-btn class="btn"
             style="background: #FF0000; color: white"
             label="删 除"
             @click="deleteTestCase" />
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
    <!-- TestCase字段 -->
    <div class="q-pa-md row">

      <div class="new_input">
        <div class="row">
          <q-input v-model="Name"
                   :dense="false"
                   class="col">
            <template v-slot:before>
              <span style="font-size:14px">Name:</span>
            </template>
          </q-input>
          <q-input v-model="EngineType"
                   :dense="false"
                   class="col"
                   style="margin-left:50px;">
            <template v-slot:before>
              <span style="font-size:14px">EngineType:</span>
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
              <span style="font-size:14px">Configuration:</span>
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
          <div class="text-h6">创建SlaveHost</div>
        </q-card-section>

        <q-separator />
        <div class="new_input">
          <div class="row">
            <q-input v-model="SlaveHostName"
                     :dense="false"
                     class="col">
              <template v-slot:before>
                <span style="font-size:14px">SlaveHostName:</span>
              </template>
            </q-input>
            <q-input v-model="SlaveCount"
                     :dense="false"
                     class="col">
              <template v-slot:before>
                <span style="font-size:14px">Count:</span>
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
                <span style="font-size:14px">ExtensionInfo:</span>
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
        <q-table title="SalveHost列表"
                 :data="SlaveHostList"
                 :columns="columns"
                 row-key="id"
                 selection="multiple"
                 :selected.sync="SlaveHostSelected"
                 @row-dblclick="toSlaveHostDetail"
                 :rows-per-page-options=[0]>
          <template v-slot:top-right>

            <q-btn class="btn"
                   color="primary"
                   label="新 增"
                   @click="openSlaveHost" />
            <q-btn class="btn"
                   style="background: #FF0000; color: white"
                   label="删 除"
                   @click="deleteSlaveHost" />
          </template>
          <template v-slot:bottom>
          </template>
        </q-table>
      </q-list>
      <!-- 历史记录列表 -->
      <q-list bordered
              class="col-xs-12 col-sm-6 col-xl-6">
        <q-table title="History列表"
                 :data="HistoryList"
                 :columns="HistoryColumns"
                 row-key="id"
                 selection="multiple"
                 :selected.sync="HistorySelected"
                 @row-dblclick="toHistoryDetail"
                 :rows-per-page-options=[0]>
          <template v-slot:top-right>
            <q-btn class="btn"
                   style="background: #FF0000; color: white"
                   label="删 除"
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
export default {
  name: 'TestCaseDetail',
  components: {
    lookUp
  },
  data () {
    return {
      createFixed: false,//createslave Flag
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

      Name: '',           //Name
      Configuration: '',  //Configuration
      EngineType: '',     //EngineType
      MasterHostID: '',   //MasterHostID 主机ID


      SlaveHostName: '',   //从机名称
      SlaveCount: '',      //从机数量
      SlaveExtensionInfo: '',   //从机拓展信息

      SlaveHostList: [],       //从机列表
      SlaveHostSelected: [],   //从机表格选择
      //从机表格配置
      columns: [
        {
          name: 'slaveName',
          required: true,
          label: 'SlaveName',
          align: 'left',
          field: row => row.slaveName,
          format: val => `${val}`,
        },
        { name: 'count', align: 'left', label: 'Count', field: 'count', },
        { name: 'extensionInfo', label: 'ExtensionInfo', align: 'left', field: 'extensionInfo', },
      ],


      HistoryList: [],//历史记录列表
      HistorySelected: [],//历史记录选择
      //历史记录表格配置
      HistoryColumns: [
        {
          name: 'createTime',
          required: true,
          label: 'CreateTime',
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
  },
  methods: {
    //获得TestCase详情
    getTestCaseDetail () {
      this.$q.loading.show()
      Apis.getTestCaseDetail({ id: this.$route.query.id }).then((res) => {
        console.log(res)
        this.masterHostSelect = res.data.masterHost.address;
        this.MasterHostID = res.data.masterHost.id;
        this.detailData = res.data;
        this.Name = res.data.name;
        this.Configuration = res.data.configuration;
        this.EngineType = res.data.engineType;
        this.getMasterHostList();
        this.getSlaveHostsList();
        this.getHistoryList();
      })
    },
    //获得从机列表
    getSlaveHostsList () {
      Apis.getSlaveHostsList({ caseId: this.$route.query.id }).then((res) => {
        console.log(res)
        this.SlaveHostList = res.data;
        this.$q.loading.hide()
      })
    },
    //获得主机列表
    getMasterHostList () {
      Apis.getMasterHostList().then((res) => {
        console.log(res)
        this.masterHostList = res.data;
        for (let i = 0; i < res.data.length; i++) {
          if (res.data[i].id == this.detailData.masterHost.id) {
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
        this.$q.loading.hide()
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
        message: '您确定要删除当前的TestCase吗',
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
        let para = {
          id: this.detailData.id
        }
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
        "HostID": this.detailData.masterHost.id,
        "TestCaseID": this.detailData.id,
        "SlaveName": this.SlaveHostName,
        "Count": Number(this.SlaveCount),
        "ExtensionInfo": this.SlaveExtensionInfo
      }
      if (this.detailData.id && this.SlaveHostName && this.SlaveCount && this.SlaveExtensionInfo && this.detailData.masterHost.id) {
        this.$q.loading.show()
        Apis.postCreateSlaveHost(para).then((res) => {
          console.log(res)
          this.$q.notify({
            position: 'top',
            message: '提示',
            caption: '创建成功',
            color: 'secondary',
          })
          this.createFixed = false;
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
    //删除从机
    deleteSlaveHost () {
      console.log(this.SlaveHostSelected, this.detailData)

      this.$q.dialog({
        title: '提示',
        message: '您确定要删除当前选择的SalveHost吗',
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
      sessionStorage.setItem('SlaveHostDetailData', JSON.stringify(row))
      this.$router.push({
        name: 'SlaveHostDetail'
      })
    },
    newCancel () {
      this.createFixed = false;
      this.SlaveHostName = '';
      this.SlaveCount = '';
      this.SlaveExtensionInfo = '';
    },

    //删除历史记录
    deleteHistory () {
      this.$q.dialog({
        title: '提示',
        message: '您确定要删除当前选择的History吗',
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
      console.log(evt, row)
      this.$router.push({
        name: 'HistoryDetail',
        query: {
          historyId: row.id,
          caseId: row.caseID
        }
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
  .HostList {
    .btn {
      margin: 5px 10px;
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
