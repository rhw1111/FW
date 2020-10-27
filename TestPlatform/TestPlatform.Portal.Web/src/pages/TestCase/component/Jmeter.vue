<template>
  <div>
    <div class="row new_input">
      <q-input :dense="false"
               type="file"
               class="col"
               ref="File"
               accept=".jmx"
               @input="getUploadedFile">
        <template v-slot:before>
          <span style="font-size:14px">Jmeter文件:</span>
        </template>
        <template v-slot:append>
          ...
        </template>
        <!-- <template v-slot:after>
          <q-btn color="white"
                 text-color="black"
                 label="上传"
                 @click="JmeterDataSource" />
        </template> -->
      </q-input>
    </div>

    <div class="row new_input JmeterFile"
         v-for="(value,index) in DataSourceVars"
         :key="index">

      <q-select v-model="value.DataSourceName"
                :options="dataSourceName"
                class="col"
                placeholder="请选择测试数据源"
                :dense="false">
        <template v-slot:before>
          <span style="font-size:14px;overflow:hidden;text-overflow:ellipsis;white-space: nowrap;">{{value.Name}}: <div style="display:inline"
                 :title="value.Data">{{value.Path}}</div></span>
        </template>
      </q-select>

    </div>

    <q-list bordered
            style="margin-top: 30px;"
            class="listTestarea">
      <q-expansion-item label="数据源"
                        style="text-align:left;position:relative;"
                        expand-icon-toggle
                        expand-separator
                        v-model="ConfigTextExpanded">
        <template v-slot:header>
          <q-item-section>
            配置文本:
          </q-item-section>
          <q-item-section side>
            <q-btn class="btn"
                   color="primary"
                   style="margin:10px 0 10px 20px;float:right"
                   label="生 成"
                   @click="CreateJson" />
          </q-item-section>
        </template>
        <q-card>
          <div class="row input_row">
            <q-input v-model="Configuration"
                     :dense="false"
                     style="overflow:hidden"
                     autogrow
                     class="col-12"
                     readonly
                     type="textarea"
                     outlined>
            </q-input>
          </div>
        </q-card>
      </q-expansion-item>
    </q-list>

  </div>
</template>

