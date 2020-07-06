<template>
  <div class="detail">
    <div class="detail_header">
      <q-btn class="btn"
             color="primary"
             label="保 存"
             @click="putTestDataSource" />
      <q-btn class="btn"
             style="background: #FF0000; color: white"
             label="删 除"
             @click="deleteTestDataSource" />
    </div>
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
    </div>
  </div>
</template>

<script>
import * as Apis from "@/api/index"
export default {
  name: 'TestCaseDetail',
  data () {
    return {
      TestDataSourceDetail: '',//详情数据

      Id: '',
      Name: '',
      Type: '',
      Data: '',
    }
  },
  mounted () {
    this.Id = this.$route.query.id;
    this.getTestDataSourceDetail();
  },
  methods: {
    //获得TestDataSource详情数据
    getTestDataSourceDetail () {
      this.$q.loading.show()
      let para = {
        id: this.Id
      }
      Apis.getTestDataSourceDetail(para).then((res) => {
        console.log(res)
        this.TestDataSourceDetail = res.data;
        this.Name = res.data.name;
        this.Type = res.data.type;
        this.Data = res.data.data;
        this.$q.loading.hide()
      })
    },
    //更新TestDataSource
    putTestDataSource () {
      let para = {
        ID: this.Id,
        Name: this.Name,
        Type: this.Type,
        Data: this.Data
      }
      if (this.Id && this.Name && this.Type && this.Data) {
        this.$q.loading.show()
        Apis.putTestDataSource(para).then((res) => {
          console.log(res)
          this.getTestDataSourceDetail();
          this.$q.notify({
            position: 'top',
            message: '提示',
            caption: '保存成功',
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
    //删除TestDataSource
    deleteTestDataSource () {
      this.$q.dialog({
        title: '提示',
        message: '您确定要删除当前的测试数据源吗',
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
        //单个删除TestDataSource
        this.$q.loading.show()
        let para = `?id=${this.Id}`;
        Apis.deleteTestDataSource(para).then(() => {
          this.$router.push({ name: 'TestDataSource' })
        })
      })
    },
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

