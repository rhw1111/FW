import Axios from 'axios'
import { Loading, Message } from 'element-ui'
import HTTP_STATUS from './HttpStatus'
import router from '../router/index.js'
import HTTP_LOCATION from "./HttpLocation"
import HTTP_APIKEY from "./HttpApiKey"

function UP (name) {
  var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
  if (window.location.hash.substr(window.location.hash.indexOf('?') + 1).match(reg)) {
    var r = window.location.hash.substr(window.location.hash.indexOf('?') + 1).match(reg);
    if (r != null) return decodeURIComponent(r[2]).substring(1, -1) == '{' ? decodeURIComponent(r[2]).substring(1, decodeURIComponent(r[2]).length - 1) : decodeURIComponent(r[2]); return '';
  } else { return ''; }
}
let loadingInstance = null;
Axios.defaults.baseURL = HTTP_LOCATION;
Axios.interceptors.request.use(
  config => {
    if (config.url === '/file/uploadFiles') {
      config.headers = {
        'Content-Type': 'multipart/form-data'
      }
    } else {
      let token = sessionStorage.getItem('IamToken') ? sessionStorage.getItem('IamToken') : UP('crmtoken');
      config.headers = {
        'Content-Type': 'application/json',
        'Authorization': 'Bearer ' + token,
        'X-Gaia-Api-Key': HTTP_APIKEY
      }
    }
    const temp = config.data || config.params
    // 是否开启loading
    if (config.url !== '/cart/cartCount') {
      let isLoading = true
      if (temp && Object.keys(temp).indexOf('isLoading') !== -1) {
        isLoading = temp['isLoading']
        delete temp.isLoading
      }
      isLoading &&
        (loadingInstance = Loading.service({
          fullscreen: true
        }))
    }
    return config
  },
  () => {
    return Promise.reject(new Error('请检查网络状况'))
  }
)

Axios.interceptors.response.use(
  ({ status, data, request, headers, config }) => {
    if (loadingInstance) {
      loadingInstance.close()
      loadingInstance = null
    }
    if (status === 200) {
      if (
        (request && request.responseType === 'blob') ||
        (config && config.responseType === 'blob')
      ) {
        return {
          data,
          headers
        }
      }
      if (data.code === 200 || data.code === '200') {
        return {
          success: true,
          code: 200,
          message: data.message || '成功',
          data: data.data
        }
      } else if (data.code === 401 || data.code === '401') {
        router.push({
          name: 'Login',
          params: {
            pageType: 'login'
          }
        })
      } else if (config.url.includes('token/checkIAMToken')) {
        return data
      } else {
        return {
          success: false,
          code: data.code,
          message: data.message || '失败',
          data: data.data
        }
      }
    } else {
      return {
        success: false,
        code: data.code,
        message: data.message || '失败',
        data: data.data
      }
    }
  },
  err => {
    let errMsg = '网络错误'
    if (err.response && err.response.status === 500) {
      if (err.response.data.message && err.response.data.message !== 'No message available') {
        Message({
          showClose: true,
          message: err.response.data.message,
          type: 'error',
          duration: 1000
        })
      }
    } else if (err.response && err.response.status) {
      errMsg = HTTP_STATUS[err.response.status] // 返回各种状态的状态码
    }
    return Promise.reject(new Error(errMsg))
  }
)

export const fetch = (url, payload) => {
  return Axios.get(url, { params: payload })
    .then(res => {
      return Promise.resolve(res)
    })
    .catch(err => {
      if (loadingInstance) {
        loadingInstance.close()
        loadingInstance = null
      }
      return Promise.reject(err)
    })
}
export const post = (url, payload) => {
  return Axios.post(url, payload)
    .then(res => {
      return Promise.resolve(res)
    })
    .catch(err => {
      if (loadingInstance) {
        loadingInstance.close()
        loadingInstance = null
      }
      return Promise.reject(err)
    })
}
export const del = (url, payload) => {
  return Axios.delete(url, payload)
    .then(res => {
      return Promise.resolve(res)
    })
    .catch(err => {
      if (loadingInstance) {
        loadingInstance.close()
        loadingInstance = null
      }
      return Promise.reject(err)
    })
}
export default Axios
