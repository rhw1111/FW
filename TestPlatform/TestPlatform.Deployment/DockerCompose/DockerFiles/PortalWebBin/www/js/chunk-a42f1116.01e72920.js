(window["webpackJsonp"]=window["webpackJsonp"]||[]).push([["chunk-a42f1116"],{"174b":function(t,e,a){},"1cd1":function(t,e,a){"use strict";var n=a("990c"),i=a.n(n);i.a},"244f":function(t,e,a){"use strict";a.r(e);var n=function(){var t=this,e=t.$createElement,a=t._self._c||e;return a("div",{staticClass:"TestDataSource"},[a("div",{staticClass:"q-pa-md"},[a("q-table",{attrs:{title:"测试数据源列表",data:t.TestDataSourceList,columns:t.columns,selection:"multiple",selected:t.selected,"row-key":"id","rows-per-page-options":[0],"table-style":"max-height: 500px","no-data-label":"暂无数据更新"},on:{"update:selected":function(e){t.selected=e}},scopedSlots:t._u([{key:"top-right",fn:function(){return[a("q-btn",{staticClass:"btn",attrs:{color:"primary",label:"新 增"},on:{click:t.openCreate}}),a("q-btn",{staticClass:"btn",staticStyle:{background:"#FF0000",color:"white"},attrs:{label:"删 除"},on:{click:t.deleteTestDataSource}})]},proxy:!0},{key:"body-cell-id",fn:function(e){return[a("q-td",{staticClass:"text-left",attrs:{props:e}},[a("q-btn",{staticClass:"btn",attrs:{color:"primary",label:"更 新"},on:{click:function(a){return t.toDetail(e)}}})],1)]}},{key:"bottom",fn:function(){return[a("q-pagination",{staticClass:"col offset-md-10",attrs:{max:t.pagination.rowsNumber,input:!0},on:{input:t.switchPage},model:{value:t.pagination.page,callback:function(e){t.$set(t.pagination,"page",e)},expression:"pagination.page"}})]},proxy:!0}])})],1),a("q-dialog",{attrs:{persistent:""},model:{value:t.createFixed,callback:function(e){t.createFixed=e},expression:"createFixed"}},[a("q-card",{staticStyle:{width:"100%"}},[a("q-card-section",[a("div",{staticClass:"text-h6"},[t._v("创建测试数据源")])]),a("q-separator"),a("div",{staticClass:"new_input"},[a("div",{staticClass:"row input_row"},[a("q-input",{staticClass:"col",attrs:{dense:!1},scopedSlots:t._u([{key:"before",fn:function(){return[a("span",{staticStyle:{"font-size":"14px"}},[t._v("名称:")])]},proxy:!0}]),model:{value:t.Name,callback:function(e){t.Name=e},expression:"Name"}}),a("q-select",{staticClass:"col",attrs:{options:["String","Int","Json"],dense:!1},scopedSlots:t._u([{key:"before",fn:function(){return[a("span",{staticStyle:{"font-size":"14px"}},[t._v("类型:")])]},proxy:!0},{key:"prepend",fn:function(){},proxy:!0}]),model:{value:t.Type,callback:function(e){t.Type=e},expression:"Type"}})],1),a("div",{staticClass:"row input_row"},[a("q-input",{staticClass:"col-xs-12",attrs:{dense:!1,type:"textarea",outlined:""},scopedSlots:t._u([{key:"before",fn:function(){return[a("span",{staticStyle:{"font-size":"14px"}},[t._v("数据:")])]},proxy:!0}]),model:{value:t.Data,callback:function(e){t.Data=e},expression:"Data"}})],1)]),a("q-separator"),a("q-card-actions",{attrs:{align:"right"}},[a("q-btn",{attrs:{flat:"",label:"取消",color:"primary"},on:{click:t.newCancel}}),a("q-btn",{attrs:{flat:"",label:"创建",color:"primary"},on:{click:t.newCreate}})],1)],1)],1)],1)},i=[],s=(a("b0c0"),a("365c")),o={name:"TestDataSource",data:function(){return{createFixed:!1,TestDataSourceList:[],Name:"",Type:"",Data:"",selected:[],columns:[{name:"name",required:!0,label:"名称",align:"left",field:function(t){return t.name},format:function(t){return"".concat(t)}},{name:"type",align:"left",label:"类型",field:"type"},{name:"data",label:"数据",align:"left",field:"data"},{name:"id",label:"操作",align:"right",field:"id",headerStyle:"text-align:center",style:"width: 10%"}],pagination:{page:1,rowsNumber:1}}},mounted:function(){this.getTestDataSource()},methods:{getTestDataSource:function(t){var e=this;this.$q.loading.show();var a={matchName:"",page:t||1,pageSize:50};s["x"](a).then((function(a){console.log(a),e.pagination.page=t||1,e.pagination.rowsNumber=Math.ceil(a.data.totalCount/50),e.TestDataSourceList=a.data.results,e.selected=[],e.$q.loading.hide()}))},openCreate:function(){this.Name="",this.Type="",this.Data="",this.createFixed=!0},toDetail:function(t){this.$router.push({name:"TestDataSourceDetail",query:{id:t.row.id}})},switchPage:function(t){this.getTestDataSource(t)},newCreate:function(){var t=this,e={Name:this.Name,Type:this.Type,Data:this.Data};this.Name&&this.Type&&this.Data?(this.$q.loading.show(),s["E"](e).then((function(){t.getTestDataSource(),t.createFixed=!1,t.$q.notify({position:"top",message:"提示",caption:"创建成功",color:"secondary"})}))):this.$q.notify({position:"top",message:"提示",caption:"请填写完整信息",color:"red"})},newCancel:function(){this.createFixed=!1},deleteTestDataSource:function(){var t=this;0!=this.selected.length?this.$q.dialog({title:"提示",message:"您确定要删除当前选择的测试数据源吗",persistent:!0,ok:{push:!0,label:"确定"},cancel:{push:!0,label:"取消"}}).onOk((function(){if(1==t.selected.length){t.$q.loading.show();var e="?id=".concat(t.selected[0].id);s["g"](e).then((function(){t.selected=[],t.getTestDataSource()}))}else if(t.selected.length>1){t.$q.loading.show();for(var a=[],n=0;n<t.selected.length;n++)a.push(t.selected[n].id);var i={delArr:a};s["h"](i).then((function(){t.selected=[],t.getTestDataSource()}))}})):this.$q.notify({position:"top",message:"提示",caption:"请选择您要删除的测试数据源",color:"red"})}}},c=o,l=(a("c5cd"),a("1cd1"),a("2877")),r=Object(l["a"])(c,n,i,!1,null,"3f4d2f8e",null);e["default"]=r.exports},"990c":function(t,e,a){},c5cd:function(t,e,a){"use strict";var n=a("174b"),i=a.n(n);i.a}}]);
//# sourceMappingURL=chunk-a42f1116.01e72920.js.map