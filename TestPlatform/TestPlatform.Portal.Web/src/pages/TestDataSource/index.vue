<template>
  <div class="TestDataSource">
    <!-- TestDataSource列表 -->
    <div class="q-pa-md">
      <transition name="TreeEntity-slid">
        <TreeEntity v-show="expanded"
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
        <q-table title="测试数据源列表"
                 :data="TestDataSourceList"
                 :columns="columns"
                 selection="multiple"
                 :selected.sync="selected"
                 row-key="id"
                 :rows-per-page-options=[0]
                 no-data-label="暂无数据更新">

          <template v-slot:top-right>
            <q-btn class="btn"
                   color="primary"
                   label="新 增"
                   @click="openCreate" />
            <q-btn class="btn"
                   style="background: #FF0000; color: white"
                   label="删 除"
                   @click="deleteTestDataSource" />
          </template>

          <template v-slot:body-cell-id="props">
            <q-td class="text-left"
                  :props="props">
              <q-btn class="btn"
                     color="primary"
                     label="更 新"
                     @click="getTestDataSourceDetail(props)" />
              <q-btn class="btn"
                     color="red"
                     label="删 除"
                     @click="deleteTestDataSourceOne(props.row)" />
            </q-td>
          </template>
          <template v-slot:bottom
                    class="row">
            <q-pagination v-model="pagination.page"
                          :max="pagination.rowsNumber"
                          :input="true"
                          @input="switchPage"
                          class="col offset-md-10">
            </q-pagination>
          </template>
        </q-table>
      </div>
    </div>
    <!-- 新增TestDataSource框 -->
    <q-dialog v-model="createFixed"
              persistent>
      <q-card style="width:100%; max-width: 70vw;">
        <q-card-section>
          <div class="text-h6">创建测试数据源</div>
        </q-card-section>

        <q-separator />
        <CreatePut ref="createDataSource"
                   :currentDirectory="SelectLocation" />

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

    <!-- 查看更新TestDataSource框 -->
    <q-dialog v-model="LookDataSourceFixed"
              persistent>
      <q-card style="width:100%; max-width: 70vw;">
        <q-card-section>
          <div class="text-h6">更新测试数据源</div>
        </q-card-section>

        <q-separator />
        <CreatePut ref="putDataSource"
                   :detailData="detailData" />

        <q-separator />

        <q-card-actions align="right">
          <q-btn flat
                 label="取消"
                 color="primary"
                 @click="cancelPutDataSource" />
          <q-btn flat
                 label="更新"
                 color="primary"
                 @click="putTestDataSource" />
        </q-card-actions>
      </q-card>
    </q-dialog>
  </div>
</template>

