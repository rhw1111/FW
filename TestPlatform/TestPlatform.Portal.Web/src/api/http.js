import Axios from 'axios'
import Vue from 'vue'
import HTTP_STATUS from './HttpStatus'
import router from '../router/index.js'
import HTTP_LOCATION from "./HttpLocation"

let loadingInstance = null;
Axios.defaults.baseURL = HTTP_LOCATION;
Axios.interceptors.request.use(
  config => {
    config.headers = {
      'Content-Type': 'application/json',
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
      if (data.code === 200 || data.code === '200') {
        return {
          success: true,
          code: 200,
          //message: data.message || '成功',
          data: data
        }
      } else if (data.code === 401 || data.code === '401') {
        router.push({
          name: 'Login',
          params: {
            pageType: 'login'
          }
        })
      } else {
        return {
          data
        }
      }
    } else {
      return {
        data
      }
    }
  },
  err => {
    console.log(err)
    let errMsg = '网络错误'
    if (err.response && err.response.status === 500) {
      if (err.response.data.Message) {
        errMsg = err.response.data.Message;
        console.log(errMsg)
        Vue.prototype.$q.notify({
          position: 'top',
          message: '提示',
          caption: err.response.data.Message,
          color: 'red',
        })
        setTimeout(() => {
          Vue.prototype.$q.loading.hide()
        }, 2000)
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
  return Axios.post(url, payload.singleArray ? payload.singleArray : payload)
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
  console.log(url, payload)
  return Axios.delete(url, payload.delArr ? { data: payload.delArr } : { data: payload })
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
export const put = (url, payload) => {
  return Axios.put(url, payload)
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
