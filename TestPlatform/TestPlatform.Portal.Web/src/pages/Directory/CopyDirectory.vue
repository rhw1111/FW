<template>
  <div class="new_input">
    <!-- 更改文件目录 -->
    <q-dialog v-model="ChangeFileDirectoryFlag"
              persistent>
      <q-card style="width: 100%;">
        <q-card-section>
          <div class="text-h6">选择文件目录</div>
        </q-card-section>

        <q-separator />

        <TreeEntity ref="TreeEntity" />

        <q-separator />

        <q-card-actions align="right">
          <q-btn flat
                 label="取消"
                 color="primary"
                 @click="ChangeFileDirectoryFlag = false" />
          <q-btn flat
                 label="确定"
                 color="primary"
                 @click="SelectDirectoryLocation" />
        </q-card-actions>
      </q-card>
    </q-dialog>
    <div class="row input_row">

      <q-input v-model="ChangeFileDirectoryName"
               :dense="false"
               class="col"
               readonly
               placeholder="点击右侧加号选择文件要复制到哪个目录">
        <template v-slot:before>
          <span style="font-size:14px">文件目录位置:</span>
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
  </div>
</template>
<script>
import * as Apis from "@/api/index"
import TreeEntity from "@/components/TreeEntity.vue"                          //目录管理结构树
export default {
  props: ['selection'],
  components: {
    TreeEntity
  },
  data () {
    return {
      directorName: '',//复制目录的名称

      ChangeFileDirectoryFlag: false, //树状图弹窗是否显示flag
      ChangeFileDirectoryName: '',    //选择的目录名称
      ChangeFileDirectoryId: null,    //选择的目录id
      ChangeFileDirectoryValue: '',   //选择的目录

      selectionArr: [],               //初始选择目录或文件的数组
      dataStructure: [],              //目录数据结构数组
      FileList: [],                   //文件数组
    }
  },
  mounted () {
    this.selectionArr = this.selection;
  },
  methods: {
    //打开目录选择弹窗
    ChangeFileDirectory () {
      this.ChangeFileDirectoryFlag = true;
    },
    //选择目录位置   
    SelectDirectoryLocation () {
      if (!this.$refs.TreeEntity.getDirectoryLocation()) {
        this.$q.notify({
          position: 'top',
          message: '提示',
          caption: '请选择目录位置',
          color: 'red',
        })
        return;
      }
      this.ChangeFileDirectoryName = this.$refs.TreeEntity.getDirectoryLocation().label;
      this.ChangeFileDirectoryId = this.$refs.TreeEntity.getDirectoryLocation().id;
      this.ChangeFileDirectoryValue = this.$refs.TreeEntity.getDirectoryLocation();
      this.ChangeFileDirectoryFlag = false;
    },
    //复制创建
    copyDirectorCreate () {
      if (!this.ChangeFileDirectoryName) {
        this.$q.notify({
          position: 'top',
          message: '提示',
          caption: '请选择目录位置',
          color: 'red',
        })

      }
      console.log(this.selectionArr)
      this.recheckingSelection(this.ChangeFileDirectoryValue);
    },
    //创建数据结构和文件数组
    recheckingSelection (value) {
      this.$q.loading.show();
      for (let i = 0; i < this.selectionArr.length; i++) {
        if (this.selectionArr[i].type == 1) {
          this.dataStructure.push({
            originalNode: this.selectionArr[i],             //原有节点 == 原有目录
            newNode: value                                  //新节点   == 新目录（要创建到哪个目录下）
          })
        } else {
          this.FileList.push(this.selectionArr[i])
        }
      }
      console.log(this.dataStructure, this.FileList)
      this.CreateDataStructureAndFiles(this.dataStructure[0], this.ChangeFileDirectoryValue);
    },
    //复制创建文件目录和文件
    CreateDataStructureAndFiles (value, copyFileValue) {
      //value(执行的目录)        copyFileValue(当前要创建文件的新目录)
      console.log('------------开始执行复制操作---------------')
      let _this = this;
      let copyFileNum = 0;//递归执行创建文件的数
      //判断文件数组是否有，有则先执行创建文件，没有则直接执行创建目录操作
      if (this.FileList.length != 0) {
        copyFile();
      } else {
        copyFolder()
      }


      //创建文件
      function copyFile () {
        //递归执行创建文件的操作
        if (copyFileNum != _this.FileList.length) {
          console.log(_this.FileList, copyFileNum)
          let para = {
            ID: _this.FileList[copyFileNum].value,
            ParentID: copyFileValue.id,
            Type: _this.FileList[copyFileNum].type == 2 ? 'TestCase' : 'TestDataSource'
          }
          Apis.postTreeEntityCopyFile(para).then(res => {
            console.log(res)
            //如果当前目录已存在相同文件则提示
            if (!res.data) {
              _this.$q.notify({
                position: 'top',
                message: '提示',
                caption: `已存在${_this.FileList[copyFileNum].name}`,
                color: 'red',
              })
            }
            copyFileNum++;
            copyFile();
          })
        } else {
          //递归完成创建文件的操作
          _this.FileList = [];
          //如果value没有则已经是最后的操作，复制结束
          if (value) {
            //执行创建目录
            copyFolder();
          } else {
            _this.$q.notify({
              position: 'top',
              message: '提示',
              caption: '复制完成',
              color: 'secondary',
            })
            console.log('------------复制操作结束---------------')
            _this.$q.loading.hide();
          }
        }
      }



      //创建目录
      function copyFolder () {
        let para = {
          Name: value.originalNode.name,
          FolderID: value.newNode.id
        }
        Apis.postTreeEntityCopyFolder(para).then(res => {
          console.log(res)
          getTreeChildren(value.originalNode, res.data)
        })
      }



      //获取原有节点下的子节点，如果是目录，创建新的数据结构记录
      function getTreeChildren (oldValue, newValue) {
        let para = {
          parentId: oldValue.id,
          matchName: '',
          page: 1,
          type: null,
          pageSize: 99999
        }
        Apis.getTreeEntityChildrenList(para).then(res => {
          console.log(res)
          if (res.data.results.length != 0) {
            let childrenArr = res.data.results;
            //创建新的数据结构
            for (let i = 0; i < childrenArr.length; i++) {
              if (childrenArr[i].type == 1) {
                _this.dataStructure.push({
                  originalNode: childrenArr[i],
                  newNode: newValue
                })
              } else {
                _this.FileList.push(childrenArr[i])
              }
            }
          }
          //在数据结构当中删除已经执行完成的目录
          _this.dataStructure.splice(0, 1, '');
          _this.dataStructure = _this.dataStructure.filter(item => item);
          console.log(_this.dataStructure, _this.FileList)
          //判断数据结构是否还有需要执行的目录
          if (_this.dataStructure.length != 0) {
            //数据结构里还有则继续执行递归操作
            _this.CreateDataStructureAndFiles(_this.dataStructure[0], newValue);
          } else {
            //数据结构里没有了，判断文件数组是否还有
            if (_this.FileList.length != 0) {
              //文件数组有则继续执行
              _this.CreateDataStructureAndFiles(_this.dataStructure[0], newValue);
            } else {
              //文件数组没有则复制结束
              _this.$q.notify({
                position: 'top',
                message: '提示',
                caption: '复制完成',
                color: 'secondary',
              })
              console.log('------------复制操作结束---------------')
              _this.$q.loading.hide();
            }
          }

        })
      }

    },

  }
}
</script>

<style lang="scss" scoped>
</style>
<style lang="scss">
.new_input {
  width: 100%;
  padding: 10px 30px;
  .input_row {
    margin-bottom: 30px;
  }
}
</style>