<template>
  <div class="col-md-7 col-sm-12 col-xs-12">
    <!-- SSH端口列表 -->
    <q-table title="SSH终结点列表"
             :data="SSHEndpointList"
             :columns="SSHEndpointColumns"
             selection="multiple"
             :selected.sync="SSHEndpointSelected"
             row-key="id"
             :rows-per-page-options=[0]
             table-style="max-height: 500px"
             no-data-label="暂无数据更新">

      <template v-slot:top-right>
        <q-btn class="btn"
               color="primary"
               label="新 增"
               style="margin-right:20px;"
               @click="openSSHCreate" />
        <q-btn class="btn"
               color="red"
               label="删 除"
               @click="deleteSSH" />
      </template>
      <template v-slot:body-cell-id="props">
        <q-td class="text-left"
              :props="props">
          <q-btn class="btn"
                 color="primary"
                 style="margin-right:20px;"
                 label="更 新"
                 @click="toSSHEndpointDetail(props)" />
          <q-btn class="btn"
                 color="red"
                 label="删 除"
                 @click="delectSelectSSHEndpoint(props)" />
        </q-td>
      </template>
      <template v-slot:bottom
                class="row">
        <q-pagination v-model="SSHEndpointPagination.page"
                      :max="SSHEndpointPagination.rowsNumber"
                      :input="true"
                      class="col offset-md-9"
                      @input="SSHNextPage">
        </q-pagination>
      </template>
    </q-table>
    <!-- 新增SSH端口框 -->
    <q-dialog v-model="createSSHEndpointFlag"
              persistent>
      <q-card style="width:100%;max-width:50vw">
        <q-card-section>
          <div class="text-h6">创建SSH终结点</div>
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
            <q-select v-model="Type"
                      :options="['Default']"
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
            <q-input v-model="Configuration"
                     :dense="false"
                     class="col-xs-12"
                     :input-style="{height:'400px'}"
                     type="textarea"
                     outlined>
              <template v-slot:before>
                <span style="font-size:14px">配置:</span>
              </template>
            </q-input>
          </div>
        </div>

        <q-separator />

        <q-card-actions align="right">
          <q-btn flat
                 label="取消"
                 color="primary"
                 @click="newSSHCancel" />
          <q-btn flat
                 label="创建"
                 color="primary"
                 @click="newSSHCreate" />
        </q-card-actions>
      </q-card>
    </q-dialog>
    <!-- 更新SSH端口框 -->
    <q-dialog v-model="updateSSHEndpointFlag"
              persistent>
      <q-card style="width:100%;max-width:50vw">
        <q-card-section>
          <div class="text-h6">更新SSH终结点</div>
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
            <q-select v-model="Type"
                      :options="['Default']"
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
            <q-input v-model="Configuration"
                     :dense="false"
                     class="col-xs-12"
                     :input-style="{height:'400px'}"
                     type="textarea"
                     outlined>
              <template v-slot:before>
                <span style="font-size:14px">配置:</span>
              </template>
            </q-input>
          </div>
        </div>

        <q-separator />

        <q-card-actions align="right">
          <q-btn flat
                 label="取消"
                 color="primary"
                 @click="updateSSHCancel" />
          <q-btn flat
                 label="更新"
                 color="primary"
                 @click="updateSSHCreate" />
        </q-card-actions>
      </q-card>
    </q-dialog>
  </div>
</template>

