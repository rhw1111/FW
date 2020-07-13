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
            <!-- <q-input outlined
                     readonly
                     v-model="masterHostSelect"
                     label="主机："
                     stack-label
                     :dense="false"
                     class="col-xs-12"
                     @dblclick="masterHost" /> -->
          </div>
          <div class="row input_row">
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
        { name: 'id', label: '操作', align: 'left', field: 'id', },
      ],
      //分页配置
      pagination: {
        page: 1,          //页码
        rowsNumber: 1     //总页数
      },
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
    },
    //新增弹窗创建按钮
    newCreate () {
      let para = {
        Name: this.Name,
        Configuration: this.Configuration,
        EngineType: this.EngineType,
        MasterHostID: this.MasterHostID
      }
      if (this.Name && this.Configuration && this.EngineType && this.MasterHostID) {
        this.$q.loading.show()
        Apis.postCreateTestCase(para).then((res) => {
          console.log(res)
          this.getTestCaseList();
          this.createFixed = false;
          this.$q.notify({
            position: 'top',
            message: '提示',
            caption: '创建成功',
            color: 'secondary',
          })
          this.Name = '';
          this.Configuration = '';
          this.EngineType = '';
          this.MasterHostID = '';
          this.masterHostSelect = '';
          this.$refs.lookUp.selectIndex = -1;
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
    }
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
  resize: none;
}
@media (min-width: 600px) {
  .q-dialog__inner--minimized > div {
    max-width: 700px;
  }
}
</style>