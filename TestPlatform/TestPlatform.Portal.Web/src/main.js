import Vue from 'vue';
import App from './App.vue';
import router from './router/index'
import './quasar'
window.VueInstance = Vue;

new Vue({
  router,
  render: h => h(App)
}).$mount('#app');
