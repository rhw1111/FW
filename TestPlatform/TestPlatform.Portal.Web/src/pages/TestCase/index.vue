<template>
  <div class="TestCase">
    <!-- TestCase列表 -->
    <div class="q-pa-md">
      <!-- <q-list bordered
              separator>
        <q-item clickable
                v-ripple
                v-for="(val,ind) in TestCaseList"
                :key="ind"
                tag="section"
                @dblclick="toDetail(val.id)">
          <q-item-section avatar>
            <q-checkbox v-model="value"
                        :val="val.id" />
          </q-item-section>
          <q-item-section>
            <q-item-label>{{val.name}}</q-item-label>
            <q-item-label caption>{{val.configuration}}</q-item-label>
            <q-item-label caption>{{val.engineType}}</q-item-label>
          </q-item-section>

        </q-item>

      </q-list> -->

      <q-table title="TestCase列表"
               :data="TestCaseList"
               :columns="columns"
               row-key="id"
               @row-dblclick="toDetail"
               :rows-per-page-options=[0]>
        <!-- 
               selection="multiple"
               :selected.sync="selected"
               :pagination.sync="pagination"
               :pagination-label="paginationLabel" -->
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
          <q-td class="text-right"
                :props="props">
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
    <!-- EngineType选择框 -->
    <EngineTypeLookUp :fixed="EngineTypeFixed"
                      :EngineTypeIndex="EngineTypeSelect"
                      @cancelEngineType="cancelEngineType"
                      @addEngineType="addEngineType"
                      ref='TypelookUp' />
    <!-- 新增TestCase框 -->
    <q-dialog v-model="createFixed"
              persistent>
      <q-card style="width:100%">
        <q-card-section>
          <div class="text-h6">创建TestCase</div>
        </q-card-section>

        <q-separator />
        <div class="new_input">
          <div class="row">
            <q-input v-model="Name"
                     :dense="false"
                     class="col">
              <template v-slot:before>
                <span style="font-size:14px">Name:</span>
              </template>
            </q-input>
            <q-input v-model="EngineType"
                     :dense="false"
                     class="col"
                     style="margin-left:50px;"
                     readonly
                     @dblclick="openEngineType">
              <template v-slot:before>
                <span style="font-size:14px">EngineType:</span>
              </template>
            </q-input>
          </div>
          <div class="row">
            <q-input :dense="false"
                     class="col col-xs-12"
                     readonly
                     v-model="masterHostSelect"
                     @dblclick="masterHost">
              <template v-slot:before>
                <span style="font-size:14px">MasterHost:</span>
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
          <div class="row">
            <q-input v-model="Configuration"
                     :dense="false"
                     class="col-xs-12"
                     type="textarea"
                     outlined>
              <template v-slot:before>
                <span style="font-size:14px">Configuration:</span>
              </template>
            </q-input>
          </div>
          <!-- <q-input v-model="Name"
                   label="Name" />
          <q-input v-model="EngineType"
                   label="EngineType" />
          <q-input v-model="Configuration"
                   label="Configuration" />
          <div class="q-pa-md row">
            <q-input outlined
                     readonly
                     v-model="masterHostSelect"
                     label="主机："
                     stack-label
                     :dense="false"
                     class="col-xs-12 col-sm-4 col-xl-4"
                     @dblclick="masterHost" />
          </div> -->
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
import EngineTypeLookUp from "@/components/EngineTypeLookUp.vue"
export default {
  name: 'TestCase',
  components: {
    lookUp,
    EngineTypeLookUp
  },
  data () {
    return {
      createFixed: false,   //新建flag
      HostFixed: false,     //主机flag
      TestCaseList: [],     //TeseCase列表
      masterHostList: [], //主机列表
      masterHostSelect: '', //主机选择
      masterHostIndex: -1,//主机下标


      EngineTypeFixed: false,//EngineTypeFlag
      EngineTypeSelect: -1,//EngineType选择


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
          label: 'Name',
          align: 'left',
          field: row => row.name,
          format: val => `${val}`,
        },
        { name: 'engineType', align: 'left', label: 'EngineType', field: 'engineType', },
        { name: 'configuration', label: 'Configuration', align: 'left', field: 'configuration', },
        { name: 'id', label: '', align: 'left', field: 'id', },
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
        message: '您确定要删除当前的TestCase吗',
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

    // 打开EngineType框
    openEngineType () {
      this.EngineTypeFixed = true;
      this.createFixed = false;
    },
    //添加EngineType
    addEngineType (value, index) {
      if (value == undefined) {
        return false;
      }
      console.log(value, index)
      this.EngineType = value[index];
      this.EngineTypeSelect = index;
      this.createFixed = true;
      this.EngineTypeFixed = false;
    },
    //取消EngineType框
    cancelEngineType () {
      this.EngineTypeFixed = false;
      this.createFixed = true;
      this.$refs.TypelookUp.selectIndex = this.EngineTypeSelect;
    },


    //新增弹窗取消按钮
    newCancel () {
      this.Name = '';
      this.Configuration = '';
      this.EngineType = '';
      this.MasterHostID = '';
      this.masterHostSelect = '';
      this.$refs.lookUp.selectIndex = -1;
      this.$refs.TypelookUp.selectIndex = -1;
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
    toDetail (evt, row) {
      console.log(row)
      this.$router.push({
        name: 'TestCaseDetail',
        query: {
          id: row.id
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
  .cursor-pointer {
    .text-left {
      white-space: nowrap;
      overflow: hidden;
      text-overflow: ellipsis;
    }
    .q-table--col-auto-width {
      width: 75px !important;
    }
  }
}
.q-table--col-auto-width {
  width: 75px;
}
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