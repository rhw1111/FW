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
          <q-input v-model="Data"
                   :dense="false"
                   class="col-xs-12"
                   type="textarea"
                   autogrow
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
        Data: this.Data.trim()
      }
      if (this.Id && this.Name && this.Type && this.Data.trim()) {
        if (!this.isDataType(this.Type)) { return; }
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
    //判断类型是否正确
    isDataType (type) {
      if (type == 'Int') {
        if (!Number(this.Data.trim())) {
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
      }
    },
    //判断是否是JSON格式
    isJSON (str) {
      if (typeof str == 'string') {
        try {
          var obj = JSON.parse(str);
          if (typeof obj == 'object' && obj != null) {
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

  .input_row {
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

