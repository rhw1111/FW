(window["webpackJsonp"]=window["webpackJsonp"]||[]).push([["chunk-5d1eb69c"],{"0efa":function(e,t,a){"use strict";var n=a("38bd"),i=a.n(n);i.a},"1d47":function(e,t,a){"use strict";var n=a("f48d"),i=a.n(n);i.a},"38bd":function(e,t,a){},5954:function(e,t,a){"use strict";a.r(t);var n=function(){var e=this,t=e.$createElement,a=e._self._c||t;return a("div",{staticClass:"TestCase"},[a("div",{staticClass:"q-pa-md"},[a("transition",{attrs:{name:"TreeEntity-slid"}},[a("TreeEntity",{directives:[{name:"show",rawName:"v-show",value:e.expanded,expression:"expanded"}],staticStyle:{"max-width":"20%",height:"100%",overflow:"auto",float:"left"},attrs:{refs:"TreeEntity"},on:{getDirectoryLocation:e.getDirectoryLocation}})],1),a("div",{staticStyle:{height:"100%"}},[a("q-btn",{staticStyle:{width:"2%",height:"100%",float:"left"},attrs:{color:"grey",flat:"",dense:"",icon:e.expanded?"keyboard_arrow_left":"keyboard_arrow_right"},on:{click:function(t){e.expanded=!e.expanded}}}),a("q-table",{attrs:{title:"测试用例列表",data:e.TestCaseList,columns:e.columns,"row-key":"id",selection:"multiple",selected:e.selected,"rows-per-page-options":[0],"no-data-label":"暂无数据更新"},on:{"update:selected":function(t){e.selected=t}},scopedSlots:e._u([{key:"top-right",fn:function(){return[a("q-btn",{staticClass:"btn",attrs:{color:"primary",label:"运 行"},on:{click:e.openRunModel}}),a("q-btn",{staticClass:"btn",attrs:{color:"primary",label:"新 增"},on:{click:e.openCrateTestCase}})]},proxy:!0},{key:"bottom",fn:function(){return[a("q-pagination",{staticClass:"col offset-md-10",attrs:{max:e.pagination.rowsNumber,input:!0},on:{input:e.nextPage},model:{value:e.pagination.page,callback:function(t){e.$set(e.pagination,"page",t)},expression:"pagination.page"}})]},proxy:!0},{key:"body-cell-id",fn:function(t){return[a("q-td",{attrs:{props:t}},[a("q-btn",{directives:[{name:"show",rawName:"v-show",value:"正在运行"==t.row.status,expression:"props.row.status=='正在运行'"}],staticClass:"btn",attrs:{color:"primary",label:"查看主机日志"},on:{click:function(a){return e.lookMasterLog(t)}}}),a("q-btn",{staticClass:"btn",attrs:{color:"primary",label:"更 新"},on:{click:function(a){return e.toDetail(t)}}}),a("q-btn",{staticClass:"btn",staticStyle:{background:"#FF0000",color:"white"},attrs:{label:"删 除"},on:{click:function(a){return e.deleteTestCase(t)}}})],1)]}}])})],1)],1),a("q-dialog",{attrs:{persistent:""},model:{value:e.createFixed,callback:function(t){e.createFixed=t},expression:"createFixed"}},[a("q-card",{staticStyle:{width:"100%","max-width":"70vw"}},[a("q-card-section",[a("div",{staticClass:"text-h6"},[e._v("创建测试用例")])]),a("q-separator"),a("CreateShowTestCase",{ref:"CSTestCase",attrs:{masterHostList:e.masterHostList,currentDirectory:e.SelectLocation}}),a("q-separator"),a("q-card-actions",{attrs:{align:"right"}},[a("q-btn",{attrs:{flat:"",label:"取消",color:"primary"},on:{click:e.newCancel}}),a("q-btn",{attrs:{flat:"",label:"创建",color:"primary"},on:{click:e.newCreate}})],1)],1)],1),a("q-dialog",{attrs:{persistent:""},model:{value:e.runFixed,callback:function(t){e.runFixed=t},expression:"runFixed"}},[a("q-card",{staticStyle:{width:"100%","max-width":"50vw"}},[a("q-card-section",[a("div",{staticClass:"text-h6"},[e._v("请选择运行测试用例的模式")])]),a("q-separator"),a("div",{staticClass:"q-pa-md"},[a("q-radio",{attrs:{val:"parallel",label:"并行模式"},model:{value:e.runModel,callback:function(t){e.runModel=t},expression:"runModel"}}),a("q-radio",{attrs:{val:"sequential",label:"顺序执行"},model:{value:e.runModel,callback:function(t){e.runModel=t},expression:"runModel"}}),a("div",{directives:[{name:"show",rawName:"v-show",value:"parallel"==e.runModel,expression:"runModel=='parallel'"}]},e._l(e.runModelArray,(function(t,n){return a("div",{key:n,staticClass:"input_row row",staticStyle:{"margin-bottom":"10px",width:"100%",display:"inlin-block"}},[a("q-input",{staticClass:"col-8",attrs:{outlined:"",dense:!0,placeholder:"请输入执行时间,默认0秒。"},on:{input:function(a){return e.forceUpdate(t.executionTime,n)}},scopedSlots:e._u([{key:"before",fn:function(){return[a("span",{staticStyle:{"font-size":"14px",width:"150px","word-wrap":"break-word"}},[e._v(e._s(t.name)+":")])]},proxy:!0},{key:"append",fn:function(){return[a("q-avatar",[e._v(" 秒 ")])]},proxy:!0}],null,!0),model:{value:t.executionTime,callback:function(a){e.$set(t,"executionTime",a)},expression:"value.executionTime"}})],1)})),0)],1),a("q-separator"),a("q-card-actions",{attrs:{align:"right"}},[a("q-btn",{attrs:{flat:"",label:"取消",color:"primary"},on:{click:e.runCancelTestCase}}),a("q-btn",{attrs:{flat:"",label:"运行",color:"primary"},on:{click:e.runTestCase}})],1)],1)],1)],1)},i=[],o=(a("99af"),a("a15b"),a("b0c0"),a("a9e3"),a("ac1f"),a("5319"),a("365c")),s=a("10b4"),l=a("08d6"),r={name:"TestCase",components:{TreeEntity:l["a"],CreateShowTestCase:s["a"]},data:function(){return{createFixed:!1,TestCaseList:[],masterHostList:[],dataSourceName:[],selected:[],columns:[{name:"name",required:!0,label:"名称",align:"left",field:function(e){return e.name},format:function(e){return"".concat(e)}},{name:"engineType",align:"left",label:"引擎类型",field:"engineType",style:"max-width: 50px",headerStyle:"max-width: 50px"},{name:"configuration",label:"配置",align:"left",field:"configuration",style:"max-width: 250px",headerStyle:"max-width: 250px"},{name:"status",label:"状态",align:"left",field:"status"},{name:"id",label:"操作",align:"right",field:"id",headerStyle:"text-align:center"}],pagination:{page:1,rowsNumber:1},dismiss:null,expanded:!1,SelectLocation:"",runFixed:!1,runModel:"parallel",runModelArray:[]}},mounted:function(){this.getTestCaseList(1,null,!0)},methods:{openCrateTestCase:function(){this.createFixed=!0},newCancel:function(){this.$refs.CSTestCase.newCancel(),this.createFixed=!1},newCreate:function(){var e=this;if(this.$refs.CSTestCase.newCreate()){var t=this.$refs.CSTestCase.newCreate();this.$q.loading.show(),o["K"](t).then((function(t){console.log(t),e.getTestCaseList(1,e.SelectLocation.id),e.$q.notify({position:"top",message:"提示",caption:"创建成功",color:"secondary"}),e.newCancel()}))}},getTestCaseList:function(e,t,a){var n=this;this.$q.loading.show();var i={parentId:t||null,matchName:"",page:e||1};o["y"](i).then((function(t){console.log(t),n.TestCaseList=t.data.results,n.pagination.page=e||1,n.pagination.rowsNumber=Math.ceil(t.data.totalCount/50),a?n.expanded=!0:n.$q.loading.hide()}))},deleteTestCase:function(e){var t=this;console.log(e),this.$q.dialog({title:"提示",message:"您确定要删除当前的测试用例吗",persistent:!0,ok:{push:!0,label:"确定"},cancel:{push:!0,label:"取消"}}).onOk((function(){if(t.$q.loading.show(),null==e.row.treeID){var a="?id=".concat(e.row.id);o["f"](a).then((function(e){console.log(e),t.getTestCaseList(1,t.SelectLocation.id)}))}else{var n="?id=".concat(e.row.treeID);o["i"](n).then((function(e){console.log(e),t.getTestCaseList(1,t.SelectLocation.id)}))}}))},getMasterHostList:function(){var e=this;o["q"]().then((function(t){console.log(t),e.masterHostList=t.data,e.$q.loading.hide()}))},getSlaveHostsList:function(e,t){o["v"]({caseId:e}).then((function(e){0==e.data.length?t(!0):t(!1)}))},getDataSourceName:function(){var e=this,t={};o["k"](t).then((function(t){console.log(t),e.dataSourceName=t.data,e.$q.loading.hide()}))},nextPage:function(e){this.getTestCaseList(e,this.SelectLocation.id)},toDetail:function(e){this.$router.push({name:"TestCaseDetail",query:{id:e.row.id}})},openRunModel:function(){0!=this.selected.length?this.isSlaveHost():this.$q.notify({position:"top",message:"提示",caption:"请选择测试用例",color:"red"})},isSlaveHost:function(){var e=this;this.$q.loading.show();for(var t=[],a=0;a<this.selected.length;a++)"正在运行"==this.selected[a].status&&t.push(this.selected[a].name);if(0!=t.length)return this.$q.notify({position:"top",message:"提示",caption:"当前测试用例".concat(t.join("，"),"正在运行当中,请重新选择"),color:"red"}),void this.$q.loading.hide();for(var n=function(a){e.getSlaveHostsList(e.selected[a].id,(function(n){if(n&&t.push(e.selected[a].name),a==e.selected.length-1){if(0!=t.length)return e.$q.notify({position:"top",message:"提示",caption:"当前测试用例".concat(t.join("，"),"下没有从主机，请添加从主机再进行运行。"),color:"red"}),void e.$q.loading.hide();e.$q.loading.hide(),e.runFixed=!0;for(var i=0;i<e.selected.length;i++)e.runModelArray.push(e.selected[i]),e.runModelArray[i].executionTime=null,e.runModelArray[i].runStatus="未执行"}}))},i=0;i<this.selected.length;i++)n(i)},runTestCase:function(){"parallel"==this.runModel?this.ParallelExecution():this.run(0)},runCancelTestCase:function(){this.runFixed=!1,this.runModelArray=[]},ParallelExecution:function(){var e=this,t=this.$q.loading,a=0;t.show();for(var n=function(n){setTimeout((function(){var i="?caseId=".concat(e.selected[n].id);o["Q"](i).then((function(){a++,e.$q.notify({position:"top",message:"提示",caption:"".concat(e.selected[n].name,"运行完成"),color:"secondary"}),a==e.selected.length&&(e.runCancelTestCase(),e.getTestCaseList(1,e.SelectLocation),e.selected=[])})).catch((function(i){console.log(i),a++,e.$q.notify({position:"top",message:"提示",caption:"".concat(e.selected[n].name,"运行失败"),color:"red"}),setTimeout((function(){t.show()}),3e3),a==e.selected.length&&(t.hide(),e.runCancelTestCase(),e.getTestCaseList(1,e.SelectLocation),e.selected=[])}))}),1e3*e.selected[n].executionTime)},i=0;i<this.selected.length;i++)n(i)},run:function(e){var t=this;console.log(e),this.dismiss&&this.dismiss();var a=e;this.$q.loading.show();var n="?caseId=".concat(this.selected[a].id);o["Q"](n).then((function(e){console.log(e),t.dismiss=t.$q.notify({position:"top-right",caption:"当前测试用例".concat(t.selected[a].name,"正在运行当中。").concat(a+1,"/").concat(t.selected.length),color:"teal",timeout:"0"}),t.timerOut=window.setInterval((function(){setTimeout(t.getTestCaseStatus(a),0)}),3e3)}))},getTestCaseStatus:function(e){var t=this;o["z"]({caseId:this.selected[e].id}).then((function(a){if(!a.data){if(e==t.selected.length-1)return clearInterval(t.timerOut),t.timerOut=null,t.dismiss(),t.getMasterHostList(),t.runFixed=!1,t.selected=[],t.runModelArray=[],void t.$q.notify({position:"top",message:"提示",caption:"执行完成",color:"secondary"});clearInterval(t.timerOut),t.timerOut=null,setTimeout((function(){t.run(e+1)}),12e4)}}))},forceUpdate:function(e,t){this.$set(this.runModelArray[t],"executionTime",Number(e.replace(/[^\d]/g,"").replace(/^0/g,""))),this.$forceUpdate()},lookMasterLog:function(e){var t=this;this.$q.loading.show(),o["r"]({caseId:e.row.id}).then((function(e){t.$q.loading.hide(),t.$q.dialog({title:"提示",message:e.data,style:{width:"100%","max-width":"65vw","white-space":"pre-line","overflow-x":"hidden","word-break":"break-all"}})}))},getDirectoryLocation:function(e){console.log(e),this.getTestCaseList(1,e.id),this.SelectLocation=e}}},c=r,d=(a("0efa"),a("1d47"),a("2877")),u=Object(d["a"])(c,n,i,!1,null,"7ac1d051",null);t["default"]=u.exports},f48d:function(e,t,a){}}]);
//# sourceMappingURL=chunk-5d1eb69c.5d0975af.js.map