<template>
  <div class="TestDataSource">
    <!-- TestDataSource列表 -->
    <div class="q-pa-md">
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
                   @click="toDetail(props)" />
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
    <!-- 新增TestDataSource框 -->
    <q-dialog v-model="createFixed"
              persistent>
      <q-card style="width:100%">
        <q-card-section>
          <div class="text-h6">创建测试数据源</div>
        </q-card-section>

        <q-separator />
        <div class="new_input">
          <div class="row input_row">
            <q-input v-model="Name"
                     :dense="false"
                     class="col">
              <template v-slot:before>
                <span style="font-size:14px">名称:</span>
              </template>
            </q-input>
            <q-input v-model="Type"
                     :dense="false"
                     class="col"
                     style="margin-left:50px;">
              <template v-slot:before>
                <span style="font-size:14px">类型:</span>
              </template>
            </q-input>
          </div>
          <div class="row input_row">
            <q-input v-model="Data"
                     :dense="false"
                     class="col-xs-12"
                     type="textarea"
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
      Type: '',
      Data: '',

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
        { name: 'data', label: '数据', align: 'left', field: 'data', },
        { name: 'id', label: '操作', align: 'left', field: 'id', },
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
    getTestDataSource (page) {
      this.$q.loading.show()
      let para = {
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
    //打开
    openCreate () {
      this.Name = '';
      this.Type = '';
      this.Data = '';
      this.createFixed = true;
    },
    //跳转到详情
    toDetail (env) {
      this.$router.push({
        name: 'TestDataSourceDetail',
        query: {
          id: env.row.id
        }
      })
    },
    //页码切换
    switchPage (value) {
      this.getTestDataSource(value)
    },
    //新建TestDataSource
    newCreate () {
      let para = {
        Name: this.Name,
        Type: this.Type,
        Data: this.Data,
      }
      if (this.Name && this.Type && this.Data) {
        this.$q.loading.show()
        Apis.postCreateTestDataSource(para).then(() => {
          this.getTestDataSource();
          this.createFixed = false;
          this.$q.notify({
            position: 'top',
            message: '提示',
            caption: '创建成功',
            color: 'secondary',
          })
        })
      } else {
        this.$q.notify({
          position: 'top',
          message: '提示',
          caption: '请填写完整信息',
          color: 'red',
        })
      }
    },
    //取消新建TestDataSource
    newCancel () {
      this.createFixed = false;
    },
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
          let para = `?id=${this.selected[0].id}`;
          Apis.deleteTestDataSource(para).then(() => {
            this.getTestDataSource();
          })
        } else if (this.selected.length > 1) {
          //批量删除TestDataSource
          this.$q.loading.show()
          let delArr = [];
          for (let i = 0; i < this.selected.length; i++) {
            delArr.push(this.selected[i].id)
          }
          let para = {
            delArr: delArr
          };
          Apis.deleteTestDataSourceArr(para).then(() => {
            this.getTestDataSource();
          })
        }
      })
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
  .q-table {
    table-layout: fixed;
    .cursor-pointer {
      .text-left {
        white-space: nowrap;
        overflow: hidden;
        text-overflow: ellipsis;
      }
      .q-table--col-auto-width {
        width: 75px;
      }
    }
  }
}
</style>
<style lang="scss">
.q-table {
  table-layout: fixed;
  .cursor-pointer {
    .text-left {
      white-space: nowrap;
      overflow: hidden;
      text-overflow: ellipsis;
    }
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
</style>