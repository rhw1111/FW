(window["webpackJsonp"]=window["webpackJsonp"]||[]).push([["chunk-c1ffd8e2"],{"1a8e":function(t,e,a){"use strict";a.r(e);var s=function(){var t=this,e=t.$createElement,a=t._self._c||e;return a("div",{staticClass:"detail"},[a("div",{staticClass:"detail_header"},[a("q-btn",{staticClass:"btn",attrs:{color:"primary",label:"保 存"},on:{click:t.putTestDataSource}}),a("q-btn",{staticClass:"btn",staticStyle:{background:"#FF0000",color:"white"},attrs:{label:"删 除"},on:{click:t.deleteTestDataSource}})],1),a("div",{staticClass:"q-pa-md row"},[a("div",{staticClass:"new_input"},[a("div",{staticClass:"row input_row"},[a("q-input",{staticClass:"col",attrs:{dense:!1},scopedSlots:t._u([{key:"before",fn:function(){return[a("span",{staticStyle:{"font-size":"14px"}},[t._v("名称:")])]},proxy:!0}]),model:{value:t.Name,callback:function(e){t.Name=e},expression:"Name"}}),a("q-input",{staticClass:"col",staticStyle:{"margin-left":"50px"},attrs:{dense:!1},scopedSlots:t._u([{key:"before",fn:function(){return[a("span",{staticStyle:{"font-size":"14px"}},[t._v("类型:")])]},proxy:!0}]),model:{value:t.Type,callback:function(e){t.Type=e},expression:"Type"}})],1),a("div",{staticClass:"row input_row"},[a("q-input",{staticClass:"col-xs-12",attrs:{dense:!1,type:"textarea",outlined:""},scopedSlots:t._u([{key:"before",fn:function(){return[a("span",{staticStyle:{"font-size":"14px"}},[t._v("数据:")])]},proxy:!0}]),model:{value:t.Data,callback:function(e){t.Data=e},expression:"Data"}})],1)])])])},i=[],n=(a("b0c0"),a("365c")),o={name:"TestCaseDetail",data:function(){return{TestDataSourceDetail:"",Id:"",Name:"",Type:"",Data:""}},mounted:function(){this.Id=this.$route.query.id,this.getTestDataSourceDetail()},methods:{getTestDataSourceDetail:function(){var t=this;this.$q.loading.show();var e={id:this.Id};n["z"](e).then((function(e){console.log(e),t.TestDataSourceDetail=e.data,t.Name=e.data.name,t.Type=e.data.type,t.Data=e.data.data,t.$q.loading.hide()}))},putTestDataSource:function(){var t=this,e={ID:this.Id,Name:this.Name,Type:this.Type,Data:this.Data};this.Id&&this.Name&&this.Type&&this.Data?(this.$q.loading.show(),n["M"](e).then((function(e){console.log(e),t.getTestDataSourceDetail(),t.$q.notify({position:"top",message:"提示",caption:"保存成功",color:"secondary"})}))):this.$q.notify({position:"top",message:"提示",caption:"请填写完整信息",color:"red"})},deleteTestDataSource:function(){var t=this;this.$q.dialog({title:"提示",message:"您确定要删除当前的测试数据源吗",persistent:!0,ok:{push:!0,label:"确定"},cancel:{push:!0,label:"取消"}}).onOk((function(){t.$q.loading.show();var e="?id=".concat(t.Id);n["h"](e).then((function(){t.$router.push({name:"TestDataSource"})}))}))}}},c=o,l=(a("27e9"),a("653f"),a("2877")),r=Object(l["a"])(c,s,i,!1,null,"63479806",null);e["default"]=r.exports},"27e9":function(t,e,a){"use strict";var s=a("e1c8"),i=a.n(s);i.a},"32af":function(t,e,a){},"653f":function(t,e,a){"use strict";var s=a("32af"),i=a.n(s);i.a},e1c8:function(t,e,a){}}]);
//# sourceMappingURL=chunk-c1ffd8e2.87b3d493.js.map