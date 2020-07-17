// const envUrl = {
//   DEV: '52.188.14.158',                 //开发
//   PROD: '172.17.193.219',               //客户
//   PROD2: '172.17.38.20',                //客户
//   localhost: 'localhost'
// }
// function judgeUrl (host = window.location.hostname) {
//   if (host.includes(envUrl.DEV)) {
//     return 'http://52.188.14.158:8081/'
//   } else if (host.includes(envUrl.PROD)) {
//     return 'http://172.17.193.219:8081/'
//   } else if (host.includes(envUrl.PROD2)) {
//     return 'http://172.17.38.20:8081/'
//   } else {
//     return '/api'
//   }
// }

const envUrl = {
  localhost: 'localhost'  //本地
}
function judgeUrl (host = window.location.hostname) {
  if (host.includes(envUrl.localhost)) {
    return '/api'
  } else {
    return window.location.origin + ':8081/'
  }
}

export default judgeUrl();