<template>
  <div id="Directory"
       class="column item-start">
    <!-- button -->
    <div class="col-auto  detail_header row">
      <div class="col-7 col-md-7 row">
        <q-btn class="btn"
               color="primary"
               label="上一级"
               v-show="RecordDirectorySite.length!=0"
               @click="toPrevLevel" />
        <q-btn class="btn"
               color="primary"
               label="返回根目录"
               v-show="RecordDirectorySite.length!=0"
               @click="returnToRoot" />
        <q-btn class="btn"
               color="primary"
               label="新建目录"
               v-show="(isSearchStatus && RecordDirectorySite.length!=0) || !isSearchStatus"
               @click="newFolderCreate" />
        <q-btn class="btn"
               color="primary"
               label="移动目录"
               @click="ChangeFileDirectory" />
        <q-btn class="btn"
               color="primary"
               label="目录重命名"
               @click="ModifyFolderName" />
        <q-btn class="btn"
               color="primary"
               label="复制"
               @click="openCopyDirector" />
        <q-btn class="btn"
               color="red"
               label="删除"
               @click="DeleteFolder" />
        <q-btn class="btn"
               color="primary"
               label="新建测试用例"
               v-show="(isSearchStatus && RecordDirectorySite.length!=0) || !isSearchStatus"
               @click="createTestCaseFixed = true;" />
        <q-btn class="btn"
               color="primary"
               label="新建测试数据源"
               v-show="(isSearchStatus && RecordDirectorySite.length!=0) || !isSearchStatus"
               @click="createDataSourceFixed = true;" />
      </div>
      <div class="col-5 col-md-5  row">

        <div class="col-8 row">
          <q-input class="col-6"
                   style="height:40px;"
                   v-model="searchText"
                   outlined
                   placeholder="请输入文件名称"
                   type="search"
                   :dense='true'>
            <template v-slot:append>
              <q-icon name="search" />
            </template>
          </q-input>

          <q-select class="col-5"
                    v-model="FileType"
                    style="margin-left:10px;height:40px;"
                    :options="['目录','测试用例','测试数据源']"
                    label="类型"
                    outlined
                    :dense='true' />
        </div>

        <div class="col-4 row">
          <q-btn class="btn col-4"
                 color="primary"
                 label="搜索"
                 @click="searchFile(1,true)" />

          <q-btn class="btn col-6"
                 color="primary"
                 label="取消搜索"
                 @click="cancelSearch" />
        </div>
      </div>
    </div>

    <div class="col-auto  pathLocation q-pa-md">
      {{currentDirectoryLocation}}
    </div>
    <!-- 目录 -->
    <div class="col-9 col-md-8  folder">
      <div class="q-pa-md">
        <div class="Rootfolder">
          <div class="Rootfolder_list"
               v-for="(value ,index) in FolderList"
               :key="index"
               @dblclick="toNextLevel(value,true,1)"
               tag="label">

            <q-checkbox v-model="selection"
                        :val="value"
                        :dense="true"
                        label=""
                        class="checkbox" />
            <span class="svg-container"
                  v-if="value.type==1">
              <svg class="icon iconfolder"
                   aria-hidden="true">
                <use xlink:href="#icon-wenjianjia2"></use>
              </svg>
            </span>
            <span class="svg-container"
                  v-if="value.type==2">
              <svg class="icon iconfolder"
                   aria-hidden="true">
                <use xlink:href="#icon--wenjian"></use>
              </svg>
            </span>
            <span class="svg-container"
                  v-if="value.type==3">
              <svg class="icon iconfolder"
                   aria-hidden="true">
                <use xlink:href="#icon-shujuyuan"></use>
              </svg>
            </span>

            <p>{{value.name}}</p>
          </div>
        </div>
      </div>
    </div>

    <div class="folderList_page">
      <el-pagination background
                     style="float:right;margin-top:10px;margin-right:5%;"
                     layout="prev, pager, next"
                     :current-page="folderPage.folderPageCurrent"
                     :page-size="100"
                     @current-change="switchFolderPage"
                     :total="folderPage.folderPageTotal">
      </el-pagination>
    </div>

    <!-- 更改文件目录 -->
    <q-dialog v-model="ChangeFileDirectoryFlag"
              persistent>
      <q-card style="width: 100%;">
        <q-card-section>
          <div class="text-h6">选择文件目录</div>
        </q-card-section>

        <q-separator />

        <TreeEntity ref="TreeEntity"
                    :existingDirectories='existingDirectories' />

        <q-separator />

        <q-card-actions align="right">
          <q-btn flat
                 label="取消"
                 color="primary"
                 @click="ChangeFileDirectoryFlag = false;" />
          <q-btn flat
                 label="确定"
                 color="primary"
                 @click="SelectDirectoryLocation" />
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

        <CreateShowTestCase ref="CSTestCase"
                            :currentDirectory="RecordDirectorySite[RecordDirectorySite.length-1]" />

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
        <CreatePut ref="createDataSource"
                   :currentDirectory="RecordDirectorySite[RecordDirectorySite.length-1]" />
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

    <!-- 查看更新TestDataSource框 -->
    <q-dialog v-model="LookDataSourceFixed"
              persistent>
      <q-card style="width:100%; max-width: 70vw;">
        <q-card-section>
          <div class="text-h6">更新测试数据源</div>
        </q-card-section>

        <q-separator />
        <CreatePut ref="putDataSource"
                   :detailData="dataSourceDetail" />

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

    <!-- 复制 -->

    <q-dialog v-model="copyDirectorFlag"
              persistent>
      <q-card style="width: 100%; max-width: 70vw;">
        <q-card-section>
          <div class="text-h6">复制目录/文件</div>
        </q-card-section>

        <q-separator />
        <CopyDirector ref="copyDirector"
                      :selection="selection"
                      :existingDirectories='existingDirectories' />
        <q-separator />

        <q-card-actions align="right">
          <q-btn flat
                 label="取消"
                 color="primary"
                 @click="copyDirectorFlag = false" />
          <q-btn flat
                 label="复制"
                 color="primary"
                 @click="copyDirectorCreate" />
        </q-card-actions>
      </q-card>
    </q-dialog>
  </div>
