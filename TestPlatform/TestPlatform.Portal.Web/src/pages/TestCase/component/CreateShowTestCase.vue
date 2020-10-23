<template>
  <div>
    <!-- 更改文件目录 -->
    <q-dialog v-model="ChangeFileDirectoryFlag"
              persistent>
      <q-card style="width: 100%;">
        <q-card-section>
          <div class="text-h6">选择文件目录位置</div>
        </q-card-section>

        <q-separator />

        <TreeEntity ref="TreeEntity" />

        <q-separator />

        <q-card-actions align="right">
          <q-btn flat
                 label="取消"
                 color="primary"
                 @click="ChangeFileDirectoryFlag = false;" />
          <q-btn flat
                 label="确定"
                 color="primary"
                 @click="SelectDirectoryLocation" />
        </q-card-actions>
      </q-card>
    </q-dialog>
    <!--  主机选择框  -->
    <lookUp :masterHostList="masterHostList"
            :masterSelectIndex="masterHostIndex"
            :fixed="HostFixed"
            @addMasterHost='addMasterHost'
            @cancelMasterHost='cancelMasterHost'
            ref='lookUp' />
    <div class="new_input">
      <!-- 基础配置 -->
      <div class="row input_row">
        <q-input v-model="Name"
                 :dense="false"
                 class="col">
          <template v-slot:before>
            <span style="font-size:14px">名称:</span>
          </template>
        </q-input>
        <q-select v-model="EngineType"
                  :options="['Http','Tcp','WebSocket','Jmeter']"
                  class="col"
                  :dense="false">
          <template v-slot:before>
            <span style="font-size:14px">引擎类型:</span>
          </template>
          <template v-slot:prepend>
          </template>
        </q-select>

      </div>

      <!-- 基础配置 -->
      <div class="row input_row">
        <q-input :dense="false"
                 class="col"
                 readonly
                 v-model="masterHostSelect"
                 placeholder="点击右侧加号选择主机">
          <template v-slot:before>
            <span style="font-size:14px">主机:</span>
          </template>
          <template v-slot:append>
            <q-btn round
                   dense
                   flat
                   icon="add"
                   @click="masterHost" />
          </template>
        </q-input>
        <q-input v-model="LocustMasterBindPort"
                 @input="ifRegular(LocustMasterBindPort)"
                 :dense="false"
                 class="col"
                 v-if="EngineType!='Jmeter'"
                 placeholder="值范围(15557~25557) 默认15557">
          <template v-slot:before>
            <span style="font-size:14px">主机端口:</span>
          </template>
        </q-input>
      </div>
      <div class="row input_row">
        <q-input v-model="ChangeFileDirectoryName"
                 :dense="false"
                 class="col"
                 readonly
                 placeholder="点击右侧加号选择文件目录位置">
          <template v-slot:before>
            <span style="font-size:14px">文件目录位置:</span>
          </template>
          <template v-slot:append>
            <q-btn round
                   dense
                   flat
                   icon="add"
                   @click="ChangeFileDirectory" />
          </template>
        </q-input>
      </div>
      <!-- 参数配置 -->
      <ParameterConfig ref="ParameterConfig"
                       v-if="TypeFlag && EngineType!='Jmeter'"
                       :paraConfiguration="paraConfig"
                       :LocustMasterBindPort="LocustMasterBindPort" />
      <!-- Jmeter -->
      <Jmeter ref="Jmeter"
              v-if="TypeFlag && EngineType=='Jmeter'"
              :paraConfiguration="paraConfig" />
    </div>
  </div>
</template>

