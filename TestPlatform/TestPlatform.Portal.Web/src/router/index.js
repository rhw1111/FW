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
        // ---------------------------------------------- 测试用例 ------------------------------------------------
        // 测试用例首页
        {
          path: '/TestCase',
          name: 'TestCase',
          component: () => import('@/pages/TestCase/index.vue'),
        },
        //测试用例详情
        {
          path: '/TestCase/Detail',
          name: 'TestCaseDetail',
          component: () => import('@/pages/TestCase/Detail.vue'),
        },
        // ---------------------------------------------- 测试数据源 ------------------------------------------------
        //测试数据源首页
        {
          path: '/TestDataSource',
          name: 'TestDataSource',
          component: () => import('@/pages/TestDataSource/index.vue'),
        },
        //测试数据源详情
        {
          path: '/TestDataSource/Detail',
          name: 'TestDataSourceDetail',
          component: () => import('@/pages/TestDataSource/Detail.vue'),
        },
        // ---------------------------------------------- 主机 ------------------------------------------------
        //主机首页
        {
          path: '/MasterHost',
          name: 'MasterHost',
          component: () => import('@/pages/MasterHost/index.vue'),
        },
      ]
    },
  ]
})