<script>
import * as Apis from "@/api/index"
export default {
  props: ['paraConfiguration'], // 详情数据
  data () {
    return {
      File: '',            //文件
      xmlDoc: null,       //xml对象
      xmlStr: null,       //xml字符串
      DataSourceVars: [], //数据文件数组

      dataSourceName: [], //数据源名称数组

      // ------- 配置文本 --------
      ConfigTextExpanded: false, //配置文本显隐
      Configuration: '',         //配置文本内容
    }
  },
  mounted () {
    console.log(this.paraConfiguration)
    //详情回调
    if (this.paraConfiguration) {
      this.DataSourceVars = this.paraConfiguration.DataSourceVars;
      this.Configuration = JSON.stringify(this.paraConfiguration, null, 2);
      this.xmlStr = this.paraConfiguration.FileContent;
    }
    this.getDataSourceName();
  },
  methods: {
    //获得Jmeter类型的数据源名称
    getDataSourceName () {
      let para = {
        isJmeter: true
      };
      Apis.getDataSourceName(para).then((res) => {
        console.log(res);
        for (let i = 0; i < res.data.length; i++) {
          this.dataSourceName.push(res.data[i].name)
        }
        this.$q.loading.hide();
      })
    },

    //获得jmx文件
    getUploadedFile (file) {
      console.log(file)
      this.File = file[0];
      let _this = this;
      //没有选择文件
      if (file.length == 0) {
        this.RemoveParameters();
        return;
      }
      //验证是否选择的是后缀名为.jmx文件并解析
      let suffix = file[0].name.substring(file[0].name.lastIndexOf(".") + 1);
      if (suffix !== 'jmx') {
        this.$q.notify({
          position: 'top',
          message: '提示',
          caption: '请选择后缀名为.jmx的Jmeter文件',
          color: 'red',
        })
        console.log(this.$refs.File.target);
        this.RemoveParameters();
        this.File = file[0];
        return;
      }


      this.Configuration = '';

      //兼容ie解析xml文件
      let reader = new FileReader();
      reader.readAsText(file[0]);
      reader.onload = function (evt) {
        _this.xmlStr = evt.target.result;
        if (window.DOMParser) {
          let parser = new DOMParser();
          _this.xmlDoc = parser.parseFromString(evt.target.result, "text/xml");
          _this.parsingCSVDataSet();
        } else {
          //IE
          // eslint-disable-next-line no-undef
          _this.xmlDoc = new ActiveXObject("Microsoft.XMLDOM");
          _this.xmlDoc.async = "false";
          _this.xmlDoc.loadXML(evt.target.result);
          _this.parsingCSVDataSet();
        }
      }
    },

    //找到标签为CSVDataSet并解析出来数据文件
    parsingCSVDataSet () {
      this.DataSourceVars = [];
      let csvDatasetList = this.xmlDoc.querySelectorAll("CSVDataSet");
      if (csvDatasetList && csvDatasetList.length > 0) {
        csvDatasetList.forEach((el, index) => {
          this.DataSourceVars.push({
            Name: el.getAttribute("testname"),
            Type: '',
            DataSourceName: '',
            Data: '',
            Path: '',
          })
          const children = el.children || [];
          Array.from(children).forEach(val => {
            if (val.getAttribute("name") == 'filename') {
              this.DataSourceVars[index].Path = val.innerHTML || '';
              return;
            }
          })
        })
      }
    },

    //清除参数
    RemoveParameters () {
      this.File = '';
      this.xmlDoc = null;
      this.xmlStr = null;
      this.DataSourceVars = [];
      this.Configuration = '';
    },

    //生成JSON 在配置文本里展示
    CreateJson () {
      let _this = this;
      this.Configuration = JSON.stringify({
        "FileContent": _this.xmlStr,
        "DataSourceVars": _this.DataSourceVars
      }, null, 2);
      this.ConfigTextExpanded = true;
    },

    //判断Jmeter数据文件的数据源名称是否选择
    JmeterDataSource () {
      console.log(this.File);
      if (this.paraConfiguration) {
        if (this.File) {
          if (this.File.name.substring(this.File.name.lastIndexOf(".") + 1) !== 'jmx') {
            this.$q.notify({
              position: 'top',
              message: '提示',
              caption: '请选择后缀名为.jmx的Jmeter文件',
              color: 'red',
            })
            return true;
          }
        }
      } else {
        if (!this.File) {
          this.$q.notify({
            position: 'top',
            message: '提示',
            caption: '请选择后缀名为.jmx的Jmeter文件',
            color: 'red',
          })
          return true;
        } else if (this.File.name.substring(this.File.name.lastIndexOf(".") + 1) !== 'jmx') {
          this.$q.notify({
            position: 'top',
            message: '提示',
            caption: '请选择后缀名为.jmx的Jmeter文件',
            color: 'red',
          })
          return true;
        }
      }




      //验证数据文件是否选择测试数据源
      let DataSourceVars = this.DataSourceVars;
      for (let i = 0; i < DataSourceVars.length; i++) {
        if (!DataSourceVars[i].DataSourceName) {
          this.$q.notify({
            position: 'top',
            message: '提示',
            caption: `${DataSourceVars[i].Name}的测试数据源没有选择`,
            color: 'red',
          })
          return true;
        }
      }
    },

    //解析xml
    UploadParsing () {
      if (!this.xmlDoc) {
        this.$q.notify({
          position: 'top',
          message: '提示',
          caption: '请上传后缀名为.jmx的Jmeter文件',
          color: 'red',
        })
        return;
      }
      //找到标签为CSVDataSet并解析出来数据文件
      this.DataSourceVars = [];
      let csvDatasetList = this.xmlDoc.querySelectorAll("CSVDataSet");
      if (csvDatasetList && csvDatasetList.length > 0) {
        csvDatasetList.forEach((el, index) => {
          this.DataSourceVars.push({
            Name: el.getAttribute("testname"),
            Type: '',
            DataSourceName: '',
            Data: '',
            Path: '',
          })
          const children = el.children || [];
          Array.from(children).forEach(val => {
            if (val.getAttribute("name") == 'filename') {
              this.DataSourceVars[index].Path = val.innerHTML || '';
              console.log(this.DataSourceVars)
              return;
            }
          })
        })
      }
    },

  }
}
</script>

<style lang="scss" scoped>
.new_input {
  padding: 0;
}
</style>
<style lang="scss">
.JmeterFile {
  .q-field__before {
    width: 50%;
  }
}
.q-field__native[type="file"] {
  line-height: 3em;
}
.listTestarea textarea {
  white-space: pre;
}
</style>