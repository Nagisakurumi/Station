package com.station.martian.utils;

import com.github.pagehelper.PageInfo;
import com.station.martian.items.PageResult;
import org.springframework.data.domain.PageRequest;

import javax.mail.Message;
import javax.mail.MessagingException;
import javax.mail.Session;
import javax.mail.Transport;
import javax.mail.internet.InternetAddress;
import javax.mail.internet.MimeMessage;
import javax.servlet.http.Cookie;
import javax.servlet.http.HttpServletRequest;
import javax.servlet.http.HttpServletResponse;
import java.security.MessageDigest;
import java.util.Date;
import java.util.Properties;
import java.util.Random;

public class Util {

    /**
     * 返回大写MD5
     *
     * @param origin 加密原始数据
     * @param charsetname 字符编码
     * @param token 加密token
     * @return 密文
     */
    public static String MD5Encode(String origin, String charsetname, String token) {
        String resultString = null;
        try {
            if(token != null){
                origin += token;
            }
            resultString = new String(origin);
            MessageDigest md = MessageDigest.getInstance("MD5");
            if (charsetname == null || "".equals(charsetname))
                resultString = byteArrayToHex(md.digest(resultString.getBytes()));
            else
                resultString = byteArrayToHex(md.digest(resultString.getBytes(charsetname)));
        } catch (Exception exception) {
        }
        return resultString.toUpperCase();
    }


    /**
     * 把字节转换到进制字符串
     * @param byteArray
     * @return
     */
    public static String byteArrayToHex(byte[] byteArray) {
        // 首先初始化一个字符数组，用来存放每个16进制字符
        char[] hexDigits = {'1','2','3','4','5','6','7','8','9', '0', 'A','B','C','D','E','F' };
        // new一个字符数组，这个就是用来组成结果字符串的（解释一下：一个byte是八位二进制，也就是2位十六进制字符（2的8次方等于16的2次方））
        char[] resultCharArray =new char[byteArray.length * 2];
        // 遍历字节数组，通过位运算（位运算效率高），转换成字符放到字符数组中去
        int index = 0;
        for (byte b : byteArray) {
            resultCharArray[index++] = hexDigits[b >>> (index % 4) & 0xf];
            resultCharArray[index++] = hexDigits[b& 0xf];
        }
        // 字符数组组合成字符串返回
        return new String(resultCharArray);
    }

    /**
     * 发送邮件
     * @param title  标题
     * @param address 目标邮箱
     * @param content 内容
     * @return
     */
    public static boolean sendemail(String title, String address, String content) {
        try {
            Properties props = new Properties();
            // 设置发送邮件的邮件服务器的属性（这里使用网易的smtp服务器）
            props.put("mail.smtp.host", "smtp.qq.com");
            // 需要经过授权，也就是有户名和密码的校验，这样才能通过验证
            props.put("mail.smtp.auth", "true");
            props.put("mail.smtp.port", 587);
            props.put("mail.smtp.ssl.enable", true);
            // 用刚刚设置好的props对象构建一个session

            Session session = Session.getInstance(props);
            session.setDebug(true);

            Message msg = new MimeMessage(session);
            msg.setSubject(title);
            msg.setText(content);
            msg.setFrom(new InternetAddress("martianscript@qq.com")); //发件人邮箱
            msg.setRecipient(Message.RecipientType.TO,
                    new InternetAddress(address)); //收件人邮箱(我的QQ邮箱)
            msg.saveChanges();
            Transport transport = session.getTransport();
            transport.connect("1018429593","pyclfwjwxtlwbdff");//发件人邮箱,授权码(可以在邮箱设置中获取到授权码的信息)
            transport.sendMessage(msg, msg.getAllRecipients());
            transport.close();
            return true;
        }catch (MessagingException e){
            e.printStackTrace();
            return false;
        }
    }

    /**
     * 从cookie中获取指定的值
     * @param request
     * @param key
     * @return
     */
    public static String getValueByCookie(HttpServletRequest request, String key){
        Cookie [] cookies = request.getCookies();
        for (Cookie cookie : cookies) {
            if(cookie.getName().equals(key)){
                return cookie.getValue();
            }
        }
        return null;
    }

    /**
     * 设置cookie中的值
     * @param response
     * @param key
     * @param value
     * @return
     */
    public static boolean setValueToCookie(HttpServletResponse response, String key, String value){
        Cookie cookie = new Cookie(key,value);
        cookie.setPath("/");
        cookie.setMaxAge(3600);
        response.addCookie(cookie);
        return true;
    }

    /**
     * 获取一个token
     * @return
     */
    public static String getToken(){
        return new Date().toString();
    }

    /**
     * 创建验证码
     * @return
     */
    public static String createVerificationCode(){
        Random random = new Random();
        String code = "";
        for (int i = 0; i < 4; i ++){
            code += random.nextInt(9);
        }
        return code;
    }


    /**
     * 将分页信息封装到统一的接口
     * @param pageRequest
     * @param pageInfo
     * @return
     */
    public static PageResult getPageResult(PageRequest pageRequest, PageInfo<?> pageInfo) {
        PageResult pageResult = new PageResult();
        pageResult.setPageNum(pageInfo.getPageNum());
        pageResult.setPageSize(pageInfo.getPageSize());
        pageResult.setTotalSize(pageInfo.getTotal());
        pageResult.setTotalPages(pageInfo.getPages());
        pageResult.setContent(pageInfo.getList());
        return pageResult;
    }
}
