(window["webpackJsonp"]=window["webpackJsonp"]||[]).push([["chunk-6e3bf917"],{"08d6":function(t,e,n){"use strict";var i=function(){var t=this,e=t.$createElement,n=t._self._c||e;return n("div",{staticClass:"TreeEntity"},[n("div",{staticClass:"q-pa-md"},[n("el-tree",{attrs:{data:t.simple,props:t.defaultProps,"highlight-current":!0,"expand-on-click-node":!1},on:{"node-expand":t.unfoldTree,"node-click":t.handleNodeClick}})],1)])},a=[],o=(n("4de4"),n("a434"),n("b0c0"),n("365c")),s={name:"TreeEntity",props:["existingDirectories"],data:function(){return{simple:[{id:null,label:"根目录",children:[]}],defaultProps:{children:"children",label:"label"},DisablesSelectedDirectories:[],SelectLocation:""}},mounted:function(){this.DisablesSelectedDirectories=this.existingDirectories||[],console.log(this.DisablesSelectedDirectories),this.getTreeEntityList()},methods:{getTreeEntityList:function(t,e){var n=this;this.$q.loading.show();var i={parentId:e||null,matchName:"",page:t||1,type:1,pageSize:100};o["E"](i).then((function(t){console.log(t);for(var e=t.data.results,i=0;i<t.data.results.length;i++)for(var a=0;a<n.DisablesSelectedDirectories.length;a++)n.DisablesSelectedDirectories[a].parentID!=t.data.results[i].id&&n.DisablesSelectedDirectories[a].id!=t.data.results[i].id||e.splice(i,1,"");e=e.filter((function(t){return t}));for(var o=0;o<e.length;o++)n.simple[0].children.push({id:e[o].id,label:e[o].name,parentID:e[o].parentID,type:e[o].type,value:e[o].value,children:[{}]});n.$q.loading.hide()}))},unfoldTree:function(t){var e=this;console.log(t),console.log(this.DisablesSelectedDirectories),this.$q.loading.show();var n={parentId:t.id,matchName:"",page:1,type:1,pageSize:100};o["E"](n).then((function(n){console.log(n);for(var i=n.data.results,a=0;a<n.data.results.length;a++){console.log(n.data.results);for(var o=0;o<e.DisablesSelectedDirectories.length;o++)if(e.DisablesSelectedDirectories[o].parentID==n.data.results[a].id||e.DisablesSelectedDirectories[o].id==n.data.results[a].id){i[a]="";break}}i=i.filter((function(t){return t})),t.children=[];for(var s=0;s<i.length;s++)t.children||e.$set(t,"children",[{}]),t.children.push({id:i[s].id,label:i[s].name,parentID:i[s].parentID,type:i[s].type,value:i[s].value,children:[{}]});e.$q.loading.hide()}))},handleNodeClick:function(t){console.log(t),this.$emit("getDirectoryLocation",t),this.SelectLocation=t},getDirectoryLocation:function(){return this.SelectLocation}}},r=s,c=n("2877"),l=Object(c["a"])(r,i,a,!1,null,"062abec0",null);e["a"]=l.exports},"10b4":function(t,e,n){"use strict";var i=function(){var t=this,e=t.$createElement,n=t._self._c||e;return n("div",[n("q-dialog",{attrs:{persistent:""},model:{value:t.ChangeFileDirectoryFlag,callback:function(e){t.ChangeFileDirectoryFlag=e},expression:"ChangeFileDirectoryFlag"}},[n("q-card",{staticStyle:{width:"100%"}},[n("q-card-section",[n("div",{staticClass:"text-h6"},[t._v("选择文件目录位置")])]),n("q-separator"),n("TreeEntity",{ref:"TreeEntity"}),n("q-separator"),n("q-card-actions",{attrs:{align:"right"}},[n("q-btn",{attrs:{flat:"",label:"取消",color:"primary"},on:{click:function(e){t.ChangeFileDirectoryFlag=!1}}}),n("q-btn",{attrs:{flat:"",label:"确定",color:"primary"},on:{click:t.SelectDirectoryLocation}})],1)],1)],1),n("lookUp",{ref:"lookUp",attrs:{masterHostList:t.masterHostList,masterSelectIndex:t.masterHostIndex,fixed:t.HostFixed},on:{addMasterHost:t.addMasterHost,cancelMasterHost:t.cancelMasterHost}}),n("div",{staticClass:"new_input"},[n("div",{staticClass:"row input_row"},[n("q-input",{staticClass:"col",attrs:{dense:!1},scopedSlots:t._u([{key:"before",fn:function(){return[n("span",{staticStyle:{"font-size":"14px"}},[t._v("名称:")])]},proxy:!0}]),model:{value:t.Name,callback:function(e){t.Name=e},expression:"Name"}}),n("q-select",{staticClass:"col",attrs:{options:["Http","Tcp","WebSocket"],dense:!1},scopedSlots:t._u([{key:"before",fn:function(){return[n("span",{staticStyle:{"font-size":"14px"}},[t._v("引擎类型:")])]},proxy:!0},{key:"prepend",fn:function(){},proxy:!0}]),model:{value:t.EngineType,callback:function(e){t.EngineType=e},expression:"EngineType"}})],1),n("div",{staticClass:"row input_row"},[n("q-input",{staticClass:"col",attrs:{dense:!1,readonly:"",placeholder:"点击右侧加号选择主机"},scopedSlots:t._u([{key:"before",fn:function(){return[n("span",{staticStyle:{"font-size":"14px"}},[t._v("主机:")])]},proxy:!0},{key:"append",fn:function(){return[n("q-btn",{attrs:{round:"",dense:"",flat:"",icon:"add"},on:{click:t.masterHost}})]},proxy:!0}]),model:{value:t.masterHostSelect,callback:function(e){t.masterHostSelect=e},expression:"masterHostSelect"}}),n("q-input",{staticClass:"col",attrs:{dense:!1,placeholder:"值范围(15557~25557) 默认15557"},on:{input:function(e){return t.ifRegular("LocustMasterBindPort",t.paraConfig.LocustMasterBindPort)}},scopedSlots:t._u([{key:"before",fn:function(){return[n("span",{staticStyle:{"font-size":"14px"}},[t._v("主机端口:")])]},proxy:!0}]),model:{value:t.paraConfig.LocustMasterBindPort,callback:function(e){t.$set(t.paraConfig,"LocustMasterBindPort",e)},expression:"paraConfig.LocustMasterBindPort"}})],1),n("div",{staticClass:"row input_row"},[n("q-input",{staticClass:"col",attrs:{dense:!1,readonly:"",placeholder:"点击右侧加号选择文件目录位置"},scopedSlots:t._u([{key:"before",fn:function(){return[n("span",{staticStyle:{"font-size":"14px"}},[t._v("文件目录位置:")])]},proxy:!0},{key:"append",fn:function(){return[n("q-btn",{attrs:{round:"",dense:"",flat:"",icon:"add"},on:{click:t.ChangeFileDirectory}})]},proxy:!0}]),model:{value:t.ChangeFileDirectoryName,callback:function(e){t.ChangeFileDirectoryName=e},expression:"ChangeFileDirectoryName"}})],1),n("span",{staticStyle:{"font-size":"14px"}},[t._v("参数配置:")]),n("div",{staticClass:"row",staticStyle:{"margin-bottom":"10px"}},[n("q-input",{staticClass:"col",attrs:{maxlength:"6",type:"text",filled:"","bottom-slots":"",dense:!0},on:{input:function(e){return t.ifRegular("UserCount",t.paraConfig.UserCount)}},scopedSlots:t._u([{key:"before",fn:function(){return[n("span",{staticStyle:{"font-size":"14px",width:"100px"}},[t._v("压测用户总数:")])]},proxy:!0},{key:"append",fn:function(){return[n("span",{staticStyle:{"font-size":"14px"}},[t._v("个")])]},proxy:!0}]),model:{value:t.paraConfig.UserCount,callback:function(e){t.$set(t.paraConfig,"UserCount",e)},expression:"paraConfig.UserCount"}}),n("q-input",{staticClass:"col",attrs:{maxlength:"6",filled:"","bottom-slots":"",dense:!0},on:{input:function(e){return t.ifRegular("PerSecondUserCount",t.paraConfig.PerSecondUserCount)}},scopedSlots:t._u([{key:"before",fn:function(){return[n("span",{staticStyle:{"font-size":"14px",width:"105px","margin-left":"10px"}},[t._v("每秒加载用户数:")])]},proxy:!0},{key:"append",fn:function(){return[n("span",{staticStyle:{"font-size":"14px"}},[t._v("个/秒")])]},proxy:!0}]),model:{value:t.paraConfig.PerSecondUserCount,callback:function(e){t.$set(t.paraConfig,"PerSecondUserCount",e)},expression:"paraConfig.PerSecondUserCount"}}),n("q-input",{staticClass:"col",attrs:{maxlength:"6",filled:"","bottom-slots":"",dense:!0},on:{input:function(e){return t.ifRegular("Duration",t.paraConfig.Duration)}},scopedSlots:t._u([{key:"before",fn:function(){return[n("span",{staticStyle:{"font-size":"14px",width:"100px","margin-left":"10px"}},[t._v("压测时间:")])]},proxy:!0},{key:"append",fn:function(){return[n("span",{staticStyle:{"font-size":"14px"}},[t._v("秒")])]},proxy:!0}]),model:{value:t.paraConfig.Duration,callback:function(e){t.$set(t.paraConfig,"Duration",e)},expression:"paraConfig.Duration"}})],1),n("div",{staticClass:"row",staticStyle:{"margin-bottom":"10px"}},[n("q-input",{staticClass:"col",attrs:{filled:"","bottom-slots":"",dense:!0,placeholder:"请输入被测服务器地址"},scopedSlots:t._u([{key:"before",fn:function(){return[n("span",{staticStyle:{"font-size":"14px",width:"100px"}},[t._v("被测服务器:")])]},proxy:!0},{key:"append",fn:function(){return[n("span",{staticStyle:{"font-size":"14px"}},[t._v("地址")])]},proxy:!0}]),model:{value:t.paraConfig.Address,callback:function(e){t.$set(t.paraConfig,"Address",e)},expression:"paraConfig.Address"}}),n("q-input",{staticClass:"col",attrs:{maxlength:"5",filled:"","bottom-slots":"",dense:!0,placeholder:"范围0-65535之间"},on:{input:function(e){return t.ifRegular("Port",t.paraConfig.Port)}},scopedSlots:t._u([{key:"before",fn:function(){return[n("span",{staticStyle:{"font-size":"14px",width:"105px","margin-left":"10px"}},[t._v("被测服务器端口:")])]},proxy:!0},{key:"append",fn:function(){return[n("span",{staticStyle:{"font-size":"14px"}},[t._v("端口")])]},proxy:!0}]),model:{value:t.paraConfig.Port,callback:function(e){t.$set(t.paraConfig,"Port",e)},expression:"paraConfig.Port"}}),n("q-input",{staticClass:"col",attrs:{filled:"","bottom-slots":"",dense:!0},scopedSlots:t._u([{key:"before",fn:function(){return[n("span",{staticStyle:{"font-size":"14px",width:"105px","margin-left":"10px"}},[t._v("结束分隔符:")])]},proxy:!0}]),model:{value:t.paraConfig.ResponseSeparator,callback:function(e){t.$set(t.paraConfig,"ResponseSeparator",e)},expression:"paraConfig.ResponseSeparator"}})],1),n("div",{staticClass:"row",staticStyle:{"margin-bottom":"10px"}},[n("q-select",{staticClass:"col-4",attrs:{options:t.PrintLogOptions,"emit-value":"","map-options":"",dense:!1},scopedSlots:t._u([{key:"before",fn:function(){return[n("span",{staticStyle:{"font-size":"14px"}},[t._v("是否打印日志:")])]},proxy:!0},{key:"prepend",fn:function(){},proxy:!0}]),model:{value:t.paraConfig.IsPrintLog,callback:function(e){t.$set(t.paraConfig,"IsPrintLog",e)},expression:"paraConfig.IsPrintLog"}}),n("q-select",{staticClass:"col-4",staticStyle:{"margin-left":"10px"},attrs:{options:t.SyncTypeOptions,"emit-value":"","map-options":"",dense:!1},scopedSlots:t._u([{key:"before",fn:function(){return[n("span",{staticStyle:{"font-size":"14px"}},[t._v("同步类型:")])]},proxy:!0},{key:"prepend",fn:function(){},proxy:!0}]),model:{value:t.paraConfig.SyncType,callback:function(e){t.$set(t.paraConfig,"SyncType",e)},expression:"paraConfig.SyncType"}})],1),n("q-list",{staticClass:"rounded-borders",attrs:{bordered:""}},[n("q-expansion-item",{staticStyle:{"text-align":"left",position:"relative"},attrs:{label:"数据源","expand-icon-toggle":"","expand-separator":""},scopedSlots:t._u([{key:"header",fn:function(){return[n("q-item-section",[t._v(" 数据源 ")]),n("q-item-section",{attrs:{side:""}},[n("div",{staticClass:"row items-center"},[n("q-btn",{staticClass:"btn ",staticStyle:{margin:"0px 0px 0px 20px"},attrs:{color:"primary",label:"添加数据源参数"},on:{click:function(e){return t.addDataVars("DataSource")}}})],1)])]},proxy:!0}]),model:{value:t.DataSourceExpanded,callback:function(e){t.DataSourceExpanded=e},expression:"DataSourceExpanded"}},[n("q-card",[n("q-card-section",{directives:[{name:"show",rawName:"v-show",value:0==t.paraConfig.DataSourceVars.length,expression:"paraConfig.DataSourceVars.length==0"}]},[t._v(" 暂无参数配置，请点击添加数据源参数按钮进行添加。 ")]),n("transition-group",{attrs:{name:"MoveList"}},t._l(t.paraConfig.DataSourceVars,(function(e,i){return n("q-card-section",{key:i},[n("span",{staticStyle:{"font-size":"14px"}},[t._v("参数"+t._s(i+1)+":")]),n("div",{staticClass:"row"},[n("q-input",{staticClass:"col-5",attrs:{filled:"",dense:!0},scopedSlots:t._u([{key:"before",fn:function(){return[n("span",{staticStyle:{"font-size":"14px",width:"100px"}},[t._v("名称:")])]},proxy:!0}],null,!0),model:{value:t.paraConfig.DataSourceVars[i].Name,callback:function(e){t.$set(t.paraConfig.DataSourceVars[i],"Name",e)},expression:"paraConfig.DataSourceVars[ind].Name"}}),n("q-select",{staticClass:"col-5",attrs:{options:t.dataSourceName,dense:!0},scopedSlots:t._u([{key:"before",fn:function(){return[n("span",{staticStyle:{"font-size":"14px","margin-left":"10px",width:"80px"}},[t._v("数据源名称:")])]},proxy:!0},{key:"prepend",fn:function(){},proxy:!0}],null,!0),model:{value:t.paraConfig.DataSourceVars[i].DataSourceName,callback:function(e){t.$set(t.paraConfig.DataSourceVars[i],"DataSourceName",e)},expression:"paraConfig.DataSourceVars[ind].DataSourceName"}}),n("div",{staticClass:"col-2 row"},[n("div",{staticClass:"col-1",staticStyle:{"margin-left":"20px"}},[n("q-icon",{staticClass:"pointer",staticStyle:{display:"block"},attrs:{name:"ion-arrow-up"},on:{click:function(e){return t.moveUpList("DataSourceVars",i)}}}),n("q-icon",{staticClass:"pointer",staticStyle:{display:"block","margin-top":"10px"},attrs:{name:"ion-arrow-down"},on:{click:function(e){return t.moveDownList("DataSourceVars",i)}}})],1),n("q-btn",{staticClass:"btn col-4",staticStyle:{background:"#FF0000",color:"white","margin-left":"20px",display:"inline-block"},attrs:{label:"删 除"},on:{click:function(e){return t.deleteDataVars("DataSource",i)}}})],1)],1)])})),1),n("q-btn",{staticClass:"btn ",staticStyle:{margin:"0px 0px 20px 20px"},attrs:{color:"primary",label:"添加数据源参数"},on:{click:function(e){return t.addDataVars("DataSource")}}})],1)],1),n("q-expansion-item",{staticStyle:{"text-align":"left",position:"relative"},attrs:{label:"连接初始化","expand-icon-toggle":"","expand-separator":""},scopedSlots:t._u([{key:"header",fn:function(){return[n("q-item-section",[t._v(" 连接初始化 ")]),n("q-item-section",{attrs:{side:""}},[n("div",{staticClass:"row items-center"},[n("q-btn",{staticClass:"btn ",staticStyle:{margin:"0px 0px 0px 20px"},attrs:{color:"primary",label:"添加初始化参数"},on:{click:function(e){return t.addDataVars("ConnectInit")}}})],1)])]},proxy:!0}]),model:{value:t.ConnectInitExpanded,callback:function(e){t.ConnectInitExpanded=e},expression:"ConnectInitExpanded"}},[n("q-card",[n("q-card-section",{directives:[{name:"show",rawName:"v-show",value:0==t.paraConfig.ConnectInit.VarSettings.length,expression:"paraConfig.ConnectInit.VarSettings.length==0"}]},[t._v("暂无参数配置，请点击添加初始化参数按钮进行添加。")]),n("transition-group",{attrs:{name:"MoveList"}},t._l(t.paraConfig.ConnectInit.VarSettings,(function(e,i){return n("q-card-section",{key:i},[n("span",{staticStyle:{"font-size":"14px"}},[t._v("参数"+t._s(i+1)+":")]),n("div",{staticClass:"row"},[n("q-input",{staticClass:"col-5",attrs:{filled:"",dense:!0},scopedSlots:t._u([{key:"before",fn:function(){return[n("span",{staticStyle:{"font-size":"14px",width:"100px"}},[t._v("名称:")])]},proxy:!0}],null,!0),model:{value:t.paraConfig.ConnectInit.VarSettings[i].Name,callback:function(e){t.$set(t.paraConfig.ConnectInit.VarSettings[i],"Name",e)},expression:"paraConfig.ConnectInit.VarSettings[ind].Name"}}),n("q-input",{staticClass:"col-5",attrs:{filled:"",dense:!0},scopedSlots:t._u([{key:"before",fn:function(){return[n("span",{staticStyle:{"font-size":"14px",width:"80px","margin-left":"10px"}},[t._v("内容:")])]},proxy:!0}],null,!0),model:{value:t.paraConfig.ConnectInit.VarSettings[i].Content,callback:function(e){t.$set(t.paraConfig.ConnectInit.VarSettings[i],"Content",e)},expression:"paraConfig.ConnectInit.VarSettings[ind].Content"}}),n("div",{staticClass:"col-2 row"},[n("div",{staticClass:"col-1",staticStyle:{"margin-left":"20px"}},[n("q-icon",{staticClass:"pointer",staticStyle:{display:"block"},attrs:{name:"ion-arrow-up"},on:{click:function(e){return t.moveUpList("ConnectInit",i)}}}),n("q-icon",{staticClass:"pointer",staticStyle:{display:"block","margin-top":"10px"},attrs:{name:"ion-arrow-down"},on:{click:function(e){return t.moveDownList("ConnectInit",i)}}})],1),n("q-btn",{staticClass:"btn col-4",staticStyle:{background:"#FF0000",color:"white","margin-left":"20px",display:"inline-block"},attrs:{label:"删 除"},on:{click:function(e){return t.deleteDataVars("ConnectInit",i)}}})],1)],1)])})),1),n("q-btn",{staticClass:"btn ",staticStyle:{margin:"0px 0px 20px 0px"},attrs:{color:"primary",label:"添加初始化参数"},on:{click:function(e){return t.addDataVars("ConnectInit")}}})],1)],1),n("q-expansion-item",{staticStyle:{"text-align":"left",position:"relative"},attrs:{label:"发送初始化","expand-icon-toggle":"","expand-separator":""},scopedSlots:t._u([{key:"header",fn:function(){return[n("q-item-section",[t._v(" 发送初始化 ")]),n("q-item-section",{attrs:{side:""}},[n("div",{staticClass:"row items-center"},[n("q-btn",{staticClass:"btn ",staticStyle:{margin:"0px 0px 0px 20px"},attrs:{color:"primary",label:"添加初始化参数"},on:{click:function(e){return t.addDataVars("SendInit")}}})],1)])]},proxy:!0}]),model:{value:t.SendInitExpanded,callback:function(e){t.SendInitExpanded=e},expression:"SendInitExpanded"}},[n("q-card",[n("q-card-section",{directives:[{name:"show",rawName:"v-show",value:0==t.paraConfig.SendInit.VarSettings.length,expression:"paraConfig.SendInit.VarSettings.length==0"}]},[t._v("暂无参数配置，请点击添加初始化参数按钮进行添加。")]),n("transition-group",{attrs:{name:"MoveList"}},t._l(t.paraConfig.SendInit.VarSettings,(function(e,i){return n("q-card-section",{key:i},[n("span",{staticStyle:{"font-size":"14px"}},[t._v("参数"+t._s(i+1)+":")]),n("div",{staticClass:"row"},[n("q-input",{staticClass:"col-5",attrs:{filled:"",dense:!0},scopedSlots:t._u([{key:"before",fn:function(){return[n("span",{staticStyle:{"font-size":"14px",width:"100px"}},[t._v("名称:")])]},proxy:!0}],null,!0),model:{value:t.paraConfig.SendInit.VarSettings[i].Name,callback:function(e){t.$set(t.paraConfig.SendInit.VarSettings[i],"Name",e)},expression:"paraConfig.SendInit.VarSettings[ind].Name"}}),n("q-input",{staticClass:"col-5",attrs:{filled:"",dense:!0},scopedSlots:t._u([{key:"before",fn:function(){return[n("span",{staticStyle:{"font-size":"14px",width:"80px","margin-left":"10px"}},[t._v("内容:")])]},proxy:!0}],null,!0),model:{value:t.paraConfig.SendInit.VarSettings[i].Content,callback:function(e){t.$set(t.paraConfig.SendInit.VarSettings[i],"Content",e)},expression:"paraConfig.SendInit.VarSettings[ind].Content"}}),n("div",{staticClass:"col-2 row"},[n("div",{staticClass:"col-1",staticStyle:{"margin-left":"20px"}},[n("q-icon",{staticClass:"pointer",staticStyle:{display:"block"},attrs:{name:"ion-arrow-up"},on:{click:function(e){return t.moveUpList("SendInit",i)}}}),n("q-icon",{staticClass:"pointer",staticStyle:{display:"block","margin-top":"10px"},attrs:{name:"ion-arrow-down"},on:{click:function(e){return t.moveDownList("SendInit",i)}}})],1),n("q-btn",{staticClass:"btn col-4",staticStyle:{background:"#FF0000",color:"white","margin-left":"20px",display:"inline-block"},attrs:{label:"删 除"},on:{click:function(e){return t.deleteDataVars("SendInit",i)}}})],1)],1)])})),1),n("q-btn",{staticClass:"btn ",staticStyle:{margin:"0px 0px 20px 0px"},attrs:{color:"primary",label:"添加初始化参数"},on:{click:function(e){return t.addDataVars("SendInit")}}})],1)],1),n("q-expansion-item",{staticStyle:{"text-align":"left",position:"relative"},attrs:{label:"停止初始化","expand-icon-toggle":"","expand-separator":""},scopedSlots:t._u([{key:"header",fn:function(){return[n("q-item-section",[t._v(" 停止初始化 ")]),n("q-item-section",{attrs:{side:""}},[n("div",{staticClass:"row items-center"},[n("q-btn",{staticClass:"btn ",staticStyle:{margin:"0px 0px 0px 20px"},attrs:{color:"primary",label:"添加初始化参数"},on:{click:function(e){return t.addDataVars("StopInit")}}})],1)])]},proxy:!0}]),model:{value:t.StopInitExpanded,callback:function(e){t.StopInitExpanded=e},expression:"StopInitExpanded"}},[n("q-card",[n("q-card-section",{directives:[{name:"show",rawName:"v-show",value:0==t.paraConfig.StopInit.VarSettings.length,expression:"paraConfig.StopInit.VarSettings.length==0"}]},[t._v("暂无参数配置，请点击添加初始化参数按钮进行添加。")]),n("transition-group",{attrs:{name:"MoveList"}},t._l(t.paraConfig.StopInit.VarSettings,(function(e,i){return n("q-card-section",{key:i},[n("span",{staticStyle:{"font-size":"14px"}},[t._v("参数"+t._s(i+1)+":")]),n("div",{staticClass:"row"},[n("q-input",{staticClass:"col-5",attrs:{filled:"",dense:!0},scopedSlots:t._u([{key:"before",fn:function(){return[n("span",{staticStyle:{"font-size":"14px",width:"100px"}},[t._v("名称:")])]},proxy:!0}],null,!0),model:{value:t.paraConfig.StopInit.VarSettings[i].Name,callback:function(e){t.$set(t.paraConfig.StopInit.VarSettings[i],"Name",e)},expression:"paraConfig.StopInit.VarSettings[ind].Name"}}),n("q-input",{staticClass:"col-5",attrs:{filled:"",dense:!0},scopedSlots:t._u([{key:"before",fn:function(){return[n("span",{staticStyle:{"font-size":"14px",width:"80px","margin-left":"10px"}},[t._v("内容:")])]},proxy:!0}],null,!0),model:{value:t.paraConfig.StopInit.VarSettings[i].Content,callback:function(e){t.$set(t.paraConfig.StopInit.VarSettings[i],"Content",e)},expression:"paraConfig.StopInit.VarSettings[ind].Content"}}),n("div",{staticClass:"col-2 row"},[n("div",{staticClass:"col-1",staticStyle:{"margin-left":"20px"}},[n("q-icon",{staticClass:"pointer",staticStyle:{display:"block"},attrs:{name:"ion-arrow-up"},on:{click:function(e){return t.moveUpList("StopInit",i)}}}),n("q-icon",{staticClass:"pointer",staticStyle:{display:"block","margin-top":"10px"},attrs:{name:"ion-arrow-down"},on:{click:function(e){return t.moveDownList("StopInit",i)}}})],1),n("q-btn",{staticClass:"btn col-4",staticStyle:{background:"#FF0000",color:"white","margin-left":"20px",display:"inline-block"},attrs:{label:"删 除"},on:{click:function(e){return t.deleteDataVars("StopInit",i)}}})],1)],1)])})),1),n("q-btn",{staticClass:"btn ",staticStyle:{margin:"0px 0px 20px 0px"},attrs:{color:"primary",label:"添加初始化参数"},on:{click:function(e){return t.addDataVars("StopInit")}}})],1)],1)],1),n("q-list",{attrs:{bordered:""}},[n("q-expansion-item",{staticStyle:{"text-align":"left",position:"relative"},attrs:{label:"数据源","expand-icon-toggle":"","expand-separator":""},scopedSlots:t._u([{key:"header",fn:function(){return[n("q-item-section",[t._v(" 配置文本: ")]),n("q-item-section",{attrs:{side:""}},[n("q-btn",{staticClass:"btn",staticStyle:{margin:"10px 0 10px 20px",float:"right"},attrs:{color:"primary",label:"生 成"},on:{click:t.CreateJson}})],1)]},proxy:!0}]),model:{value:t.ConfigTextExpanded,callback:function(e){t.ConfigTextExpanded=e},expression:"ConfigTextExpanded"}},[n("q-card",[n("div",{staticClass:"row input_row"},[n("q-input",{staticClass:"col-12",staticStyle:{overflow:"hidden"},attrs:{dense:!1,autogrow:"",type:"textarea",outlined:""},model:{value:t.Configuration,callback:function(e){t.Configuration=e},expression:"Configuration"}})],1)])],1)],1)],1)],1)},a=[],o=(n("a15b"),n("a434"),n("b0c0"),n("a9e3"),n("ac1f"),n("5319"),n("498a"),n("53ca")),s=n("365c"),r=n("7729"),c=n("08d6"),l={name:"CreateShowTestCase",props:["detailData","currentDirectory"],components:{lookUp:r["a"],TreeEntity:c["a"]},watch:{detailData:function(t){if(t){console.log(t);var e=JSON.parse(t.configuration);this.paraConfig={UserCount:e.UserCount||"",PerSecondUserCount:e.PerSecondUserCount||"",Address:e.Address||"",Port:e.Port||"",Duration:e.Duration||"",ResponseSeparator:e.ResponseSeparator||"",DataSourceVars:e.DataSourceVars||[],LocustMasterBindPort:e.LocustMasterBindPort||15557,IsPrintLog:1==e.IsPrintLog?"是":"否",SyncType:0==e.SyncType?"异步模式":"同步模式",ConnectInit:e.ConnectInit||{VarSettings:[]},SendInit:e.SendInit||{VarSettings:[]},StopInit:e.StopInit||{VarSettings:[]}},this.Name=t.name,this.EngineType=t.engineType,this.masterHostSelect=t.masterHostAddress,this.MasterHostID=t.masterHostID,t.treeID?this.getTreeEntityTreePath(t.treeID,!0):this.ChangeFileDirectoryName=""!=t.parentName?t.parentName:"根目录",this.ChangeFileDirectoryId=t.treeID,this.Configuration=JSON.stringify(JSON.parse(t.configuration),null,2)}}},mounted:function(){this.currentDirectory?(this.getTreeEntityTreePath(this.currentDirectory.id),this.ChangeFileDirectoryId=this.currentDirectory.id):this.ChangeFileDirectoryName="根目录",console.log(this.currentDirectory),this.getDataSourceName()},data:function(){return{dataSourceName:[],HostFixed:!1,masterHostSelect:"",masterHostIndex:-1,masterHostList:[],Name:"",Configuration:"",EngineType:"",FolderID:null,MasterHostID:"",paraConfig:{UserCount:"",PerSecondUserCount:"",Address:"",Port:"",Duration:"",ResponseSeparator:"",DataSourceVars:[],LocustMasterBindPort:"",IsPrintLog:"否",SyncType:"同步模式",ConnectInit:{VarSettings:[]},SendInit:{VarSettings:[]},StopInit:{VarSettings:[]}},PrintLogOptions:[{label:"是",value:!0},{label:"否",value:!1}],SyncTypeOptions:[{label:"同步模式",value:!0},{label:"异步模式",value:!1}],DataSourceExpanded:!1,ConnectInitExpanded:!1,SendInitExpanded:!1,StopInitExpanded:!1,ConfigTextExpanded:!1,ChangeFileDirectoryFlag:!1,ChangeFileDirectoryName:"",ChangeFileDirectoryId:null}},methods:{getDataSourceName:function(){var t=this,e={};s["k"](e).then((function(e){console.log(e);for(var n=0;n<e.data.length;n++)t.dataSourceName.push(e.data[n].name);t.$q.loading.hide()}))},newCancel:function(){this.Name="",this.Configuration="",this.EngineType="",this.MasterHostID="",this.masterHostSelect="",this.$refs.lookUp.selectIndex=-1,this.paraConfig={UserCount:"",PerSecondUserCount:"",Address:"",Port:"",Duration:"",ResponseSeparator:"",DataSourceVars:[],ConnectInit:{VarSettings:[]},SendInit:{VarSettings:[]},StopInit:{VarSettings:[]}}},newCreate:function(){var t={Name:this.Name,Configuration:this.Configuration.trim(),EngineType:this.EngineType,MasterHostID:this.MasterHostID,FolderID:this.ChangeFileDirectoryId};return console.log(t),this.Name&&this.isJSON(this.Configuration)&&this.EngineType&&this.MasterHostID?t:(""==this.Name?this.$q.notify({position:"top",message:"提示",caption:"请填写名称",color:"red"}):""==this.EngineType?this.$q.notify({position:"top",message:"提示",caption:"请填选择引擎类型",color:"red"}):""==this.MasterHostID&&this.$q.notify({position:"top",message:"提示",caption:"请选择主机",color:"red"}),!1)},masterHost:function(){this.HostFixed=!0,this.$q.loading.show(),this.getMasterHostList()},getMasterHostList:function(){var t=this;s["q"]().then((function(e){console.log(e),t.masterHostList=e.data,t.$q.loading.hide()}))},addMasterHost:function(t){if(void 0==t)return!1;this.masterHostSelect=this.masterHostList[t].address,this.MasterHostID=this.masterHostList[t].id,this.masterHostIndex=t,this.createFixed=!0,this.HostFixed=!1},cancelMasterHost:function(){this.createFixed=!0,this.HostFixed=!1,this.$refs.lookUp.selectIndex=this.masterHostIndex},CreateJson:function(){if(""==this.Configuration.trim()){if(!this.isPort(this.paraConfig.Port))return;if(!this.isMasterPort(this.paraConfig.LocustMasterBindPort))return;if(!this.ifDataVars())return;this.Configuration=JSON.stringify({UserCount:this.paraConfig.UserCount?Number(this.paraConfig.UserCount):"",PerSecondUserCount:this.paraConfig.PerSecondUserCount?Number(this.paraConfig.PerSecondUserCount):"",Address:this.paraConfig.Address,Port:this.paraConfig.Port?Number(this.paraConfig.Port):"",Duration:this.paraConfig.Duration?Number(this.paraConfig.Duration):"",ResponseSeparator:this.paraConfig.ResponseSeparator,DataSourceVars:this.paraConfig.DataSourceVars,LocustMasterBindPort:Number(this.paraConfig.LocustMasterBindPort)||15557,IsPrintLog:1==this.paraConfig.IsPrintLog||"是"==this.paraConfig.IsPrintLog,SyncType:1==this.paraConfig.SyncType||"同步模式"==this.paraConfig.SyncType,ConnectInit:{VarSettings:this.paraConfig.ConnectInit.VarSettings},SendInit:{VarSettings:this.paraConfig.SendInit.VarSettings},StopInit:{VarSettings:this.paraConfig.StopInit.VarSettings}},null,2),this.paraConfig.LocustMasterBindPort=JSON.parse(this.Configuration).LocustMasterBindPort,this.ConfigTextExpanded=!0}else if(this.isJSON(this.Configuration.trim())){if(!this.isPort(this.paraConfig.Port))return;if(!this.isMasterPort(this.paraConfig.LocustMasterBindPort))return;if(!this.ifDataVars())return;this.Configuration=JSON.parse(this.Configuration),this.Configuration.UserCount=this.paraConfig.UserCount?Number(this.paraConfig.UserCount):"",this.Configuration.PerSecondUserCount=this.paraConfig.PerSecondUserCount?Number(this.paraConfig.PerSecondUserCount):"",this.Configuration.Address=this.paraConfig.Address,this.Configuration.Port=this.paraConfig.Port?Number(this.paraConfig.Port):"",this.Configuration.Duration=this.paraConfig.Duration?Number(this.paraConfig.Duration):"",this.Configuration.ResponseSeparator=this.paraConfig.ResponseSeparator,this.Configuration.DataSourceVars=this.paraConfig.DataSourceVars,this.Configuration.LocustMasterBindPort=Number(this.paraConfig.LocustMasterBindPort)||15557,this.Configuration.IsPrintLog=1==this.paraConfig.IsPrintLog||"是"==this.paraConfig.IsPrintLog,this.Configuration.SyncType=1==this.paraConfig.SyncType||"同步模式"==this.paraConfig.SyncType,this.Configuration.ConnectInit.VarSettings=this.paraConfig.ConnectInit.VarSettings,this.Configuration.SendInit.VarSettings=this.paraConfig.SendInit.VarSettings,this.Configuration.StopInit.VarSettings=this.paraConfig.StopInit.VarSettings,this.Configuration=JSON.stringify(this.Configuration,null,2),this.ConfigTextExpanded=!0}},isJSON:function(t){if("string"==typeof t)try{var e=JSON.parse(t);if("object"==Object(o["a"])(e)&&e){if("{"==t.substr(0,1)&&"}"==t.substr(-1))return!0;this.$q.notify({position:"top",message:"提示",caption:"配置不是正确的JSON格式",color:"red"})}else this.$q.notify({position:"top",message:"提示",caption:"配置不是正确的JSON格式",color:"red"})}catch(n){this.$q.notify({position:"top",message:"提示",caption:"配置不是正确的JSON格式",color:"red"})}},isValidIp:function(t){return console.log(t),""==t||(!!/^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$/.test(t)||(this.$q.notify({position:"top",message:"提示",caption:"被测服务器ip地址不正确",color:"red"}),!1))},isPort:function(t){return""==t||(!!(/^[1-9]\d*$/.test(t)&&1<=1*t&&1*t<=65535)||(this.$q.notify({position:"top",message:"提示",caption:"被测服务器端口不符合范围：0-65535",color:"red"}),!1))},isMasterPort:function(t){return""==t||(!!(/^[1-9]\d*$/.test(t)&&15557<=1*t&&1*t<=25557)||(this.$q.notify({position:"top",message:"提示",caption:"主机端口不符合范围：15557-25557",color:"red"}),!1))},ifRegular:function(t,e){var n=this;this.$nextTick((function(){if("Port"==t){var i=e.replace(/[^\d]/g,"");n.$set(n.paraConfig,t,i>65535?65535:i)}else if("LocustMasterBindPort"==t){var a=e.replace(/[^\d]/g,"");n.$set(n.paraConfig,t,a)}else n.$set(n.paraConfig,t,e.replace(/[^\d]/g,"").replace(/^0/g,""))}))},ifDataVars:function(){for(var t=0;t<this.paraConfig.DataSourceVars.length;t++){var e=this.paraConfig.DataSourceVars[t];if(console.log(e),""==e.Name||""==e.DataSourceName)return this.$q.notify({position:"top",message:"提示",caption:"数据源参数".concat(t+1,"名称和数据源名称为必填"),color:"red"}),!1}for(var n=0;n<this.paraConfig.ConnectInit.VarSettings.length;n++){var i=this.paraConfig.ConnectInit.VarSettings[n];if(console.log(i),""==i.Content)return this.$q.notify({position:"top",message:"提示",caption:"连接初始化参数".concat(n+1,"内容为必填"),color:"red"}),!1}for(var a=0;a<this.paraConfig.SendInit.VarSettings.length;a++){var o=this.paraConfig.SendInit.VarSettings[a];if(console.log(o),""==o.Content)return this.$q.notify({position:"top",message:"提示",caption:"发送初始化参数".concat(a+1,"内容为必填"),color:"red"}),!1}for(var s=0;s<this.paraConfig.StopInit.VarSettings.length;s++){var r=this.paraConfig.StopInit.VarSettings[s];if(console.log(r),""==r.Content)return this.$q.notify({position:"top",message:"提示",caption:"停止初始化参数".concat(s+1,"内容为必填"),color:"red"}),!1}return!0},addDataVars:function(t){"DataSource"==t?(this.paraConfig.DataSourceVars.push({Name:"",Type:"",DataSourceName:"",Data:""}),this.DataSourceExpanded=!0):"ConnectInit"==t?(this.paraConfig.ConnectInit.VarSettings.push({Name:"",Content:""}),this.ConnectInitExpanded=!0):"SendInit"==t?(this.paraConfig.SendInit.VarSettings.push({Name:"",Content:""}),this.SendInitExpanded=!0):"StopInit"==t&&(this.paraConfig.StopInit.VarSettings.push({Name:"",Content:""}),this.StopInitExpanded=!0)},deleteDataVars:function(t,e){var n=this;this.$q.dialog({title:"提示",message:"您确定要删除当前参数".concat(e+1,"吗"),persistent:!0,ok:{push:!0,label:"确定"},cancel:{push:!0,label:"取消"}}).onOk((function(){"DataSource"==t?n.paraConfig.DataSourceVars.splice(e,1):"ConnectInit"==t?n.paraConfig.ConnectInit.VarSettings.splice(e,1):"SendInit"==t?n.paraConfig.SendInit.VarSettings.splice(e,1):"StopInit"==t&&n.paraConfig.StopInit.VarSettings.splice(e,1)}))},moveUpList:function(t,e){"DataSourceVars"==t&&0!=e?this.paraConfig.DataSourceVars[e]=this.paraConfig.DataSourceVars.splice(e-1,1,this.paraConfig.DataSourceVars[e])[0]:"ConnectInit"==t&&0!=e?this.paraConfig.ConnectInit.VarSettings[e]=this.paraConfig.ConnectInit.VarSettings.splice(e-1,1,this.paraConfig.ConnectInit.VarSettings[e])[0]:"SendInit"==t&&0!=e?this.paraConfig.SendInit.VarSettings[e]=this.paraConfig.SendInit.VarSettings.splice(e-1,1,this.paraConfig.SendInit.VarSettings[e])[0]:"StopInit"==t&&0!=e&&(this.paraConfig.StopInit.VarSettings[e]=this.paraConfig.StopInit.VarSettings.splice(e-1,1,this.paraConfig.StopInit.VarSettings[e])[0])},moveDownList:function(t,e){"DataSourceVars"==t&&e<this.paraConfig.DataSourceVars.length-1?this.paraConfig.DataSourceVars[e]=this.paraConfig.DataSourceVars.splice(e+1,1,this.paraConfig.DataSourceVars[e])[0]:"ConnectInit"==t&&e<this.paraConfig.ConnectInit.VarSettings.length-1?this.paraConfig.ConnectInit.VarSettings[e]=this.paraConfig.ConnectInit.VarSettings.splice(e+1,1,this.paraConfig.ConnectInit.VarSettings[e])[0]:"SendInit"==t&&e<this.paraConfig.SendInit.VarSettings.length-1?this.paraConfig.SendInit.VarSettings[e]=this.paraConfig.SendInit.VarSettings.splice(e+1,1,this.paraConfig.SendInit.VarSettings[e])[0]:"StopInit"==t&&e<this.paraConfig.StopInit.VarSettings.length-1&&(this.paraConfig.StopInit.VarSettings[e]=this.paraConfig.StopInit.VarSettings.splice(e+1,1,this.paraConfig.StopInit.VarSettings[e])[0])},ChangeFileDirectory:function(){this.ChangeFileDirectoryFlag=!0},SelectDirectoryLocation:function(){this.$refs.TreeEntity.getDirectoryLocation().id?this.getTreeEntityTreePath(this.$refs.TreeEntity.getDirectoryLocation().id):this.ChangeFileDirectoryName=this.$refs.TreeEntity.getDirectoryLocation().label,this.ChangeFileDirectoryId=this.$refs.TreeEntity.getDirectoryLocation().id||null,this.ChangeFileDirectoryFlag=!1},getTreeEntityTreePath:function(t,e){var n=this;this.$q.loading.show();var i={id:t};s["G"](i).then((function(t){console.log(t),e?(t.data.pop(),n.ChangeFileDirectoryName="根目录 > "+t.data.join(" > "),n.$q.loading.hide()):(n.ChangeFileDirectoryName="根目录 > "+t.data.join(" > "),n.$q.loading.hide())}))}}},p=l,d=(n("d8e7"),n("db00"),n("2877")),u=Object(d["a"])(p,i,a,!1,null,"3acff118",null);e["a"]=u.exports},"31e8":function(t,e,n){},7729:function(t,e,n){"use strict";var i=function(){var t=this,e=t.$createElement,n=t._self._c||e;return n("q-dialog",{attrs:{persistent:""},model:{value:t.fixed,callback:function(e){t.fixed=e},expression:"fixed"}},[n("q-card",[n("q-card-section",[n("div",{staticClass:"text-h6"},[t._v("主机列表")])]),n("q-separator"),n("div",{staticClass:"new_input"},t._l(t.masterHostList,(function(e,i){return n("q-item",{directives:[{name:"ripple",rawName:"v-ripple"}],key:i,attrs:{tag:"label"}},[n("q-item-section",{attrs:{avatar:""}},[n("q-radio",{attrs:{val:i,color:"teal"},model:{value:t.selectIndex,callback:function(e){t.selectIndex=e},expression:"selectIndex"}})],1),n("q-item-section",[n("q-item-label",[t._v(t._s(e.address)+" "),n("q-icon",{staticClass:"pointer",style:{background:1==e.isAvailable?"green":"red"},attrs:{name:"ion-ellipse"}})],1)],1)],1)})),1),n("q-separator"),n("q-card-actions",{attrs:{align:"right"}},[n("q-btn",{attrs:{flat:"",label:"取消",color:"primary"},on:{click:t.cancel}}),n("q-btn",{attrs:{flat:"",label:"添加",color:"primary"},on:{click:t.confirm}})],1)],1)],1)},a=[],o={props:["fixed","masterHostList","masterSelectIndex"],name:"lookUp",data:function(){return{selectIndex:-1}},watch:{masterSelectIndex:function(t){this.selectIndex=t}},methods:{confirm:function(){this.$emit("addMasterHost",this.selectIndex)},cancel:function(){this.$emit("cancelMasterHost")}}},s=o,r=(n("ae48"),n("2877")),c=Object(r["a"])(s,i,a,!1,null,"e603da2c",null);e["a"]=c.exports},"9c98":function(t,e,n){},ae48:function(t,e,n){"use strict";var i=n("9c98"),a=n.n(i);a.a},ba03:function(t,e,n){},d8e7:function(t,e,n){"use strict";var i=n("31e8"),a=n.n(i);a.a},db00:function(t,e,n){"use strict";var i=n("ba03"),a=n.n(i);a.a}}]);
//# sourceMappingURL=chunk-6e3bf917.20cde44e.js.map