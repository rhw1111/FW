const apiConstants = {
  //---------------------------------------- TestCase -------------------------------------
  postCreateTestCase: 'api/testcase/add',     //创建TestCase

  deleteTestCase: 'api/testcase/delete',        //删除TestCase

  getMasterHostList: 'api/testhost/queryall', //获得主机列表

  getTestCaseList: 'api/testcase/querybypage',    //获得TeseCase列表

  putTestCase: 'api/testcase/update', //更新TestCase

  getTestCaseDetail: 'api/testcase/testcase',      //获得TeseCase详情

  postCreateSlaveHost: 'api/testcase/addslavehost',//创建从机

  getSlaveHostsList: 'api/testcase/queryslavehosts',//获得从机列表

  putSlaveHost: 'api/testcase/UpdateSlaveHost',//更新从机

  deleteSlaveHost: 'api/testcase/deleteslavehost',//删除从机

  deleteSlaveHostArr: 'api/testcase/deleteslavehosts',//批量删除从机

  getHistoryList: 'api/testcase/histories',//获得历史记录列表

  postSelectedHistories: 'api/testcase/selectedhistories',//获得批量历史记录详情

  getHistoryDetail: 'api/testcase/history',//获得历史记录详情

  deleteHistory: 'api/testcase/deletehistory',//删除单条历史记录

  deleteHistoryArr: 'api/testcase/deletehistories',//批量删除历史记录

  postTestCaseRun: 'api/testcase/run',//TestCase运行

  postTestCaseStop: 'api/testcase/stop',//TestCaseStop停止

  getMasterLog: 'api/testcase/getmasterlog',//查看master日志

  getSlaveLog: 'api/testcase/getslavelog',//查看Slave日志

  getSlaveIndexLog: 'api/testcase/GetSlaveLog',//查看Slave日志

  getCheckStatus: 'api/testcase/checkstatus',//查看状态

  getTestCaseStatus: 'api/testcase/querytestcasestatus',//查看当前TestCase状态

  getHistoryTransferFile: 'api/testcase/transfernetgatewaydatafile',//历史记录列表转移文件

  getHistoryViewFileStatus: 'api/testcase/checkdataanalysisstatus',//历史记录查看网关文件分析状态

  getHistoryGatewayDataFormatList: 'api/testcase/getnetgatewaydataformattypes',//获取网关数据格式列表

  postHistoryUpdateGatewayDataFormat: 'api/testcase/updatenetgatewaydataformat',//保存网关数据格式

  postQueryHostPorts: 'api/testcase/queryhostports',//判断当前主机端口号是否正在被使用

  // ---------------------------------------- TestDataSource -------------------------------------------------------
  getTestDataSource: 'api/testdatasource/querybypage',//获得TestDataSource

  postCreateTestDataSource: 'api/testdatasource/add', //创建TestDataSource

  deleteTestDataSource: 'api/testdatasource/delete',//单个删除TestDataSource

  deleteTestDataSourceArr: 'api/testdatasource/deletemultiple',//批量删除TestDataSource

  getTestDataSourceDetail: 'api/testdatasource/testdatasource',//获得TestDataSource详情数据

  putTestDataSource: 'api/testdatasource/update',//更新TestDataSource

  getDataSourceName: 'api/testdatasource/datasources',//获得 数据源名称

  // ------------------------------------------------ SSHEndpoint ------------------------------------------------------
  getSSHEndpointList: 'api/sshendpoint/querybypage',//获得SSH端口列表

  getSSHEndpointData: 'api/sshendpoint/queryall',//获得SSH端口数据

  postCreateSSHEndpoint: 'api/sshendpoint/add',  //创建SSH端口

  deleteSSHEndpoint: 'api/sshendpoint/delete',//删除单个SSHEndpoint

  deleteSSHEndpointArr: 'api/sshendpoint/deletemultiple',//批量删除多个SSHEndpoint

  getSSHEndpointDetail: 'api/sshendpoint/sshendpoint',//获得SSH端口详情

  putSSHEndpoint: 'api/sshendpoint/update', //更新SSH端口
  // ---------------------------------------------- TestHost ---------------------------------------------------------
  getTestHostList: 'api/testhost/querybypage',  //获得测试机列表

  getTestHostDetail: 'api/testhost/testhost',//获得测试机详情

  postCreateTestHost: 'api/testhost/add',//创建测试机

  putTestHost: 'api/testhost/update',//更新测试机

  deleteTestHost: 'api/testhost/delete',//单个删除TestHost

  deleteTestHostArr: 'api/testhost/deletemultiple',//批量删除TestHost 

  // ---------------------------------------------- TreeEntity ---------------------------------------------------------

  getTreeEntityList: 'api/treeentity/querybypage',//获得树结构根目录

  getTreeEntityChildrenList: 'api/treeentity/querychildren',//获得树结构子级目录

  getgobackpreviousTreeEntity: 'api/treeentity/gobackprevious',//回到上一级 

  postCreateTreeEntity: 'api/treeentity/add',//创建目录

  deleteTreeEntity: 'api/treeentity/delete',//删除树结构的某一个

  putTreeEntityParent: 'api/treeentity/updateparent',//更新父节点

  putTreeEntityName: 'api/treeentity/updatename',//更新名称

  postTreeEntityCopyFile: 'api/treeentity/copy',//复制创建测试用例和测试数据源

  postTreeEntityCopyFolder: 'api/treeentity/createfolder',//复制创建目录

  getTreeEntityTreePath: 'api/treeentity/treepath',//获取节点的路径
}
export default apiConstants
