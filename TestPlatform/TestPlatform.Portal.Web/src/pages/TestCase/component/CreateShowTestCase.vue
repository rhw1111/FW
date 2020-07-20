<template>
  <div>
    <!--  主机选择框  -->
    <lookUp :masterHostList="masterHostList"
            :masterSelectIndex="masterHostIndex"
            :fixed="HostFixed"
            @addMasterHost='addMasterHost'
            @cancelMasterHost='cancelMasterHost'
            ref='lookUp' />
    <div class="new_input">
      <div class="row input_row">
        <q-input v-model="Name"
                 :dense="false"
                 class="col">
          <template v-slot:before>
            <span style="font-size:14px">名称:</span>
          </template>
        </q-input>
        <q-select v-model="EngineType"
                  :options="['Http','Tcp']"
                  class="col"
                  :dense="false">
          <template v-slot:before>
            <span style="font-size:14px">引擎类型:</span>
          </template>
          <template v-slot:prepend>
          </template>
        </q-select>

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

      </div>
      <span style="font-size:14px">参数配置:</span>
      <div class="row"
           style="margin-bottom:10px;">
        <!-- 压测用户总数 -->
        <q-input v-model="paraConfig.UserCount"
                 @input="ifRegular('UserCount',paraConfig.UserCount)"
                 maxlength='6'
                 type="text"
                 filled
                 bottom-slots
                 class="col"
                 :dense="true">
          <template v-slot:before>
            <span style="font-size:14px;width:100px">压测用户总数:</span>
          </template>
          <template v-slot:append>
            <span style="font-size:14px">个</span>
          </template>
        </q-input>
        <!-- 每秒加载用户数 -->
        <q-input v-model="paraConfig.PerSecondUserCount"
                 @input="ifRegular('PerSecondUserCount',paraConfig.PerSecondUserCount)"
                 maxlength='6'
                 filled
                 bottom-slots
                 class="col"
                 :dense="true">
          <template v-slot:before>
            <span style="font-size:14px;width:105px;margin-left:10px;">每秒加载用户数:</span>
          </template>
          <template v-slot:append>
            <span style="font-size:14px">个/秒</span>
          </template>
        </q-input>

        <!-- 压测时间 -->
        <q-input v-model="paraConfig.Duration"
                 @input="ifRegular('Duration',paraConfig.Duration)"
                 maxlength='6'
                 filled
                 bottom-slots
                 class="col"
                 :dense="true">
          <template v-slot:before>
            <span style="font-size:14px;width:100px;margin-left:10px;">压测时间:</span>
          </template>
          <template v-slot:append>
            <span style="font-size:14px">秒</span>
          </template>
        </q-input>
      </div>
      <div class="row"
           style="margin-bottom:10px;">
        <!-- 被测服务器 -->
        <q-input filled
                 bottom-slots
                 v-model="paraConfig.Address"
                 class="col"
                 :dense="true"
                 placeholder="请输入ipv4地址">
          <template v-slot:before>
            <span style="font-size:14px;width:100px;">被测服务器:</span>
          </template>
          <template v-slot:append>
            <span style="font-size:14px">ip地址</span>
          </template>
        </q-input>
        <!-- 被测服务器端口 -->
        <q-input v-model="paraConfig.Port"
                 @input="ifRegular('Port',paraConfig.Port)"
                 maxlength='5'
                 filled
                 bottom-slots
                 class="col"
                 :dense="true"
                 placeholder="范围0-65535之间">
          <template v-slot:before>
            <span style="font-size:14px;width:105px;margin-left:10px;">被测服务器端口:</span>
          </template>
          <template v-slot:append>
            <span style="font-size:14px;">端口</span>
          </template>
        </q-input>
        <!-- 结束分隔符 -->
        <q-input v-model="paraConfig.ResponseSeparator"
                 filled
                 bottom-slots
                 class="col"
                 :dense="true">
          <template v-slot:before>
            <span style="font-size:14px;width:105px;margin-left:10px;">结束分隔符:</span>
          </template>
        </q-input>
      </div>
      <q-list bordered
              class="rounded-borders">
        <!-- 数据源 -->
        <q-expansion-item label="数据源"
                          style="text-align:left;position:relative"
                          expand-icon-toggle
                          expand-separator
                          v-model="DataSourceExpanded">
          <template v-slot:header>
            <q-item-section>
              数据源
            </q-item-section>
            <q-item-section side>
              <div class="row items-center">
                <q-btn class="btn "
                       color="primary"
                       style="margin:0px 0px 0px 20px;"
                       label="添加数据源参数"
                       @click="addDataVars('DataSource')" />
              </div>
            </q-item-section>
          </template>
          <q-card>
            <q-card-section v-show="paraConfig.DataSourceVars.length==0">暂无参数配置，请点击添加数据源参数按钮进行添加。</q-card-section>
            <q-card-section v-for="(val,ind) in paraConfig.DataSourceVars"
                            :key="ind">
              <span style="font-size:14px">参数配置{{ind+1}}:</span>
              <div class="row">
                <q-input v-model="paraConfig.DataSourceVars[ind].Name"
                         filled
                         class="col-5"
                         :dense="true">
                  <template v-slot:before>
                    <span style="font-size:14px;width:100px;">名称:</span>
                  </template>
                </q-input>
                <q-select v-model="paraConfig.DataSourceVars[ind].DataSourceName"
                          :options="dataSource"
                          class="col-5"
                          :dense="true">
                  <template v-slot:before>
                    <span style="font-size:14px;margin-left:10px;width:80px;">数据源名称:</span>
                  </template>
                  <template v-slot:prepend>
                  </template>
                </q-select>
                <div class="col-2">
                  <q-btn class="btn"
                         style="background: #FF0000; color: white;margin-left:20px;"
                         label="删 除"
                         @click="deleteDataVars('DataSource',ind)" />
                </div>
              </div>
            </q-card-section>

          </q-card>
        </q-expansion-item>
        <!-- 连接初始化 -->
        <q-expansion-item label="连接初始化"
                          style="text-align:left;position:relative"
                          expand-icon-toggle
                          expand-separator
                          v-model="ConnectInitExpanded">
          <template v-slot:header>
            <q-item-section>
              连接初始化
            </q-item-section>
            <q-item-section side>
              <div class="row items-center">
                <q-btn class="btn "
                       color="primary"
                       style="margin:0px 0px 0px 20px;"
                       label="添加初始化参数"
                       @click="addDataVars('ConnectInit')" />
              </div>
            </q-item-section>
          </template>
          <q-card>
            <q-card-section v-show="paraConfig.ConnectInit.VarSettings.length==0">暂无参数配置，请点击添加初始化参数按钮进行添加。</q-card-section>
            <q-card-section v-for="(val,ind) in paraConfig.ConnectInit.VarSettings"
                            :key="ind">
              <span style="font-size:14px">参数配置{{ind+1}}:</span>
              <div class="row">
                <q-input v-model="paraConfig.ConnectInit.VarSettings[ind].Name"
                         filled
                         class="col-5"
                         :dense="true">
                  <template v-slot:before>
                    <span style="font-size:14px;width:100px">名称:</span>
                  </template>
                </q-input>
                <q-input v-model="paraConfig.ConnectInit.VarSettings[ind].Content"
                         filled
                         class="col-5"
                         :dense="true">
                  <template v-slot:before>
                    <span style="font-size:14px;width:80px;margin-left:10px;">内容:</span>
                  </template>
                </q-input>
                <div class="col-2">
                  <q-btn class="btn"
                         style="background: #FF0000; color: white;margin-left:20px;"
                         label="删 除"
                         @click="deleteDataVars('ConnectInit',ind)" />
                </div>
              </div>
            </q-card-section>

          </q-card>
        </q-expansion-item>
        <!-- 发送初始化 -->
        <q-expansion-item label="发送初始化"
                          style="text-align:left;position:relative"
                          expand-icon-toggle
                          expand-separator
                          v-model="SendInitExpanded">
          <template v-slot:header>
            <q-item-section>
              发送初始化
            </q-item-section>
            <q-item-section side>
              <div class="row items-center">
                <q-btn class="btn "
                       color="primary"
                       style="margin:0px 0px 0px 20px;"
                       label="添加初始化参数"
                       @click="addDataVars('SendInit')" />
              </div>
            </q-item-section>
          </template>
          <q-card>
            <q-card-section v-show="paraConfig.SendInit.VarSettings.length==0">暂无参数配置，请点击添加初始化参数按钮进行添加。</q-card-section>
            <q-card-section v-for="(val,ind) in paraConfig.SendInit.VarSettings"
                            :key="ind">
              <span style="font-size:14px">参数配置{{ind+1}}:</span>
              <div class="row">
                <q-input v-model="paraConfig.SendInit.VarSettings[ind].Name"
                         filled
                         class="col-5"
                         :dense="true">
                  <template v-slot:before>
                    <span style="font-size:14px;width:100px">名称:</span>
                  </template>
                </q-input>
                <q-input v-model="paraConfig.SendInit.VarSettings[ind].Content"
                         filled
                         class="col-5"
                         :dense="true">
                  <template v-slot:before>
                    <span style="font-size:14px;width:80px;margin-left:10px;">内容:</span>
                  </template>
                </q-input>
                <div class="col-2">
                  <q-btn class="btn"
                         style="background: #FF0000; color: white;margin-left:20px;"
                         label="删 除"
                         @click="deleteDataVars('SendInit',ind)" />
                </div>
              </div>
            </q-card-section>

          </q-card>
        </q-expansion-item>
        <!-- 停止初始化 -->
        <q-expansion-item label="停止初始化"
                          style="text-align:left;position:relative"
                          expand-icon-toggle
                          expand-separator
                          v-model="StopInitExpanded">
          <template v-slot:header>
            <q-item-section>
              停止初始化
            </q-item-section>
            <q-item-section side>
              <div class="row items-center">
                <q-btn class="btn "
                       color="primary"
                       style="margin:0px 0px 0px 20px;"
                       label="添加初始化参数"
                       @click="addDataVars('StopInit')" />
              </div>
            </q-item-section>
          </template>
          <q-card>
            <q-card-section v-show="paraConfig.StopInit.VarSettings.length==0">暂无参数配置，请点击添加初始化参数按钮进行添加。</q-card-section>
            <q-card-section v-for="(val,ind) in paraConfig.StopInit.VarSettings"
                            :key="ind">
              <span style="font-size:14px">参数配置{{ind+1}}:</span>
              <div class="row">
                <q-input v-model="paraConfig.StopInit.VarSettings[ind].Name"
                         filled
                         class="col-5"
                         :dense="true">
                  <template v-slot:before>
                    <span style="font-size:14px;width:100px">名称:</span>
                  </template>
                </q-input>
                <q-input v-model="paraConfig.StopInit.VarSettings[ind].Content"
                         filled
                         class="col-5"
                         :dense="true">
                  <template v-slot:before>
                    <span style="font-size:14px;width:80px;margin-left:10px;">内容:</span>
                  </template>
                </q-input>
                <div class="col-2">
                  <q-btn class="btn"
                         style="background: #FF0000; color: white;margin-left:20px;"
                         label="删 除"
                         @click="deleteDataVars('StopInit',ind)" />
                </div>
              </div>
            </q-card-section>

          </q-card>
        </q-expansion-item>
      </q-list>
    </div>
  </div>
