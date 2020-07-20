import {
  post,
  fetch,
  del,
  put
} from './http'
import apiConstants from './apiConstants'
/**
 *  根据传入的apiName(getXXX/postXXX) 返回fetch/post请求
 *  apiName: 请求api的名称
 *  payload: 请求参数
 *  postfix: 当时get、delete请求时，缀到url后面的参数
 * responseType: 响应类型（暂时只有下载功能返回 blob 时使用）
 */
const action = ({
  apiName,
  payload,
  postfix,
  id,
  responseType
}) => {
  let api = apiConstants[apiName]
  if (apiName.includes('get')) {
    if (postfix && id) {
      api = `${api}/${postfix}/${id}`
    } else if (postfix) {
      postfix && (api = `${api}/${postfix}`)
    }
    return fetch(api, {
      ...payload
    }, responseType)
  }
  if (apiName.includes('delete')) {
    if (postfix && id) {
      api = `${api}/${postfix}/${id}`
    } else if (postfix) {
      postfix && (api = `${api}/${postfix}`)
    }
    console.log(payload)
    return del(api, {
      ...payload,
    }, responseType)
  }
  if (apiName.includes('put')) {
    return put(api, {
      ...payload
    }, responseType)
  }
  if (apiName.includes('post')) {
    if (postfix && id) {
      api = `${api}/${postfix}/${id}`
    } else if (postfix) {
      postfix && (api = `${api}/${postfix}`)
    }
    return post(api, {
      ...payload
    }, responseType)
  }
}



//创建TestCase
export const postCreateTestCase = payload =>
  action({
    apiName: 'postCreateTestCase',
    payload
  })

//删除TestCase
export const deleteTestCase = postfix =>
  action({
    apiName: 'deleteTestCase',
    postfix
  })


//获得主机列表
export const getMasterHostList = payload =>
  action({
    apiName: 'getMasterHostList',
    payload
  })

//获得TestCase列表
export const getTestCaseList = payload =>
  action({
    apiName: 'getTestCaseList',
    payload
  })

//获得TestCase详情
export const getTestCaseDetail = payload =>
  action({
    apiName: 'getTestCaseDetail',
    payload
  })


//获得从主机列表
export const getSlaveHostsList = payload =>
  action({
    apiName: 'getSlaveHostsList',
    payload
  })

//更新TestCase
export const putTestCase = payload =>
  action({
    apiName: 'putTestCase',
    payload
  })

//创建从机
export const postCreateSlaveHost = payload =>
  action({
    apiName: 'postCreateSlaveHost',
    payload
  })

//更新从机
export const putSlaveHost = payload =>
  action({
    apiName: 'putSlaveHost',
    payload
  })
//删除从机
export const deleteSlaveHost = postfix =>
  action({
    apiName: 'deleteSlaveHost',
    postfix
  })

//批量删除从机
export const deleteSlaveHostArr = payload =>
  action({
    apiName: 'deleteSlaveHostArr',
    payload
  })

//获得历史记录列表
export const getHistoryList = payload =>
  action({
    apiName: 'getHistoryList',
    payload
  })

//获得历史记录详情
export const getHistoryDetail = payload =>
  action({
    apiName: 'getHistoryDetail',
    payload
  })

//单个删除历史记录
export const deleteHistory = postfix =>
  action({
    apiName: 'deleteHistory',
    postfix
  })

//批量删除历史记录
export const deleteHistoryArr = payload =>
  action({
    apiName: 'deleteHistoryArr',
    payload
  })

//获得TestDataSource
export const getTestDataSource = payload =>
  action({
    apiName: 'getTestDataSource',
    payload
  })

//创建TestDataSource
export const postCreateTestDataSource = payload =>
  action({
    apiName: 'postCreateTestDataSource',
    payload
  })

//单个删除TestDataSource
export const deleteTestDataSource = postfix =>
  action({
    apiName: 'deleteTestDataSource',
    postfix
  })

//批量删除TestDataSource
export const deleteTestDataSourceArr = payload =>
  action({
    apiName: 'deleteTestDataSourceArr',
    payload
  })

//获得TestDataSource详情
export const getTestDataSourceDetail = payload =>
  action({
    apiName: 'getTestDataSourceDetail',
    payload: payload
  })

//更新TestDataSource
export const putTestDataSource = payload =>
  action({
    apiName: 'putTestDataSource',
    payload
  })

//TestCase运行
export const postTestCaseRun = postfix =>
  action({
    apiName: 'postTestCaseRun',
    postfix
  })

//TestCase停止
export const postTestCaseStop = postfix =>
  action({
    apiName: 'postTestCaseStop',
    postfix
  })

//查看master日志
export const getMasterLog = payload =>
  action({
    apiName: 'getMasterLog',
    payload
  })

//查看Slave日志
export const getSlaveLog = payload =>
  action({
    apiName: 'getSlaveLog',
    payload
  })

//查看CheckStatus日志
export const getCheckStatus = payload =>
  action({
    apiName: 'getCheckStatus',
    payload
  })


//查看TestCaseStatus状态
export const getTestCaseStatus = payload =>
  action({
    apiName: 'getTestCaseStatus',
    payload
  })

// ------------------------------------------- SSHEndpoint ----------------------------------------------
//获得SSHEndpoint列表
export const getSSHEndpointList = payload =>
  action({
    apiName: 'getSSHEndpointList',
    payload
  })

//获得SSHEndpoint数据
export const getSSHEndpointData = payload =>
  action({
    apiName: 'getSSHEndpointData',
    payload
  })

//创建SSHEndpoint
export const postCreateSSHEndpoint = payload =>
  action({
    apiName: 'postCreateSSHEndpoint',
    payload
  })

//单个删除SSHEndpoint
export const deleteSSHEndpoint = postfix =>
  action({
    apiName: 'deleteSSHEndpoint',
    postfix
  })

//批量删除SSHEndpoint
export const deleteSSHEndpointArr = payload =>
  action({
    apiName: 'deleteSSHEndpointArr',
    payload
  })

//获得SSH端口详情
export const getSSHEndpointDetail = payload =>
  action({
    apiName: 'getSSHEndpointDetail',
    payload
  })

// 更新SSH端口
export const putSSHEndpoint = payload =>
  action({
    apiName: 'putSSHEndpoint',
    payload
  })

// ------------------------------------------- TestHost ----------------------------------------------
//获得测试机列表
export const getTestHostList = payload =>
  action({
    apiName: 'getTestHostList',
    payload
  })

//获得测试机详情
export const getTestHostDetail = payload =>
  action({
    apiName: 'getTestHostDetail',
    payload
  })

//创建测试机详情
export const postCreateTestHost = payload =>
  action({
    apiName: 'postCreateTestHost',
    payload
  })

//更新测试机
export const putTestHost = payload =>
  action({
    apiName: 'putTestHost',
    payload
  })

//单个删除测试机
export const deleteTestHost = postfix =>
  action({
    apiName: 'deleteTestHost',
    postfix
  })

//批量删除测试机
export const deleteTestHostArr = payload =>
  action({
    apiName: 'deleteTestHostArr',
    payload
  })


//获得数据源名称
export const getDataSourceName = payload =>
  action({
    apiName: 'getDataSourceName',
    payload
  })
































































