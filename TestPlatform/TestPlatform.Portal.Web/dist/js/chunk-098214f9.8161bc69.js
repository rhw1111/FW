(window["webpackJsonp"]=window["webpackJsonp"]||[]).push([["chunk-098214f9"],{3843:function(t,e,s){"use strict";var a=s("c719"),o=s.n(a);o.a},7729:function(t,e,s){"use strict";var a=function(){var t=this,e=t.$createElement,s=t._self._c||e;return s("q-dialog",{attrs:{persistent:""},model:{value:t.fixed,callback:function(e){t.fixed=e},expression:"fixed"}},[s("q-card",[s("q-card-section",[s("div",{staticClass:"text-h6"},[t._v("Master Host List")])]),s("q-separator"),s("div",{staticClass:"new_input"},t._l(t.masterHostList,(function(e,a){return s("q-item",{directives:[{name:"ripple",rawName:"v-ripple"}],key:a,attrs:{tag:"label"}},[s("q-item-section",{attrs:{avatar:""}},[s("q-radio",{attrs:{val:a,color:"teal"},model:{value:t.selectIndex,callback:function(e){t.selectIndex=e},expression:"selectIndex"}})],1),s("q-item-section",[s("q-item-label",[t._v(t._s(e.address))])],1)],1)})),1),s("q-separator"),s("q-card-actions",{attrs:{align:"right"}},[s("q-btn",{attrs:{flat:"",label:"取消",color:"primary"},on:{click:t.cancel}}),s("q-btn",{attrs:{flat:"",label:"添加",color:"primary"},on:{click:t.confirm}})],1)],1)],1)},o=[],n={props:["fixed","masterHostList","masterSelectIndex"],name:"lookUp",data:function(){return{selectIndex:-1}},watch:{masterSelectIndex:function(t){this.selectIndex=t}},methods:{confirm:function(){this.$emit("addMasterHost",this.selectIndex)},cancel:function(){this.$emit("cancelMasterHost")}}},i=n,l=s("2877"),c=Object(l["a"])(i,a,o,!1,null,"1210330f",null);e["a"]=c.exports},a5e2:function(t,e,s){"use strict";var a=s("a962"),o=s.n(a);o.a},a962:function(t,e,s){},c5c3:function(t,e,s){"use strict";s.r(e);var a=function(){var t=this,e=t.$createElement,s=t._self._c||e;return s("div",{staticClass:"detail"},[s("lookUp",{ref:"lookUp",attrs:{masterHostList:t.masterHostList,masterSelectIndex:t.masterSelectIndex,fixed:t.HostFixed},on:{addMasterHost:t.addMasterHost,cancelMasterHost:t.cancelMasterHost}}),s("div",{staticClass:"detail_header"},[s("q-btn",{staticClass:"btn",attrs:{color:"primary",label:"保 存"},on:{click:t.putSlaveHost}}),s("q-btn",{staticClass:"btn",staticStyle:{background:"#FF0000",color:"white"},attrs:{label:"删 除"},on:{click:t.deleteSlaveHost}})],1),s("div",{staticClass:"q-pa-md row"},[s("div",{staticClass:"new_input"},[s("div",{staticClass:"row"},[s("q-input",{staticClass:"col",attrs:{dense:!1},scopedSlots:t._u([{key:"before",fn:function(){return[s("span",{staticStyle:{"font-size":"14px"}},[t._v("SlaveName:")])]},proxy:!0}]),model:{value:t.SlaveHostName,callback:function(e){t.SlaveHostName=e},expression:"SlaveHostName"}}),s("q-input",{staticClass:"col",staticStyle:{"margin-left":"50px"},attrs:{dense:!1},scopedSlots:t._u([{key:"before",fn:function(){return[s("span",{staticStyle:{"font-size":"14px"}},[t._v("Count:")])]},proxy:!0}]),model:{value:t.SlaveCount,callback:function(e){t.SlaveCount=e},expression:"SlaveCount"}}),s("q-input",{staticClass:"col",attrs:{dense:!1,readonly:""},on:{dblclick:t.masterHost},scopedSlots:t._u([{key:"before",fn:function(){return[s("span",{staticStyle:{"font-size":"14px"}},[t._v("主机:")])]},proxy:!0}]),model:{value:t.masterHostSelect,callback:function(e){t.masterHostSelect=e},expression:"masterHostSelect"}})],1),s("div",{staticClass:"row"},[s("q-input",{staticClass:"col-xs-12",attrs:{dense:!1,type:"textarea",outlined:""},scopedSlots:t._u([{key:"before",fn:function(){return[s("span",{staticStyle:{"font-size":"14px"}},[t._v("ExtensionInfo:")])]},proxy:!0}]),model:{value:t.SlaveExtensionInfo,callback:function(e){t.SlaveExtensionInfo=e},expression:"SlaveExtensionInfo"}})],1)])])],1)},o=[],n=(s("99af"),s("a9e3"),s("365c")),i=s("7729"),l={name:"SlaveHostDetail",components:{lookUp:i["a"]},data:function(){return{HostFixed:!1,masterHostList:[],masterHostSelect:"",masterSelectIndex:"",MasterHostID:"",SlaveHostData:"",SlaveHostName:"",SlaveCount:"",SlaveExtensionInfo:""}},mounted:function(){this.$q.loading.show(),this.SlaveHostData=JSON.parse(sessionStorage.getItem("SlaveHostDetailData")),console.log(this.SlaveHostData),this.getMasterHostList(),this.SlaveHostName=this.SlaveHostData.slaveName,this.SlaveCount=this.SlaveHostData.count,this.SlaveExtensionInfo=this.SlaveHostData.extensionInfo,this.MasterHostID=this.SlaveHostData.hostID},methods:{getMasterHostList:function(){var t=this;n["k"]().then((function(e){console.log(e),t.masterHostList=e.data;for(var s=0;s<e.data.length;s++)if(e.data[s].id==t.SlaveHostData.hostID){t.masterHostSelect=e.data[s].address,t.masterSelectIndex=s;break}t.$q.loading.hide()}))},masterHost:function(){this.HostFixed=!0},addMasterHost:function(t){if(void 0==t)return!1;this.masterHostSelect=this.masterHostList[t].address,this.MasterHostID=this.masterHostList[t].id,this.masterSelectIndex=t,this.HostFixed=!1,console.log(this.masterHostSelect,this.MasterHostID,this.masterSelectIndex)},cancelMasterHost:function(){this.HostFixed=!1,this.$refs.lookUp.selectIndex=this.masterSelectIndex},getSlaveHostsList:function(){var t=this;n["m"]({caseId:this.SlaveHostData.testCaseID}).then((function(e){console.log(e);for(var s=0;s<e.data.length;s++)if(e.data[s].id==t.SlaveHostData.id){sessionStorage.setItem("SlaveHostDetailData",JSON.stringify(e.data[s])),t.$q.loading.hide();break}}))},putSlaveHost:function(){var t=this,e={ID:this.SlaveHostData.id,HostID:this.MasterHostID,TestCaseID:this.SlaveHostData.testCaseID,SlaveName:this.SlaveHostName,Count:Number(this.SlaveCount),ExtensionInfo:this.SlaveExtensionInfo};this.SlaveHostData.id&&this.SlaveHostData.testCaseID&&this.SlaveHostName&&this.SlaveCount&&this.SlaveExtensionInfo&&this.MasterHostID?(this.$q.loading.show(),n["y"](e).then((function(e){console.log(e),t.getSlaveHostsList(),t.$q.notify({position:"top",message:"提示",caption:"保存成功",color:"secondary"})}))):this.$q.notify({position:"top",message:"提示",caption:"请填写完整信息",color:"red"})},deleteSlaveHost:function(){var t=this;this.$q.dialog({title:"提示",message:"您确定要删除当前SalveHost吗",persistent:!0,ok:{push:!0,label:"确定"},cancel:{push:!0,label:"取消"}}).onOk((function(){t.$q.loading.show();var e="?id=".concat(t.SlaveHostData.id,"&caseId=").concat(t.SlaveHostData.testCaseID);n["c"](e).then((function(e){console.log(e),t.$router.push({name:"TestCaseDetail",query:{id:t.SlaveHostData.testCaseID}})}))})).onCancel((function(){}))}}},c=l,r=(s("3843"),s("a5e2"),s("2877")),d=Object(r["a"])(c,a,o,!1,null,"00bf2859",null);e["default"]=d.exports},c719:function(t,e,s){}}]);
//# sourceMappingURL=chunk-098214f9.8161bc69.js.map