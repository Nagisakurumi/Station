import Vue from 'vue'
import App from './App.vue'
import vuetify from './plugins/vuetify'
import router from './router'
import store from './store'
import axios from 'axios'
import Storage from 'vue-ls'
import VueClipboard from 'vue-clipboard2'
import motion from './plugins/motion'
import config from '@/defaultSettings'

axios.defaults.withCredentials = true

Vue.config.productionTip = false
Vue.prototype.axios = axios
Vue.prototype.console = window.console
Vue.use(Storage, config.storageOptions)
// 使用剪切板
Vue.use(VueClipboard)
// 使用转场动画
Vue.directive('motion', motion)

new Vue({
	vuetify,
	router,
	store,
	render: h => h(App)
}).$mount('#app')
