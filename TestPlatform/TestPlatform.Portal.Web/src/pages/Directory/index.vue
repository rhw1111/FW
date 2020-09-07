<template>
  <div id="Directory">
    <!-- button -->
    <div class="detail_header row detail_fixed">
      <div class="col-7">
        <q-btn class="btn"
               color="primary"
               label="上一级"
               v-show="!isRootFolderFlag"
               @click="toPrevLevel" />
        <q-btn class="btn"
               color="primary"
               label="目录重命名"
               @click="ModifyFolderName" />
        <q-btn class="btn"
               color="red"
               label="删除"
               @click="DeleteFolder" />
        <q-btn class="btn"
               color="primary"
               label="移动目录"
               @click="ChangeFileDirectory" />
        <q-btn class="btn"
               color="primary"
               label="新建目录"
               @click="newFolderCreate" />
        <q-btn class="btn"
               color="primary"
               label="新建测试用例"
               @click="createTestCaseFixed = true;" />
        <q-btn class="btn"
               color="primary"
               label="新建测试数据源"
               @click="createDataSourceFixed = true;" />
      </div>

      <div class="col-5 row">

        <div class="col-8 row">
          <q-input class="col-8"
                   v-model="searchText"
                   outlined
                   placeholder="请输入文件名称"
                   type="search"
                   :dense='true'>
            <template v-slot:append>
              <q-icon name="search" />
            </template>
          </q-input>

          <q-select class="col-3"
                    v-model="FileType"
                    style="margin-left:10px;"
                    :options="['文件夹','测试用例','测试数据源']"
                    label="类型"
                    outlined
                    :dense='true' />
        </div>

        <div class="col-4">
          <q-btn class="btn"
                 color="primary"
                 label="搜索"
                 @click="searchFile" />

          <q-btn class="btn"
                 color="primary"
                 label="取消搜索"
                 @click="cancelSearch" />
        </div>
      </div>
    </div>
    <!-- 目录 -->
    <div class="folder">
      <div class="q-pa-md">
        <div class="Rootfolder row">
          <div class="Rootfolder_list"
               v-for="(value ,index) in FolderList"
               :key="index"
               @dblclick="toNextLevel(value,index)"
               tag="label">

            <q-checkbox v-model="selection"
                        :val="value"
                        :dense="true"
                        label=""
                        class="checkbox" />
            <span class="svg-container"
                  v-show="value.type==1">
              <svg class="icon iconfolder"
                   aria-hidden="true">
                <use xlink:href="#icon-wenjianjia2"></use>
              </svg>
            </span>
            <span class="svg-container"
                  v-show="value.type==2">
              <svg class="icon iconfolder"
                   aria-hidden="true">
                <use xlink:href="#icon--wenjian"></use>
              </svg>
            </span>
            <span class="svg-container"
                  v-show="value.type==3">
              <svg class="icon iconfolder"
                   aria-hidden="true">
                <use xlink:href="#icon-shujuyuan"></use>
              </svg>
            </span>

            <p>{{value.name}}</p>
          </div>
        </div>
        <div class="folderList_page">
          <q-pagination v-model="folderPageCurrent"
                        :max="folderPageMaxCurrent"
                        style="float:right;margin-top:5px;margin-right:5%;"
                        :input="true"
                        @input="switchFolderPage">
          </q-pagination>
        </div>
      </div>
    </div>

    <!-- 更改文件目录 -->
    <q-dialog v-model="ChangeFileDirectoryFlag"
              persistent>
      <q-card style="width: 100%;">
        <q-card-section>
          <div class="text-h6">选择文件目录</div>
        </q-card-section>

        <q-separator />

        <TreeEntity />

        <q-separator />

        <q-card-actions align="right">
          <q-btn flat
                 label="取消"
                 color="primary"
                 @click="ChangeFileDirectoryFlag = false;" />
          <q-btn flat
                 label="确定"
                 color="primary"
                 @click="ChangeFileDirectoryFlag = false;" />
        </q-card-actions>
      </q-card>
    </q-dialog>

    <!-- 新增TestCase框 -->
    <q-dialog v-model="createTestCaseFixed"
              persistent>
      <q-card style="width: 100%; max-width: 70vw;">
        <q-card-section>
          <div class="text-h6">创建测试用例</div>
        </q-card-section>

        <q-separator />

        <CreateShowTestCase :dataSourceName="dataSourceName"
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

    <!-- 新增TestDataSource框 -->
    <q-dialog v-model="createDataSourceFixed"
              persistent>
      <q-card style="width:100%; max-width: 70vw;">
        <q-card-section>
          <div class="text-h6">创建测试数据源</div>
        </q-card-section>

        <q-separator />
        <div class="new_input">
          <div class="row input_row">
            <q-input v-model="DataSourceName"
                     :dense="false"
                     class="col">
              <template v-slot:before>
                <span style="font-size:14px">名称:</span>
              </template>
            </q-input>
            <q-select v-model="DataSourceType"
                      :options="['String','Int','Json','Label']"
                      class="col"
                      :dense="false">
              <template v-slot:before>
                <span style="font-size:14px">类型:</span>
              </template>
              <template v-slot:prepend>
              </template>
            </q-select>
            <q-input :dense="false"
                     class="col"
                     readonly
                     v-model="DataSourceType"
                     placeholder="点击右侧加号选择文件目录">
              <template v-slot:before>
                <span style="font-size:14px">文件目录:</span>
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
          <div class="row input_row">
            <q-input v-model="DataSourceData"
                     :dense="false"
                     class="col-xs-12"
                     type="textarea"
                     :input-style="{height:'400px'}"
                     outlined>
              <template v-slot:before>
                <span style="font-size:14px">数据:</span>
              </template>
            </q-input>
          </div>
        </div>

        <q-separator />

        <q-card-actions align="right">
          <q-btn flat
                 label="取消"
                 color="primary"
                 @click="newDataSourceCancel" />
          <q-btn flat
                 label="创建"
                 color="primary"
                 @click="newDataSourceCreate" />
        </q-card-actions>
      </q-card>
    </q-dialog>

  </div>
