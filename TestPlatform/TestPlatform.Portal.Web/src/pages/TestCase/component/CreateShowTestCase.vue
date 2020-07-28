/* eslint-disable vue/valid-v-for */
/* eslint-disable vue/valid-v-for */
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
                 placeholder="请输入被测服务器地址">
          <template v-slot:before>
            <span style="font-size:14px;width:100px;">被测服务器:</span>
          </template>
          <template v-slot:append>
            <span style="font-size:14px">地址</span>
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
      <div class="row"
           style="margin-bottom:10px;">
        <q-select v-model="paraConfig.IsPrintLog"
                  :options="PrintLogOptions"
                  class="col-4"
                  emit-value
                  map-options
                  :dense="false">
          <template v-slot:before>
            <span style="font-size:14px">是否打印日志:</span>
          </template>
          <template v-slot:prepend>
          </template>
        </q-select>

        <q-select v-model="paraConfig.SyncType"
                  :options="SyncTypeOptions"
                  class="col-4"
                  emit-value
                  map-options
                  :dense="false"
                  style="margin-left:10px;">
          <template v-slot:before>
            <span style="font-size:14px">同步类型:</span>
          </template>
          <template v-slot:prepend>
          </template>
        </q-select>

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
            <transition-group name="MoveList">
              <q-card-section v-for="(val,ind) in paraConfig.DataSourceVars"
                              :key="ind">
                <span style="font-size:14px">参数{{ind+1}}:</span>
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
                  <div class="col-2 row">
                    <div class="col-1"
                         style="margin-left:20px;">
                      <q-icon name="ion-arrow-up"
                              style="display:block;"
                              class="pointer"
                              @click="moveUpList('DataSourceVars',ind)" />
                      <q-icon name="ion-arrow-down"
                              class="pointer"
                              style="display:block;margin-top:10px;"
                              @click="moveDownList('DataSourceVars',ind)" />
                    </div>

                    <q-btn class="btn col-4"
                           style="background: #FF0000; color: white;margin-left:20px;display:inline-block;"
                           label="删 除"
                           @click="deleteDataVars('DataSource',ind)" />
                  </div>
                </div>
              </q-card-section>
            </transition-group>

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

            <transition-group name="MoveList">
              <q-card-section v-for="(val,ind) in paraConfig.ConnectInit.VarSettings"
                              :key="ind">
                <span style="font-size:14px">参数{{ind+1}}:</span>
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
                  <div class="col-2 row">
                    <div class="col-1"
                         style="margin-left:20px;">
                      <q-icon name="ion-arrow-up"
                              style="display:block;"
                              class="pointer"
                              @click="moveUpList('ConnectInit',ind)" />
                      <q-icon name="ion-arrow-down"
                              class="pointer"
                              style="display:block;margin-top:10px;"
                              @click="moveDownList('ConnectInit',ind)" />
                    </div>

                    <q-btn class="btn col-4"
                           style="background: #FF0000; color: white;margin-left:20px;display:inline-block;"
                           label="删 除"
                           @click="deleteDataVars('ConnectInit',ind)" />
                  </div>
                </div>
              </q-card-section>
            </transition-group>

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

            <transition-group name="MoveList">
              <q-card-section v-for="(val,ind) in paraConfig.SendInit.VarSettings"
                              :key="ind">
                <span style="font-size:14px">参数{{ind+1}}:</span>
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
                  <div class="col-2 row">
                    <div class="col-1"
                         style="margin-left:20px;">
                      <q-icon name="ion-arrow-up"
                              class="pointer"
                              style="display:block;"
                              @click="moveUpList('SendInit',ind)" />
                      <q-icon name="ion-arrow-down"
                              class="pointer"
                              style="display:block;margin-top:10px;"
                              @click="moveDownList('SendInit',ind)" />
                    </div>

                    <q-btn class="btn col-4"
                           style="background: #FF0000; color: white;margin-left:20px;display:inline-block;"
                           label="删 除"
                           @click="deleteDataVars('SendInit',ind)" />
                  </div>
                </div>
              </q-card-section>
            </transition-group>

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

            <transition-group name="MoveList">
              <q-card-section v-for="(val,ind) in paraConfig.StopInit.VarSettings"
                              :key="ind">
                <span style="font-size:14px">参数{{ind+1}}:</span>
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
                  <div class="col-2 row">
                    <div class="col-1"
                         style="margin-left:20px;">
                      <q-icon name="ion-arrow-up"
                              class="pointer"
                              style="display:block;"
                              @click="moveUpList('StopInit',ind)" />
                      <q-icon name="ion-arrow-down"
                              class="pointer"
                              style="display:block;margin-top:10px;"
                              @click="moveDownList('StopInit',ind)" />
                    </div>

                    <q-btn class="btn col-4"
                           style="background: #FF0000; color: white;margin-left:20px;display:inline-block;"
                           label="删 除"
                           @click="deleteDataVars('StopInit',ind)" />
                  </div>
                </div>
              </q-card-section>
            </transition-group>

          </q-card>
        </q-expansion-item>
      </q-list>
      <!-- <div class="row">
        <div class="col-2  offset-md-10">

          <q-btn class="btn"
                 color="primary"
                 style="margin:10px 0 10px 20px;float:right"
                 label="生 成"
                 @click="CreateJson" />
        </div>
      </div> -->

      <q-list bordered>
        <q-expansion-item label="数据源"
                          style="text-align:left;position:relative"
                          expand-icon-toggle
                          expand-separator
                          v-model="ConfigTextExpanded">
          <template v-slot:header>
            <q-item-section>
              配置文本:
            </q-item-section>
            <q-item-section side>
              <q-btn class="btn tag"
                     color="primary"
                     style="margin:10px 0 10px 20px;float:right"
                     label="复 制"
                     @click="CopyPageText(Configuration)" />
            </q-item-section>
            <q-item-section side>
              <q-btn class="btn"
                     color="primary"
                     style="margin:10px 0 10px 20px;float:right"
                     label="生 成"
                     @click="CreateJson" />
            </q-item-section>
          </template>
          <q-card>
            <div class="row input_row">
              <q-input v-model="Configuration"
                       :dense="false"
                       style="overflow:hidden"
                       autogrow
                       class="col-12"
                       type="textarea"
                       outlined>
              </q-input>
            </div>
          </q-card>
        </q-expansion-item>
      </q-list>

      <!-- <div class="row input_row">
        <q-input v-model="Configuration"
                 :dense="false"
                 style="overflow:hidden"
                 autogrow
                 class="col-12"
                 type="textarea"
                 outlined>
        </q-input>
      </div> -->

    </div>
  </div>
