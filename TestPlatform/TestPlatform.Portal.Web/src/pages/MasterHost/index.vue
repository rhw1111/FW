<template>
  <div class="MasterHost">
    <div class="q-pa-md row masterhost">
      <!-- 主机列表 -->
      <q-table class="col-md-4 col-sm-12 col-xs-12"
               title="主机列表"
               :data="MasterHostList"
               :columns="MasterHostColumns"
               selection="multiple"
               :selected.sync="MasterHostSelected"
               row-key="id"
               table-style="max-height: 500px"
               :virtual-scroll-sticky-start="48"
               :rows-per-page-options=[0]
               no-data-label="暂无数据更新"
               @row-dblclick="toTestHostDetail">

        <template v-slot:top-right>
          <q-btn class="btn"
                 color="primary"
                 label="新 增"
                 @click="openHostCreate" />
          <q-btn class="btn"
                 color="red"
                 label="删 除"
                 @click="deleteTestHost" />
        </template>
        <template v-slot:bottom
                  class="row">
          <q-pagination v-model="MasterHostPagination.page"
                        :max="MasterHostPagination.rowsNumber"
                        :input="true"
                        class="col offset-md-7"
                        @input="TestHostNextPage">
          </q-pagination>
        </template>
      </q-table>
      <!-- SSH端口列表 -->
      <q-table class="col-md-8 col-sm-12 col-xs-12"
               title="SSH端点列表"
               :data="SSHEndpointList"
               :columns="SSHEndpointColumns"
               selection="multiple"
               :selected.sync="SSHEndpointSelected"
               row-key="id"
               :rows-per-page-options=[0]
               table-style="max-height: 500px"
               no-data-label="暂无数据更新"
               @row-dblclick="toSSHEndpointDetail">

        <template v-slot:top-right>
          <q-btn class="btn"
                 color="primary"
                 label="新 增"
                 @click="openSSHCreate" />
          <q-btn class="btn"
                 color="red"
                 label="删 除"
                 @click="deleteSSH" />
        </template>
        <template v-slot:bottom
                  class="row">
          <q-pagination v-model="SSHEndpointPagination.page"
                        :max="SSHEndpointPagination.rowsNumber"
                        :input="true"
                        class="col offset-md-9"
                        @input="SSHNextPage">
          </q-pagination>
        </template>
      </q-table>
    </div>

    <!-- 新增SSH端口框 -->
    <q-dialog v-model="createSSHEndpointFlag"
              persistent>
      <q-card style="width:100%">
        <q-card-section>
          <div class="text-h6">创建SSH端点</div>
        </q-card-section>

        <q-separator />
        <div class="new_input">
          <div class="row">
            <q-input v-model="Name"
                     :dense="false"
                     class="col">
              <template v-slot:before>
                <span style="font-size:14px">名称:</span>
              </template>
            </q-input>
            <q-input v-model="Type"
                     :dense="false"
                     class="col"
                     style="margin-left:50px;">
              <template v-slot:before>
                <span style="font-size:14px">类型:</span>
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

        <q-separator />

        <q-card-actions align="right">
          <q-btn flat
                 label="取消"
                 color="primary"
                 @click="newSSHCancel" />
          <q-btn flat
                 label="创建"
                 color="primary"
                 @click="newSSHCreate" />
        </q-card-actions>
      </q-card>
    </q-dialog>

    <!-- 新增主机 -->
    <q-dialog v-model="createTestHostFlag"
              persistent>
      <q-card style="width:100%">
        <q-card-section>
          <div class="text-h6">创建主机</div>
        </q-card-section>

        <q-separator />
        <div class="new_input">
          <div class="row">
            <q-input v-model="TestHostName"
                     :dense="false"
                     class="col">
              <template v-slot:before>
                <span style="font-size:14px">地址:</span>
              </template>
            </q-input>
          </div>
          <div class="row">
            <q-input :dense="false"
                     class="col col-xs-12"
                     readonly
                     v-model="SSHSelect"
                     @dblclick="openSSH">
              <template v-slot:before>
                <span style="font-size:14px">SSH端点:</span>
              </template>
            </q-input>
          </div>
        </div>

        <q-separator />

        <q-card-actions align="right">
          <q-btn flat
                 label="取消"
                 color="primary"
                 @click="newTestHostCancel" />
          <q-btn flat
                 label="创建"
                 color="primary"
                 @click="newTestHostConfirm" />
        </q-card-actions>
      </q-card>
    </q-dialog>

    <!-- 新增TestHost SSH端口dialog -->
    <SSHLookUp :fixed="SSHlookUpFlag"
               :SSHEndpointIndex='SSHSelectIndex'
               :SSHEndpointList='SSHEndpointDataArr'
               @addSSHEndpoint="addSSHEndpoint"
               @cancelSSHEndpoint="cancelSSHEndpoint"
               ref='SSHLookUp' />
  </div>
