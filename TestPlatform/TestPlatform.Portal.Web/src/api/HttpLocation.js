const envUrl = {
  localhost: 'localhost',  //本地
  DEV: '52.188.14.158'
}
function judgeUrl (host = window.location.hostname) {
  if (host.includes(envUrl.localhost)) {
    return '/api'
  } else if (host.includes(envUrl.DEV)) {
    return 'https://52.188.14.158:8081/'
  } else {
    return window.location.origin + ':8081/'
  }
}

export default judgeUrl();