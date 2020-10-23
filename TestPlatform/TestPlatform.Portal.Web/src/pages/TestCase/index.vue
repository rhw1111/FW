<template>
  <div class="TestCase">
    <!-- TestCase列表 -->
    <div class="q-pa-md">

      <transition name="TreeEntity-slid">
        <TreeEntity v-if="expanded"
                    ref="TreeEntity"
                    style="max-width:20%;height:100%;overflow:auto;float:left;"
                    @getDirectoryLocation="getDirectoryLocation"
                    :DirectoryLocation="DirectoryLocation"
                    :TestCaseTreeExpanded="treeExpanded" />
      </transition>

      <div style="height:100%;">
        <q-btn color="grey"
               flat
               dense
               style="width:2%;height:100%;float:left;"
               :icon="expanded ? 'keyboard_arrow_left' : 'keyboard_arrow_right'"
               @click="expanded = !expanded" />

        <q-table title="测试用例列表"
                 :data="TestCaseList"
                 :columns="columns"
                 row-key="id"
                 selection="multiple"
                 :selected.sync="selected"
                 :rows-per-page-options=[0]
                 no-data-label="暂无数据更新">

          <template v-slot:top-right>
            <q-btn class="btn"
                   color="primary"
                   label="运 行"
                   @click="openRunModel" />
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
                     v-show="props.row.status=='正在运行'"
                     label="查看主机日志"
                     @click="lookMasterLog(props)" />
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

    </div>

    <!-- 新增TestCase框 -->
    <q-dialog v-model="createFixed"
              persistent>
      <q-card style="width: 100%; max-width: 70vw;">
        <q-card-section>
          <div class="text-h6">创建测试用例</div>
        </q-card-section>

        <q-separator />

        <CreateShowTestCase :masterHostList="masterHostList"
                            :currentDirectory="SelectLocation"
                            ref="CSTestCase" />

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

    <MixedRunTest ref="MixedRunTest"
                  :runFixedSH="runFixed"
                  :selectedArr="selected" />
  </div>
</template>

