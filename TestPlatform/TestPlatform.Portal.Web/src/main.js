import Vue from 'vue';
import App from './App.vue';
import router from './router/index'
import './assets/iconfont/iconfont.js'
import './assets/iconfont/icon.css'
import './quasar'
import ElementUI from 'element-ui';
import 'element-ui/lib/theme-chalk/index.css';
import x2js from 'x2js' //xml数据处理插件

Vue.prototype.$x2js = new x2js() //创建x2js对象，挂到vue原型上

Vue.use(ElementUI);

window.VueInstance = Vue;
if (window.Axios_baseURL != '/api') {
  window.console.log = () => { };
}
new Vue({
  router,
  render: h => h(App)
}).$mount('#app');
