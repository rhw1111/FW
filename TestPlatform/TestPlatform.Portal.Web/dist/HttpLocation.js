//配置新环境PotalHostName和WebApi的key值必须相同

//此处填写链接主机名
const PotalHostName = {
  DEV: '52.188.14.158'
}
//此处填写接口地址
const WebApi = {
  DEV: 'https://52.188.14.158:8081/'
}
console.log('------------------ 测试地址 ----------------')
function judgeUrl (host = window.location.hostname) {
  for (let val in PotalHostName) {
    console.log(val)
    if (host.includes(PotalHostName[val])) return WebApi[val];
  }
}

window.Axios_baseURL = judgeUrl();