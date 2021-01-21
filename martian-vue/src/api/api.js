
import { axios } from '../plugins/request'

let baseDomain = process.env.VUE_APP_BASE_API;
//post
function post(url,parameter) {
    return axios({
        url: url,
        method:'post' ,
        data: parameter
    })
}
  
//get
function get(url,parameter) {
    return axios({
        url: url,
        method: 'get',
        params: parameter
    })
}

//get
function getDownload(url,parameter) {
    return axios({
        url: url,
        method: 'get',
        params: parameter,
        responseType: 'blob'
    })
}


//api接口
export default {
    //用户接口
    user : {
        login : (params) => {
            return get('user/login', params)
        },
        logout : (params) => {
            return get('user/logout', params)
        },
        islogin : (params) => {
            return post('user/islogin', params)
        },
        getUserInfo:() => {
            return get('user/getuserinfo')
        },
        upload : (params) => {
            return post('user/upload', params)
        },
        uploadTemp : (params) => {
            return post('user/uploadtemp', params)
        },
        // getfiles : ()=>{
        //     return get('user/getfiles', null)
        // },
        download :(params) =>{
            return getDownload('user/download/' + params)
            // return baseDomain + 'user/download/' + params
        },
        downloadTemp :() =>{
            return getDownload('user/downloadtemp')
            // return baseDomain + 'user/downloadtemp'
        },
        downloadfiles :(params) =>{
            return getDownload('user/downloadfiles/' + params)
            // return baseDomain + 'user/downloadfiles/' + params
        }
    },
    organization:{
        getDropDownList:(params)=>{
            return post('organization/getdropdownlist', params)
        },
    },
    file:{
        searchFiles:(params)=>{
            return post('file/searchfiles', params)
        },
        deleteFiles:(params)=>{
            return post('file/deletefiles', params)
        },
    },
    
    
}