</template>

<script>
import * as Apis from "@/api/index"
import CreateShowTestCase from '../TestCase/component/CreateShowTestCase.vue'
import TreeEntity from "@/components/TreeEntity.vue"
export default {
  name: 'Directory',
  components: {
    CreateShowTestCase,
    TreeEntity
  },
  data () {
    return {
      FolderList: [],//目录列表
      isRootFolderFlag: true,//是否是根目录
      selection: [],
      SelectFolder: '',//当前选择的目录
      folderPageCurrent: 1,//文件目录页码
      folderPageMaxCurrent: 1,//最大页码
      // -------- 搜索 -------
      searchText: '',//搜索内容
      FileType: null,//文件类型
      // -------- 更改文件目录 -------
      ChangeFileDirectoryFlag: false,//更改文件目录Flag
      // -------- 新建测试用例 -------
      createTestCaseFixed: false,
      dataSourceName: [],//数据源名称

      // -------- 新建测试数据源 -------
      createDataSourceFixed: false,
      DataSourceName: '',
      DataSourceType: '',
      DataSourceData: '',

    }
  },
  created () {
    this.getTreeEntityList();
    //this.getDataSourceName();
  },
  methods: {
    //获得根目录
    getTreeEntityList (Page) {
      this.$q.loading.show();
      let para = {
        matchName: '',
        page: Page ? Page : 1,
        type: null,
        pageSize: 100
      }
      Apis.getTreeEntityList(para).then(res => {
        console.log(res)
        this.folderPageCurrent = Page ? Page : 1;
        this.folderPageMaxCurrent = Math.ceil(res.data.totalCount / 100);

        // for (let i = 0; i < 100; i++) {
        //   this.FolderList.push({
        //     createTime: "2020-09-01T12:57:41",
        //     id: "0ad1fd7e-de4b-40ed-ab6e-18359a584059",
        //     name: "目录2",
        //     parentID: null,
        //     type: 1,
        //     value: null
        //   })
        // }
        this.FolderList = res.data.results;
        this.$q.loading.hide();
      })
    },
    //上一级
    toPrevLevel () {
      let para = {
        parentId: this.SelectFolder.id,
        page: 1,
        pageSize: 100
      }
      Apis.getgobackpreviousTreeEntity(para).then(res => {
        console.log(res)
      })
    },
    //下一级
    toNextLevel (value, index) {
      let para = {
        parentId: value.id,
        matchName: '',
        page: 1,
        type: null,
        pageSize: 100
      }
      this.$q.loading.show();
      Apis.getTreeEntityChildrenList(para).then(res => {
        console.log(res)
        this.isRootFolderFlag = false;
        this.SelectFolder = value;
        this.FolderList = res.data.results;
        this.$q.loading.hide();
      })
      console.log(value, index)
    },
    //删除文件夹
    DeleteFolder () {
      if (this.selection.length == 0) {
        this.$q.notify({
          position: 'top',
          message: '提示',
          caption: '请选择目录',
          color: 'red',
        })
      } else {
        this.$q.dialog({
          title: '提示',
          message: `您确定要删除当前选择的的目录吗`,
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
        }).onCancel(() => {
        })
      }

    },
    //更改文件夹目录
    ChangeFileDirectory () {
      this.ChangeFileDirectoryFlag = true;
    },
    //切换目录页码
    switchFolderPage (value) {
      console.log(value)
      this.getTreeEntityList(value)
    },
    //------------------- 搜索 ---------------------
    searchFile () {
      this.getTreeEntityList();
    },
    cancelSearch () {
      this.searchText = ''; this.FileType = null;
      this.isRootFolderFlag = false;
    },
    //------------------- 新建目录 ---------------------
    newFolderCreate () {
      this.$q.dialog({
        title: '新建目录',
        message: '目录名称',
        prompt: {
          model: '',
          type: 'text' // optional
        },
        persistent: true,
        ok: {
          push: true,
          label: '确定'
        },
        cancel: {
          push: true,
          label: '取消'
        },
      }).onOk(data => {
        this.$q.loading.show();
        let para = {
          Name: data,
          FolderID: this.SelectFolder ? this.SelectFolder.id : this.SelectFolder,
        }
        Apis.postCreateTreeEntity(para).then(res => {
          console.log(res)
          this.$q.loading.hide();
        })
      })
    },
    //重命名
    ModifyFolderName () {
      console.log(this.selection)
      if (this.selection.length == 0) {
        this.$q.notify({
          position: 'top',
          message: '提示',
          caption: '请选择目录、测试用例、测试数据源',
          color: 'red',
        })
      } else if (this.selection.length > 1) {
        this.$q.notify({
          position: 'top',
          message: '提示',
          caption: '只能选择一个文件进行修改（目录、测试用例、测试数据源）',
          color: 'red',
        })
      } else {
        console.log(this.selection)
        this.$q.dialog({
          title: '目录重命名',
          message: '目录名称',
          prompt: {
            model: this.selection[0].Name,
            type: 'text' // optional
          },
          persistent: true,
          ok: {
            push: true,
            label: '确定'
          },
          cancel: {
            push: true,
            label: '取消'
          },
        }).onOk(data => {
          console.log(data)
        })
      }
    },
    //------------------- 新建测试用例 ---------------------
    //获得数据源名称
    getDataSourceName () {
      this.$q.loading.show()
      let para = {}
      Apis.getDataSourceName(para).then((res) => {
        console.log(res)
        this.dataSourceName = res.data;
        this.$q.loading.hide()
      })
    },
    //新增弹窗取消按钮
    newCancel () {
      this.$refs.CSTestCase.newCancel();
      this.createTestCaseFixed = false;
    },
    //新增弹窗创建按钮
    newCreate () {
      if (!this.$refs.CSTestCase.newCreate()) { return; }
      let para = this.$refs.CSTestCase.newCreate();
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
        this.getTreeEntityList();
      })
    },
    // ------------------- 新建测试数据源 ---------------------
    //新建TestDataSource
    newDataSourceCreate () {
      this.newDataSourceCancel();
    },
    //取消新建TestDataSource
    newDataSourceCancel () {
      this.DataSourceName = '';
      this.DataSourceType = '';
      this.DataSourceData = '';
      this.createDataSourceFixed = false;
    },
  }
}
</script>

