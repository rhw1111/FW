<template>
  <div class="TestCase">
    <!-- TestCase列表 -->
    <div class="q-pa-md">

      <transition name="TreeEntity-slid">
        <TreeEntity v-show="expanded"
                    refs="TreeEntity"
                    style="max-width:20%;height:100%;overflow:auto;float:left;"
                    @getDirectoryLocation="getDirectoryLocation" />
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

    <!-- 运行执行框 -->
    <q-dialog v-model="runFixed"
              persistent>
      <q-card style="width: 100%; max-width: 50vw;">
        <q-card-section>
          <div class="text-h6">请选择运行测试用例的模式</div>
        </q-card-section>

        <q-separator />
        <div class="q-pa-md">
          <q-radio v-model="runModel"
                   val="parallel"
                   label="并行模式" />
          <q-radio v-model="runModel"
                   val="sequential"
                   label="顺序执行" />

          <div v-show="runModel=='parallel'">

            <div class="input_row row"
                 style="margin-bottom:10px;width:100%;display:inlin-block;"
                 v-for="(value,index) in runModelArray"
                 :key="index">
              <q-input v-model="value.executionTime"
                       outlined
                       class="col-8"
                       :dense="true"
                       placeholder="请输入用例运行的开始时间。"
                       @input="forceUpdate(value.executionTime,index)">
                <template v-slot:before>
                  <span style="font-size:14px;width:150px;word-wrap:break-word;">{{value.name}}:</span>
                </template>
                <template v-slot:append>
                  <q-avatar>
                    秒
                  </q-avatar>
                </template>
              </q-input>

            </div>

          </div>
        </div>
        <q-separator />

        <q-card-actions align="right">
          <q-btn flat
                 label="取消"
                 color="primary"
                 @click="runCancelTestCase" />
          <q-btn flat
                 label="运行"
                 color="primary"
                 @click="runTestCase" />
        </q-card-actions>
      </q-card>
    </q-dialog>

  </div>
</template>

