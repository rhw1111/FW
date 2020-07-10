const envUrl = {
  DEV: '52.188.14.158',                 //开发
  PROD: '172.17.193.219'                //客户
}
function judgeUrl (host = window.location.hostname) {
  if (host.includes(envUrl.DEV)) {
    return 'http://52.188.14.158:8081/'
  } else if (host.includes(envUrl.PROD)) {
    return 'http://172.17.193.219:8081/'
  } else {
    return '/api'
  }

}

export default judgeUrl();