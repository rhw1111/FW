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

}
export default apiConstants