<style lang="scss" scoped>
#Directory {
  .detail_header {
    padding: 10px 16px 5px;
    width: 100%;
    box-sizing: border-box;
    border-bottom: 1px solid #ccc;
    background: #ffffff;
    .btn {
      margin-right: 10px;
      margin-bottom: 5px;
    }
  }
  .detail_fixed {
    position: fixed;
    top: 48px;
    left: 0;
    z-index: 10;
  }
  .folder {
    padding: 50px 0px;
    box-sizing: border-box;
    .Rootfolder {
      width: 100%;
      .Rootfolder_list {
        position: relative;
        width: 10%;
        margin: 10px 0px;
        text-align: center;
        box-sizing: border-box;
        .checkbox {
          position: absolute;
          left: 10%;
          top: 35%;
        }
        p {
          width: 70%;
          margin: 0 auto;
          word-wrap: break-word;
        }
        .iconfolder {
          font-size: 50px;
        }
      }
      .Rootfolder_list:hover {
        position: relative;
        width: 10%;
        margin: 10px 0px;
        text-align: center;
        box-sizing: border-box;
        background-color: rgba(204, 232, 255, 0.8);
        .iconfolder {
          font-size: 50px;
        }
        p {
          width: 70%;
          margin: 0 auto;
        }
      }
    }
    .folderList_page {
      position: fixed;
      bottom: 0%;
      right: 0%;
      width: 100%;
      height: 50px;
      border-top: 1px solid #ccc;
      box-sizing: border-box;
      background: white;
    }
  }
}
</style>