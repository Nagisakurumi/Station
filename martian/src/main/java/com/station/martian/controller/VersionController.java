package com.station.martian.controller;


import com.station.martian.interfaces.IVersionInterface;
import com.station.martian.items.Result;
import com.station.martian.items.ResultCode;
import com.station.martian.model.Version;
import com.station.martian.utils.FileUtil;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RequestMethod;
import org.springframework.web.bind.annotation.RequestParam;
import org.springframework.web.bind.annotation.RestController;
import org.springframework.web.multipart.MultipartFile;

import javax.xml.crypto.Data;
import java.util.Date;

@RestController
@RequestMapping(value = "/version")
public class VersionController {
    /**
     * 版本号接口
     */
    @Autowired
    private IVersionInterface versionInterface;
    /**
     * 日志
     */
    private Logger logger = LoggerFactory.getLogger(getClass());

    @RequestMapping(value = "/uploadversion", method = RequestMethod.POST)
    public Result uploadVersion(@RequestParam("versionNum") String versionNum, @RequestParam("file") MultipartFile fileArray){
        Result result = new Result();
        try {
            Version version = new Version();
            version.setCreatetime(new Date());
            version.setVersionnum(versionNum);
            String url = FileUtil.getLocalStaticPath() + version.getCreatetime().toString() + ".zip";
            version.setPath(url);
            FileUtil.saveFile(url, fileArray.getInputStream());
        }catch (Exception e){
            e.printStackTrace();
            result.setResultCode(ResultCode.UNKNOW);
            logger.error("上传文件失败, " + e.getMessage());
        }
        return result;
    }

    /**
     * 获取下载更新包的下载地址
     * @param versionNum 版本号
     * @return
     */
    @RequestMapping(value = "/getpacketurl", method = RequestMethod.POST)
    public Result getPacketVersionUrl(@RequestParam("versionNum") String versionNum){
        Result result = new Result();
        try {
            Version version = versionInterface.getVersionByVersionNumber(versionNum);
            if(version == null){
                result.setResultCode(ResultCode.UPDATE_PACKAGE_NOT_EXIT);
                result.setResultMessage("更新包的版本号不存在!");
            }
            String url = version.getPath();
            result.setData(url);
        }catch (Exception e){
            e.printStackTrace();
            logger.error("下载更新包失败 : " + e.getMessage());
            result.setResultCode(ResultCode.UNKNOW);
        }
        return result;
    }
}