</template>

<script>
import * as Apis from "@/api/index"
import lookUp from "@/components/lookUp.vue"
import Clipboard from 'clipboard';
export default {
  name: 'CreateShowTestCase',
  props: ['dataSourceName', 'detailData'],
  components: {
    lookUp
  },
  watch: {
    detailData (val) {
      if (val) {
        console.log(val)
        let configuration = JSON.parse(val.configuration)
        this.paraConfig = {
          UserCount: configuration.UserCount || '',//压测用户总数
          PerSecondUserCount: configuration.PerSecondUserCount || '',//每秒加载用户数
          Address: configuration.Address || '',//被测服务器
          Port: configuration.Port || '',//被测服务器端口
          Duration: configuration.Duration || '',//压测时间
          ResponseSeparator: configuration.ResponseSeparator || '',//结束分隔符
          DataSourceVars: configuration.DataSourceVars || [],//数据源
          IsPrintLog: configuration.IsPrintLog == true ? '是' : '否',//是否打印日志
          SyncType: configuration.SyncType == false ? '异步模式' : '同步模式',//是否同步异步
          ConnectInit: configuration.ConnectInit || { VarSettings: [] },//连接初始化
          SendInit: configuration.SendInit || { VarSettings: [] },//发送初始化
          StopInit: configuration.StopInit || { VarSettings: [] }//停止初始化
        }

        this.Name = val.name;
        this.EngineType = val.engineType;
        this.masterHostSelect = val.masterHostAddress;
        this.MasterHostID = val.masterHostID;
        this.Configuration = JSON.stringify(JSON.parse(val.configuration), null, 2);
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
      masterHostList: [],//主机列表

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
        IsPrintLog: '否',//是否打印日志
        SyncType: '同步模式',//是否同步异步
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
      //是否打印日志
      PrintLogOptions: [
        {
          label: '是',
          value: true,
        },
        {
          label: '否',
          value: false,
        }
      ],
      SyncTypeOptions: [
        {
          label: '同步模式',
          value: true,
        },
        {
          label: '异步模式',
          value: false,
        }
      ],
      DataSourceExpanded: false, //DataSourceVars 扩展框flag
      ConnectInitExpanded: false,//ConnectInit扩展框flag
      SendInitExpanded: false,//SendInit扩展框flag
      StopInitExpanded: false,//StopInit
      ConfigTextExpanded: false,//配置文本
    }
  },
  methods: {
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
        Configuration: this.Configuration.trim(),
        EngineType: this.EngineType,
        MasterHostID: this.MasterHostID
      }
      console.log(para)
      if (this.Name && this.isJSON(this.Configuration) && this.EngineType && this.MasterHostID) {
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
    //复制文本
    CopyPageText (data) {
      if (data.trim() == '') {
        this.$q.notify({
          position: 'top',
          message: '提示',
          caption: '当前配置文本为空',
          color: 'red',
        })
        return;
      }
      let clipboard = new Clipboard('.tag', {
        text: function () {
          return data
        }
      })
      clipboard.on('success', () => {
        this.$q.notify({
          position: 'top',
          message: '提示',
          caption: '复制成功',
          color: 'secondary',
        })
        // 释放内存
        clipboard.destroy()
      })
      clipboard.on('error', () => {
        this.$q.notify({
          position: 'top',
          message: '提示',
          caption: '复制失败，当前浏览器不支持复制。',
          color: 'red',
        })
        clipboard.destroy()
      })
    },
    //生成JSON
    CreateJson () {
      if (this.Configuration.trim() == '') {
        //验证ip地址是否正确
        //if (!this.isValidIp(this.paraConfig.Address)) { return; }
        //验证端口号是否正确
        if (!this.isPort(this.paraConfig.Port)) { return; }
        if (!this.ifDataVars()) { return; }
        this.Configuration = JSON.stringify({
          UserCount: this.paraConfig.UserCount ? Number(this.paraConfig.UserCount) : '',//压测用户总数
          PerSecondUserCount: this.paraConfig.PerSecondUserCount ? Number(this.paraConfig.PerSecondUserCount) : '',//每秒加载用户数
          Address: this.paraConfig.Address,//被测服务器
          Port: this.paraConfig.Port ? Number(this.paraConfig.Port) : '',//被测服务器端口
          Duration: this.paraConfig.Duration ? Number(this.paraConfig.Duration) : '',//压测时间
          ResponseSeparator: this.paraConfig.ResponseSeparator,//结束分隔符
          DataSourceVars: this.paraConfig.DataSourceVars,//数据源
          IsPrintLog: this.paraConfig.IsPrintLog == true || this.paraConfig.IsPrintLog == '是' ? true : false,//是否打印日志
          SyncType: this.paraConfig.SyncType == true || this.paraConfig.SyncType == '同步模式' ? true : false,//是否同步异步
          ConnectInit: {
            VarSettings: this.paraConfig.ConnectInit.VarSettings
          },//连接初始化
          SendInit: {
            VarSettings: this.paraConfig.SendInit.VarSettings
          },//发送初始化
          StopInit: {
            VarSettings: this.paraConfig.StopInit.VarSettings
          }//停止初始化
        }, null, 2);
        this.ConfigTextExpanded = true;
      } else if (this.isJSON(this.Configuration.trim())) {
        //验证ip地址是否正确
        //if (!this.isValidIp(this.paraConfig.Address)) { return; }
        //验证端口号是否正确
        if (!this.isPort(this.paraConfig.Port)) { return; }
        if (!this.ifDataVars()) { return; }
        this.Configuration = JSON.parse(this.Configuration);
        this.Configuration.UserCount = this.paraConfig.UserCount ? Number(this.paraConfig.UserCount) : '';
        this.Configuration.PerSecondUserCount = this.paraConfig.PerSecondUserCount ? Number(this.paraConfig.PerSecondUserCount) : '';
        this.Configuration.Address = this.paraConfig.Address;
        this.Configuration.Port = this.paraConfig.Port ? Number(this.paraConfig.Port) : '';
        this.Configuration.Duration = this.paraConfig.Duration ? Number(this.paraConfig.Duration) : '';
        this.Configuration.ResponseSeparator = this.paraConfig.ResponseSeparator;
        this.Configuration.DataSourceVars = this.paraConfig.DataSourceVars;
        this.Configuration.IsPrintLog = this.paraConfig.IsPrintLog == true || this.paraConfig.IsPrintLog == '是' ? true : false;
        this.Configuration.SyncType = this.paraConfig.SyncType == true || this.paraConfig.SyncType == '同步模式' ? true : false;
        this.Configuration.ConnectInit.VarSettings = this.paraConfig.ConnectInit.VarSettings;
        this.Configuration.SendInit.VarSettings = this.paraConfig.SendInit.VarSettings;
        this.Configuration.StopInit.VarSettings = this.paraConfig.StopInit.VarSettings;
        this.Configuration = JSON.stringify(this.Configuration, null, 2);
        this.ConfigTextExpanded = true;
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
    //判断数据源数据是否正确
    ifDataVars () {
      //数据源数组 
      for (let i = 0; i < this.paraConfig.DataSourceVars.length; i++) {
        let DataSourceVars = this.paraConfig.DataSourceVars[i];
        console.log(DataSourceVars)
        if (DataSourceVars.Name == '' || DataSourceVars.DataSourceName == '') {
          this.$q.notify({
            position: 'top',
            message: '提示',
            caption: `数据源参数${i + 1}名称和数据源名称为必填`,
            color: 'red',
          })
          return false;
        }
      }
      //发送初始化数组
      for (let i = 0; i < this.paraConfig.ConnectInit.VarSettings.length; i++) {
        let ConnectVarSettings = this.paraConfig.ConnectInit.VarSettings[i];
        console.log(ConnectVarSettings)
        if (ConnectVarSettings.Content == '') {
          this.$q.notify({
            position: 'top',
            message: '提示',
            caption: `连接初始化参数${i + 1}内容为必填`,
            color: 'red',
          })
          return false;
        }
      }
      //发送初始化
      for (let i = 0; i < this.paraConfig.SendInit.VarSettings.length; i++) {
        let SendVarSettings = this.paraConfig.SendInit.VarSettings[i];
        console.log(SendVarSettings)
        if (SendVarSettings.Content == '') {
          this.$q.notify({
            position: 'top',
            message: '提示',
            caption: `发送初始化参数${i + 1}内容为必填`,
            color: 'red',
          })
          return false;
        }
      }
      //暂停初始化
      for (let i = 0; i < this.paraConfig.StopInit.VarSettings.length; i++) {
        let StopVarSettings = this.paraConfig.StopInit.VarSettings[i];
        console.log(StopVarSettings)
        if (StopVarSettings.Content == '') {
          this.$q.notify({
            position: 'top',
            message: '提示',
            caption: `停止初始化参数${i + 1}内容为必填`,
            color: 'red',
          })
          return false;
        }
      }
      return true;
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
        message: `您确定要删除当前参数${index + 1}吗`,
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
    },
    //上移列表
    moveUpList (value, index) {
      if (value == 'DataSourceVars' && index != 0) {
        this.paraConfig.DataSourceVars[index] = this.paraConfig.DataSourceVars.splice(index - 1, 1, this.paraConfig.DataSourceVars[index])[0];
      } else if (value == "ConnectInit" && index != 0) {
        this.paraConfig.ConnectInit.VarSettings[index] = this.paraConfig.ConnectInit.VarSettings.splice(index - 1, 1, this.paraConfig.ConnectInit.VarSettings[index])[0];
      } else if (value == "SendInit" && index != 0) {
        this.paraConfig.SendInit.VarSettings[index] = this.paraConfig.SendInit.VarSettings.splice(index - 1, 1, this.paraConfig.SendInit.VarSettings[index])[0];
      } else if (value == "StopInit" && index != 0) {
        this.paraConfig.StopInit.VarSettings[index] = this.paraConfig.StopInit.VarSettings.splice(index - 1, 1, this.paraConfig.StopInit.VarSettings[index])[0];
      }
    },
    //下移列表
    moveDownList (value, index) {
      if (value == 'DataSourceVars' && index < this.paraConfig.DataSourceVars.length - 1) {

        this.paraConfig.DataSourceVars[index] = this.paraConfig.DataSourceVars.splice(index + 1, 1, this.paraConfig.DataSourceVars[index])[0];

      } else if (value == "ConnectInit" && index < this.paraConfig.ConnectInit.VarSettings.length - 1) {

        this.paraConfig.ConnectInit.VarSettings[index] = this.paraConfig.ConnectInit.VarSettings.splice(index + 1, 1, this.paraConfig.ConnectInit.VarSettings[index])[0];

      } else if (value == "SendInit" && index < this.paraConfig.SendInit.VarSettings.length - 1) {

        this.paraConfig.SendInit.VarSettings[index] = this.paraConfig.SendInit.VarSettings.splice(index + 1, 1, this.paraConfig.SendInit.VarSettings[index])[0];

      } else if (value == "StopInit" && index < this.paraConfig.StopInit.VarSettings.length - 1) {

        this.paraConfig.StopInit.VarSettings[index] = this.paraConfig.StopInit.VarSettings.splice(index + 1, 1, this.paraConfig.StopInit.VarSettings[index])[0];

      }
    }
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