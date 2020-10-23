//配置新环境PotalHostName和WebApi的key值必须相同

//此处填写链接主机名
const PotalHostName = {
  DEV: '52.188.14.158',
  PROD1: '172.17.38.74',
  PROD2: '172.17.38.20'
}
//此处填写接口地址
const WebApi = {
  DEV: 'https://52.188.14.158:8081/',
  PROD1: 'https://172.17.38.74:8081/',
  PROD2: 'https://172.17.38.20:8081/'
}
console.log('------------------ 环境地址 ----------------')
function judgeUrl (host = window.location.hostname) {
  for (let val in PotalHostName) {
    if (host.includes(PotalHostName[val])) return WebApi[val];
  }
}
console.log(judgeUrl());
window.Axios_baseURL = judgeUrl();