<script>
import * as Apis from "@/api/index"
import ParameterConfig from "./ParameterConfig.vue"   //参数配置
import Jmeter from "./Jmeter.vue"                     //参数配置
import lookUp from "@/components/lookUp.vue"          //主机列表
import TreeEntity from "@/components/TreeEntity.vue"  //目录管理结构树
export default {
  name: 'CreateShowTestCase',
  props: ['detailData', 'currentDirectory'],
  components: {
    lookUp,
    TreeEntity,
    ParameterConfig,
    Jmeter
  },
  watch: {
    //如果是详情数据直接替换
    detailData (val) {
      if (val) {
        console.log(val)
        let configuration = JSON.parse(val.configuration);
        console.log(configuration)
        if (val.engineType == 'Jmeter') {
          this.paraConfig = configuration;
        } else {
          this.paraConfig = {
            UserCount: configuration.UserCount || '',//压测用户总数
            PerSecondUserCount: configuration.PerSecondUserCount || '',//每秒加载用户数
            Address: configuration.Address || '',//被测服务器
            Port: configuration.Port || '',//被测服务器端口
            Duration: configuration.Duration || '',//压测时间
            ResponseSeparator: configuration.ResponseSeparator || '',//结束分隔符
            DataSourceVars: configuration.DataSourceVars || [],//数据源
            LocustMasterBindPort: configuration.LocustMasterBindPort || '',//数据源
            IsPrintLog: configuration.IsPrintLog == true ? '是' : '否',//是否打印日志
            SyncType: configuration.SyncType == false ? '异步模式' : '同步模式',//是否同步异步 
            ConnectInit: configuration.ConnectInit || { VarSettings: [] },//连接初始化
            SendInit: configuration.SendInit.VarSettings ? configuration.SendInit : { VarSettings: [] },//发送初始化
            StopInit: configuration.StopInit.VarSettings ? configuration.StopInit : { VarSettings: [] }//停止初始化
          }
        }


        this.Name = val.name;
        this.EngineType = val.engineType;
        this.masterHostSelect = val.masterHostAddress;
        this.MasterHostID = val.masterHostID;
        this.LocustMasterBindPort = configuration.LocustMasterBindPort;
        if (val.parentID) {
          this.getTreeEntityTreePath(val.parentID);
        } else {
          this.ChangeFileDirectoryName = val.parentName != '' ? val.parentName : '根目录 >';
        }
        this.ChangeFileDirectoryId = val.parentID;
        this.Configuration = JSON.stringify(JSON.parse(val.configuration), null, 2);
        this.TypeFlag = true;
      }
    },
  },
  mounted () {
    //如果不是在详情则直接打开组件
    if (this.$route.name != 'TestCaseDetail') {
      this.TypeFlag = true;
    }
    //判断当前的目录是什么
    if (this.currentDirectory) {
      this.getTreeEntityTreePath(this.currentDirectory.id);
      this.ChangeFileDirectoryId = this.currentDirectory.id;
    } else {
      this.ChangeFileDirectoryName = '根目录 >';
    }
    console.log(this.currentDirectory)
    //this.getDataSourceName();
  },
  data () {
    return {
      dataSourceName: [],//数据源名称数组
      // --------------------- 主机选择 ---------------------
      HostFixed: false,     //主机flag
      masterHostSelect: '', //主机选择
      masterHostIndex: -1,//主机下标
      masterHostList: [],//主机列表


      //------------------------ 基础参数配置 -----------------------
      Name: '',           //Name
      Configuration: '',  //Configuration
      EngineType: '',     //EngineType
      FolderID: null,     //目录位置
      MasterHostID: '',   //MasterHostID 主机ID

      // ----------------------- 参数配置 ---------------------------
      TypeFlag: false,//如果是在详情后展示组件
      paraConfig: '',
      LocustMasterBindPort: '',//主机端口


      // -------- 目录管理 -------
      ChangeFileDirectoryFlag: false,//更改文件目录Flag
      ChangeFileDirectoryName: '',   //选择的目录名称
      ChangeFileDirectoryId: null,   //选择的目录Id
    }
  },
  methods: {
    //获得数据源名称
    getDataSourceName () {
      let para = {}
      Apis.getDataSourceName(para).then((res) => {
        console.log(res)
        for (let i = 0; i < res.data.length; i++) {
          this.dataSourceName.push(res.data[i].name)
        }
        this.$q.loading.hide()
      })
    },
    // ---------------------------------------- 测试用例创建取消 ------------------------------------ 
    //取消创建测试用例
    newCancel () {
      this.Name = '';
      this.Configuration = '';
      this.EngineType = '';
      this.MasterHostID = '';
      this.masterHostSelect = '';
      this.$refs.lookUp.selectIndex = -1;

      // this.paraConfig = {
      //   UserCount: '',//压测用户总数
      //   PerSecondUserCount: '',//每秒加载用户数
      //   Address: '',//被测服务器
      //   Port: '',//被测服务器端口
      //   Duration: '',//压测时间
      //   ResponseSeparator: '',//结束分隔符
      //   DataSourceVars: [],//数据源
      //   ConnectInit: {
      //     VarSettings: []
      //   },//连接初始化
      //   SendInit: {
      //     VarSettings: []
      //   },//发送初始化
      //   StopInit: {
      //     VarSettings: []
      //   }//停止初始化
      // }
    },
    //创建测试用例
    newCreate () {
      //判断当前选择的测试用例名称和引擎类型是否选择或填写
      if (this.Name && this.EngineType && this.MasterHostID) {
        //判断当前引擎类型是否是Jmeter
        if (this.EngineType != 'Jmeter') {
          this.Configuration = this.$refs.ParameterConfig.Configuration;
          let para = {
            Name: this.Name,
            Configuration: this.Configuration.trim(),
            EngineType: this.EngineType,
            MasterHostID: this.MasterHostID,
            FolderID: this.ChangeFileDirectoryId,
          }
          console.log(para)
          //判断基础参数是否选择
          if (this.isJSON(this.Configuration) && JSON.parse(this.Configuration).LocustMasterBindPort) {
            return para
          } else {
            if (!JSON.parse(this.Configuration).LocustMasterBindPort) {
              this.$q.notify({
                position: 'top',
                message: '提示',
                caption: '请填写并生成主机端口',
                color: 'red',
              })
            }
            return false;
          }
        } else {
          //Jmeter
          let para = {
            Name: this.Name,
            Configuration: JSON.stringify({
              "FileContent": this.$refs.Jmeter.xmlStr,
              "DataSourceVars": this.$refs.Jmeter.DataSourceVars
            }, null, 2),
            EngineType: this.EngineType,
            MasterHostID: this.MasterHostID,
            FolderID: this.ChangeFileDirectoryId,
          }
          if (!this.JmeterDataSource()) {
            console.log(para)
            return para;
          }
        }
      } else {
        if (this.Name == '') {
          this.$q.notify({
            position: 'top',
            message: '提示',
            caption: '请填写名称',
            color: 'red',
          })
        } else if (this.EngineType == '') {
          this.$q.notify({
            position: 'top',
            message: '提示',
            caption: '请填选择引擎类型',
            color: 'red',
          })
        } else if (this.MasterHostID == '') {
          this.$q.notify({
            position: 'top',
            message: '提示',
            caption: '请选择主机',
            color: 'red',
          })
        }
        return false;
      }
    },
    // -------------------------------- 主机 ---------------------------
    //打开主机lookUP选择框
    masterHost () {
      this.HostFixed = true;
      this.$q.loading.show();
      this.getMasterHostList();
    },
    //获得主机列表
    getMasterHostList () {
      Apis.getMasterHostList().then((res) => {
        console.log(res)
        this.masterHostList = res.data;
        this.$q.loading.hide()
      })
    },
    //主机lookUP选择框添加按钮
    addMasterHost (value) {
      if (value == undefined) {
        return false;
      }
      this.masterHostSelect = this.masterHostList[value].address;
      this.MasterHostID = this.masterHostList[value].id;
      this.masterHostIndex = value;
      this.createFixed = true;
      this.HostFixed = false;
    },
    //取消主机lookUP选择框
    cancelMasterHost () {
      this.createFixed = true;
      this.HostFixed = false;
      this.$refs.lookUp.selectIndex = this.masterHostIndex;
    },


    //---------------------------------------------- 目录管理 -------------------------------------------
    //打开文件目录位置弹窗
    ChangeFileDirectory () {
      this.ChangeFileDirectoryFlag = true;
    },
    //选择目录位置   
    SelectDirectoryLocation () {
      if (this.$refs.TreeEntity.getDirectoryLocation().id) {
        this.getTreeEntityTreePath(this.$refs.TreeEntity.getDirectoryLocation().id);
      } else {
        this.ChangeFileDirectoryName = this.$refs.TreeEntity.getDirectoryLocation().label || '根目录 >';
      }
      this.ChangeFileDirectoryId = this.$refs.TreeEntity.getDirectoryLocation().id || null;
      this.ChangeFileDirectoryFlag = false;
    },
    //获得文件目录路径
    getTreeEntityTreePath (ID) {
      //ID 当前文件目录ID  
      this.$q.loading.show();
      let para = { id: ID };
      Apis.getTreeEntityTreePath(para).then((res) => {
        console.log(res)
        this.ChangeFileDirectoryName = `根目录 > ` + res.data.join(' > ');
        this.$q.loading.hide();
      })
    },


    //---------------------------------------------- 参数配置 -------------------------------------------
    //判断验证正则
    ifRegular (val) {
      this.$nextTick(() => {
        this.LocustMasterBindPort = val.replace(/[^\d]/g, "");
      })
    },
    isJSON (str) {
      if (typeof str == 'string') {
        try {
          var obj = JSON.parse(str);
          if (typeof obj == 'object' && obj) {
            if (str.substr(0, 1) == '{' && str.substr(-1) == '}') {
              return true;
            } else {
              this.$q.notify({
                position: 'top',
                message: '提示',
                caption: '配置不是正确的JSON格式',
                color: 'red',
              })
            }
          } else {
            this.$q.notify({
              position: 'top',
              message: '提示',
              caption: '配置不是正确的JSON格式',
              color: 'red',
            })
          }

        } catch (e) {
          this.$q.notify({
            position: 'top',
            message: '提示',
            caption: '配置不是正确的JSON格式',
            color: 'red',
          })
        }
      }
    },

    //---------------------------------------------- Jmeter -------------------------------------------
    //判断Jmeter数据文件的数据源名称是否选择
    JmeterDataSource () {
      let DataSourceVars = this.$refs.Jmeter.DataSourceVars;
      for (let i = 0; i < DataSourceVars.length; i++) {
        if (!DataSourceVars[i].DataSourceName) {
          this.$q.notify({
            position: 'top',
            message: '提示',
            caption: `${DataSourceVars[i].Name}的测试数据源没有选择`,
            color: 'red',
          })
          return true;
        }
      }
    },

  }
}
</script>
<style lang="scss" scoped>
.pointer {
  cursor: pointer;
}
</style>
<style lang="scss">
.new_input {
  width: 100%;
  padding: 10px 30px;
  .input_row {
    margin-bottom: 30px;
  }
}
.q-textarea .q-field__native {
  resize: none;
}
.MoveList-enter,
.MoveList-leave-to {
  opacity: 0;
  transform: translateY(80px);
}

.MoveList-enter-active,
.MoveList-leave-active {
  transition: all 0.6s ease;
}

.MoveList-move {
  transition: all 0.6s ease;
}
</style>