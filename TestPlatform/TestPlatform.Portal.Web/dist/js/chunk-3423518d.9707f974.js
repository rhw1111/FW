(window["webpackJsonp"]=window["webpackJsonp"]||[]).push([["chunk-3423518d"],{"1d20":function(t,e,a){},"1d47":function(t,e,a){"use strict";var n=a("f48d"),i=a.n(n);i.a},5954:function(t,e,a){"use strict";a.r(e);var n=function(){var t=this,e=t.$createElement,a=t._self._c||e;return a("div",{staticClass:"TestCase"},[a("div",{staticClass:"q-pa-md"},[a("transition",{attrs:{name:"TreeEntity-slid"}},[a("TreeEntity",{directives:[{name:"show",rawName:"v-show",value:t.expanded,expression:"expanded"}],staticStyle:{"max-width":"20%",height:"600px",overflow:"auto",float:"left"},attrs:{refs:"TreeEntity"},on:{getDirectoryLocation:t.getDirectoryLocation}})],1),a("div",[a("q-btn",{staticStyle:{width:"2%",height:"600px",float:"left"},attrs:{color:"grey",flat:"",dense:"",icon:t.expanded?"keyboard_arrow_left":"keyboard_arrow_right"},on:{click:function(e){t.expanded=!t.expanded}}}),a("q-table",{attrs:{title:"测试用例列表",data:t.TestCaseList,columns:t.columns,"row-key":"id",selection:"multiple",selected:t.selected,"rows-per-page-options":[0],"table-style":"max-height: 500px;","no-data-label":"暂无数据更新"},on:{"update:selected":function(e){t.selected=e}},scopedSlots:t._u([{key:"top-right",fn:function(){return[a("q-btn",{staticClass:"btn",attrs:{color:"primary",label:"运 行"},on:{click:t.runTestCase}}),a("q-btn",{staticClass:"btn",attrs:{color:"primary",label:"新 增"},on:{click:t.openCrateTestCase}})]},proxy:!0},{key:"bottom",fn:function(){return[a("q-pagination",{staticClass:"col offset-md-10",attrs:{max:t.pagination.rowsNumber,input:!0},on:{input:t.nextPage},model:{value:t.pagination.page,callback:function(e){t.$set(t.pagination,"page",e)},expression:"pagination.page"}})]},proxy:!0},{key:"body-cell-id",fn:function(e){return[a("q-td",{attrs:{props:e}},[a("q-btn",{directives:[{name:"show",rawName:"v-show",value:"正在运行"==e.row.status,expression:"props.row.status=='正在运行'"}],staticClass:"btn",attrs:{color:"primary",label:"查看主机日志"},on:{click:function(a){return t.lookMasterLog(e)}}}),a("q-btn",{staticClass:"btn",attrs:{color:"primary",label:"更 新"},on:{click:function(a){return t.toDetail(e)}}}),a("q-btn",{staticClass:"btn",staticStyle:{background:"#FF0000",color:"white"},attrs:{label:"删 除"},on:{click:function(a){return t.deleteTestCase(e)}}})],1)]}}])})],1)],1),a("q-dialog",{attrs:{persistent:""},model:{value:t.createFixed,callback:function(e){t.createFixed=e},expression:"createFixed"}},[a("q-card",{staticStyle:{width:"100%","max-width":"70vw"}},[a("q-card-section",[a("div",{staticClass:"text-h6"},[t._v("创建测试用例")])]),a("q-separator"),a("CreateShowTestCase",{ref:"CSTestCase",attrs:{masterHostList:t.masterHostList,currentDirectory:t.SelectLocation}}),a("q-separator"),a("q-card-actions",{attrs:{align:"right"}},[a("q-btn",{attrs:{flat:"",label:"取消",color:"primary"},on:{click:t.newCancel}}),a("q-btn",{attrs:{flat:"",label:"创建",color:"primary"},on:{click:t.newCreate}})],1)],1)],1)],1)},i=[],s=(a("99af"),a("a15b"),a("b0c0"),a("53ca")),o=a("365c"),l=a("10b4"),r=a("08d6"),c={name:"TestCase",components:{TreeEntity:r["a"],CreateShowTestCase:l["a"]},data:function(){return{createFixed:!1,TestCaseList:[],masterHostList:[],dataSourceName:[],selected:[],columns:[{name:"name",required:!0,label:"名称",align:"left",field:function(t){return t.name},format:function(t){return"".concat(t)}},{name:"engineType",align:"left",label:"引擎类型",field:"engineType",style:"max-width: 50px",headerStyle:"max-width: 50px"},{name:"configuration",label:"配置",align:"left",field:"configuration",style:"max-width: 250px",headerStyle:"max-width: 250px"},{name:"status",label:"状态",align:"left",field:"status"},{name:"id",label:"操作",align:"right",field:"id",headerStyle:"text-align:center"}],pagination:{page:1,rowsNumber:1},dismiss:null,expanded:!0,SelectLocation:""}},mounted:function(){this.getTestCaseList()},methods:{openCrateTestCase:function(){this.createFixed=!0},newCancel:function(){this.$refs.CSTestCase.newCancel(),this.createFixed=!1},newCreate:function(){var t=this;if(this.$refs.CSTestCase.newCreate()){var e=this.$refs.CSTestCase.newCreate();this.$q.loading.show(),o["K"](e).then((function(e){console.log(e),t.getTestCaseList(1,t.SelectLocation.id),t.$q.notify({position:"top",message:"提示",caption:"创建成功",color:"secondary"}),t.newCancel()}))}},getTestCaseList:function(t,e){var a=this;this.$q.loading.show();var n={parentId:e||null,matchName:"",page:t||1};o["y"](n).then((function(e){console.log(e),a.TestCaseList=e.data.results,a.pagination.page=t||1,a.pagination.rowsNumber=Math.ceil(e.data.totalCount/50),a.$q.loading.hide()}))},deleteTestCase:function(t){var e=this;console.log(t),this.$q.dialog({title:"提示",message:"您确定要删除当前的测试用例吗",persistent:!0,ok:{push:!0,label:"确定"},cancel:{push:!0,label:"取消"}}).onOk((function(){if(e.$q.loading.show(),null==t.row.treeID){var a="?id=".concat(t.row.id);o["f"](a).then((function(t){console.log(t),e.getTestCaseList(1,e.SelectLocation.id)}))}else{var n="?id=".concat(t.row.treeID);o["i"](n).then((function(t){console.log(t),e.getTestCaseList(1,e.SelectLocation.id)}))}}))},getMasterHostList:function(){var t=this;o["q"]().then((function(e){console.log(e),t.masterHostList=e.data,t.$q.loading.hide()}))},getSlaveHostsList:function(t,e){o["v"]({caseId:t}).then((function(t){0==t.data.length?e(!0):e(!1)}))},getDataSourceName:function(){var t=this,e={};o["k"](e).then((function(e){console.log(e),t.dataSourceName=e.data,t.$q.loading.hide()}))},nextPage:function(t){this.getTestCaseList(t,this.SelectLocation.id)},toDetail:function(t){this.$router.push({name:"TestCaseDetail",query:{id:t.row.id}})},runTestCase:function(){var t=this;if(0!=this.selected.length){var e=function(){for(var e=[],a=0;a<t.selected.length;a++)"正在运行"==t.selected[a].status&&e.push(t.selected[a].name);if(0!=e.length)return t.$q.notify({position:"top",message:"提示",caption:"当前测试用例".concat(e.join("，"),"正在运行当中,请重新选择"),color:"red"}),{v:void 0};t.$q.loading.show();for(var n=function(a){t.getSlaveHostsList(t.selected[a].id,(function(n){n&&e.push(t.selected[a].name),a==t.selected.length-1&&(0!=e.length?(t.$q.notify({position:"top",message:"提示",caption:"当前测试用例".concat(e.join("，"),"下没有从主机，请添加从主机再进行运行。"),color:"red"}),t.$q.loading.hide()):t.run(0))}))},i=0;i<t.selected.length;i++)n(i)}();if("object"===Object(s["a"])(e))return e.v}else this.$q.notify({position:"top",message:"提示",caption:"请选择测试用例",color:"red"})},run:function(t){var e=this;console.log(t),this.dismiss&&this.dismiss();var a=t;this.$q.loading.show();var n="?caseId=".concat(this.selected[a].id);o["Q"](n).then((function(t){console.log(t),e.dismiss=e.$q.notify({position:"top-right",caption:"当前测试用例".concat(e.selected[a].name,"正在运行当中。").concat(a+1,"/").concat(e.selected.length),color:"teal",timeout:"0"}),e.timerOut=window.setInterval((function(){setTimeout(e.getTestCaseStatus(a),0)}),3e3)}))},getTestCaseStatus:function(t){var e=this;o["z"]({caseId:this.selected[t].id}).then((function(a){if(!a.data){if(t==e.selected.length-1)return clearInterval(e.timerOut),e.timerOut=null,e.dismiss(),e.getMasterHostList(),void(e.selected=[]);clearInterval(e.timerOut),e.timerOut=null,setTimeout((function(){e.run(t+1)}),12e4)}}))},lookMasterLog:function(t){var e=this;this.$q.loading.show(),o["r"]({caseId:t.row.id}).then((function(t){e.$q.loading.hide(),e.$q.dialog({title:"提示",message:t.data,style:{width:"100%","max-width":"65vw","white-space":"pre-line","overflow-x":"hidden","word-break":"break-all"}})}))},getDirectoryLocation:function(t){console.log(t),this.getTestCaseList(1,t.id),this.SelectLocation=t}}},d=c,u=(a("66f1"),a("1d47"),a("2877")),f=Object(u["a"])(d,n,i,!1,null,"3d3282cf",null);e["default"]=f.exports},"66f1":function(t,e,a){"use strict";var n=a("1d20"),i=a.n(n);i.a},f48d:function(t,e,a){}}]);
//# sourceMappingURL=chunk-3423518d.9707f974.js.map