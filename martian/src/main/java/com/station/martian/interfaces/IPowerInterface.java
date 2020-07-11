package com.station.martian.interfaces;

import com.station.martian.model.Power;

import java.util.List;

public interface IPowerInterface {

    /**
     * 获取所有权限
     * @return
     */
    public List<Power> getPowers();

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
}
