(window["webpackJsonp"]=window["webpackJsonp"]||[]).push([["chunk-3c34e05c"],{"3d36":function(t,e,a){"use strict";a.r(e);var o=function(){var t=this,e=t.$createElement,a=t._self._c||e;return a("div",{staticClass:"detail"},[a("div",{staticClass:"detail_header"},["DirectoryTestCaseDetail"==t.$route.name?a("q-btn",{staticClass:"btn",attrs:{color:"primary",label:"返 回 目 录"},on:{click:t.returnDirectory}}):t._e(),a("q-btn",{staticClass:"btn",attrs:{color:"primary",label:"保 存",disable:1==t.isNoRun},on:{click:t.putTestCase}}),a("q-btn",{staticClass:"btn",staticStyle:{background:"#FF0000",color:"white"},attrs:{label:"删 除",disable:1==t.isNoRun},on:{click:t.deleteTestCase}}),a("q-btn",{staticClass:"btn",attrs:{color:"primary",label:"运 行",disable:1==t.isNoRun},on:{click:t.isSlaveHost}}),a("q-btn",{staticClass:"btn",attrs:{color:"primary",label:"停 止",disable:1!=t.isNoRun},on:{click:t.stop}}),a("q-btn",{staticClass:"btn",attrs:{color:"primary",label:"查 看 状 态"},on:{click:t.lookStatus}}),a("q-btn",{staticClass:"btn",attrs:{color:"primary",label:"查 看 主 机 日 志"},on:{click:t.lookMasterLog}}),a("q-btn",{staticClass:"btn",attrs:{color:"primary",label:"性 能 监 测"},on:{click:t.lookMonitorUrl}})],1),a("div",{staticClass:"q-pa-md"},[a("CreateShowTestCase",{ref:"CSTestCase",attrs:{masterHostList:t.masterHostList,detailData:t.detailData}})],1),a("div",{staticClass:"q-pa-md row HostList"},[a("SlaveHost",{ref:"TestCaseSlaveHost",attrs:{isNoRun:t.isNoRun,detailData:t.detailData}}),a("History",{ref:"TestCaseHistory",attrs:{isNoRun:t.isNoRun,detailData:t.detailData}})],1),a("q-dialog",{attrs:{persistent:""},model:{value:t.CopyTestCaseFixed,callback:function(e){t.CopyTestCaseFixed=e},expression:"CopyTestCaseFixed"}},[a("q-card",{staticStyle:{width:"100%","max-width":"60vw"}},[a("q-card-section",[a("div",{staticClass:"text-h6"},[t._v("创建测试用例")])]),a("q-separator"),a("div",{staticClass:"new_input"},[a("div",{staticClass:"row input_row"},[a("q-input",{staticClass:"col",attrs:{dense:!1},scopedSlots:t._u([{key:"before",fn:function(){return[a("span",{staticStyle:{"font-size":"14px"}},[t._v("测试用例名称:")])]},proxy:!0}]),model:{value:t.CopyTestCaseName,callback:function(e){t.CopyTestCaseName=e},expression:"CopyTestCaseName"}}),a("q-select",{staticClass:"col",attrs:{options:["是","否"],dense:!1},scopedSlots:t._u([{key:"before",fn:function(){return[a("span",{staticStyle:{"font-size":"14px"}},[t._v("是否复制从主机:")])]},proxy:!0},{key:"prepend",fn:function(){},proxy:!0}]),model:{value:t.CopyTestCaseSlaveFlag,callback:function(e){t.CopyTestCaseSlaveFlag=e},expression:"CopyTestCaseSlaveFlag"}})],1)]),a("q-separator"),a("q-card-actions",{attrs:{align:"right"}},[a("q-btn",{attrs:{flat:"",label:"取消",color:"primary"},on:{click:t.CopyTestCaseCancel}}),a("q-btn",{attrs:{flat:"",label:"创建",color:"primary"},on:{click:t.CopyTestCaseCreate}})],1)],1)],1),a("q-dialog",{model:{value:t.lookMasterLogFlag,callback:function(e){t.lookMasterLogFlag=e},expression:"lookMasterLogFlag"}},[a("q-card",{staticClass:"q-dialog-plugin full-height",staticStyle:{width:"100%","max-width":"80vw",height:"800px",overflow:"hidden"}},[a("q-card-section",{staticClass:"row"},[a("div",{staticClass:"text-h6 col-11"},[t._v("主机日志")]),a("q-btn",{staticClass:"col-1",attrs:{color:"primary",label:"关 闭"},on:{click:function(e){t.lookMasterLogFlag=!1}}})],1),a("q-separator"),a("q-card-section",{staticStyle:{height:"85%",overflow:"hidden scroll","white-space":"pre-line","word-break":"break-all"}},[t._v(" "+t._s(t.lookMasterLogText)+" ")]),a("q-separator")],1)],1)],1)},s=[],i=(a("b0c0"),a("365c")),l=a("10b4"),n=function(){var t=this,e=t.$createElement,a=t._self._c||e;return a("div",{staticClass:"col-xs-12 col-sm-6 col-xl-6"},[a("q-list",{attrs:{bordered:""}},[a("q-table",{attrs:{title:"历史记录列表",data:t.HistoryList,columns:t.HistoryColumns,"row-key":"id",selection:"multiple",selected:t.HistorySelected,"rows-per-page-options":[0],"table-style":"max-height: 500px","no-data-label":"暂无数据更新"},on:{"update:selected":function(e){t.HistorySelected=e}},scopedSlots:t._u([{key:"top-right",fn:function(){return[a("q-btn",{staticClass:"btn",staticStyle:{"margin-right":"20px"},attrs:{color:"primary",label:"比 较",disable:1==t.isNoRun},on:{click:t.compareLog}}),a("q-btn",{staticClass:"btn",staticStyle:{background:"#FF0000",color:"white"},attrs:{label:"删 除",disable:1==t.isNoRun},on:{click:t.deleteHistory}})]},proxy:!0},{key:"body-cell-id",fn:function(e){return[a("q-td",{attrs:{props:e}},[a("q-btn",{staticClass:"btn",attrs:{color:"primary",label:"查 看",disable:1==t.isNoRun},on:{click:function(a){return t.getHistoryDetail(e)}}})],1)]}},{key:"bottom",fn:function(){return[a("q-pagination",{staticClass:"col offset-md-8",attrs:{max:t.pagination.rowsNumber,input:!0},on:{input:t.switchPage},model:{value:t.pagination.page,callback:function(e){t.$set(t.pagination,"page",e)},expression:"pagination.page"}})]},proxy:!0}])})],1),a("q-dialog",{attrs:{persistent:""},model:{value:t.lookHistoryDetailFlag,callback:function(e){t.lookHistoryDetailFlag=e},expression:"lookHistoryDetailFlag"}},[a("q-card",{staticStyle:{width:"100%","max-width":"60vw"}},[a("q-card-section",{staticClass:"row"},[a("div",{staticClass:"text-h6 col-6"},[t._v("历史记录")]),a("q-btn",{staticClass:"col-2",attrs:{flat:"",color:"primary",label:"日志分析",disable:1==t.isNoRun},on:{click:t.TransferFile}}),a("q-btn",{staticClass:"col-2",attrs:{flat:"",color:"primary",label:"日志分析状态",disable:1==t.isNoRun},on:{click:t.ViewFileStatus}}),a("q-btn",{staticClass:"col-2",attrs:{flat:"",color:"primary",label:"日志监测",disable:1==t.isNoRun},on:{click:t.lookMonitorUrl}})],1),a("q-separator"),a("div",{staticClass:"new_input"},[a("div",{staticClass:"row input_row"},[a("q-input",{staticClass:"col-7",attrs:{readonly:"",dense:!1},scopedSlots:t._u([{key:"before",fn:function(){return[a("span",{staticStyle:{"font-size":"14px"}},[t._v("创建时间:")])]},proxy:!0}]),model:{value:t.createTime,callback:function(e){t.createTime=e},expression:"createTime"}}),a("q-select",{staticClass:"col-5",attrs:{options:t.GatewayDataFormatList,dense:!1},scopedSlots:t._u([{key:"before",fn:function(){return[a("span",{staticStyle:{"font-size":"14px"}},[t._v("网关数据格式:")])]},proxy:!0},{key:"prepend",fn:function(){},proxy:!0}]),model:{value:t.GatewayDataFormat,callback:function(e){t.GatewayDataFormat=e},expression:"GatewayDataFormat"}})],1),a("div",{staticClass:"row input_row"},[a("q-input",{staticClass:"col",attrs:{dense:!1,readonly:"",type:"textarea",outlined:""},scopedSlots:t._u([{key:"before",fn:function(){return[a("span",{staticStyle:{"font-size":"14px"}},[t._v("总结:")])]},proxy:!0}]),model:{value:t.summary,callback:function(e){t.summary=e},expression:"summary"}})],1)]),a("q-separator"),a("q-card-actions",{attrs:{align:"right"}},[a("q-btn",{attrs:{flat:"",label:"关闭",color:"primary"},on:{click:function(e){t.lookHistoryDetailFlag=!1}}}),a("q-btn",{attrs:{flat:"",label:"保存",color:"primary"},on:{click:t.UpdateGatewayDataFormat}})],1)],1)],1),a("q-dialog",{attrs:{persistent:""},model:{value:t.HistoryCompareLogFlag,callback:function(e){t.HistoryCompareLogFlag=e},expression:"HistoryCompareLogFlag"}},[a("q-card",{staticStyle:{width:"100%","max-width":"85vw"}},[a("q-card-section",[a("div",{staticClass:"text-h6"},[t._v("日志比较")])]),a("q-separator"),a("q-table",{attrs:{data:t.HistoryCompareLogList,columns:t.HistoryCompareColumns,"row-key":"id","rows-per-page-options":[0],"table-style":"max-height: 500px","no-data-label":"暂无数据更新"},scopedSlots:t._u([{key:"bottom",fn:function(){},proxy:!0}])}),a("q-separator"),a("q-card-actions",{attrs:{align:"right"}},[a("q-btn",{attrs:{flat:"",label:"关闭",color:"primary"},on:{click:t.cancelHistoryCompare}})],1)],1)],1)],1)},r=[],c=(a("99af"),{name:"History",props:["isNoRun","detailData"],data:function(){return{lookHistoryDetailFlag:!1,HistoryCompareLogFlag:!1,HistoryList:[],HistorySelected:[],HistoryColumns:[{name:"createTime",required:!0,label:"创建时间",align:"left",field:function(t){return t.createTime},format:function(t){return"".concat(t)}},{name:"id",label:"操作",align:"right",field:"id",headerStyle:"text-align:center"}],pagination:{page:1,rowsNumber:1},HistoryCompareLogList:[],HistoryCompareColumns:[{name:"createTime",required:!0,label:"创建时间",align:"left",field:function(t){return t.createTime},format:function(t){return"".concat(t)},sortable:!0},{name:"ConnectCount",label:"连接数",align:"left",field:"ConnectCount",sortable:!0},{name:"ConnectFailCount",label:"连接失败数",align:"left",field:"ConnectFailCount",sortable:!0},{name:"ReqCount",label:"请求数",align:"left",field:"ReqCount",sortable:!0},{name:"ReqFailCount",label:"请求失败数",align:"left",field:"ReqFailCount",sortable:!0},{name:"MaxQPS",label:"最大QPS",align:"left",field:"MaxQPS",sortable:!0},{name:"MinQPS",label:"最小QPS",align:"left",field:"MinQPS",sortable:!0},{name:"AvgQPS",label:"平均QPS",align:"left",field:"AvgQPS",sortable:!0},{name:"MaxDuration",label:"最大响应时间（微秒）",align:"left",field:"MaxDuration",sortable:!0},{name:"MinDurartion",label:"最小响应时间（微秒）",align:"left",field:"MinDurartion",sortable:!0},{name:"AvgDuration",label:"平均响应时间（微秒）",align:"left",field:"AvgDuration",sortable:!0}],HistoryDetailData:{},createTime:"",summary:"",GatewayDataFormat:"",GatewayDataFormatList:[]}},methods:{getHistoryList:function(t){var e=this,a={caseId:this.$route.query.id,page:t||1,pageSize:50};i["m"](a).then((function(a){console.log(a),e.pagination.page=t||1,e.pagination.rowsNumber=Math.ceil(a.data.totalCount/50),e.HistoryList=a.data.results,e.HistorySelected=[],e.$q.loading.hide()}))},getHistoryDetail:function(t){var e=this;this.$q.loading.show();var a={caseId:this.$route.query.id,historyId:t.row.id};i["k"](a).then((function(t){console.log(t),e.HistoryDetailData=t.data,e.createTime=t.data.createTime,e.summary=JSON.stringify(JSON.parse(t.data.summary),null,2),e.GatewayDataFormat=t.data.netGatewayDataFormat,i["l"]().then((function(t){console.log(t),e.GatewayDataFormatList=t.data,e.$q.loading.hide(),e.lookHistoryDetailFlag=!0}))}))},deleteHistory:function(){var t=this;0!=this.HistorySelected.length?this.$q.dialog({title:"提示",message:"您确定要删除当前选择的历史记录吗",persistent:!0,ok:{push:!0,label:"确定"},cancel:{push:!0,label:"取消"}}).onOk((function(){if(t.$q.loading.show(),1==t.HistorySelected.length){var e="?caseId=".concat(t.detailData.id,"&historyId=").concat(t.HistorySelected[0].id);i["a"](e).then((function(){t.HistorySelected=[],t.getHistoryList()}))}else if(t.HistorySelected.length>1){for(var a=[],o=0;o<t.HistorySelected.length;o++)a.push(t.HistorySelected[o].id);console.log(a);var s={CaseID:t.detailData.id,IDS:a};i["b"](s).then((function(){t.HistorySelected=[],t.getHistoryList()}))}})):this.$q.notify({position:"top",message:"提示",caption:"请选择历史记录",color:"red"})},switchPage:function(t){this.$q.loading.show(),this.getHistoryList(t)},toHistoryDetail:function(t){this.$router.push({name:"HistoryDetail",query:{historyId:t.row.id,caseId:t.row.caseID}})},UpdateGatewayDataFormat:function(){var t=this;if(""!=this.GatewayDataFormat){var e={CaseID:this.HistoryDetailData.caseID,ID:this.HistoryDetailData.id,NetGatewayDataFormat:this.GatewayDataFormat};this.$q.loading.show(),i["O"](e).then((function(e){console.log(e),t.$q.loading.hide(),t.$q.notify({position:"top",message:"提示",caption:"保存成功",color:"secondary"})}))}else this.$q.notify({position:"top",message:"提示",caption:"请选择网关数据格式",color:"red"})},compareLog:function(){var t=this;if(console.log(this.HistorySelected),0!=this.HistorySelected.length&&1!=this.HistorySelected.length){for(var e=[],a=0;a<this.HistorySelected.length;a++)e.push(this.HistorySelected[a].id);this.$q.loading.show();var o={CaseID:this.$route.query.id,IDS:e};i["Q"](o).then((function(e){console.log(e);for(var a=0;a<e.data.length;a++)t.HistoryCompareLogList.push(JSON.parse(e.data[a].summary)),t.HistoryCompareLogList[a].createTime=e.data[a].createTime;t.$q.loading.hide(),t.HistoryCompareLogFlag=!0}))}else this.$q.notify({position:"top",message:"提示",caption:"请选择两个或两个以上的历史记录进行比较。",color:"red"})},cancelHistoryCompare:function(){this.HistoryCompareLogFlag=!1,this.HistoryCompareLogList=[]},lookMonitorUrl:function(){window.open(this.HistoryDetailData.monitorUrl)},TransferFile:function(){var t=this;this.$q.loading.show();var e={caseId:this.HistoryDetailData.caseID,historyId:this.HistoryDetailData.id};i["k"](e).then((function(a){console.log(a),a.data.netGatewayDataFormat?i["n"](e).then((function(e){console.log(e),0===e.data?(t.$q.notify({position:"top",message:"提示",caption:"没有需要分析的日志 ",color:"secondary"}),t.$q.loading.hide()):(t.$q.notify({position:"top",message:"提示",caption:"有".concat(e.data,"个文件开始分析"),color:"secondary"}),t.$q.loading.hide())})):(t.$q.loading.hide(),t.$q.notify({position:"top",message:"提示",caption:"当前历史记录网关数据格式为空，请选择网关数据格式并保存。",color:"red"}))}))},ViewFileStatus:function(){var t=this,e={caseId:this.HistoryDetailData.caseID,historyId:this.HistoryDetailData.id};this.$q.loading.show(),i["o"](e).then((function(e){console.log(e),t.$q.loading.hide(),e.data?t.$q.notify({position:"top",message:"提示",caption:"没有文件需要分析",color:"secondary"}):t.$q.notify({position:"top",message:"提示",caption:"还有文件需要分析",color:"secondary"})}))}}}),d=c,u=a("2877"),p=Object(u["a"])(d,n,r,!1,null,"73e9ee58",null),h=p.exports,f=function(){var t=this,e=t.$createElement,a=t._self._c||e;return a("div",{staticClass:"col-xs-12 col-sm-6 col-xl-6"},[a("lookUp",{ref:"SlaveHostHostlookUp",attrs:{masterHostList:t.masterHostList,masterSelectIndex:t.SlaveHostHostIndex,fixed:t.SlaveHostFixed},on:{addMasterHost:t.addSlaveHostHost,cancelMasterHost:t.cancelSlaveHostHost}}),a("q-dialog",{attrs:{persistent:""},model:{value:t.createFixed,callback:function(e){t.createFixed=e},expression:"createFixed"}},[a("q-card",{staticStyle:{width:"100%","max-width":"60vw"}},[a("q-card-section",[a("div",{staticClass:"text-h6"},[t._v("创建从主机")])]),a("q-separator"),a("div",{staticClass:"new_input"},[a("div",{staticClass:"row input_row"},[a("q-input",{staticClass:"col",attrs:{dense:!1},scopedSlots:t._u([{key:"before",fn:function(){return[a("span",{staticStyle:{"font-size":"14px"}},[t._v("从主机名称:")])]},proxy:!0}]),model:{value:t.SlaveHostName,callback:function(e){t.SlaveHostName=e},expression:"SlaveHostName"}}),a("q-input",{staticClass:"col",attrs:{dense:!1,placeholder:"副本数"},on:{keyup:function(e){t.SlaveCount=t.SlaveCount.replace(/[^\d]/g,"")}},scopedSlots:t._u([{key:"before",fn:function(){return[a("span",{staticStyle:{"font-size":"14px"}},[t._v("数量:")])]},proxy:!0}]),model:{value:t.SlaveCount,callback:function(e){t.SlaveCount=e},expression:"SlaveCount"}}),a("q-input",{staticClass:"col",attrs:{dense:!1,readonly:"",placeholder:"点击右侧加号选择主机"},scopedSlots:t._u([{key:"before",fn:function(){return[a("span",{staticStyle:{"font-size":"14px"}},[t._v("主机:")])]},proxy:!0},{key:"append",fn:function(){return[a("q-btn",{attrs:{round:"",dense:"",flat:"",icon:"add"},on:{click:t.dblSlaveHostHost}})]},proxy:!0}]),model:{value:t.SlaveHostHostSelect,callback:function(e){t.SlaveHostHostSelect=e},expression:"SlaveHostHostSelect"}})],1),a("div",{staticClass:"row input_row"},[a("q-input",{staticClass:"col",attrs:{dense:!1,type:"textarea",outlined:""},scopedSlots:t._u([{key:"before",fn:function(){return[a("span",{staticStyle:{"font-size":"14px"}},[t._v("扩展信息:")])]},proxy:!0}]),model:{value:t.SlaveExtensionInfo,callback:function(e){t.SlaveExtensionInfo=e},expression:"SlaveExtensionInfo"}})],1)]),a("q-separator"),a("q-card-actions",{attrs:{align:"right"}},[a("q-btn",{attrs:{flat:"",label:"取消",color:"primary"},on:{click:t.newCancel}}),a("q-btn",{attrs:{flat:"",label:"创建",color:"primary"},on:{click:t.newCreate}})],1)],1)],1),a("q-dialog",{attrs:{persistent:""},model:{value:t.UpdateFixed,callback:function(e){t.UpdateFixed=e},expression:"UpdateFixed"}},[a("q-card",{staticStyle:{width:"100%","max-width":"60vw"}},[a("q-card-section",[a("div",{staticClass:"text-h6"},[t._v("更新从主机")])]),a("q-separator"),a("div",{staticClass:"new_input"},[a("div",{staticClass:"row input_row"},[a("q-input",{staticClass:"col",attrs:{dense:!1},scopedSlots:t._u([{key:"before",fn:function(){return[a("span",{staticStyle:{"font-size":"14px"}},[t._v("从主机名称:")])]},proxy:!0}]),model:{value:t.SlaveHostName,callback:function(e){t.SlaveHostName=e},expression:"SlaveHostName"}}),a("q-input",{staticClass:"col",attrs:{dense:!1,placeholder:"副本数"},on:{keyup:function(e){t.SlaveCount=t.SlaveCount.replace(/[^\d]/g,"")}},scopedSlots:t._u([{key:"before",fn:function(){return[a("span",{staticStyle:{"font-size":"14px"}},[t._v("数量:")])]},proxy:!0}]),model:{value:t.SlaveCount,callback:function(e){t.SlaveCount=e},expression:"SlaveCount"}}),a("q-input",{staticClass:"col",attrs:{dense:!1,readonly:"",placeholder:"点击右侧加号选择主机"},scopedSlots:t._u([{key:"before",fn:function(){return[a("span",{staticStyle:{"font-size":"14px"}},[t._v("主机:")])]},proxy:!0},{key:"append",fn:function(){return[a("q-btn",{attrs:{round:"",dense:"",flat:"",icon:"add"},on:{click:t.dblSlaveHostHost}})]},proxy:!0}]),model:{value:t.SlaveHostHostSelect,callback:function(e){t.SlaveHostHostSelect=e},expression:"SlaveHostHostSelect"}})],1),a("div",{staticClass:"row input_row"},[a("q-input",{staticClass:"col",attrs:{dense:!1,type:"textarea",outlined:""},scopedSlots:t._u([{key:"before",fn:function(){return[a("span",{staticStyle:{"font-size":"14px"}},[t._v("扩展信息:")])]},proxy:!0}]),model:{value:t.SlaveExtensionInfo,callback:function(e){t.SlaveExtensionInfo=e},expression:"SlaveExtensionInfo"}})],1)]),a("q-separator"),a("q-card-actions",{attrs:{align:"right"}},[a("q-btn",{attrs:{flat:"",label:"取消",color:"primary"},on:{click:t.newUpdateCancel}}),a("q-btn",{attrs:{flat:"",label:"更新",color:"primary"},on:{click:t.newUpdateCreate}})],1)],1)],1),a("q-list",{attrs:{bordered:""}},[a("q-table",{attrs:{title:"从主机列表",data:t.SlaveHostList,columns:t.columns,"row-key":"id",selection:"multiple",selected:t.SlaveHostSelected,"table-style":"max-height: 500px","rows-per-page-options":[0],"no-data-label":"暂无数据更新"},on:{"update:selected":function(e){t.SlaveHostSelected=e}},scopedSlots:t._u([{key:"body-cell-id",fn:function(e){return[a("q-td",{attrs:{props:e}},["Jmeter"!=t.detailData.engineType?a("q-btn",{staticClass:"btn",staticStyle:{"margin-right":"20px"},attrs:{color:"primary",label:"查 看 日 志"},on:{click:function(a){return t.lookSlaveLog(e)}}}):t._e(),a("q-btn",{staticClass:"btn",attrs:{color:"primary",label:"更 新",disable:1==t.isNoRun},on:{click:function(a){return t.toSlaveHostDetail(e)}}})],1)]}},{key:"top-right",fn:function(){return[a("q-btn",{staticClass:"\n               btn",staticStyle:{"margin-right":"20px"},attrs:{color:"primary",label:"新 增",disable:1==t.isNoRun},on:{click:t.openSlaveHost}}),a("q-btn",{staticClass:"btn",staticStyle:{background:"#FF0000",color:"white"},attrs:{label:"删 除",disable:1==t.isNoRun},on:{click:t.deleteSlaveHost}})]},proxy:!0},{key:"bottom",fn:function(){},proxy:!0}])})],1),a("q-dialog",{attrs:{persistent:""},model:{value:t.lookLogFlag,callback:function(e){t.lookLogFlag=e},expression:"lookLogFlag"}},[a("q-card",{staticClass:"q-dialog-plugin full-height",staticStyle:{width:"100%","max-width":"80vw",height:"800px",overflow:"hidden"}},[a("q-card-section",{staticClass:"row"},[a("div",{staticClass:"text-h6 col-11"},[t._v("查看从主机日志")]),a("q-btn",{staticClass:"col-1",attrs:{color:"primary",label:"关 闭"},on:{click:t.cancelLookLog}})],1),a("q-separator"),a("q-card-section",{staticStyle:{height:"85%",overflow:"hidden"}},[a("q-splitter",{staticStyle:{height:"100%"},scopedSlots:t._u([{key:"before",fn:function(){return[a("q-tabs",{staticClass:"text-primary",attrs:{vertical:"","no-caps":!0},model:{value:t.tab,callback:function(e){t.tab=e},expression:"tab"}},t._l(t.SLaveHostSelect.count,(function(e,o){return a("q-tab",{key:o,attrs:{name:"tab"+o,label:t.SLaveHostSelect.slaveName+"_"+o},on:{click:function(e){return t.lookSalveIndexLog(o)}}})})),1)]},proxy:!0},{key:"after",fn:function(){return[a("q-tab-panel",{staticStyle:{height:"100%",overflow:"hidden scroll","white-space":"pre-line","word-break":"break-all"},attrs:{name:""}},[t._v(" "+t._s(t.SlaveLogText)+" ")])]},proxy:!0}]),model:{value:t.splitterModel,callback:function(e){t.splitterModel=e},expression:"splitterModel"}})],1),a("q-separator")],1)],1)],1)},g=[],v=(a("a9e3"),a("7729")),y={props:["isNoRun","detailData"],components:{lookUp:v["a"]},data:function(){return{createFixed:!1,UpdateFixed:!1,lookLogFlag:!1,SlaveHostFixed:!1,masterHostList:[],SlaveHostDetailData:"",SlaveHostName:"",SlaveCount:"",SlaveExtensionInfo:"",SlaveHostHostId:"",SlaveHostHostSelect:"",SlaveHostHostIndex:-1,SlaveHostList:[],SLaveHostSelect:"",SlaveHostSelected:[],columns:[{name:"slaveName",required:!0,label:"名称",align:"left",field:function(t){return t.slaveName},format:function(t){return"".concat(t)}},{name:"address",align:"left",label:"ip",field:"address"},{name:"count",align:"left",label:"数量",field:"count",style:"max-width: 50px",headerStyle:"max-width: 50px"},{name:"extensionInfo",label:"扩展信息",align:"left",field:"extensionInfo",style:"width:100px;"},{name:"id",label:"操作",align:"right",field:"id",headerStyle:"text-align:center"}],tab:"tab0",splitterModel:2,SlaveLogText:"",SlaveLogTextArr:[]}},methods:{getSlaveHostsList:function(){var t=this;this.$q.loading.show(),i["u"]({caseId:this.$route.query.id}).then((function(e){console.log(e),t.SlaveHostList=e.data,t.SlaveHostSelected=[],t.$q.loading.hide()}))},getMasterHostList:function(){var t=this;this.$q.loading.show(),i["p"]().then((function(e){console.log(e),t.masterHostList=e.data;for(var a=0;a<e.data.length;a++)if(e.data[a].id==t.detailData.masterHostID){t.masterSelectIndex=a;break}t.$q.loading.hide()}))},dblSlaveHostHost:function(){this.SlaveHostFixed=!0,this.getMasterHostList()},addSlaveHostHost:function(t){if(void 0==t)return!1;this.SlaveHostHostSelect=this.masterHostList[t].address,this.SlaveHostHostId=this.masterHostList[t].id,this.SlaveHostHostIndex=t,this.SlaveHostFixed=!1,console.log(this.SlaveHostHostSelect,this.SlaveHostHostId,this.SlaveHostHostIndex)},cancelSlaveHostHost:function(){this.SlaveHostFixed=!1,this.$refs.SlaveHostHostlookUp.selectIndex=this.SlaveHostHostIndex},openSlaveHost:function(){this.createFixed=!0},newCreate:function(){var t=this,e={HostID:this.SlaveHostHostId,TestCaseID:this.detailData.id,SlaveName:this.SlaveHostName,Count:Number(this.SlaveCount),ExtensionInfo:this.SlaveExtensionInfo};this.detailData.id&&this.SlaveHostName&&this.SlaveCount&&this.SlaveExtensionInfo&&this.SlaveHostHostId?(this.$q.loading.show(),i["J"](e).then((function(e){console.log(e),t.$q.notify({position:"top",message:"提示",caption:"创建成功",color:"secondary"}),t.newCancel(),t.getSlaveHostsList()}))):this.$q.notify({position:"top",message:"提示",caption:"请填写完整信息",color:"red"})},newCancel:function(){this.createFixed=!1,this.SlaveHostName="",this.SlaveCount="",this.SlaveExtensionInfo="",this.SlaveHostHostSelect="",this.SlaveHostHostId="",this.SlaveHostHostIndex=""},deleteSlaveHost:function(){var t=this;0!=this.SlaveHostSelected.length?this.$q.dialog({title:"提示",message:"您确定要删除当前选择的从主机吗",persistent:!0,ok:{push:!0,label:"确定"},cancel:{push:!0,label:"取消"}}).onOk((function(){if(t.$q.loading.show(),1==t.SlaveHostSelected.length){var e="?caseId=".concat(t.detailData.id,"&id=").concat(t.SlaveHostSelected[0].id);i["d"](e).then((function(e){console.log(e),t.SlaveHostSelected=[],t.getSlaveHostsList(),t.$q.notify({position:"top",message:"提示",caption:"删除成功",color:"secondary"})}))}else if(t.SlaveHostSelected.length>1){for(var a=[],o=0;o<t.SlaveHostSelected.length;o++)a.push(t.SlaveHostSelected[o].id);console.log(a);var s={CaseID:t.detailData.id,IDS:a};i["e"](s).then((function(e){console.log(e),t.SlaveHostSelected=[],t.getSlaveHostsList(),t.$q.notify({position:"top",message:"提示",caption:"删除成功",color:"secondary"})}))}})).onCancel((function(){})):this.$q.notify({position:"top",message:"提示",caption:"请选择从主机",color:"red"})},toSlaveHostDetail:function(t){var e=this;this.UpdateFixed=!0,this.$q.loading.show(),i["u"]({caseId:this.detailData.id}).then((function(a){console.log(a);for(var o=0;o<a.data.length;o++)a.data[o].id==t.row.id&&(console.log(a.data[o]),e.SlaveHostDetailData=a.data[o],e.SlaveHostName=a.data[o].slaveName,e.SlaveCount=a.data[o].count,e.SlaveExtensionInfo=a.data[o].extensionInfo,e.getUpdateMasterHostList(a.data[o].address))}))},getUpdateMasterHostList:function(t){var e=this;this.$q.loading.show(),i["p"]().then((function(a){console.log(a),e.masterHostList=a.data;for(var o=0;o<a.data.length;o++)a.data[o].address==t&&(console.log(a.data[o]),e.SlaveHostHostSelect=e.masterHostList[o].address,e.SlaveHostHostId=e.masterHostList[o].id,e.SlaveHostHostIndex=o);e.$q.loading.hide()}))},newUpdateCancel:function(){this.UpdateFixed=!1,this.SlaveHostDetailData="",this.SlaveHostName="",this.SlaveCount="",this.SlaveExtensionInfo="",this.SlaveHostHostSelect="",this.SlaveHostHostId="",this.SlaveHostHostIndex=-1},newUpdateCreate:function(){var t=this,e={ID:this.SlaveHostDetailData.id,HostID:this.SlaveHostHostId,TestCaseID:this.SlaveHostDetailData.testCaseID,SlaveName:this.SlaveHostName,Count:Number(this.SlaveCount),ExtensionInfo:this.SlaveExtensionInfo};this.SlaveHostName&&this.SlaveCount&&this.SlaveExtensionInfo?(this.$q.loading.show(),i["W"](e).then((function(e){console.log(e),t.getSlaveHostsList(),t.newUpdateCancel(),t.$q.notify({position:"top",message:"提示",caption:"保存成功",color:"secondary"})}))):this.$q.notify({position:"top",message:"提示",caption:"请填写完整信息",color:"red"})},lookSlaveLog:function(t){console.log(t),this.SLaveHostSelect=t.row,this.tab="tab0",this.lookLogFlag=!0;for(var e=0;e<t.row.count;e++)this.SlaveLogTextArr.push("");this.lookSalveIndexLog(0)},lookSalveIndexLog:function(t){var e=this;if(console.log(t,this.SlaveLogTextArr[t]),""==this.SlaveLogTextArr[t]){var a={caseId:this.$route.query.id,slaveHostId:this.SLaveHostSelect.id,idx:t};console.log(this.splitterModel),this.$q.loading.show(),i["v"](a).then((function(a){console.log(a),e.SlaveLogText=a.data,e.SlaveLogTextArr[t]=a.data,e.$q.loading.hide()}))}else this.SlaveLogText=this.SlaveLogTextArr[t]},cancelLookLog:function(){this.SLaveHostSelect="",this.lookLogFlag=!1,this.SlaveLogTextArr=[]}}},S=y,m=Object(u["a"])(S,f,g,!1,null,null,null),H=m.exports,C={name:"TestCaseDetail",components:{CreateShowTestCase:l["a"],History:h,SlaveHost:H},data:function(){return{isNoRun:0,timerOut:null,detailData:"",masterHostList:[],CopyTestCaseFixed:!1,CopyTestCaseName:"",CopyTestCaseSlaveFlag:"否",lookMasterLogFlag:!1,lookMasterLogText:""}},mounted:function(){this.getTestCaseDetail()},beforeDestroy:function(){clearInterval(this.timerOut),this.timerOut=null},methods:{getTestCaseDetail:function(){var t=this;this.$q.loading.show(),i["w"]({id:this.$route.query.id}).then((function(e){console.log(e),t.detailData=e.data,t.$refs.TestCaseSlaveHost.getSlaveHostsList(),t.$refs.TestCaseHistory.getHistoryList(),1==e.data.status?t.timerOut=window.setInterval((function(){setTimeout(t.getTestCaseStatus(),0)}),3e3):i["y"]({caseId:t.$route.query.id}).then((function(e){t.isNoRun=e.data,e.data||(clearInterval(t.timerOut),t.timerOut=null)}))}))},getTestCaseStatus:function(){var t=this;i["y"]({caseId:this.$route.query.id}).then((function(e){t.isNoRun=e.data,e.data||(clearInterval(t.timerOut),t.timerOut=null,t.getTestCaseDetail())}))},putTestCase:function(){var t=this;if(this.$refs.CSTestCase.newCreate()){var e=this.$refs.CSTestCase.newCreate();e.ID=this.detailData.id,this.$q.loading.show(),i["X"](e).then((function(e){console.log(e),t.$q.notify({position:"top",message:"提示",caption:"保存成功",color:"secondary"}),t.getTestCaseDetail()}))}},deleteTestCase:function(){var t=this;this.$q.dialog({title:"提示",message:"您确定要删除当前的测试用例吗",persistent:!0,ok:{push:!0,label:"确定"},cancel:{push:!0,label:"取消"}}).onOk((function(){if(t.$q.loading.show(),null==t.detailData.treeID){var e="?id=".concat(t.detailData.id);i["f"](e).then((function(e){console.log(e),"DirectoryTestCaseDetail"==t.$route.name?t.returnDirectory():t.$router.push({name:"TestCase"})}))}else{var a="?id=".concat(t.detailData.treeID);i["i"](a).then((function(e){console.log(e),"DirectoryTestCaseDetail"==t.$route.name?t.returnDirectory():t.$router.push({name:"TestCase"})}))}}))},run:function(){var t=this;this.$q.loading.show();var e={CaseId:this.$route.query.id,IsStop:!0};i["R"](e).then((function(e){console.log(e),t.getTestCaseDetail(),t.$q.notify({position:"top",message:"提示",caption:"运行成功",color:"secondary"})}))},stop:function(){var t=this;this.$q.loading.show();var e="?caseId=".concat(this.$route.query.id);i["S"](e).then((function(e){console.log(e),t.getTestCaseDetail(),t.$q.notify({position:"top",message:"提示",caption:"停止成功",color:"secondary"})}))},isSlaveHost:function(){var t=this;this.$q.loading.show(),i["u"]({caseId:this.$route.query.id}).then((function(e){0==e.data.length?(t.$q.notify({position:"top",message:"提示",caption:"当前测试用例下没有从主机，请添加从主机再进行运行。",color:"red"}),t.$q.loading.hide()):t.isHostPortRun()}))},isHostPortRun:function(){var t=this;if("Jmeter"!=this.detailData.engineType){var e=[this.$route.query.id],a={singleArray:e};i["P"](a).then((function(e){console.log(e),e.data[0].isAvailable?t.run():(t.$q.notify({position:"top",message:"提示",caption:"当前测试用例的主机端口号已被其他正在运行的测试用例使用。",color:"red"}),t.$q.loading.hide())}))}else this.run()},lookStatus:function(){console.log(this.detailData),this.$q.dialog({title:"提示",message:this.detailData.status?"当前测试用例正在运行":"当前测试用例为停止状态"})},lookMasterLog:function(){var t=this;this.$q.loading.show(),i["q"]({caseId:this.$route.query.id}).then((function(e){t.$q.loading.hide(),t.lookMasterLogFlag=!0,t.lookMasterLogText=e.data}))},lookMonitorUrl:function(){window.open(this.detailData.monitorUrl)},CopyTestCase:function(){this.CopyTestCaseFixed=!0,this.CopyTestCaseName=this.detailData.name+"_1"},CopyTestCaseCancel:function(){this.CopyTestCaseFixed=!1,this.CopyTestCaseName="",this.CopyTestCaseSlaveFlag="否"},CopyTestCaseCreate:function(){var t=this,e=this;this.$q.loading.show();var a={Name:this.CopyTestCaseName,Configuration:this.detailData.configuration,EngineType:this.detailData.engineType,MasterHostID:this.detailData.masterHostID};function o(t,a){if(console.log(a,e.SlaveHostList.length),a==e.SlaveHostList.length)e.$q.notify({position:"top",message:"提示",caption:"创建成功",color:"secondary"}),e.CopyTestCaseFixed=!1,e.$q.loading.hide(),e.$router.push({name:"TestCaseDetail",query:{id:t.data.id}}),e.getTestCaseDetail();else{console.log(e.SlaveHostList,a,e.SlaveHostList[a],e.SlaveHostList[a].HostID);var s={HostID:e.SlaveHostList[a].hostID,TestCaseID:t.data.id,SlaveName:e.SlaveHostList[a].slaveName,Count:e.SlaveHostList[a].count,ExtensionInfo:e.SlaveHostList[a].extensionInfo};i["J"](s).then((function(){o(t,a+1)}))}}i["K"](a).then((function(e){console.log(e),"是"==t.CopyTestCaseSlaveFlag&&0!=t.SlaveHostList.length?o(e,0):(t.$q.notify({position:"top",message:"提示",caption:"创建成功",color:"secondary"}),t.CopyTestCaseFixed=!1,t.$q.loading.hide(),t.$router.push({name:"TestCaseDetail",query:{id:e.data.id}})),t.getTestCaseDetail()}))},returnDirectory:function(){this.$router.push({path:"/Directory"})}}},b=C,q=(a("f1f3"),a("c309"),Object(u["a"])(b,o,s,!1,null,"6e8e0b31",null));e["default"]=q.exports},"8d00":function(t,e,a){},c309:function(t,e,a){"use strict";var o=a("8d00"),s=a.n(o);s.a},cc05:function(t,e,a){},f1f3:function(t,e,a){"use strict";var o=a("cc05"),s=a.n(o);s.a}}]);
//# sourceMappingURL=chunk-3c34e05c.6596dfea.js.map