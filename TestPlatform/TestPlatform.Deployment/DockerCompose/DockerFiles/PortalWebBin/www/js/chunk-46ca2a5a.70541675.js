(window["webpackJsonp"]=window["webpackJsonp"]||[]).push([["chunk-46ca2a5a"],{"0cca":function(t,e,n){"use strict";var a=n("7df1"),i=n.n(a);i.a},4830:function(t,e,n){},"7df1":function(t,e,n){},a17a:function(t,e,n){"use strict";var a=n("4830"),i=n.n(a);i.a},b146:function(t,e,n){"use strict";n.r(e);var a=function(){var t=this,e=t.$createElement,n=t._self._c||e;return n("div",{staticClass:"detail"},[n("div",{staticClass:"detail_header"},[n("q-btn",{staticClass:"btn",attrs:{color:"primary",label:"保 存"},on:{click:t.putTestHost}}),n("q-btn",{staticClass:"btn",staticStyle:{background:"#FF0000",color:"white"},attrs:{label:"删 除"},on:{click:t.deleteTestHost}})],1),n("div",{staticClass:"q-pa-md row"},[n("div",{staticClass:"new_input"},[n("div",{staticClass:"row input_row"},[n("q-input",{staticClass:"col",attrs:{dense:!1},scopedSlots:t._u([{key:"before",fn:function(){return[n("span",{staticStyle:{"font-size":"14px"}},[t._v("地址:")])]},proxy:!0}]),model:{value:t.Name,callback:function(e){t.Name=e},expression:"Name"}})],1),n("div",{staticClass:"row input_row"},[n("q-input",{staticClass:"col col-xs-12",attrs:{dense:!1,readonly:""},on:{dblclick:t.openSSH},scopedSlots:t._u([{key:"before",fn:function(){return[n("span",{staticStyle:{"font-size":"14px"}},[t._v("SSH终结点:")])]},proxy:!0}]),model:{value:t.SSHSelect,callback:function(e){t.SSHSelect=e},expression:"SSHSelect"}})],1)])]),n("SSHLookUp",{ref:"SSHLookUp",attrs:{fixed:t.SSHlookUpFlag,SSHEndpointIndex:t.SSHSelectIndex,SSHEndpointList:t.SSHEndpointDataArr},on:{addSSHEndpoint:t.addSSHEndpoint,cancelSSHEndpoint:t.cancelSSHEndpoint}})],1)},i=[],o=(n("b0c0"),n("365c")),s=n("b5bd"),c={name:"TestHostDetail",components:{SSHLookUp:s["a"]},data:function(){return{DetailId:"",TestHostDetail:"",Name:"",SSHEndpoint:"",SSHlookUpFlag:!1,SSHSelect:"",SSHSelectId:"",SSHSelectIndex:-1,SSHEndpointDataArr:[],TestHostName:""}},mounted:function(){this.DetailId=this.$route.query.id,this.getTestHostDetail()},methods:{getTestHostDetail:function(){var t=this;this.$q.loading.show();var e={id:this.DetailId};o["y"](e).then((function(e){console.log(e),t.TestHostDetail=e.data,t.Name=e.data.address,t.getSSHEndpointData(e.data.sshEndpointID)}))},getSSHEndpointData:function(t){var e=this;o["o"]({}).then((function(n){console.log(n),e.SSHEndpointDataArr=n.data;for(var a=0;a<n.data.length;a++)if(n.data[a].id==t){e.SSHSelect=n.data[a].name,e.SSHSelectId=n.data[a].id,e.SSHSelectIndex=a;break}e.$q.loading.hide()}))},putTestHost:function(){var t=this,e={ID:this.DetailId,Address:this.Name,SSHEndpointID:this.SSHSelectId};""!=this.Name&&""!=this.SSHSelectId?(this.$q.loading.show(),o["L"](e).then((function(){t.$q.notify({position:"top",message:"提示",caption:"更新成功",color:"secondary"}),t.getTestHostDetail()}))):this.$q.notify({position:"top",message:"提示",caption:"请填写完整信息",color:"red"})},deleteTestHost:function(){var t=this;this.$q.dialog({title:"提示",message:"您确定要删除当前选择的主机吗",persistent:!0,ok:{push:!0,label:"确定"},cancel:{push:!0,label:"取消"}}).onOk((function(){var e="?id=".concat(t.DetailId);t.$q.loading.show(),o["i"](e).then((function(){t.$q.notify({position:"top",message:"提示",caption:"删除成功",color:"secondary"}),t.$router.push({name:"MasterHost"})}))}))},openSSH:function(){this.SSHlookUpFlag=!0,this.createTestHostFlag=!1},addSSHEndpoint:function(t){if(void 0==t)return!1;this.SSHSelect=this.SSHEndpointDataArr[t].name,this.SSHSelectId=this.SSHEndpointDataArr[t].id,this.SSHSelectIndex=t,this.SSHlookUpFlag=!1,this.createTestHostFlag=!0},cancelSSHEndpoint:function(){this.SSHlookUpFlag=!1,this.createTestHostFlag=!0,this.$refs.SSHLookUp.selectIndex=this.SSHSelectIndex}}},l=c,d=(n("a17a"),n("0cca"),n("2877")),r=Object(d["a"])(l,a,i,!1,null,"2967d4ba",null);e["default"]=r.exports},b5bd:function(t,e,n){"use strict";var a=function(){var t=this,e=t.$createElement,n=t._self._c||e;return n("q-dialog",{attrs:{persistent:""},model:{value:t.fixed,callback:function(e){t.fixed=e},expression:"fixed"}},[n("q-card",[n("q-card-section",[n("div",{staticClass:"text-h6"},[t._v("SSH端口列表")])]),n("q-separator"),n("div",{staticClass:"new_input"},t._l(t.SSHEndpointList,(function(e,a){return n("q-item",{directives:[{name:"ripple",rawName:"v-ripple"}],key:a,attrs:{tag:"label"}},[n("q-item-section",{attrs:{avatar:""}},[n("q-radio",{attrs:{val:a,color:"teal"},model:{value:t.selectIndex,callback:function(e){t.selectIndex=e},expression:"selectIndex"}})],1),n("q-item-section",[n("q-item-label",[t._v(t._s(e.name))])],1)],1)})),1),n("q-separator"),n("q-card-actions",{attrs:{align:"right"}},[n("q-btn",{attrs:{flat:"",label:"取消",color:"primary"},on:{click:t.cancel}}),n("q-btn",{attrs:{flat:"",label:"添加",color:"primary"},on:{click:t.confirm}})],1)],1)],1)},i=[],o={props:["fixed","SSHEndpointIndex","SSHEndpointList"],name:"lookUp",data:function(){return{selectIndex:-1}},watch:{SSHEndpointIndex:function(t){this.selectIndex=t}},methods:{confirm:function(){this.$emit("addSSHEndpoint",this.selectIndex)},cancel:function(){this.$emit("cancelSSHEndpoint")}}},s=o,c=n("2877"),l=Object(c["a"])(s,a,i,!1,null,"1842ebc3",null);e["a"]=l.exports}}]);
//# sourceMappingURL=chunk-46ca2a5a.70541675.js.map