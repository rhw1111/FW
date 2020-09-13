<template>
  <div>
    <!-- 更改文件目录 -->
    <q-dialog v-model="ChangeFileDirectoryFlag"
              persistent>
      <q-card style="width: 100%;">
        <q-card-section>
          <div class="text-h6">选择文件目录位置</div>
        </q-card-section>

        <q-separator />

        <TreeEntity ref="TreeEntity" />

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
    <div class="new_input">
      <div class="row input_row">
        <q-input v-model="Name"
                 :dense="false"
                 class="col">
          <template v-slot:before>
            <span style="font-size:14px">名称:</span>
          </template>
        </q-input>
        <q-select v-model="Type"
                  :options="['String','Int','Json','Label']"
                  class="col"
                  :dense="false">
          <template v-slot:before>
            <span style="font-size:14px">类型:</span>
          </template>
          <template v-slot:prepend>
          </template>
        </q-select>
      </div>
      <div class="row input_row">
        <q-input v-model="ChangeFileDirectoryName"
                 :dense="false"
                 class="col"
                 readonly
                 placeholder="点击右侧加号选择文件目录位置">
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
      <div class="row input_row">
        <q-input v-model="Data"
                 :dense="false"
                 class="col-xs-12"
                 type="textarea"
                 :input-style="{height:'380px'}"
                 outlined>
          <template v-slot:before>
            <span style="font-size:14px">数据:</span>
          </template>
        </q-input>
      </div>
    </div>
  </div>
</template>

<script>
import * as Apis from "@/api/index"
import TreeEntity from "@/components/TreeEntity.vue"  //目录管理结构树
export default {
  props: ['detailData', 'currentDirectory'],
  components: { TreeEntity },
  data () {
    return {
      SelectedId: '',//选中的详情ID
      Name: '', //数据源名称
      Type: '', //数据源类型
      Data: '', //数据源内容

      // -------- 更改文件目录 -------
      ChangeFileDirectoryFlag: false,//更改文件目录Flag
      ChangeFileDirectoryName: '',//数据源保存的文件目录名称
      ChangeFileDirectoryId: null,//数据源保存的文件目录
    }
  },
  mounted () {
    //判断是不是详情数据
    if (this.detailData) {
      let value = this.detailData;
      console.log(value)
      this.SelectedId = value.SelectedId;
      this.Name = value.Name;
      this.Type = value.Type;
      this.Data = value.Data;
      if (value.ChangeFileDirectoryId) {
        this.getTreeEntityTreePath(value.ChangeFileDirectoryId, true);
      } else {
        this.ChangeFileDirectoryName = value.ChangeFileDirectoryName != '' ? value.ChangeFileDirectoryName : '根目录';
      }
      this.ChangeFileDirectoryId = value.ChangeFileDirectoryId;
    }
    //判断当前的目录是什么
    if (this.currentDirectory) {
      this.getTreeEntityTreePath(this.currentDirectory.id);
      this.ChangeFileDirectoryId = this.currentDirectory.id;
    } else {
      this.ChangeFileDirectoryName = '根目录';
    }
  },
  methods: {
    //---------------------------------------------- 新建取消创建测试数据源 -------------------------------------------
    //新建TestDataSource
    newCreate () {
      let para = {
        Name: this.Name.trim(),
        Type: this.Type,
        Data: this.Data.trim(),
        FolderID: this.ChangeFileDirectoryId
      }
      if (this.Name.trim() && this.Type && this.Data.trim()) {
        if (!this.isDataType(this.Type)) { return; }
        return para
      } else {
        this.$q.notify({
          position: 'top',
          message: '提示',
          caption: '请填写完整信息',
          color: 'red',
        })
        return false
      }
    },
    //取消新建TestDataSource
    newCancel () {
      this.Name = '';
      this.Type = '';
      this.Data = '';
    },
    //---------------------------------------------- 更新取消创建测试数据源 -------------------------------------------
    //取消更新测试数据源
    cancelPutDataSource () {
      this.SelectedId = '';
      this.Name = '';
      this.Type = '';
      this.Data = '';
      this.ChangeFileDirectoryName = '';
      this.ChangeFileDirectoryId = null;
    },
    //更新TestDataSource
    putTestDataSource () {
      let para = {
        ID: this.SelectedId,
        Name: this.Name.trim(),
        Type: this.Type,
        Data: this.Data.trim(),
        FolderID: this.ChangeFileDirectoryId,
      }
      if (this.SelectedId && this.Name.trim() && this.Type && this.Data.trim()) {
        if (!this.isDataType(this.Type)) { return; }
        this.$q.loading.show()
        return para
      } else {
        this.$q.notify({
          position: 'top',
          message: '提示',
          caption: '请填写完整信息',
          color: 'red',
        })
        return false
      }
    },
    //---------------------------------------------- 判断类型 -------------------------------------------
    //判断类型是否正确
    isDataType (type) {
      if (type == 'Int') {
        if (!Number(this.Data)) {
          this.$q.notify({
            position: 'top',
            message: '提示',
            caption: '当前数据不是Int类型',
            color: 'red',
          })
          return false;
        }
        return true;
      } else if (type == 'Json') {
        if (!this.isJSON(this.Data.trim())) {
          return false;
        }
        return true
      } else {
        return true;
      }
    },
    //判断是否是JSON格式
    isJSON (str) {
      if (typeof str == 'string') {
        try {
          var obj = JSON.parse(str);
          if (typeof obj == 'object' && obj) {
            return true
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
    //---------------------------------------------- 目录管理 -------------------------------------------
    //打开文件目录位置弹窗
    ChangeFileDirectory () {
      this.ChangeFileDirectoryFlag = true;
    },
    //选择目录位置   
    SelectDirectoryLocation () {
      if (this.$refs.TreeEntity.getDirectoryLocation().id) {
        this.getTreeEntityTreePath(this.$refs.TreeEntity.getDirectoryLocation().id);
      } else {
        this.ChangeFileDirectoryName = this.$refs.TreeEntity.getDirectoryLocation().label;
      }
      this.ChangeFileDirectoryId = this.$refs.TreeEntity.getDirectoryLocation().id || null;
      this.ChangeFileDirectoryFlag = false;
    },
    //获得文件目录路径
    getTreeEntityTreePath (ID, isDetail) {
      //ID 当前文件目录ID  isDetail 是否是进入测试数据源或测试用例
      console.log(ID)
      this.$q.loading.show();
      let para = { id: ID };
      Apis.getTreeEntityTreePath(para).then((res) => {
        console.log(res)
        if (!isDetail) {
          this.ChangeFileDirectoryName = `根目录 > ` + res.data.join(' > ');
          this.$q.loading.hide();
        } else {
          res.data.pop();
          this.ChangeFileDirectoryName = `根目录 > ` + res.data.join(' > ');
          this.$q.loading.hide();
        }
      })
    },
  }
}
</script>

<style lang="scss" scoped>
</style>