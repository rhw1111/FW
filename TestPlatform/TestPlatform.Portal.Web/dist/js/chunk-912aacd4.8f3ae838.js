(window["webpackJsonp"]=window["webpackJsonp"]||[]).push([["chunk-912aacd4"],{"0442":function(e,t,n){},"1d47":function(e,t,n){"use strict";var s=n("0442"),i=n.n(s);i.a},"4e0f":function(e,t,n){"use strict";var s=function(){var e=this,t=e.$createElement,n=e._self._c||t;return n("q-dialog",{attrs:{persistent:""},model:{value:e.fixed,callback:function(t){e.fixed=t},expression:"fixed"}},[n("q-card",[n("q-card-section",[n("div",{staticClass:"text-h6"},[e._v("引擎类型列表")])]),n("q-separator"),n("div",{staticClass:"new_input"},e._l(e.EngineTypeList,(function(t,s){return n("q-item",{directives:[{name:"ripple",rawName:"v-ripple"}],key:s,attrs:{tag:"label"}},[n("q-item-section",{attrs:{avatar:""}},[n("q-radio",{attrs:{val:s,color:"teal"},model:{value:e.selectIndex,callback:function(t){e.selectIndex=t},expression:"selectIndex"}})],1),n("q-item-section",[n("q-item-label",[e._v(e._s(t))])],1)],1)})),1),n("q-separator"),n("q-card-actions",{attrs:{align:"right"}},[n("q-btn",{attrs:{flat:"",label:"取消",color:"primary"},on:{click:e.cancel}}),n("q-btn",{attrs:{flat:"",label:"添加",color:"primary"},on:{click:e.confirm}})],1)],1)],1)},i=[],a={props:["fixed","EngineTypeIndex"],name:"lookUp",data:function(){return{selectIndex:-1,EngineTypeList:["Http","Tcp"]}},watch:{EngineTypeIndex:function(e){this.selectIndex=e}},methods:{confirm:function(){this.$emit("addEngineType",this.EngineTypeList,this.selectIndex)},cancel:function(){this.$emit("cancelEngineType")}}},o=a,c=n("2877"),r=Object(c["a"])(o,s,i,!1,null,"2a34b3c9",null);t["a"]=r.exports},5954:function(e,t,n){"use strict";n.r(t);var s=function(){var e=this,t=e.$createElement,n=e._self._c||t;return n("div",{staticClass:"TestCase"},[n("div",{staticClass:"q-pa-md"},[n("q-table",{attrs:{title:"测试用例列表",data:e.TestCaseList,columns:e.columns,"row-key":"id","rows-per-page-options":[0],"no-data-label":"暂无数据更新"},on:{"row-dblclick":e.toDetail},scopedSlots:e._u([{key:"top-right",fn:function(){return[n("q-btn",{staticClass:"btn",attrs:{color:"primary",label:"新 增"},on:{click:e.openCrateTestCase}})]},proxy:!0},{key:"bottom",fn:function(){return[n("q-pagination",{staticClass:"col offset-md-10",attrs:{max:e.pagination.rowsNumber,input:!0},on:{input:e.nextPage},model:{value:e.pagination.page,callback:function(t){e.$set(e.pagination,"page",t)},expression:"pagination.page"}})]},proxy:!0},{key:"body-cell-id",fn:function(t){return[n("q-td",{staticClass:"text-right",attrs:{props:t}},[n("q-btn",{staticClass:"btn",staticStyle:{background:"#FF0000",color:"white"},attrs:{label:"删 除"},on:{click:function(n){return e.deleteTestCase(t)}}})],1)]}}])})],1),n("lookUp",{ref:"lookUp",attrs:{masterHostList:e.masterHostList,masterSelectIndex:e.masterHostIndex,fixed:e.HostFixed},on:{addMasterHost:e.addMasterHost,cancelMasterHost:e.cancelMasterHost}}),n("EngineTypeLookUp",{ref:"TypelookUp",attrs:{fixed:e.EngineTypeFixed,EngineTypeIndex:e.EngineTypeSelect},on:{cancelEngineType:e.cancelEngineType,addEngineType:e.addEngineType}}),n("q-dialog",{attrs:{persistent:""},model:{value:e.createFixed,callback:function(t){e.createFixed=t},expression:"createFixed"}},[n("q-card",{staticStyle:{width:"100%"}},[n("q-card-section",[n("div",{staticClass:"text-h6"},[e._v("创建测试用例")])]),n("q-separator"),n("div",{staticClass:"new_input"},[n("div",{staticClass:"row"},[n("q-input",{staticClass:"col",attrs:{dense:!1},scopedSlots:e._u([{key:"before",fn:function(){return[n("span",{staticStyle:{"font-size":"14px"}},[e._v("名称:")])]},proxy:!0}]),model:{value:e.Name,callback:function(t){e.Name=t},expression:"Name"}}),n("q-input",{staticClass:"col",staticStyle:{"margin-left":"50px"},attrs:{dense:!1,readonly:""},on:{dblclick:e.openEngineType},scopedSlots:e._u([{key:"before",fn:function(){return[n("span",{staticStyle:{"font-size":"14px"}},[e._v("引擎类型:")])]},proxy:!0}]),model:{value:e.EngineType,callback:function(t){e.EngineType=t},expression:"EngineType"}})],1),n("div",{staticClass:"row"},[n("q-input",{staticClass:"col col-xs-12",attrs:{dense:!1,readonly:""},on:{dblclick:e.masterHost},scopedSlots:e._u([{key:"before",fn:function(){return[n("span",{staticStyle:{"font-size":"14px"}},[e._v("主机:")])]},proxy:!0}]),model:{value:e.masterHostSelect,callback:function(t){e.masterHostSelect=t},expression:"masterHostSelect"}})],1),n("div",{staticClass:"row"},[n("q-input",{staticClass:"col-xs-12",attrs:{dense:!1,type:"textarea",outlined:""},scopedSlots:e._u([{key:"before",fn:function(){return[n("span",{staticStyle:{"font-size":"14px"}},[e._v("配置:")])]},proxy:!0}]),model:{value:e.Configuration,callback:function(t){e.Configuration=t},expression:"Configuration"}})],1)]),n("q-separator"),n("q-card-actions",{attrs:{align:"right"}},[n("q-btn",{attrs:{flat:"",label:"取消",color:"primary"},on:{click:e.newCancel}}),n("q-btn",{attrs:{flat:"",label:"创建",color:"primary"},on:{click:e.newCreate}})],1)],1)],1)],1)},i=[],a=(n("b0c0"),n("365c")),o=n("7729"),c=n("4e0f"),r={name:"TestCase",components:{lookUp:o["a"],EngineTypeLookUp:c["a"]},data:function(){return{createFixed:!1,HostFixed:!1,TestCaseList:[],masterHostList:[],masterHostSelect:"",masterHostIndex:-1,EngineTypeFixed:!1,EngineTypeSelect:-1,Name:"",Configuration:"",EngineType:"",MasterHostID:"",selected:[],columns:[{name:"name",required:!0,label:"名称",align:"left",field:function(e){return e.name},format:function(e){return"".concat(e)}},{name:"engineType",align:"left",label:"引擎类型",field:"engineType"},{name:"configuration",label:"配置",align:"left",field:"configuration"},{name:"id",label:"",align:"left",field:"id"}],pagination:{page:1,rowsNumber:1}}},mounted:function(){this.getTestCaseList()},methods:{openCrateTestCase:function(){this.createFixed=!0},getMasterHostList:function(){var e=this;a["k"]().then((function(t){console.log(t),e.masterHostList=t.data,e.$q.loading.hide()}))},getTestCaseList:function(e){var t=this;this.$q.loading.show();var n={matchName:"",page:e||1};a["p"](n).then((function(n){console.log(n),t.TestCaseList=n.data.results,t.pagination.page=e||1,t.pagination.rowsNumber=Math.ceil(n.data.totalCount/50),t.getMasterHostList()}))},nextPage:function(e){this.getTestCaseList(e)},deleteTestCase:function(e){var t=this;console.log(e),this.$q.dialog({title:"提示",message:"您确定要删除当前的测试用例吗",persistent:!0,ok:{push:!0,label:"确定"},cancel:{push:!0,label:"取消"}}).onOk((function(){t.$q.loading.show();var n="?id=".concat(e.row.id);a["e"](n).then((function(e){console.log(e),t.getTestCaseList()}))})).onCancel((function(){}))},masterHost:function(){this.HostFixed=!0,this.createFixed=!1},addMasterHost:function(e){if(void 0==e)return!1;this.masterHostSelect=this.masterHostList[e].address,this.MasterHostID=this.masterHostList[e].id,this.masterHostIndex=e,this.createFixed=!0,this.HostFixed=!1},cancelMasterHost:function(){this.createFixed=!0,this.HostFixed=!1,this.$refs.lookUp.selectIndex=this.masterHostIndex},openEngineType:function(){this.EngineTypeFixed=!0,this.createFixed=!1},addEngineType:function(e,t){if(void 0==e)return!1;console.log(e,t),this.EngineType=e[t],this.EngineTypeSelect=t,this.createFixed=!0,this.EngineTypeFixed=!1},cancelEngineType:function(){this.EngineTypeFixed=!1,this.createFixed=!0,this.$refs.TypelookUp.selectIndex=this.EngineTypeSelect},newCancel:function(){this.Name="",this.Configuration="",this.EngineType="",this.MasterHostID="",this.masterHostSelect="",this.$refs.lookUp.selectIndex=-1,this.$refs.TypelookUp.selectIndex=-1,this.createFixed=!1},newCreate:function(){var e=this,t={Name:this.Name,Configuration:this.Configuration,EngineType:this.EngineType,MasterHostID:this.MasterHostID};this.Name&&this.Configuration&&this.EngineType&&this.MasterHostID?(this.$q.loading.show(),a["u"](t).then((function(t){console.log(t),e.getTestCaseList(),e.createFixed=!1,e.$q.notify({position:"top",message:"提示",caption:"创建成功",color:"secondary"}),e.Name="",e.Configuration="",e.EngineType="",e.MasterHostID="",e.masterHostSelect="",e.$refs.lookUp.selectIndex=-1,e.$refs.TypelookUp.selectIndex=-1}))):this.$q.notify({position:"top",message:"提示",caption:"请填写完整信息",color:"red"})},toDetail:function(e,t){console.log(t),this.$router.push({name:"TestCaseDetail",query:{id:t.id}})}}},l=r,d=(n("713a"),n("1d47"),n("2877")),p=Object(d["a"])(l,s,i,!1,null,"5ecf4c2e",null);t["default"]=p.exports},"713a":function(e,t,n){"use strict";var s=n("cfe5"),i=n.n(s);i.a},7729:function(e,t,n){"use strict";var s=function(){var e=this,t=e.$createElement,n=e._self._c||t;return n("q-dialog",{attrs:{persistent:""},model:{value:e.fixed,callback:function(t){e.fixed=t},expression:"fixed"}},[n("q-card",[n("q-card-section",[n("div",{staticClass:"text-h6"},[e._v("主机列表")])]),n("q-separator"),n("div",{staticClass:"new_input"},e._l(e.masterHostList,(function(t,s){return n("q-item",{directives:[{name:"ripple",rawName:"v-ripple"}],key:s,attrs:{tag:"label"}},[n("q-item-section",{attrs:{avatar:""}},[n("q-radio",{attrs:{val:s,color:"teal"},model:{value:e.selectIndex,callback:function(t){e.selectIndex=t},expression:"selectIndex"}})],1),n("q-item-section",[n("q-item-label",[e._v(e._s(t.address))])],1)],1)})),1),n("q-separator"),n("q-card-actions",{attrs:{align:"right"}},[n("q-btn",{attrs:{flat:"",label:"取消",color:"primary"},on:{click:e.cancel}}),n("q-btn",{attrs:{flat:"",label:"添加",color:"primary"},on:{click:e.confirm}})],1)],1)],1)},i=[],a={props:["fixed","masterHostList","masterSelectIndex"],name:"lookUp",data:function(){return{selectIndex:-1}},watch:{masterSelectIndex:function(e){this.selectIndex=e}},methods:{confirm:function(){this.$emit("addMasterHost",this.selectIndex)},cancel:function(){this.$emit("cancelMasterHost")}}},o=a,c=n("2877"),r=Object(c["a"])(o,s,i,!1,null,"24a28892",null);t["a"]=r.exports},cfe5:function(e,t,n){}}]);
//# sourceMappingURL=chunk-912aacd4.8f3ae838.js.map