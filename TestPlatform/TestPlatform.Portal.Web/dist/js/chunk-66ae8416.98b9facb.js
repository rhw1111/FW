(window["webpackJsonp"]=window["webpackJsonp"]||[]).push([["chunk-66ae8416"],{"0442":function(t,e,n){},"1d47":function(t,e,n){"use strict";var s=n("0442"),i=n.n(s);i.a},5954:function(t,e,n){"use strict";n.r(e);var s=function(){var t=this,e=t.$createElement,n=t._self._c||e;return n("div",{staticClass:"TestCase"},[n("div",{staticClass:"q-pa-md"},[n("q-table",{attrs:{title:"测试用例列表",data:t.TestCaseList,columns:t.columns,"row-key":"id","rows-per-page-options":[0],"table-style":"max-height: 500px","no-data-label":"暂无数据更新"},scopedSlots:t._u([{key:"top-right",fn:function(){return[n("q-btn",{staticClass:"btn",attrs:{color:"primary",label:"新 增"},on:{click:t.openCrateTestCase}})]},proxy:!0},{key:"bottom",fn:function(){return[n("q-pagination",{staticClass:"col offset-md-10",attrs:{max:t.pagination.rowsNumber,input:!0},on:{input:t.nextPage},model:{value:t.pagination.page,callback:function(e){t.$set(t.pagination,"page",e)},expression:"pagination.page"}})]},proxy:!0},{key:"body-cell-id",fn:function(e){return[n("q-td",{attrs:{props:e}},[n("q-btn",{staticClass:"btn",attrs:{color:"primary",label:"更 新"},on:{click:function(n){return t.toDetail(e)}}}),n("q-btn",{staticClass:"btn",staticStyle:{background:"#FF0000",color:"white"},attrs:{label:"删 除"},on:{click:function(n){return t.deleteTestCase(e)}}})],1)]}}])})],1),n("lookUp",{ref:"lookUp",attrs:{masterHostList:t.masterHostList,masterSelectIndex:t.masterHostIndex,fixed:t.HostFixed},on:{addMasterHost:t.addMasterHost,cancelMasterHost:t.cancelMasterHost}}),n("q-dialog",{attrs:{persistent:""},model:{value:t.createFixed,callback:function(e){t.createFixed=e},expression:"createFixed"}},[n("q-card",{staticStyle:{width:"900px","max-width":"90vw"}},[n("q-card-section",[n("div",{staticClass:"text-h6"},[t._v("创建测试用例")])]),n("q-separator"),n("div",{staticClass:"new_input"},[n("div",{staticClass:"row input_row"},[n("q-input",{staticClass:"col",attrs:{dense:!1},scopedSlots:t._u([{key:"before",fn:function(){return[n("span",{staticStyle:{"font-size":"14px"}},[t._v("名称:")])]},proxy:!0}]),model:{value:t.Name,callback:function(e){t.Name=e},expression:"Name"}}),n("q-select",{staticClass:"col",attrs:{options:["Http","Tcp"],dense:!1},scopedSlots:t._u([{key:"before",fn:function(){return[n("span",{staticStyle:{"font-size":"14px"}},[t._v("引擎类型:")])]},proxy:!0},{key:"prepend",fn:function(){},proxy:!0}]),model:{value:t.EngineType,callback:function(e){t.EngineType=e},expression:"EngineType"}}),n("q-input",{staticClass:"col",attrs:{dense:!1,readonly:""},on:{dblclick:t.masterHost},scopedSlots:t._u([{key:"before",fn:function(){return[n("span",{staticStyle:{"font-size":"14px"}},[t._v("主机:")])]},proxy:!0}]),model:{value:t.masterHostSelect,callback:function(e){t.masterHostSelect=e},expression:"masterHostSelect"}})],1),n("span",{staticStyle:{"font-size":"14px"}},[t._v("参数配置:")]),n("div",{staticClass:"row",staticStyle:{"margin-bottom":"10px"}},[n("q-input",{staticClass:"col",attrs:{filled:"","bottom-slots":"",dense:!0},on:{keyup:function(e){t.paraConfig.UserCount=t.paraConfig.UserCount.replace(/[^\d]/g,"")}},scopedSlots:t._u([{key:"before",fn:function(){return[n("span",{staticStyle:{"font-size":"14px",width:"100px"}},[t._v("压测用户总数:")])]},proxy:!0},{key:"append",fn:function(){return[n("span",{staticStyle:{"font-size":"14px"}},[t._v("个")])]},proxy:!0}]),model:{value:t.paraConfig.UserCount,callback:function(e){t.$set(t.paraConfig,"UserCount",e)},expression:"paraConfig.UserCount"}}),n("q-input",{staticClass:"col",attrs:{filled:"","bottom-slots":"",dense:!0},on:{keyup:function(e){t.paraConfig.PerSecondUserCount=t.paraConfig.PerSecondUserCount.toString().replace(/[^\d]/g,"")}},scopedSlots:t._u([{key:"before",fn:function(){return[n("span",{staticStyle:{"font-size":"14px",width:"105px","margin-left":"10px"}},[t._v("每秒加载用户数:")])]},proxy:!0},{key:"append",fn:function(){return[n("span",{staticStyle:{"font-size":"14px"}},[t._v("个/秒")])]},proxy:!0}]),model:{value:t.paraConfig.PerSecondUserCount,callback:function(e){t.$set(t.paraConfig,"PerSecondUserCount",e)},expression:"paraConfig.PerSecondUserCount"}}),n("q-input",{staticClass:"col",attrs:{filled:"","bottom-slots":"",dense:!0},on:{keyup:function(e){t.paraConfig.Duration=t.paraConfig.Duration.toString().replace(/[^\d]/g,"")}},scopedSlots:t._u([{key:"before",fn:function(){return[n("span",{staticStyle:{"font-size":"14px",width:"100px","margin-left":"10px"}},[t._v("压测时间:")])]},proxy:!0},{key:"append",fn:function(){return[n("span",{staticStyle:{"font-size":"14px"}},[t._v("秒")])]},proxy:!0}]),model:{value:t.paraConfig.Duration,callback:function(e){t.$set(t.paraConfig,"Duration",e)},expression:"paraConfig.Duration"}})],1),n("div",{staticClass:"row",staticStyle:{"margin-bottom":"10px"}},[n("q-input",{staticClass:"col-5",attrs:{filled:"","bottom-slots":"",dense:!0},scopedSlots:t._u([{key:"before",fn:function(){return[n("span",{staticStyle:{"font-size":"14px",width:"100px"}},[t._v("被测服务器:")])]},proxy:!0},{key:"append",fn:function(){return[n("span",{staticStyle:{"font-size":"14px"}},[t._v("ip地址")])]},proxy:!0}]),model:{value:t.paraConfig.Address,callback:function(e){t.$set(t.paraConfig,"Address",e)},expression:"paraConfig.Address"}}),n("q-input",{staticClass:"col-5",attrs:{filled:"","bottom-slots":"",dense:!0},on:{input:function(e){t.paraConfig.Port=t.paraConfig.Port.toString().replace(/[^\d]/g,"")}},scopedSlots:t._u([{key:"before",fn:function(){return[n("span",{staticStyle:{"font-size":"14px",width:"105px","margin-left":"10px"}},[t._v("被测服务器端口:")])]},proxy:!0},{key:"append",fn:function(){return[n("span",{staticStyle:{"font-size":"14px"}},[t._v("端口")])]},proxy:!0}]),model:{value:t.paraConfig.Port,callback:function(e){t.$set(t.paraConfig,"Port",e)},expression:"paraConfig.Port"}}),n("div",{staticClass:"col-2"},[n("q-btn",{staticClass:"btn ",staticStyle:{margin:"0px 0px 0px 20px"},attrs:{color:"primary",label:"生 成"},on:{click:t.CreateJson}})],1)],1),n("div",{staticClass:"row",staticStyle:{"margin-bottom":"10px"}},[n("q-input",{staticClass:"col-xs-12",attrs:{dense:!1,type:"textarea","input-style":{height:"300px"},outlined:""},scopedSlots:t._u([{key:"before",fn:function(){return[n("span",{staticStyle:{"font-size":"14px"}},[t._v("配置:")])]},proxy:!0}]),model:{value:t.Configuration,callback:function(e){t.Configuration=e},expression:"Configuration"}})],1)]),n("q-separator"),n("q-card-actions",{attrs:{align:"right"}},[n("q-btn",{attrs:{flat:"",label:"取消",color:"primary"},on:{click:t.newCancel}}),n("q-btn",{attrs:{flat:"",label:"创建",color:"primary"},on:{click:t.newCreate}})],1)],1)],1)],1)},i=[],o=(n("b0c0"),n("a9e3"),n("498a"),n("53ca")),a=n("365c"),r=n("7729"),c={name:"TestCase",components:{lookUp:r["a"]},data:function(){return{createFixed:!1,HostFixed:!1,TestCaseList:[],masterHostList:[],masterHostSelect:"",masterHostIndex:-1,Name:"",Configuration:"",EngineType:"",MasterHostID:"",selected:[],columns:[{name:"name",required:!0,label:"名称",align:"left",field:function(t){return t.name},format:function(t){return"".concat(t)}},{name:"engineType",align:"left",label:"引擎类型",field:"engineType"},{name:"configuration",label:"配置",align:"left",field:"configuration"},{name:"id",label:"操作",align:"right",field:"id",headerStyle:"text-align:center"}],pagination:{page:1,rowsNumber:1},paraConfig:{UserCount:"",PerSecondUserCount:"",Address:"",Port:"",Duration:""}}},mounted:function(){this.getTestCaseList()},methods:{openCrateTestCase:function(){this.createFixed=!0},getMasterHostList:function(){var t=this;a["m"]().then((function(e){console.log(e),t.masterHostList=e.data,t.$q.loading.hide()}))},getTestCaseList:function(t){var e=this;this.$q.loading.show();var n={matchName:"",page:t||1};a["u"](n).then((function(n){console.log(n),e.TestCaseList=n.data.results,e.pagination.page=t||1,e.pagination.rowsNumber=Math.ceil(n.data.totalCount/50),e.getMasterHostList()}))},nextPage:function(t){this.getTestCaseList(t)},deleteTestCase:function(t){var e=this;console.log(t),this.$q.dialog({title:"提示",message:"您确定要删除当前的测试用例吗",persistent:!0,ok:{push:!0,label:"确定"},cancel:{push:!0,label:"取消"}}).onOk((function(){e.$q.loading.show();var n="?id=".concat(t.row.id);a["f"](n).then((function(t){console.log(t),e.getTestCaseList()}))})).onCancel((function(){}))},masterHost:function(){this.HostFixed=!0,this.createFixed=!1},addMasterHost:function(t){if(void 0==t)return!1;this.masterHostSelect=this.masterHostList[t].address,this.MasterHostID=this.masterHostList[t].id,this.masterHostIndex=t,this.createFixed=!0,this.HostFixed=!1},cancelMasterHost:function(){this.createFixed=!0,this.HostFixed=!1,this.$refs.lookUp.selectIndex=this.masterHostIndex},newCancel:function(){this.Name="",this.Configuration="",this.EngineType="",this.MasterHostID="",this.masterHostSelect="",this.$refs.lookUp.selectIndex=-1,this.createFixed=!1,this.paraConfig.UserCount="",this.paraConfig.PerSecondUserCount="",this.paraConfig.Address="",this.paraConfig.Port="",this.paraConfig.Duration=""},newCreate:function(){var t=this,e={Name:this.Name,Configuration:this.Configuration.trim(),EngineType:this.EngineType,MasterHostID:this.MasterHostID};this.Name&&this.isJSON(this.Configuration.trim())&&this.EngineType&&this.MasterHostID?(this.$q.loading.show(),a["C"](e).then((function(e){console.log(e),t.getTestCaseList(),t.$q.notify({position:"top",message:"提示",caption:"创建成功",color:"secondary"}),t.newCancel()}))):this.$q.notify({position:"top",message:"提示",caption:"请填写完整信息",color:"red"})},toDetail:function(t){this.$router.push({name:"TestCaseDetail",query:{id:t.row.id}})},CreateJson:function(){if(""==this.Configuration.trim()){if(""!=this.paraConfig.Address&&!this.isValidIp(this.paraConfig.Address))return void this.$q.notify({position:"top",message:"提示",caption:"被测服务器ip地址不正确",color:"red"});this.Configuration=JSON.stringify({UserCount:this.paraConfig.UserCount?Number(this.paraConfig.UserCount):"",PerSecondUserCount:this.paraConfig.PerSecondUserCount?Number(this.paraConfig.PerSecondUserCount):"",Address:this.paraConfig.Address,Port:this.paraConfig.Port?Number(this.paraConfig.Port):"",Duration:this.paraConfig.Duration?Number(this.paraConfig.Duration):""},null,2)}else if(this.isJSON(this.Configuration.trim())){if(""!=this.paraConfig.Address&&!this.isValidIp(this.paraConfig.Address))return void this.$q.notify({position:"top",message:"提示",caption:"被测服务器ip地址不正确",color:"red"});this.Configuration=JSON.parse(this.Configuration),this.Configuration.UserCount=this.paraConfig.UserCount?Number(this.paraConfig.UserCount):"",this.Configuration.PerSecondUserCount=this.paraConfig.PerSecondUserCount?Number(this.paraConfig.PerSecondUserCount):"",this.Configuration.Address=this.paraConfig.Address,this.Configuration.Port=this.paraConfig.Port?Number(this.paraConfig.Port):"",this.Configuration.Duration=this.paraConfig.Duration?Number(this.paraConfig.Duration):"",this.Configuration=JSON.stringify(this.Configuration,null,2)}},isJSON:function(t){if("string"==typeof t)try{var e=JSON.parse(t);if("object"==Object(o["a"])(e)&&e){if("{"==t.substr(0,1)&&"}"==t.substr(-1))return!0;this.$q.notify({position:"top",message:"提示",caption:"配置不是正确的JSON格式",color:"red"})}else this.$q.notify({position:"top",message:"提示",caption:"配置不是正确的JSON格式",color:"red"})}catch(n){this.$q.notify({position:"top",message:"提示",caption:"配置不是正确的JSON格式",color:"red"})}},isValidIp:function(t){return/^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$/.test(t)}}},l=c,p=(n("c974"),n("1d47"),n("2877")),u=Object(p["a"])(l,s,i,!1,null,"984024f0",null);e["default"]=u.exports},7729:function(t,e,n){"use strict";var s=function(){var t=this,e=t.$createElement,n=t._self._c||e;return n("q-dialog",{attrs:{persistent:""},model:{value:t.fixed,callback:function(e){t.fixed=e},expression:"fixed"}},[n("q-card",[n("q-card-section",[n("div",{staticClass:"text-h6"},[t._v("主机列表")])]),n("q-separator"),n("div",{staticClass:"new_input"},t._l(t.masterHostList,(function(e,s){return n("q-item",{directives:[{name:"ripple",rawName:"v-ripple"}],key:s,attrs:{tag:"label"}},[n("q-item-section",{attrs:{avatar:""}},[n("q-radio",{attrs:{val:s,color:"teal"},model:{value:t.selectIndex,callback:function(e){t.selectIndex=e},expression:"selectIndex"}})],1),n("q-item-section",[n("q-item-label",[t._v(t._s(e.address))])],1)],1)})),1),n("q-separator"),n("q-card-actions",{attrs:{align:"right"}},[n("q-btn",{attrs:{flat:"",label:"取消",color:"primary"},on:{click:t.cancel}}),n("q-btn",{attrs:{flat:"",label:"添加",color:"primary"},on:{click:t.confirm}})],1)],1)],1)},i=[],o={props:["fixed","masterHostList","masterSelectIndex"],name:"lookUp",data:function(){return{selectIndex:-1}},watch:{masterSelectIndex:function(t){this.selectIndex=t}},methods:{confirm:function(){this.$emit("addMasterHost",this.selectIndex)},cancel:function(){this.$emit("cancelMasterHost")}}},a=o,r=n("2877"),c=Object(r["a"])(a,s,i,!1,null,"24a28892",null);e["a"]=c.exports},"78c7":function(t,e,n){},c974:function(t,e,n){"use strict";var s=n("78c7"),i=n.n(s);i.a}}]);
//# sourceMappingURL=chunk-66ae8416.98b9facb.js.map