</template>

<script>
import * as Apis from "@/api/index"
import CreateShowTestCase from '../TestCase/component/CreateShowTestCase.vue' //创建测试用例
import TreeEntity from "@/components/TreeEntity.vue"                          //目录管理结构树
import CreatePut from "../TestDataSource/CreatePut.vue"                       //创建更新测试数据源
import CopyDirector from "./CopyDirectory.vue"                                //复制文件或目录
export default {
  name: 'Directory',
  components: {
    CreateShowTestCase,
    TreeEntity,
    CreatePut,
    CopyDirector
  },
  data () {
    return {
      FolderList: [],                 //目录列表
      selection: [],                  //选择的文件目录
      RecordDirectorySite: [],        //记录目录位置
      folderPage: {
        folderPageCurrent: null,      //文件目录页码
        folderPageTotal: null,        //总数量
      },
      isSearchStatus: false,          //是否是搜索状态
      currentDirectoryLocation: '',    //当前所在目录位置
      // -------- 搜索 -------
      searchText: '',//搜索内容
      FileType: null,//文件类型
      // -------- 更改文件目录 -------
      ChangeFileDirectoryFlag: false, //更改文件目录Flag
      ChangeFileDirectoryValue: '',   //选择文件目录值
      // -------- 新建测试用例 -------
      createTestCaseFixed: false,
      // -------- 新建更新测试数据源弹窗 -------
      dataSourceDetail: '',           //测试数据源详情数据
      createDataSourceFixed: false,   //新建测试数据源弹窗
      LookDataSourceFixed: false,     //更新测试数据源弹窗
      // -------- 复制 -------
      copyDirectorFlag: false,         //复制目录或Flag

      existingDirectories: []//复制或移动目录的时候禁止出现已选择的目录

    }
  },
  created () {
    this.getEchoLocation();
  },
  methods: {
    //回到之前的位置
    getEchoLocation (currentChange) {
      //currentChange 切换的页码
      let _this = this;
      this.selection = [];//清除选择的文件
      let Page = JSON.parse(sessionStorage.getItem('Page'));
      this.folderPage.folderPageCurrent = currentChange;
      this.isSearchStatus = JSON.parse(sessionStorage.getItem('isSearchStatus')) || false;
      //判断当前目录是否搜索状态
      if (this.isSearchStatus) {
        console.log(this.isSearchStatus)
        this.RecordDirectorySite = JSON.parse(sessionStorage.getItem('RecordSearchDirectorySite'));
        //判断是否是从测试用例页面跳转过来
        if (Page) {
          this.searchText = Page.searchText;
          this.FileType = Page.FileType;
          //判断当前搜索列表是不是根目录
          if (this.RecordDirectorySite.length == 0) {
            this.searchFile(Page.folderPageCurrent, false);
          } else {
            //执行获取子级目录接口
            this.toNextLevel(this.RecordDirectorySite[this.RecordDirectorySite.length - 1], false, Page.folderPageCurrent)
          }
          if (Page.type == 2) {
            sessionStorage.removeItem('Page')
          }
        } else {
          if (this.RecordDirectorySite.length == 0) {
            this.searchFile(currentChange, false);
          } else {
            //执行获取子级目录接口
            this.toNextLevel(this.RecordDirectorySite[this.RecordDirectorySite.length - 1], false, currentChange)
          }
        }
      } else {
        isParentChildren();
      }

      //判断当前目录位置执行哪个状态
      function isParentChildren () {
        _this.RecordDirectorySite = JSON.parse(sessionStorage.getItem('RecordDirectorySite'));
        console.log(_this.folderPage, Page)
        //如果是根目录回来的则复制空数组；
        if (_this.RecordDirectorySite == null) { _this.RecordDirectorySite = []; }
        let page = Page ? Page.folderPageCurrent : currentChange;  //回显当前的页码
        if (_this.RecordDirectorySite.length != 0) {
          //执行获取子级目录接口
          _this.toNextLevel(_this.RecordDirectorySite[_this.RecordDirectorySite.length - 1], false, page)
        } else {
          //执行获取根目录接口
          _this.getTreeEntityList(page);
        }
        sessionStorage.removeItem('Page')
      }


    },
    //获得根目录
    getTreeEntityList (Page) {
      this.$q.loading.show();
      let para = {
        parentId: null,
        matchName: '',
        page: Page ? Page : 1,
        type: null,
        pageSize: 100
      }
      Apis.getTreeEntityChildrenList(para).then(res => {
        console.log(res)
        this.folderPage.folderPageCurrent = Page ? Page : 1;
        this.folderPage.folderPageTotal = res.data.totalCount;

        this.FolderList = res.data.results;
        this.getTreeEntityTreePath();
      })
    },
    //上一级
    toPrevLevel () {
      //回显搜索状态值
      if (this.isSearchStatus) {
        let Page = JSON.parse(sessionStorage.getItem('Page'));
        if (Page) {
          this.searchText = Page.searchText;
          this.FileType = Page.FileType;
        }
      } else {
        this.searchText = '';
        this.FileType = null;
      }

      if (this.isSearchStatus && this.RecordDirectorySite.length == 1) {
        this.searchFile(this.RecordDirectorySite[0].pageCurrent, true);
        return;
      }
      let para = {
        //获取记录目录位置的上一个的id
        parentId: this.RecordDirectorySite[this.RecordDirectorySite.length - 1].id,
        page: this.RecordDirectorySite[this.RecordDirectorySite.length - 1].pageCurrent,
        pageSize: 100
      }
      this.$q.loading.show();
      Apis.getgobackpreviousTreeEntity(para).then(res => {
        console.log(res)



        //删除当前记录目录位置的最后一个并且重新保存到session
        this.RecordDirectorySite.pop();
        //判断当前是否搜索状态
        if (this.isSearchStatus) {
          sessionStorage.setItem('RecordSearchDirectorySite', JSON.stringify(this.RecordDirectorySite));
        } else {
          sessionStorage.setItem('RecordDirectorySite', JSON.stringify(this.RecordDirectorySite));
        }


        this.selection = [];
        this.folderPage.folderPageCurrent = res.data.currentPage;
        this.folderPage.folderPageTotal = res.data.totalCount;
        this.FolderList = res.data.results;


        this.getTreeEntityTreePath();

      })
    },
    //下一级
    toNextLevel (value, TypeFlag, page) {
      //回显搜索状态值
      if (this.isSearchStatus) {
        let Page = JSON.parse(sessionStorage.getItem('Page'));
        if (Page) {
          this.searchText = Page.searchText;
          this.FileType = Page.FileType;
        }
      } else {
        this.searchText = '';
        this.FileType = null;
      }
      console.log(value)
      if (value.type === 1) {
        //----------------- 当前进入的是目录 ----------------- 
        let para = {
          parentId: value.id,
          matchName: '',
          page: page || 1,
          type: null,
          pageSize: 100
        }
        this.$q.loading.show();
        Apis.getTreeEntityChildrenList(para).then(res => {
          console.log(res)


          //是否保存当前目录位置
          if (TypeFlag) {
            value.pageCurrent = this.folderPage.folderPageCurrent;
            this.RecordDirectorySite.push(value);
            //判断当前是否搜索状态
            if (this.isSearchStatus) {
              sessionStorage.setItem('RecordSearchDirectorySite', JSON.stringify(this.RecordDirectorySite));
            } else {
              sessionStorage.setItem('RecordDirectorySite', JSON.stringify(this.RecordDirectorySite));
            }
            console.log(this.RecordDirectorySite)
          }

          this.folderPage.folderPageCurrent = res.data.currentPage;
          this.folderPage.folderPageTotal = res.data.totalCount;

          this.selection = [];
          this.FolderList = res.data.results;

          this.getTreeEntityTreePath();
        })
      } else if (value.type === 2) {
        //----------------- 当前进入的是测试用例 ----------------- 
        //存储搜索条件
        sessionStorage.setItem('Page', JSON.stringify({
          searchText: this.isSearchStatus ? this.searchText.trim() : '',//搜索内容
          FileType: this.isSearchStatus ? this.FileType : null,//文件类型
          folderPageCurrent: this.folderPage.folderPageCurrent,
          folderPageTotal: this.folderPage.folderPageTotal,
          type: 2
        }))
        this.$router.push({
          name: 'DirectoryTestCaseDetail',
          query: {
            id: value.value
          },
        })
      } else if (value.type === 3) {
        //----------------- 当前进入的是测试数据源 ----------------- 
        console.log(value)
        this.getTestDataSourceDetail(value.value)
      }
    },
    //删除文件
    DeleteFolder () {
      if (this.selection.length == 0) {
        this.$q.notify({
          position: 'top',
          message: '提示',
          caption: '请选择文件（目录、测试用例、测试数据源）',
          color: 'red',
        })
      } else {
        this.$q.dialog({
          title: '提示',
          message: `您确定要删除当前选择的文件吗`,
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
          let _this = this;
          let delNum = 0;
          delFolder();
          //递归执行删除文件的操作
          function delFolder () {
            _this.$q.loading.show();
            let para = `?id=${_this.selection[delNum].id}`
            Apis.deleteTreeEntity(para).then(res => {
              console.log(res)
              if (delNum != _this.selection.length - 1) {
                delNum++;
                delFolder()
              } else {
                _this.getEchoLocation();
              }
            })
          }
        })
      }

    },
    //切换目录页码
    switchFolderPage (value) {
      console.log(value)
      this.getEchoLocation(value)
    },
    //获得文件目录路径
    getTreeEntityTreePath () {
      console.log(this.RecordDirectorySite)
      this.$q.loading.show();
      if (this.isSearchStatus) {
        if (this.RecordDirectorySite.length > 0) {
          let para = { id: this.RecordDirectorySite[this.RecordDirectorySite.length - 1].id };
          Apis.getTreeEntityTreePath(para).then((res) => {
            console.log(res)
            this.currentDirectoryLocation = `当前所在目录位置：根目录 > ` + res.data.join(' > ');
            this.$q.loading.hide();
          })
        } else {
          this.currentDirectoryLocation = `当前所在目录位置：根目录 > `;
          this.$q.loading.hide();
        }
      } else {
        if (this.RecordDirectorySite.length != 0) {
          let para = { id: this.RecordDirectorySite[this.RecordDirectorySite.length - 1].id };
          Apis.getTreeEntityTreePath(para).then((res) => {
            console.log(res)
            this.currentDirectoryLocation = '当前所在目录位置：根目录 > ' + res.data.join(' > ');
            this.$q.loading.hide();
          })
        } else {
          this.currentDirectoryLocation = '当前所在目录位置：根目录 > ';
          this.$q.loading.hide();
        }
      }

    },
    //---------------------------------------------- 搜索 -------------------------------------------
    //搜索
    searchFile (Page, saveDirectory) {
      console.log(Page)
      console.log(this.searchText)
      if (!this.searchText.trim() && !this.FileType) {
        this.$q.notify({
          position: 'top',
          message: '提示',
          caption: '请填写搜索内容或选择类型进行搜索',
          color: 'red',
        })
        return;
      }
      console.log(this.FileType)
      let FileType = null;
      switch (this.FileType) {
        case '目录':
          FileType = 1;
          break;
        case '测试用例':
          FileType = 2;
          break;
        case '测试数据源':
          FileType = 3;
          break;
      }
      let para = {
        matchName: this.searchText,
        page: Page || 1,
        type: FileType,
        pageSize: 100
      }
      console.log(para)
      this.$q.loading.show();
      Apis.getTreeEntityList(para).then(res => {
        console.log(res)
        this.folderPage.folderPageCurrent = res.data.currentPage;
        this.folderPage.folderPageTotal = res.data.totalCount;

        this.isSearchStatus = true;

        sessionStorage.setItem('Page', JSON.stringify({
          searchText: this.searchText.trim(),//搜索内容
          FileType: this.FileType,//文件类型
          folderPageCurrent: this.folderPage.folderPageCurrent,
          folderPageTotal: this.folderPage.folderPageTotal,
          type: 1
        }))
        if (saveDirectory) {
          sessionStorage.setItem('RecordSearchDirectorySite', JSON.stringify([]));    //保存搜索目录位置
          sessionStorage.setItem('isSearchStatus', true);
          this.RecordDirectorySite = JSON.parse(sessionStorage.getItem('RecordSearchDirectorySite'));
        }
        console.log(this.RecordDirectorySite)

        this.FolderList = res.data.results;
        this.getTreeEntityTreePath();
      })
    },
    //取消搜索
    cancelSearch () {
      if (this.isSearchStatus) {
        sessionStorage.removeItem('Page');
        sessionStorage.setItem('RecordSearchDirectorySite', JSON.stringify([]));
        sessionStorage.setItem('isSearchStatus', false)
        this.isSearchStatus = false;
        this.searchText = ''; this.FileType = null;
        this.getEchoLocation();
      }
    },
    //---------------------------------------------- 目录操作 -------------------------------------------
    //返回根目录
    returnToRoot () {
      //判断当前是否搜索状态
      if (this.isSearchStatus) {
        this.RecordDirectorySite = [];
        sessionStorage.removeItem('Page');
        sessionStorage.setItem('RecordSearchDirectorySite', JSON.stringify([]));
        sessionStorage.setItem('isSearchStatus', false)
        this.isSearchStatus = false;
        this.searchText = ''; this.FileType = null;
        this.getTreeEntityList();
      } else {
        this.RecordDirectorySite = [];
        this.getTreeEntityList();
        sessionStorage.setItem('RecordDirectorySite', JSON.stringify(this.RecordDirectorySite));
      }
    },
    //新建目录
    newFolderCreate () {
      this.$q.dialog({
        title: '新建目录',
        message: '目录名称（目录名称不能为空）',
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
        if (!data.trim()) {
          this.$q.notify({
            position: 'top',
            message: '提示',
            caption: '目录名称不能为空',
            color: 'red',
          })
          return;
        }
        this.$q.loading.show();
        let para = {
          Name: data,
          //通过保存的目录位置来判断是否是根目录
          FolderID: this.RecordDirectorySite.length ? this.RecordDirectorySite[this.RecordDirectorySite.length - 1].id : null,
        }
        Apis.postCreateTreeEntity(para).then(res => {
          console.log(res)
          this.getEchoLocation();
        })
      })
    },
    //目录重命名
    ModifyFolderName () {
      console.log(this.selection)
      if (this.selection.length == 0) {
        this.$q.notify({
          position: 'top',
          message: '提示',
          caption: '请选择目录进行修改',
          color: 'red',
        })
      } else if (this.selection.length > 1) {
        this.$q.notify({
          position: 'top',
          message: '提示',
          caption: '只能选择一个目录进行修改',
          color: 'red',
        })
      } else {
        if (this.selection[0].type != 1) {
          this.$q.notify({
            position: 'top',
            message: '提示',
            caption: '只能选择目录进行修改',
            color: 'red',
          })
          return;
        }
        this.$q.dialog({
          title: '目录重命名',
          message: '目录名称',
          prompt: {
            model: this.selection[0].name,
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
          if (!data.trim()) {
            this.$q.notify({
              position: 'top',
              message: '提示',
              caption: '目录名称不能为空',
              color: 'red',
            })
            return;
          }
          let para = `?id=${this.selection[0].id}&name=${data}`
          this.$q.loading.show();
          Apis.putTreeEntityName(para).then(res => {
            console.log(res)
            this.$q.notify({
              position: 'top',
              message: '提示',
              caption: '修改成功',
              color: 'secondary',
            })
            this.getEchoLocation();
          })
        })
      }
    },
    //移动目录
    ChangeFileDirectory () {
      if (this.selection.length == 0) {
        this.$q.notify({
          position: 'top',
          message: '提示',
          caption: '请选择文件（目录、测试用例、测试数据源）',
          color: 'red',
        })
      } else {
        //存储目录防止移动时出现相同目录
        this.existingDirectories = [];
        for (let i = 0; i < this.selection.length; i++) {
          if (this.selection[i].type == 1) { this.existingDirectories.push(this.selection[i]) }
        }
        this.ChangeFileDirectoryFlag = true;
      }
    },
    //确定选择目录位置
    SelectDirectoryLocation () {
      let _this = this;
      let putNum = 0;
      this.ChangeFileDirectoryValue = this.$refs.TreeEntity.getDirectoryLocation();
      //判断是否选择要移动的目录
      if (!this.ChangeFileDirectoryValue) {
        this.$q.notify({
          position: 'top',
          message: '提示',
          caption: '请选择要移动到的目录位置',
          color: 'red',
        })
        return;
      } else {
        this.$q.loading.show();
        UpdateFolder();
      }
      console.log(this.ChangeFileDirectoryValue)
      //递归执行移动目录操作
      function UpdateFolder () {
        let para = '';
        if (_this.ChangeFileDirectoryValue.id) {
          para = `?id=${_this.selection[putNum].id}&parentId=${_this.ChangeFileDirectoryValue.id}`
        } else {
          para = `?id=${_this.selection[putNum].id}`
        }
        Apis.putTreeEntityParent(para).then(res => {
          console.log(res)
          if (putNum != _this.selection.length - 1) {
            putNum++;
            UpdateFolder()
          } else {
            _this.ChangeFileDirectoryFlag = false;
            _this.getEchoLocation();
          }
        })
      }
    },
    //---------------------------------------------- 新建取消创建测试用例 -------------------------------------------
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
        this.$q.notify({
          position: 'top',
          message: '提示',
          caption: '创建成功',
          color: 'secondary',
        })
        this.newCancel()
        this.getEchoLocation();
      })
    },
    //---------------------------------------------- 新建取消创建测试数据源 -------------------------------------------
    //新建TestDataSource
    newDataSourceCreate () {
      if (!this.$refs.createDataSource.newCreate()) { return }
      let para = this.$refs.createDataSource.newCreate()
      this.$q.loading.show()
      Apis.postCreateTestDataSource(para).then(() => {
        this.getEchoLocation();
        this.newDataSourceCancel();
        this.$q.notify({
          position: 'top',
          message: '提示',
          caption: '创建成功',
          color: 'secondary',
        })
      })
    },
    //取消新建TestDataSource
    newDataSourceCancel () {
      this.$refs.createDataSource.newCancel();
      this.createDataSourceFixed = false;
    },
    //获得TestDataSource详情数据
    getTestDataSourceDetail (valueId) {
      this.$q.loading.show()
      let para = {
        id: valueId
      }
      Apis.getTestDataSourceDetail(para).then((res) => {
        console.log(res)
        let data = res.data;
        this.dataSourceDetail = {
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
        this.getEchoLocation();
        this.cancelPutDataSource();
        this.$q.notify({
          position: 'top',
          message: '提示',
          caption: '更新成功',
          color: 'secondary',
        })
      })
    },
    //---------------------------------------------- 复制 -------------------------------------------
    //打开复制弹窗
    openCopyDirector () {
      if (this.selection.length == 0) {
        this.$q.notify({
          position: 'top',
          message: '提示',
          caption: '请选择要复制的目录文件',
          color: 'red',
        })
        return;
      }
      //存储目录防止移动时出现相同目录
      this.existingDirectories = [];
      for (let i = 0; i < this.selection.length; i++) {
        if (this.selection[i].type == 1) { this.existingDirectories.push(this.selection[i]) }
      }
      this.copyDirectorFlag = true;
    },
    //复制按钮
    copyDirectorCreate () {
      this.$refs.copyDirector.copyDirectorCreate();
      this.copyDirectorFlag = false;
      this.selection = [];
    }
  }
}
</script>

<style lang="scss" scoped>
#Directory {
  position: fixed;
  width: 100%;
  height: 100%;
  .detail_header {
    padding: 10px 16px 5px;
    width: 100%;
    box-sizing: border-box;
    border-bottom: 1px solid #ccc;
    background: #ffffff;
    .btn {
      margin-right: 10px;
      margin-bottom: 5px;
      height: 36px;
    }
  }

  .pathLocation {
    width: 100%;
    box-sizing: border-box;
    border-bottom: 1px solid #ccc;
    word-wrap: break-word;
  }

  .folder {
    padding-bottom: 30px;
    box-sizing: border-box;
    overflow-y: scroll;
    .Rootfolder {
      width: 100%;
      .Rootfolder_list {
        position: relative;
        width: 10%;
        display: inline-block;
        vertical-align: top;
        margin: 8px 0px;
        padding: 5px 0px;
        text-align: center;
        box-sizing: border-box;
        .checkbox {
          position: absolute;
          left: 10%;
          top: 30px;
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
        display: inline-block;
        vertical-align: top;
        margin: 8px 0px;
        padding: 5px 0px;
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
</style>