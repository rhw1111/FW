(window["webpackJsonp"]=window["webpackJsonp"]||[]).push([["chunk-000e2325"],{"08d6":function(e,t,a){"use strict";var i=function(){var e=this,t=e.$createElement,a=e._self._c||t;return a("div",{staticClass:"TreeEntity"},[a("div",{staticClass:"q-pa-md"},[a("el-tree",{attrs:{data:e.simple,props:e.defaultProps,"highlight-current":!0,"expand-on-click-node":!1},on:{"node-expand":e.unfoldTree,"node-click":e.handleNodeClick}})],1)])},o=[],n=(a("4de4"),a("a434"),a("b0c0"),a("365c")),r={name:"TreeEntity",props:["existingDirectories","DirectoryLocation"],data:function(){return{simple:[{id:null,label:"根目录",children:[]}],defaultProps:{children:"children",label:"label"},DisablesSelectedDirectories:[],SelectLocation:""}},mounted:function(){console.log(this.DirectoryLocation),this.DisablesSelectedDirectories=this.existingDirectories||[],console.log(this.DisablesSelectedDirectories),this.getTreeEntityList()},methods:{getTreeEntityList:function(e,t){var a=this;this.$q.loading.show();var i={parentId:t||null,matchName:"",page:e||1,type:1,pageSize:100};n["D"](i).then((function(e){console.log(e);for(var t=e.data.results,i=0;i<e.data.results.length;i++)for(var o=0;o<a.DisablesSelectedDirectories.length;o++)a.DisablesSelectedDirectories[o].parentID!=e.data.results[i].id&&a.DisablesSelectedDirectories[o].id!=e.data.results[i].id||t.splice(i,1,"");t=t.filter((function(e){return e}));for(var n=0;n<t.length;n++)a.simple[0].children.push({id:t[n].id,label:t[n].name,parentID:t[n].parentID,type:t[n].type,value:t[n].value,children:[{}]});a.$q.loading.hide()}))},unfoldTree:function(e){var t=this;console.log(e),console.log(this.DisablesSelectedDirectories),this.$q.loading.show();var a={parentId:e.id,matchName:"",page:1,type:1,pageSize:100};n["D"](a).then((function(a){console.log(a);for(var i=a.data.results,o=0;o<a.data.results.length;o++){console.log(a.data.results);for(var n=0;n<t.DisablesSelectedDirectories.length;n++)if(t.DisablesSelectedDirectories[n].parentID==a.data.results[o].id||t.DisablesSelectedDirectories[n].id==a.data.results[o].id){i[o]="";break}}i=i.filter((function(e){return e})),e.children=[];for(var r=0;r<i.length;r++)e.children||t.$set(e,"children",[{}]),e.children.push({id:i[r].id,label:i[r].name,parentID:i[r].parentID,type:i[r].type,value:i[r].value,children:[{}]});t.$q.loading.hide()}))},handleNodeClick:function(e){console.log(e),this.$emit("getDirectoryLocation",e),this.SelectLocation=e},getDirectoryLocation:function(){return this.SelectLocation},getDirectoryStructure:function(){return this.simple}}},s=r,c=a("2877"),l=Object(c["a"])(s,i,o,!1,null,"0ec03a33",null);t["a"]=l.exports},"1cd1":function(e,t,a){"use strict";var i=a("ff4b"),o=a.n(i);o.a},"244f":function(e,t,a){"use strict";a.r(t);var i=function(){var e=this,t=e.$createElement,a=e._self._c||t;return a("div",{staticClass:"TestDataSource"},[a("div",{staticClass:"q-pa-md"},[a("transition",{attrs:{name:"TreeEntity-slid"}},[e.expanded?a("TreeEntity",{staticStyle:{"max-width":"20%",height:"100%",overflow:"auto",float:"left"},on:{getDirectoryLocation:e.getDirectoryLocation}}):e._e()],1),a("div",{staticStyle:{height:"100%"}},[a("q-btn",{staticStyle:{width:"2%",height:"100%",float:"left"},attrs:{color:"grey",flat:"",dense:"",icon:e.expanded?"keyboard_arrow_left":"keyboard_arrow_right"},on:{click:function(t){e.expanded=!e.expanded}}}),a("q-table",{attrs:{title:"测试数据源列表",data:e.TestDataSourceList,columns:e.columns,selection:"multiple",selected:e.selected,"row-key":"id","rows-per-page-options":[0],"no-data-label":"暂无数据更新"},on:{"update:selected":function(t){e.selected=t}},scopedSlots:e._u([{key:"top-right",fn:function(){return[a("q-btn",{staticClass:"btn",attrs:{color:"primary",label:"新 增"},on:{click:e.openCreate}}),a("q-btn",{staticClass:"btn",staticStyle:{background:"#FF0000",color:"white"},attrs:{label:"删 除"},on:{click:e.deleteTestDataSource}})]},proxy:!0},{key:"body-cell-id",fn:function(t){return[a("q-td",{staticClass:"text-left",attrs:{props:t}},[a("q-btn",{staticClass:"btn",attrs:{color:"primary",label:"更 新"},on:{click:function(a){return e.getTestDataSourceDetail(t)}}}),a("q-btn",{staticClass:"btn",attrs:{color:"red",label:"删 除"},on:{click:function(a){return e.deleteTestDataSourceOne(t.row)}}})],1)]}},{key:"bottom",fn:function(){return[a("q-pagination",{staticClass:"col offset-md-10",attrs:{max:e.pagination.rowsNumber,input:!0},on:{input:e.switchPage},model:{value:e.pagination.page,callback:function(t){e.$set(e.pagination,"page",t)},expression:"pagination.page"}})]},proxy:!0}])})],1)],1),a("q-dialog",{attrs:{persistent:""},model:{value:e.createFixed,callback:function(t){e.createFixed=t},expression:"createFixed"}},[a("q-card",{staticStyle:{width:"100%","max-width":"70vw"}},[a("q-card-section",[a("div",{staticClass:"text-h6"},[e._v("创建测试数据源")])]),a("q-separator"),a("CreatePut",{ref:"createDataSource",attrs:{currentDirectory:e.SelectLocation}}),a("q-separator"),a("q-card-actions",{attrs:{align:"right"}},[a("q-btn",{attrs:{flat:"",label:"取消",color:"primary"},on:{click:e.newCancel}}),a("q-btn",{attrs:{flat:"",label:"创建",color:"primary"},on:{click:e.newCreate}})],1)],1)],1),a("q-dialog",{attrs:{persistent:""},model:{value:e.LookDataSourceFixed,callback:function(t){e.LookDataSourceFixed=t},expression:"LookDataSourceFixed"}},[a("q-card",{staticStyle:{width:"100%","max-width":"70vw"}},[a("q-card-section",[a("div",{staticClass:"text-h6"},[e._v("更新测试数据源")])]),a("q-separator"),a("CreatePut",{ref:"putDataSource",attrs:{detailData:e.detailData}}),a("q-separator"),a("q-card-actions",{attrs:{align:"right"}},[a("q-btn",{attrs:{flat:"",label:"取消",color:"primary"},on:{click:e.cancelPutDataSource}}),a("q-btn",{attrs:{flat:"",label:"更新",color:"primary"},on:{click:e.putTestDataSource}})],1)],1)],1)],1)},o=[],n=(a("b0c0"),a("365c")),r=a("bed4"),s=a("08d6"),c={name:"TestDataSource",components:{TreeEntity:s["a"],CreatePut:r["a"]},data:function(){return{createFixed:!1,LookDataSourceFixed:!1,TestDataSourceList:[],detailData:"",selected:[],columns:[{name:"name",required:!0,label:"名称",align:"left",field:function(e){return e.name},format:function(e){return"".concat(e)}},{name:"type",align:"left",label:"类型",field:"type"},{name:"data",label:"数据",align:"left",field:"data",style:"max-width: 250px",headerStyle:"max-width: 250px"},{name:"id",label:"操作",align:"right",field:"id",headerStyle:"text-align:center",style:"width: 10%"}],pagination:{page:1,rowsNumber:1},expanded:!1,SelectLocation:""}},mounted:function(){this.getTestDataSource(1,null,!0)},methods:{getTestDataSource:function(e,t,a){var i=this;this.$q.loading.show();var o={parentId:t||null,matchName:"",page:e||1,pageSize:50};n["z"](o).then((function(t){console.log(t),i.pagination.page=e||1,i.pagination.rowsNumber=Math.ceil(t.data.totalCount/50),i.TestDataSourceList=t.data.results,i.selected=[],a?i.expanded=!0:i.$q.loading.hide()}))},getTestDataSourceDetail:function(e){var t=this;this.$q.loading.show();var a={id:e.row.id};n["A"](a).then((function(e){console.log(e);var a=e.data;t.detailData={SelectedId:a.id,Name:a.name,Type:a.type,Data:a.data,ChangeFileDirectoryName:a.parentName,ChangeFileDirectoryId:a.parentID},t.LookDataSourceFixed=!0,t.$q.loading.hide()}))},openCreate:function(){this.createFixed=!0},toDetail:function(){this.LookDataSourceFixed=!0,this.getTestDataSourceDetail()},switchPage:function(e){this.getTestDataSource(e,this.SelectLocation.id)},newCreate:function(){var e=this;if(this.$refs.createDataSource.newCreate()){var t=this.$refs.createDataSource.newCreate();this.$q.loading.show(),n["K"](t).then((function(){e.getTestDataSource(1,e.SelectLocation.id),e.createFixed=!1,e.$q.notify({position:"top",message:"提示",caption:"创建成功",color:"secondary"})}))}},newCancel:function(){this.$refs.createDataSource.newCancel(),this.createFixed=!1},cancelPutDataSource:function(){this.$refs.putDataSource.cancelPutDataSource(),this.LookDataSourceFixed=!1},putTestDataSource:function(){var e=this;if(this.$refs.putDataSource.putTestDataSource()){var t=this.$refs.putDataSource.putTestDataSource();this.$q.loading.show(),n["X"](t).then((function(t){console.log(t),e.getTestDataSource(1,e.SelectLocation.id),e.$q.notify({position:"top",message:"提示",caption:"更新成功",color:"secondary"})}))}},deleteTestDataSource:function(){var e=this;if(0!=this.selected.length){this.$q.dialog({title:"提示",message:"您确定要删除当前选择的测试数据源吗",persistent:!0,ok:{push:!0,label:"确定"},cancel:{push:!0,label:"取消"}}).onOk((function(){if(1==e.selected.length)if(e.$q.loading.show(),null==e.selected[0].treeID){var t="?id=".concat(e.selected[0].id);n["g"](t).then((function(){e.selected=[],e.$q.notify({position:"top",message:"提示",caption:"删除成功",color:"secondary"}),e.getTestDataSource(1,e.SelectLocation.id)}))}else{var a="?id=".concat(e.selected[0].treeID);n["i"](a).then((function(t){e.selected=[],console.log(t),e.$q.notify({position:"top",message:"提示",caption:"删除成功",color:"secondary"}),e.getTestDataSource(1,e.SelectLocation.id)}))}else e.selected.length>1&&(e.$q.loading.show(),i())}));var t=this,a=0}else this.$q.notify({position:"top",message:"提示",caption:"请选择您要删除的测试数据源",color:"red"});function i(){if(a!=t.selected.length)if(null==t.selected[a].treeID){var e="?id=".concat(t.selected[a].id);n["g"](e).then((function(e){console.log(e),a++,i()}))}else{var o="?id=".concat(t.selected[a].treeID);n["i"](o).then((function(e){console.log(e),a++,i()}))}else t.selected=[],t.$q.notify({position:"top",message:"提示",caption:"删除成功",color:"secondary"}),t.getTestDataSource(1,t.SelectLocation.id)}},deleteTestDataSourceOne:function(e){var t=this;this.$q.dialog({title:"提示",message:"您确定要删除当前选择的测试数据源吗",persistent:!0,ok:{push:!0,label:"确定"},cancel:{push:!0,label:"取消"}}).onOk((function(){if(t.$q.loading.show(),null==e.treeID){var a="?id=".concat(e.id);n["g"](a).then((function(){t.selected=[],t.$q.notify({position:"top",message:"提示",caption:"删除成功",color:"secondary"}),t.getTestDataSource(1,t.SelectLocation.id)}))}else{var i="?id=".concat(e.treeID);n["i"](i).then((function(e){t.selected=[],console.log(e),t.$q.notify({position:"top",message:"提示",caption:"删除成功",color:"secondary"}),t.getTestDataSource(1,t.SelectLocation.id)}))}}))},getDirectoryLocation:function(e){console.log(e),this.getTestDataSource(1,e.id),this.SelectLocation=e}}},l=c,d=(a("c364"),a("1cd1"),a("2877")),u=Object(d["a"])(l,i,o,!1,null,"9292a2ba",null);t["default"]=u.exports},"780b":function(e,t,a){},bed4:function(e,t,a){"use strict";var i=function(){var e=this,t=e.$createElement,a=e._self._c||t;return a("div",[a("q-dialog",{attrs:{persistent:""},model:{value:e.ChangeFileDirectoryFlag,callback:function(t){e.ChangeFileDirectoryFlag=t},expression:"ChangeFileDirectoryFlag"}},[a("q-card",{staticStyle:{width:"100%"}},[a("q-card-section",[a("div",{staticClass:"text-h6"},[e._v("选择文件目录位置")])]),a("q-separator"),a("TreeEntity",{ref:"TreeEntity"}),a("q-separator"),a("q-card-actions",{attrs:{align:"right"}},[a("q-btn",{attrs:{flat:"",label:"取消",color:"primary"},on:{click:function(t){e.ChangeFileDirectoryFlag=!1}}}),a("q-btn",{attrs:{flat:"",label:"确定",color:"primary"},on:{click:e.SelectDirectoryLocation}})],1)],1)],1),a("div",{staticClass:"new_input"},[a("div",{staticClass:"row input_row"},[a("q-input",{staticClass:"col",attrs:{dense:!1},scopedSlots:e._u([{key:"before",fn:function(){return[a("span",{staticStyle:{"font-size":"14px"}},[e._v("名称:")])]},proxy:!0}]),model:{value:e.Name,callback:function(t){e.Name=t},expression:"Name"}}),a("q-select",{staticClass:"col",attrs:{options:["String","Int","Json","Label"],dense:!1},scopedSlots:e._u([{key:"before",fn:function(){return[a("span",{staticStyle:{"font-size":"14px"}},[e._v("类型:")])]},proxy:!0},{key:"prepend",fn:function(){},proxy:!0}]),model:{value:e.Type,callback:function(t){e.Type=t},expression:"Type"}})],1),a("div",{staticClass:"row input_row"},[a("q-input",{staticClass:"col",attrs:{dense:!1,readonly:"",placeholder:"点击右侧加号选择文件目录位置"},scopedSlots:e._u([{key:"before",fn:function(){return[a("span",{staticStyle:{"font-size":"14px"}},[e._v("文件目录位置:")])]},proxy:!0},{key:"append",fn:function(){return[a("q-btn",{attrs:{round:"",dense:"",flat:"",icon:"add"},on:{click:e.ChangeFileDirectory}})]},proxy:!0}]),model:{value:e.ChangeFileDirectoryName,callback:function(t){e.ChangeFileDirectoryName=t},expression:"ChangeFileDirectoryName"}})],1),a("div",{staticClass:"row input_row"},[a("q-input",{staticClass:"col-xs-12",attrs:{dense:!1,type:"textarea","input-style":{height:"380px"},outlined:""},scopedSlots:e._u([{key:"before",fn:function(){return[a("span",{staticStyle:{"font-size":"14px"}},[e._v("数据:")])]},proxy:!0}]),model:{value:e.Data,callback:function(t){e.Data=t},expression:"Data"}})],1)])],1)},o=[],n=(a("a15b"),a("a9e3"),a("498a"),a("53ca")),r=a("365c"),s=a("08d6"),c={props:["detailData","currentDirectory"],components:{TreeEntity:s["a"]},data:function(){return{SelectedId:"",Name:"",Type:"",Data:"",ChangeFileDirectoryFlag:!1,ChangeFileDirectoryName:"",ChangeFileDirectoryId:null}},mounted:function(){if(this.detailData){var e=this.detailData;console.log(e),this.SelectedId=e.SelectedId,this.Name=e.Name,this.Type=e.Type,this.Data=e.Data,e.ChangeFileDirectoryId?this.getTreeEntityTreePath(e.ChangeFileDirectoryId):this.ChangeFileDirectoryName=""!=e.ChangeFileDirectoryName?e.ChangeFileDirectoryName:"根目录 >",this.ChangeFileDirectoryId=e.ChangeFileDirectoryId}this.currentDirectory?(this.getTreeEntityTreePath(this.currentDirectory.id),this.ChangeFileDirectoryId=this.currentDirectory.id):this.ChangeFileDirectoryName="根目录 >"},methods:{newCreate:function(){var e={Name:this.Name.trim(),Type:this.Type,Data:this.Data.trim(),FolderID:this.ChangeFileDirectoryId};if(this.Name.trim()&&this.Type&&this.Data.trim()){if(!this.isDataType(this.Type))return;return e}return this.$q.notify({position:"top",message:"提示",caption:"请填写完整信息",color:"red"}),!1},newCancel:function(){this.Name="",this.Type="",this.Data=""},cancelPutDataSource:function(){this.SelectedId="",this.Name="",this.Type="",this.Data="",this.ChangeFileDirectoryName="",this.ChangeFileDirectoryId=null},putTestDataSource:function(){var e={ID:this.SelectedId,Name:this.Name.trim(),Type:this.Type,Data:this.Data.trim(),FolderID:this.ChangeFileDirectoryId};if(this.SelectedId&&this.Name.trim()&&this.Type&&this.Data.trim()){if(!this.isDataType(this.Type))return;return this.$q.loading.show(),e}return this.$q.notify({position:"top",message:"提示",caption:"请填写完整信息",color:"red"}),!1},isDataType:function(e){return"Int"==e?!!Number(this.Data)||(this.$q.notify({position:"top",message:"提示",caption:"当前数据不是Int类型",color:"red"}),!1):"Json"!=e||!!this.isJSON(this.Data.trim())},isJSON:function(e){if("string"==typeof e)try{var t=JSON.parse(e);if("object"==Object(n["a"])(t)&&t)return!0;this.$q.notify({position:"top",message:"提示",caption:"配置不是正确的JSON格式",color:"red"})}catch(a){this.$q.notify({position:"top",message:"提示",caption:"配置不是正确的JSON格式",color:"red"})}},ChangeFileDirectory:function(){this.ChangeFileDirectoryFlag=!0},SelectDirectoryLocation:function(){this.$refs.TreeEntity.getDirectoryLocation().id?this.getTreeEntityTreePath(this.$refs.TreeEntity.getDirectoryLocation().id):this.ChangeFileDirectoryName=this.$refs.TreeEntity.getDirectoryLocation().label||"根目录 >",this.ChangeFileDirectoryId=this.$refs.TreeEntity.getDirectoryLocation().id||null,this.ChangeFileDirectoryFlag=!1},getTreeEntityTreePath:function(e){var t=this;console.log(e),this.$q.loading.show();var a={id:e};r["F"](a).then((function(e){console.log(e),t.ChangeFileDirectoryName="根目录 > "+e.data.join(" > "),t.$q.loading.hide()}))}}},l=c,d=a("2877"),u=Object(d["a"])(l,i,o,!1,null,"a3dc0bea",null);t["a"]=u.exports},c364:function(e,t,a){"use strict";var i=a("780b"),o=a.n(i);o.a},ff4b:function(e,t,a){}}]);
//# sourceMappingURL=chunk-000e2325.dbbe88a5.js.map