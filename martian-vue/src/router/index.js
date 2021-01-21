import Vue from 'vue'
import Router from 'vue-router'
import { ACCESS_TOKEN, TENANT_ID } from "@/store/mutation-types"

Vue.use(Router)

const router = new Router({
    mode: 'hash',
    routes: [{
        path: '/login',
        name: 'Login',
        component: () => import('@/login/login')
    }, {
        path: '/home',
        name: 'Home',
        component: () => import('@/layout/home'),
        meta: '首页',
        children: [{
            path: '/upload/dataupload',
            name: 'upload',
            component: () => import('@/views/upload/dataupload'),
            meta: '测试页面'
        }]
    }, {
        path: '*',
        redirect: '/upload/dataupload'
    }]
})


// router.beforeEach((to,from,next)=>{
//     console.log('page_jump -> ', 'to : ', to , ",  from : ", from)
//     if(to.path ==='/login'){
//       next();
//     }else {
//         const token = Vue.ls.get(ACCESS_TOKEN)
//         if(token === null || token === ''){
//             next('/login');
//         }else {
//             next();
//         }
//     }
//   });

export default router