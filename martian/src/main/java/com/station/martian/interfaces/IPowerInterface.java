package com.station.martian.interfaces;

import com.github.pagehelper.PageInfo;
import com.station.martian.model.Power;

import java.util.List;

public interface IPowerInterface {

    /**
     * 获取所有权限
     * @param page 页码
     * @param size 数量
     * @return
     */
    public PageInfo<Power> getPowers(int page, int size);

    /**
     * 获取权力根据id
     * @param id
     * @return
     */
    public Power getPowerById(int id);

    /**
     * 根据权限名称获取权限信息
     * @param name
     * @return
     */
    public Power getPowerByName(String name);

    /**
     * 添加权力
     * @param power
     * @return
     */
    public boolean addPower(Power power);

    /**
     * 删除权限
     * @param id
     * @return
     */
    public boolean deletePower(int id);

    /**
     * 修改权限内容
     * @param power 权限
     * @return
     */
    public boolean modifyPower(Power power);
}
