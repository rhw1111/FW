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

      SelectLocation: '',//选择的位置

    }
  },
  mounted () {
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
        for (let i = 0; i < res.data.results.length; i++) {
          this.simple[0].children.push({
            id: res.data.results[i].id,
            label: res.data.results[i].name,
            parentID: res.data.results[i].parentID,
            type: res.data.results[i].type,
            value: res.data.results[i].value,
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

        value.children = [];
        for (let i = 0; i < res.data.results.length; i++) {
          if (!value.children) {
            this.$set(value, 'children', [{}]);
          }
          value.children.push({
            id: res.data.results[i].id,
            label: res.data.results[i].name,
            parentID: res.data.results[i].parentID,
            type: res.data.results[i].type,
            value: res.data.results[i].value,
            children: [{}]
          })
        }
        this.$q.loading.hide();
      })
    },
    //选择目录
    handleNodeClick (data) {
      console.log(data);
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