</template>

<script>
import lookUp from "@/components/lookUp.vue"
export default {
  name: 'CreateShowTestCase',
  props: ['masterHostList', 'dataSourceName', 'detailData'],
  components: {
    lookUp
  },
  watch: {
    detailData (val) {
      if (val) {
        console.log(val)
        this.Name = val.name;
        this.paraConfig = JSON.parse(val.configuration);
        this.EngineType = val.engineType;

        this.masterHostSelect = val.masterHostAddress;
        this.MasterHostID = val.masterHostID;
      }
    },
    dataSourceName (val) {
      console.log(val)
      if (val) {
        for (let i = 0; i < val.length; i++) {
          this.dataSource.push(val[i].name)
        }
      }
    }
  },
  mounted () {
    for (let i = 0; i < this.dataSourceName.length; i++) {
      this.dataSource.push(this.dataSourceName[i].name)
    }
  },
  data () {
    return {

      HostFixed: false,     //主机flag
      masterHostSelect: '', //主机选择
      masterHostIndex: -1,//主机下标

      dataSource: [],//数据源名称数组

      Name: '',           //Name
      Configuration: '',  //Configuration
      EngineType: '',     //EngineType
      MasterHostID: '',   //MasterHostID 主机ID

      //Configuration参数配置
      paraConfig: {
        UserCount: '',//压测用户总数
        PerSecondUserCount: '',//每秒加载用户数
        Address: '',//被测服务器
        Port: '',//被测服务器端口
        Duration: '',//压测时间
        ResponseSeparator: '',//结束分隔符
        DataSourceVars: [],//数据源
        ConnectInit: {
          VarSettings: []
        },//连接初始化
        SendInit: {
          VarSettings: []
        },//发送初始化
        StopInit: {
          VarSettings: []
        }//停止初始化
      },

      DataSourceExpanded: false, //DataSourceVars 扩展框flag
      ConnectInitExpanded: false,//ConnectInit扩展框flag
      SendInitExpanded: false,//SendInit扩展框flag
      StopInitExpanded: false,//StopInit
    }
  },
  methods: {
    //打开主机lookUP选择框
    masterHost () {
      this.HostFixed = true;
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
    //新增弹窗取消按钮
    newCancel () {
      this.Name = '';
      this.Configuration = '';
      this.EngineType = '';
      this.MasterHostID = '';
      this.masterHostSelect = '';
      this.$refs.lookUp.selectIndex = -1;

      this.paraConfig = {
        UserCount: '',//压测用户总数
        PerSecondUserCount: '',//每秒加载用户数
        Address: '',//被测服务器
        Port: '',//被测服务器端口
        Duration: '',//压测时间
        ResponseSeparator: '',//结束分隔符
        DataSourceVars: [],//数据源
        ConnectInit: {
          VarSettings: []
        },//连接初始化
        SendInit: {
          VarSettings: []
        },//发送初始化
        StopInit: {
          VarSettings: []
        }//停止初始化
      }
    },
    newCreate () {
      let para = {
        Name: this.Name,
        Configuration: JSON.stringify(this.paraConfig).trim(),
        EngineType: this.EngineType,
        MasterHostID: this.MasterHostID
      }
      if (this.Name && this.EngineType && this.MasterHostID) {
        //验证ip地址是否正确
        if (!this.isValidIp(this.paraConfig.Address)) { return false; }
        //验证端口号是否正确
        if (!this.isPort(this.paraConfig.Port)) { return false; }
        return para
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
    // ---------------------------------------- 参数配置 ------------------------------------ 
    //生成JSON
    CreateJson () {
      if (this.Configuration.trim() == '') {
        //验证ip地址是否正确
        if (!this.isValidIp(this.paraConfig.Address)) { return; }
        //验证端口号是否正确
        if (!this.isPort(this.paraConfig.Port)) { return; }
        this.Configuration = JSON.stringify(this.paraConfig, null, 2);
      } else if (this.isJSON(this.Configuration.trim())) {
        //验证ip地址是否正确
        if (!this.isValidIp(this.paraConfig.Address)) { return; }
        //验证端口号是否正确
        if (!this.isPort(this.paraConfig.Port)) { return; }

        this.Configuration = JSON.parse(this.Configuration);
        this.Configuration.UserCount = this.paraConfig.UserCount;
        this.Configuration.PerSecondUserCount = this.paraConfig.PerSecondUserCount;
        this.Configuration.Address = this.paraConfig.Address;
        this.Configuration.Port = this.paraConfig.Port;
        this.Configuration.Duration = this.paraConfig.Duration;
        this.Configuration = JSON.stringify(this.Configuration, null, 2);
      }
    },
    //判断是否是JSON格式
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
    //判断ip地址是否正确
    isValidIp (val) {
      console.log(val)
      if (val != '') {
        if (!/^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$/.test(val)) {
          this.$q.notify({
            position: 'top',
            message: '提示',
            caption: '被测服务器ip地址不正确',
            color: 'red',
          })
          return false;
        } else {
          return true;
        }
      } else { return true; }
    },
    //判断端口号是否正确
    isPort (val) {
      if (val != '') {
        if (!(/^[1-9]\d*$/.test(val) && 1 <= 1 * val && 1 * val <= 65535)) {
          this.$q.notify({
            position: 'top',
            message: '提示',
            caption: '被测服务器端口不符合范围：0-65535',
            color: 'red',
          })
          return false;
        } else {
          return true;
        }
      } else { return true; }
    },
    //判断验证正则
    ifRegular (key, val) {
      this.$nextTick(() => {
        if (key == 'Port') {
          let str = val.replace(/[^\d]/g, "");
          this.$set(this.paraConfig, key, str > 65535 ? 65535 : str)
        } else {
          this.$set(this.paraConfig, key, val.replace(/[^\d]/g, "").replace(/^0/g, ""))
        }
      })
    },

    //增加数据DataVars 
    addDataVars (val) {
      if (val == 'DataSource') {             //数据源
        this.paraConfig.DataSourceVars.push({
          "Name": "",
          "Type": "",
          "DataSourceName": "",
          "Data": ""
        })
        this.DataSourceExpanded = true;
      } else if (val == 'ConnectInit') {     //连接初始化
        this.paraConfig.ConnectInit.VarSettings.push({
          "Name": "",
          "Content": ""
        })
        this.ConnectInitExpanded = true;
      } else if (val == 'SendInit') {        //发送初始化
        this.paraConfig.SendInit.VarSettings.push({
          "Name": "",
          "Content": ""
        })
        this.SendInitExpanded = true;
      } else if (val == 'StopInit') {            //停止初始化
        this.paraConfig.StopInit.VarSettings.push({
          "Name": "",
          "Content": ""
        })
        this.StopInitExpanded = true;
      }


    },
    //删除数据DataVars
    deleteDataVars (val, index) {
      this.$q.dialog({
        title: '提示',
        message: `您确定要删除当前参数配置${index + 1}吗`,
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
        if (val == 'DataSource') {            //数据源
          this.paraConfig.DataSourceVars.splice(index, 1);
        } else if (val == 'ConnectInit') {    //连接初始化
          this.paraConfig.ConnectInit.VarSettings.splice(index, 1);
        } else if (val == 'SendInit') {       //发送初始化
          this.paraConfig.SendInit.VarSettings.splice(index, 1);
        } else if (val == 'StopInit') {       //停止初始化
          this.paraConfig.StopInit.VarSettings.splice(index, 1);
        }
      })
    }
  }
}
</script>

<style lang="scss">
.new_input {
  width: 100%;
  padding: 10px 30px;
  .input_row {
    margin-bottom: 30px;
  }
}
</style>