<script>
import * as Apis from "@/api/index"
export default {
  data () {
    return {
      // ------------------------------------ SSH端口 ------------------------------------
      SSHEndpointList: [], //SSH端口列表
      //SSH端口列表分页配置
      SSHEndpointPagination: {
        page: 1,          //页码
        rowsNumber: 1     //总页数
      },
      SSHEndpointSelected: [],//SSH端口选择
      //SSH端口表格配置
      SSHEndpointColumns: [
        {
          name: 'name',
          required: true,
          label: '名称',
          align: 'left',
          field: row => row.name,
          format: val => `${val}`,
        },
        { name: 'type', align: 'left', label: '类型', field: 'type', },
        { name: 'configuration', label: '配置', align: 'left', field: 'configuration', style: 'max-width: 250px', headerStyle: 'max-width: 250px' },
        { name: 'id', label: '操作', align: 'right', field: 'id', headerStyle: 'text-align:center' },
      ],

      createSSHEndpointFlag: false,//创建SSHdialogFlag

      Name: '',//SSH名称
      Type: '',//SSH类型
      Configuration: '',//SSH配置

      updateSSHEndpointFlag: false,//更新SSHdialogFlag
      SSHEndpointDetailId: '',//ssh端口详情id
    }
  },
  mounted () {
    this.getSSHEndpointList();
  },
  methods: {
    //获得SSH端口列表
    getSSHEndpointList (page) {
      this.$q.loading.show()
      let para = {
        matchName: '',
        page: page || 1,
        pageSize: 50
      }
      Apis.getSSHEndpointList(para).then((res) => {
        console.log(res)
        this.SSHEndpointList = res.data.results;
        this.SSHEndpointPagination.page = page || 1;
        this.SSHEndpointPagination.rowsNumber = Math.ceil(res.data.totalCount / 50);
        this.$q.loading.hide()
      })
    },
    openSSHCreate () {
      this.createSSHEndpointFlag = true;
    },
    //取消关闭SSH创建框
    newSSHCancel () {
      this.Name = '';
      this.Type = '';
      this.Configuration = '';
      this.createSSHEndpointFlag = false;
    },
    //创建SSH端口
    newSSHCreate () {
      let para = {
        Name: this.Name,
        Type: this.Type,
        Configuration: this.Configuration,
      }
      if (this.Name && this.Type && this.Configuration) {
        this.$q.loading.show()
        console.log(para)
        Apis.postCreateSSHEndpoint(para).then((res) => {
          console.log(res)
          this.newSSHCancel();
          this.getSSHEndpointList();
          this.getSSHEndpointData();
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
    //删除SSH端口
    deleteSSH () {
      if (this.SSHEndpointSelected.length == 0) {
        this.$q.notify({
          position: 'top',
          message: '提示',
          caption: '请选择SSH端口',
          color: 'red',
        })
        return;
      }
      this.$q.dialog({
        title: '提示',
        message: '您确定要删除当前选择的SSH端口吗',
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
        //单个批量删除SSH端口

        let SelectedLength = this.SSHEndpointSelected.length;
        let DelNum = 0;

        for (let i = 0; i < this.SSHEndpointSelected.length; i++) {
          let para = `?id=${this.SSHEndpointSelected[i].id}`
          this.$q.loading.show()
          Apis.deleteSSHEndpoint(para).then(() => {
            DelNum++;
            if (DelNum == SelectedLength) {
              this.$q.notify({
                position: 'top',
                message: '提示',
                caption: '删除成功',
                color: 'secondary',
              })
              this.SSHEndpointSelected = [];
              this.getSSHEndpointList();
            }
          }).catch(() => {
            DelNum++;
            if (DelNum == SelectedLength) {
              this.SSHEndpointSelected = [];
              this.getSSHEndpointList();
            }
          })
        }
      })
    },
    //删除单个SSH端口
    delectSelectSSHEndpoint (value) {
      this.$q.dialog({
        title: '提示',
        message: '您确定要删除当前选择的SSH端口吗',
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

        let para = `?id=${value.row.id}`
        this.$q.loading.show()
        Apis.deleteSSHEndpoint(para).then(() => {
          this.$q.notify({
            position: 'top',
            message: '提示',
            caption: '删除成功',
            color: 'secondary',
          })
          this.SSHEndpointSelected = [];
          this.getSSHEndpointList();
        })
      })
    },
    //SSH端口分页
    SSHNextPage (value) {
      this.getSSHEndpointList(value)
    },
    // --------------------------------------------------  更新 -------------------------------
    //打开SSH端口更新
    toSSHEndpointDetail (env) {
      this.$q.loading.show()
      let para = {
        id: env.row.id
      }
      this.SSHEndpointDetailId = env.row.id;
      Apis.getSSHEndpointDetail(para).then((res) => {
        console.log(res)
        this.Name = res.data.name;
        this.Type = res.data.type;
        this.Configuration = res.data.configuration;
        this.$q.loading.hide();
        this.updateSSHEndpointFlag = true;
      })
    },
    //取消更新SSH端口
    updateSSHCancel () {
      this.Name = '';
      this.Type = '';
      this.Configuration = '';
      this.SSHEndpointDetailId = '';
      this.updateSSHEndpointFlag = false;
    },
    //更新SSH端口
    updateSSHCreate () {
      let para = {
        ID: this.SSHEndpointDetailId,
        Name: this.Name,
        Type: this.Type,
        Configuration: this.Configuration
      }
      if (this.Name == '' || this.Type == '' || this.Configuration == '') {
        this.$q.notify({
          position: 'top',
          message: '提示',
          caption: '请填写完整信息',
          color: 'red',
        })
        return;
      }
      this.$q.loading.show()
      Apis.putSSHEndpoint(para).then(() => {
        this.$q.notify({
          position: 'top',
          message: '提示',
          caption: '更新成功',
          color: 'secondary',
        })
        this.getSSHEndpointList();
        this.updateSSHCancel();
      })
    }
  }
}
</script>

<style lang="scss" scoped>
</style>