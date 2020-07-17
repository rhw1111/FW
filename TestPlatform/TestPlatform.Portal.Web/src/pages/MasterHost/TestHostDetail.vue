<template>
  <div class="detail">
    <!-- button -->
    <div class="detail_header">
      <q-btn class="btn"
             color="primary"
             label="保 存"
             @click="putTestHost" />
      <q-btn class="btn"
             style="background: #FF0000; color: white"
             label="删 除"
             @click="deleteTestHost" />
    </div>
    <!-- 从机详情字段 -->
    <div class="q-pa-md row">

      <div class="new_input">
        <div class="row input_row">
          <q-input v-model="Name"
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
                   @dblclick="openSSH">
            <template v-slot:before>
              <span style="font-size:14px">SSH终结点:</span>
            </template>
          </q-input>
        </div>
      </div>
    </div>
    <!-- TestHost SSH端口dialog -->
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
  name: 'TestHostDetail',
  components: {
    SSHLookUp
  },
  data () {
    return {
      DetailId: '',  //详情ID
      TestHostDetail: '',//SSH端口详情

      Name: '',
      SSHEndpoint: '',

      SSHlookUpFlag: false,//新增TestHost SSH弹窗
      SSHSelect: '',       //选择的SSH
      SSHSelectId: '',     //选择的SSHID
      SSHSelectIndex: -1,  //选择的SSH下标
      SSHEndpointDataArr: [],//SSH端口数据列表

      TestHostName: '',//TestHost名称
    }
  },
  mounted () {
    this.DetailId = this.$route.query.id;
    this.getTestHostDetail();
  },
  methods: {
    //获得主机详情
    getTestHostDetail () {
      this.$q.loading.show()
      let para = {
        id: this.DetailId
      }
      Apis.getTestHostDetail(para).then((res) => {
        console.log(res)
        this.TestHostDetail = res.data;
        this.Name = res.data.address;
        this.getSSHEndpointData(res.data.sshEndpointID);
      })
    },
    //获得SSH端口数据
    getSSHEndpointData (sshEndpointID) {
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
    //更新SSH端口
    putTestHost () {
      let para = {
        ID: this.DetailId,
        Address: this.Name,
        SSHEndpointID: this.SSHSelectId,
      }

      if (this.Name == '' || this.SSHSelectId == '') {
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
        this.getTestHostDetail();
      })
    },
    //删除主机
    deleteTestHost () {
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
        let para = `?id=${this.DetailId}`
        this.$q.loading.show()
        Apis.deleteTestHost(para).then(() => {
          this.$q.notify({
            position: 'top',
            message: '提示',
            caption: '删除成功',
            color: 'secondary',
          })
          this.$router.push({
            name: 'MasterHost'
          })
        })
      })
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
  .input_row {
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
