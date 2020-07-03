//CRM后台
const envUrl = {
  DEV: '52.188.14.158',                 //开发
}
function judgeUrl (host = window.location.hostname) {
  if (host.includes(envUrl.DEV)) {
    return 'http://52.188.14.158:8081/'
  } else {
    return '/api'
  }

}

export default judgeUrl();