<script>
import * as Apis from "@/api/index"
import CreatePut from "./CreatePut.vue"               //创建更新测试数据源
import TreeEntity from "@/components/TreeEntity.vue"  //目录管理结构树
export default {
  name: 'TestDataSource',
  components: {
    TreeEntity,
    CreatePut
  },
  data () {
    return {

      createFixed: false,  //新增Flag
      LookDataSourceFixed: false,//查看更新DataSource
      TestDataSourceList: [], //TestDataSource列表

      detailData: '',
      selected: [],//批量选择
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
        { name: 'type', align: 'left', label: '类型', field: 'type', },
        { name: 'data', label: '数据', align: 'left', field: 'data', style: 'max-width: 250px', headerStyle: 'max-width: 250px' },
        { name: 'id', label: '操作', align: 'right', field: 'id', headerStyle: 'text-align:center', style: 'width: 10%', },
      ],
      //分页配置
      pagination: {
        page: 1,          //页码
        rowsNumber: 1     //总页数
      },
      //------------------------ 目录 -------------------------------
      expanded: true,//目录展开收缩flag
      SelectLocation: '',//选择目录的位置
    }
  },
  mounted () {
    this.getTestDataSource();
  },
  methods: {
    //获得TestDataSource列表
    getTestDataSource (page, parentId) {
      this.$q.loading.show()
      let para = {
        parentId: parentId || null,
        matchName: '',
        page: page || 1,
        pageSize: 50
      }
      Apis.getTestDataSource(para).then((res) => {
        console.log(res)
        this.pagination.page = page || 1;
        this.pagination.rowsNumber = Math.ceil(res.data.totalCount / 50);
        this.TestDataSourceList = res.data.results;
        this.selected = [];
        this.$q.loading.hide();
      })
    },
    //获得TestDataSource详情数据
    getTestDataSourceDetail (env) {
      this.$q.loading.show()
      let para = {
        id: env.row.id
      }
      Apis.getTestDataSourceDetail(para).then((res) => {
        console.log(res)
        let data = res.data;
        this.detailData = {
          SelectedId: data.id,
          Name: data.name,
          Type: data.type,
          Data: data.data,
          ChangeFileDirectoryName: data.parentName,
          ChangeFileDirectoryId: data.parentID
        }
        this.LookDataSourceFixed = true;
        this.$q.loading.hide()
      })
    },
    //打开
    openCreate () {
      this.createFixed = true;
    },
    //跳转到详情
    toDetail () {
      this.LookDataSourceFixed = true;
      this.getTestDataSourceDetail()
    },
    //页码切换
    switchPage (value) {
      this.getTestDataSource(value, this.SelectLocation.id)
    },
    //---------------------------------------------- 新建取消创建测试数据源 -------------------------------------------
    //新建TestDataSource
    newCreate () {
      if (!this.$refs.createDataSource.newCreate()) { return }
      let para = this.$refs.createDataSource.newCreate()
      this.$q.loading.show()
      Apis.postCreateTestDataSource(para).then(() => {
        this.getTestDataSource(1, this.SelectLocation.id);
        this.createFixed = false;
        this.$q.notify({
          position: 'top',
          message: '提示',
          caption: '创建成功',
          color: 'secondary',
        })
      })
    },
    //取消新建TestDataSource
    newCancel () {
      this.$refs.createDataSource.newCancel();
      this.createFixed = false;
    },

    //---------------------------------------------- 更新取消创建测试数据源 -------------------------------------------
    //取消更新测试数据源
    cancelPutDataSource () {
      this.$refs.putDataSource.cancelPutDataSource();
      this.LookDataSourceFixed = false;
    },
    //更新TestDataSource
    putTestDataSource () {
      if (!this.$refs.putDataSource.putTestDataSource()) { return }
      let para = this.$refs.putDataSource.putTestDataSource();
      this.$q.loading.show()
      Apis.putTestDataSource(para).then((res) => {
        console.log(res)
        this.getTestDataSource(1, this.SelectLocation.id);
        this.$q.notify({
          position: 'top',
          message: '提示',
          caption: '更新成功',
          color: 'secondary',
        })
      })
    },
    //---------------------------------------------- 删除测试数据源 -------------------------------------------
    //删除TestDataSource
    deleteTestDataSource () {
      if (this.selected.length == 0) {
        this.$q.notify({
          position: 'top',
          message: '提示',
          caption: '请选择您要删除的测试数据源',
          color: 'red',
        })
        return;
      }
      this.$q.dialog({
        title: '提示',
        message: '您确定要删除当前选择的测试数据源吗',
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
        if (this.selected.length == 1) {
          //单个删除TestDataSource
          this.$q.loading.show()

          //判断当前的测试用例是否存在目录管理里面，执行不同的删除方法
          if (this.selected[0].treeID == null) {
            let para = `?id=${this.selected[0].id}`;
            Apis.deleteTestDataSource(para).then(() => {
              this.selected = [];
              this.$q.notify({
                position: 'top',
                message: '提示',
                caption: '删除成功',
                color: 'secondary',
              })
              this.getTestDataSource(1, this.SelectLocation.id)
            })
          } else {
            let para = `?id=${this.selected[0].treeID}`
            Apis.deleteTreeEntity(para).then((res) => {
              this.selected = [];
              console.log(res)
              this.$q.notify({
                position: 'top',
                message: '提示',
                caption: '删除成功',
                color: 'secondary',
              })
              this.getTestDataSource(1, this.SelectLocation.id)
            })
          }

        } else if (this.selected.length > 1) {
          //批量删除TestDataSource
          this.$q.loading.show()
          delDataSource();

          // this.$q.loading.show()
          // let delArr = [];
          // for (let i = 0; i < this.selected.length; i++) {
          //   delArr.push(this.selected[i].id)
          // }
          // let para = {
          //   delArr: delArr
          // };
          // Apis.deleteTestDataSourceArr(para).then(() => {
          //   this.selected = [];
          //   this.$q.notify({
          //     position: 'top',
          //     message: '提示',
          //     caption: '删除成功',
          //     color: 'secondary',
          //   })
          //   this.getTestDataSource();
          // })
        }
      })
      let _this = this;
      let delNum = 0;
      //批量删除测试数据源
      function delDataSource () {
        if (delNum != _this.selected.length) {
          if (_this.selected[delNum].treeID == null) {
            let para = `?id=${_this.selected[delNum].id}`;
            Apis.deleteTestDataSource(para).then((res) => {
              console.log(res)
              delNum++;
              delDataSource();
            })
          } else {
            let para = `?id=${_this.selected[delNum].treeID}`
            Apis.deleteTreeEntity(para).then((res) => {
              console.log(res)
              delNum++;
              delDataSource();
            })
          }
        } else {
          _this.selected = [];
          _this.$q.notify({
            position: 'top',
            message: '提示',
            caption: '删除成功',
            color: 'secondary',
          })
          _this.getTestDataSource(1, _this.SelectLocation.id)
        }
      }
    },
    //单个删除TestDataSource
    deleteTestDataSourceOne (value) {
      this.$q.dialog({
        title: '提示',
        message: '您确定要删除当前选择的测试数据源吗',
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
        if (value.treeID == null) {
          let para = `?id=${value.id}`;
          Apis.deleteTestDataSource(para).then(() => {
            this.selected = [];
            this.$q.notify({
              position: 'top',
              message: '提示',
              caption: '删除成功',
              color: 'secondary',
            })
            this.getTestDataSource(1, this.SelectLocation.id)
          })
        } else {
          let para = `?id=${value.treeID}`
          Apis.deleteTreeEntity(para).then((res) => {
            this.selected = [];
            console.log(res)
            this.$q.notify({
              position: 'top',
              message: '提示',
              caption: '删除成功',
              color: 'secondary',
            })
            this.getTestDataSource(1, this.SelectLocation.id)
          })
        }
      })
    },
    //---------------------------------------------- 目录 -------------------------------------------
    //获得选择的目录
    getDirectoryLocation (data) {
      console.log(data)
      this.getTestDataSource(1, data.id)
      this.SelectLocation = data;
    },
  }
}
</script>

<style lang="scss" scoped>
.TestDataSource {
  position: fixed;
  width: 100%;
  height: 100%;
  .TestDataSource_header {
    position: fixed;
    left: 0;
    right: 0;
    padding: 10px 16px 5px;
    width: 100%;
    z-index: 10;
    box-sizing: border-box;
    background: #ffffff;
    .btn {
      margin-right: 10px;
    }
  }
  .q-pa-md {
    height: 100%;
  }
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
    margin-bottom: 10px;
  }
}
.q-pa-md {
  .btn {
    margin-right: 10px;
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