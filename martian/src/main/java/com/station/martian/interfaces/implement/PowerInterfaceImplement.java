package com.station.martian.interfaces.implement;

import com.github.pagehelper.PageHelper;
import com.github.pagehelper.PageInfo;
import com.station.martian.interfaces.IPowerInterface;
import com.station.martian.mapper.PowerMapper;
import com.station.martian.model.Power;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;

import java.util.List;

@Service
public class PowerInterfaceImplement implements IPowerInterface {

    /**
     * 映射权力
     */
    @Autowired
    private PowerMapper powerMapper;

    /**
     * 获取所有的权限
     * @return
     */
    public PageInfo<Power> getPowers(int page, int size){
        PageHelper.startPage(page, size);
        List<Power> powers = powerMapper.selectAll();
        PageInfo<Power> pageInfo = new PageInfo<>(powers);
        return pageInfo;
    }

    @Override
    public Power getPowerById(int id) {
        return null;
    }

    @Override
    public Power getPowerByName(String name) {
        return null;
    }

    /**
     * 添加权力
     * @param power
     * @return
     */
    @Override
    public boolean addPower(Power power) {
        int res = powerMapper.insert(power);
        return res > 0;
    }

    @Override
    public boolean deletePower(int id) {
        int res = powerMapper.deleteByPrimaryKey(id);
        return res > 0;
    }

    @Override
    public boolean modifyPower(Power power) {
        int res = powerMapper.updateByPrimaryKey(power);
        return res > 0;
    }
}