<script>
import * as Apis from "@/api/index"
import CreateShowTestCase from './component/CreateShowTestCase.vue' //新建测试用例参数
import TreeEntity from "@/components/TreeEntity.vue"                //目录管理树状图
import MixedRunTest from "./component/MixedRunTest.vue"             //混合场景测试
export default {
  name: 'TestCase',
  components: {
    TreeEntity,
    CreateShowTestCase,
    MixedRunTest
  },
  data () {
    return {
      createFixed: false,   //新建flag
      TestCaseList: [],     //TeseCase列表
      masterHostList: [],//主机列表
      dataSourceName: [],//数据源名称列表

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
        { name: 'engineType', align: 'left', label: '引擎类型', field: 'engineType', style: 'max-width: 50px', headerStyle: 'max-width: 50px' },
        { name: 'configuration', label: '配置', align: 'left', field: 'configuration', style: 'max-width: 250px', headerStyle: 'max-width: 250px' },
        { name: 'status', label: '状态', align: 'left', field: 'status', },
        { name: 'id', label: '操作', align: 'right', field: 'id', headerStyle: 'text-align:center' },
      ],
      //分页配置
      pagination: {
        page: 1,          //页码
        rowsNumber: 1     //总页数
      },
      dismiss: null,  //批量运行测试用例的提示

      //------------------------------- 目录 ---------------------------
      expanded: false,//目录展开收缩flag
      SelectLocation: '',//选择的位置
      DirectoryLocation: [],//目录结构树
      treeExpanded: [],//已选的目录结构树
      // --------------------------------- 运行 --------------------------
      runFixed: false,//运行执行逻辑框
      runModel: 'parallel',//运行模式
      runModelArray: [],//运行模式数组


    }
  },
  mounted () {
    //this.DetailToTestCase();
    this.getTestCaseList(1, null, true);
  },
  methods: {
    //判断是否从详情页回来的
    DetailToTestCase () {
      this.$q.loading.show()
      let detailTestCase = JSON.parse(sessionStorage.getItem('TestCaseLocation'));
      console.log(detailTestCase)
      //判断当前是否在根目录
      if (!detailTestCase) {
        this.getTestCaseList(1, null, true);
      } else {
        if (detailTestCase.SelectLocation == '' || detailTestCase.SelectLocation.id == null) {
          this.getTestCaseList(1, null, true);
        } else {
          let para = { id: detailTestCase.SelectLocation.id };
          Apis.getTreeEntityTreePathId(para).then((res) => {
            console.log(res)

            this.SelectLocation = detailTestCase.SelectLocation.id;
            this.DirectoryLocation = detailTestCase;
            this.treeExpanded.push(null)
            for (let i = 0; i < res.data.length; i++) {
              this.treeExpanded.push(res.data[i])
            }
            this.treeExpanded.pop();
            this.getTestCaseList(1, detailTestCase.SelectLocation.id, true);
          })
        }
      }
    },
    //打开新增测试用例界面
    openCrateTestCase () {
      this.createFixed = true;
    },
    //新增弹窗取消按钮
    newCancel () {
      this.$refs.CSTestCase.newCancel();
      this.createFixed = false;
    },
    //创建测试用例
    newCreate () {
      if (!this.$refs.CSTestCase.newCreate()) { return; }
      let para = this.$refs.CSTestCase.newCreate();
      this.$q.loading.show()
      Apis.postCreateTestCase(para).then((res) => {
        console.log(res)
        this.getTestCaseList(1, this.SelectLocation.id);
        this.$q.notify({
          position: 'top',
          message: '提示',
          caption: '创建成功',
          color: 'secondary',
        })
        this.newCancel()
      })
    },
    //获得TeseCase列表
    getTestCaseList (page, ParentId, expandedBfalse) {
      this.$q.loading.show()
      let para = {
        parentId: ParentId || null,
        matchName: '',
        //pageSize: 50,
        page: page || 1
      }
      Apis.getTestCaseList(para).then((res) => {
        console.log(res)
        this.TestCaseList = res.data.results;
        this.pagination.page = page || 1;
        this.pagination.rowsNumber = Math.ceil(res.data.totalCount / 50);
        //后执行树状图组件
        if (expandedBfalse) {
          this.expanded = true;
          return;
        }
        this.$q.loading.hide();
      })
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
        this.$q.notify({
          position: 'top',
          message: '提示',
          caption: '删除成功',
          color: 'secondary',
        })
        this.selected = [];
        //判断当前的测试用例是否存在目录管理里面，执行不同的删除方法
        if (value.row.treeID == null) {
          let para = `?id=${value.row.id}`
          Apis.deleteTestCase(para).then((res) => {
            console.log(res)
            this.getTestCaseList(1, this.SelectLocation.id);
          })
        } else {
          let para = `?id=${value.row.treeID}`
          Apis.deleteTreeEntity(para).then((res) => {
            console.log(res)
            this.getTestCaseList(1, this.SelectLocation.id);
          })
        }
      })
    },
    //获得主机列表
    getMasterHostList () {
      Apis.getMasterHostList().then((res) => {
        console.log(res)
        this.masterHostList = res.data;
        this.$q.loading.hide()
      })
    },
    //获得数据源名称
    getDataSourceName () {
      let para = {}
      Apis.getDataSourceName(para).then((res) => {
        console.log(res)
        this.dataSourceName = res.data;
        this.$q.loading.hide()
      })
    },
    //列表分页切换
    nextPage (value) {
      this.getTestCaseList(value, this.SelectLocation.id);
    },
    //跳转TestCase详情
    toDetail (evt) {
      this.$router.push({
        name: 'TestCaseDetail',
        query: {
          id: evt.row.id
        },
      })
      sessionStorage.setItem('TestCaseLocation', JSON.stringify({
        SelectLocation: this.SelectLocation,
        DirectoryStructure: this.$refs.TreeEntity.getDirectoryStructure()
      }))
    },
    // --------------------- 运行 --------------------
    //打开运行选择模式界面
    openRunModel () {
      console.log(this.$refs.MixedRunTest)
      //判断是否选择测试用例
      if (this.selected.length != 0) {
        this.$refs.MixedRunTest.openRunModel();
      } else {
        this.$q.notify({
          position: 'top',
          message: '提示',
          caption: '请选择测试用例',
          color: 'red',
        })
      }
    },
    //关闭运行选择模式界面
    closeRunModel () {
      this.selected = [];
      this.getTestCaseList(1, this.SelectLocation.id);
    },
    //查看主机日志
    lookMasterLog (value) {
      this.$q.loading.show()
      Apis.getMasterLog({ caseId: value.row.id }).then((res) => {
        this.$q.loading.hide()
        this.$q.dialog({
          title: '提示',
          message: res.data,
          style: { 'width': '100%', 'max-width': '65vw', "white-space": "pre-line", "overflow-x": "hidden", "word-break": "break-all" }
        })
      })
    },
    // --------------------- 目录 --------------------
    //获得选择的目录
    getDirectoryLocation (data) {
      console.log(data)
      this.getTestCaseList(1, data.id)
      this.SelectLocation = data;
    },
  }
}
</script>

<style lang="scss" scoped>
.TestCase {
  position: fixed;
  width: 100%;
  height: 100%;
  //overflow: hidden;

  .btn {
    margin-right: 10px;
  }
  .q-pa-md {
    height: 100%;
  }
}
.pointer {
  cursor: pointer;
  margin-left: 20px;
  font-size: 12px;
  border-radius: 50%;
  display: inline-block;
}
</style>
<style lang="scss">
.q-table__container {
  height: 95%;
}
.q-table {
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
.TreeEntity-slid-enter-active,
.TreeEntity-slid-leave-active {
  transition: all 0.3s;
}
.TreeEntity-slid-enter,
.TreeEntity-slid-leave-active {
  transform: translate3d(-3rem, 0, 0);
  opacity: 0;
}
</style>