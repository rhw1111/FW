<template>
  <div class="col-md-5 col-sm-12 col-xs-12">
    <!-- 主机列表 -->
    <q-table title="主机列表"
             :data="MasterHostList"
             :columns="MasterHostColumns"
             selection="multiple"
             :selected.sync="MasterHostSelected"
             row-key="id"
             table-style="max-height: 500px"
             :virtual-scroll-sticky-start="48"
             :rows-per-page-options=[0]
             no-data-label="暂无数据更新">

      <template v-slot:top-right>
        <q-btn class="btn"
               color="primary"
               label="新 增"
               style="margin-right:20px;"
               @click="openHostCreate" />
        <q-btn class="btn"
               color="red"
               label="删 除"
               @click="deleteTestHost" />
      </template>
      <template v-slot:body-cell-id="props">
        <q-td class="text-left"
              :props="props">
          <q-btn class="btn"
                 color="primary"
                 style="margin-right:20px;"
                 label="更 新"
                 @click="toTestHostDetail(props)" />
          <q-btn class="btn"
                 color="red"
                 label="删 除"
                 @click="delectSelectTestHost(props)" />
        </q-td>
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
    <!-- 新增主机 -->
    <q-dialog v-model="createTestHostFlag"
              persistent>
      <q-card style="width:100%">
        <q-card-section>
          <div class="text-h6">创建主机</div>
        </q-card-section>

        <q-separator />
        <div class="new_input">
          <div class="row input_row">
            <q-input v-model="TestHostName"
                     :dense="false"
                     class="col">
              <template v-slot:before>
                <span style="font-size:14px">地址:</span>
              </template>
            </q-input>
          </div>
          <div class="row input_row">
            <q-input :dense="false"
                     class="col col-xs-12"
                     readonly
                     v-model="SSHSelect"
                     placeholder="点击右侧加号选择SSH终结点">
              <template v-slot:before>
                <span style="font-size:14px">SSH终结点:</span>
              </template>
              <template v-slot:append>
                <q-btn round
                       dense
                       flat
                       icon="add"
                       @click="openSSH" />
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
    <!-- 更新主机 -->
    <q-dialog v-model="updateTestHostFlag"
              persistent>
      <q-card style="width:100%">
        <q-card-section>
          <div class="text-h6">更新主机</div>
        </q-card-section>

        <q-separator />
        <div class="new_input">
          <div class="row input_row">
            <q-input v-model="TestHostName"
                     :dense="false"
                     class="col">
              <template v-slot:before>
                <span style="font-size:14px">地址:</span>
              </template>
            </q-input>
          </div>
          <div class="row input_row">
            <q-input :dense="false"
                     class="col col-xs-12"
                     readonly
                     v-model="SSHSelect"
                     placeholder="点击右侧加号选择SSH终结点">
              <template v-slot:before>
                <span style="font-size:14px">SSH终结点:</span>
              </template>
              <template v-slot:append>
                <q-btn round
                       dense
                       flat
                       icon="add"
                       @click="openSSH" />
              </template>
            </q-input>
          </div>
        </div>

        <q-separator />

        <q-card-actions align="right">
          <q-btn flat
                 label="取消"
                 color="primary"
                 @click="updateTestHostCancel" />
          <q-btn flat
                 label="更新"
                 color="primary"
                 @click="updateTestHostConfirm" />
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
  components: {
    SSHLookUp,
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
        {
          name: 'sshEndpointName', label: 'SSH终结点', align: 'left', field: 'sshEndpointName',
        },
        { name: 'id', label: '操作', align: 'right', field: 'id', headerStyle: 'text-align:center' },
      ],

      updateTestHostFlag: false,//更新主机flag
      updateTestHostId: '',//更新主机的ID
    }
  },
  mounted () {
    this.getTestHostList();
  },
  methods: {
    // --------------------------------- TestHost ---------------------------
    //获得TestHost列表
    getTestHostList (page) {
      this.$q.loading.show()
      let para = {
        matchName: '',
        page: page || 1,
        pageSize: 50
      }
      Apis.getTestHostList(para).then((res) => {
        console.log(res)
        this.MasterHostList = res.data.results;
        this.MasterHostPagination.page = page || 1;
        this.MasterHostPagination.rowsNumber = Math.ceil(res.data.totalCount / 50);
        this.getSSHEndpointData();
      })
    },
    //获得SSH端口数据
    getSSHEndpointData () {
      Apis.getSSHEndpointData({}).then((res) => {
        this.SSHEndpointDataArr = res.data;
        this.$q.loading.hide()
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
          this.getTestHostList();
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
    },
    //TestHost取消添加SSH端口
    cancelSSHEndpoint () {
      this.SSHlookUpFlag = false;
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
        let SelectedLength = this.MasterHostSelected.length;
        let DelNum = 0;
        for (let i = 0; i < this.MasterHostSelected.length; i++) {
          //单个批量删除SSH端口
          let para = `?id=${this.MasterHostSelected[i].id}`
          this.$q.loading.show()
          Apis.deleteTestHost(para).then(() => {
            DelNum++;
            console.log(DelNum, SelectedLength)
            if (DelNum == SelectedLength) {
              this.$q.notify({
                position: 'top',
                message: '提示',
                caption: '删除成功',
                color: 'secondary',
              })
              this.MasterHostSelected = [];
              this.getTestHostList();
            }
          }).catch(() => {
            DelNum++;
            if (DelNum == SelectedLength) {
              this.MasterHostSelected = [];
              this.getTestHostList();
            }
          })
        }

      })
    },
    //单个删除TestHost
    delectSelectTestHost (value) {
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
        this.$q.loading.show()
        let para = `?id=${value.row.id}`
        Apis.deleteTestHost(para).then(() => {
          this.$q.notify({
            position: 'top',
            message: '提示',
            caption: '删除成功',
            color: 'secondary',
          })
          this.MasterHostSelected = [];
          this.getTestHostList();
        })
      })
    },
    //TestHost分页
    TestHostNextPage (value) {
      this.getTestHostList(value)
    },
    // --------------------------------------------------------------------   更新主机  --------------------------------------------------------------------
    //跳转TestHost详情
    toTestHostDetail (env) {
      this.$q.loading.show()
      let para = {
        id: env.row.id
      }
      this.updateTestHostId = env.row.id;
      Apis.getTestHostDetail(para).then((res) => {
        console.log(res)
        this.updateTestHostFlag = true;
        this.TestHostName = res.data.address;
        this.getSSHEndpointUpdateData(res.data.sshEndpointID);
      })
    },
    //获得SSH端口数据
    getSSHEndpointUpdateData (sshEndpointID) {
      Apis.getSSHEndpointData({}).then((res) => {
        console.log(res)
        this.SSHEndpointDataArr = res.data;
        for (let i = 0; i < res.data.length; i++) {
          if (res.data[i].id == sshEndpointID) {
            this.SSHSelect = res.data[i].name;
            this.SSHSelectId = res.data[i].id;
            this.SSHSelectIndex = i;
            break;
          }
        }
        this.$q.loading.hide()
      })
    },
    //取消更新
    updateTestHostCancel () {
      this.SSHEndpointDataArr = '';
      this.updateTestHostId = '';
      this.TestHostName = 'res.data.address';
      this.SSHSelect = '';
      this.SSHSelectId = '';
      this.SSHSelectIndex = 0;
      this.updateTestHostFlag = false;
    },
    //更新
    updateTestHostConfirm () {
      let para = {
        ID: this.updateTestHostId,
        Address: this.TestHostName,
        SSHEndpointID: this.SSHSelectId,
      }

      if (this.TestHostName == '' || this.SSHSelectId == '') {
        this.$q.notify({
          position: 'top',
          message: '提示',
          caption: '请填写完整信息',
          color: 'red',
        })
        return;
      }
      this.$q.loading.show()
      Apis.putTestHost(para).then(() => {
        this.$q.notify({
          position: 'top',
          message: '提示',
          caption: '更新成功',
          color: 'secondary',
        })
        this.getTestHostList();
        this.updateTestHostCancel();
      })
    }
  }
}
</script>

<style lang="stylus" scoped></style>