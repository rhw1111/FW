<template>
  <div class="col-xs-12 col-sm-6 col-xl-6">
    <!-- SlaveHost选择Host -->
    <lookUp :masterHostList="masterHostList"
            :masterSelectIndex='SlaveHostHostIndex'
            :fixed="SlaveHostFixed"
            @addMasterHost='addSlaveHostHost'
            @cancelMasterHost='cancelSlaveHostHost'
            ref='SlaveHostHostlookUp' />
    <!-- 创建从机 -->
    <q-dialog v-model="createFixed"
              persistent>
      <q-card style="width: 100%; max-width: 60vw;">
        <q-card-section>
          <div class="text-h6">创建从主机</div>
        </q-card-section>

        <q-separator />
        <div class="new_input">
          <div class="row input_row">
            <q-input v-model="SlaveHostName"
                     :dense="false"
                     class="col">
              <template v-slot:before>
                <span style="font-size:14px">从主机名称:</span>
              </template>
            </q-input>
            <q-input v-model="SlaveCount"
                     :dense="false"
                     class="col"
                     @keyup="SlaveCount=SlaveCount.replace(/[^\d]/g,'')"
                     placeholder="副本数">
              <template v-slot:before>
                <span style="font-size:14px">数量:</span>
              </template>
            </q-input>
            <q-input :dense="false"
                     class="col"
                     readonly
                     v-model="SlaveHostHostSelect"
                     placeholder="点击右侧加号选择主机">
              <template v-slot:before>
                <span style="font-size:14px">主机:</span>
              </template>
              <template v-slot:append>
                <q-btn round
                       dense
                       flat
                       icon="add"
                       @click="dblSlaveHostHost" />
              </template>
            </q-input>
          </div>
          <div class="row input_row">
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
    <!-- 查看更新从机详情 -->
    <q-dialog v-model="UpdateFixed"
              persistent>
      <q-card style="width: 100%; max-width: 60vw;">
        <q-card-section>
          <div class="text-h6">更新从主机</div>
        </q-card-section>

        <q-separator />
        <div class="new_input">
          <div class="row input_row">
            <q-input v-model="SlaveHostName"
                     :dense="false"
                     class="col">
              <template v-slot:before>
                <span style="font-size:14px">从主机名称:</span>
              </template>
            </q-input>
            <q-input v-model="SlaveCount"
                     :dense="false"
                     class="col"
                     @keyup="SlaveCount=SlaveCount.replace(/[^\d]/g,'')"
                     placeholder="副本数">
              <template v-slot:before>
                <span style="font-size:14px">数量:</span>
              </template>
            </q-input>
            <q-input :dense="false"
                     class="col"
                     readonly
                     v-model="SlaveHostHostSelect"
                     placeholder="点击右侧加号选择主机">
              <template v-slot:before>
                <span style="font-size:14px">主机:</span>
              </template>
              <template v-slot:append>
                <q-btn round
                       dense
                       flat
                       icon="add"
                       @click="dblSlaveHostHost" />
              </template>
            </q-input>
          </div>
          <div class="row input_row">
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
                 @click="newUpdateCancel" />
          <q-btn flat
                 label="更新"
                 color="primary"
                 @click="newUpdateCreate" />
        </q-card-actions>
      </q-card>
    </q-dialog>
    <!-- 从机列表 -->
    <q-list bordered>
      <q-table title="从主机列表"
               :data="SlaveHostList"
               :columns="columns"
               row-key="id"
               selection="multiple"
               :selected.sync="SlaveHostSelected"
               table-style="max-height: 500px"
               :rows-per-page-options=[0]
               no-data-label="暂无数据更新">
        <template v-slot:body-cell-id="props">
          <q-td :props="props">
            <q-btn class="btn"
                   v-if="detailData.engineType!='Jmeter'"
                   color="primary"
                   label="查 看 日 志"
                   style="margin-right:20px;"
                   @click="lookSlaveLog(props)" />
            <q-btn class="btn"
                   color="primary"
                   label="更 新"
                   :disable="isNoRun!=1?false:true"
                   @click="toSlaveHostDetail(props)" />
          </q-td>
        </template>
        <template v-slot:top-right>
          <q-btn class="
                 btn"
                 color="primary"
                 label="新 增"
                 style="margin-right:20px;"
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
    <!-- 从主机日志提示 -->
    <q-dialog v-model="lookLogFlag"
              persistent>
      <q-card class="q-dialog-plugin full-height"
              style="width: 100%; max-width: 80vw;height:800px;overflow:hidden;">
        <q-card-section class="row">
          <div class="text-h6 col-11">查看从主机日志</div>
          <q-btn color="primary"
                 label="关 闭"
                 class="col-1"
                 @click="cancelLookLog" />
        </q-card-section>

        <q-separator />
        <q-card-section style="height:85%;overflow:hidden;">
          <q-splitter v-model="splitterModel"
                      style="height:100%;">

            <template v-slot:before>
              <q-tabs v-model="tab"
                      vertical
                      :no-caps="true"
                      class="text-primary">
                <q-tab v-for="(val,index) in SLaveHostSelect.count"
                       :name="'tab'+index"
                       :key="index"
                       :label="SLaveHostSelect.slaveName+'_'+(index)"
                       @click="lookSalveIndexLog(index)" />
              </q-tabs>
            </template>

            <template v-slot:after>
              <q-tab-panel name=""
                           style="height:100%;overflow: hidden scroll;white-space: pre-line;word-break:break-all;">
                {{SlaveLogText}}
              </q-tab-panel>
            </template>

          </q-splitter>
        </q-card-section>
        <q-separator />
      </q-card>
    </q-dialog>
  </div>
