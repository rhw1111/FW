<template>
  <div class="TestCase">
    <!-- TestCase列表 -->
    <div class="q-pa-md">

      <q-table title="测试用例列表"
               :data="TestCaseList"
               :columns="columns"
               row-key="id"
               :rows-per-page-options=[0]
               no-data-label="暂无数据更新">

        <template v-slot:top-right>
          <q-btn class="btn"
                 color="primary"
                 label="新 增"
                 @click="openCrateTestCase" />
        </template>
        <template v-slot:bottom
                  class="row">
          <q-pagination v-model="pagination.page"
                        :max="pagination.rowsNumber"
                        :input="true"
                        class="col offset-md-10"
                        @input="nextPage">
          </q-pagination>
        </template>
        <template v-slot:body-cell-id="props">
          <q-td class=""
                :props="props">
            <q-btn class="btn"
                   color="primary"
                   label="更 新"
                   @click="toDetail(props)" />
            <q-btn class="btn"
                   style="background: #FF0000; color: white"
                   label="删 除"
                   @click="deleteTestCase(props)" />
          </q-td>
        </template>
      </q-table>
    </div>
    <!--  主机选择框  -->
    <lookUp :masterHostList="masterHostList"
            :masterSelectIndex="masterHostIndex"
            :fixed="HostFixed"
            @addMasterHost='addMasterHost'
            @cancelMasterHost='cancelMasterHost'
            ref='lookUp' />
    <!-- 新增TestCase框 -->
    <q-dialog v-model="createFixed"
              persistent>
      <q-card style="width:100%">
        <q-card-section>
          <div class="text-h6">创建测试用例</div>
        </q-card-section>

        <q-separator />
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
          </div>
          <div class="row input_row">
            <q-input :dense="false"
                     class="col col-xs-12"
                     readonly
                     v-model="masterHostSelect"
                     @dblclick="masterHost">
              <template v-slot:before>
                <span style="font-size:14px">主机:</span>
              </template>
            </q-input>
          </div>
          <span style="font-size:14px">参数配置:</span>
          <div class="row input_row">
            <!-- 压测用户总数 -->
            <q-input filled
                     bottom-slots
                     v-model="paraConfig.UserCount"
                     class="col"
                     :dense="false"
                     @keyup="paraConfig.UserCount=Number(paraConfig.UserCount.toString().replace(/[^\d]/g,''))">
              <template v-slot:before>
                <span style="font-size:14px;width:100px">压测用户总数:</span>
              </template>
              <template v-slot:append>
                <span style="font-size:14px">个</span>
              </template>
            </q-input>
            <!-- 每秒加载用户数 -->
            <q-input filled
                     bottom-slots
                     v-model="paraConfig.PerSecondUserCount"
                     class="col"
                     :dense="false"
                     @keyup="paraConfig.PerSecondUserCount=Number(paraConfig.PerSecondUserCount.toString().replace(/[^\d]/g,''))">
              <template v-slot:before>
                <span style="font-size:14px;width:105px;margin-left:10px;">每秒加载用户数:</span>
              </template>
              <template v-slot:append>
                <span style="font-size:14px">个/秒</span>
              </template>
            </q-input>
          </div>

          <div class="row input_row">
            <!-- 被测服务器 -->
            <q-input filled
                     bottom-slots
                     v-model="paraConfig.Address"
                     class="col"
                     :dense="false"
                     @keyup="paraConfig.Address=paraConfig.Address.replace(/[^\d^\.]+/g,'')">
              <template v-slot:before>
                <span style="font-size:14px;width:100px">被测服务器:</span>
              </template>
              <template v-slot:append>
                <span style="font-size:14px">ip地址</span>
              </template>
            </q-input>
            <!-- 被测服务器端口 -->
            <q-input filled
                     bottom-slots
                     v-model="paraConfig.Port"
                     class="col"
                     :dense="false"
                     @input="paraConfig.Port=Number(paraConfig.Port.toString().replace(/[^\d]/g,''))">
              <template v-slot:before>
                <span style="font-size:14px;width:105px;margin-left:10px;">被测服务器端口:</span>
              </template>
              <template v-slot:append>
                <span style="font-size:14px">端口</span>
              </template>
            </q-input>
          </div>

          <div class="row input_row">
            <!-- 压测时间 -->
            <q-input filled
                     bottom-slots
                     v-model="paraConfig.Duration"
                     class="col-6"
                     :dense="false"
                     @keyup="paraConfig.Duration=Number(paraConfig.Duration.toString().replace(/[^\d]/g,''))">
              <template v-slot:before>
                <span style="font-size:14px;width:100px">压测时间:</span>
              </template>
              <template v-slot:append>
                <span style="font-size:14px">秒</span>
              </template>
            </q-input>
            <div class="col offset-md-4">
              <q-btn class="btn "
                     color="primary"
                     style="margin-top:10px"
                     label="生 成"
                     @click="CreateJson" />
            </div>
          </div>

          <div class="row input_row">
            <!-- 配置 -->
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
                 @click="newCancel" />
          <q-btn flat
                 label="创建"
                 color="primary"
                 @click="newCreate" />
        </q-card-actions>
      </q-card>
    </q-dialog>

  </div>