<script>
import * as Apis from "@/api/index"
import CreateShowTestCase from './component/CreateShowTestCase.vue' //新建测试用例参数
import TreeEntity from "@/components/TreeEntity.vue"                //目录管理树状图
export default {
  name: 'TestCase',
  components: {
    TreeEntity,
    CreateShowTestCase,
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
      // --------------------------------- 运行 --------------------------
      runFixed: false,//运行执行逻辑框
      runModel: 'parallel',//运行模式
      runModelArray: [],//运行模式数组

    }
  },
  mounted () {
    this.getTestCaseList(1, null, true);
  },
  methods: {
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
    //获得从机列表
    getSlaveHostsList (id, callback) {
      Apis.getSlaveHostsList({ caseId: id }).then((res) => {
        if (res.data.length == 0) {
          callback(true)
        } else {
          callback(false)
        }
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
    },
    // --------------------- 运行 --------------------
    //打开运行选择模式界面
    openRunModel () {
      //判断是否选择测试用例
      if (this.selected.length != 0) {
        this.isSlaveHost()
      } else {
        this.$q.notify({
          position: 'top',
          message: '提示',
          caption: '请选择测试用例',
          color: 'red',
        })
      }
    },
    // 判断当前运行的测试用例是否运行或者包含从主机，必须包含从主机才能进行运行
    isSlaveHost () {
      this.$q.loading.show();
      let runArray = [];
      //判断是否有正在运行的测试用例
      for (let i = 0; i < this.selected.length; i++) {
        if (this.selected[i].status == '正在运行') {
          runArray.push(this.selected[i].name)
        }
      }
      if (runArray.length != 0) {
        this.$q.notify({
          position: 'top',
          message: '提示',
          caption: `当前测试用例${runArray.join('，')}正在运行当中,请重新选择`,
          color: 'red',
        })
        this.$q.loading.hide();
        return;
      }
      //判断当前选择的测试用例下是否有从主机
      for (let i = 0; i < this.selected.length; i++) {
        this.getSlaveHostsList(this.selected[i].id, (flag) => {
          if (flag) { runArray.push(this.selected[i].name) }
          if (i == this.selected.length - 1) {
            if (runArray.length != 0) {
              this.$q.notify({
                position: 'top',
                message: '提示',
                caption: `当前测试用例${runArray.join('，')}下没有从主机，请添加从主机再进行运行。`,
                color: 'red',
              })
              this.$q.loading.hide();
              return;
            } else {
              this.$q.loading.hide();
              this.runFixed = true;
              for (let i = 0; i < this.selected.length; i++) {
                this.runModelArray.push(this.selected[i]);
                this.runModelArray[i].executionTime = null;
                this.runModelArray[i].runStatus = '未执行';
              }
            }
          }
        })
      }
    },
    //运行TestCase
    runTestCase () {
      //判断是并行模式还是顺序模式
      if (this.runModel == 'parallel') {
        //并行模式执行
        this.ParallelExecution();
      } else {
        //顺序模式执行
        this.run(0);
      }
    },
    //取消运行TestCase
    runCancelTestCase () {
      this.runFixed = false;
      this.runModelArray = [];
    },
    //并行模式执行
    ParallelExecution () {
      let ParrallelLoading = this.$q.loading;
      let runNum = 0;
      ParrallelLoading.show();

      for (let i = 0; i < this.selected.length; i++) {
        setTimeout(() => {
          let para = `?caseId=${this.selected[i].id}`
          Apis.postTestCaseRun(para).then(() => {
            runNum++;
            this.$q.notify({
              position: 'top',
              message: '提示',
              caption: `${this.selected[i].name}运行完成`,
              color: 'secondary',
            })
            if (runNum == this.selected.length) {
              this.runCancelTestCase();
              this.getTestCaseList(1, this.SelectLocation);
              this.selected = [];
            }
          }).catch(err => {
            console.log(err)
            runNum++;
            this.$q.notify({
              position: 'top',
              message: '提示',
              caption: `${this.selected[i].name}运行失败`,
              color: 'red',
            })
            setTimeout(() => {
              ParrallelLoading.show();
            }, 3000);
            if (runNum == this.selected.length) {
              ParrallelLoading.hide();
              this.runCancelTestCase();
              this.getTestCaseList(1, this.SelectLocation);
              this.selected = [];
            }
          });
        }, this.selected[i].executionTime * 1000);
      }
      //查看并行TestCase是否执行完成
      // function getTestCaseStatus (index) {
      //   this.$q.loading.show();
      //   Apis.getTestCaseStatus({ caseId: _this.selected[index].id }).then((res) => {
      //     if (!res.data) {
      //       runNum++;
      //       _this.runModelArray[index].runStatus = '运行完成';
      //       if (runNum == _this.selected.length) {
      //         _this.$q.notify({
      //           position: 'top',
      //           message: '提示',
      //           caption: '运行完成',
      //           color: 'secondary',
      //         })
      //         _this.$q.loading.hide();
      //         _this.runFixed = false;
      //         _this.selected = [];
      //         _this.runModelArray = [];
      //         return;
      //       }

      //       runArray.splice(index, 1, '');
      //     } else {
      //       setTimeout(() => { getTestCaseStatus(index); }, 3000);
      //     }
      //   })
      // }

    },
    //递归运行TestCase
    run (index) {
      console.log(index)
      if (this.dismiss) {
        this.dismiss();
      }
      let runNum = index;
      this.$q.loading.show()
      let para = `?caseId=${this.selected[runNum].id}`
      Apis.postTestCaseRun(para).then((res) => {
        console.log(res)
        this.dismiss = this.$q.notify({
          position: 'top-right',
          caption: `当前测试用例${this.selected[runNum].name}正在运行当中。${runNum + 1}/${this.selected.length}`,
          color: 'teal',
          timeout: '0'
        })
        this.timerOut = window.setInterval(() => {
          setTimeout(this.getTestCaseStatus(runNum), 0);
        }, 3000);
      })

    },
    //查看TestCase是否运行
    getTestCaseStatus (index) {
      Apis.getTestCaseStatus({ caseId: this.selected[index].id }).then((res) => {
        if (!res.data) {
          if (index == this.selected.length - 1) {
            clearInterval(this.timerOut);
            this.timerOut = null;
            this.dismiss();
            this.getMasterHostList();
            this.runFixed = false;
            this.selected = [];
            this.runModelArray = [];
            this.$q.notify({
              position: 'top',
              message: '提示',
              caption: '执行完成',
              color: 'secondary',
            })
            return;
          }
          clearInterval(this.timerOut);
          this.timerOut = null;
          setTimeout(() => {
            this.run(index + 1)
          }, 120000)
        }
      })
    },
    //正则验证并行运行测试用例是否正确
    forceUpdate (val, index) {
      this.$set(this.runModelArray[index], 'executionTime', Number(val.replace(/[^\d]/g, "").replace(/^0/g, "")));
      this.$forceUpdate();
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