</template>

<script>
import * as Apis from "@/api/index"
import SSHLookUp from "@/components/SSHLookUp"
export default {
  name: 'MasterHost',
  components: {
    SSHLookUp
  },
  data () {
    return {
      // ------------------------------------MasterHost ---------------------------------
      SSHlookUpFlag: false,//新增TestHost SSH弹窗
      SSHSelect: '',       //选择的SSH
      SSHSelectId: '',     //选择的SSHID
      SSHSelectIndex: -1,  //选择的SSH下标
      SSHEndpointDataArr: [],//SSH端口数据列表

      TestHostName: '',//TestHost名称


      createTestHostFlag: false,//新建主机dialogFlag
      MasterHostList: [], //主机列表
      //主机分页配置
      MasterHostPagination: {
        page: 1,          //页码
        rowsNumber: 1     //总页数
      },
      MasterHostSelected: [],//主机列表选择
      //主机表格配置
      MasterHostColumns: [
        {
          name: 'address',
          required: true,
          label: '地址',
          align: 'left',
          field: row => row.address,
          format: val => `${val}`,
        },
      ],
      // ------------------------------------ SSH端口 ------------------------------------
      SSHEndpointList: [], //SSH端口列表
      //SSH端口列表分页配置
      SSHEndpointPagination: {
        page: 1,          //页码
        rowsNumber: 1     //总页数
      },
      SSHEndpointSelected: [],//SSH端口选择
      //SSH端口表格配置
      SSHEndpointColumns: [
        {
          name: 'name',
          required: true,
          label: '名称',
          align: 'left',
          field: row => row.name,
          format: val => `${val}`,
        },
        { name: 'type', align: 'left', label: '类型', field: 'type', },
        { name: 'configuration', label: '配置', align: 'left', field: 'configuration', },
      ],

      createSSHEndpointFlag: false,//创建dialogFlag

      Name: '',//SSH名称
      Type: '',//SSH类型
      Configuration: '',//SSH配置

    }
  },
  mounted () {
    this.getSSHEndpointList();
  },
  methods: {
    //获得SSH端口列表
    getSSHEndpointList (page) {
      this.$q.loading.show()
      let para = {
        matchName: '',
        page: page || 1,
        pageSize: ''
      }
      Apis.getSSHEndpointList(para).then((res) => {
        console.log(res)
        this.SSHEndpointList = res.data.results;
        this.SSHEndpointPagination.page = page || 1;
        this.SSHEndpointPagination.rowsNumber = Math.ceil(res.data.totalCount / 50);
        this.getTestHostList();
        this.getSSHEndpointData();
      })
    },
    openSSHCreate () {
      this.createSSHEndpointFlag = true;
    },
    //取消关闭SSH创建框
    newSSHCancel () {
      this.Name = '';
      this.Type = '';
      this.Configuration = '';
      this.createSSHEndpointFlag = false;
    },
    //创建SSH端口
    newSSHCreate () {
      let para = {
        Name: this.Name,
        Type: this.Type,
        Configuration: this.Configuration,
      }
      if (!this.Name && !this.Type && !this.Configuration) {
        this.$q.notify({
          position: 'top',
          message: '提示',
          caption: '请填写完整信息',
          color: 'red',
        })
        return;
      }
      this.$q.loading.show()
      console.log(para)
      Apis.postCreateSSHEndpoint(para).then((res) => {
        console.log(res)
        this.newSSHCancel();
        this.getSSHEndpointList();
        this.$q.notify({
          position: 'top',
          message: '提示',
          caption: '创建成功',
          color: 'secondary',
        })
      })
    },
    //删除SSH端口
    deleteSSH () {
      if (this.SSHEndpointSelected.length == 0) {
        this.$q.notify({
          position: 'top',
          message: '提示',
          caption: '请选择SSH端口',
          color: 'red',
        })
        return;
      }
      this.$q.dialog({
        title: '提示',
        message: '您确定要删除当前选择的SSH端口吗',
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
        if (this.SSHEndpointSelected.length == 1) {
          //单个删除SSH端口
          let para = `?id=${this.SSHEndpointSelected[0].id}`
          this.$q.loading.show()
          Apis.deleteSSHEndpoint(para).then(() => {
            this.$q.notify({
              position: 'top',
              message: '提示',
              caption: '删除成功',
              color: 'secondary',
            })
            this.getSSHEndpointList();
          })
        } else if (this.SSHEndpointSelected.length > 1) {
          //批量删除SSH端口
          let para = {
            delArr: []
          }
          for (let i = 0; i < this.SSHEndpointSelected.length; i++) {
            para.delArr.push(this.SSHEndpointSelected[i].id)
          }
          this.$q.loading.show()
          Apis.deleteSSHEndpointArr(para).then(() => {
            this.$q.notify({
              position: 'top',
              message: '提示',
              caption: '删除成功',
              color: 'secondary',
            })
            this.getSSHEndpointList();
          })
        }
      })
    },
    //跳转SSH端口详情
    toSSHEndpointDetail (val, row) {
      this.$router.push({
        name: 'SSHEndpointDetail',
        query: {
          id: row.id
        }
      })
    },
    //SSH端口分页
    SSHNextPage (value) {
      this.getSSHEndpointList(value)
    },
    // --------------------------------- TestHost ---------------------------
    //获得TestHost列表
    getTestHostList (page) {
      let para = {
        matchName: '',
        page: page || 1,
        pageSize: ''
      }
      Apis.getTestHostList(para).then((res) => {
        console.log(res)
        this.MasterHostList = res.data.results;
        this.MasterHostPagination.page = page || 1;
        this.MasterHostPagination.rowsNumber = Math.ceil(res.data.totalCount / 50);
        this.$q.loading.hide()
      })
    },
    //获得SSH端口数据
    getSSHEndpointData () {
      Apis.getSSHEndpointData({}).then((res) => {
        console.log(res)
        this.SSHEndpointDataArr = res.data;
      })
    },
    //新增HostFlag
    openHostCreate () {
      this.createTestHostFlag = true;
    },
    //取消新建TestHost
    newTestHostCancel () {
      this.createTestHostFlag = false;

      this.TestHostName = '';
      this.SSHSelect = '';
      this.SSHSelectId = '';
      this.SSHSelectIndex = -1;
    },
    //新建TestHost
    newTestHostConfirm () {
      if (this.TestHostName && this.SSHSelectId) {
        this.$q.loading.show()
        let para = {
          Address: this.TestHostName,
          SSHEndpointID: this.SSHSelectId
        }
        Apis.postCreateTestHost(para).then(() => {
          this.getSSHEndpointList();
          this.newTestHostCancel();
          this.$q.notify({
            position: 'top',
            message: '提示',
            caption: '创建成功',
            color: 'secondary',
          })
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
    //打开SSH端口
    openSSH () {
      this.SSHlookUpFlag = true;
      this.createTestHostFlag = false;
    },
    //TestHost添加SSH端口
    addSSHEndpoint (index) {
      if (index == undefined) {
        return false;
      }
      this.SSHSelect = this.SSHEndpointDataArr[index].name;
      this.SSHSelectId = this.SSHEndpointDataArr[index].id;
      this.SSHSelectIndex = index;

      this.SSHlookUpFlag = false;
      this.createTestHostFlag = true;
    },
    //TestHost取消添加SSH端口
    cancelSSHEndpoint () {
      this.SSHlookUpFlag = false;
      this.createTestHostFlag = true;
      this.$refs.SSHLookUp.selectIndex = this.SSHSelectIndex;
    },
    //删除TestHost
    deleteTestHost () {
      if (this.MasterHostSelected.length == 0) {
        this.$q.notify({
          position: 'top',
          message: '提示',
          caption: '请选择主机',
          color: 'red',
        })
        return;
      }
      this.$q.dialog({
        title: '提示',
        message: '您确定要删除当前选择的主机吗',
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
        if (this.MasterHostSelected.length == 1) {
          //单个删除SSH端口
          let para = `?id=${this.MasterHostSelected[0].id}`
          this.$q.loading.show()
          Apis.deleteTestHost(para).then(() => {
            this.$q.notify({
              position: 'top',
              message: '提示',
              caption: '删除成功',
              color: 'secondary',
            })
            this.getSSHEndpointList();
          })
        } else if (this.MasterHostSelected.length > 1) {
          //批量删除SSH端口
          let para = {
            delArr: []
          }
          for (let i = 0; i < this.MasterHostSelected.length; i++) {
            para.delArr.push(this.MasterHostSelected[i].id)
          }
          this.$q.loading.show()
          Apis.deleteTestHostArr(para).then(() => {
            this.$q.notify({
              position: 'top',
              message: '提示',
              caption: '删除成功',
              color: 'secondary',
            })
            this.getSSHEndpointList();
          })
        }
      })
    },
    //跳转TestHost详情
    toTestHostDetail (val, row) {
      this.$router.push({
        name: 'TestHostDetail',
        query: {
          id: row.id
        }
      })
    },
    //TestHost分页
    TestHostNextPage (value) {
      this.getTestHostList(value)
    }
  }
}
</script>

<style lang="scss" scoped>
.masterhost {
  .btn {
    margin-right: 10px;
  }
}
</style>
<style lang="scss">
.q-table {
  table-layout: fixed;
  .text-left {
    white-space: nowrap;
    overflow: hidden;
    text-overflow: ellipsis;
  }
}
.q-table--col-auto-width {
  width: 75px;
}
.q-textarea .q-field__native {
  resize: none;
}
.new_input {
  width: 100%;
  padding: 10px 30px;
  .row {
    margin-bottom: 10px;
  }
}
</style>