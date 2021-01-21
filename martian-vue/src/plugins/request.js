import Vue from 'vue'
import axios from 'axios'
// import store from '@/store'
import { VueAxios } from './axios'
import { ACCESS_TOKEN, TENANT_ID } from "@/store/mutation-types"
let Base64 = require('js-base64').Base64

//自动设置后台服务 baseURL (也可以手工指定写死项目名字)
let baseDomain = process.env.VUE_APP_BASE_API;
//前端也带上项目名称，方便部署
// let baseProject = baseDomain.substring(baseDomain.lastIndexOf("/"));
let baseProject = baseDomain;
let userTokenKey = "token"
console.log("baseDomain= ",baseDomain)
console.log("baseProject= ",baseProject)
console.log("VUE_APP_BASE_API= ",process.env.VUE_APP_BASE_API)

// 创建 axios 实例
// 这里缺省ip端口，启动代理
const service = axios.create({
  //baseURL: '/jeecg-boot',
  baseURL: process.env.VUE_APP_BASE_API, // api base_url
  timeout: 60000 // 请求超时时间(毫秒)
})

const err = (error) => {
  return Promise.reject(error)
};
//根据response下载文件
const downloadFile = (res) =>{
  let fileName = Base64.decode(res.headers["filename"])
  // new Blob([data])用来创建URL的file对象或者blob对象
  let url = window.URL.createObjectURL(new Blob([res.data])); 
  // 生成一个a标签
  let link = document.createElement("a");
  link.style.display = "none";
  link.href = url;
  // 生成时间戳
  let timestamp=new Date().getTime();   
  link.download = fileName;   
  document.body.appendChild(link);
  link.click();
  document.body.removeChild(link)
}

// request interceptor
service.interceptors.request.use(config => {
  const token = Vue.ls.get(ACCESS_TOKEN)
  console.log('token : ', token)
  // if (token) {
  config.headers[userTokenKey] = token // 让每个请求携带自定义 token 请根据实际情况自行修改
  // }
  //update-begin-author:taoyan date:2020707 for:多租户
  let tenantid = Vue.ls.get(TENANT_ID)
  if (!tenantid) {
    tenantid = 0;
  }
  config.headers[ 'Access-Control-Allow-Origin' ] = "*"
  config.headers[ 'Access-Control-Allow-Credentials' ] = true
  config.headers[ 'tenant_id' ] = tenantid
  if(config.params instanceof FormData){
    config.headers["Content-Type"] = "multipart/form-data"
  }
  return config
},(error) => {
  return Promise.reject(error)
})

// response interceptor
service.interceptors.response.use((response) => {
    // console.log('response -> ', response)
    //如果是登陆的返回则自动截获token
    if(response.config.url == 'user/login' && response.status == 200 && response.data.code == 0){
      console.log('GetToken', response.data.data.token)
      Vue.ls.set(ACCESS_TOKEN, response.data.data.token, 60 * 60 * 1000 * 10)
    }
    //文件下载
    else if(response.headers["content-type"] != "" && response.headers["content-type"] === "application/octet-stream"){
      downloadFile(response)
    }
    return response.data
  }, err)

const installer = {
  vm: {},
  install (Vue, router = {}) {
    Vue.use(VueAxios, router, service)
  }
}

export {
  installer as VueAxios,
  service as axios
}