</template>

<script>
import * as Apis from "@/api/index"
import lookUp from '@/components/lookUp.vue'
export default {
  props: ['isNoRun', 'detailData'],
  components: {
    lookUp
  },
  data () {
    return {
      createFixed: false,//createslave Flag
      UpdateFixed: false,//

      lookLogFlag: false,//查看从主机日志log
      SlaveHostFixed: false,//从机Host LookUP Flag

      masterHostList: [], //主机列表

      SlaveHostDetailData: '',//从主机详情数据
      SlaveHostName: '',   //从机名称
      SlaveCount: '',      //从机数量
      SlaveExtensionInfo: '',   //从机拓展信息


      SlaveHostHostId: '',   //SlaveHostHost ID
      SlaveHostHostSelect: '',//主机已选择列表
      SlaveHostHostIndex: -1,  //主机已选择下标

      SlaveHostList: [],       //从机列表
      SLaveHostSelect: '',//从机单选数据
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
        { name: 'address', align: 'left', label: 'ip', field: 'address', },
        { name: 'count', align: 'left', label: '数量', field: 'count', style: 'max-width: 50px', headerStyle: 'max-width: 50px' },
        { name: 'extensionInfo', label: '扩展信息', align: 'left', field: 'extensionInfo', style: 'width:100px;' },
        { name: 'id', label: '操作', align: 'right', field: 'id', headerStyle: 'text-align:center' },
      ],

      // ------------------- 查看从主机日志 ------------------------
      tab: 'tab0',
      splitterModel: 2,
      SlaveLogText: '',        //从机日志内容
      SlaveLogTextArr: [],    //从机日志arr

    }
  },
  methods: {
    //获得从机列表
    getSlaveHostsList () {
      this.$q.loading.show()
      Apis.getSlaveHostsList({ caseId: this.$route.query.id }).then((res) => {
        console.log(res)
        this.SlaveHostList = res.data;
        this.SlaveHostSelected = [];
        this.$q.loading.hide()
      })
    },
    //获得主机列表
    getMasterHostList () {
      this.$q.loading.show()
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

    //--------------------------------------- 选择主机 -------------------------------
    //双击从机host
    dblSlaveHostHost () {
      this.SlaveHostFixed = true;

      this.getMasterHostList()
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
      console.log(this.SlaveHostHostSelect, this.SlaveHostHostId, this.SlaveHostHostIndex)
    },
    //取消添加从机Host
    cancelSlaveHostHost () {
      this.SlaveHostFixed = false;
      this.$refs.SlaveHostHostlookUp.selectIndex = this.SlaveHostHostIndex;
    },

    //-------------------------------------- 新建从主机 ---------------------------------
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
            this.SlaveHostSelected = [];
            this.getSlaveHostsList();
            this.$q.notify({
              position: 'top',
              message: '提示',
              caption: '删除成功',
              color: 'secondary',
            })
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
            this.SlaveHostSelected = [];
            this.getSlaveHostsList();
            this.$q.notify({
              position: 'top',
              message: '提示',
              caption: '删除成功',
              color: 'secondary',
            })
          })
        }
      }).onCancel(() => {
      })
    },

    // -------------------------------------- 从主机详情 -----------------------------------
    //跳转从机详情
    toSlaveHostDetail (evt) {
      this.UpdateFixed = true;
      this.$q.loading.show()
      Apis.getSlaveHostsList({ caseId: this.detailData.id }).then((res) => {
        console.log(res)
        for (let i = 0; i < res.data.length; i++) {
          if (res.data[i].id == evt.row.id) {
            console.log(res.data[i])
            this.SlaveHostDetailData = res.data[i];
            this.SlaveHostName = res.data[i].slaveName;
            this.SlaveCount = res.data[i].count;
            this.SlaveExtensionInfo = res.data[i].extensionInfo;
            this.getUpdateMasterHostList(res.data[i].address);
          }
        }
      })
    },
    //获得主机列表
    getUpdateMasterHostList (AddressValue) {
      this.$q.loading.show()
      Apis.getMasterHostList().then((res) => {
        console.log(res)
        this.masterHostList = res.data;
        for (let i = 0; i < res.data.length; i++) {
          if (res.data[i].address == AddressValue) {
            console.log(res.data[i])
            this.SlaveHostHostSelect = this.masterHostList[i].address;
            this.SlaveHostHostId = this.masterHostList[i].id;
            this.SlaveHostHostIndex = i;
          }
        }
        this.$q.loading.hide()
      })
    },
    //取消更新从主机
    newUpdateCancel () {
      this.UpdateFixed = false;
      this.SlaveHostDetailData = '';
      this.SlaveHostName = '';
      this.SlaveCount = '';
      this.SlaveExtensionInfo = '';
      this.SlaveHostHostSelect = '';
      this.SlaveHostHostId = '';
      this.SlaveHostHostIndex = -1;
    },
    //更新从主机
    newUpdateCreate () {
      let para = {
        "ID": this.SlaveHostDetailData.id,
        "HostID": this.SlaveHostHostId,
        "TestCaseID": this.SlaveHostDetailData.testCaseID,
        "SlaveName": this.SlaveHostName,
        "Count": Number(this.SlaveCount),
        "ExtensionInfo": this.SlaveExtensionInfo
      }
      if (this.SlaveHostName && this.SlaveCount && this.SlaveExtensionInfo) {
        this.$q.loading.show()
        Apis.putSlaveHost(para).then((res) => {
          console.log(res)
          this.getSlaveHostsList();
          this.newUpdateCancel();
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

    //-------------------------------- 查看日志 -------------------------------------------
    //查看Slave日志
    lookSlaveLog (value) {
      console.log(value)
      this.SLaveHostSelect = value.row;
      this.tab = 'tab0';
      this.lookLogFlag = true;
      for (let i = 0; i < value.row.count; i++) {
        this.SlaveLogTextArr.push('')
      }
      this.lookSalveIndexLog(0);
    },
    //查看指定Slave日志
    lookSalveIndexLog (index) {
      console.log(index, this.SlaveLogTextArr[index])
      if (this.SlaveLogTextArr[index] != '') {
        this.SlaveLogText = this.SlaveLogTextArr[index];
        return;
      }
      let para = {
        caseId: this.$route.query.id,
        slaveHostId: this.SLaveHostSelect.id,
        idx: index
      }
      console.log(this.splitterModel)
      this.$q.loading.show()
      Apis.getSlaveLog(para).then((res) => {
        console.log(res)
        this.SlaveLogText = res.data;
        this.SlaveLogTextArr[index] = res.data;
        this.$q.loading.hide()
      })
    },
    //关闭Salve日志界面
    cancelLookLog () {
      this.SLaveHostSelect = '';
      this.lookLogFlag = false;
      this.SlaveLogTextArr = [];
    },

  }
}
</script>



