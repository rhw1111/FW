<template>
  <div class="TestDataSource">
    <!-- TestDataSource列表 -->
    <div class="q-pa-md">
      <q-table title="TestCase列表"
               :data="TestDataSourceList"
               :columns="columns"
               row-key="id"
               @row-dblclick="toDetail"
               :rows-per-page-options=[0]>

        <template v-slot:top-right>
          <q-btn class="btn"
                 color="primary"
                 label="新 增"
                 @click="openCreate" />
        </template>
        <template v-slot:bottom
                  class="row">
          <q-pagination v-model="pagination.page"
                        :max="pagination.rowsNumber"
                        :input="true"
                        class="col offset-md-10">
          </q-pagination>
        </template>
        <template v-slot:body-cell-id="props">
          <q-td class="text-right"
                :props="props">
            <q-btn class="btn"
                   style="background: #FF0000; color: white"
                   label="删 除" />
          </q-td>
        </template>
      </q-table>
    </div>
    <!-- 新增TestDataSource框 -->
    <q-dialog v-model="createFixed"
              persistent>
      <q-card style="width:100%">
        <q-card-section>
          <div class="text-h6">创建TestDataSource</div>
        </q-card-section>

        <q-separator />
        <div class="new_input">
          <div class="row">
            <q-input v-model="Name"
                     :dense="false"
                     class="col">
              <template v-slot:before>
                <span style="font-size:14px">Name:</span>
              </template>
            </q-input>
            <q-input v-model="type"
                     :dense="false"
                     class="col"
                     style="margin-left:50px;">
              <template v-slot:before>
                <span style="font-size:14px">Type:</span>
              </template>
            </q-input>
          </div>
          <div class="row">
            <q-input v-model="data"
                     :dense="false"
                     class="col-xs-12"
                     type="textarea"
                     outlined>
              <template v-slot:before>
                <span style="font-size:14px">Data:</span>
              </template>
            </q-input>
          </div>
        </div>

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
  </div>
</template>

<script>
import * as Apis from "@/api/index"
export default {
  name: 'TestDataSource',
  data () {
    return {
      createFixed: false,  //新增Flag
      TestDataSourceList: [], //TestDataSource列表

      Name: '',
      type: '',
      data: '',

      selected: [],   //表格选择
      //表格配置
      columns: [
        {
          name: 'name',
          required: true,
          label: 'Name',
          align: 'left',
          field: row => row.name,
          format: val => `${val}`,
        },
        { name: 'type', align: 'left', label: 'Type', field: 'type', },
        { name: 'data', label: 'Data', align: 'left', field: 'data', },
        { name: 'id', label: '', align: 'left', field: 'id', },
      ],
      //分页配置
      pagination: {
        page: 1,          //页码
        rowsNumber: 1     //总页数
      },
    }
  },
  mounted () {
    this.getTestDataSource();
  },
  methods: {
    //获得TestDataSource列表
    getTestDataSource () {
      let para = {
        matchName: '',
        page: 1,
        pageSize: 50
      }
      Apis.getTestDataSource(para).then((res) => {
        console.log(res)
        this.TestDataSourceList = res.data.results;
      })
    },
    //打开
    openCreate () {

    },
    //跳转到详情
    toDetail () {

    },
    //新建TestDataSource
    newCreate () {

    },
    //取消新建TestDataSource
    newCancel () {

    }
  }
}
</script>

<style lang="scss" scoped>
.TestDataSource {
  width: 100%;
  overflow: hidden;
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
    margin-top: 40px;
  }
}
</style>