</template>

<script>
import * as Apis from "@/api/index"
import lookUp from "@/components/lookUp.vue"
export default {
  name: 'TestCase',
  components: {
    lookUp,
  },
  data () {
    return {
      createFixed: false,   //新建flag
      HostFixed: false,     //主机flag
      TestCaseList: [],     //TeseCase列表
      masterHostList: [], //主机列表
      masterHostSelect: '', //主机选择
      masterHostIndex: -1,//主机下标



      Name: '',           //Name
      Configuration: '',  //Configuration
      EngineType: '',     //EngineType
      MasterHostID: '',   //MasterHostID 主机ID

      selected: [],   //表格选择
      //表格配置
      columns: [
        {
          name: 'name',
          required: true,
          label: '名称',
          align: 'left',
          field: row => row.name,
          format: val => `${val}`,
        },
        { name: 'engineType', align: 'left', label: '引擎类型', field: 'engineType', },
        { name: 'configuration', label: '配置', align: 'left', field: 'configuration', },
        { name: 'id', label: '操作', align: 'right', field: 'id', headerStyle: 'text-align:center' },
      ],
      //分页配置
      pagination: {
        page: 1,          //页码
        rowsNumber: 1     //总页数
      },

      //Configuration参数配置
      paraConfig: {
        UserCount: '',//压测用户总数
        PerSecondUserCount: '',//每秒加载用户数
        Address: '',//被测服务器
        Port: '',//被测服务器端口
        Duration: '',//压测时间
      }

    }
  },
  mounted () {
    this.getTestCaseList();
  },
  methods: {
    //新增
    openCrateTestCase () {
      this.createFixed = true;
    },
    //获得主机列表
    getMasterHostList () {
      Apis.getMasterHostList().then((res) => {
        console.log(res)
        this.masterHostList = res.data;
        this.$q.loading.hide()
      })
    },
    //获得TeseCase列表
    getTestCaseList (page) {
      this.$q.loading.show()
      let para = {
        matchName: '',
        //pageSize: 50,
        page: page || 1
      }
      Apis.getTestCaseList(para).then((res) => {
        console.log(res)
        this.TestCaseList = res.data.results;
        this.pagination.page = page || 1;
        this.pagination.rowsNumber = Math.ceil(res.data.totalCount / 50);
        this.getMasterHostList();
      })
    },
    //列表下一页
    nextPage (value) {
      this.getTestCaseList(value)
    },
    //删除Testcase
    deleteTestCase (value) {
      console.log(value)
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
        let para = `?id=${value.row.id}`
        Apis.deleteTestCase(para).then((res) => {
          console.log(res)
          this.getTestCaseList();
        })
      }).onCancel(() => {
      })
    },
    //打开主机lookUP选择框
    masterHost () {
      this.HostFixed = true;
      this.createFixed = false;
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
      this.createFixed = false;

      this.paraConfig.UserCount = '';
      this.paraConfig.PerSecondUserCount = '';
      this.paraConfig.Address = '';
      this.paraConfig.Port = '';
      this.paraConfig.Duration = '';
    },
    //新增弹窗创建按钮
    newCreate () {
      let para = {
        Name: this.Name,
        Configuration: this.Configuration,
        EngineType: this.EngineType,
        MasterHostID: this.MasterHostID
      }
      if (this.Name && this.isJSON(this.Configuration) && this.EngineType && this.MasterHostID) {
        this.$q.loading.show()
        Apis.postCreateTestCase(para).then((res) => {
          console.log(res)
          this.getTestCaseList();
          this.$q.notify({
            position: 'top',
            message: '提示',
            caption: '创建成功',
            color: 'secondary',
          })
          this.newCancel()
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
    //跳转TestCase详情
    toDetail (evt) {
      this.$router.push({
        name: 'TestCaseDetail',
        query: {
          id: evt.row.id
        },
      })
    },
    //生成JSON
    CreateJson () {
      if (this.Configuration == '') {
        this.Configuration = JSON.stringify(this.paraConfig, null, 2);
      } else if (this.isJSON(this.Configuration)) {
        this.Configuration = JSON.parse(this.Configuration);
        this.Configuration.UserCount = Number(this.paraConfig.UserCount);
        this.Configuration.PerSecondUserCount = Number(this.paraConfig.PerSecondUserCount);
        this.Configuration.Address = this.paraConfig.Address;
        this.Configuration.Port = Number(this.paraConfig.Port);
        this.Configuration.Duration = Number(this.paraConfig.Duration);
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
  }
}
</script>

<style lang="scss" scoped>
.TestCase {
  width: 100%;
  overflow: hidden;
  .btn {
    margin-right: 10px;
  }
  .q-pa-md {
    margin-top: 40px;
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
.new_input {
  width: 100%;
  padding: 10px 30px;
  .input_row {
    margin-bottom: 30px;
  }
}
.q-textarea .q-field__native {
  height: 400px;
  resize: none;
}
@media (min-width: 600px) {
  .q-dialog__inner--minimized > div {
    max-width: 800px;
  }
}
</style>