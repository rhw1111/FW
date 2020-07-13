<template>
  <div class="detail">
    <!-- button -->
    <div class="detail_header">
      <q-btn class="btn"
             color="primary"
             label="保 存"
             @click="putSSHEndpoint" />
      <q-btn class="btn"
             style="background: #FF0000; color: white"
             label="删 除"
             @click="deleteSSHEndpoint" />
    </div>
    <!-- 从机详情字段 -->
    <div class="q-pa-md row">

      <div class="new_input">
        <div class="row">
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

        <div class="row">
          <q-input v-model="Configuration"
                   :dense="false"
                   class="col-xs-12"
                   type="textarea"
                   outlined>
            <template v-slot:before>
              <span style="font-size:14px">配置:</span>
            </template>
          </q-input>
        </div>
      </div>
    </div>
  </div>
</template>

<script>
import * as Apis from "@/api/index"
export default {
  name: 'SSHEndpointDetail',
  data () {
    return {
      DetailId: '',  //详情ID
      SSHEndpointData: '',//SSH端口详情

      Name: '',
      Type: '',
      Configuration: ''
    }
  },
  mounted () {
    this.DetailId = this.$route.query.id;
    this.getSSHEndpointDetail();
  },
  methods: {
    //获得SSH端口详情
    getSSHEndpointDetail () {
      this.$q.loading.show()
      let para = {
        id: this.DetailId
      }
      Apis.getSSHEndpointDetail(para).then((res) => {
        console.log(res)
        this.SSHEndpointData = res.data;
        this.Name = res.data.name;
        this.Type = res.data.type;
        this.Configuration = res.data.configuration;
        this.$q.loading.hide()
      })
    },
    //更新SSH端口
    putSSHEndpoint () {
      let para = {
        ID: this.DetailId,
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
        this.getSSHEndpointDetail();
      })
    },
    //删除SSH端口
    deleteSSHEndpoint () {
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
        let para = `?id=${this.DetailId}`
        this.$q.loading.show()
        Apis.deleteSSHEndpoint(para).then(() => {
          this.$q.notify({
            position: 'top',
            message: '提示',
            caption: '删除成功',
            color: 'secondary',
          })
          this.$router.push({
            name: 'MasterHost'
          })
        })
      })
    }
  }
}
</script>

<style lang="scss" scoped>
.detail {
  width: 100%;
  overflow: hidden;
  .detail_header {
    padding: 10px 16px 5px;
    width: 100%;
    z-index: 10;
    box-sizing: border-box;
    background: #ffffff;
    .btn {
      margin-right: 10px;
      margin-bottom: 5px;
    }
  }
}
</style>
<style lang="scss">
.new_input {
  width: 100%;
  padding: 10px 30px;

  .row {
    margin-bottom: 10px;
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
