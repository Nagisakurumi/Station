package com.station.martian.controller;

import com.github.pagehelper.PageInfo;
import com.station.martian.interfaces.IPowerInterface;
import com.station.martian.items.Result;
import com.station.martian.items.ResultCode;
import com.station.martian.model.Power;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.*;

import java.util.List;

@RestController
@RequestMapping(value = "/power")
public class PowerController {
    /**
     * 日志
     */
    private Logger logger = LoggerFactory.getLogger(getClass());
    /**
     * 权限接口
     */
    @Autowired
    private IPowerInterface powerInterface;
    /**
     * 获取所有权限列表
     * @return
     */
    @RequestMapping(value = "/powerlist/{page}/{size}", method = RequestMethod.GET)
    public Result getPowerList(@PathVariable int page, @PathVariable int size){
        Result result = new Result();
        try {
            PageInfo<Power> pageInfo = powerInterface.getPowers(page, size);
            result.setData(pageInfo);
        }catch (Exception e){
            e.printStackTrace();
            logger.error("获取权限列表失败 : " + e.getMessage());
            result.setResultCode(ResultCode.UNKNOW);
        }
        return result;
    }

    /**
     * 添加权限
     * @param power 权限
     * @return
     */
    @RequestMapping(value = "/add", method = RequestMethod.POST)
    public Result addPower(@RequestParam Power power){
        Result result = new Result();
        try {
            if(powerInterface.addPower(power)){
                result.setResultMessage("添加成功!");
            }else{
                result.setResultCode(ResultCode.OPTION_ERROR);
                result.setResultMessage("添加权限失败!");
            }
        }catch (Exception e){
            e.printStackTrace();
            logger.error("添加权力失败 : " + e.getMessage());
            result.setResultCode(ResultCode.UNKNOW);
            result.setResultMessage("权力添加失败!");
        }
        return result;
    }

    /**
     * 根据id 删除权限
     * @param id 权限id
     * @return
     */
    @RequestMapping(value = "/deletepower/{id}", method = RequestMethod.GET)
    public Result deletePower(@PathVariable int id){
        Result result = new Result();
        try {
            if(powerInterface.deletePower(id)){
                result.setResultMessage("删除权限成功!");
            }else{
                result.setResultCode(ResultCode.OPTION_ERROR);
                result.setResultMessage("删除权限失败!");
            }
        }catch (Exception e){
            e.printStackTrace();
            logger.error("删除失败 : " + e.getMessage());
            result.setResultCode(ResultCode.UNKNOW);
            result.setResultMessage("删除权限失败!");
        }
        return result;
    }

    /**
     * 修改权限
     * @param power 权限
     * @return
     */
    @RequestMapping(value = "/modify", method = RequestMethod.POST)
    public Result modifyPower(@RequestParam Power power){
        Result result = new Result();
        try {
            if(powerInterface.modifyPower(power)){
                result.setResultMessage("修改成功!");
            }else{
                result.setResultCode(ResultCode.OPTION_ERROR);
                result.setResultMessage("修改权限失败!");
            }
        }catch (Exception e){
            e.printStackTrace();
            logger.error("修改权力失败 : " + e.getMessage());
            result.setResultCode(ResultCode.UNKNOW);
            result.setResultMessage("权力修改失败!");
        }
        return result;
    }
}
