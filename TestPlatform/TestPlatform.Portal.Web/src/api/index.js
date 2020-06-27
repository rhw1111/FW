import {
  post,
  fetch,
  del
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
    return del(api, {
      ...payload
    }, responseType)
  }
  if (apiName.includes('post')) {
    return post(api, {
      ...payload
    }, responseType)
  }
}



export const getbypage = payload =>
  action({
    apiName: 'getbypage',
    payload
  })





