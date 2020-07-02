<template>
  <div class="detail">
    <!-- 主机选择框 -->
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
             @click="putSlaveHost" />
      <q-btn class="btn"
             style="background: #FF0000; color: white"
             label="删 除"
             @click="deleteSlaveHost" />
    </div>
    <!-- 从机详情字段 -->
    <div class="q-pa-md row">

      <div class="new_input">
        <div class="row">
          <q-input v-model="SlaveHostName"
                   :dense="false"
                   class="col">
            <template v-slot:before>
              <span style="font-size:14px">SlaveName:</span>
            </template>
          </q-input>
          <q-input v-model="SlaveCount"
                   :dense="false"
                   class="col"
                   style="margin-left:50px;">
            <template v-slot:before>
              <span style="font-size:14px">Count:</span>
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
          <q-input v-model="SlaveExtensionInfo"
                   :dense="false"
                   class="col-xs-12"
                   type="textarea"
                   outlined>
            <template v-slot:before>
              <span style="font-size:14px">ExtensionInfo:</span>
            </template>
          </q-input>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import * as Apis from "@/api/index"
import lookUp from "@/components/lookUp.vue"
export default {
  name: 'SlaveHostDetail',
  components: {
    lookUp
  },
  data () {
    return {
      HostFixed: false,   //主机lookUp Flag
      masterHostList: [],//主机列表
      masterHostSelect: '',//主机已选择列表
      masterSelectIndex: '',  //主机已选择下标
      MasterHostID: '',//主机已选择ID


      SlaveHostData: '',//从机数据

      SlaveHostName: '',   //从机名称
      SlaveCount: '',      //从机数量
      SlaveExtensionInfo: '',   //从机拓展信息
    }
  },
  mounted () {
    this.$q.loading.show()
    this.SlaveHostData = JSON.parse(sessionStorage.getItem('SlaveHostDetailData'));
    console.log(this.SlaveHostData)
    this.getMasterHostList();
    this.SlaveHostName = this.SlaveHostData.slaveName;
    this.SlaveCount = this.SlaveHostData.count;
    this.SlaveExtensionInfo = this.SlaveHostData.extensionInfo;
    this.MasterHostID = this.SlaveHostData.hostID;
  },
  methods: {
    //获得主机列表
    getMasterHostList () {
      Apis.getMasterHostList().then((res) => {
        console.log(res)
        this.masterHostList = res.data;
        for (let i = 0; i < res.data.length; i++) {
          if (res.data[i].id == this.SlaveHostData.hostID) {
            this.masterHostSelect = res.data[i].address
            this.masterSelectIndex = i;
            break;
          }
        }
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
    //获得从机列表
    getSlaveHostsList () {
      Apis.getSlaveHostsList({ caseId: this.SlaveHostData.testCaseID }).then((res) => {
        console.log(res)
        for (let i = 0; i < res.data.length; i++) {
          if (res.data[i].id == this.SlaveHostData.id) {
            sessionStorage.setItem('SlaveHostDetailData', JSON.stringify(res.data[i]))
            this.$q.loading.hide()
            break;
          }
        }
      })
    },
    //保存更新从机
    putSlaveHost () {
      let para = {
        "ID": this.SlaveHostData.id,
        "HostID": this.MasterHostID,
        "TestCaseID": this.SlaveHostData.testCaseID,
        "SlaveName": this.SlaveHostName,
        "Count": Number(this.SlaveCount),
        "ExtensionInfo": this.SlaveExtensionInfo
      }
      if (this.SlaveHostData.id && this.SlaveHostData.testCaseID && this.SlaveHostName && this.SlaveCount && this.SlaveExtensionInfo && this.MasterHostID) {
        this.$q.loading.show()
        Apis.putSlaveHost(para).then((res) => {
          console.log(res)
          this.getSlaveHostsList();
          this.$q.notify({
            position: 'top',
            message: '提示',
            caption: '保存成功',
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
    //删除当前从机
    deleteSlaveHost () {
      this.$q.dialog({
        title: '提示',
        message: '您确定要删除当前SalveHost吗',
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
          caseId: this.SlaveHostData.testCaseID,
          id: this.SlaveHostData.id
        }
        Apis.deleteSlaveHost(para).then((res) => {
          console.log(res)
          this.$router.push({ name: 'TestCaseDetail', query: { id: this.SlaveHostData.testCaseID } })
        })
      }).onCancel(() => {
      })
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
