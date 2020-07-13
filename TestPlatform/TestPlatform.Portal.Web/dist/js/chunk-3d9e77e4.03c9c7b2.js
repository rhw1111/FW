(window["webpackJsonp"]=window["webpackJsonp"]||[]).push([["chunk-3d9e77e4"],{1298:function(t,e,n){},"8fa2":function(t,e,n){"use strict";n.r(e);var o=function(){var t=this,e=t.$createElement,n=t._self._c||e;return n("div",{staticClass:"MasterHost"},[n("div",{staticClass:"q-pa-md row masterhost"},[n("q-table",{staticClass:"col-md-4 col-sm-12 col-xs-12",attrs:{title:"主机列表",data:t.MasterHostList,columns:t.MasterHostColumns,selection:"multiple",selected:t.MasterHostSelected,"row-key":"id","table-style":"max-height: 500px","virtual-scroll-sticky-start":48,"rows-per-page-options":[0],"no-data-label":"暂无数据更新"},on:{"update:selected":function(e){t.MasterHostSelected=e},"row-dblclick":t.toTestHostDetail},scopedSlots:t._u([{key:"top-right",fn:function(){return[n("q-btn",{staticClass:"btn",attrs:{color:"primary",label:"新 增"},on:{click:t.openHostCreate}}),n("q-btn",{staticClass:"btn",attrs:{color:"red",label:"删 除"},on:{click:t.deleteTestHost}})]},proxy:!0},{key:"bottom",fn:function(){return[n("q-pagination",{staticClass:"col offset-md-7",attrs:{max:t.MasterHostPagination.rowsNumber,input:!0},on:{input:t.TestHostNextPage},model:{value:t.MasterHostPagination.page,callback:function(e){t.$set(t.MasterHostPagination,"page",e)},expression:"MasterHostPagination.page"}})]},proxy:!0}])}),n("q-table",{staticClass:"col-md-8 col-sm-12 col-xs-12",attrs:{title:"SSH端点列表",data:t.SSHEndpointList,columns:t.SSHEndpointColumns,selection:"multiple",selected:t.SSHEndpointSelected,"row-key":"id","rows-per-page-options":[0],"table-style":"max-height: 500px","no-data-label":"暂无数据更新"},on:{"update:selected":function(e){t.SSHEndpointSelected=e},"row-dblclick":t.toSSHEndpointDetail},scopedSlots:t._u([{key:"top-right",fn:function(){return[n("q-btn",{staticClass:"btn",attrs:{color:"primary",label:"新 增"},on:{click:t.openSSHCreate}}),n("q-btn",{staticClass:"btn",attrs:{color:"red",label:"删 除"},on:{click:t.deleteSSH}})]},proxy:!0},{key:"bottom",fn:function(){return[n("q-pagination",{staticClass:"col offset-md-9",attrs:{max:t.SSHEndpointPagination.rowsNumber,input:!0},on:{input:t.SSHNextPage},model:{value:t.SSHEndpointPagination.page,callback:function(e){t.$set(t.SSHEndpointPagination,"page",e)},expression:"SSHEndpointPagination.page"}})]},proxy:!0}])})],1),n("q-dialog",{attrs:{persistent:""},model:{value:t.createSSHEndpointFlag,callback:function(e){t.createSSHEndpointFlag=e},expression:"createSSHEndpointFlag"}},[n("q-card",{staticStyle:{width:"100%"}},[n("q-card-section",[n("div",{staticClass:"text-h6"},[t._v("创建SSH端点")])]),n("q-separator"),n("div",{staticClass:"new_input"},[n("div",{staticClass:"row"},[n("q-input",{staticClass:"col",attrs:{dense:!1},scopedSlots:t._u([{key:"before",fn:function(){return[n("span",{staticStyle:{"font-size":"14px"}},[t._v("名称:")])]},proxy:!0}]),model:{value:t.Name,callback:function(e){t.Name=e},expression:"Name"}}),n("q-input",{staticClass:"col",staticStyle:{"margin-left":"50px"},attrs:{dense:!1},scopedSlots:t._u([{key:"before",fn:function(){return[n("span",{staticStyle:{"font-size":"14px"}},[t._v("类型:")])]},proxy:!0}]),model:{value:t.Type,callback:function(e){t.Type=e},expression:"Type"}})],1),n("div",{staticClass:"row"},[n("q-input",{staticClass:"col-xs-12",attrs:{dense:!1,type:"textarea",outlined:""},scopedSlots:t._u([{key:"before",fn:function(){return[n("span",{staticStyle:{"font-size":"14px"}},[t._v("配置:")])]},proxy:!0}]),model:{value:t.Configuration,callback:function(e){t.Configuration=e},expression:"Configuration"}})],1)]),n("q-separator"),n("q-card-actions",{attrs:{align:"right"}},[n("q-btn",{attrs:{flat:"",label:"取消",color:"primary"},on:{click:t.newSSHCancel}}),n("q-btn",{attrs:{flat:"",label:"创建",color:"primary"},on:{click:t.newSSHCreate}})],1)],1)],1),n("q-dialog",{attrs:{persistent:""},model:{value:t.createTestHostFlag,callback:function(e){t.createTestHostFlag=e},expression:"createTestHostFlag"}},[n("q-card",{staticStyle:{width:"100%"}},[n("q-card-section",[n("div",{staticClass:"text-h6"},[t._v("创建主机")])]),n("q-separator"),n("div",{staticClass:"new_input"},[n("div",{staticClass:"row"},[n("q-input",{staticClass:"col",attrs:{dense:!1},scopedSlots:t._u([{key:"before",fn:function(){return[n("span",{staticStyle:{"font-size":"14px"}},[t._v("地址:")])]},proxy:!0}]),model:{value:t.TestHostName,callback:function(e){t.TestHostName=e},expression:"TestHostName"}})],1),n("div",{staticClass:"row"},[n("q-input",{staticClass:"col col-xs-12",attrs:{dense:!1,readonly:""},on:{dblclick:t.openSSH},scopedSlots:t._u([{key:"before",fn:function(){return[n("span",{staticStyle:{"font-size":"14px"}},[t._v("SSH端点:")])]},proxy:!0}]),model:{value:t.SSHSelect,callback:function(e){t.SSHSelect=e},expression:"SSHSelect"}})],1)]),n("q-separator"),n("q-card-actions",{attrs:{align:"right"}},[n("q-btn",{attrs:{flat:"",label:"取消",color:"primary"},on:{click:t.newTestHostCancel}}),n("q-btn",{attrs:{flat:"",label:"创建",color:"primary"},on:{click:t.newTestHostConfirm}})],1)],1)],1),n("SSHLookUp",{ref:"SSHLookUp",attrs:{fixed:t.SSHlookUpFlag,SSHEndpointIndex:t.SSHSelectIndex,SSHEndpointList:t.SSHEndpointDataArr},on:{addSSHEndpoint:t.addSSHEndpoint,cancelSSHEndpoint:t.cancelSSHEndpoint}})],1)},a=[],s=(n("b0c0"),n("365c")),i=n("b5bd"),l={name:"MasterHost",components:{SSHLookUp:i["a"]},data:function(){return{SSHlookUpFlag:!1,SSHSelect:"",SSHSelectId:"",SSHSelectIndex:-1,SSHEndpointDataArr:[],TestHostName:"",createTestHostFlag:!1,MasterHostList:[],MasterHostPagination:{page:1,rowsNumber:1},MasterHostSelected:[],MasterHostColumns:[{name:"address",required:!0,label:"地址",align:"left",field:function(t){return t.address},format:function(t){return"".concat(t)}}],SSHEndpointList:[],SSHEndpointPagination:{page:1,rowsNumber:1},SSHEndpointSelected:[],SSHEndpointColumns:[{name:"name",required:!0,label:"名称",align:"left",field:function(t){return t.name},format:function(t){return"".concat(t)}},{name:"type",align:"left",label:"类型",field:"type"},{name:"configuration",label:"配置",align:"left",field:"configuration"}],createSSHEndpointFlag:!1,Name:"",Type:"",Configuration:""}},mounted:function(){this.getSSHEndpointList()},methods:{getSSHEndpointList:function(t){var e=this;this.$q.loading.show();var n={matchName:"",page:t||1,pageSize:""};s["s"](n).then((function(n){console.log(n),e.SSHEndpointList=n.data.results,e.SSHEndpointPagination.page=t||1,e.SSHEndpointPagination.rowsNumber=Math.ceil(n.data.totalCount/50),e.getTestHostList(),e.getSSHEndpointData()}))},openSSHCreate:function(){this.createSSHEndpointFlag=!0},newSSHCancel:function(){this.Name="",this.Type="",this.Configuration="",this.createSSHEndpointFlag=!1},newSSHCreate:function(){var t=this,e={Name:this.Name,Type:this.Type,Configuration:this.Configuration};this.Name||this.Type||this.Configuration?(this.$q.loading.show(),console.log(e),s["C"](e).then((function(e){console.log(e),t.newSSHCancel(),t.getSSHEndpointList(),t.$q.notify({position:"top",message:"提示",caption:"创建成功",color:"secondary"})}))):this.$q.notify({position:"top",message:"提示",caption:"请填写完整信息",color:"red"})},deleteSSH:function(){var t=this;0!=this.SSHEndpointSelected.length?this.$q.dialog({title:"提示",message:"您确定要删除当前选择的SSH端口吗",persistent:!0,ok:{push:!0,label:"确定"},cancel:{push:!0,label:"取消"}}).onOk((function(){if(1==t.SSHEndpointSelected.length){var e="?id=".concat(t.SSHEndpointSelected[0].id);t.$q.loading.show(),s["c"](e).then((function(){t.$q.notify({position:"top",message:"提示",caption:"删除成功",color:"secondary"}),t.getSSHEndpointList()}))}else if(t.SSHEndpointSelected.length>1){for(var n={delArr:[]},o=0;o<t.SSHEndpointSelected.length;o++)n.delArr.push(t.SSHEndpointSelected[o].id);t.$q.loading.show(),s["d"](n).then((function(){t.$q.notify({position:"top",message:"提示",caption:"删除成功",color:"secondary"}),t.getSSHEndpointList()}))}})):this.$q.notify({position:"top",message:"提示",caption:"请选择SSH端口",color:"red"})},toSSHEndpointDetail:function(t,e){this.$router.push({name:"SSHEndpointDetail",query:{id:e.id}})},SSHNextPage:function(t){this.getSSHEndpointList(t)},getTestHostList:function(t){var e=this,n={matchName:"",page:t||1,pageSize:""};s["B"](n).then((function(n){console.log(n),e.MasterHostList=n.data.results,e.MasterHostPagination.page=t||1,e.MasterHostPagination.rowsNumber=Math.ceil(n.data.totalCount/50),e.$q.loading.hide()}))},getSSHEndpointData:function(){var t=this;s["q"]({}).then((function(e){console.log(e),t.SSHEndpointDataArr=e.data}))},openHostCreate:function(){this.createTestHostFlag=!0},newTestHostCancel:function(){this.createTestHostFlag=!1,this.TestHostName="",this.SSHSelect="",this.SSHSelectId="",this.SSHSelectIndex=-1},newTestHostConfirm:function(){var t=this;if(this.TestHostName&&this.SSHSelectId){this.$q.loading.show();var e={Address:this.TestHostName,SSHEndpointID:this.SSHSelectId};s["G"](e).then((function(){t.getSSHEndpointList(),t.newTestHostCancel(),t.$q.notify({position:"top",message:"提示",caption:"创建成功",color:"secondary"})}))}else this.$q.notify({position:"top",message:"提示",caption:"请填写完整信息",color:"red"})},openSSH:function(){this.SSHlookUpFlag=!0,this.createTestHostFlag=!1},addSSHEndpoint:function(t){if(void 0==t)return!1;this.SSHSelect=this.SSHEndpointDataArr[t].name,this.SSHSelectId=this.SSHEndpointDataArr[t].id,this.SSHSelectIndex=t,this.SSHlookUpFlag=!1,this.createTestHostFlag=!0},cancelSSHEndpoint:function(){this.SSHlookUpFlag=!1,this.createTestHostFlag=!0,this.$refs.SSHLookUp.selectIndex=this.SSHSelectIndex},deleteTestHost:function(){var t=this;0!=this.MasterHostSelected.length?this.$q.dialog({title:"提示",message:"您确定要删除当前选择的主机吗",persistent:!0,ok:{push:!0,label:"确定"},cancel:{push:!0,label:"取消"}}).onOk((function(){if(1==t.MasterHostSelected.length){var e="?id=".concat(t.MasterHostSelected[0].id);t.$q.loading.show(),s["j"](e).then((function(){t.$q.notify({position:"top",message:"提示",caption:"删除成功",color:"secondary"}),t.getSSHEndpointList()}))}else if(t.MasterHostSelected.length>1){for(var n={delArr:[]},o=0;o<t.MasterHostSelected.length;o++)n.delArr.push(t.MasterHostSelected[o].id);t.$q.loading.show(),s["k"](n).then((function(){t.$q.notify({position:"top",message:"提示",caption:"删除成功",color:"secondary"}),t.getSSHEndpointList()}))}})):this.$q.notify({position:"top",message:"提示",caption:"请选择主机",color:"red"})},toTestHostDetail:function(t,e){this.$router.push({name:"TestHostDetail",query:{id:e.id}})},TestHostNextPage:function(t){this.getTestHostList(t)}}},c=l,r=(n("e3c7"),n("acce"),n("2877")),S=Object(r["a"])(c,o,a,!1,null,"16c21c98",null);e["default"]=S.exports},acce:function(t,e,n){"use strict";var o=n("1298"),a=n.n(o);a.a},b201:function(t,e,n){},b5bd:function(t,e,n){"use strict";var o=function(){var t=this,e=t.$createElement,n=t._self._c||e;return n("q-dialog",{attrs:{persistent:""},model:{value:t.fixed,callback:function(e){t.fixed=e},expression:"fixed"}},[n("q-card",[n("q-card-section",[n("div",{staticClass:"text-h6"},[t._v("SSH端口列表")])]),n("q-separator"),n("div",{staticClass:"new_input"},t._l(t.SSHEndpointList,(function(e,o){return n("q-item",{directives:[{name:"ripple",rawName:"v-ripple"}],key:o,attrs:{tag:"label"}},[n("q-item-section",{attrs:{avatar:""}},[n("q-radio",{attrs:{val:o,color:"teal"},model:{value:t.selectIndex,callback:function(e){t.selectIndex=e},expression:"selectIndex"}})],1),n("q-item-section",[n("q-item-label",[t._v(t._s(e.name))])],1)],1)})),1),n("q-separator"),n("q-card-actions",{attrs:{align:"right"}},[n("q-btn",{attrs:{flat:"",label:"取消",color:"primary"},on:{click:t.cancel}}),n("q-btn",{attrs:{flat:"",label:"添加",color:"primary"},on:{click:t.confirm}})],1)],1)],1)},a=[],s={props:["fixed","SSHEndpointIndex","SSHEndpointList"],name:"lookUp",data:function(){return{selectIndex:-1}},watch:{SSHEndpointIndex:function(t){this.selectIndex=t}},methods:{confirm:function(){this.$emit("addSSHEndpoint",this.selectIndex)},cancel:function(){this.$emit("cancelSSHEndpoint")}}},i=s,l=n("2877"),c=Object(l["a"])(i,o,a,!1,null,"1842ebc3",null);e["a"]=c.exports},e3c7:function(t,e,n){"use strict";var o=n("b201"),a=n.n(o);a.a}}]);
//# sourceMappingURL=chunk-3d9e77e4.03c9c7b2.js.map