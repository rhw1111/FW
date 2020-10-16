import Vue from 'vue';
import App from './App.vue';
import router from './router/index'
import './assets/iconfont/iconfont.js'
import './assets/iconfont/icon.css'
import './quasar'
import ElementUI from 'element-ui';
import 'element-ui/lib/theme-chalk/index.css';

Vue.use(ElementUI);

window.VueInstance = Vue;
if (window.Axios_baseURL != '/api') {
  window.console.log = () => { };
}
new Vue({
  router,
  render: h => h(App)
}).$mount('#app');
