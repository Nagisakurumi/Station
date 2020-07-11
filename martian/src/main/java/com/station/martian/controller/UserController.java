package com.station.martian.controller;

import com.station.martian.interfaces.IUserInterface;
import com.station.martian.items.HelpCodes;
import com.station.martian.items.Result;
import com.station.martian.items.ResultCode;
import com.station.martian.model.User;
import com.station.martian.utils.Util;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.data.redis.core.RedisTemplate;
import org.springframework.data.redis.core.ValueOperations;
import org.springframework.web.bind.annotation.*;

import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;
import java.util.Date;

@RestController
@RequestMapping(value = "/user")
public class UserController {
    /**
     * redis 缓存
     * */
    @Autowired
    private RedisTemplate redisTemplate;
    /**
     * 用户接口
     */
    @Autowired
    private IUserInterface userInterface;
    /**
     * 日志
     */
    Logger logger = LoggerFactory.getLogger(getClass());

    /*
     根据用户id获取用户
     */
    @RequestMapping(value = "/getuserbyid/{id}", method = RequestMethod.GET)
    public Result getUserById(@PathVariable int id){
        Result result = new Result();
        User user = new User();
        user.setAccount("test");
        user.setPassword("dddddd");
        result.setData(user);
        return result;
    }

    /**
     * 登陆
     * @param userName
     * @param password
     * @return
     */
    @RequestMapping(value = "/login/{userName}/{password}", method = RequestMethod.GET)
    public Result login(@PathVariable String userName, @PathVariable String password, HttpServletRequest request, HttpServletResponse response){
        Result result = new Result();
        try{
            //查询用户
            User user = userInterface.getUserByUserAccount(userName);
            //获取密文
            String code = Util.MD5Encode(userName, "utf-8", Util.getValueByCookie(request, HelpCodes.TOKEN_COOKIE_NAME));
            //检测是否已经登陆了
            if(isLogin(request).getResultCode() == 0){
                logger.warn("账号 : " + userName + "已经登陆过，重复登陆!");
                Util.sendemail("异地登陆!", user.getEmail(), "您的账号从另一个地方登陆!");
            }

            if(user == null){
                result.setData(null);
                result.setResultCode(ResultCode.USER_NOT_EXIST);
                result.setResultMessage("账号或者密码不存在");
            }
            //获取token
            String token = Util.getToken();
            //计算密文
            code = Util.MD5Encode(user.getAccount(), "utf-8", token);
            //存入cookie
            Util.setValueToCookie(response, HelpCodes.TOKEN_COOKIE_NAME, token);
            //存入redis缓存
            ValueOperations<String, User> operations = redisTemplate.opsForValue();
            //设置缓存
            operations.set(code, user);
            //返回数据
            result.setData(code);
        }catch (Exception e){
            e.printStackTrace();
        }
        return result;
    }

    /**
     * 注册用户
     * @param user
     * @return
     */
    @RequestMapping(value = "/register", method = RequestMethod.POST)
    public Result register(@RequestBody User user){
        Result result = new Result();
        try{
            User find = userInterface.getUserByUserAccount(user.getAccount());
            if(find != null){
                result.setData(null);
                result.setResultCode(ResultCode.ACCOUNT_EXIST);
                result.setResultMessage("账号已存在!");
            }
            user.setPassword(Util.MD5Encode(user.getPassword(), "utf-8", null));
            if(!userInterface.registerUser(user)){
                result.setResultCode(ResultCode.UNKNOW);
                result.setResultMessage("未知的错误,注册失败!");
            }else{
                Util.sendemail("注册成功!", user.getEmail(), "恭喜你成功注册火星人脚本，您的登陆账号为 : " + user.getAccount());
            }
        }catch (Exception e){
            e.printStackTrace();
        }
        return result;
    }

    /**
     * 根据登陆码 检测用户是否已经登陆
     * @return
     */
    @RequestMapping(value = "/islogin", method = RequestMethod.GET)
    public Result isLogin(HttpServletRequest request){
        Result result = new Result();
        try{
            User user = isHasLogin(request);
            if(user != null){
                result.setResultMessage("已登录!");
            }else{
                result.setResultCode(ResultCode.NOT_LOGIN);
                result.setResultMessage("没有登陆!");
            }
        }catch (Exception e){
            e.printStackTrace();
            logger.error(e.getMessage());
        }
        return result;
    }

    /**
     * 获取用户信息
     * @return
     */
    @RequestMapping(value = "/getuserinfo")
    public Result getUserInfo(HttpServletRequest request){
        Result result = new Result();
        try {
            User user = isHasLogin(request);
            if(user == null){
                result.setResultMessage("请重新登陆");
                result.setResultCode(ResultCode.NOT_LOGIN);
            } else{
                user.setPassword("");
                result.setData(user);
            }
        }catch (Exception e){
            e.printStackTrace();
        }
        return result;
    }

    /**
     * 发送验证码到邮箱
     * @param account
     * @param email
     * @return
     */
    @RequestMapping(value = "/verification/{account}/{email}", method = RequestMethod.GET)
    public Result sendVerificationCode(@PathVariable String account, @PathVariable String email){
        Result result = new Result();
        try{
            String code = Util.createVerificationCode();
            ValueOperations<String, String> operations = redisTemplate.opsForValue();
            operations.set(account, code);
        }catch (Exception e){
            result.setResultCode(ResultCode.UNKNOW);
            e.printStackTrace();
            logger.error("发送验证码异常 : " + e.getMessage());
        }
        return result;
    }

    /**
     * 重置密码
     * @param account
     * @return
     */
    @RequestMapping(value = "/restpassword/{account}")
    public Result restPassword(@PathVariable String account){
        Result result = new Result();
        try{
            User user = userInterface.getUserByUserAccount(account);
            if(user == null){
                result.setResultCode(ResultCode.USER_NOT_EXIST);
                result.setResultMessage("账号不存在!");
            }else{
                String password = Util.getToken();
                user.setPassword(Util.MD5Encode(password, "utf-8", null));
                Util.sendemail("重置密码", user.getEmail(), "重置后的新密码 : " + password);
                userInterface.modifyUser(user);
            }
        }catch (Exception e){
            result.setResultCode(ResultCode.UNKNOW);
            e.printStackTrace();
            logger.error("重置密码错误, " + e.getMessage());
        }
        return result;
    }

    @RequestMapping(value = "/getuserlist", method = RequestMethod.POST)
    public Result getUserList(HttpServletRequest request){
        Result result = new Result();
        try {
            User user = isHasLogin(request);
            if(user == null){
                result.setResultCode(ResultCode.NOT_LOGIN);
                result.setResultMessage("用户未登录!");
            }else{
                //TOD :: 先进行权限检测是否有权限获取所有用户列表，然后进行查询和返回
            }
        }catch (Exception e){
            e.printStackTrace();
            logger.error("获取用户列表失败 : " + e.getMessage());
            result.setResultCode(ResultCode.UNKNOW);
        }
        return result;
    }

    /**
     * 获取已经登陆的用户
     * @param request
     * @return
     */
    private User isHasLogin(HttpServletRequest request){
        String code = Util.getValueByCookie(request, HelpCodes.TOKEN_COOKIE_NAME);
        return (User)redisTemplate.opsForValue().get(code);
    }
}
