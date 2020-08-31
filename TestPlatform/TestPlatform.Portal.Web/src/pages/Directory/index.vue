<template>
  <div id="Directory">
    <!-- button -->
    <div class="detail_header row">
      <div class="col-7">
        <q-btn class="btn"
               color="primary"
               label="上一级"
               v-show="!isRootFolderFlag"
               @click="prevLevel" />
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
               label="移动文件"
               v-show="!isRootFolderFlag"
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

        <q-input class="col-5"
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
                  style="margin:0 10px;"
                  :options="['文件夹','测试用例','测试数据源']"
                  label="类型"
                  outlined
                  :dense='true' />

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
    <!-- 目录 -->
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
                      v-if="value.Type==0"
                      class="checkbox" />
          <span class="svg-container"
                v-show="value.Type==0">
            <svg class="icon iconfolder"
                 aria-hidden="true">
              <use xlink:href="#icon-wenjianjia2"></use>
            </svg>
          </span>
          <span class="svg-container"
                v-show="value.Type==1">
            <svg class="icon iconfolder"
                 aria-hidden="true">
              <use xlink:href="#icon--wenjian"></use>
            </svg>
          </span>
          <span class="svg-container"
                v-show="value.Type==2">
            <svg class="icon iconfolder"
                 aria-hidden="true">
              <use xlink:href="#icon-shujuyuan"></use>
            </svg>
          </span>

          <p>{{value.Name}}</p>
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
      FolderList: [],
      RootFolderList: [
        {
          ID: '1',
          Name: '文件夹1',
          Type: 0,
          Value: '',
          ParentID: null,
        },
        {
          ID: '2',
          Name: '文件夹2',
          Type: 0,
          Value: '',
          ParentID: null,
        },
        {
          ID: '3',
          Name: '文件夹3',
          Type: 0,
          Value: '',
          ParentID: null,
        },
        {
          ID: '3a949390-4c40-4735-9ec8-a3e414a37406',
          Name: 'JasonTest12',
          Type: 1,
          Value: '',
          ParentID: null,
        },
        {
          ID: 'f64823a1-3327-4a24-b49f-58a2e96ca400',
          Name: 'JsonTest13',
          Type: 1,
          Value: '',
          ParentID: null,
        },
        {
          ID: '1039e6cd-d096-11ea-b225-00ffb1d16cf9',
          Name: 'datasource_host',
          Type: 2,
          Value: '',
          ParentID: null,
        },
      ],//根目录名称
      ChildFolderList: [
        {
          ID: '001',
          Name: '子文件夹1',
          Type: 0,
          Value: '',
          ParentID: '1',
        },
        {
          ID: '002',
          Name: '子文件夹2',
          Type: 0,
          Value: '',
          ParentID: '1',
        },
        {
          ID: '003',
          Name: '子文件夹3',
          Type: 0,
          Value: '',
          ParentID: '2',
        },
      ],
      isRootFolderFlag: true,//是否是根目录
      selection: [],
      // -------- 搜索 -------
      searchText: '',//搜索内容
      FileType: '',//文件类型
      // -------- 新建文件夹 -------
      createFolderFixed: false,
      FolderName: '',//文件夹名称
      SelectFolder: '',//进入到哪个文件夹
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
    this.prevLevel();
    this.getDataSourceName();
  },
  methods: {
    toNextLevel (value, index) {
      console.log(value, index)
      if (value.Type == 0) {
        this.FolderList = [];
        this.isRootFolderFlag = false;
        for (let i = 0; i < this.ChildFolderList.length; i++) {
          if (this.ChildFolderList[i].ParentID === value.ID) {
            this.FolderList.push(this.ChildFolderList[i])
          }
        }
        this.SelectFolder = value;
      } else if (value.Type == 1) {
        this.$router.push({
          path: '/Directory/TestCase/Detail',
          query: {
            id: value.ID
          },
        })
      } else if (value.Type == 2) {
        this.$router.push({
          path: '/Directory/TestDataSource/Detail',
          query: {
            id: value.ID
          },
        })
      }
    },
    prevLevel () {
      this.FolderList = [];

      this.isRootFolderFlag = true;
      for (let i = 0; i < this.RootFolderList.length; i++) {
        this.FolderList.push(this.RootFolderList[i])
      }
    },
    //删除文件夹
    DeleteFolder () {
      if (this.selection.length == 0) {
        this.$q.notify({
          position: 'top',
          message: '提示',
          caption: '请选择文件夹',
          color: 'red',
        })
      } else {
        this.$q.dialog({
          title: '提示',
          message: `您确定要删除当前选择的的文件夹吗`,
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
          for (let i = 0; i < this.RootFolderList.length; i++) {
            if (this.RootFolderList[i].ID == this.SelectFolder.ID) {
              this.RootFolderList.splice(i, 1)
            }
          }
          this.prevLevel();
        }).onCancel(() => {
        })
      }

    },
    //更改文件夹目录
    ChangeFileDirectory () {
      this.ChangeFileDirectoryFlag = true;
    },
    //------------------- 搜索 ---------------------
    searchFile () {
      if (this.searchText === '' && this.FileType === '') {
        this.$q.notify({
          position: 'top',
          message: '提示',
          caption: '搜索条件必须必须有一个',
          color: 'red',
        })
      } else {
        let searchIndex = -1;
        if (this.FileType === '文件夹') { searchIndex = 0 } else if (this.FileType === '测试用例') { searchIndex = 1 } else if (this.FileType === '测试数据源') { searchIndex = 2 }
        this.isRootFolderFlag = true;
        let reg = new RegExp(this.searchText.trim(), 'i')
        this.FolderList = [];
        for (let i = 0; i < this.RootFolderList.length; i++) {
          if (this.searchText.trim() != '' && this.FileType != '') {
            if (this.RootFolderList[i].Name.match(reg) && this.RootFolderList[i].Type == searchIndex) {
              this.FolderList.push(this.RootFolderList[i])
            }
          } else {
            if (this.searchText.trim() != '') {
              if (this.RootFolderList[i].Name.match(reg)) {
                this.FolderList.push(this.RootFolderList[i])
              }
            } else {
              if (this.RootFolderList[i].Type == searchIndex) {
                this.FolderList.push(this.RootFolderList[i])
              }
            }
          }
        }

        for (let i = 0; i < this.ChildFolderList.length; i++) {
          if (this.searchText.trim() != '' && this.FileType != '') {
            if (this.ChildFolderList[i].Name.match(reg) && this.ChildFolderList[i].Type == searchIndex) {
              this.FolderList.push(this.ChildFolderList[i])
            }
          } else {
            if (this.searchText.trim() != '') {
              if (this.ChildFolderList[i].Name.match(reg)) {
                this.FolderList.push(this.ChildFolderList[i])
              }
            } else {
              if (this.ChildFolderList[i].Type == searchIndex) {
                this.FolderList.push(this.ChildFolderList[i])
              }
            }
          }
        }

      }
    },
    cancelSearch () {
      this.searchText = ''; this.FileType = '';
      this.isRootFolderFlag = false;
      this.prevLevel();
    },
    //------------------- 新建文件夹 ---------------------
    newFolderCancel () {
      this.FolderName = '';
      this.createFolderFixed = false;
    },
    newFolderCreate () {
      this.$q.dialog({
        title: '新建文件夹',
        message: '文件夹名称',
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
        console.log(data)
      }).onCancel(() => {
        // console.log('>>>> Cancel')
      })
    },
    //重命名
    ModifyFolderName () {
      console.log(this.selection)
      if (this.selection.length == 0) {
        this.$q.notify({
          position: 'top',
          message: '提示',
          caption: '请选择文件夹',
          color: 'red',
        })
      } else if (this.selection.length > 1) {
        this.$q.notify({
          position: 'top',
          message: '提示',
          caption: '只能选择一个文件夹',
          color: 'red',
        })
      } else {
        console.log(this.selection)
        this.$q.dialog({
          title: '重命名',
          message: '文件夹名称',
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
      this.newCancel();
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
    background: #ffffff;
    .btn {
      margin-right: 10px;
      margin-bottom: 5px;
    }
  }
  .Rootfolder {
    width: 100%;
    .Rootfolder_list {
      position: relative;
      width: 10%;
      height: 80px;
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
        overflow: hidden;
        text-overflow: ellipsis;
        white-space: nowrap;
      }
      .iconfolder {
        font-size: 50px;
      }
    }
    .Rootfolder_list:hover {
      position: relative;
      width: 10%;
      height: 80px;
      margin: 10px 0px;
      text-align: center;
      box-sizing: border-box;
      background-color: rgba(204, 232, 255, 0.8);
      .iconfolder {
        font-size: 50px;
      }
    }
  }
}
</style>