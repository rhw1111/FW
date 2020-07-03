import Vue from 'vue';
import Router from 'vue-router';

Vue.use(Router);

export default new Router({
  routes: [
    {
      path: '/',
      name: 'index',
      component: () => import('@/pages/index.vue'),
      children: [
        {
          path: '/TestCase',
          name: 'TestCase',
          component: () => import('@/pages/TestCase/index.vue'),
        },
        {
          path: '/TestCase/Detail',
          name: 'TestCaseDetail',
          component: () => import('@/pages/TestCase/Detail.vue'),
        },
        {
          path: '/TestCase/Detail/SlaveHostDetail',
          name: 'SlaveHostDetail',
          component: () => import('@/pages/TestCase/SlaveHostDetail.vue'),
        },
        {
          path: '/TestCase/Detail/HistoryDetail',
          name: 'HistoryDetail',
          component: () => import('@/pages/TestCase/HistoryDetail.vue'),
        },

        {
          path: '/TestDataSource',
          name: 'TestDataSource',
          component: () => import('@/pages/TestDataSource/index.vue'),
        },
        {
          path: '/TestDataSource/Detail',
          name: 'TestDataSourceDetail',
          component: () => import('@/pages/TestDataSource/Detail.vue'),
        }
      ]
    },
  ]
})