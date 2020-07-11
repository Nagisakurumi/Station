package com.station.martian.interfaces.implement;

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
    public List<Power> getPowers(){

    }
}
