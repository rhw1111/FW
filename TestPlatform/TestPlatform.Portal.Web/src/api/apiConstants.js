const apiConstants = {
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

  getHistoryDetail: 'api/testcase/history',//获得历史记录详情

  deleteHistory: 'api/testcase/deletehistory',//删除单挑历史记录

  deleteHistoryArr: 'api/testcase/deletehistories',//批量删除历史记录

  getTestDataSource: 'api/testdatasource/querybypage',//获得TestDataSource

  postCreateTestDataSource: 'api/testdatasource/add', //创建TestDataSource

  deleteTestDataSource: 'api/testdatasource/delete',//单个删除TestDataSource

  deleteTestDataSourceArr: 'api/testdatasource/deletemultiple',//批量删除TestDataSource

  getTestDataSourceDetail: 'api/testdatasource/testdatasource',//获得TestDataSource详情数据

  putTestDataSource: 'api/testdatasource/update',//更新TestDataSource

  postTestCaseRun: 'api/testcase/run',//TestCase运行

  postTestCaseStop: 'api/testcase/stop',//TestCaseStop停止

  getMasterLog: 'api/testdatasource/getmasterlog',//查看master日志
}
export default apiConstants
