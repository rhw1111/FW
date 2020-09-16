<template>
  <div class="TreeEntity">
    <div class="q-pa-md">
      <el-tree :data="simple"
               :props="defaultProps"
               :highlight-current="true"
               :expand-on-click-node="false"
               @node-expand="unfoldTree"
               @node-click="handleNodeClick"></el-tree>
    </div>
  </div>
</template>

<script>
import * as Apis from "@/api/index"
export default {
  name: 'TreeEntity',
  props: ['existingDirectories'],
  data () {
    return {
      //目录
      simple: [
        {
          id: null,
          label: '根目录',
          children: [
          ]
        }
      ],
      //目录配置
      defaultProps: {
        children: 'children',
        label: 'label'
      },
      DisablesSelectedDirectories: [],//禁止选择的目录
      SelectLocation: '',//选择的位置

    }
  },
  mounted () {
    this.DisablesSelectedDirectories = this.existingDirectories || [];
    console.log(this.DisablesSelectedDirectories)
    this.getTreeEntityList();
  },
  methods: {
    //获得目录
    getTreeEntityList (Page, parentID) {
      this.$q.loading.show();
      let para = {
        parentId: parentID || null,
        matchName: '',
        page: Page ? Page : 1,
        type: 1,
        pageSize: 100
      }
      Apis.getTreeEntityChildrenList(para).then(res => {
        console.log(res)
        let resultsArr = res.data.results;
        for (let i = 0; i < res.data.results.length; i++) {
          for (let j = 0; j < this.DisablesSelectedDirectories.length; j++) {
            if (this.DisablesSelectedDirectories[j].parentID == res.data.results[i].id || this.DisablesSelectedDirectories[j].id == res.data.results[i].id) {
              resultsArr.splice(i, 1, '');
            }
          }
        }
        resultsArr = resultsArr.filter(item => item);
        for (let i = 0; i < resultsArr.length; i++) {

          this.simple[0].children.push({
            id: resultsArr[i].id,
            label: resultsArr[i].name,
            parentID: resultsArr[i].parentID,
            type: resultsArr[i].type,
            value: resultsArr[i].value,
            children: [
              {}
            ]
          })
        }


        this.$q.loading.hide();
      })
    },
    //展开
    unfoldTree (value) {
      console.log(value)
      console.log(this.DisablesSelectedDirectories)
      this.$q.loading.show();
      let para = {
        parentId: value.id,
        matchName: '',
        page: 1,
        type: 1,
        pageSize: 100
      }
      Apis.getTreeEntityChildrenList(para).then(res => {
        console.log(res)

        let resultsArr = res.data.results;
        for (let i = 0; i < res.data.results.length; i++) {
          console.log(res.data.results)
          for (let j = 0; j < this.DisablesSelectedDirectories.length; j++) {
            if (this.DisablesSelectedDirectories[j].parentID == res.data.results[i].id || this.DisablesSelectedDirectories[j].id == res.data.results[i].id) {
              resultsArr[i] = '';
              break;
            }
          }
        }
        resultsArr = resultsArr.filter(item => item);
        value.children = [];
        for (let i = 0; i < resultsArr.length; i++) {
          if (!value.children) {
            this.$set(value, 'children', [{}]);
          }
          value.children.push({
            id: resultsArr[i].id,
            label: resultsArr[i].name,
            parentID: resultsArr[i].parentID,
            type: resultsArr[i].type,
            value: resultsArr[i].value,
            children: [{}]
          })
        }

        // }
        this.$q.loading.hide();
      })
    },
    //选择目录
    handleNodeClick (data) {
      console.log(data);
      this.$emit('getDirectoryLocation', data)
      this.SelectLocation = data;
    },
    //获得选择目录的位置
    getDirectoryLocation () {
      return this.SelectLocation;
    }
  }
}
</script>

<style lang="scss" scoped>
</style>